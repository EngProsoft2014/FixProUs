
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Input;
using Stripe;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Linq;
using FixProUs.ViewModels;
using FixProUs.Services.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using FixProUs.Models;
using FixProUs.Helpers;
using CommunityToolkit.Mvvm.Input;
using FixProUs.Controls;
using Microsoft.Maui.Networking;
using Controls.UserDialogs.Maui;
using Mopups.Services;
using System.Reflection;
using Microsoft.Maui.Graphics.Platform;
using SkiaSharp;
using FixProUs.Pages.MenuPages;


namespace FixProUs.ViewModels
{
    public partial class AccountViewModel : BaseViewModel
    {

        readonly ServicesService _service = new ServicesService();

        //private MediaFile _mediaFile;

        [ObservableProperty]
        EmployeeModel loginModel;

        [ObservableProperty]
        BranchesModel oneBranches;

        [ObservableProperty]
        ObservableCollection<BranchesModel> lstBranches;

        [ObservableProperty]
        ImageSource accountPhoto;

        GenericRepository ORep = new GenericRepository();

        public AccountViewModel()
        {
            Init();
        }

        async void Init()
        {
            LoginModel = new EmployeeModel();
            OneBranches = new BranchesModel();
            LstBranches = new ObservableCollection<BranchesModel>();

            LoginModel.Id = int.Parse(Settings.UserIdGet);
            LoginModel.UserName = Settings.UserNameGet;
            LoginModel.FirstName = Settings.UserFristNameGet;
            LoginModel.LastName = Settings.UserLastNameGet;
            LoginModel.EmailUserName = Settings.EmailGet;
            LoginModel.Phone1 = Settings.PhoneGet;
            LoginModel.Password = Settings.PasswordGet;


            if (!string.IsNullOrEmpty(Settings.CreateDateGet))
            {
                LoginModel.CreateDate = Convert.ToDateTime(Settings.CreateDateGet);
            }

            await GetBranches();

            try
            {
                if (!string.IsNullOrEmpty(Settings.UserPrictureGet))
                {
                    AccountPhoto = Settings.UserPrictureGet;
                    //var byteArray = new WebClient().DownloadData(Settings.UserPrictureGet);
                    //AccountPhoto = ImageSource.FromStream(() => new MemoryStream(byteArray));
                }
                else
                {
                    AccountPhoto = "avatar.png";
                }
            }
            catch (Exception)
            {
                AccountPhoto = "avatar.png";
            }

            LoginModel.OldPicture = StaticMembers.OldProfileImageSt;
        }

        //Get All Branches
        async Task GetBranches()
        {
            IsBusy = true;

            if (Connectivity.Current.NetworkAccess == Microsoft.Maui.Networking.NetworkAccess.Internet)
            {
                UserDialogs.Instance.ShowLoading();
                string UserToken = await _service.UserToken();
                var json = await ORep.GetAsync<ObservableCollection<BranchesModel>>(string.Format("api/Employee/GetEmpolyeeBranches?" + "AccountId=" + Settings.AccountIdGet + "&" + "EmpId=" + Settings.UserIdGet), UserToken);

                if (json != null)
                {
                    LstBranches = json;

                    OneBranches = LstBranches.Where(x => x.Id == int.Parse(Settings.BranchIdGet)).FirstOrDefault()!;
                }

                UserDialogs.Instance.HideHud();
            }
               
            IsBusy = false;
        }

        [RelayCommand]
        void SelectBranch(BranchesModel model)
        {
            IsBusy = true;
            OneBranches = model;
            Preferences.Default.Set(Settings.BranchId, model.Id.ToString());
            Preferences.Default.Set(Settings.BranchName, model.Name);
            IsBusy = false;
        }

