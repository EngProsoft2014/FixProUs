
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Controls.UserDialogs.Maui;
using FixProUs.Models;
using FixProUs.Pages;
using Mopups.Services;
using Newtonsoft.Json;
using Stripe;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace FixProUs.ViewModels
{
    public partial class CallsViewModel : BaseViewModel
    {

        readonly Services.Data.ServicesService _service = new Services.Data.ServicesService();

        [ObservableProperty]
        CallModel oneCall;

        [ObservableProperty]
        CustomersModel customerDetails;

        [ObservableProperty]
        ReasonModel oneReason;

        [ObservableProperty]
        CampaignModel oneCampaign;

        [ObservableProperty]
        ObservableCollection<CallModel> lstCalls;

        [ObservableProperty]
        ObservableCollection<ReasonModel> lstReasons;

        [ObservableProperty]
        ObservableCollection<CampaignModel> lstCampaigns;

        [ObservableProperty]
        int isShowBtnSch;

        [ObservableProperty]
        int totalCalls;

        [ObservableProperty]
        int createOrDetailsCall;


        Helpers.GenericRepository ORep = new Helpers.GenericRepository();


        public CallsViewModel()
        {
            Init();
            IsShowBtnSch = 0;
        }

        public CallsViewModel(CustomersModel model)
        {
            Init();
            IsShowBtnSch = 0;
            OneCall.PhoneNum = model.Phone1;
            OneCall.CustomerId = model.Id;
            OneCall.CustomerName = model.FirstName + " " + model.LastName;
        }

        public CallsViewModel(CallModel model)
        {
            Init();
            OneCall = model;
            IsShowBtnSch = 0;
            if (OneCall.Id != 0)
            {
                IsShowBtnSch = 1;//add sch
                if (OneCall.ScheduleId != 0 && OneCall.ScheduleId != null)
                {
                    IsShowBtnSch = 2;//Show sch
                }
            }

            if (OneCall.CustomerId != 0 && OneCall.CustomerId != null)
            {
                GetOneCustomerDetials(OneCall.CustomerId);
            }


            if (OneCall.ScheduleTitle == null)
                OneCall.ScheduleTitle = "";
        }


        async void Init()
        {
            LstCalls = new ObservableCollection<CallModel>();
            LstReasons = new ObservableCollection<ReasonModel>();
            LstCampaigns = new ObservableCollection<CampaignModel>();
            CustomerDetails = new CustomersModel();
            OneCall = new CallModel();
            OneReason = new ReasonModel();
            OneCampaign = new CampaignModel();

            await GetCalls();

            if (Controls.StaticMembers.CreateOrDetailsCall == 1)
            {
                UserDialogs.Instance.ShowLoading();
                await GetReasons();
                await GetCampaigns();
                UserDialogs.Instance.HideHud();
            }
        }

        //Get One Customer Detials
        async void GetOneCustomerDetials(int? CustId)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                UserDialogs.Instance.ShowLoading();

                string UserToken = await _service.UserToken();

                var json = await ORep.GetAsync<CustomersModel>(string.Format("api/Customers/GetOneCustDetails?" + "CustId=" + CustId), UserToken);

                if (json != null)
                {
                    CustomerDetails = json;
                }

                UserDialogs.Instance.HideHud();
            }
        }

        //Get Reasons
        async Task GetReasons()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                string UserToken = await _service.UserToken();

                var json = await ORep.GetAsync<ObservableCollection<ReasonModel>>(string.Format("api/Calls/GetReasons?" + "AccountId=" + Helpers.Settings.AccountIdGet), UserToken);

                if (json != null)
                {
                    LstReasons = json;
                    OneReason = LstReasons.Where(x => x.Id == OneCall.ReasonId).FirstOrDefault()!;
                }
            }
        }

        //Get Campaigns
        async Task GetCampaigns()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                string UserToken = await _service.UserToken();

                var json = await ORep.GetAsync<ObservableCollection<CampaignModel>>(string.Format("api/Calls/GetCampaigns?" + "AccountId=" + Helpers.Settings.AccountIdGet), UserToken);

                if (json != null)
                {
                    LstCampaigns = json;
                    OneCampaign = LstCampaigns.Where(x => x.Id == OneCall.CampaignId).FirstOrDefault()!;
                }
            }
        }

        //Get Calls
        async Task GetCalls()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                UserDialogs.Instance.ShowLoading();

                string UserToken = await _service.UserToken();

                var json = await ORep.GetAsync<ObservableCollection<CallModel>>(string.Format("api/Calls/GetAllCalls?" + "AccountId=" + Helpers.Settings.AccountIdGet), UserToken);

                if (json != null)
                {
                    LstCalls = json;
                    TotalCalls = LstCalls.Count;
                }

                UserDialogs.Instance.HideHud();
            }

        }

        [RelayCommand]
        async Task SelectCallDetails(CallModel model)
        {
            IsBusy = true;
            UserDialogs.Instance.ShowLoading();
            Controls.StaticMembers.CreateOrDetailsCall = 1; //For Get Reasons and Campaigns
            var ViewModel = new CallsViewModel(model);
            var page = new Pages.CallPages.NewCallPage();
            page.BindingContext = ViewModel;
            await App.Current!.MainPage!.Navigation.PushAsync(page);
            UserDialogs.Instance.HideHud();
            IsBusy = false;
        }

        [RelayCommand]
        async Task CreateNewCall()
        {
            IsBusy = true;
            UserDialogs.Instance.ShowLoading();
            Controls.StaticMembers.CreateOrDetailsCall = 1; //For Get Reasons and Campaigns
            var ViewModel = new CallsViewModel();
            var page = new Pages.CallPages.NewCallPage();
            page.BindingContext = ViewModel;
            await App.Current!.MainPage!.Navigation.PushAsync(page);
            UserDialogs.Instance.HideHud();
            IsBusy = false;
        }

        [RelayCommand]
        async Task CreateNewCustomer()
        {
            IsBusy = true;
            UserDialogs.Instance.ShowLoading();
            Controls.StaticMembers.WayCreateCust = 1;
            var ViewModel = new CustomersViewModel(true);
            var page = new Pages.CustomerPages.CreateNewCustomerPage();
            page.BindingContext = ViewModel;
            await App.Current!.MainPage!.Navigation.PushAsync(page);
            await MopupService.Instance.PopAsync();
            UserDialogs.Instance.HideHud();
            IsBusy = false;
        }

        [RelayCommand]
        async Task OpenFilterCalls()
        {
            IsBusy = true;

            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
                    return;
                }
                else
                {
                    string UserToken = await _service.UserToken();

                    var pageView = new FilterCallsViewModel();
                    pageView.CallClose += async (call) =>
                    {
                        UserDialogs.Instance.ShowLoading();

                        var json = await ORep.GetAsync<ObservableCollection<CallModel>>(string.Format("api/Calls/GetFilterCalls?" + "StartDate=" + call.StartDate + "&" + "EndDate=" + call.EndDate + "&" + "PhoneNum=" + call.PhoneNum + "&" + "ReasonId=" + call.ReasonId + "&" + "CampaignId=" + call.CampaignId + "&" + "EmployeeId=" + call.CreateUser + "&" + "SchTitle=" + call.ScheduleTitle), UserToken);

                        if (json != null)
                        {
                            LstCalls = json;

                            TotalCalls = LstCalls.Count;
                        }
                        UserDialogs.Instance.HideHud();
                    };

                    var page = new Pages.CallPages.FilterCallPage();
                    page.BindingContext = pageView;
                    await App.Current!.MainPage!.Navigation.PushAsync(page);
                }
            }
            catch (Exception ex)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
            }

            IsBusy = false;
        }

        [RelayCommand]
        async Task SubmitCall(CallModel model)
        {
            IsBusy = true;

            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
                    return;
                }
                else
                {
                    string UserToken = await _service.UserToken();

                    OneCall = model;

                    OneCall.AccountId = int.Parse(Helpers.Settings.AccountIdGet);
                    OneCall.BrancheId = int.Parse(Helpers.Settings.BranchIdGet);
                    OneCall.Active = true;
                    OneCall.CreateUser = int.Parse(Helpers.Settings.UserIdGet);
                    OneCall.ReasonId = OneReason != null ? OneReason.Id : 0;
                    OneCall.CampaignId = OneCampaign != null ? OneCampaign.Id : 0;

                    if (string.IsNullOrEmpty(model.PhoneNum))
                    {
                        await App.Current!.MainPage!.DisplayAlert("Alert", $"Please Complete This Field Required: Phone.", "Ok");
                    }
                    else if (OneReason == null)
                    {
                        await App.Current!.MainPage!.DisplayAlert("Alert", $"Please Complete This Field Required: Choose Reason.", "Ok");
                    }
                    else if (OneCampaign == null)
                    {
                        await App.Current!.MainPage!.DisplayAlert("Alert", $"Please Complete This Field Required : Choose Campaign.", "Ok");
                    }
                    else if (string.IsNullOrEmpty(model.Notes))
                    {
                        await App.Current!.MainPage!.DisplayAlert("Alert", $"Please Complete This Field Required: Notes.", "Ok");
                    }
                    else
                    {
                        if (OneCall != null)
                        {
                            var json = "";

                            if (model.Id == 0)
                            {
                                UserDialogs.Instance.ShowLoading();
                                json = await ORep.PostDataAsync("api/Calls/PostCall", OneCall, UserToken);
                                UserDialogs.Instance.HideHud();
                            }
                            else
                            {
                                UserDialogs.Instance.ShowLoading();
                                json = await ORep.PutDataAsync("api/Calls/PutCall", OneCall, UserToken);
                                UserDialogs.Instance.HideHud();
                            }

                            if (json != "Bad Request" && json != "api not responding")
                            {
                                await App.Current!.MainPage!.DisplayAlert("FixPro", "Succes for Save Call.", "Ok");

                                CallModel Call = JsonConvert.DeserializeObject<CallModel>(json)!;

                                var ViewModel = new CallsViewModel(Call);
                                var page = new Pages.CallPages.NewCallPage();
                                page.BindingContext = ViewModel;
                                await App.Current.MainPage.Navigation.PushAsync(page);
                                App.Current.MainPage.Navigation.RemovePage(App.Current.MainPage.Navigation.NavigationStack[App.Current.MainPage.Navigation.NavigationStack.Count - 2]);
                            }
                            else
                            {
                                await App.Current!.MainPage!.DisplayAlert("Alert", "Faild for add or edit Call.", "Ok");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
            }

            IsBusy = false;
        }

        [RelayCommand]
        async Task DeleteCall(int CallId)
        {
            IsBusy = true;
            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
                    return;
                }
                else
                {
                    UserDialogs.Instance.ShowLoading();
                    string UserToken = await _service.UserToken();

                    var json = await ORep.DeleteStrItemAsync(string.Format("api/Calls/DeleteCall/{0}", CallId), UserToken);

                    if (json != null && json != "api not responding")
                    {
                        await App.Current!.MainPage!.DisplayAlert("FixPro", "Succes Delete Call.", "Ok");
                        await App.Current.MainPage.Navigation.PushAsync(new MainPage());
                    }
                    else
                    {
                        await App.Current!.MainPage!.DisplayAlert("Alert", "Failed Delete Call.", "Ok");
                    }
                    UserDialogs.Instance.HideHud();
                }
            }
            catch (Exception ex)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
            }

            IsBusy = false;
        }

        [RelayCommand]
        async Task SelectGoJob(CallModel model)
        {
            IsBusy = true;
            UserDialogs.Instance.ShowLoading();
            var ViewModel = new SchedulesViewModel(model.ScheduleId!.Value, model.ScheduleDateId!.Value);
            var page = new Pages.SchedulePages.ScheduleDetailsPage();
            page.BindingContext = ViewModel;
            await App.Current!.MainPage!.Navigation.PushAsync(page);
            UserDialogs.Instance.HideHud();
            IsBusy = false;
        }

        [RelayCommand]
        async Task CreateScheduleFromCall(CustomersModel model)
        {
            IsBusy = true;

            UserDialogs.Instance.ShowLoading();

            if (model.Id != 0)
            {
                SchedulesViewModel ViewModel;
                if (Controls.StaticMembers.WayAfterChooseCust == 0)
                {
                    model.CallId = OneCall.Id;
                    ViewModel = new SchedulesViewModel(model);
                    var page = new Pages.SchedulePages.NewSchedulePage();
                    page.BindingContext = ViewModel;
                    await App.Current!.MainPage!.Navigation.PushAsync(page);
                }
            }
            else
            {
                await App.Current!.MainPage!.DisplayAlert("Alert", "Sorry, This Call Don't Have Customer Details.", "Ok");
            }

            UserDialogs.Instance.HideHud();

            IsBusy = false;
        }

        [RelayCommand]
        async Task ResetCalls()
        {
            IsBusy = true;

            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
                    return;
                }
                else
                {
                    UserDialogs.Instance.ShowLoading();

                    await GetCalls();

                    UserDialogs.Instance.HideHud();
                }
            }
            catch (Exception ex)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
            }

            IsBusy = false;
        }
    }
}
