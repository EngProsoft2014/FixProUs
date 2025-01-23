
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Controls.UserDialogs.Maui;
using FixProUs.Models;
using FixProUs.ViewModels;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace FixProUs.ViewModels
{
    public partial class FilterCallsViewModel : BaseViewModel
    {

        readonly Services.Data.ServicesService _service = new Services.Data.ServicesService();

        [ObservableProperty]
        CallModel oneFilter;

        [ObservableProperty]
        ReasonModel oneReason;

        [ObservableProperty]
        CampaignModel oneCampaign;

        [ObservableProperty]
        EmployeeModel oneEmployee;

        [ObservableProperty]
        ObservableCollection<ReasonModel> lstReasons;

        [ObservableProperty]
        ObservableCollection<CampaignModel> lstCampaigns;

        [ObservableProperty]
        ObservableCollection<EmployeeModel> lstEmployees;

        [ObservableProperty]
        DateTime startDate;

        [ObservableProperty]
        DateTime endDate;

        [ObservableProperty]
        string schTitle;

        [ObservableProperty]
        bool withDate;


        Helpers.GenericRepository ORep = new Helpers.GenericRepository();

        public delegate void CallDelegte(CallModel Call);
        public event CallDelegte CallClose;


        public FilterCallsViewModel()
        {
            Init();
            if(Controls.StaticMembers.FilterCallModel != null)
            {
                OneFilter = Controls.StaticMembers.FilterCallModel;
                if (string.IsNullOrEmpty(OneFilter.StartDate) != true)
                {
                    StartDate = DateTime.Parse(OneFilter.StartDate);
                    WithDate = true;
                }
                if (string.IsNullOrEmpty(OneFilter.EndDate) != true)
                {
                    EndDate = DateTime.Parse(OneFilter.EndDate);
                    WithDate = true;
                }
            }   

        }

        async void Init()
        {
            LstReasons = new ObservableCollection<ReasonModel>();
            LstCampaigns = new ObservableCollection<CampaignModel>();
            LstEmployees = new ObservableCollection<EmployeeModel>();
            OneFilter = new CallModel();
            OneReason = new ReasonModel();
            OneCampaign = new CampaignModel();
            OneEmployee = new EmployeeModel();

            StartDate = DateTime.Now;
            EndDate = DateTime.Now;

            WithDate = false;

            UserDialogs.Instance.ShowLoading();
            await GetReasons();
            await GetCampaigns();
            await GetEmployeesInCall();
            UserDialogs.Instance.HideHud();
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

                    OneReason = LstReasons.Where(x => x.Id == OneFilter.ReasonId).FirstOrDefault();
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

                    OneCampaign = LstCampaigns.Where(x => x.Id == OneFilter.CampaignId).FirstOrDefault();
                }
            }      
        }

        // Get Employees 
        async Task GetEmployeesInCall()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                string UserToken = await _service.UserToken();

                var json = await ORep.GetAsync<ObservableCollection<EmployeeModel>>(string.Format("api/Employee/GetEmpInCall/{0}/{1}/{2}", Helpers.Settings.AccountIdGet, Controls.StartData.EmployeeDataStatic.UserRole, Helpers.Settings.UserIdGet), UserToken);

                if (json != null)
                {
                    LstEmployees = json;

                    OneEmployee = LstEmployees.Where(x => x.Id == OneFilter.CreateUser).FirstOrDefault();
                }
            }       
        }

        [RelayCommand]
        async void ApplyFilterCalls(CallModel model)
        {
            IsBusy = true;

            model.PhoneNum = model.PhoneNum != null ? model.PhoneNum : "";
            model.ReasonId = OneReason?.Id;
            model.CampaignId = OneCampaign?.Id;
            model.ScheduleTitle = SchTitle;
            model.CreateUser = OneEmployee?.Id;

            if (WithDate == true)
            {
                model.StartDate = StartDate.ToString("MM-dd-yyyy");
                model.EndDate = EndDate.ToString("MM-dd-yyyy");
            }

            Controls.StaticMembers.FilterCallModel = model;

            CallClose.Invoke(model);
            await App.Current!.MainPage!.Navigation.PopAsync();

            IsBusy = false;
        }
    }
}
