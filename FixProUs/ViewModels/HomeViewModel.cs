
using GoogleApi.Entities.Translate.Common.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
//using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Twilio.TwiML.Messaging;
using System.Reactive.Concurrency;
using GoogleApi.Entities.Search.Video.Common;
using FixProUs.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Controls.UserDialogs.Maui;
using FixProUs.Pages;

namespace FixProUs.ViewModels
{
    public partial class HomeViewModel : BaseViewModel
    {

        readonly Services.Data.ServicesService _service = new Services.Data.ServicesService();

        [ObservableProperty]
        ObservableCollection<EmployeeModel> lstWorkingEmployees;

        [ObservableProperty]
        ObservableCollection<EmployeeModel> lstEmpInAccountId;

        [ObservableProperty]
        ObservableCollection<NotificationsModel> messages;

        [ObservableProperty]
        int numNotify;

        [ObservableProperty]
        string userRole;

        [ObservableProperty]
        ImageSource accountPhoto;

        [ObservableProperty]
        string headerNotify;

        [ObservableProperty]
        string contentNotify;

        [ObservableProperty]
        EmployeeModel login;

        [ObservableProperty]
        EmployeeModel employeePermission;


        //private readonly Microsoft.AspNet.SignalR.Client.HubConnection connection;
        //private readonly IHubProxy hubProxy;
        //public event Action<string, string> MessageReceived;

        //ObservableCollection<string> _Massages;
        //public ObservableCollection<string> Massages
        //{
        //    get
        //    {
        //        return _Massages;
        //    }
        //    set
        //    {
        //        _Massages = value;
        //        if (PropertyChanged != null)
        //        {
        //            PropertyChanged(this, new PropertyChangedEventArgs("Massages"));
        //        }
        //    }
        //}

        //public class ChatService
        //{
        //    private readonly Microsoft.AspNet.SignalR.Client.HubConnection connection;
        //    private readonly IHubProxy hubProxy;

        //    public event Action<string, string> MessageReceived;

        //    public ChatService()
        //    {
        //        ///"https://localhost:44322/"
        //        //////connection = new Microsoft.AspNet.SignalR.Client.HubConnection("https://localhost:44322/");
        //        connection = new Microsoft.AspNet.SignalR.Client.HubConnection("https://projectservicesapi.engprosoft.com/");
        //        hubProxy = connection.CreateHubProxy("ChatHub");

        //        hubProxy.On<string, string, string, string>("addMessage", (name, message, fromUserId, toUserId) =>
        //        {
        //            MessageReceived?.Invoke(name, message);
        //        });
        //    }

        //    public async Task Connect()
        //    {
        //        await connection.Start();
        //    }

        //    public async Task Disconnect()
        //    {
        //        connection.Stop();
        //    }
        //}

        Helpers.GenericRepository ORep = new Helpers.GenericRepository();

        public HomeViewModel()
        {
            IsBusy = true;

            if (!string.IsNullOrEmpty(Helpers.Settings.UserRoleGet))
            {
                UserRole = Helpers.Settings.UserRoleGet;
            }
            
            Init();
            GetPerrmission();
            GetEmployeesInAccountId();
        }

        //Get Perrmission for User
        public async void GetPerrmission()
        {
            EmployeePermission = new EmployeeModel();
            await Controls.StartData.CheckPermissionEmployee();
            EmployeePermission = Controls.StartData.EmployeeDataStatic;
        }

        public async void GetEmployeesInAccountId()
        {

            if (!string.IsNullOrEmpty(Helpers.Settings.AccountIdGet))
            {
                await GetEmployeesInAccountId(int.Parse(Helpers.Settings.AccountIdGet));
            }

        }