        //Pick Photo
        [RelayCommand]
        private async Task SelectePickAccountPhoto()
        {
            string UserToken = await _service.UserToken();

            await MopupService.Instance.PopAsync();

            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.Photos>();
                if (status != PermissionStatus.Granted)
                {
                    // Permissions are not granted, request permissions from the user
                    status = await Permissions.RequestAsync<Permissions.Photos>();
                    if (status != PermissionStatus.Granted)
                    {
                        // Permissions are denied, show a message to the user
                        await App.Current!.MainPage!.DisplayAlert("Permission Denied", "You need to grant photo library permission to use this feature.", "OK");
                    }
                }
                else
                {

                    // Open the photo gallery
                    var photo = await MediaPicker.Default.PickPhotoAsync();

                    if (photo != null)
                    {
                        using var stream = await photo.OpenReadAsync();
                        using var memoryStream = new MemoryStream();

                        // Load the image into SkiaSharp and resize it
                        using var originalBitmap = SKBitmap.Decode(stream);
                        var resizedBitmap = originalBitmap.Resize(new SKImageInfo(800, 600), SKFilterQuality.Medium);

                        using var image = SKImage.FromBitmap(resizedBitmap);
                        using var data = image.Encode(SKEncodedImageFormat.Jpeg, 75); // Compression level: 75%
                        data.SaveTo(memoryStream);

                        memoryStream.Position = 0;

                        // Display the selected photo in the Image control
                        LoginModel.Picture = Convert.ToBase64String(memoryStream.ToArray());

                        if (Connectivity.NetworkAccess == Microsoft.Maui.Networking.NetworkAccess.Internet)
                        {
                            //Upload Image To Server
                            UserDialogs.Instance.ShowLoading();

                            var Postjson = await ORep.PostAsync(string.Format("api/ImageMobile/ReplacePostOneImageProfileImageOnlyMobile"), LoginModel, UserToken);
                            UserDialogs.Instance.HideHud();

                            if (Postjson != null)
                            {
                                //EmployeeModel UserInfo = JsonConvert.DeserializeObject<EmployeeModel>(Postjson);

                                Preferences.Default.Set(Settings.UserPricture, (Utility.PathServerProfileImages + Postjson.Picture));
                                LoginModel.Picture = Postjson.Picture;
                                StaticMembers.OldProfileImageSt = LoginModel.OldPicture = Postjson.Picture;
                                //AccountPhoto = ImageSource.FromStream(() => memoryStream);
                                //AccountPhoto = ImageSource.FromStream(() =>
                                //{
                                //    return memoryStream;  
                                //});

                                var imageBytes = memoryStream.ToArray();

                                AccountPhoto = ImageSource.FromStream(() =>
                                {
                                    // Return a new MemoryStream each time, so it remains accessible across different UI contexts
                                    return new MemoryStream(imageBytes);
                                });
                            }

                            await App.Current!.MainPage!.Navigation.PushAsync(new AccountPage());
                            App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);

                        }

                    }
                }

            }
            catch (Exception)
            {
                await App.Current!.MainPage!.DisplayAlert("No Camera", ":( No camera available.", "Ok");
            }
        }

        //Camera Photo
        [RelayCommand]
        private async Task SelecteCamAccountPhoto()
        {
            string UserToken = await _service.UserToken();

            await MopupService.Instance.PopAsync();

            try
            {

                // Check if camera permission is granted
                var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
                if (status != PermissionStatus.Granted)
                {
                    // If permission is not granted, request permission from the user
                    status = await Permissions.RequestAsync<Permissions.Camera>();
                    if (status != PermissionStatus.Granted)
                    {
                        // Permission denied by user, show a message or take action accordingly
                        await App.Current!.MainPage!.DisplayAlert("Permission Denied", "You need to grant camera permission to use this feature.", "OK");
                    }
                }
                else
                {
                    if (MediaPicker.Default.IsCaptureSupported)
                    {


                        // Capture the photo
                        var photo = await MediaPicker.Default.CapturePhotoAsync();

                        if (photo != null)
                        {
                            using var stream = await photo.OpenReadAsync();
                            using var memoryStream = new MemoryStream();

                            // Load the image into SkiaSharp and resize it
                            using var originalBitmap = SKBitmap.Decode(stream);
                            var resizedBitmap = originalBitmap.Resize(new SKImageInfo(800, 600), SKFilterQuality.Medium);

                            using var image = SKImage.FromBitmap(resizedBitmap);
                            using var data = image.Encode(SKEncodedImageFormat.Jpeg, 75); // Compression level: 75%
                            data.SaveTo(memoryStream);

                            memoryStream.Position = 0;

                            // Display the image
                            LoginModel.Picture = Convert.ToBase64String(memoryStream.ToArray());

                            UserDialogs.Instance.ShowLoading();
                            var Postjson = await ORep.PostAsync(string.Format("api/ImageMobile/ReplacePostOneImageProfileImageOnlyMobile"), LoginModel, UserToken);
                            UserDialogs.Instance.HideHud();

                            if (Postjson != null)
                            {
                                //EmployeeModel UserInfo = JsonConvert.DeserializeObject<EmployeeModel>(Postjson);

                                Preferences.Default.Set(Settings.UserPricture, (Utility.PathServerProfileImages + Postjson.Picture));
                                LoginModel.Picture = Postjson.Picture;
                                StaticMembers.OldProfileImageSt = LoginModel.OldPicture = Postjson.Picture;
                                //AccountPhoto = ImageSource.FromStream(() => memoryStream);
                                //AccountPhoto = ImageSource.FromStream(() =>
                                //{
                                //    return memoryStream;
                                //});
                                
                                var imageBytes = memoryStream.ToArray();

                                AccountPhoto = ImageSource.FromStream(() =>
                                {
                                    // Return a new MemoryStream each time, so it remains accessible across different UI contexts
                                    return new MemoryStream(imageBytes);
                                });
                            }
                           
                            await App.Current!.MainPage!.Navigation.PushAsync(new AccountPage());
                            App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);
                        }         
                    }
                }

            }
            catch (Exception ex)
            {
                await App.Current!.MainPage!.DisplayAlert("No Camera", ":( No camera available.", "Ok");
            }
        }

        //private async Task<byte[]> ResizePhotoStream(FileResult photo)
        //{
        //    byte[] result = null;

        //    using (var stream = await photo.OpenReadAsync())
        //    {
        //        if (stream.Length > _imageMaxSizeBytes)
        //        {
        //            var image = PlatformImage.FromStream(stream);
        //            if (image != null)
        //            {
        //                var newImage = image.Downsize(_imageMaxResolution, true);
        //                result = newImage.AsBytes();
        //            }
        //        }
        //        else
        //        {
        //            using (var binaryReader = new BinaryReader(stream))
        //            {
        //                result = binaryReader.ReadBytes((int)stream.Length);
        //            }
        //        }
        //    }

        //    return result;
        //}
    }
}
