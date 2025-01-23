using FixPro.Services.Data;
using FixProUs.Pages;
using FixProUs.ViewModels;
using Microsoft.IdentityModel.Tokens;
using Mopups.PreBaked.Interfaces;
using OneSignalSDK.DotNet;
using OneSignalSDK.DotNet.Core.Debug;

namespace FixProUs
{
    public partial class App : Application
    {

        //async void MainThread()
        //{
        //    Device.StartTimer(new TimeSpan(0, 0, 5), () =>
        //    {
        //        if (string.IsNullOrEmpty(Helpers.Settings.PlayerIdGet))
        //        {
        //            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
        //            {
        //                // do something every 1 seconds
        //                Device.BeginInvokeOnMainThread(async () =>
        //                {
        //                    if(!string.IsNullOrEmpty(OneSignal.User.PushSubscription.Id))
        //                    {
        //                        Preferences.Default.Set(Helpers.Settings.PlayerId, );
        //                    }
        //                    else
        //                    {
        //                        Preferences.Default.Set(Helpers.Settings.PlayerId, "");
        //                    }      
        //                });

        //                return true;
        //            }
        //        }
        //        return false; // runs again, or false to stop
        //    });

        //    if (!string.IsNullOrEmpty(Helpers.Settings.AccountIdGet) && !string.IsNullOrEmpty(Helpers.Settings.UserIdGet) && !string.IsNullOrEmpty(Helpers.Settings.PlayerIdGet))
        //    {
        //        await RunThread(7);
        //    }

        //}

        //public async Task CallMethodEveryXSecondsYTimes()// Method for Check Device every 5 Second
        //{

        //    if (Controls.StartData.IsRunning == true)
        //    {
        //        if (!string.IsNullOrEmpty(Helpers.Settings.AccountIdGet) && !string.IsNullOrEmpty(Helpers.Settings.UserIdGet) && !string.IsNullOrEmpty(Helpers.Settings.PlayerIdGet))
        //        {
        //            string UserToken = await _service.UserToken();
        //            string PlayerId = await ORep.GetAsync<string>("api/Employee/GetEmployeePlayerId?" + "AccountId=" + Helpers.Settings.AccountIdGet + "&" + "EmpId=" + Helpers.Settings.UserIdGet, UserToken);
        //            if (Helpers.Settings.PlayerIdGet != PlayerId)
        //            { 
        //                Preferences.Default.Set(Helpers.Settings.UserId, "");
        //                Preferences.Default.Set(Helpers.Settings.UserName, "");
        //                Preferences.Default.Set(Helpers.Settings.UserFristName, "");
        //                Preferences.Default.Set(Helpers.Settings.Email, "");
        //                Preferences.Default.Set(Helpers.Settings.Phone, "");
        //                Preferences.Default.Set(Helpers.Settings.Password, "");
        //                Preferences.Default.Set(Helpers.Settings.CreateDate, "");
        //                Preferences.Default.Set(Helpers.Settings.BranchId, "");
        //                Preferences.Default.Set(Helpers.Settings.BranchName, "");
        //                Preferences.Default.Set(Helpers.Settings.UserRole, "");
        //                Preferences.Default.Clear();
        //                await App.Current!.MainPage!.Navigation.PushAsync(new LoginPage());
        //                Controls.StartData.IsRunning = false;
        //                await App.Current!.MainPage!.DisplayAlert("Alert", "Sorry, for Logout, because the App is open from another device", "Ok");
        //            }
        //        }
        //    }
        //}

        //public async Task RunThread(int waitSeconds)
        //{
        //    Device.StartTimer(new TimeSpan(0, 0, waitSeconds), () =>
        //    {
        //        // do something every 5 seconds
        //        Device.BeginInvokeOnMainThread(async () =>
        //        {
        //            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
        //            {
        //                await CallMethodEveryXSecondsYTimes();
        //            }
        //        });
        //        return Controls.StartData.IsRunning; // runs again, or false to stop

        //    });
        //}




        //============================================================

        Helpers.GenericRepository ORep = new Helpers.GenericRepository();
        private SignalRService _signalRService;
        private SignalRServiceChangeUserData _signalRServiceChangeUserData;

        readonly Services.Data.ServicesService _service = new Services.Data.ServicesService();