        async void Init()
        {

            Login = new EmployeeModel();
            LstEmpInAccountId = new ObservableCollection<EmployeeModel>();

            await Controls.StartData.GetAccountKeysAsync();

            NumNotify = 0;

            await GetNotifications();

            Login.UserName = Helpers.Settings.UserNameGet;
            Login.Phone1 = Helpers.Settings.PhoneGet;

            if (Helpers.Settings.UserPrictureGet != "" && Helpers.Settings.UserPrictureGet != null && Helpers.Settings.UserPrictureGet != "https://fixpro.engprosoft.net/EmployeePic/")
            {
                AccountPhoto = Login.Picture = Helpers.Settings.UserPrictureGet;
            }
            else
            {
                AccountPhoto = Login.Picture = "avatar.png";
            }
        }

        public async Task GetNotifications()
        {

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                string UserToken = await _service.UserToken();
                Messages = await ORep.GetAsync<ObservableCollection<NotificationsModel>>("api/Notifications/GetNotifications?" + "EmployeeId=" + Helpers.Settings.UserIdGet, UserToken);
                NumNotify = Messages.Count;
            }
        }

        // Get Employees in Account Id
        public async Task GetEmployeesInAccountId(int AccountId)
        {
            IsBusy = true;

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                string UserToken = await _service.UserToken();

                var json = await ORep.GetAsync<ObservableCollection<EmployeeModel>>("api/Employee/GetEmployeesInAccountId?" + "AccountId=" + AccountId, UserToken);

                if (json != null)
                {
                    LstEmpInAccountId = json;
                }

            }

