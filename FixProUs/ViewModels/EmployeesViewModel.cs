
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Controls.UserDialogs.Maui;
using FixProUs.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reactive;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;


namespace FixProUs.ViewModels
{
    public partial class EmployeesViewModel : BaseViewModel
    {

        readonly Services.Data.ServicesService _service = new Services.Data.ServicesService();

        [ObservableProperty]
        ObservableCollection<EmployeeModel> lstEmployeesInBranch;

        [ObservableProperty]
        ObservableCollection<EmployeeModel> lstEmployees;

        [ObservableProperty]
        ObservableCollection<EmployeeModel> lstEmployeesOnePage;

        [ObservableProperty]
        ObservableCollection<EmployeeModel> lstWorkingEmployees;

        [ObservableProperty]
        ObservableCollection<DataMapsModel> listmap;

        [ObservableProperty]
        ObservableCollection<DataMapsModel> lastListmap;

        [ObservableProperty]
        EmployeeModel oneEmployee;

        [ObservableProperty]
        EmployeeModel employeePermission;

        [ObservableProperty]
        bool isRefresh;


        public int PageNumber { get; set; }

        public int TotalPage { get; set; }

        public int BranchIdVM { get; set; }

        public string BranchNameVM { get; set; }

        public static DataMapsModel MapsModel { get; set; }

        Helpers.GenericRepository ORep = new Helpers.GenericRepository();

        DataTable employeesTable;

        DataMapsModel CurrentTrack { get; set; }
        DataSet ds = new DataSet();
        XDocument document = new XDocument();

        public EmployeesViewModel()
        {
            Init();

            Listmap = new ObservableCollection<DataMapsModel>();
            LastListmap = new ObservableCollection<DataMapsModel>();
        }

        //Tracking Constructor
        public EmployeesViewModel(EmployeeModel employee)
        {
            OneEmployee = employee;
            Listmap = new ObservableCollection<DataMapsModel>();
            LastListmap = new ObservableCollection<DataMapsModel>();
            CurrentTrack = new DataMapsModel();
            GetDataEmployee();
            new Timer((Object stateInfo) => { GetDataEmployee(); }, new AutoResetEvent(false), 0, 3000);
        }

        //Employees Working Today Constructor
        public EmployeesViewModel(string startTracking)
        {

            InitTraking();
        }

        async void Init()
        {
            PageNumber = 1;
            LstEmployeesOnePage = new ObservableCollection<EmployeeModel>();
            LstEmployees = new ObservableCollection<EmployeeModel>();
            LstEmployeesInBranch = new ObservableCollection<EmployeeModel>();
            BranchIdVM = int.Parse(Helpers.Settings.BranchIdGet);
            BranchNameVM = Helpers.Settings.BranchNameGet;

            GetPerrmission();

            if (Controls.StaticMembers.EmployeesPages == 2)
            {
                await GetEmployees();
            }

        }

        async void InitTraking()
        {
            if (Connectivity.NetworkAccess == Microsoft.Maui.Networking.NetworkAccess.Internet)
            {
                LstWorkingEmployees = new ObservableCollection<EmployeeModel>();

                string Date = DateTime.Now.ToString("yyyy-MM-dd");

                await Controls.StartData.GetWorkingEmployees(int.Parse(Helpers.Settings.AccountIdGet), Date);

                LstWorkingEmployees = Controls.StartData.LstWorkingEmployeesStatic;
            }     
        }

        //Get Perrmission for User
        public async void GetPerrmission()
        {
            if (Connectivity.NetworkAccess == Microsoft.Maui.Networking.NetworkAccess.Internet)
            {
                EmployeePermission = new EmployeeModel();
                await Controls.StartData.CheckPermissionEmployee();
                EmployeePermission = Controls.StartData.EmployeeDataStatic;
            }      
        }

        //Get All Employees
        public async Task GetEmployees()
        {         
            if (Connectivity.NetworkAccess == Microsoft.Maui.Networking.NetworkAccess.Internet)
            {
                UserDialogs.Instance.ShowLoading();

                string UserToken = await _service.UserToken();

                var json = await ORep.GetAsync<EmployeesInPageModel>(string.Format("api/Employee/GetEmpInPage/{0}/{1}/{2}/{3}/{4}", PageNumber, Helpers.Settings.AccountIdGet, Helpers.Settings.BranchIdGet, Controls.StartData.EmployeeDataStatic.UserRole, Helpers.Settings.UserIdGet), UserToken);

                if (json != null)
                {
                    EmployeesInPageModel Employee = json;
                    TotalPage = Employee.Pages;
                    PageNumber += 1;

                    LstEmployeesOnePage = new ObservableCollection<EmployeeModel>(Employee.EmployeesInPage);

                    if (LstEmployees.Count == 0)
                    {
                        LstEmployees = new ObservableCollection<EmployeeModel>(LstEmployeesOnePage.OrderBy(x => x.UserName).ToList());
                    }
                    else
                    {
                        if (LstEmployees != LstEmployeesOnePage)
                        {
                            LstEmployees = new ObservableCollection<EmployeeModel>(LstEmployees.Concat(LstEmployeesOnePage).OrderBy(x => x.UserName).ToList());
                        }
                    }
                }

                UserDialogs.Instance.HideHud();
            }
                
        }

        private async void GetDataEmployee()
        {
            try
            {
                if (Connectivity.NetworkAccess != Microsoft.Maui.Networking.NetworkAccess.Internet)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
                    return;
                }
                else
                {
                    string uri = "https://fixpro.engprosoft.net/XMLData/" + OneEmployee.Id + ".xml";

                    document = XDocument.Load(uri);

                    ds.Clear();
                    ds.ReadXml(new XmlTextReader(new StringReader(document.ToString())));

                    employeesTable = ds.Tables[0];

                    CurrentTrack = (from DataRow dr in employeesTable.Rows
                                    select new DataMapsModel()
                                    {
                                        Id = int.Parse(dr["Tracking_id"].ToString()),
                                        EmployeeId = int.Parse(dr["EmployeeId"].ToString()),
                                        Lat = dr["lat"].ToString(),
                                        Long = dr["log"].ToString(),
                                        Time = dr["time"].ToString(),
                                        CreateDate = dr["date"].ToString(),
                                        MPosition = new Location(double.Parse(dr["lat"].ToString()), double.Parse(dr["log"].ToString())),
                                    }).FirstOrDefault();

                    LastListmap.Clear();
                    LastListmap.Add(CurrentTrack);
                    MapsModel = CurrentTrack;
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async void SelectedEmployeeInMap(EmployeeModel employee)
        {
            IsBusy = true;

            try
            {
                if (Connectivity.NetworkAccess != Microsoft.Maui.Networking.NetworkAccess.Internet)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
                    return;
                }
                else
                {
                    UserDialogs.Instance.ShowLoading();
                    var popupView = new EmployeesViewModel(employee);
                    var page = new Pages.MenuPages.TrckingMapPage(MapsModel);
                    page.BindingContext = popupView;
                    await App.Current.MainPage.Navigation.PushAsync(page);
                    UserDialogs.Instance.HideHud();
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }

            IsBusy = false;
        }
    }
}
