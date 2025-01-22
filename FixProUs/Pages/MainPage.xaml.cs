using Akavache;
using Controls.UserDialogs.Maui;
using FixProUs.Models;
using FixProUs.ViewModels;
using Newtonsoft.Json;
using System.Reactive.Linq;

namespace FixProUs.Pages
{
    public partial class MainPage : Controls.CustomsPage
    {
        HomeViewModel ViewModel { get => BindingContext as HomeViewModel; set => BindingContext = value; }

        static int Idincerment = 0;

        public MainPage()
        {
            InitializeComponent();

            lblLoginName.Text = Helpers.Settings.UserNameGet;
            lblLoginPhone.Text = Helpers.Settings.PhoneGet;

            //StartGetLocation();
        }

        async Task Animation()
        {
            await yumImgLogo.TranslateTo(0, 0, 500);
            await yumTimeSheet.ScaleTo(1, 300);
            await yumCustomers.ScaleTo(1, 300);
            await yumSchedules.ScaleTo(1, 300);
            await yumCalls.ScaleTo(1, 300);
            await imgWave.TranslateTo(0, 0, 200);
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();

            //await Animation();
            AccountImg.Source = !string.IsNullOrEmpty(Helpers.Settings.UserPrictureGet) ? Helpers.Settings.UserPrictureGet : "avatar.png";

            //await chatService.Connect();
            //BadgeNotifications.Num = Messages.Count;
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();

            //await chatService.Disconnect();
            //BadgeNotifications.Num = Messages.Count;
        }



        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var exit = false;
                exit = await this.DisplayAlert("FixPro", "Do you want to exit the program?", "Ok", "I want to stay").ConfigureAwait(false);
                if (exit)
                {
                    System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
                }
            });
            return true;
        }


        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            ViewModel.GetPerrmission();

            navigationDrawer.DrawerSettings.Position = Syncfusion.Maui.NavigationDrawer.Position.Left;
            navigationDrawer.ToggleDrawer();
        }

        //private void actIndLoading_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if (actIndLoading.IsRunning == true)
        //        this.IsEnabled = false;
        //    else
        //        this.IsEnabled = true;
        //}

        private async void Button_Clicked_1(object sender, EventArgs e) //Logout
        {
            UserDialogs.Instance.ShowLoading();

            Preferences.Default.Set(Helpers.Settings.AccountId, "");
            Preferences.Default.Set(Helpers.Settings.UserId, "");
            Preferences.Default.Set(Helpers.Settings.UserName, "");
            Preferences.Default.Set(Helpers.Settings.UserFristName, "");
            Preferences.Default.Set(Helpers.Settings.Email, "");
            Preferences.Default.Set(Helpers.Settings.Phone, "");
            Preferences.Default.Set(Helpers.Settings.Password, "");
            Preferences.Default.Set(Helpers.Settings.CreateDate, "");
            Preferences.Default.Set(Helpers.Settings.BranchId, "");
            Preferences.Default.Set(Helpers.Settings.BranchName, "");
            Preferences.Default.Set(Helpers.Settings.UserRole, "");
            Preferences.Default.Clear();
            await BlobCache.LocalMachine.InvalidateAll();
            await BlobCache.LocalMachine.Vacuum();
            await App.Current!.MainPage!.Navigation.PushAsync(new LoginPage());
            Controls.StartData.IsRunning = false;

            UserDialogs.Instance.HideHud();
        }


        async void StartGetLocation()
        {
            var permission = await Permissions.RequestAsync<Permissions.LocationAlways>();

            if (permission == PermissionStatus.Denied)
            {
                // TODO Let the user know they need to accept
                return;
            }

            if (Device.RuntimePlatform == Device.iOS)
            {
                if (Geolocation.Default.IsListeningForeground)
                {
                    Geolocation.Default.StopListeningForeground();
                    Geolocation.Default.LocationChanged -= Default_LocationChanged;
                    return;
                }

                await Geolocation.Default.StartListeningForegroundAsync(new GeolocationListeningRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(5)));


                //await Geolocation.Default.StartListeningForegroundAsync(TimeSpan.FromSeconds(3), 10, false, new Plugin.Geolocator.Abstractions.ListenerSettings
                //{
                //    ActivityType = Plugin.Geolocator.Abstractions.ActivityType.AutomotiveNavigation,
                //    AllowBackgroundUpdates = true,
                //    DeferLocationUpdates = false,
                //    DeferralDistanceMeters = 10,
                //    DeferralTime = TimeSpan.FromSeconds(5),
                //    ListenForSignificantChanges = true,
                //    PauseLocationUpdatesAutomatically = true
                //});

                Geolocation.Default.LocationChanged += Default_LocationChanged;
            }
        }

        private async void Default_LocationChanged(object? sender, GeolocationLocationChangedEventArgs e)
        {
            try
            {
                List<DataMapsModel> Listmap = new List<DataMapsModel>();
                Idincerment += 1;

                Listmap.Add(new DataMapsModel
                {
                    Id = Idincerment,
                    BranchId = int.Parse(Helpers.Settings.BranchIdGet),
                    EmployeeId = int.Parse(Helpers.Settings.UserIdGet),
                    Lat = e.Location.Latitude.ToString(),
                    Long = e.Location.Longitude.ToString(),
                    Time = e.Location.Timestamp.TimeOfDay.ToString(),
                    CreateDate = DateTime.Now.ToShortDateString(),
                    MPosition = new Location(e.Location.Latitude, e.Location.Longitude),
                });

                await Helpers.Utility.PostData("api/UploadXML/PostXmlFile", JsonConvert.SerializeObject(Listmap, Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings()
                            {
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                            }));


            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Failed save your position for tracking !!", "OK");
            }
        }
    }

}