        public App()
        {
            InitializeComponent();

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(Helpers.Settings.syncFusionLicence);

            // Enable verbose OneSignal logging to debug issues if needed.
            OneSignal.Debug.LogLevel = LogLevel.VERBOSE;

            // OneSignal Initialization
            OneSignal.Initialize("5b69a003-fa95-4080-bfe9-789dcdea7e39");

            // RequestPermissionAsync will show the notification permission prompt.
            // We recommend removing the following code and instead using an In-App Message to prompt for notification permission (See step 5)
            OneSignal.Notifications.RequestPermissionAsync(true);

            if (!string.IsNullOrEmpty(Helpers.Settings.UserNameGet) && !string.IsNullOrEmpty(Helpers.Settings.PasswordGet))
            {
                MainPage = new NavigationPage(new MainPage());
            }
            else
            {
                MainPage = new NavigationPage(new LoginPage());
            }

            //MainPage = new NavigationPage(new Pages.PlansPages.ChoosePlanPage());

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            OneSignal.Notifications.Clicked += Notifications_Clicked;
        }

        private async void Notifications_Clicked(object? sender, OneSignalSDK.DotNet.Core.Notifications.NotificationClickedEventArgs result)
        {
            if (result.Notification.AdditionalData != null && result.Notification.AdditionalData.ContainsKey("deeplink"))
            {
                string deepLink = result.Notification.AdditionalData["deeplink"].ToString()!;

                if (deepLink.StartsWith("Schedule"))
                {
                    List<string> list = deepLink.Split(',').ToList(); //list[1] = ScheduleId , list[2] = ScheduleDateId

                    int ScheduleId = 0, ScheduleDateId = 0;

                    bool Try1 = int.TryParse(list[1], out ScheduleId);
                    bool Try2 = int.TryParse(list[2], out ScheduleDateId);

                    if (Try1 && Try2)
                    {
                        var VM = new SchedulesViewModel(ScheduleId, ScheduleDateId);
                        //var page = new NewSchedulePage();
                        var page = new Pages.SchedulePages.ScheduleDetailsPage();
                        page.BindingContext = VM;
                        await App.Current!.MainPage!.Navigation.PushAsync(page);
                    }
                    else
                    {
                        await App.Current!.MainPage!.Navigation.PushAsync(new MainPage());
                    }
                }
                else if (deepLink == "Meeting")
                {
                    await App.Current!.MainPage!.Navigation.PushAsync(new Pages.NotificationsPage());
                }
                else
                {
                    await App.Current!.MainPage!.Navigation.PushAsync(new MainPage());
                }
            }
        }

        private async void Connectivity_ConnectivityChanged(object? sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess != NetworkAccess.Internet)
            {
                // Connection to internet is Not available
                await App.Current!.MainPage!.Navigation.PushAsync(new NoInternetPage());
                return;
            }
        }

