
using GoogleApi.Entities.Translate.Common.Enums;
using Newtonsoft.Json;
using Stripe;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Xml.Linq;
using Stripe.FinancialConnections;
using FixProUs.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Controls.UserDialogs.Maui;
using CommunityToolkit.Maui;
using Mopups.Services;
using FixProUs.Pages;


namespace FixProUs.ViewModels
{
    public partial class CustomersViewModel : BaseViewModel
    {

        #region Properties
        [ObservableProperty]
        CampaignModel oneCampaign;

        [ObservableProperty]
        ObservableCollection<CustomersModel> lstCustomers;

        [ObservableProperty]
        ObservableCollection<SchedulesModel> lstSchedules;

        [ObservableProperty]
        ObservableCollection<InvoiceModel> lstInvoices;

        [ObservableProperty]
        ObservableCollection<EstimateModel> lstEstimates;

        [ObservableProperty]
        ObservableCollection<InvoiceItemServicesModel> lstItemsInvoice;

        [ObservableProperty]
        ObservableCollection<EstimateItemServicesModel> lstItemsEstimate;

        [ObservableProperty]
        ObservableCollection<CampaignModel> lstCampaigns;

        [ObservableProperty]
        ObservableCollection<SchaduleDateModel> lstEstimateDates;

        [ObservableProperty]
        ObservableCollection<SchaduleDateModel> lstInvoiceDates;

        [ObservableProperty]
        ObservableCollection<SchaduleDateModel> lstEstimateSchaduleDates;

        [ObservableProperty]
        ObservableCollection<SchaduleDateModel> lstEstimateSchaduleDatesActual;

        [ObservableProperty]
        ObservableCollection<SchaduleDateModel> lstInvoiceSchaduleDates;

        [ObservableProperty]
        ObservableCollection<SchaduleDateModel> lstInvoiceSchaduleDatesActual;

        [ObservableProperty]
        ObservableCollection<CustomersCustomFieldModel> lstCustomField;

        [ObservableProperty]
        CustomersModel customerDetails;

        [ObservableProperty]
        SchedulesModel scheduleDetails;

        [ObservableProperty]
        InvoiceModel oneInvoice;

        [ObservableProperty]
        EstimateModel oneEstimate;

        [ObservableProperty]
        CustomersCategoryModel oneCategoryModel;

        [ObservableProperty]
        MemberModel oneMemberModel;

        [ObservableProperty]
        TaxModel oneTaxModel;

        [ObservableProperty]
        CustomerfeaturesModel customerFeatures;

        [ObservableProperty]
        EmployeeModel employeePermission;

        [ObservableProperty]
        int lstHeight;

        [ObservableProperty]
        string branchName;

        [ObservableProperty]
        string phoneView;

        [ObservableProperty]
        decimal? discount;

        [ObservableProperty]
        decimal? subTotal;

        [ObservableProperty]
        decimal? net;

        [ObservableProperty]
        decimal? paid;

        [ObservableProperty]
        decimal? totalDue;

        [ObservableProperty]
        bool pending;

        [ObservableProperty]
        bool accept;

        [ObservableProperty]
        bool declind;

        [ObservableProperty]
        string address;

        [ObservableProperty]
        string houseValue;

        [ObservableProperty]
        string _yearBuilt;

        [ObservableProperty]
        string squareFootage;

        [ObservableProperty]
        bool showArrowsBar;

        [ObservableProperty]
        bool showArrowsBarFeatures;

        [ObservableProperty]
        string signatureImageByte64Estimate;

        [ObservableProperty]
        string city;

        [ObservableProperty]
        string state;

        [ObservableProperty]
        string zipCode;

        [ObservableProperty]
        bool showScheduleName;

        [ObservableProperty]
        bool showEstimateConvertToInvoice;

        [ObservableProperty]
        bool withContract;

        [ObservableProperty]
        bool amountOrPersent;

        [ObservableProperty]
        bool showDropdownDatesEstimate;

        [ObservableProperty]
        bool showDropdownDatesInvoice;

        [ObservableProperty]
        bool isShowScheduleDates;

        [ObservableProperty]
        string strEstimateDates;

        [ObservableProperty]
        string strInvoiceDates;

        [ObservableProperty]
        int isUpdateCust;

        [ObservableProperty]
        bool isMemberShip;

        public int BranchIdVM; 
        #endregion

        Helpers.GenericRepository ORep = new Helpers.GenericRepository();
        readonly Services.Data.ServicesService _service = new Services.Data.ServicesService();

        public CustomersViewModel()
        {
            IsBusy = true;

            BranchName = Helpers.Settings.BranchNameGet;
            BranchIdVM = int.Parse(Helpers.Settings.BranchIdGet);
            LstCustomers = new ObservableCollection<CustomersModel>();

            GetAllCustomers();
        }

        //Feature Part To Create New Customer
        public CustomersViewModel(bool FeaturePart)
        {
            GetPerrmission();

            Init();

            GetCampaigns();
            GetCustomerFeatures(int.Parse(Helpers.Settings.AccountIdGet));
        }

        //List Customers or Update Customer
        public CustomersViewModel(CustomersModel model)
        {
            GetPerrmission();

            LstInvoices = new ObservableCollection<InvoiceModel>();
            LstSchedules = new ObservableCollection<SchedulesModel>();
            CustomerDetails = new CustomersModel();
            CustomerDetails.LstSchedules = new List<SchedulesModel>();
            CustomerDetails.LstInvoices = new List<InvoiceModel>();
            LstEstimates = new ObservableCollection<EstimateModel>();
            CustomerDetails.LstEstimates = new List<EstimateModel>();

            Init();

            CustomerDetails = model;

            GetCampaigns();
            GetCustomerFeatures(int.Parse(Helpers.Settings.AccountIdGet));


            if (CustomerDetails.MemeberType == true)
            {
                if (CustomerDetails.MemberDTO != null)
                {
                    Discount = CustomerDetails.MemberDTO.MemberValue;
                }
            }
            else
            {
                Discount = CustomerDetails.Discount;
            }

            if (Discount == null)
            {
                Discount = 0;
            }

            GetListsForCustomer(model.Id);
        }

        //Update Customer
        public CustomersViewModel(CustomersModel model, int isUpdateCust)
        {
            IsUpdateCust = isUpdateCust;
            Init();

            CustomerDetails = model;

            if (CustomerDetails.MemeberType == true)
            {
                if (CustomerDetails.MemberDTO != null)
                {
                    CustomerDetails.Discount = CustomerDetails.MemberDTO.MemberValue;
                }
            }

            if (CustomerDetails.MemeberType == true)
            {
                IsMemberShip = true;
            }
            else
            {
                IsMemberShip = false;
            }

            GetCampaigns();
            GetCustomerFeatures(int.Parse(Helpers.Settings.AccountIdGet));

            Address = CustomerDetails.Address;
            YearBuilt = CustomerDetails.YearBuit;
            HouseValue = CustomerDetails.EstimedValue;
            SquareFootage = CustomerDetails.Squirefootage;
        }

        //From Invoices Tab
        public CustomersViewModel(InvoiceModel model, CustomersModel Cust)
        {
            GetPerrmission();

            IsShowScheduleDates = true; //Show schedule Dates

            if (model.ScheduleId != null)
                ShowScheduleName = true;

            LstItemsInvoice = new ObservableCollection<InvoiceItemServicesModel>();
            LstInvoiceSchaduleDates = new ObservableCollection<SchaduleDateModel>();
            LstInvoiceSchaduleDatesActual = new ObservableCollection<SchaduleDateModel>();
            LstInvoiceDates = new ObservableCollection<SchaduleDateModel>();
            OneInvoice = new InvoiceModel();

            if (model.LstScdDate != null)
            {
                LstInvoiceSchaduleDatesActual = new ObservableCollection<SchaduleDateModel>(model.LstScdDate);
            }

            GetOneInvoiceDetails(model.Id, null);

            CustomerDetails = Cust;
            BranchName = Helpers.Settings.BranchNameGet;
        }

