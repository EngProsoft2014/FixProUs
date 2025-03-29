using Akavache;
using Controls.UserDialogs.Maui;
using FixPro.Services.Data;
using FixProUs.Models;
using FixProUs.ViewModels;
using Microsoft.AspNetCore.SignalR.Client;
using Mopups.PreBaked.Interfaces;
using Newtonsoft.Json;
using System.Reactive.Linq;

namespace FixProUs.Pages
{
    public partial class MainPage : Controls.CustomsPage
    {
        HomeViewModel ViewModel { get => BindingContext as HomeViewModel; set => BindingContext = value; }

        private readonly bool stopping = false;
        //private readonly HubConnection _hubConnection;
        //public event Action<DataMapsModel> OnMessageReceivedLocation;

        private SignalRService _signalRService;

        static int Idincerment = 0;

        public MainPage()
        {
            InitializeComponent();

            lblLoginName.Text = Helpers.Settings.UserNameGet;
            lblLoginPhone.Text = Helpers.Settings.PhoneGet;

           
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

            await SignalRservice();
            await StartGetLocation();

            //await Animation();
            //AccountImg.Source = !string.IsNullOrEmpty(Helpers.Settings.UserPrictureGet) ? Helpers.Settings.UserPrictureGet : "avatar.png";

            try
            {
                AccountImg.Source = Preferences.Default.Get(Helpers.Settings.UserPricture, "avatar.png");
            }
            catch (Exception)
            {
                AccountImg.Source = "avatar.png";
            }
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
                    Application.Current.Quit();
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


        private async void Button_Clicked_1(object sender, EventArgs e) //Logout
        {
            UserDialogs.Instance.ShowLoading();

            Preferences.Default.Clear();
            await BlobCache.LocalMachine.InvalidateAll();
            await BlobCache.LocalMachine.Vacuum();
            await App.Current!.MainPage!.Navigation.PushAsync(new LoginPage());
            Controls.StartData.IsRunning = false;

            UserDialogs.Instance.HideHud();
        }

        public async Task SignalRservice()
        {
            _signalRService = new SignalRService();

            await _signalRService.StartAsync();
        }

        async Task StartGetLocation()
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
            else if(Device.RuntimePlatform == Device.Android)
            {

                if (Helpers.Settings.UserIdGet != "4")
                {
                    CancellationToken token = CancellationToken.None;
                    await StartAsync(token);
                }
            }

        }

        //SignalR Location iOS
        private async void Default_LocationChanged(object? sender, GeolocationLocationChangedEventArgs e)
        {
            try
            {
                //List<DataMapsModel> Listmap = new List<DataMapsModel>();
                //Idincerment += 1;

                //Listmap.Add(new DataMapsModel
                //{
                //    Id = Idincerment,
                //    BranchId = int.Parse(Helpers.Settings.BranchIdGet),
                //    EmployeeId = int.Parse(Helpers.Settings.UserIdGet),
                //    Lat = e.Location.Latitude.ToString(),
                //    Long = e.Location.Longitude.ToString(),
                //    Time = e.Location.Timestamp.TimeOfDay.ToString(),
                //    CreateDate = DateTime.Now.ToShortDateString(),
                //    MPosition = new Location(e.Location.Latitude, e.Location.Longitude),
                //});

                //await Helpers.Utility.PostData("api/UploadXML/PostXmlFile", JsonConvert.SerializeObject(Listmap, Newtonsoft.Json.Formatting.None,
                //            new JsonSerializerSettings()
                //            {
                //                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                //            }));

                if (Helpers.Settings.UserIdGet != "4")
                {
                    var location = await MainThread.InvokeOnMainThreadAsync<Location>(() =>
                    {
                        var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                        return Geolocation.GetLocationAsync(request);
                    });

                    if (location != null)
                    {
                        Idincerment += 1;

                        var locationData = new DataMapsModel
                        {
                            Id = Idincerment,
                            BranchId = int.Parse(Helpers.Settings.BranchIdGet),
                            EmployeeId = int.Parse(Helpers.Settings.UserIdGet),
                            Lat = location.Latitude.ToString(),
                            Long = location.Longitude.ToString(),
                            Time = location.Timestamp.ToString(),
                            CreateDate = DateTime.Now.ToShortDateString(),
                            MPosition = new Location(location.Latitude, location.Longitude),
                        };

                        // Send location data via SignalR
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await _signalRService.SendLocation(locationData);
                        });
                    }
                }               
            }
            catch (Exception)
            {
                await App.Current!.MainPage!.DisplayAlert("Alert", "Failed save your position for tracking !!", "OK");
            }
        }

        //SignalR Location Android
        public async Task StartAsync(CancellationToken token)
        {
            try
            {
                await Task.Run(async () =>
                {
                    while (!stopping)
                    {
                        token.ThrowIfCancellationRequested();
                        try
                        {
                            var location = await MainThread.InvokeOnMainThreadAsync<Location>(() =>
                            {
                                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                                return Geolocation.GetLocationAsync(request);
                            });

                            if (location != null)
                            {
                                Idincerment += 1;

                                var locationData = new DataMapsModel
                                {
                                    Id = Idincerment,
                                    BranchId = int.Parse(Helpers.Settings.BranchIdGet),
                                    EmployeeId = int.Parse(Helpers.Settings.UserIdGet),
                                    Lat = location.Latitude.ToString(),
                                    Long = location.Longitude.ToString(),
                                    Time = location.Timestamp.ToString(),
                                    CreateDate = DateTime.Now.ToShortDateString(),
                                    MPosition = new Location(location.Latitude, location.Longitude),
                                };

                                // Send location data via SignalR
                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                    await _signalRService.SendLocation(locationData);
                                });
                               
                            }
                        }
                        catch (Exception ex)
                        {

                        }

                        await Task.Delay(2000); // Reduce CPU usage
                    }
                }, token);
            }
            catch (Exception ex)
            {

            }
        }
    }

}