        protected async override void OnStart()
        {
            base.OnStart();

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                //await GetPlayerIdFromOneSignal();

                string plyId = !string.IsNullOrEmpty(OneSignal.User.PushSubscription.Id) ? OneSignal.User.PushSubscription.Id : "";
                Preferences.Default.Set(Helpers.Settings.PlayerId, plyId);

                //ObjCRuntime.Class.ThrowOnInitFailure = false;

                //await SignalRservice();
                //await SignalRserviceChangeUserData();

                await Controls.StartData.GetCom_Main();
            }
            else
            {
                await App.Current!.MainPage!.Navigation.PushAsync(new NoInternetPage(new MainPage()));
                return;
            }
        }

        protected async override void OnSleep()
        {

            await SignalRNotservice();
            await SignalRNotserviceChangeUserData();

            // Save the current page state
            //var currentPage = Application.Current!.MainPage as NavigationPage;
            //var state = currentPage!.CurrentPage.BindingContext;
            //App.Current.Properties["CurrentPageState"] = state;

            Controls.StartData.IsRunning = false;
            //MainThread();

            _signalRService.OnMessageReceived += _signalRService_OnMessageReceivedInSleep;
            _signalRServiceChangeUserData.OnMessageReceived += _signalRService_OnMessageReceivedChangeUserDataInSleep;

            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;

            base.OnSleep();
        }


        protected async override void OnResume()
        {
            base.OnResume();

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

            // Retrieve the saved page state and set the page properties
            //if (App.Current.Properties.ContainsKey("CurrentPageState"))
            //{
            //    var state = App.Current.Properties["CurrentPageState"];
            //    var currentPage = App.Current!.MainPage as NavigationPage;
            //    currentPage!.CurrentPage.BindingContext = state;
            //}

            Controls.StartData.IsRunning = true;
            //MainThread();

            if (Helpers.Settings.UserNameGet == "" && Helpers.Settings.PasswordGet == "")
            {
                await App.Current!.MainPage!.Navigation.PushAsync(new Pages.LoginPage());
                await App.Current!.MainPage!.DisplayAlert("Alert", "You’ve been logged out.\r\n(account is opened on another device)\r\n", "Ok");
            }

            _signalRService.OnMessageReceived -= _signalRService_OnMessageReceivedInSleep;
            _signalRServiceChangeUserData.OnMessageReceived -= _signalRService_OnMessageReceivedChangeUserDataInSleep;

            await SignalRservice();
            await SignalRserviceChangeUserData();
        }

        [Obsolete]
        public async Task GetPlayerIdFromOneSignal()
        {
            Device.StartTimer(new TimeSpan(0, 0, 1), () =>
            {
                if (Helpers.Settings.PlayerIdGet == "")
                {
                    if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    {
                        // do something every 1 seconds
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            string plyId = !string.IsNullOrEmpty(OneSignal.User.PushSubscription.Id) ? OneSignal.User.PushSubscription.Id : "";
                            Preferences.Default.Set(Helpers.Settings.PlayerId, plyId);
                        });

                        return true;
                    }
                }
                return false; // runs again, or false to stop
            });
        }


        public async Task SignalRNotservice()
        {
            _signalRService.OnMessageReceived -= _signalRService_OnMessageReceived;
        }

        public async Task SignalRservice()
        {
            _signalRService = new SignalRService();

            _signalRService.OnMessageReceived += _signalRService_OnMessageReceived;

            await _signalRService.StartAsync();
        }


        public async Task SignalRNotserviceChangeUserData()
        {
            _signalRServiceChangeUserData.OnMessageReceived -= _signalRService_OnMessageReceivedChangeUserData;
        }

        public async Task SignalRserviceChangeUserData()
        {
            _signalRServiceChangeUserData = new SignalRServiceChangeUserData();

            _signalRServiceChangeUserData.OnMessageReceived += _signalRService_OnMessageReceivedChangeUserData;

            await _signalRServiceChangeUserData.StartAsync();
        }

        [Obsolete]
        private async void _signalRService_OnMessageReceived(string arg1, string arg2, string arg3, string arg4)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (Helpers.Settings.UserNameGet != "" && Helpers.Settings.PasswordGet != "")
                {
                    if (!string.IsNullOrEmpty(arg1) && arg1 != Helpers.Settings.PlayerIdGet && arg2.ToLower() == (Helpers.Settings.UserNameGet).ToLower())
                    {
                        Preferences.Default.Clear();
                        Helpers.Utility.ServerUrl = Helpers.Utility.ServerUrlStatic;
                        await App.Current!.MainPage!.Navigation.PushAsync(new Pages.LoginPage());
                        Controls.StartData.IsRunning = false;
                        await App.Current!.MainPage!.DisplayAlert("Alert", "You’ve been logged out.\r\n(account is opened on another device)\r\n", "Ok");
                    }
                }
            });

        }

        [Obsolete]
        private async void _signalRService_OnMessageReceivedChangeUserData(string arg1, string arg2, string arg3, string arg4)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (Helpers.Settings.AccountIdGet == arg1 && Helpers.Settings.UserIdGet == arg2 && (Helpers.Settings.UserNameGet != arg3 || Helpers.Settings.PasswordGet != arg4))
                {

                    Preferences.Default.Clear();
                    Helpers.Utility.ServerUrl = Helpers.Utility.ServerUrlStatic;
                    await App.Current!.MainPage!.Navigation.PushAsync(new Pages.LoginPage());
                    Controls.StartData.IsRunning = false;
                    await App.Current!.MainPage!.DisplayAlert("Alert", "You’ve been logged out.\r\n(account is changed username or password)\r\n", "Ok");
                }
            });

        }

        [Obsolete]
        private void _signalRService_OnMessageReceivedInSleep(string arg1, string arg2, string arg3, string arg4)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (Helpers.Settings.UserNameGet != "" && Helpers.Settings.PasswordGet != "")
                {
                    if (!string.IsNullOrEmpty(arg1) && arg1 != Helpers.Settings.PlayerIdGet && arg2.ToLower() == (Helpers.Settings.UserNameGet).ToLower())
                    {
                        Preferences.Default.Clear();
                        Helpers.Utility.ServerUrl = Helpers.Utility.ServerUrlStatic;

                        Controls.StartData.IsRunning = false;

                    }
                }
            });
        }

        [Obsolete]
        private void _signalRService_OnMessageReceivedChangeUserDataInSleep(string arg1, string arg2, string arg3, string arg4)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (Helpers.Settings.AccountIdGet == arg1 && Helpers.Settings.UserIdGet == arg2 && (Helpers.Settings.UserNameGet != arg3 || Helpers.Settings.PasswordGet != arg4))
                {
                    Preferences.Default.Clear();
                    Helpers.Utility.ServerUrl = Helpers.Utility.ServerUrlStatic;

                    Controls.StartData.IsRunning = false;
                }
            });
        }

    }
}