        //From Estimate Tab
        public CustomersViewModel(EstimateModel model, CustomersModel Cust)
        {
            GetPerrmission();

            IsShowScheduleDates = true; //Show schedule Dates

            if (model.ScheduleId != null)
                ShowScheduleName = true;

            LstItemsEstimate = new ObservableCollection<EstimateItemServicesModel>();
            LstEstimateSchaduleDates = new ObservableCollection<SchaduleDateModel>();
            LstEstimateSchaduleDatesActual = new ObservableCollection<SchaduleDateModel>();
            LstEstimateDates = new ObservableCollection<SchaduleDateModel>();
            OneEstimate = new EstimateModel();

            if (model.LstScdDate != null)
            {
                LstEstimateSchaduleDatesActual = new ObservableCollection<SchaduleDateModel>(model.LstScdDate);
            }

            GetOneInvoiceDetails(null, model.Id);

            CustomerDetails = Cust;
            BranchName = Helpers.Settings.BranchNameGet;
        }

        //From Invoices Tab
        public CustomersViewModel(SchedulesModel model)
        {
            GetPerrmission();

            ScheduleDetails = new SchedulesModel();

            ScheduleDetails = model;

            BranchName = Helpers.Settings.BranchNameGet;
        }


        void Init()
        {
            BranchIdVM = int.Parse(Helpers.Settings.BranchIdGet);
            CustomerDetails = new CustomersModel();
            CustomerDetails.LstCustomersCustomField = new List<CustomersCustomFieldModel>();
            CustomerFeatures = new CustomerfeaturesModel();
            LstCampaigns = new ObservableCollection<CampaignModel>();
            OneCampaign = new CampaignModel();
            CustomerFeatures = new CustomerfeaturesModel();
            OneCategoryModel = new CustomersCategoryModel();
            OneMemberModel = new MemberModel();
            OneTaxModel = new TaxModel();
        }