            IsBusy = false;
        }

        [RelayCommand]
        async void SelectedSendNotifications()
        {
            IsBusy = true;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "No Internet Avialable !!", "OK");
                return;
            }
            else
            {
                string strEmployees = string.Empty;

                List<int> oEmployees = new List<int>();
                oEmployees = LstEmpInAccountId.Where(x => x.IsChecked == true).Select(m => m.Id).ToList();
                if (oEmployees.Count > 0)
                {
                    oEmployees.ForEach(f => strEmployees += $",{f}");
                    strEmployees = strEmployees.Remove(0, 1);
                }

                if (string.IsNullOrEmpty(strEmployees))
                {
                    await App.Current.MainPage.DisplayAlert("Alert", $"Please Complete This Field Required : Choose Employees.", "Ok");
                }
                else if (string.IsNullOrEmpty(HeaderNotify))
                {
                    await App.Current.MainPage.DisplayAlert("Alert", $"Please Complete This Field Required : Header of Notify.", "Ok");
                }
                else if (string.IsNullOrEmpty(ContentNotify))
                {
                    await App.Current.MainPage.DisplayAlert("Alert", $"Please Complete This Field Required : Content of Notify.", "Ok");
                }
                else
                {
                    string UserToken = await _service.UserToken();

                    NotificationsSpecificModel model = new NotificationsSpecificModel()
                    {
                        AccountId = int.Parse(Helpers.Settings.AccountIdGet),

                        app_id = Helpers.Settings.OneSignalAppIdGet,

                        Header = HeaderNotify,

                        Content = ContentNotify,

                        include_player_ids = LstEmpInAccountId.Where(v => v.IsChecked == true && !string.IsNullOrEmpty(v.OneSignalPlayerId)).Select(m => m.OneSignalPlayerId).ToArray(),

                        Employees = strEmployees,

                        CreateUser = int.Parse(Helpers.Settings.UserIdGet),
                    };


                    UserDialogs.Instance.ShowLoading();
                    string json = await ORep.PostDataAsync("api/Notifications/PostNotificationSpecific", model, UserToken);
                    UserDialogs.Instance.HideHud();

                    if (!string.IsNullOrEmpty(json))
                    {
                        await App.Current.MainPage.DisplayAlert("FixPro", "Succes for Send Notifications.", "Ok");
                        await App.Current!.MainPage!.Navigation.PushAsync(new MainPage());
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Alert", "Faild for Send Notifications.", "Ok");
                    }
                }
            }

            IsBusy = false;
        }

        [RelayCommand]
        async void SelectedNotificationsPage()
        {
            IsBusy = true;
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current!.MainPage!.Navigation.PushAsync(new NoInternetPage(new MainPage()));
                return;
            }
            else
            {
                UserDialogs.Instance.ShowLoading();

                string UserToken = await _service.UserToken();

                Messages = await ORep.GetAsync<ObservableCollection<NotificationsModel>>("api/Notifications/GetNotifications?" + "EmployeeId=" + Helpers.Settings.UserIdGet, UserToken);
                NumNotify = Messages.Count;
                await App.Current!.MainPage!.Navigation.PushAsync(new Pages.NotificationsPage());
                UserDialogs.Instance.HideHud();
            }

            IsBusy = false;
        }

        [RelayCommand]
        async void SelectedCreateNotificationsPage()
        {
            IsBusy = true;

            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await App.Current!.MainPage!.Navigation.PushAsync(new NoInternetPage(new MainPage()));
                    //return;
                }
                else
                {
                    UserDialogs.Instance.ShowLoading();

                    await App.Current!.MainPage!.Navigation.PushAsync(new CreateNotificationsPage());

                    UserDialogs.Instance.HideHud();
                }
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
                //throw;
            }

            IsBusy = false;
        }

        [RelayCommand]
        async void DeactiveNotify(NotificationsModel model)
        {

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "No Internet Avialable !!", "OK");
                return;
            }
            else
            {
                model.UpdateDate = DateTime.Now;
                model.UpdateUser = int.Parse(Helpers.Settings.UserIdGet);

                var exit = false;
                exit = await App.Current.MainPage.DisplayAlert("FixPro", "Do you want to Deactive the notify?", "Yes", "No").ConfigureAwait(false);
                if (exit)
                {
                    IsBusy = true;
                    string UserToken = await _service.UserToken();
                    UserDialogs.Instance.ShowLoading();
                    var json = await ORep.PutAsync("api/Notifications/PutDeactiveNotify", model, UserToken);
                    UserDialogs.Instance.HideHud();
                    IsBusy = false;

                    if (json.Active == false)
                    {
                        Messages.Remove(model);
                    }
                }
            }

        }

        [RelayCommand]
        async Task SelectedNotificationDetails(NotificationsModel model)
        {
            IsBusy = true;
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current!.MainPage!.Navigation.PushAsync(new NoInternetPage(new NotificationsPage()));
                return;
            }
            else
            {
                UserDialogs.Instance.ShowLoading();
                if (model.ScheduleId != null && model.ScheduleDateId != null && !string.IsNullOrEmpty(model.ScheduleDate))
                {
                    var VM = new SchedulesViewModel(model.ScheduleId.Value, model.ScheduleDateId.Value);
                    var page = new Pages.SchedulePages.ScheduleDetailsPage();
                    page.BindingContext = VM;
                    await App.Current!.MainPage!.Navigation.PushAsync(page);
                }
                UserDialogs.Instance.HideHud();
            }

            IsBusy = false;
        }

        [RelayCommand]
        async Task SelectedEmployeesWorkingPage(string startTracking)
        {
            IsBusy = true;

            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await App.Current!.MainPage!.Navigation.PushAsync(new NoInternetPage(new MainPage()));
                    return;
                }
                else
                {
                    UserDialogs.Instance.ShowLoading();

                    var popupView = new EmployeesViewModel(startTracking);
                    var page = new Pages.MenuPages.EmployeesWorkingPage();
                    page.BindingContext = popupView;
                    await App.Current!.MainPage!.Navigation.PushAsync(page);

                    UserDialogs.Instance.HideHud();
                }
            }
            catch (Exception)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
                //throw;
            }

            IsBusy = false;
        }

        [RelayCommand]
        async Task SelectedCustomersPage()
        {
            IsBusy = true;

            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await App.Current!.MainPage!.Navigation.PushAsync(new NoInternetPage(new MainPage()));
                    return;
                }
                else
                {
                    UserDialogs.Instance.ShowLoading();

                    await App.Current!.MainPage!.Navigation.PushAsync(new Pages.CustomerPages.CustomersPage());

                    UserDialogs.Instance.HideHud();
                }
            }
            catch (Exception)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
                //throw;
            }


            IsBusy = false;
        }

        [RelayCommand]
        async Task SelectedCallsPage()
        {
            IsBusy = true;

            try
            {
                UserDialogs.Instance.ShowLoading();

                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await App.Current!.MainPage!.Navigation.PushAsync(new NoInternetPage(new MainPage()));
                }
                else
                {
                    Controls.StaticMembers.CreateOrDetailsCall = 0; // Get Calls Only
                    await App.Current!.MainPage!.Navigation.PushAsync(new Pages.CallPages.CallsPage());
                }

                UserDialogs.Instance.HideHud();
            }
            catch (Exception)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
                //throw;
            }

            IsBusy = false;
        }

        [RelayCommand]
        async Task SelectedSchedulePage()
        {
            IsBusy = true;
            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await App.Current!.MainPage!.Navigation.PushAsync(new NoInternetPage(new MainPage()));
                    //return;
                }
                else
                {
                    UserDialogs.Instance.ShowLoading();
                    //var ViewModel = new SchedulesViewModel(1);
                    //var page = new Pages.SchedulePages.SchedulePage(1);
                    //page.BindingContext = ViewModel;
                    //await App.Current!.MainPage!.Navigation.PushAsync(page);
                    await App.Current!.MainPage!.Navigation.PushAsync(new Pages.SchedulePages.SchedulePage());

                    UserDialogs.Instance.HideHud();
                }
            }
            catch (Exception)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
                //throw;
            }

            IsBusy = false;
        }

        [RelayCommand]
        async Task SelectedAllEmployeesPage()
        {
            IsBusy = true;

            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await App.Current!.MainPage!.Navigation.PushAsync(new NoInternetPage(new MainPage()));
                    //return;
                }
                else
                {
                    UserDialogs.Instance.ShowLoading();

                    Controls.StaticMembers.EmployeesPages = 2;
                    await App.Current!.MainPage!.Navigation.PushAsync(new Pages.MenuPages.AllEmployeesPage());

                    UserDialogs.Instance.HideHud();
                }
            }
            catch (Exception)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
                //throw;
            }

            IsBusy = false;
        }

        [RelayCommand]
        async Task SelectedTimeSheetPage()
        {
            IsBusy = true;

            try
            {
                UserDialogs.Instance.ShowLoading();

                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await App.Current!.MainPage!.Navigation.PushAsync(new NoInternetPage(new MainPage()));
                }
                else
                {
                    await App.Current!.MainPage!.Navigation.PushAsync(new TimeSheetPage());
                }

                UserDialogs.Instance.HideHud();
            }
            catch (Exception)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
                throw;
            }

            IsBusy = false;
        }

        [RelayCommand]
        async Task SelectedLocationsPage()
        {
            IsBusy = true;
            //UserDialogs.Instance.ShowLoading();
            //await App.Current!.MainPage!.Navigation.PushAsync(new Pages.LocationPage());
            //UserDialogs.Instance.HideHud;
            IsBusy = false;
        }

        [RelayCommand]
        async Task SelectedReturnSalesPopup()
        {
            IsBusy = true;
            //UserDialogs.Instance.ShowLoading();
            //await PopupNavigation.Instance.PushAsync(new Pages.ReturnSalesPopup());
            //UserDialogs.Instance.HideHud;
            IsBusy = false;
        }

        [RelayCommand]
        async Task SelectedProductsPage()
        {
            IsBusy = true;
            //UserDialogs.Instance.ShowLoading();
            //await App.Current!.MainPage!.Navigation.PushAsync(new Pages.ProductsPage());
            //UserDialogs.Instance.HideHud;
            IsBusy = false;
        }

        [RelayCommand]
        async Task SelectedAccountPage()
        {
            IsBusy = true;

            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
                    //return;
                }
                else
                {
                    UserDialogs.Instance.ShowLoading();

                    await App.Current!.MainPage!.Navigation.PushAsync(new Pages.MenuPages.AccountPage());

                    UserDialogs.Instance.HideHud();
                }
            }
            catch (Exception)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
                //throw;
            }

            IsBusy = false;
        }



    }
}