        //Get Perrmission for User
        public async void GetPerrmission()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                EmployeePermission = new EmployeeModel();
                await Controls.StartData.CheckPermissionEmployee();
                EmployeePermission = Controls.StartData.EmployeeDataStatic;
            }
        }

        async Task GetScheduleDates(int ScheduleId, int Type)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                UserDialogs.Instance.ShowLoading();
                string UserToken = await _service.UserToken();
                var json = await ORep.GetAsync<ObservableCollection<SchaduleDateModel>>(string.Format("api/Schedules/GetScheduleDates?" + "ScheduleId=" + ScheduleId + "&" + "Type=" + Type), UserToken);

                if (json != null)
                {
                    LstEstimateSchaduleDates = json;
                    LstInvoiceSchaduleDates = json;
                }

                UserDialogs.Instance.HideHud();
            }

        }

        //Get Campaigns
        async void GetCampaigns()
        {
            string UserToken = await _service.UserToken();
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                var json = await ORep.GetAsync<ObservableCollection<CampaignModel>>(string.Format("api/Calls/GetCampaigns?" + "AccountId=" + Helpers.Settings.AccountIdGet), UserToken);

                if (json != null)
                {
                    LstCampaigns = json;

                    if (IsUpdateCust == 2)
                    {
                        OneCampaign = LstCampaigns.Where(x => x.Id == CustomerDetails.Source.Value).FirstOrDefault();
                    }
                }
            }

        }

        //Get One Estimate Or Invoice Details
        async Task GetOneInvoiceDetails(int? InvoiceId, int? EstimateId)
        {
            UserDialogs.Instance.ShowLoading();

            string UserToken = await _service.UserToken();

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                var Customer = await ORep.GetAsync<ObjectOfCustomerModel>(string.Format("api/Customers/GetObjectOfCustomer?" + "InvoiceId=" + InvoiceId + "&" + "EstimateId=" + EstimateId), UserToken);

                if (Customer != null)
                {
                    if (Customer.ObjInvoice != null)
                    {
                        OneInvoice = Customer.ObjInvoice;

                        AmountOrPersent = OneInvoice.DiscountAmountOrPercent == "%" ? false : true;
                        Discount = OneInvoice.Discount;

                        if (OneInvoice.ContractId != null)
                        {
                            WithContract = true;
                        }

                        if (Customer.ObjInvoice.ScheduleId != null && Customer.ObjInvoice.ScheduleId != 0)
                        {
                            ShowDropdownDatesInvoice = true;

                            await GetScheduleDates(Customer.ObjInvoice.ScheduleId.Value, 1); // All Schedule Dates

                            LstInvoiceSchaduleDatesActual = new ObservableCollection<SchaduleDateModel>(Customer.ObjInvoice.LstScdDate);

                            if (LstInvoiceSchaduleDatesActual.Count == 1)
                            {
                                IsShowScheduleDates = false; //Don't Show schedule Dates
                            }

                            string str = "";
                            LstInvoiceDates = new ObservableCollection<SchaduleDateModel>();
                            foreach (var Date in Customer.ObjInvoice.LstScdDate)
                            {
                                str += (" , " + Date.Date);
                                LstInvoiceDates.Add(new SchaduleDateModel
                                {
                                    Id = Date.Id,
                                    Date = Date.Date,
                                });
                            }

                            if (!string.IsNullOrEmpty(str))
                            {
                                if (!string.IsNullOrEmpty(StrInvoiceDates))
                                {
                                    StrInvoiceDates = string.Empty;
                                    StrInvoiceDates += str;
                                    StrInvoiceDates = str.Remove(0, 2);
                                }
                                else
                                {
                                    StrInvoiceDates = str.Remove(0, 2);
                                }
                            }
                        }

                        if (OneInvoice.LstInvoiceItemServices != null)
                        {
                            if (OneInvoice.LstInvoiceItemServices.Count > 4)
                            {
                                LstHeight = 1;
                            }

                            TotalInvoice(OneInvoice);

                            LstItemsInvoice = new ObservableCollection<InvoiceItemServicesModel>(OneInvoice.LstInvoiceItemServices);
                        }
                        else
                        {
                            SubTotal = 0;
                            Net = 0;
                            Paid = 0;
                            TotalDue = 0;
                        }
                    }

                    if (Customer.ObjEstimate != null)
                    {
                        //if (Customer.ObjEstimate.InvoiceId != 0 && Customer.ObjEstimate.InvoiceId != null || Customer.ObjEstimate.Status != 1)
                        if ((Customer.ObjEstimate.InvoiceId != 0 && Customer.ObjEstimate.InvoiceId != null) || (Customer.ObjEstimate.Status != 1 && Customer.ObjEstimate.Status != 0))
                        {
                            Customer.ObjEstimate.NotShowConvert = true;//NotShowConvert
                            if (Customer.ObjEstimate.InvoiceId > 0)
                            {
                                Customer.ObjEstimate.GoToInvoice = true;
                            }
                        }

                        if (Customer.ObjEstimate.ScheduleId != null && Customer.ObjEstimate.ScheduleId != 0)
                        {
                            ShowDropdownDatesEstimate = true;

                            await GetScheduleDates(Customer.ObjEstimate.ScheduleId.Value, 1); // All Schedule Dates

                            LstEstimateSchaduleDatesActual = new ObservableCollection<SchaduleDateModel>(Customer.ObjEstimate.LstScdDate);

                            if (LstEstimateSchaduleDatesActual.Count == 1)
                            {
                                IsShowScheduleDates = false; //Don't Show schedule Dates
                            }

                            string str = "";
                            LstEstimateDates = new ObservableCollection<SchaduleDateModel>();
                            foreach (var Date in Customer.ObjEstimate.LstScdDate)
                            {
                                str += (" , " + Date.Date);
                                LstEstimateDates.Add(new SchaduleDateModel
                                {
                                    Id = Date.Id,
                                    Date = Date.Date,
                                });
                            }

                            if (!string.IsNullOrEmpty(str))
                            {
                                if (!string.IsNullOrEmpty(StrEstimateDates))
                                {
                                    StrEstimateDates = string.Empty;
                                    StrEstimateDates += str;
                                    StrEstimateDates = str.Remove(0, 2);
                                }
                                else
                                {
                                    StrEstimateDates = str.Remove(0, 2);
                                }
                            }
                        }

                        OneEstimate = Customer.ObjEstimate;

                        AmountOrPersent = OneEstimate.DiscountAmountOrPercent == "%" ? false : true;
                        Discount = OneEstimate.Discount;

                        Pending = OneEstimate.Status == 0 ? true : false;
                        Accept = OneEstimate.Status == 1 ? true : false;
                        Declind = OneEstimate.Status == 2 ? true : false;

                        if (OneEstimate.LstEstimateItemServices != null)
                        {
                            if (OneEstimate.LstEstimateItemServices.Count > 4)
                            {
                                LstHeight = 1;
                            }

                            TotalEstimate(OneEstimate);

                            LstItemsEstimate = new ObservableCollection<EstimateItemServicesModel>(OneEstimate.LstEstimateItemServices);
                        }
                        else
                        {
                            SubTotal = 0;
                            Net = 0;
                            Paid = 0;
                            TotalDue = 0;
                        }
                    }
                }
            }

            UserDialogs.Instance.HideHud();
        }

        //Get all Customers in Branch
        async void GetAllCustomers()
        {
            UserDialogs.Instance.ShowLoading();

            string UserToken = await _service.UserToken();
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                var json = await ORep.GetAsync<ObservableCollection<CustomersModel>>(string.Format("api/Customers/GetAllCustInBranch?" + "AccountId=" + Helpers.Settings.AccountIdGet), UserToken);

                if (json != null)
                {
                    LstCustomers = json;
                }
            }

            UserDialogs.Instance.HideHud();
        }

        //Get Lists For Customer
        public async void GetListsForCustomer(int? CustomerId)
        {
            UserDialogs.Instance.ShowLoading();

            string UserToken = await _service.UserToken();

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                var Customer = await ORep.GetAsync<CustomersModel>(string.Format("api/Customers/GetListsOfCustomer?" + "CustomerId=" + CustomerId), UserToken);

                if (Customer != null)
                {
                    CustomerDetails = Customer;
                    Discount = CustomerDetails.Discount == null ? 0 : CustomerDetails.Discount.Value;
                    ShowArrowsBar = CustomerDetails.LstCustomersCustomField.Count > 4 ? true : false;


                    foreach (EstimateModel ets in CustomerDetails.LstEstimates)
                    {
                        if ((ets.InvoiceId != 0 && ets.InvoiceId != null) || ets.Status != 1 || EmployeePermission.UserRole == 1 || EmployeePermission.ActiveEstimate == false || EmployeePermission.ActiveEditPrice == false)
                        {
                            ets.NotShowConvert = true;//NotShowConvert
                            if (ets.InvoiceId > 0)
                            {
                                ets.GoToInvoice = true;
                            }
                        }
                        //CustomerDetails.LstEstimates.Add(ets);
                    }

                    LstInvoices = new ObservableCollection<InvoiceModel>(CustomerDetails.LstInvoices);
                    LstSchedules = new ObservableCollection<SchedulesModel>(CustomerDetails.LstSchedules);
                    LstEstimates = new ObservableCollection<EstimateModel>(CustomerDetails.LstEstimates);
                }
            }

            UserDialogs.Instance.HideHud();
        }

        // New Customer or Update Customer
        async void GetCustomerFeatures(int? AccountId)
        {
            UserDialogs.Instance.ShowLoading();

            string UserToken = await _service.UserToken();
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                var Features = await ORep.GetAsync<CustomerfeaturesModel>(string.Format("api/Customers/GetCustomerFeatures?" + "AccountId=" + AccountId), UserToken);

                if (Features != null)
                {
                    CustomerFeatures = Features;

                    ShowArrowsBarFeatures = CustomerFeatures.LstCustomersCustomField.Count > 4 ? true : false;

                    if (IsUpdateCust == 2)// Update Customer
                    {

                        CustomerDetails.LstCustomersCustomField = CustomerFeatures.LstCustomersCustomField;

                        OneCategoryModel = CustomerFeatures.LstCustomerCategory.Where(x => x.Id == CustomerDetails.CategoryId).FirstOrDefault();

                        OneMemberModel = CustomerFeatures.LstMemberships.Where(x => x.Id == CustomerDetails.MemberDTO?.Id).FirstOrDefault();

                        OneTaxModel = CustomerFeatures.LstTaxes.Where(x => x.Id == CustomerDetails.TaxDTO?.Id).FirstOrDefault();
                    }
                }
            }

            UserDialogs.Instance.HideHud();
        }

        public void TotalInvoice(InvoiceModel model)
        {
            decimal? SumCost = model.LstInvoiceItemServices.Where(x => x.Price > 0 && (x.SkipOfTotal == false || x.SkipOfTotal == null)).Sum(s => s.Price * s.Quantity);
            decimal? DiscountVal = 0;
            if (model.DiscountAmountOrPercent == "%")
            {
                DiscountVal = SumCost * Discount / 100;
            }
            else
            {
                DiscountVal = Discount;
            }
            SubTotal = Math.Round(SumCost.Value, 2, MidpointRounding.ToEven);
            Paid = model.Status == 0 ? 0 : model.Paid;
            if (model.Taxval != null)
            {
                Net = model.DiscountAmountOrPercent == "%" ? Math.Round(((SumCost - DiscountVal) + model?.Taxval).Value, 2, MidpointRounding.ToEven) : Math.Round(((SumCost - DiscountVal) + model?.Taxval).Value, 2, MidpointRounding.ToEven);
                TotalDue = model.DiscountAmountOrPercent == "%" ? Math.Round(((SumCost - DiscountVal) + model.Taxval - Paid).Value, 2, MidpointRounding.ToEven) : Math.Round(((SumCost - DiscountVal) + model.Taxval - Paid).Value, 2, MidpointRounding.ToEven);
            }
            else
            {
                decimal? TaxValue = model.DiscountAmountOrPercent == "%" ? ((SumCost - DiscountVal) * model.Tax / 100) : ((SumCost - DiscountVal) * model.Tax / 100);
                Net = model.DiscountAmountOrPercent == "%" ? Math.Round(((SumCost - DiscountVal) + TaxValue).Value, 2, MidpointRounding.ToEven) : Math.Round(((SumCost - DiscountVal) + TaxValue).Value, 2, MidpointRounding.ToEven);
                TotalDue = model.DiscountAmountOrPercent == "%" ? Math.Round(((SumCost - DiscountVal) + TaxValue - Paid).Value, 2, MidpointRounding.ToEven) : Math.Round(((SumCost - DiscountVal) + TaxValue - Paid).Value, 2, MidpointRounding.ToEven);
            }
        }

        public void TotalEstimate(EstimateModel model)
        {
            decimal? SumCost = model.LstEstimateItemServices.Where(x => x.Price > 0).Sum(s => s.Price * s.Quantity);
            decimal? DiscountVal = 0;
            if (model.DiscountAmountOrPercent == "%")
            {
                DiscountVal = SumCost * Discount / 100;
            }
            else
            {
                DiscountVal = Discount;
            }
            SubTotal = Math.Round(SumCost.Value, 2, MidpointRounding.ToEven);
            Paid = 0;

            if (model.Taxval != null)
            {
                Net = model.DiscountAmountOrPercent == "%" ? Math.Round(((SumCost - DiscountVal) + model?.Taxval).Value, 2, MidpointRounding.ToEven) : Math.Round(((SumCost - DiscountVal) + model?.Taxval).Value, 2, MidpointRounding.ToEven);
                TotalDue = model.DiscountAmountOrPercent == "%" ? Math.Round(((SumCost - DiscountVal) + model.Taxval - Paid).Value, 2, MidpointRounding.ToEven) : Math.Round(((SumCost - DiscountVal) + model.Taxval - Paid).Value, 2, MidpointRounding.ToEven);
            }
            else
            {
                decimal? TaxValue = model.DiscountAmountOrPercent == "%" ? ((SumCost - DiscountVal) * model.Tax / 100) : ((SumCost - DiscountVal) * model.Tax / 100);
                Net = model.DiscountAmountOrPercent == "%" ? Math.Round(((SumCost - DiscountVal) + TaxValue).Value, 2, MidpointRounding.ToEven) : Math.Round(((SumCost - DiscountVal) + TaxValue).Value, 2, MidpointRounding.ToEven);
                TotalDue = model.DiscountAmountOrPercent == "%" ? Math.Round(((SumCost - DiscountVal) + TaxValue - Paid).Value, 2, MidpointRounding.ToEven) : Math.Round(((SumCost - DiscountVal) + TaxValue - Paid).Value, 2, MidpointRounding.ToEven);
            }
        }

        [RelayCommand]
        async void SelecteNewItems(InvoiceModel model)
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
                    var popupView = new ScheduleItemsServicesViewModel(true);
                    popupView.ItemClose += async (item) =>
                    {
                        UserDialogs.Instance.ShowLoading();

                        InvoiceItemServicesModel InvoiceModel = new InvoiceItemServicesModel
                        {
                            AccountId = model.AccountId,
                            BrancheId = model.BrancheId,
                            ItemsServicesId = item.Id,
                            InvoiceId = model.Id,
                            ItemsServicesName = item.Name,
                            ItemServiceDescription = item.Description,
                            TaxId = item.TaxId,
                            Tax = item.Tax,
                            Price = item.CostperUnit,
                            Total = item.QTYTime != null && item.Tax != null ? (item.CostperUnit * item.QTYTime) + (item.CostperUnit * item.QTYTime * item.Tax / 100) : item.QTYTime == null && item.Tax != null ? item.CostperUnit + (item.CostperUnit * item.QTYTime * item.Tax / 100) : item.QTYTime != null && item.Tax == null ? item.CostperUnit * item.QTYTime : item.CostperUnit,
                            Active = item.Active,
                            CreateUser = model.CreateUser,
                            CreateDate = model.CreateDate,
                            Taxable = item.Taxable,
                            Quantity = (item.QTYTime == null || item.QTYTime == 0) ? 1 : item.QTYTime,
                            Unit = item.Unit,
                            SkipOfTotal = false,
                        };

                        if (LstItemsInvoice.Count > 0)
                        {
                            var itm = LstItemsInvoice.Where(x => x.ItemsServicesId == item.Id).FirstOrDefault();
                            if (itm == null)
                            {
                                LstItemsInvoice.Add(InvoiceModel);
                                OneInvoice.LstInvoiceItemServices.Add(InvoiceModel);
                            }
                        }
                        else
                        {
                            LstItemsInvoice.Add(InvoiceModel);
                            OneInvoice.LstInvoiceItemServices.Add(InvoiceModel);
                        }

                        if (LstItemsInvoice.Count > 4)
                        {
                            LstHeight = 1;
                        }

                        TotalInvoice(model);

                        UserDialogs.Instance.HideHud();
                    };

                    var page = new Pages.SchedulePages.NewItemsServicesSchedulePage();
                    page.BindingContext = popupView;
                    await App.Current!.MainPage!.Navigation.PushAsync(page);
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
        void RemoveItem(InvoiceItemServicesModel item)
        {
            IsBusy = true;

            LstItemsInvoice.Remove(item);
            OneInvoice.LstInvoiceItemServices.Remove(item);
            TotalInvoice(OneInvoice);

            IsBusy = false;
        }

        [RelayCommand]
        async void SelecteNewItemsEstimate(EstimateModel model)
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
                    var popupView = new ScheduleItemsServicesViewModel(true);
                    popupView.ItemClose += async (item) =>
                    {
                        UserDialogs.Instance.ShowLoading();

                        EstimateItemServicesModel EstimateModel = new EstimateItemServicesModel
                        {
                            AccountId = model.AccountId,
                            BrancheId = model.BrancheId,
                            ItemsServicesId = item.Id,
                            EstimateId = model.Id,
                            ItemsServicesName = item.Name,
                            TaxId = item.TaxId,
                            Tax = item.Tax,
                            Price = item.CostperUnit,
                            Total = item.QTYTime != null && item.Tax != null ? (item.CostperUnit * item.QTYTime) + (item.CostperUnit * item.QTYTime * item.Tax / 100) : item.QTYTime == null && item.Tax != null ? item.CostperUnit + (item.CostperUnit * item.QTYTime * item.Tax / 100) : item.QTYTime != null && item.Tax == null ? item.CostperUnit * item.QTYTime : item.CostperUnit,
                            Active = item.Active,
                            CreateUser = model.CreateUser,
                            CreateDate = model.CreateDate,
                            Taxable = item.Taxable,
                            Quantity = (item.QTYTime == null || item.QTYTime == 0) ? 1 : item.QTYTime,
                            Unit = item.Unit,
                        };

                        if (LstItemsEstimate.Count > 0)
                        {
                            var itm = LstItemsEstimate.Where(x => x.ItemsServicesId == item.Id).FirstOrDefault();
                            if (itm == null)
                            {
                                LstItemsEstimate.Add(EstimateModel);
                                OneEstimate.LstEstimateItemServices.Add(EstimateModel);
                            }
                        }
                        else
                        {
                            LstItemsEstimate.Add(EstimateModel);
                            OneEstimate.LstEstimateItemServices.Add(EstimateModel);
                        }

                        if (LstItemsEstimate.Count > 4)
                        {
                            LstHeight = 1;
                        }

                        TotalEstimate(model);

                        UserDialogs.Instance.HideHud();
                    };

                    var page = new Pages.SchedulePages.NewItemsServicesSchedulePage();
                    page.BindingContext = popupView;
                    await App.Current!.MainPage!.Navigation.PushAsync(page);
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
        void RemoveItemEstimate(EstimateItemServicesModel item)
        {
            IsBusy = true;

            LstItemsEstimate.Remove(item);
            OneEstimate.LstEstimateItemServices.Remove(item);
            TotalEstimate(OneEstimate);

            IsBusy = false;
        }

        [RelayCommand]
        async void CreateNewSchedule(CustomersModel model)
        {
            IsBusy = true;
            UserDialogs.Instance.ShowLoading();

            SchedulesViewModel ViewModel;
            if (Controls.StaticMembers.WayAfterChooseCust == 0)
            {
                ViewModel = new SchedulesViewModel(model);
                var page = new Pages.SchedulePages.NewSchedulePage();
                page.BindingContext = ViewModel;
                await App.Current!.MainPage!.Navigation.PushAsync(page);
            }

            UserDialogs.Instance.HideHud();
            IsBusy = false;
        }

        [RelayCommand]
        async void SelecteCustomerDetails(CustomersModel model)
        {
            IsBusy = true;
            UserDialogs.Instance.ShowLoading();
            var ViewModel = new CustomersViewModel(model);
            var page = new Pages.CustomerPages.CustomersDetailsPage();
            page.BindingContext = ViewModel;
            await App.Current!.MainPage!.Navigation.PushAsync(page);
            UserDialogs.Instance.HideHud();
            IsBusy = false;
        }

        [RelayCommand]
        public async void SelecteScheduleDetails(SchedulesModel model)
        {
            IsBusy = true;
            UserDialogs.Instance.ShowLoading();
            var ViewModel = new SchedulesViewModel(model.Id, model.ScheduleDateId);
            var page = new Pages.SchedulePages.ScheduleDetailsPage();
            page.BindingContext = ViewModel;
            await App.Current!.MainPage!.Navigation.PushAsync(page);
            UserDialogs.Instance.HideHud();
            IsBusy = false;
        }

        [RelayCommand]
        void EditDiscountForCustomerEstimate(CustomersModel model)
        {
            Discount = CustomerDetails.Discount = model.Discount;

            TotalEstimate(OneEstimate);
        }

        [RelayCommand]
        void EditDiscountForCustomer(CustomersModel model)
        {
            Discount = CustomerDetails.Discount = model.Discount;

            TotalInvoice(OneInvoice);
        }

        [RelayCommand]
        async void SelecteInvoiceDetails(InvoiceModel model)
        {
            IsBusy = true;
            UserDialogs.Instance.ShowLoading();
            var ViewModel = new CustomersViewModel(model, CustomerDetails);
            var page = new Pages.CustomerPages.InvoiceDetailsPage();
            page.BindingContext = ViewModel;
            await App.Current!.MainPage!.Navigation.PushAsync(page);
            UserDialogs.Instance.HideHud();
            IsBusy = false;
        }

        [RelayCommand]
        async void SelecteEstimateDetails(EstimateModel model)
        {
            IsBusy = true;
            UserDialogs.Instance.ShowLoading();
            var ViewModel = new CustomersViewModel(model, CustomerDetails);
            var page = new Pages.CustomerPages.EstimateDetailsPage();
            page.BindingContext = ViewModel;
            await App.Current!.MainPage!.Navigation.PushAsync(page);
            UserDialogs.Instance.HideHud();
            IsBusy = false;
        }

        [RelayCommand]
        async void SubmitInvoice(InvoiceModel model)
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
                    OneInvoice = model;

                    if (OneInvoice != null)
                    {
                        if (OneInvoice.LstInvoiceItemServices.Count > 0)
                        {
                            string UserToken = await _service.UserToken();

                            var CheckItemoutFalse = OneInvoice.LstInvoiceItemServices.Where(m => m.SkipOfTotal == false).FirstOrDefault();
                            OneInvoice.LstScdDate = LstInvoiceSchaduleDatesActual.ToList();

                            if (CheckItemoutFalse != null)
                            {
                                OneInvoice.Total = SubTotal;
                                OneInvoice.Net = Net;
                                OneInvoice.Discount = Discount;

                                var json = "";
                                if (OneInvoice.Id == 0)
                                {
                                    UserDialogs.Instance.ShowLoading();
                                    //json = await Helpers.Utility.PostMData("api/Invoices/PostInvoice", JsonConvert.SerializeObject(OneInvoice));
                                    json = await ORep.PostMData("api/Invoices/PostInvoice", OneInvoice, UserToken);
                                    UserDialogs.Instance.HideHud();
                                }
                                else
                                {
                                    UserDialogs.Instance.ShowLoading();
                                    //json = await Helpers.Utility.PutPosData("api/Invoices/PutInvoice", JsonConvert.SerializeObject(OneInvoice));
                                    json = await ORep.PutDataAsync("api/Invoices/PutInvoice", OneInvoice, UserToken);
                                    UserDialogs.Instance.HideHud();
                                }

                                if (json != "Bad Request" && json != "api not responding" && json.Contains("Not_Enough") != true && json.Contains("This Invoice Already Exist") != true && json.Contains("Already Exist For This Schedule Date#") != true)
                                {
                                    await App.Current!.MainPage!.DisplayAlert("FixPro", "Succes Save Invoice.", "Ok");
                                    var ViewModel = new SchedulesViewModel(OneInvoice, CustomerDetails);
                                    var page = new Pages.PopupPages.PaymentMethodsPopup();
                                    page.BindingContext = ViewModel;
                                    await MopupService.Instance.PushAsync(page);
                                    //await App.Current!.MainPage!.Navigation.PushAsync(new MainPage());
                                    //App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);
                                }
                                else
                                {
                                    await App.Current!.MainPage!.DisplayAlert("Alert", json, "Ok");
                                    //await App.Current!.MainPage!.Navigation.PushAsync(new MainPage());
                                }
                            }
                            else
                            {
                                await App.Current!.MainPage!.DisplayAlert("Alert", "Please Don't Check all Item-Service Out for this Invoice.", "Ok");
                            }
                        }
                        else
                        {
                            await App.Current!.MainPage!.DisplayAlert("Alert", "Please Choose Item-Service for this Invoice.", "Ok");
                        }
                    }
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
        async void SubmitEstimate(EstimateModel model)
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
                    OneEstimate = model;

                    if (OneEstimate != null)
                    {
                        string UserToken = await _service.UserToken();

                        OneEstimate.Status = Accept == true ? 1 : Declind == true ? 2 : 0; //0 = Pending
                        OneEstimate.LstScdDate = LstEstimateSchaduleDatesActual.ToList();

                        if (OneEstimate.LstEstimateItemServices.Count > 0)
                        {
                            OneEstimate.Total = SubTotal;
                            OneEstimate.Net = Net;
                            OneEstimate.Discount = Discount;
                            OneEstimate.SignatureDraw = SignatureImageByte64Estimate == null ? OneEstimate.SignatureDraw : SignatureImageByte64Estimate;

                            var json = "";
                            if (OneEstimate.Id == 0)
                            {
                                UserDialogs.Instance.ShowLoading();
                                //json = await Helpers.Utility.PostData("api/Estimates/PostEstimate", JsonConvert.SerializeObject(OneEstimate));
                                json = await ORep.PostDataAsync("api/Estimates/PostEstimate", OneEstimate, UserToken);
                                UserDialogs.Instance.HideHud();
                            }
                            else
                            {
                                UserDialogs.Instance.ShowLoading();
                                //json = await Helpers.Utility.PutPosData("api/Estimates/PutEstimate", JsonConvert.SerializeObject(OneEstimate));
                                json = await ORep.PutDataAsync("api/Estimates/PutEstimate", OneEstimate, UserToken);
                                UserDialogs.Instance.HideHud();
                            }

                            if (json != "Bad Request" && json != "api not responding" && json.Contains("Already Exist For This Schedule Date#") != true)
                            {
                                await App.Current!.MainPage!.DisplayAlert("FixPro", "Succes Save Estimate.", "Ok");

                                var ViewModel = new CustomersViewModel(CustomerDetails);
                                var page = new Pages.CustomerPages.CustomersDetailsPage();
                                page.BindingContext = ViewModel;
                                await App.Current!.MainPage!.Navigation.PushAsync(page);
                                App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);
                                App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);

                                if (OneEstimate.Status == 1)//Status Accept
                                {
                                    ShowEstimateConvertToInvoice = true;
                                }
                                else
                                {
                                    ShowEstimateConvertToInvoice = false;
                                }
                            }
                            else
                            {
                                await App.Current!.MainPage!.DisplayAlert("Alert", json, "Ok");
                                //await App.Current!.MainPage!.DisplayAlert("Alert", "Faild Save Estimate.", "Ok");
                            }
                        }
                        else
                        {
                            await App.Current!.MainPage!.DisplayAlert("Alert", "Please Choose Item-Service for this Estimate.", "Ok");
                        }
                    }
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
        async void GoInvoice(int InvoiceId)
        {
            IsBusy = true;
            await GetOneInvoiceDetails(InvoiceId, null);
            UserDialogs.Instance.ShowLoading();
            var ViewModel = new CustomersViewModel(OneInvoice, CustomerDetails);
            var page = new Pages.CustomerPages.InvoiceDetailsPage();
            page.BindingContext = ViewModel;
            await App.Current!.MainPage!.Navigation.PushAsync(page);
            UserDialogs.Instance.HideHud();
            IsBusy = false;
        }

        [RelayCommand]
        async void ConvertToInvoice(EstimateModel model)
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
                    await GetOneInvoiceDetails(null, model.Id);

                    string UserToken = await _service.UserToken();

                    if (OneEstimate != null)
                    {
                        OneInvoice = new InvoiceModel();
                        OneInvoice.LstInvoiceItemServices = new List<InvoiceItemServicesModel>();

                        OneInvoice.AccountId = OneEstimate.AccountId;
                        OneInvoice.BrancheId = OneEstimate.BrancheId;
                        //OneInvoice.ContractId = 0;
                        //OneInvoice.ContractInvoiceId = 0;
                        OneInvoice.ScheduleId = OneEstimate.ScheduleId;
                        OneInvoice.EstimateId = OneEstimate.Id;
                        OneInvoice.InvoiceDate = DateTime.Now;
                        OneInvoice.CustomerId = OneEstimate.CustomerId;
                        OneInvoice.Total = OneEstimate.Total;
                        OneInvoice.TaxId = OneEstimate.TaxId;
                        OneInvoice.Tax = OneEstimate.Tax;
                        OneInvoice.Taxval = OneEstimate.Taxval;
                        OneInvoice.MemberId = CustomerDetails.MemeberId;
                        OneInvoice.Discount = OneEstimate.Discount;
                        OneInvoice.DiscountAmountOrPercent = OneEstimate.DiscountAmountOrPercent;
                        OneInvoice.Paid = 0;
                        OneInvoice.Net = OneEstimate.Net;
                        OneInvoice.Status = 0; //Draft status if(1=partail & 2=paid)
                        OneInvoice.Type = 2; //Installment Payment type
                        OneInvoice.SignaturePrintName = null;
                        OneInvoice.SignatureDraw = null;
                        OneInvoice.Terms = null;
                        OneInvoice.NotesForCustomer = OneEstimate.NotesForCustomer;
                        OneInvoice.Notes = OneEstimate.Notes;
                        OneInvoice.Active = OneEstimate.Active;
                        OneInvoice.CreateUser = int.Parse(Helpers.Settings.UserIdGet);
                        OneInvoice.CreateDate = DateTime.Now;
                        OneInvoice.LstScdDate = OneEstimate.LstScdDate;

                        foreach (EstimateItemServicesModel item in OneEstimate.LstEstimateItemServices)
                        {
                            InvoiceItemServicesModel ObjItem = new InvoiceItemServicesModel
                            {
                                Id = item.Id,
                                AccountId = OneEstimate.AccountId,
                                BrancheId = OneEstimate.BrancheId,
                                //ItemServiceDescription = item.ItemServiceDescription,
                                TaxId = item.TaxId,
                                Tax = item.Tax,
                                //Taxable = item.Taxable,
                                Taxable = true,
                                Unit = item.Unit,
                                Price = item.Price,
                                Quantity = item.Quantity,
                                //Discountable = CustomerDetails.MemberDTO.MemberValue != null ? true : false,
                                Discountable = true,
                                ItemsServicesId = item.ItemsServicesId,
                                ItemsServicesName = item.ItemsServicesName,
                                CreateUser = int.Parse(Helpers.Settings.UserIdGet),
                                CreateDate = DateTime.Now,
                                SkipOfTotal = false,
                                Total = item.Quantity != null && item.Tax != null ? (item.Price * item.Quantity) + (item.Price * item.Quantity * item.Tax / 100) : item.Quantity == null && item.Tax != null ? item.Price + (item.Price * item.Quantity * item.Tax / 100) : item.Quantity != null && item.Tax == null ? item.Price * item.Quantity : item.Price,
                                Active = OneEstimate.Active,
                            };
                            OneInvoice.LstInvoiceItemServices.Add(ObjItem);
                        }

                        UserDialogs.Instance.ShowLoading();
                        //var json = await Helpers.Utility.PostMData("api/Invoices/PostInvoice", JsonConvert.SerializeObject(OneInvoice));
                        var json = await ORep.PostMData("api/Invoices/PostInvoice", OneInvoice, UserToken);
                        UserDialogs.Instance.HideHud();

                        if (json != null && json != "api not responding" && json.Contains("Not_Enough") != true && json.Contains("This Invoice Already Exist") != true)
                        {
                            await App.Current!.MainPage!.DisplayAlert("FixPro", "Succes Convert To Inovice.", "Ok");
                            OneInvoice.Id = int.Parse(json.Trim('"'));

                            var ViewModel = new CustomersViewModel(OneInvoice, CustomerDetails);
                            var page = new Pages.CustomerPages.InvoiceDetailsPage();
                            page.BindingContext = ViewModel;
                            await App.Current!.MainPage!.Navigation.PushAsync(page);
                            //await App.Current!.MainPage!.Navigation.PushAsync(new MainPage());
                            App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);
                            App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);
                        }
                        else
                        {
                            await App.Current!.MainPage!.DisplayAlert("Alert", json, "Ok");
                        }
                    }
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
        async Task CreateNewCustomer()
        {
            IsBusy = true;
            UserDialogs.Instance.ShowLoading();
            Controls.StaticMembers.WayCreateCust = 2; //From Schedule
            var ViewModel = new CustomersViewModel(true);
            var page = new Pages.CustomerPages.CreateNewCustomerPage();
            page.BindingContext = ViewModel;
            await App.Current!.MainPage!.Navigation.PushAsync(page);
            UserDialogs.Instance.HideHud();
            IsBusy = false;
        }

        [RelayCommand]
        void ChooseCustomerCategory(CustomersCategoryModel model)
        {
            CustomerDetails.CustomerCategory = model;
        }

        [RelayCommand]
        void ChooseCustomerMemberShip(MemberModel model)
        {
            CustomerDetails.MemberDTO = model;
        }

        [RelayCommand]
        void ChooseCustomerTax(TaxModel model)
        {
            CustomerDetails.TaxDTO = model;
        }

        [RelayCommand]
        void ChooseCustomerCampaign(CampaignModel model)
        {
            CustomerDetails.Source = model.Id;
            CustomerDetails.CampaignDTO = model;
        }

        [RelayCommand]
        async Task SelecteAddress()
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
                    var popupView = new Pages.PopupPages.AddressPupop();
                    popupView.DidClose += async (str) =>
                    {
                        IsBusy = false;
                        CustomerDetails.AddressModel = str;
                        Address = CustomerDetails.Address = str.FullAddress;
                        CustomerDetails.locationlatitude = str.Latitude.ToString();
                        CustomerDetails.locationlongitude = str.Longitude.ToString();
                        State = CustomerDetails.State = str.State;
                        City = CustomerDetails.City = str.City;
                        ZipCode = CustomerDetails.PostalcodeZIP = str.Zip;
                        CustomerDetails.Country = str.Country;

                        CustomersModel oCust = await Controls.StartData.GetAddressDetails(CustomerDetails);

                        HouseValue = (!string.IsNullOrEmpty(oCust.EstimedValue) && oCust.EstimedValue != "None") ? string.Format("${0:#,0.#}", float.Parse(oCust.EstimedValue)) : "None";
                        CustomerDetails.EstimedValue = HouseValue;

                        YearBuilt = (!string.IsNullOrEmpty(oCust.YearBuit) && oCust.YearBuit != "None") ? oCust.YearBuit : "None";
                        CustomerDetails.YearBuit = YearBuilt;

                        SquareFootage = (!string.IsNullOrEmpty(oCust.Squirefootage) && oCust.Squirefootage != "None") ? oCust.Squirefootage : "None";
                        CustomerDetails.Squirefootage = SquareFootage;

                        CustomerDetails.YearEstimedValue = (!string.IsNullOrEmpty(oCust.YearEstimedValue) && oCust.YearEstimedValue != "None") ? oCust.YearEstimedValue : "None";
                        IsBusy = true;
                    };

                    await MopupService.Instance.PushAsync(popupView);
                }
            }
            catch (Exception ex)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
            }

            IsBusy = false;
        }

        [RelayCommand]
        async Task SelectCustToCreateEstimatePage()
        {
            IsBusy = true;
            UserDialogs.Instance.ShowLoading();
            Controls.StaticMembers.WayAfterChooseCust = 1; //Create Estimate 
            var ViewModel = new SchedulesViewModel(CustomerDetails);
            var page = new Pages.CreateEstimateWithoutSchedulePage();
            page.BindingContext = ViewModel;
            await App.Current!.MainPage!.Navigation.PushAsync(page);
            UserDialogs.Instance.HideHud();
            IsBusy = false;
        }

        [RelayCommand]
        async Task SelectCustToCreateInvoicePage()
        {
            IsBusy = true;
            UserDialogs.Instance.ShowLoading();
            Controls.StaticMembers.WayAfterChooseCust = 2; //Create Invoice 
            var ViewModel = new SchedulesViewModel(CustomerDetails);
            var page = new Pages.CreateInvoiceWithoutSchedulePage();
            page.BindingContext = ViewModel;
            await App.Current!.MainPage!.Navigation.PushAsync(page);
            UserDialogs.Instance.HideHud();
            IsBusy = false;
        }

        [RelayCommand]
        async Task InsertCustomer(CustomersModel model)
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
                    if (string.IsNullOrEmpty(model.FirstName))
                    {
                        await App.Current!.MainPage!.DisplayAlert("Alert", $"Please Complete This Field Required : First Name.", "Ok");
                    }
                    else if (string.IsNullOrEmpty(model.LastName))
                    {
                        await App.Current!.MainPage!.DisplayAlert("Alert", $"Please Complete This Field Required : Last Name.", "Ok");
                    }
                    else if (model.CustomerCategory == null)
                    {
                        await App.Current!.MainPage!.DisplayAlert("Alert", $"Please Complete This Field Required : Customer Category.", "Ok");
                    }
                    else if (model.Source == null)
                    {
                        await App.Current!.MainPage!.DisplayAlert("Alert", $"Please Complete This Field Required : Source.", "Ok");
                    }
                    else if (string.IsNullOrEmpty(model.Phone1))
                    {
                        await App.Current!.MainPage!.DisplayAlert("Alert", $"Please Complete This Field Required : Customer Phone.", "Ok");
                    }
                    //else if (string.IsNullOrEmpty(model.Email))
                    //{
                    //    await App.Current!.MainPage!.DisplayAlert("Alert", $"Please Complete This Field Required : Customer Email.", "Ok");
                    //}
                    else if (string.IsNullOrEmpty(model.Address))
                    {
                        await App.Current!.MainPage!.DisplayAlert("Alert", $"Please Complete This Field Required : Customer Address.", "Ok");
                    }
                    else
                    {
                        string UserToken = await _service.UserToken();

                        model.AccountId = int.Parse(Helpers.Settings.AccountIdGet);
                        model.BrancheId = int.Parse(Helpers.Settings.BranchIdGet);
                        model.CreateUser = int.Parse(Helpers.Settings.UserIdGet);
                        model.Phone1 = model.Phone1.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Trim();
                        model.CustomerType = 1;
                        model.AllowLogin = false;
                        model.Credit = 0;
                        model.Since = DateTime.Now;
                        model.Active = true;
                        model.Source = OneCampaign.Id;
                        model.CreateDate = DateTime.Now;
                        model.State = CustomerDetails.State != null ? CustomerDetails.State : State;
                        model.City = CustomerDetails.City != null ? CustomerDetails.City : City;
                        model.PostalcodeZIP = CustomerDetails.PostalcodeZIP != null ? CustomerDetails.PostalcodeZIP : ZipCode;

                        if (IsUpdateCust != 2) // Create New Customer
                        {
                            model.LstCustomersCustomField = new List<CustomersCustomFieldModel>();

                            foreach (CustomersCustomFieldModel item in CustomerFeatures.LstCustomersCustomField.ToList())
                            {
                                if (item.Required == false || string.IsNullOrEmpty(item.DefaultValue?.Trim()) != true)
                                {
                                    if (item.FieldType == 4 && item.DefaultValue == "True")
                                    {
                                        item.DefaultValue = "Yes";
                                    }
                                    else if (item.FieldType == 4 && item.DefaultValue == "False")
                                    {
                                        item.DefaultValue = "No";
                                    }
                                    else if (item.FieldType == 3)
                                    {
                                        item.DefaultValue = DateTime.Parse(item.DefaultValue).ToString("yyyy-MM-dd");
                                    }

                                    model.LstCustomersCustomField.Add(item);
                                }
                                else
                                {
                                    await App.Current!.MainPage!.DisplayAlert("Alert", $"Please Complete This Field : {item.CustomFieldName} Required For Custom Field.", "Ok");
                                    return;
                                }
                            }
                        }

                        //model.LstCustomersCustomField = CustomerFeatures.LstCustomersCustomField;

                        if (CustomerDetails.MemberDTO != null)
                        {
                            model.MemeberType = CustomerDetails.MemberDTO.MemberType;
                            model.MemeberId = CustomerDetails.MemberDTO.Id;
                        }
                        else
                        {
                            model.MemeberType = false;
                        }

                        if (CustomerDetails.TaxDTO != null)
                            model.TaxId = CustomerDetails.TaxDTO.Id;

                        model.Taxable = CustomerDetails.Taxable == null ? false : CustomerDetails.Taxable;

                        if (CustomerDetails.CustomerCategory != null)
                            model.CategoryId = CustomerDetails.CustomerCategory.Id;

                        //var json = await Helpers.Utility.PostData("api/Customers/PostCustomer", JsonConvert.SerializeObject(model));
                        //var json = await ORep.PostDataAsync("api/Customers/PostCustomer", model, UserToken);
                        var json = "";
                        if (model.Id == 0)
                        {
                            UserDialogs.Instance.ShowLoading();
                            json = await ORep.PostDataAsync("api/Customers/PostCustomer", model, UserToken);
                            UserDialogs.Instance.HideHud();
                        }
                        else
                        {
                            UserDialogs.Instance.ShowLoading();
                            json = await ORep.PutDataAsync("api/Customers/PutCustomer", model, UserToken);
                            UserDialogs.Instance.HideHud();
                        }
                        if (json != null && json != "api not responding" && json != "Multiple Choices")
                        {
                            CustomersModel Customer = JsonConvert.DeserializeObject<CustomersModel>(json);

                            if (Controls.StaticMembers.WayCreateCust == 1)//From CallPage
                            {
                                var ViewModel = new CallsViewModel(Customer);
                                var page = new Pages.CallPages.NewCallPage();
                                page.BindingContext = ViewModel;
                                await App.Current!.MainPage!.Navigation.PushAsync(page);
                                App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);
                                App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);
                                await MopupService.Instance.PopAsync();
                            }
                            else if (Controls.StaticMembers.WayCreateCust == 2) //From Schedule create new customer
                            {
                                var ViewModel = new SchedulesViewModel(Customer);
                                var page = new Pages.SchedulePages.NewSchedulePage();
                                page.BindingContext = ViewModel;
                                await App.Current!.MainPage!.Navigation.PushAsync(page);
                                App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);
                                App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);
                            }
                            else if (Controls.StaticMembers.WayCreateCust == 3) //From Schedule can edit customer and return schedule again
                            {
                                var ViewModel = new SchedulesViewModel(Controls.StaticMembers.ScheduleIdStatic, Controls.StaticMembers.ScheduleDateIdStatic);
                                var page = new Pages.SchedulePages.ScheduleDetailsPage();
                                page.BindingContext = ViewModel;
                                await App.Current!.MainPage!.Navigation.PushAsync(page);
                                App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);
                                App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);
                            }
                            else
                            {
                                if (model.Id == 0) // new Customer
                                {
                                    await App.Current!.MainPage!.DisplayAlert("FixPro", "Success Insert Customer.", "Ok");
                                    await App.Current!.MainPage!.Navigation.PushAsync(new MainPage());
                                }
                                else // Update Customer
                                {
                                    await App.Current!.MainPage!.DisplayAlert("FixPro", "Success Update Customer.", "Ok");
                                    var VM = new CustomersViewModel();
                                    var page = new Pages.CustomerPages.CustomersPage();
                                    page.BindingContext = VM;
                                    await App.Current!.MainPage!.Navigation.PushAsync(page);
                                    App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);
                                    App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);

                                }
                            }
                        }
                        else if (json == "Multiple Choices")
                        {
                            await App.Current!.MainPage!.DisplayAlert("FixPro", "This Customer phone already exists.", "Ok");
                        }
                        else
                        {
                            if (model.Id == 0)
                            {
                                await App.Current!.MainPage!.DisplayAlert("FixPro", "Field Insert Customer.", "Ok");
                            }
                            else
                            {
                                await App.Current!.MainPage!.DisplayAlert("FixPro", "Field Update Customer.", "Ok");
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
                //throw;
            }

            IsBusy = false;
        }

        [RelayCommand]
        async void CreditPayment(InvoiceModel model)
        {
            IsBusy = true;
            UserDialogs.Instance.ShowLoading();
            Controls.StaticMembers.PayCashOrCredit = 2;
            var ViewModel = new PaymentsViewModel(model, CustomerDetails);
            var page = new Pages.CustomerPages.CashOrCreditPaymentPage();
            page.BindingContext = ViewModel;
            await App.Current!.MainPage!.Navigation.PushAsync(page);
            UserDialogs.Instance.HideHud();
            IsBusy = false;
        }

        [RelayCommand]
        async void CashPayment(InvoiceModel model)
        {
            IsBusy = true;
            UserDialogs.Instance.ShowLoading();
            Controls.StaticMembers.PayCashOrCredit = 1;
            var ViewModel = new PaymentsViewModel(model, CustomerDetails);
            var page = new Pages.CustomerPages.CashOrCreditPaymentPage();
            page.BindingContext = ViewModel;
            await App.Current!.MainPage!.Navigation.PushAsync(page);
            UserDialogs.Instance.HideHud();
            IsBusy = false;
        }

        [RelayCommand]
        async void DeleteEstimate(int EstId)
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
                    string UserToken = await _service.UserToken();

                    var json = await ORep.DeleteStrItemAsync(string.Format("api/Estimates/DeleteEstimate/{0}", EstId), UserToken);

                    if (json != null && json != "api not responding" && json.Contains("This Estimate Can`t Deleted") != true)
                    {
                        await App.Current!.MainPage!.DisplayAlert("FixPro", "Success Delete Estimate.", "Ok");
                        //await App.Current!.MainPage!.Navigation.PushAsync(new MainPage());

                        var ViewModel = new CustomersViewModel(CustomerDetails);
                        var page = new Pages.CustomerPages.CustomersDetailsPage();
                        page.BindingContext = ViewModel;
                        await App.Current!.MainPage!.Navigation.PushAsync(page);
                        App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);

                    }
                    else
                    {
                        await App.Current!.MainPage!.DisplayAlert("Alert", json, "Ok");
                        //await App.Current!.MainPage!.DisplayAlert("Alert", "Failed Delete Inovice.", "Ok");
                    }
                    UserDialogs.Instance.HideHud();
                }
            }
            catch (Exception)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", "Failed for Deleted", "OK");
                //throw;
            }

            IsBusy = false;
        }

        [RelayCommand]
        async void DeleteInvoice(int InvoiceId)
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
                    string UserToken = await _service.UserToken();

                    var json = await ORep.DeleteStrItemAsync(string.Format("api/Invoices/DeleteInvoice/{0}", InvoiceId), UserToken);

                    if (json != null && json != "api not responding" && json.Contains("Is Not Deleted") != true)
                    {
                        await App.Current!.MainPage!.DisplayAlert("FixPro", "Success Delete Inovice.", "Ok");
                        //await App.Current!.MainPage!.Navigation.PushAsync(new MainPage());
                        var ViewModel = new CustomersViewModel(CustomerDetails);
                        var page = new Pages.CustomerPages.CustomersDetailsPage();
                        page.BindingContext = ViewModel;
                        await App.Current!.MainPage!.Navigation.PushAsync(page);
                        App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);
                    }
                    else
                    {
                        await App.Current!.MainPage!.DisplayAlert("Alert", json, "Ok");
                        //await App.Current!.MainPage!.DisplayAlert("Alert", "Failed Delete Inovice.", "Ok");
                    }
                    UserDialogs.Instance.HideHud();
                }
            }
            catch (Exception)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", "Failed for Deleted", "OK");
                //throw;
            }

            IsBusy = false;
        }

        [RelayCommand]
        async void OpenEstimateScheduleDates()
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
                    foreach (SchaduleDateModel dt in LstEstimateSchaduleDates)
                    {
                        foreach (SchaduleDateModel dt2 in LstEstimateSchaduleDatesActual)
                        {
                            if (dt.Id == dt2.Id)
                            {
                                dt.IsChecked = true;
                            }
                        }
                    }

                    var popupView = new Pages.PopupPages.ScheduleDatesPopup(LstEstimateSchaduleDates);
                    popupView.DatesClose += (Dates) =>
                    {

                        if (Dates.Count != 0)
                        {
                            LstEstimateSchaduleDatesActual = new ObservableCollection<SchaduleDateModel>(Dates);
                            string str = "";
                            LstEstimateDates.Clear();
                            foreach (var Date in Dates)
                            {
                                str += (" , " + Date.Date);
                                LstEstimateDates.Add(new SchaduleDateModel
                                {
                                    Id = Date.Id,
                                    Date = Date.Date,
                                });
                            }

                            if (!string.IsNullOrEmpty(StrEstimateDates))
                            {
                                StrEstimateDates = string.Empty;
                                StrEstimateDates += str;
                                StrEstimateDates = str.Remove(0, 2);
                            }
                            else
                            {
                                StrEstimateDates = str.Remove(0, 2);
                            }
                        }
                    };

                    await MopupService.Instance.PushAsync(popupView);
                }
            }
            catch (Exception ex)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
                //throw;
            }

            IsBusy = false;
        }

        [RelayCommand]
        async void OpenInvoiceScheduleDates()
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
                    foreach (SchaduleDateModel dt in LstInvoiceSchaduleDates)
                    {
                        foreach (SchaduleDateModel dt2 in LstInvoiceSchaduleDatesActual)
                        {
                            if (dt.Id == dt2.Id)
                            {
                                dt.IsChecked = true;
                            }
                        }
                    }

                    var popupView = new Pages.PopupPages.ScheduleDatesPopup(LstInvoiceSchaduleDates);
                    popupView.DatesClose += (Dates) =>
                    {

                        if (Dates.Count != 0)
                        {
                            LstInvoiceSchaduleDatesActual = new ObservableCollection<SchaduleDateModel>(Dates);

                            string str = "";
                            LstInvoiceDates.Clear();
                            foreach (var Date in Dates)
                            {
                                str += (" , " + Date.Date);
                                LstInvoiceDates.Add(new SchaduleDateModel
                                {
                                    Id = Date.Id,
                                    Date = Date.Date,
                                });
                            }

                            if (!string.IsNullOrEmpty(StrInvoiceDates))
                            {
                                StrInvoiceDates = string.Empty;
                                StrInvoiceDates += str;
                                StrInvoiceDates = str.Remove(0, 2);
                            }
                            else
                            {
                                StrInvoiceDates = str.Remove(0, 2);
                            }
                        }
                    };

                    await MopupService.Instance.PushAsync(popupView);
                }
            }
            catch (Exception ex)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
                //throw;
            }

            IsBusy = false;
        }

        [RelayCommand]
        void RemoveEstimateDate(SchaduleDateModel Date)
        {
            LstEstimateDates.Remove(Date);
            SchaduleDateModel DataOfScddt = LstEstimateSchaduleDatesActual.Where(x => x.Id == Date.Id).FirstOrDefault();
            LstEstimateSchaduleDatesActual.Remove(DataOfScddt);

            foreach (SchaduleDateModel dt in LstEstimateSchaduleDates)
            {
                foreach (SchaduleDateModel dt2 in LstEstimateSchaduleDatesActual)
                {
                    if (dt.Id == dt2.Id)
                    {
                        dt.IsChecked = true;
                    }
                }
            }

            //Dates Names 
            int index = StrEstimateDates.IndexOf(Date.Date + " , ");
            StrEstimateDates = (index < 0) ? StrEstimateDates : StrEstimateDates.Remove(index, (Date.Date + " , ").Length);

            int index2 = StrEstimateDates.IndexOf(" , " + Date.Date);
            StrEstimateDates = (index2 < 0) ? StrEstimateDates : StrEstimateDates.Remove(index2, (" , " + Date.Date).Length);

            int index3 = StrEstimateDates.IndexOf(Date.Date);
            StrEstimateDates = (index3 < 0) ? StrEstimateDates : StrEstimateDates.Remove(index3, (Date.Date).Length);
        }

        [RelayCommand]
        void RemoveInvoiceDate(SchaduleDateModel Date)
        {
            LstInvoiceDates.Remove(Date);
            SchaduleDateModel DataOfScddt = LstInvoiceSchaduleDatesActual.Where(x => x.Id == Date.Id).FirstOrDefault();
            LstInvoiceSchaduleDatesActual.Remove(DataOfScddt);

            foreach (SchaduleDateModel dt in LstInvoiceSchaduleDates)
            {
                foreach (SchaduleDateModel dt2 in LstInvoiceSchaduleDatesActual)
                {
                    if (dt.Id == dt2.Id)
                    {
                        dt.IsChecked = true;
                    }
                }
            }

            //Dates Names 
            int index = StrInvoiceDates.IndexOf(Date.Date + " , ");
            StrInvoiceDates = (index < 0) ? StrInvoiceDates : StrInvoiceDates.Remove(index, (Date.Date + " , ").Length);

            int index2 = StrInvoiceDates.IndexOf(" , " + Date.Date);
            StrInvoiceDates = (index2 < 0) ? StrInvoiceDates : StrInvoiceDates.Remove(index2, (" , " + Date.Date).Length);

            int index3 = StrInvoiceDates.IndexOf(Date.Date);
            StrInvoiceDates = (index3 < 0) ? StrInvoiceDates : StrInvoiceDates.Remove(index3, (Date.Date).Length);
        }

        [RelayCommand]
        async void SelectSendEmailInvoice(InvoiceModel model)
        {
            IsBusy = true;
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
                //return;
            }
            else
            {
                string UserToken = await _service.UserToken();

                UserDialogs.Instance.ShowLoading();
                var json = await ORep.PostStrAsync("api/Invoices/PostInvoiceEmail", model, UserToken);
                UserDialogs.Instance.HideHud();

                if (!string.IsNullOrEmpty(json) && json.Contains("Send Success") == true)
                {
                    await App.Current!.MainPage!.DisplayAlert("FixPro", "Succes Send Email to Customer.", "Ok");
                }
                else
                {
                    await App.Current!.MainPage!.DisplayAlert("FixPro", "Field Send Email to Customer.", "Ok");
                }

            }

            IsBusy = false;
        }

        [RelayCommand]
        async void SelectSendEmailEstimate(EstimateModel model)
        {
            IsBusy = true;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
                //return;
            }
            else
            {
                string UserToken = await _service.UserToken();

                UserDialogs.Instance.ShowLoading();
                var json = await ORep.PostStrAsync("api/Estimates/PostEstimateEmail", model, UserToken);
                UserDialogs.Instance.HideHud();

                if (!string.IsNullOrEmpty(json) && json.Contains("Send Success") == true)
                {
                    await App.Current!.MainPage!.DisplayAlert("FixPro", "Succes Send Email to Customer.", "Ok");
                }
                else
                {
                    await App.Current!.MainPage!.DisplayAlert("FixPro", "Field Send Email to Customer.", "Ok");
                }

            }

            IsBusy = false;
        }

        [RelayCommand]
        async void UpdateCustomer(CustomersModel model)
        {
            IsBusy = true;
            UserDialogs.Instance.ShowLoading();
            var ViewModel = new CustomersViewModel(model, 2); // update Cust
            var page = new Pages.CustomerPages.CreateNewCustomerPage();
            page.BindingContext = ViewModel;
            await App.Current!.MainPage!.Navigation.PushAsync(page);
            UserDialogs.Instance.HideHud();
            IsBusy = false;
        }

    }
}
