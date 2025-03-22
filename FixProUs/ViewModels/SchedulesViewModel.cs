
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Runtime.InteropServices;
using Stripe;
using GoogleApi.Entities.Translate.Common.Enums;
using System.IO;
using GoogleApi.Entities.Search.Video.Common;
using Splat.ModeDetection;
using Twilio.Types;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using FixProUs.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Controls.UserDialogs.Maui;
using Mopups.Services;
using FixProUs.Pages;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Graphics.Platform;
using Syncfusion.Maui.Calendar;
using FixProUs.Pages.PopupPages;
using CommunityToolkit.Maui.Alerts;
using SkiaSharp;
using FixProUs.Pages.SchedulePages;


namespace FixProUs.ViewModels
{
    public partial class SchedulesViewModel : BaseViewModel
    {

        readonly Services.Data.ServicesService _service = new Services.Data.ServicesService();

        [ObservableProperty]
        ObservableCollection<SchedulesModel> lstSchedules;

        [ObservableProperty]
        ObservableCollection<SchedulesModel> calendarDataToday;

        [ObservableProperty]
        ObservableCollection<SchedulesModel> lstSchedulesSearch;

        [ObservableProperty]
        ObservableCollection<ScheduleItemsServicesModel> lstItems;

        [ObservableProperty]
        ObservableCollection<ScheduleItemsServicesModel> lstItemsInvoice;

        [ObservableProperty]
        ObservableCollection<ScheduleItemsServicesModel> lstFreeServices;

        [ObservableProperty]
        ObservableCollection<ScheduleMaterialReceiptModel> lstMaterialReceipt;

        [ObservableProperty]
        ObservableCollection<ScheduleItemsServicesModel> lstItemsEstimate;

        [ObservableProperty]
        ObservableCollection<SheetColorModel> lstColors;

        [ObservableProperty]
        ObservableCollection<EmployeesCategoryModel> lstEmpCategory;

        [ObservableProperty]
        ObservableCollection<EmployeeModel> lstEmpInOneCategory;

        [ObservableProperty]
        SchedulePicturesModel onePictureModel;

        [ObservableProperty]
        ObservableCollection<SchedulePicturesModel> lstAllPictures;

        [ObservableProperty]
        ObservableCollection<SchedulePicturesModel> lstATwoPictures;

        [ObservableProperty]
        ObservableCollection<SchedulePicturesModel> lstNewPictures;

        [ObservableProperty]
        ObservableCollection<PriorityModel> lstPriority;

        [ObservableProperty]
        ObservableCollection<ScheduleEmployeesModel> lstEmps;

        [ObservableProperty]
        ObservableCollection<SchaduleDateModel> lstEstimateDates;

        [ObservableProperty]
        ObservableCollection<SchaduleDateModel> lstInvoiceDates;

        [ObservableProperty]
        ObservableCollection<ItemsServicesModel> lstServices;

        [ObservableProperty]
        ObservableCollection<SchaduleDateModel> lstEstimateSchaduleDates;

        [ObservableProperty]
        ObservableCollection<SchaduleDateModel> lstEstimateSchaduleDatesActual;

        [ObservableProperty]
        ObservableCollection<SchaduleDateModel> lstInvoiceSchaduleDates;

        [ObservableProperty]
        ObservableCollection<SchaduleDateModel> lstInvoiceSchaduleDatesActual;

        
        //CalendarEventCollection lstScheduleEvevts;


        [ObservableProperty]
        EmployeeModel oneEmployee;

        [ObservableProperty]
        InvoiceModel oneInvoice;

        [ObservableProperty]
        EstimateModel oneEstimate;

        [ObservableProperty]
        EmployeesCategoryModel empCategory;

        [ObservableProperty]
        SchedulesModel scheduleDetails;

        [ObservableProperty]
        CustomersModel customerDetails;

        [ObservableProperty]
        EmployeesCategoryModel selectedCateory;

        [ObservableProperty]
        EmployeeModel selectedEmployeeAddDate;

        [ObservableProperty]
        ItemsServicesModel selectedService;

        [ObservableProperty]
        SchaduleDateModel oneScheduleDate;

        [ObservableProperty]
        EmployeeModel employeePermission;

        [ObservableProperty]
        PriorityModel onePriorityModel;

        [ObservableProperty]
        Object selectedColor;

        [ObservableProperty]
        string branchName;

        [ObservableProperty]
        string strEmployees;

        [ObservableProperty]
        string strEstimateDates;

        [ObservableProperty]
        string strInvoiceDates;

        [ObservableProperty]
        string strEmployeesId;

        [ObservableProperty]
        int lstHeight;

        [ObservableProperty]
        DateTime scheduleDate;

        [ObservableProperty]
        DateTime scheduleAddDate;

        [ObservableProperty]
        DateTime invoiceDate;

        [ObservableProperty]
        TimeSpan timeFrom;

        [ObservableProperty]
        TimeSpan timeTo;

        [ObservableProperty]
        TimeSpan timeFromAddDate;

        [ObservableProperty]
        TimeSpan timeToAddDate;

        [ObservableProperty]
        string startTime;

        [ObservableProperty]
        string endTime;

        [ObservableProperty]
        string spentHours;

        [ObservableProperty]
        string spentMin;

        [ObservableProperty]
        ImageSource schedulePhoto;

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
        decimal? subTotalEst;

        [ObservableProperty]
        decimal? netEst;

        [ObservableProperty]
        decimal? paidEst;

        [ObservableProperty]
        decimal? totalDueEst;

        [ObservableProperty]
        bool doneFlag;

        [ObservableProperty]
        bool showImages;

        [ObservableProperty]
        int showDispatch;

        [ObservableProperty]
        bool showEstimateButton;

        [ObservableProperty]
        bool showQty; //Don't Show Qty in Schedule items but Show Qty in Estimate items and Invoice items

        [ObservableProperty]
        bool isReOpen;

        [ObservableProperty]
        bool showEstimateConvertToInvoice;

        [ObservableProperty]
        bool isShowSearchByItem;

        [ObservableProperty]
        int schedulePage;

        [ObservableProperty]
        bool pending;

        [ObservableProperty]
        bool accept;

        [ObservableProperty]
        bool declind;

        [ObservableProperty]
        string oldResonNotServiced;

        [ObservableProperty]
        bool amountOrPersent;

        [ObservableProperty]
        bool _IsShowAllSchedule;

        [ObservableProperty]
        bool isShowScheduleDates;

        [ObservableProperty]
        int photosCount;

        [ObservableProperty]
        string signatureImageByte64Estimate;

        [ObservableProperty]
        ObservableCollection<SchedulesModel> groupedList;

        public int BranchIdVM;

        Helpers.GenericRepository ORep = new Helpers.GenericRepository();

        public bool GetPictures { get; set; } = false;


        public SchedulesViewModel()
        {
            BranchIdVM = Helpers.Settings.UserRoleGet == "4" ? int.Parse(Helpers.Settings.AccountIdGet) : int.Parse(Helpers.Settings.BranchIdGet);

            CustomerDetails = new CustomersModel();
            ScheduleDetails = new SchedulesModel();
            EmpCategory = new EmployeesCategoryModel();
            OneEmployee = new EmployeeModel();
            LstEmpInOneCategory = new ObservableCollection<EmployeeModel>();
            LstSchedules = new ObservableCollection<SchedulesModel>();
            ScheduleDetails.LstScheduleItemsServices = new List<ScheduleItemsServicesModel>();
            ScheduleDetails.LstFreeServices = new List<ScheduleItemsServicesModel>();
            CustomerDetails.LstCustItemsServices = new List<ScheduleItemsServicesModel>();
            ScheduleDetails.LstMaterialReceipt = new List<ScheduleMaterialReceiptModel>();
            LstItemsInvoice = new ObservableCollection<ScheduleItemsServicesModel>();
            OnePictureModel = new SchedulePicturesModel();
            OnePriorityModel = new PriorityModel();
            LstServices = new ObservableCollection<ItemsServicesModel>();
            SelectedService = new ItemsServicesModel();
            LstATwoPictures = new ObservableCollection<SchedulePicturesModel>();
            SelectedEmployeeAddDate = new EmployeeModel();

            LstItems = new ObservableCollection<ScheduleItemsServicesModel>();
            LstMaterialReceipt = new ObservableCollection<ScheduleMaterialReceiptModel>();
            LstColors = new ObservableCollection<SheetColorModel>();
            LstEmpCategory = new ObservableCollection<EmployeesCategoryModel>();
            CalendarDataToday = new ObservableCollection<SchedulesModel>();
            GroupedList = new ObservableCollection<SchedulesModel>();

            GetPerrmission();
            GetAllSchedules();
        }

        public SchedulesViewModel(CustomersModel model)
        {
            ShowQty = false; //New Schedule
            if (Controls.StaticMembers.WayAfterChooseCust == 1 || Controls.StaticMembers.WayAfterChooseCust == 2)
            {
                ShowQty = true; //New Estimate Or Invoice
            }
            SchedulePage = 0; //New Schedule
            ScheduleDetails = new SchedulesModel();

            GetPerrmission();

            Init();

            //StrEmployees = "Choose Employees";
            ScheduleDate = DateTime.Now;

            InvoiceDate = DateTime.Now;
            BranchName = Helpers.Settings.BranchNameGet;

            //Chech the year now because change value for House details
            CheckHouseDataCust(model);


            CustomerDetails = model;

            AmountOrPersent = CustomerDetails.MemeberType == false ? false : CustomerDetails.MemberDTO == null ? false : CustomerDetails.MemberDTO.MemberType == true ? true : false;

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
        }

        //One Schedule Details Or From CallViewModel
        public SchedulesViewModel(int SchedulId, int ScheduleDateId)
        {
            ShowQty = false; //Old Schedule 
            IsShowAllSchedule = true; // Show all schedule details
            Init();

            GetOneScheduleDetails(SchedulId, ScheduleDateId);
        }

        //Schedule Update
        public SchedulesViewModel(SchedulesModel model)
        {
            //&& model.Recurring == false //old Check
            IsReOpen = ((String.IsNullOrEmpty(model.OneScheduleDate.StartTime) == true && model.OneScheduleDate.Status == 0)) ? true : false; //Show ReOpen Button If Don't Start Job after NotServiced only
            OldResonNotServiced = model.OneScheduleDate.Reasonnotserve;
            ShowQty = true; //Invoice Page
            GetPerrmission();

            if (model.LstScheduleItemsServices.Count > 4)
            {
                LstHeight = 1;
            }

            InitData(model);

            IsShowScheduleDates = true; //Show all Schedules Dates
            GetScheduleDates(model.Id, 1); //All Schedule Dates

            ScheduleAddDate = DateTime.Now;
        }

        public SchedulesViewModel(InvoiceModel model, CustomersModel Cust)
        {
            GetPerrmission();

            OneInvoice = model;
            CustomerDetails = Cust;
        }

        async void Init()
        {
            CustomerDetails = new CustomersModel();
            ScheduleDetails = new SchedulesModel();
            EmpCategory = new EmployeesCategoryModel();
            OneEmployee = new EmployeeModel();
            LstItems = new ObservableCollection<ScheduleItemsServicesModel>();
            LstFreeServices = new ObservableCollection<ScheduleItemsServicesModel>();
            LstMaterialReceipt = new ObservableCollection<ScheduleMaterialReceiptModel>();
            LstItemsEstimate = new ObservableCollection<ScheduleItemsServicesModel>();
            LstItemsInvoice = new ObservableCollection<ScheduleItemsServicesModel>();
            ScheduleDetails.LstScheduleItemsServices = new List<ScheduleItemsServicesModel>();
            ScheduleDetails.LstFreeServices = new List<ScheduleItemsServicesModel>();
            ScheduleDetails.LstScheduleEmployeeDTO = new List<ScheduleEmployeesModel>();
            CustomerDetails.LstCustItemsServices = new List<ScheduleItemsServicesModel>();
            LstColors = new ObservableCollection<SheetColorModel>();
            LstEmpCategory = new ObservableCollection<EmployeesCategoryModel>();
            LstEmpInOneCategory = new ObservableCollection<EmployeeModel>();
            OneInvoice = new InvoiceModel();
            OneInvoice.LstInvoiceItemServices = new List<InvoiceItemServicesModel>();
            OneEstimate = new EstimateModel();
            OneEstimate.LstEstimateItemServices = new List<EstimateItemServicesModel>();
            LstPriority = new ObservableCollection<PriorityModel>();
            LstEmps = new ObservableCollection<ScheduleEmployeesModel>();
            LstServices = new ObservableCollection<ItemsServicesModel>();
            SelectedService = new ItemsServicesModel();
            LstATwoPictures = new ObservableCollection<SchedulePicturesModel>();
            LstEstimateSchaduleDates = new ObservableCollection<SchaduleDateModel>();
            LstEstimateSchaduleDatesActual = new ObservableCollection<SchaduleDateModel>();
            LstInvoiceSchaduleDates = new ObservableCollection<SchaduleDateModel>();
            LstInvoiceSchaduleDatesActual = new ObservableCollection<SchaduleDateModel>();
            LstInvoiceDates = new ObservableCollection<SchaduleDateModel>();
            LstEstimateDates = new ObservableCollection<SchaduleDateModel>();
            
            //Schdule Date
            ScheduleDetails.OneScheduleDate = new SchaduleDateModel();
            OneScheduleDate = new SchaduleDateModel();

            OnePictureModel = new SchedulePicturesModel();
            BranchIdVM = int.Parse(Helpers.Settings.BranchIdGet);


            LstColors.Add(new SheetColorModel() { ColorName = "Red", ColorHex = "#eb4034" });
            LstColors.Add(new SheetColorModel() { ColorName = "Blue", ColorHex = "#2f6fde" });
            LstColors.Add(new SheetColorModel() { ColorName = "Green", ColorHex = "#23b007" });
            LstColors.Add(new SheetColorModel() { ColorName = "Black", ColorHex = "#272927" });
            LstColors.Add(new SheetColorModel() { ColorName = "Gray", ColorHex = "#878787" });
            LstColors.Add(new SheetColorModel() { ColorName = "Brwon", ColorHex = "#7d654c" });

            LstPriority.Add(new PriorityModel() { Id = 1, Name = "Normal" });
            LstPriority.Add(new PriorityModel() { Id = 2, Name = "Urgent" });

            OnePriorityModel = new PriorityModel() { Id = 1, Name = "Normal" };

            StrEstimateDates = "Choose Schedule Dates";
            StrInvoiceDates = "Choose Schedule Dates";

            await GetServices();
            await GetEmpCategories();
        }

        async void InitData(SchedulesModel model)
        {
            ShowImages = true;
            SchedulePage = 1; //Update Schedule 

            Init();

            ScheduleDetails = model;
            CustomerDetails = model.CustomerDTO;

            if (ScheduleDetails.CustomerDTO.MemeberType == true)
            {
                if (ScheduleDetails.CustomerDTO.MemberDTO != null)
                {
                    Discount = ScheduleDetails.CustomerDTO.MemberDTO.MemberValue;
                }
            }
            else
            {
                Discount = ScheduleDetails.CustomerDTO.Discount;
            }

            if (Discount == null)
            {
                Discount = 0;
            }

            if (model.LstScheduleItemsServices.Count > 0)
            {
                LstItems = new ObservableCollection<ScheduleItemsServicesModel>(model.LstScheduleItemsServices);
                TotalInvoice(model, CustomerDetails);
            }
            else
            {
                SubTotal = 0;
                Net = 0;
                Paid = 0;
                TotalDue = 0;
            }

            if (model.LstScheduleItemsServices.Count > 0)
            {
                LstItemsEstimate = new ObservableCollection<ScheduleItemsServicesModel>(model.LstScheduleItemsServices);
                TotalEstimate(model, CustomerDetails);
            }
            else
            {
                SubTotalEst = 0;
                NetEst = 0;
                PaidEst = 0;
                TotalDueEst = 0;
            }

            if (model.LstFreeServices.Count > 0)
            {
                LstFreeServices = new ObservableCollection<ScheduleItemsServicesModel>(model.LstFreeServices);
            }

            if (model.LstMaterialReceipt.Count > 0)
            {
                LstMaterialReceipt = new ObservableCollection<ScheduleMaterialReceiptModel>(model.LstMaterialReceipt);
            }

            InvoiceDate = DateTime.Now;

            ScheduleDate = DateTime.Parse(model.OneScheduleDate.Date);
            TimeFrom = new TimeSpan(model.OneScheduleDate.TimeHourFrom, model.OneScheduleDate.TimeMinFrom, 0);
            TimeTo = new TimeSpan(model.OneScheduleDate.TimeHourTo, model.OneScheduleDate.TimeMinTo, 0);
            OnePriorityModel = LstPriority.Where(x => x.Id == model.PriorityId).FirstOrDefault()!;

            BranchName = Helpers.Settings.BranchNameGet;

            //Schedule Pictures
            if (model.LstSchedulePictures.Count != 0)
            {
                LstAllPictures = new ObservableCollection<SchedulePicturesModel>(model.LstSchedulePictures);
                LstNewPictures = new ObservableCollection<SchedulePicturesModel>(model.LstSchedulePictures.Where(x => x.Id == 0).ToList());
                await CalcSchPhotoCount(model.LstSchedulePictures.Count);
            }

            await CalcSchPhotoCount(model.CountPhotos!.Value);

            if (model.GetPictures == true)
            {
                GetPictuers(model.Id);

            }

            StartTime = (ScheduleDetails.OneScheduleDate.StartTime == null) ? "No start yet" : ScheduleDetails.OneScheduleDate.StartTime;
            EndTime = (ScheduleDetails.OneScheduleDate.EndTime == null) ? "No end yet" : ScheduleDetails.OneScheduleDate.EndTime;

            SpentHours = (ScheduleDetails.OneScheduleDate.SpentTimeHour == null) ? "Wait job finish" : ScheduleDetails.OneScheduleDate.SpentTimeHour;
            SpentMin = (ScheduleDetails.OneScheduleDate.SpentTimeMin == null) ? "Wait job finish" : ScheduleDetails.OneScheduleDate.SpentTimeMin;

            OneScheduleDate = ScheduleDetails.OneScheduleDate;

            SelectedColor = LstColors.Where(x => x.ColorHex == model.CalendarColor).Select(c => c.IsChecked = true).FirstOrDefault();

        }


        async void CheckHouseDataCust(CustomersModel model)
        {
            if (!string.IsNullOrEmpty(model.YearEstimedValue))
            {
                //if (DateTime.Now.Year - int.Parse(model.YearEstimedValue) > 1)
                //{
                //    model = await Controls.StartData.GetAddressDetails(model);
                //}

                if (int.TryParse(model.YearEstimedValue, out int estimatedYear) && (DateTime.Now.Year - estimatedYear > 1))
                {
                    model = await Controls.StartData.GetAddressDetails(model);
                }
            }
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

        //Get all Schedules in Branch
        public async void GetAllSchedules()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                UserDialogs.Instance.ShowLoading();
                string UserToken = await _service.UserToken();
                var json = await ORep.GetAsync<ObservableCollection<SchedulesModel>>(string.Format("api/Schedules/GetSchedules?" + "AccountId=" + Helpers.Settings.AccountIdGet + "&" + "EmpId=" + Helpers.Settings.UserIdGet + "&" + "EmpRole=" + Controls.StartData.EmployeeDataStatic.UserRole + "&" + "lstEmp=" + Helpers.Settings.UserEmployeesGet + "&" + "TextSearch="), UserToken);

                if (json != null)
                {
                    //GetPerrmission();

                    //var cc = Controls.StartData.EmployeeDataStatic;
                    if (Controls.StartData.EmployeeDataStatic.ActiveAllScdTr_FaorTrOnly == false) //For Dispatch
                    {
                        LstSchedules = new ObservableCollection<SchedulesModel>(json.Where(x => x.OneScheduleDate.Active == true).ToList());
                    }
                    else
                    {
                        LstSchedules = json;
                    }

                    string day = DateTime.Now.ToString("yyyy-MM-dd");
                    CalendarDataToday = new ObservableCollection<SchedulesModel>(LstSchedules.Where(x => x.StartDate == day).ToList());

                    await GetEvents(LstSchedules);
                }

                UserDialogs.Instance.HideHud();
            }
        }


        async Task GetEvents(ObservableCollection<SchedulesModel> Lstschedules)
        {
            if (LstSchedules.Count > 0)
            {
                //LstScheduleEvevts = new CalendarEventCollection();

                string Date = "";

                foreach (var item in LstSchedules.OrderBy(appointment => DateTime.Parse(appointment.StartDate)))
                {
                    if (item.StartDate != Date)
                    {
                        GroupedList.Add(item);
                        Date = item.StartDate;
                    }
                }


                //foreach (var group in groupedList)
                //{
                //    CalendarInlineEvent event1 = new CalendarInlineEvent();

                //    DateTime Startday = DateTime.Parse(group.StartDate);
                //    DateTime StartTime = DateTime.Parse(group.Time);

                //    DateTime Endday = DateTime.Parse(group.StartDate);
                //    DateTime EndTime = DateTime.Parse(group.TimeEnd);

                //    event1.StartTime = new DateTime(Startday.Year, Startday.Month, Startday.Day, StartTime.Hour, StartTime.Minute, StartTime.Second);
                //    event1.EndTime = new DateTime(Endday.Year, Endday.Month, Endday.Day, EndTime.Hour, EndTime.Minute, EndTime.Second);
                //    event1.Subject = $"Job Title: {group.Title}";
                //    event1.Color = Color.FromHex("#538dd4");

                //    LstScheduleEvevts.Add(event1);
                //}
            }
        }

        //async Task GetEvents(ObservableCollection<SchedulesModel> Lstschedules)
        //{
        //    if (LstSchedules.Count > 0)
        //    {
        //        LstScheduleEvevts = new CalendarEventCollection();
        //        ObservableCollection<SchedulesModel> groupedList = new ObservableCollection<SchedulesModel>();

        //        string Date = "";

        //        foreach (var item in LstSchedules.OrderBy(appointment => DateTime.Parse(appointment.StartDate)))
        //        {
        //            if (item.StartDate != Date)
        //            {
        //                groupedList.Add(item);
        //                Date = item.StartDate;
        //            }
        //        }

        //        foreach (var group in groupedList)
        //        {
        //            CalendarInlineEvent event1 = new CalendarInlineEvent();

        //            DateTime Startday = DateTime.Parse(group.StartDate);
        //            DateTime StartTime = DateTime.Parse(group.Time);

        //            DateTime Endday = DateTime.Parse(group.StartDate);
        //            DateTime EndTime = DateTime.Parse(group.TimeEnd);

        //            event1.StartTime = new DateTime(Startday.Year, Startday.Month, Startday.Day, StartTime.Hour, StartTime.Minute, StartTime.Second);
        //            event1.EndTime = new DateTime(Endday.Year, Endday.Month, Endday.Day, EndTime.Hour, EndTime.Minute, EndTime.Second);
        //            event1.Subject = $"Job Title: {group.Title}";
        //            event1.Color = Color.FromHex("#538dd4");

        //            LstScheduleEvevts.Add(event1);
        //        }
        //    }
        //}

        public void TotalInvoice(SchedulesModel SchModel, CustomersModel CustModel)
        {
            if (SchModel.Id != 0)
            {
                //decimal? SumCost = SchModel.LstScheduleItemsServices.Where(x => x.CostRate > 0 && x.Out == false).Sum(s => s.CostRate * s.Quantity);
                decimal? SumCost = LstItemsInvoice.Where(x => x.CostRate > 0 && x.Out == false).Sum(s => s.CostRate * s.Quantity);

                //decimal? DiscountVal = (SchModel.CustomerDTO != null && SchModel.CustomerDTO.MemeberType == false) ? (SumCost * SchModel.CustomerDTO.Discount / 100) : (SchModel.CustomerDTO != null && SchModel.CustomerDTO.MemeberType == true) ? (SumCost - SchModel.CustomerDTO.Discount) : 0;
                decimal? DiscountVal = (SchModel.CustomerDTO != null && SchModel.CustomerDTO.MemeberType == false) ? Discount != 0 ? (SumCost * Discount / 100) : 0 : (SchModel.CustomerDTO != null && SchModel.CustomerDTO.MemberDTO == null) ? Discount != 0 ? (SumCost * Discount / 100) : 0 : (SchModel.CustomerDTO != null && SchModel.CustomerDTO.MemberDTO.MemberType == false) ? Discount != 0 ? (SumCost * Discount / 100) : 0 : (Discount);

                decimal? TaxValue = SchModel.CustomerDTO.TaxDTO != null ? (SumCost - DiscountVal) * SchModel.CustomerDTO.TaxDTO.Rate / 100 : 0;

                SubTotal = Math.Round(SumCost.Value, 2, MidpointRounding.ToEven);
                Paid = 0;
                Net = Math.Round(((SumCost - DiscountVal) + TaxValue).Value, 2, MidpointRounding.ToEven);
                TotalDue = Math.Round(((SumCost - DiscountVal) + TaxValue - Paid).Value, 2, MidpointRounding.ToEven);
            }

            if (CustModel.Id != 0 && SchModel.Id == 0)
            {
                //decimal? SumCost = CustModel.LstCustItemsServices.Where(x => x.CostRate > 0 && x.Out == false).Sum(s => s.CostRate * s.Quantity);
                decimal? SumCost = LstItemsInvoice.Where(x => x.CostRate > 0 && x.Out == false).Sum(s => s.CostRate * s.Quantity);

                decimal? DiscountVal = (CustModel.MemeberType == false) ? Discount != 0 ? (SumCost * Discount / 100) : 0 : (CustModel.MemberDTO == null) ? Discount != 0 ? (SumCost * Discount / 100) : 0 : (CustModel.MemberDTO.MemberType == false) ? (SumCost * Discount / 100) : (Discount);

                decimal? TaxValue = CustModel.TaxDTO != null ? (SumCost - DiscountVal) * CustModel.TaxDTO.Rate / 100 : 0;

                SubTotal = Math.Round(SumCost.Value, 2, MidpointRounding.ToEven);
                Paid = 0;
                Net = Math.Round(((SumCost - DiscountVal) + TaxValue).Value, 2, MidpointRounding.ToEven);
                TotalDue = Math.Round(((SumCost - DiscountVal) + TaxValue - Paid).Value, 2, MidpointRounding.ToEven);
            }
            //if (CustModel.Id != 0 && SchModel.Id == 0)
            //{
            //    decimal? SumCost = CustModel.LstCustItemsServices.Where(x => x.CostRate > 0 && x.Out == false).Sum(s => s.CostRate * s.Quantity);

            //    decimal? DiscountVal = SchModel.CustomerDTO != null ? SchModel.CustomerDTO.MemberDTO != null ? (SumCost * SchModel.CustomerDTO.MemberDTO.MemberValue / 100) : 0 : 0;
            //    decimal? TaxValue = SchModel.CustomerDTO != null ? SchModel.CustomerDTO.TaxDTO != null ? (SumCost - DiscountVal) * SchModel.CustomerDTO.TaxDTO.Rate / 100 : 0 : 0;
            //    SubTotal = Math.Round(SumCost.Value, 2, MidpointRounding.ToEven);
            //    Paid = 0;
            //    Net = Math.Round(((SumCost - DiscountVal) + TaxValue).Value, 2, MidpointRounding.ToEven);
            //    TotalDue = Math.Round(((SumCost - DiscountVal) + TaxValue - Paid).Value, 2, MidpointRounding.ToEven);
            //}

        }

        public void TotalEstimate(SchedulesModel SchModel, CustomersModel CustModel)
        {
            if (SchModel.Id != 0)
            {
                //decimal? SumCost = SchModel.LstScheduleItemsServices.Where(x => x.CostRate > 0 && x.Out == false).Sum(s => s.CostRate * s.Quantity);
                decimal? SumCost = LstItemsEstimate.Where(x => x.CostRate > 0 && x.Out == false).Sum(s => s.CostRate * s.Quantity);

                //decimal? DiscountVal = (SchModel.CustomerDTO != null && SchModel.CustomerDTO.MemeberType == false) ? (SumCost * SchModel.CustomerDTO.Discount / 100) : (SchModel.CustomerDTO != null && SchModel.CustomerDTO.MemeberType == true) ? (SumCost - SchModel.CustomerDTO.Discount) : 0;
                decimal? DiscountVal = (SchModel.CustomerDTO != null && SchModel.CustomerDTO.MemeberType == false) ? Discount != 0 ? (SumCost * Discount / 100) : 0 : (SchModel.CustomerDTO != null && SchModel.CustomerDTO.MemberDTO == null) ? Discount != 0 ? (SumCost * Discount / 100) : 0 : (SchModel.CustomerDTO != null && SchModel.CustomerDTO.MemberDTO.MemberType == false) ? Discount != 0 ? (SumCost * Discount / 100) : 0 : (Discount);

                decimal? TaxValue = SchModel.CustomerDTO.TaxDTO != null ? (SumCost - DiscountVal) * SchModel.CustomerDTO.TaxDTO.Rate / 100 : 0;

                SubTotalEst = Math.Round(SumCost.Value, 2, MidpointRounding.ToEven);
                PaidEst = 0;
                NetEst = Math.Round(((SumCost - DiscountVal) + TaxValue).Value, 2, MidpointRounding.ToEven);
                TotalDueEst = Math.Round(((SumCost - DiscountVal) + TaxValue - PaidEst).Value, 2, MidpointRounding.ToEven);
            }

            if (CustModel.Id != 0 && SchModel.Id == 0)
            {
                //decimal? SumCost = CustModel.LstCustItemsServices.Where(x => x.CostRate > 0 && x.Out == false).Sum(s => s.CostRate * s.Quantity);
                decimal? SumCost = LstItemsEstimate.Where(x => x.CostRate > 0 && x.Out == false).Sum(s => s.CostRate * s.Quantity);

                decimal? DiscountVal = (CustModel.MemeberType == false) ? Discount != 0 ? (SumCost * Discount / 100) : 0 : (CustModel.MemberDTO == null) ? Discount != 0 ? (SumCost * Discount / 100) : 0 : (CustModel.MemberDTO.MemberType == false) ? (SumCost * Discount / 100) : (Discount);

                decimal? TaxValue = CustModel.TaxDTO != null ? (SumCost - DiscountVal) * CustModel.TaxDTO.Rate / 100 : 0;

                SubTotalEst = Math.Round(SumCost.Value, 2, MidpointRounding.ToEven);
                PaidEst = 0;
                NetEst = Math.Round(((SumCost - DiscountVal) + TaxValue).Value, 2, MidpointRounding.ToEven);
                TotalDueEst = Math.Round(((SumCost - DiscountVal) + TaxValue - PaidEst).Value, 2, MidpointRounding.ToEven);
            }
        }
        

        async Task GetServices()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                UserDialogs.Instance.ShowLoading();
                string UserToken = await _service.UserToken();
                var json = await ORep.GetAsync<ObservableCollection<ItemsServicesModel>>(string.Format("api/Schedules/GetServices?" + "AccountId=" + Helpers.Settings.AccountIdGet), UserToken);

                if (json != null)
                {
                    LstServices = json;

                    if (ScheduleDetails.OneScheduleService != null && ScheduleDetails.OneScheduleService.ScheduleDateId == null)
                    {
                        SelectedService = LstServices.Where(x => x.Id == ScheduleDetails.OneScheduleService.ItemsServicesId).FirstOrDefault();
                    }
                }

                UserDialogs.Instance.HideHud();
            }
        }

        async void GetScheduleDates(int ScheduleId, int Type)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                UserDialogs.Instance.ShowLoading();
                string UserToken = await _service.UserToken();
                var json = await ORep.GetAsync<ObservableCollection<SchaduleDateModel>>(string.Format("api/Schedules/GetScheduleDates?" + "ScheduleId=" + ScheduleId + "&" + "Type=" + Type), UserToken);

                if (json != null)
                {
                    if (json.Count == 1)
                    {
                        LstEstimateSchaduleDatesActual = json;
                        StrEstimateDates = json.FirstOrDefault().Date;
                        LstInvoiceSchaduleDatesActual = json;
                        StrInvoiceDates = json.FirstOrDefault().Date;
                        LstEstimateSchaduleDates = json;
                        LstInvoiceSchaduleDates = json;
                        IsShowScheduleDates = false;
                    }
                    else
                    {
                        LstEstimateSchaduleDates = json;
                        LstInvoiceSchaduleDates = json;
                    }

                }

                UserDialogs.Instance.HideHud();
            }
        }


        //Get One Schedule Details
        public async Task GetOneScheduleDetails(int ScheduleId, int ScheduleDateId)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                UserDialogs.Instance.ShowLoading();
                string UserToken = await _service.UserToken();
                var Schedule = await ORep.GetAsync<SchedulesModel>(string.Format("api/Schedules/GetScheduleDetails?" + "ScheduleId=" + ScheduleId + "&" + "ScheduleDateId=" + ScheduleDateId), UserToken);

                if (Schedule != null)
                {
                    Schedule.CustomerDTO = Schedule.CustomerDTO == null ? new CustomersModel() : Schedule.CustomerDTO;
                    Schedule.OneScheduleDate = Schedule.OneScheduleDate == null ? new SchaduleDateModel() : Schedule.OneScheduleDate;


                    Schedule.LstScheduleEmployeeDTO = Schedule.LstScheduleEmployeeDTO == null ? new List<ScheduleEmployeesModel>() : Schedule.LstScheduleEmployeeDTO;

                    Schedule.LstScheduleItemsServices = Schedule.LstScheduleItemsServices == null ? new List<ScheduleItemsServicesModel>() : Schedule.LstScheduleItemsServices;
                    Schedule.LstSchedulePictures = Schedule.LstSchedulePictures == null ? new List<SchedulePicturesModel>() : Schedule.LstSchedulePictures;
                    Schedule.LstMaterialReceipt = Schedule.LstMaterialReceipt == null ? new List<ScheduleMaterialReceiptModel>() : Schedule.LstMaterialReceipt;

                    ScheduleDetails = Schedule;


                    if (ScheduleDetails.LstScheduleItemsServices.Count > 4)
                    {
                        LstHeight = 1;
                    }

                    if (ScheduleDetails.EstimateDTO != null)
                    {
                        ShowEstimateButton = true;
                    }

                    InitData(ScheduleDetails);

                    LstEmps = new ObservableCollection<ScheduleEmployeesModel>(ScheduleDetails.LstScheduleEmployeeDTO);

                    foreach (var mod in ScheduleDetails.LstScheduleEmployeeDTO)
                    {
                        StrEmployeesId += ("," + mod.EmpId);
                    }
                    StrEmployeesId = StrEmployeesId?.Remove(0, 1);
                }

                UserDialogs.Instance.HideHud();
            }
        }

        //Get Employees Category
        public async Task GetEmpCategories()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                UserDialogs.Instance.ShowLoading();
                string UserToken = await _service.UserToken();
                var json = await ORep.GetAsync<ObservableCollection<EmployeesCategoryModel>>(string.Format("api/Employee/GetEmpCategory?" + "AccountId=" + Helpers.Settings.AccountIdGet), UserToken);

                if (json != null)
                {
                    LstEmpCategory = json;

                    if (ScheduleDetails?.EmployeeCategoryId != null && ScheduleDetails?.LstScheduleEmployeeDTO.Count > 0)
                    {
                        SelectedCateory = LstEmpCategory.Where(x => x.Id == ScheduleDetails?.EmployeeCategoryId).FirstOrDefault();

                        string str = "";
                        foreach (ScheduleEmployeesModel Emp in ScheduleDetails?.LstScheduleEmployeeDTO)
                        {
                            str += ("," + Emp.EmpUserName);
                        }
                        StrEmployees = str.Remove(0, 1);
                    }
                    else
                    {
                        SelectedCateory = LstEmpCategory.FirstOrDefault();
                    }
                }

                UserDialogs.Instance.HideHud();
            }
        }

        //Get Pictuers
        async void GetPictuers(int ScheduleId)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                UserDialogs.Instance.ShowLoading();
                string UserToken = await _service.UserToken();
                var json = await ORep.GetAsync<ObservableCollection<SchedulePicturesModel>>(string.Format("api/Schedules/GetPictures?" + "ScheduleId=" + ScheduleId), UserToken);

                if (json != null)
                {
                    LstNewPictures = new ObservableCollection<SchedulePicturesModel>(); //Check if Show Button Done
                    ScheduleDetails.LstSchedulePictures = json.ToList();
                    LstAllPictures = json;

                    SetLstTwoSchedulePhotos(ScheduleDetails.LstSchedulePictures);
                }

                UserDialogs.Instance.HideHud();
            }
        }

        // Get Employees in One Category
        [RelayCommand]
        async Task SelectedEmpCategory(EmployeesCategoryModel model)
        {

            EmpCategory = model;

            //StrEmployees = "Choose Employees";
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                UserDialogs.Instance.ShowLoading();
                string UserToken = await _service.UserToken();
                var json = await ORep.GetAsync<ObservableCollection<EmployeeModel>>(string.Format("api/Employee/GetEmpInOneCategory/{0}/{1}/{2}/{3}/{4}", Helpers.Settings.BranchIdGet, EmpCategory.Id, Helpers.Settings.AccountIdGet, Controls.StartData.EmployeeDataStatic.UserRole, Helpers.Settings.UserIdGet), UserToken);

                if (json != null)
                {
                    LstEmpInOneCategory = json;
                }

                UserDialogs.Instance.HideHud();
            }
        }

        [RelayCommand]
        async Task ScheduleDetailsformList(SchedulesModel model)
        {
            var VM = new SchedulesViewModel(model.Id, model.ScheduleDateId);
            var page = new Pages.SchedulePages.ScheduleDetailsPage();
            page.BindingContext = VM;
            await App.Current!.MainPage!.Navigation.PushAsync(page);
        }

        [RelayCommand]
        async Task ChangeTextSearchJobs(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                IsShowSearchByItem = true;
            }
            else
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    IsShowSearchByItem = false;
                    string UserToken = await _service.UserToken();
                    var json = await ORep.GetAsync<ObservableCollection<SchedulesModel>>(string.Format("api/Schedules/GetSchedules?" + "AccountId=" + Helpers.Settings.AccountIdGet + "&" + "EmpId=" + Helpers.Settings.UserIdGet + "&" + "EmpRole=" + Controls.StartData.EmployeeDataStatic.UserRole + "&" + "lstEmp=" + Helpers.Settings.UserEmployeesGet + "&" + "TextSearch=" + text), UserToken);

                    if (json != null)
                    {
                        if (Controls.StartData.EmployeeDataStatic.ActiveAllScdTr_FaorTrOnly == false) //For Dispatch
                        {
                            LstSchedulesSearch = new ObservableCollection<SchedulesModel>(json.Where(x => x.OneScheduleDate.Active == true).ToList());
                        }
                        else
                        {
                            LstSchedulesSearch = json;
                        }
                    }
                }
            }
        }

        [RelayCommand]
        async Task CallCustomer(CustomersModel model)
        {
            try
            {
                int tel;
                bool Result = int.TryParse(model.Phone1, out tel);
                if (Result)
                {
                    PhoneDialer.Open(model.Phone1);
                }
                else
                {
                    await App.Current!.MainPage!.DisplayAlert("Warning", "You don't access this customer's phone", "OK");
                }

            }
            catch (FeatureNotSupportedException ex)
            {
                // Handle the case where the phone dialer is not supported on the device
                await App.Current!.MainPage!.DisplayAlert("Error", "Phone dialer is not supported on this device.", "OK");
            }
            catch (Exception ex)
            {
                // Handle other errors that might occur
                await App.Current!.MainPage!.DisplayAlert("Error", "Unable to dial this number.", "OK");
            }

        }

        [RelayCommand]
        async Task SelectedDispatch(SchaduleDateModel model)
        {
            IsEnable = false;
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                string UserToken = await _service.UserToken();

                UserDialogs.Instance.ShowLoading();
                string Json = await ORep.PutStrAsync("api/Schedules/PutScheduleDispatch", model, UserToken);
                UserDialogs.Instance.HideHud();

                if (!string.IsNullOrEmpty(Json) && Json.Contains("Success Dispatch") == true)
                {
                    await App.Current!.MainPage!.DisplayAlert("FixPro", "Succes for Dispatch Schedule.", "Ok");
                    ShowDispatch = 2; //Don't Show Dispatch Button
                }
                else
                {
                    await App.Current!.MainPage!.DisplayAlert("FixPro", "Failed for Dispatch Schedule.", "Ok");
                }
            }
            IsEnable = true;
        }

        [RelayCommand]
        async Task ReturnBackFromScheduleImages(SchedulesModel model)
        {
            IsEnable = false;
            UserDialogs.Instance.ShowLoading();
            var popupView = new SchedulesViewModel(model.Id, model.OneScheduleDate.Id);
            var page = new Pages.SchedulePages.ScheduleDetailsPage();
            page.BindingContext = popupView;
            await App.Current!.MainPage!.Navigation.PushAsync(page);
            App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);
            App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);
            UserDialogs.Instance.HideHud();
            IsEnable = true;
        }

        [RelayCommand]
        async Task SelecteNewItemsEstimate(SchedulesModel model)
        {
            IsEnable = false;

            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
                    return;
                }
                else
                {
                    var popupView = new ScheduleItemsServicesViewModel(ShowQty);
                    popupView.ItemClose += async (item) =>
                    {
                        UserDialogs.Instance.ShowLoading();

                        ScheduleItemsServicesModel ItemsModel = new ScheduleItemsServicesModel
                        {
                            AccountId = model.AccountId,
                            BrancheId = model.BrancheId,
                            ScheduleId = model.Id,
                            ItemsServicesId = item.Id,
                            ItemsServicesName = item.Name,
                            ItemServiceDescription = item.Description,
                            TaxId = item.TaxId,
                            Tax = item.Tax,
                            CostRate = item.CostperUnit,
                            Total = item.QTYTime != null && item.Tax != null ? (item.CostperUnit * item.QTYTime) + (item.CostperUnit * item.QTYTime * item.Tax / 100) : item.QTYTime == null && item.Tax != null ? item.CostperUnit + (item.CostperUnit * item.QTYTime * item.Tax / 100) : item.QTYTime != null && item.Tax == null ? item.CostperUnit * item.QTYTime : item.CostperUnit,
                            Notes = item.Notes,
                            Active = item.Active,
                            CreateUser = model.CreateUser,
                            CreateDate = item.CreateDate,
                            Taxable = item.Taxable,
                            Quantity = (item.QTYTime == null || item.QTYTime == 0) ? 1 : item.QTYTime,
                            Unit = item.Unit,
                        };

                        if (LstItemsEstimate.Count > 0)
                        {
                            var itm = LstItemsEstimate.Where(x => x.ItemsServicesId == item.Id).FirstOrDefault();
                            if (itm == null)
                            {
                                LstItemsEstimate.Add(ItemsModel);
                            }
                        }
                        else
                        {
                            LstItemsEstimate.Add(ItemsModel);
                        }

                        if (LstItemsEstimate.Count > 4)
                        {
                            LstHeight = 1;
                        }

                        TotalEstimate(model, CustomerDetails);

                        UserDialogs.Instance.HideHud();
                    };

                    var page = new Pages.SchedulePages.NewItemsServicesSchedulePage();
                    page.BindingContext = popupView;
                    await App.Current!.MainPage!.Navigation.PushAsync(page);
                }
            }
            catch (Exception ex)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
            }

            IsEnable = true;
        }

        [RelayCommand]
        void RemoveItemEstimate(ScheduleItemsServicesModel item)
        {
            IsEnable = false;

            LstItemsEstimate.Remove(item);

            TotalEstimate(ScheduleDetails, CustomerDetails);

            IsEnable = true;
        }

        [RelayCommand]
        async Task SelecteNewItems(SchedulesModel model)
        {
            IsEnable = false;

            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await App.Current!.MainPage!.DisplayAlert("Error", "No Internet connection!", "OK");
                    return;
                }
                else
                {
                    var popupView = new ScheduleItemsServicesViewModel(ShowQty);
                    popupView.ItemClose += async (item) =>
                    {
                        UserDialogs.Instance.ShowLoading();

                        ScheduleItemsServicesModel ItemsModel = new ScheduleItemsServicesModel
                        {
                            AccountId = model.AccountId,
                            BrancheId = model.BrancheId,
                            ScheduleId = model.Id,
                            ScheduleDateId = model.OneScheduleDate.Id,
                            ItemsServicesId = item.Id,
                            ItemsServicesName = item.Name,
                            ItemServiceDescription = item.Description,
                            TaxId = item.TaxId,
                            Tax = item.Tax,
                            CostRate = item.CostperUnit,
                            Price = item.CostperUnit,
                            Total = item.QTYTime != null && item.Tax != null ? (item.CostperUnit * item.QTYTime) + (item.CostperUnit * item.QTYTime * item.Tax / 100) : item.QTYTime == null && item.Tax != null ? item.CostperUnit + (item.CostperUnit * item.QTYTime * item.Tax / 100) : item.QTYTime != null && item.Tax == null ? item.CostperUnit * item.QTYTime : item.CostperUnit,
                            Notes = item.Notes,
                            Active = item.Active,
                            CreateUser = model.CreateUser,
                            CreateDate = item.CreateDate,
                            Taxable = item.Taxable,
                            Quantity = (item.QTYTime == null || item.QTYTime == 0) ? 1 : item.QTYTime,
                            Unit = item.Unit,
                        };

                        if (ShowQty == false)// add material
                        {
                            ScheduleItemsServicesModel scheduleItemsServicesModel = new ScheduleItemsServicesModel();

                            if (LstItems.Count > 0)
                            {
                                var itm = LstItems.Where(x => x.ItemsServicesId == item.Id).FirstOrDefault();
                                if (itm == null)
                                {
                                    scheduleItemsServicesModel = await InsertOneItemService(ItemsModel);
                                    if (scheduleItemsServicesModel != null)
                                    {
                                        LstItems.Add(scheduleItemsServicesModel);
                                    }
                                }
                            }
                            else
                            {
                                scheduleItemsServicesModel = await InsertOneItemService(ItemsModel);
                                if (scheduleItemsServicesModel != null)
                                {
                                    LstItems.Add(scheduleItemsServicesModel);
                                }
                            }
                        }
                        else // add invoice item or Estimate item
                        {
                            var itm2 = LstItemsInvoice.Where(x => x.ItemsServicesId == item.Id).FirstOrDefault();
                            if (itm2 == null)
                            {
                                LstItemsInvoice.Add(ItemsModel);
                            }
                        }

                        if (LstItemsInvoice.Count > 4)
                        {
                            LstHeight = 1;
                        }


                        if (LstItems.Count > 4)
                        {
                            LstHeight = 1;
                        }

                        TotalInvoice(model, CustomerDetails);


                        UserDialogs.Instance.HideHud();
                    };

                    var page = new Pages.SchedulePages.NewItemsServicesSchedulePage();
                    page.BindingContext = popupView;
                    await App.Current!.MainPage!.Navigation.PushAsync(page);
                }
            }
            catch (Exception ex)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
            }

            IsEnable = true;
        }


        [RelayCommand]
        async Task SelecteNewFreeService(SchedulesModel model)
        {
            IsEnable = false;

            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
                    return;
                }
                else
                {
                    var popupView = new ScheduleFreeServicesViewModel(false);
                    popupView.ServiceClose += async (service) =>
                    {
                        UserDialogs.Instance.ShowLoading();

                        ScheduleItemsServicesModel ServiceModel = new ScheduleItemsServicesModel
                        {
                            AccountId = model.AccountId,
                            BrancheId = model.BrancheId,
                            ScheduleId = model.Id,
                            ScheduleDateId = model.OneScheduleDate.Id,
                            ItemsServicesId = service.Id,
                            ItemsServicesName = service.Name,
                            ItemServiceDescription = service.Description,
                            TaxId = service.TaxId,
                            Tax = service.Tax,
                            CostRate = service.CostperUnit,
                            Price = service.CostperUnit,
                            Total = service.QTYTime != null && service.Tax != null ? (service.CostperUnit * service.QTYTime) + (service.CostperUnit * service.QTYTime * service.Tax / 100) : service.QTYTime == null && service.Tax != null ? service.CostperUnit + (service.CostperUnit * service.QTYTime * service.Tax / 100) : service.QTYTime != null && service.Tax == null ? service.CostperUnit * service.QTYTime : service.CostperUnit,
                            Notes = service.Notes,
                            Active = service.Active,
                            CreateUser = model.CreateUser,
                            CreateDate = service.CreateDate,
                            Taxable = service.Taxable,
                            Quantity = (service.QTYTime == null || service.QTYTime == 0) ? 1 : service.QTYTime,
                            Unit = service.Unit,
                        };

                        ScheduleItemsServicesModel scheduleItemsServicesModel = new ScheduleItemsServicesModel();

                        if (LstFreeServices.Count > 0)
                        {
                            var itm = LstFreeServices.Where(x => x.ItemsServicesId == service.Id).FirstOrDefault();
                            if (itm == null)
                            {
                                scheduleItemsServicesModel = await InsertOneFreeService(ServiceModel);
                                if (scheduleItemsServicesModel != null)
                                {
                                    LstFreeServices.Add(scheduleItemsServicesModel);

                                }
                            }
                        }
                        else
                        {
                            scheduleItemsServicesModel = await InsertOneFreeService(ServiceModel);
                            if (scheduleItemsServicesModel != null)
                            {
                                LstFreeServices.Add(scheduleItemsServicesModel);
                            }
                        }

                        if (LstFreeServices.Count > 4)
                        {
                            LstHeight = 1;
                        }

                        UserDialogs.Instance.HideHud();
                    };

                    var page = new Pages.SchedulePages.ScheduleFreeServicesPage();
                    page.BindingContext = popupView;
                    await App.Current!.MainPage!.Navigation.PushAsync(page);
                }
            }
            catch (Exception ex)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
            }

            IsEnable = true;
        }

        [RelayCommand]
        async Task MyWay(CustomersModel model)
        {
            IsEnable = false;
            var popupView = new OnMyWayViewModel(model);
            var page = new OnMyWayPopup();
            page.BindingContext = popupView;
            await MopupService.Instance.PushAsync(page);
            IsEnable = true;
        }

        [RelayCommand]
        public async Task<ScheduleItemsServicesModel> InsertOneItemService(ScheduleItemsServicesModel model)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                string UserToken = await _service.UserToken();
                var json = await ORep.PostDataAsync("api/Schedules/PostScheduleMaterials", model, UserToken);

                if (json != "Bad Request" && json != "api not responding" && json.Contains("Not_Enough") != true && json.Contains("This Invoice Already Exist") != true)
                {
                    var oModel = JsonConvert.DeserializeObject<ScheduleItemsServicesModel>(json);
                    return oModel;
                }
                else
                {
                    await App.Current!.MainPage!.DisplayAlert("Alert", json, "Ok");
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        [RelayCommand]
        public async Task<ScheduleItemsServicesModel> InsertOneFreeService(ScheduleItemsServicesModel model)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                string UserToken = await _service.UserToken();
                var json = await ORep.PostStrAsync("api/Schedules/PostScheduleFreeServices", model, UserToken);

                if (!string.IsNullOrEmpty(json))
                {
                    var oModel = JsonConvert.DeserializeObject<ScheduleItemsServicesModel>(json);
                    return oModel;
                }
                else
                {
                    await App.Current!.MainPage!.DisplayAlert("Alert", "Faild for add this Service.", "Ok");
                    return null;
                }
            }
            else
            {
                return null;
            }

        }

        [RelayCommand]
        async Task SelecteNewMaterialReceipt(SchedulesModel model)
        {
            IsEnable = false;

            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
                    return;
                }
                else
                {
                    var popupView = new ScheduleMaterialReceiptViewModel();
                    popupView.MaterialRcClose += async (item) =>
                    {
                        UserDialogs.Instance.ShowLoading();

                        string UserToken = await _service.UserToken();

                        ScheduleMaterialReceiptModel MaterialReceiptModel = new ScheduleMaterialReceiptModel
                        {
                            AccountId = model.AccountId,
                            BrancheId = model.BrancheId,
                            ScheduleId = model.Id,
                            ScheduleDateId = model.OneScheduleDate.Id,
                            SupplierId = item.SupplierId,
                            SupplierName = item.SupplierName,
                            TechnicianId = int.Parse(Helpers.Settings.UserIdGet),
                            Cost = item.Cost,
                            Notes = item.Notes,
                            ReceiptPhoto = item.ReceiptPhoto,
                            CreateUser = model.CreateUser,
                            CreateDate = DateTime.Now,
                        };

                        var json = await ORep.PostStrAsync("api/Schedules/PostScheduleMaterialReceipt", MaterialReceiptModel, UserToken);

                        if (string.IsNullOrEmpty(json))
                        {
                            await App.Current!.MainPage!.DisplayAlert("Alert", "Faild for add this Material Receipt.", "Ok");
                        }
                        else
                        {
                            //ScheduleDetails.LstMaterialReceipt.Add(MaterialReceiptModel);
                            LstMaterialReceipt.Add(JsonConvert.DeserializeObject<ScheduleMaterialReceiptModel>(json));
                        }

                        UserDialogs.Instance.HideHud();
                    };

                    var page = new Pages.SchedulePages.MaterialReceiptPage();
                    page.BindingContext = popupView;
                    await App.Current!.MainPage!.Navigation.PushAsync(page);
                }
            }
            catch (Exception ex)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
            }

            IsEnable = true;
        }

        [RelayCommand]
        async Task RemoveItem(ScheduleItemsServicesModel item)
        {
            IsEnable = false;

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                UserDialogs.Instance.ShowLoading();
                if (ShowQty == false) // Remove material
                {
                    string UserToken = await _service.UserToken();
                    var json = await ORep.PutStrAsync("api/Schedules/PutMaterial", item, UserToken);//Delete Material

                    if (string.IsNullOrEmpty(json))
                    {
                        await App.Current!.MainPage!.DisplayAlert("Alert", "Failed to delete the material", "Ok");
                    }
                    else
                    {
                        LstItems.Remove(item);
                        ScheduleDetails.LstScheduleItemsServices.Remove(item);
                    }
                }
                else
                {
                    if (LstItemsInvoice.Count > 0) //Remove invoice item
                    {
                        LstItemsInvoice.Remove(item);
                        //ScheduleDetails.LstScheduleItemsServices.Remove(item);

                        TotalInvoice(ScheduleDetails, CustomerDetails);
                    }

                }

                UserDialogs.Instance.HideHud();
            }

            IsEnable = true;
        }

        [RelayCommand]
        async Task RemoveService(ScheduleItemsServicesModel service)
        {
            IsEnable = false;
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                UserDialogs.Instance.ShowLoading();
                string UserToken = await _service.UserToken();
                var json = await ORep.PutStrAsync("api/Schedules/PutFreeService", service, UserToken);//Delete Service

                if (string.IsNullOrEmpty(json))
                {
                    await App.Current!.MainPage!.DisplayAlert("Alert", "Faild for Delete this Service.", "Ok");
                }
                else
                {
                    LstFreeServices.Remove(service);
                    ScheduleDetails.LstFreeServices.Remove(service);

                    //TotalInvoice(ScheduleDetails, CustomerDetails);
                }

                UserDialogs.Instance.HideHud();
            }

            IsEnable = true;
        }

        [RelayCommand]
        async Task FullScreenNote(string Note)
        {
            var popupView = new FullScreenNoteViewModel(Note);
            popupView.NoteClose += (note) =>
            {
                ScheduleDetails.Notes = note;
            };
            var page = new Pages.SchedulePages.FullScreenNotePage();
            page.BindingContext = popupView;
            await App.Current!.MainPage!.Navigation.PushAsync(page);
        }

        [RelayCommand]
        async Task RemoveMaterialReceipt(ScheduleMaterialReceiptModel item)
        {
            IsEnable = false;
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                UserDialogs.Instance.ShowLoading();

                string UserToken = await _service.UserToken();

                var json = await ORep.PutStrAsync("api/Schedules/PutMaterialReceipt", item, UserToken);//Delete Material Receipt

                if (string.IsNullOrEmpty(json))
                {
                    await App.Current!.MainPage!.DisplayAlert("Alert", "Faild for Delete this Material Receipt.", "Ok");
                }
                else
                {
                    LstMaterialReceipt.Remove(item);
                    ScheduleDetails.LstMaterialReceipt.Remove(item);

                }

                UserDialogs.Instance.HideHud();
            }

            IsEnable = true;
        }

        [RelayCommand]
        void SelectedEmpInOneCategory(ObservableCollection<EmployeeModel> LstEmp)
        {
            if (LstEmp.Count != 0)
            {
                string str = "";
                string strId = "";
                foreach (var Emp in LstEmp)
                {
                    str += ("," + Emp.UserName);
                    strId += ("," + Emp.Id);
                }

                StrEmployees = str.Remove(0, 1);
                StrEmployeesId = strId.Remove(0, 1);
            }
        }

        [RelayCommand]
        async Task OpenEstimateScheduleDates()
        {
            IsEnable = false;
            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
                    return;
                }
                else
                {
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
            }

            IsEnable = true;
        }

        [RelayCommand]
        async Task OpenInvoiceScheduleDates()
        {
            IsEnable = false;
            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
                    return;
                }
                else
                {
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
            }

            IsEnable = true;
        }

        [RelayCommand]
        async Task OpenEmployeesInOneCategory()
        {
            IsEnable = false;
            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
                    return;
                }
                else
                {
                    var popupView = new Pages.PopupPages.EmployeesPopup(LstEmpInOneCategory);
                    popupView.EmployeesClose += (Empolyees) =>
                    {

                        //LstEmps.Clear();
                        //StrEmployees = "";
                        //StrEmployeesId = "";

                        if (Empolyees.Count != 0)
                        {
                            string str = "";
                            string strId = "";
                            foreach (var Emp in Empolyees)
                            {
                                str += ("," + Emp.UserName);
                                strId += ("," + Emp.Id);
                                LstEmps.Add(new ScheduleEmployeesModel
                                {
                                    EmpId = Emp.Id,
                                    EmpFullName = Emp.FirstName + " " + Emp.LastName,
                                    EmpUserName = Emp.UserName,
                                });
                            }

                            if (!string.IsNullOrEmpty(StrEmployees) && !string.IsNullOrEmpty(StrEmployeesId))
                            {
                                StrEmployees += str;
                                StrEmployeesId += strId;
                            }
                            else
                            {
                                StrEmployees = str.Remove(0, 1);
                                StrEmployeesId = strId.Remove(0, 1);
                            }
                        }
                    };

                    await MopupService.Instance.PushAsync(popupView);
                }
            }
            catch (Exception ex)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
            }

            IsEnable = true;
        }

        [RelayCommand]
        void RemoveEmployee(ScheduleEmployeesModel employee)
        {
            LstEmps.Remove(employee);

            //Empolyees Names 
            int index = StrEmployees.IndexOf(employee.EmpUserName + ",");
            StrEmployees = (index < 0) ? StrEmployees : StrEmployees.Remove(index, (employee.EmpUserName + ",").Length);

            int index2 = StrEmployees.IndexOf("," + employee.EmpUserName);
            StrEmployees = (index2 < 0) ? StrEmployees : StrEmployees.Remove(index2, ("," + employee.EmpUserName).Length);

            int index3 = StrEmployees.IndexOf(employee.EmpUserName);
            StrEmployees = (index3 < 0) ? StrEmployees : StrEmployees.Remove(index3, (employee.EmpUserName).Length);


            //Empolyees Ids 
            int indexId = StrEmployeesId.IndexOf(employee.EmpId + ",");
            StrEmployeesId = (indexId < 0) ? StrEmployeesId : StrEmployeesId.Remove(indexId, (employee.EmpId + ",").Length);

            int indexId2 = StrEmployeesId.IndexOf("," + employee.EmpId);
            StrEmployeesId = (indexId2 < 0) ? StrEmployeesId : StrEmployeesId.Remove(indexId2, ("," + employee.EmpId).Length);

            int indexId3 = StrEmployeesId.IndexOf(employee.EmpId.ToString()!);
            StrEmployeesId = (indexId3 < 0) ? StrEmployeesId : StrEmployeesId.Remove(indexId3, (employee.EmpId.ToString()!).Length);
        }

        [RelayCommand]
        void RemoveEstimateDate(SchaduleDateModel Date)
        {
            LstEstimateDates.Remove(Date);

            foreach (SchaduleDateModel dt in LstEstimateSchaduleDates)
            {
                if (dt.Id == Date.Id)
                {
                    dt.IsChecked = false;
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

            foreach (SchaduleDateModel dt in LstInvoiceSchaduleDates)
            {
                if (dt.Id == Date.Id)
                {
                    dt.IsChecked = false;
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
        async Task SelectedSubmitSchedule(SchedulesModel model)
        {

            IsEnable = false;
            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
                    return;
                }
                else
                {
                    ScheduleDetails = model;
                    ScheduleDetails.CustomerDTO = CustomerDetails;
                    ScheduleDetails.CustomerName = CustomerDetails.FirstName + " " + CustomerDetails.LastName;
                    ScheduleDetails.AccountId = int.Parse(Helpers.Settings.AccountIdGet);
                    ScheduleDetails.BrancheId = int.Parse(Helpers.Settings.BranchIdGet);
                    ScheduleDetails.CreateUser = int.Parse(Helpers.Settings.UserIdGet);
                    ScheduleDetails.Active = true;
                    ScheduleDetails.Recurring = false;
                    ScheduleDetails.FrequencyType = 1;
                    ScheduleDetails.EndType = 1;
                    ScheduleDetails.PriorityId = OnePriorityModel.Id;
                    ScheduleDetails.Time = TimeFrom.ToString(@"hh\:mm");
                    ScheduleDetails.TimeEnd = TimeTo.ToString(@"hh\:mm");
                    ScheduleDetails.ScheduleDate = ScheduleDetails.StartDate = ScheduleDate.ToString("yyyy-MM-dd");

                    ScheduleItemsServicesModel ItemsModel = new ScheduleItemsServicesModel
                    {
                        AccountId = model.AccountId,
                        BrancheId = model.BrancheId,
                        ScheduleId = model.Id,
                        ItemsServicesId = SelectedService.Id,
                        ItemServiceDescription = SelectedService.Description,
                        CostRate = SelectedService.CostperUnit,
                        Notes = SelectedService.Notes,
                        Active = SelectedService.Active,
                        CreateUser = model.CreateUser,
                        CreateDate = DateTime.Now,
                    };
                    ScheduleDetails.OneScheduleService = ItemsModel;
                    //ScheduleDetails.LstScheduleItemsServices = LstItems.ToList();

                    if (CustomerDetails.Id != 0)
                    {
                        ScheduleDetails.CustomerId = CustomerDetails.Id;
                    }
                    ScheduleDetails.Location = CustomerDetails.Address;
                    ScheduleDetails.EmployeeCategoryId = EmpCategory.Id;
                    if (StrEmployeesId != null)
                    {
                        ScheduleDetails.Employees = StrEmployeesId;
                    }
                    //ScheduleDetails.CalendarColor = LstColors.Where(x => x.IsChecked == true).Select(c => c.ColorHex).FirstOrDefault();
                    ScheduleDetails.CalendarColor = "#5e92e6";
                    ScheduleDetails.CreateDate = DateTime.Now;

                    if (string.IsNullOrEmpty(ScheduleDetails.Title))
                    {
                        await App.Current!.MainPage!.DisplayAlert("Alert", $"Please Complete This Field Required : Title.", "Ok");
                    }
                    else if (string.IsNullOrEmpty(ScheduleDetails.ScheduleDate))
                    {
                        await App.Current!.MainPage!.DisplayAlert("Alert", $"Please Complete This Field Required : Schedule Date.", "Ok");
                    }
                    else if (string.IsNullOrEmpty(ScheduleDetails.Time))
                    {
                        await App.Current!.MainPage!.DisplayAlert("Alert", $"Please Complete This Field Required : Start Time.", "Ok");
                    }
                    else if (string.IsNullOrEmpty(ScheduleDetails.TimeEnd))
                    {
                        await App.Current!.MainPage!.DisplayAlert("Alert", $"Please Complete This Field Required : End Time.", "Ok");
                    }
                    else if (ScheduleDetails.EmployeeCategoryId == null || ScheduleDetails.EmployeeCategoryId == 0)
                    {
                        await App.Current!.MainPage!.DisplayAlert("Alert", $"Please Complete This Field Required : Choose Employee Category.", "Ok");
                    }
                    else if (string.IsNullOrEmpty(ScheduleDetails.Employees))
                    {
                        await App.Current!.MainPage!.DisplayAlert("Alert", $"Please Complete This Field Required : Choose Employees.", "Ok");
                    }
                    else
                    {
                        if (ScheduleDetails != null)
                        {
                            string UserToken = await _service.UserToken();
                            var Json = "";
                            if (model.Id == 0)
                            {
                                ScheduleDetails.CallId = CustomerDetails.CallId;
                                UserDialogs.Instance.ShowLoading();
                                Json = await ORep.PostDataAsync("api/Schedules/PostSchedule", ScheduleDetails, UserToken);
                                UserDialogs.Instance.HideHud();
                            }
                            else
                            {
                                UserDialogs.Instance.ShowLoading();
                                Json = await ORep.PutDataAsync("api/Schedules/PutSchedule", ScheduleDetails, UserToken);
                                UserDialogs.Instance.HideHud();
                            }
                            if (Json != "Bad Request" && Json != "api not responding")
                            {
                                await App.Current!.MainPage!.DisplayAlert("FixPro", "Succes for Save Schedule.", "Ok");
                                if (model.Id == 0)
                                {
                                    var VM = new SchedulesViewModel();
                                    var page = new Pages.SchedulePages.SchedulePage();
                                    page.BindingContext = VM;
                                    await App.Current!.MainPage!.Navigation.PushAsync(page);
                                    App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);

                                    string Massage = $"Hi {model.CustomerName}! Your service appointment with {Helpers.Settings.AccountName} has been scheduled for {DateTime.Parse(model.StartDate).ToString("MMMM dd, yyyy")}. Your technician will arrive between {DateTime.Parse(model.Time).ToString("hh:mmtt")} - {DateTime.Parse(model.TimeEnd).ToString("hh:mmtt")} CDT.";

                                    string returnMsg = SendSMS(ScheduleDetails.CustomerDTO.Phone1, Massage);
                                    if (string.IsNullOrEmpty(returnMsg))
                                    {
                                        await App.Current!.MainPage!.DisplayAlert("Alert", "Succes for Save Schedule but Faild Send SMS to Customer.", "Ok");
                                    }
                                }
                                else
                                {
                                    var VM = new SchedulesViewModel(model.Id, model.ScheduleDateId);
                                    var page = new Pages.SchedulePages.ScheduleDetailsPage();
                                    page.BindingContext = VM;
                                    await App.Current!.MainPage!.Navigation.PushAsync(page);
                                    App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);
                                }
                            }
                            else
                            {
                                await App.Current!.MainPage!.DisplayAlert("Alert", "Faild for add or edit schedule.", "Ok");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current!.MainPage!.DisplayAlert("Alert", ex.Message, "OK");
            }

            IsEnable = true;
        }

        string SendSMS(string Phone, string Msg)
        {
            var accountSid = "AC2aa33faec930e6bddfef1daa25e3b945";
            var authToken = "744fd3259244985557d4d0c1aa2617eb";
            TwilioClient.Init(accountSid, authToken);

            var messageOptions = new CreateMessageOptions(
              new PhoneNumber("+1" + Phone));

            messageOptions.From = new PhoneNumber("+18885307372");
            messageOptions.Body = Msg;
            var message = MessageResource.Create(messageOptions);

            return message.Sid;
        }

        //string SendSMS(string Phone, string Code)
        //{
        //    string accountSid = "AC1e12d1c89d9d238bf945b2f9b8ebc6a2";
        //    string authToken = "d9212c43cc269700de08ef643b0d1dab";

        //    Twilio.TwilioClient.Init(accountSid, authToken);

        //    var message = Twilio.Rest.Api.V2010.Account.MessageResource.Create(
        //        body: "Your Hi Car verification code is:" + Code,
        //        from: new Twilio.Types.PhoneNumber("+12086182061"),
        //        to: new Twilio.Types.PhoneNumber("+1" + Phone)
        //    );

        //    return message.Sid;
        //}

        [RelayCommand]
        async Task SelectJobDetails(SchedulesModel model)
        {
            IsEnable = false;
            UserDialogs.Instance.ShowLoading();
            var popupView = new SchedulesViewModel(model);
            var page = new Pages.SchedulePages.ScheduleJobDetailsPage();
            page.BindingContext = popupView;
            await App.Current!.MainPage!.Navigation.PushAsync(page);
            UserDialogs.Instance.HideHud();
            IsEnable = true;
        }

        [RelayCommand]
        async Task OpenImages(SchedulesModel model)
        {
            IsEnable = false;
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
                    //model.GetPictures = true; //In Get Pictures Case Only
                    var popupView = new SchedulesViewModel(model);
                    var page = new Pages.SchedulePages.SchedulePicturesPage();
                    page.BindingContext = popupView;
                    await App.Current!.MainPage!.Navigation.PushAsync(page);
                    UserDialogs.Instance.HideHud();
                }
            }
            catch (Exception ex)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
            }

            IsEnable = true;
        }

        [RelayCommand]
        async Task CreateScheduleInvoice(SchedulesModel model)
        {
            IsEnable = false;
            UserDialogs.Instance.ShowLoading();
            model.InvoiceOrEstimate = 1; //Invoice
            if (model.InvoiceDTO != null)
            {
                var ViewModel = new CustomersViewModel(model.InvoiceDTO, model.CustomerDTO);
                var page = new Pages.CustomerPages.InvoiceDetailsPage();
                page.BindingContext = ViewModel;
                await App.Current!.MainPage!.Navigation.PushAsync(page);
            }
            else
            {
                var popupView = new SchedulesViewModel(model);
                var page = new Pages.SchedulePages.CreateInvoicePage();
                page.BindingContext = popupView;
                await App.Current!.MainPage!.Navigation.PushAsync(page);
            }

            UserDialogs.Instance.HideHud();
            IsEnable = true;
        }

        [RelayCommand]
        async Task CreateScheduleEstimate(SchedulesModel model)
        {
            IsEnable = false;
            UserDialogs.Instance.ShowLoading();

            if (model.EstimateDTO == null)
            {
                model.InvoiceOrEstimate = 0; //Estimate
                var popupView = new SchedulesViewModel(model);
                var page = new Pages.SchedulePages.CreateEstimatePage();
                page.BindingContext = popupView;
                await App.Current!.MainPage!.Navigation.PushAsync(page);
            }
            else
            {
                var popupView = new CustomersViewModel(model.EstimateDTO, model.CustomerDTO);
                var page = new Pages.CustomerPages.EstimateDetailsPage();
                page.BindingContext = popupView;
                await App.Current!.MainPage!.Navigation.PushAsync(page);
            }

            UserDialogs.Instance.HideHud();
            IsEnable = true;
        }

        [RelayCommand]
        void EditDiscountForCustomer(CustomersModel model)
        {
            Discount = model.Discount;
            TotalInvoice(ScheduleDetails, CustomerDetails);
        }

        [RelayCommand]
        void EditDiscountForCustomerEstimate(CustomersModel model)
        {
            Discount = model.Discount;

            TotalEstimate(ScheduleDetails, CustomerDetails);
        }

        [RelayCommand]
        async Task OpenAddImagesPopup(SchedulesModel model)
        {
            IsEnable = false;
            UserDialogs.Instance.ShowLoading();
            var popupView = new SchedulesViewModel(model);
            var page = new Pages.PopupPages.AddSchedulePhotoPupop();
            page.BindingContext = popupView;
            await MopupService.Instance.PushAsync(page);
            UserDialogs.Instance.HideHud();
            IsEnable = true;
        }

        [RelayCommand]
        async Task StartScheduleOutSide(SchedulesModel model)
        {
            IsEnable = false;
            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
                    return;
                }
                else
                {
                    if (model != null)
                    {
                        await GetOneScheduleDetails(model.Id, model.ScheduleDateId);

                        if (ScheduleDetails != null && ScheduleDetails.OneScheduleDate != null)
                        {
                            string UserToken = await _service.UserToken();
                            ScheduleDetails.OneScheduleDate.StartTime = DateTime.Now.TimeOfDay.ToString(@"hh\:mm");
                            ScheduleDetails.OneScheduleDate.Status = 1;

                            UserDialogs.Instance.ShowLoading();

                            var json = await ORep.PutStrAsync("api/Schedules/PutScheduleEmployees", ScheduleDetails.OneScheduleDate, UserToken);

                            if (!string.IsNullOrEmpty(json))
                            {
                                await App.Current!.MainPage!.DisplayAlert("FixPro", "Succes Start The Job.", "Ok");
                                model.ShowCheckBtn = 1;

                                await App.Current!.MainPage!.Navigation.PushAsync(new Pages.SchedulePages.SchedulePage());
                                App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);
                            }
                            else
                            {
                                await App.Current!.MainPage!.DisplayAlert("FixPro", "Failed Start The Job.", "Ok");
                            }
                            UserDialogs.Instance.HideHud();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
            }

            IsEnable = true;
        }

        [RelayCommand]
        async Task EndScheduleOutSide(SchedulesModel model)
        {
            IsEnable = false;
            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
                    return;
                }
                else
                {
                    if (model != null)
                    {
                        await GetOneScheduleDetails(model.Id, model.ScheduleDateId);
                        string UserToken = await _service.UserToken();

                        if (ScheduleDetails != null && ScheduleDetails.OneScheduleDate != null)
                        {
                            ScheduleDetails.OneScheduleDate.EndTime = DateTime.Now.TimeOfDay.ToString(@"hh\:mm");
                            ScheduleDetails.OneScheduleDate.Status = 2;
                            ScheduleDetails.OneScheduleDate.CalendarColor = "#676d75";

                            UserDialogs.Instance.ShowLoading();

                            var json = await ORep.PutStrAsync("api/Schedules/PutScheduleEmployees", ScheduleDetails.OneScheduleDate, UserToken);

                            if (!string.IsNullOrEmpty(json))
                            {
                                await App.Current!.MainPage!.DisplayAlert("FixPro", "Succes End The Job.", "Ok");

                                model.ShowCheckBtn = 2;

                                await App.Current!.MainPage!.Navigation.PushAsync(new Pages.SchedulePages.SchedulePage());
                                App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);
                            }
                            else
                            {
                                await App.Current!.MainPage!.DisplayAlert("FixPro", "Failed End The Job.", "Ok");
                            }
                            UserDialogs.Instance.HideHud();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
            }

            IsEnable = true;
        }

        [RelayCommand]
        async Task AddScheduleDate(SchedulesModel model)
        {
            IsEnable = false;
            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
                    return;
                }
                else
                {
                    if (model != null)
                    {
                        if (TimeFromAddDate != new TimeSpan(00, 0, 0, 00) && TimeToAddDate != new TimeSpan(00, 0, 0, 00) && SelectedEmployeeAddDate != null)
                        {
                            string UserToken = await _service.UserToken();

                            model.OneScheduleDate.Date = ScheduleAddDate.ToString("yyyy-MM-dd");
                            model.Time = model.OneScheduleDate.StartTime = TimeFromAddDate.ToString(@"hh\:mm");
                            model.TimeEnd = model.OneScheduleDate.EndTime = TimeToAddDate.ToString(@"hh\:mm");
                            model.OneScheduleDate.Status = 1;
                            model.CalendarColor = model.OneScheduleDate.CalendarColor = "#5e92e6";
                            //model.LstEmployeeDTO.Clear();
                            //model.LstEmployeeDTO.Add(SelectedEmployeeAddDate);
                            model.OneScheduleDate.OneEmployee = SelectedEmployeeAddDate;
                            //model.LstMaterialReceipt.Clear();
                            //model.LstScheduleItemsServices.Clear();
                            //model.LstSchedulePictures.Clear();
                            //model.Notes = null;


                            model.CreateDate = DateTime.Now;

                            UserDialogs.Instance.ShowLoading();

                            var json = await ORep.PostStrAsync("api/Schedules/PostAddScheduleDate", model, UserToken);

                            if (!string.IsNullOrEmpty(json))
                            {
                                await App.Current!.MainPage!.DisplayAlert("FixPro", "Succes Add Another Date.", "Ok");

                                await App.Current!.MainPage!.Navigation.PopAsync();

                                var VM = new SchedulesViewModel();
                                var page = new Pages.SchedulePages.SchedulePage();
                                page.BindingContext = VM;
                                await App.Current!.MainPage!.Navigation.PushAsync(page);
                                App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);
                                App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);

                                if (model.Id == 0)
                                {
                                    string Massage = $"Hi {model.CustomerName}! Your service appointment with {Helpers.Settings.AccountName} has been scheduled for {DateTime.Parse(model.StartDate).ToString("MMMM dd, yyyy")}. Your technician will arrive between {DateTime.Parse(model.Time).ToString("hh:mmtt")} - {DateTime.Parse(model.TimeEnd).ToString("hh:mmtt")} CDT.";

                                    string returnMsg = SendSMS(ScheduleDetails.CustomerDTO.Phone1, Massage);
                                    if (string.IsNullOrEmpty(returnMsg))
                                    {
                                        await App.Current!.MainPage!.DisplayAlert("Alert", "Succes for Save Schedule but Faild Send SMS to Customer.", "Ok");
                                    }
                                } 
                            }
                            else
                            {
                                await App.Current!.MainPage!.DisplayAlert("FixPro", "Failed for Add Another Date.", "Ok");
                            }
                            UserDialogs.Instance.HideHud();
                        }
                        else
                        {
                            await App.Current!.MainPage!.DisplayAlert("FixPro", "Complete All Fields, please .", "Ok");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
            }

            IsEnable = true;
        }

        [RelayCommand]
        async Task SaveResponNotServiceScheduleDate(SchaduleDateModel model)
        {
            IsEnable = false;
            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
                    return;
                }
                else
                {
                    if (model.Reasonnotserve != null)
                    {
                        string UserToken = await _service.UserToken();
                        model.Status = 0;

                        model.CreateDate = DateTime.Now;

                        model.Reasonnotserve = OldResonNotServiced != null ? (OldResonNotServiced + " , " + model.Reasonnotserve + "-" + DateTime.Now.ToString()) : (model.Reasonnotserve + " - " + DateTime.Now.ToString());

                        UserDialogs.Instance.ShowLoading();
                        //var json = await Helpers.Utility.PutPosData("api/Schedules/PutScheduleDate", JsonConvert.SerializeObject(model));
                        var json = await ORep.PutAsync("api/Schedules/PutScheduleDate", model, UserToken);

                        if (json != null)
                        {
                            await App.Current!.MainPage!.DisplayAlert("FixPro", "Save Respon Not Service.", "Ok");
                            await App.Current!.MainPage!.Navigation.PopAsync();
                            var VM = new SchedulesViewModel(ScheduleDetails.Id, model.Id);
                            var page = new Pages.SchedulePages.ScheduleDetailsPage();
                            page.BindingContext = VM;
                            await App.Current!.MainPage!.Navigation.PushAsync(page);
                            App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);
                            App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);
                        }
                        UserDialogs.Instance.HideHud();
                    }
                    else
                    {
                        await App.Current!.MainPage!.DisplayAlert("FixPro", "Enter Respon Not Service, please .", "Ok");
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
            }

            IsEnable = true;
        }

        [RelayCommand]
        async Task SaveReOpenScheduleDate(SchaduleDateModel model)
        {
            IsEnable = false;
            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
                    return;
                }
                else
                {
                    if (model != null)
                    {
                        string UserToken = await _service.UserToken();
                        model.Status = 1;

                        UserDialogs.Instance.ShowLoading();
                        //var json = await Helpers.Utility.PutPosData("api/Schedules/PutScheduleDate", JsonConvert.SerializeObject(model));
                        var json = await ORep.PutAsync("api/Schedules/PutScheduleDate", model, UserToken);

                        if (json != null)
                        {
                            await App.Current!.MainPage!.DisplayAlert("FixPro", "Success Re Open Service.", "Ok");
                            await App.Current!.MainPage!.Navigation.PopAsync();
                            var VM = new SchedulesViewModel(ScheduleDetails.Id, model.Id);
                            var page = new Pages.SchedulePages.ScheduleDetailsPage();
                            page.BindingContext = VM;
                            await App.Current!.MainPage!.Navigation.PushAsync(page);
                            App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);
                            App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);
                        }
                        UserDialogs.Instance.HideHud();
                    }
                    else
                    {
                        await App.Current!.MainPage!.DisplayAlert("FixPro", "This Schedule Not found.", "Ok");
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
            }

            IsEnable = true;
        }

        [RelayCommand]
        async Task DoneScheduleDate(SchaduleDateModel model)
        {
            IsEnable = false;

            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
                    return;
                }
                else
                {
                    if (model != null)
                    {
                        //if (!string.IsNullOrEmpty(StartTime) && !string.IsNullOrEmpty(EndTime))
                        //{     
                        //model.StartTime = StartTime;
                        //model.EndTime = EndTime;
                        //model.SpentTimeHour = SpentHours;
                        //model.SpentTimeMin = SpentMin;
                        string UserToken = await _service.UserToken();
                        model.Status = 2;
                        model.CalendarColor = "#676d75";

                        model.CreateDate = DateTime.Now;

                        UserDialogs.Instance.ShowLoading();
                        //var json = await Helpers.Utility.PutPosData("api/Schedules/PutScheduleDate", JsonConvert.SerializeObject(model));
                        var json = await ORep.PutStrAsync("api/Schedules/PutScheduleDate", model, UserToken);

                        if (!string.IsNullOrEmpty(json) && json.Contains("Not Done All Employee") == true)
                        {
                            await App.Current!.MainPage!.DisplayAlert("FixPro", "Failed Job Done because find employee is not finished.", "Ok");
                        }
                        else if (!string.IsNullOrEmpty(json))
                        {
                            bool answer = await App.Current!.MainPage!.DisplayAlert("Question?", "Do you want send massage to customer?", "Yes", "No");
                            if (answer)
                            {
                                string Massage = $"Hello {model.CustomerName}, thank you for choosing us. We hope your experience was satisfactory. Your feedback means a lot to us! Please consider leaving a Google review here: {model.GoogleReviewLink}. Have a great day!";

                                string returnMsg = SendSMS(model.CustomerPhone, Massage);
                                if (string.IsNullOrEmpty(returnMsg))
                                {
                                    await App.Current!.MainPage!.DisplayAlert("Alert", "Succes for Done Schedule but Faild Send SMS to Customer.", "Ok");
                                }
                            }

                            await App.Current!.MainPage!.DisplayAlert("FixPro", "Succes for End schedule Date.", "Ok");
                            await App.Current!.MainPage!.Navigation.PopAsync();
                            var VM = new SchedulesViewModel(ScheduleDetails.Id, model.Id);
                            var page = new Pages.SchedulePages.ScheduleDetailsPage();
                            page.BindingContext = VM;
                            await App.Current!.MainPage!.Navigation.PushAsync(page);
                            App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);
                            App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);
                        }
                        else
                        {
                            await App.Current!.MainPage!.DisplayAlert("FixPro", "Failed for End schedule Date.", "Ok");
                        }
                        UserDialogs.Instance.HideHud();
                        //}
                        //else
                        //{
                        //    await App.Current!.MainPage!.DisplayAlert("FixPro", "Select start and end date to check out, please .", "Ok");
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
                //throw;
            }

            IsEnable = true;
        }

        async Task UploadPictures(List<SchedulePicturesModel> LstPhotos)
        {
            IsEnable = false;

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                UserDialogs.Instance.ShowLoading();

                List<SchedulePicturesModel> LstFinalPhotos = LstPhotos.Where(x => x.Id == 0).ToList(); // if Id = 0 (Photo New)

                foreach (var source in LstFinalPhotos)
                {
                    if (source.PictureSource != null)
                        source.PictureSource = null;
                }
                //string Postjson = await Helpers.Utility.PostData("api/ImageMobile/ReplacePostOneImagesScheduleMobile", JsonConvert.SerializeObject(LstFinalPhotos, Formatting.None,
                //            new JsonSerializerSettings()
                //            {
                //                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                //            }));

                string UserToken = await _service.UserToken();
                string Postjson = await ORep.PostMultiPicAsync("api/ImageMobile/ReplacePostOneImagesScheduleMobile", LstFinalPhotos, UserToken);

                UserDialogs.Instance.HideHud();
            }

            IsEnable = true;
        }

        //Pick Photo
        [RelayCommand]
        private async Task SelectePickSchedulePhoto()
        {
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

                        var imageBytes = memoryStream.ToArray();

                        SchedulePhoto = ImageSource.FromStream(() =>
                        {
                            // Return a new MemoryStream each time, so it remains accessible across different UI contexts
                            return new MemoryStream(imageBytes);
                        });

                        OnePictureModel = new SchedulePicturesModel
                        {
                            AccountId = ScheduleDetails.AccountId,
                            BrancheId = ScheduleDetails.BrancheId,
                            ScheduleId = ScheduleDetails.Id,
                            FileName = Convert.ToBase64String(memoryStream.ToArray()),
                            Active = true,
                            ShowToCust = true,
                            CreateUser = ScheduleDetails.CreateUser,
                            CreateDate = DateTime.Now,
                            ScheduleDateId = ScheduleDetails.OneScheduleDate.Id,
                            PictureSource = SchedulePhoto,
                            Flag = 0, // new photo
                        };

                        LstNewPictures.Add(OnePictureModel);
                        LstAllPictures.Add(OnePictureModel);
                        ScheduleDetails.LstSchedulePictures.Add(OnePictureModel);
                        ScheduleDetails.GetPictures = false; //Don't entrance GetPictures Method

                        UserDialogs.Instance.ShowLoading();

                        var popupView = new SchedulesViewModel(ScheduleDetails);
                        var page = new Pages.SchedulePages.SchedulePicturesPage();
                        page.BindingContext = popupView;
                        await App.Current!.MainPage!.Navigation.PushAsync(page);

                        App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);

                        DoneFlag = true;

                        UserDialogs.Instance.HideHud();
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
        private async Task SelecteCamSchedulePhoto()
        {

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

                            var imageBytes = memoryStream.ToArray();

                            SchedulePhoto = ImageSource.FromStream(() =>
                            {
                                // Return a new MemoryStream each time, so it remains accessible across different UI contexts
                                return new MemoryStream(imageBytes);
                            });

                            OnePictureModel = new SchedulePicturesModel
                            {
                                AccountId = ScheduleDetails.AccountId,
                                BrancheId = ScheduleDetails.BrancheId,
                                ScheduleId = ScheduleDetails.Id,
                                FileName = Convert.ToBase64String(memoryStream.ToArray()),
                                Active = true,
                                ShowToCust = true,
                                CreateUser = ScheduleDetails.CreateUser,
                                CreateDate = DateTime.Now,
                                ScheduleDateId = ScheduleDetails.OneScheduleDate.Id,
                                PictureSource = SchedulePhoto,
                                Flag = 0, // new photo
                            };

                            LstNewPictures.Add(OnePictureModel);
                            LstAllPictures.Add(OnePictureModel);
                            ScheduleDetails.LstSchedulePictures.Add(OnePictureModel);
                            ScheduleDetails.GetPictures = false; //Don't entrance GetPictures Method

                            UserDialogs.Instance.ShowLoading();

                            var popupView = new SchedulesViewModel(ScheduleDetails);
                            var page = new Pages.SchedulePages.SchedulePicturesPage();
                            page.BindingContext = popupView;
                            await App.Current!.MainPage!.Navigation.PushAsync(page);
                            App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);

                            DoneFlag = true;

                            UserDialogs.Instance.HideHud();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                await App.Current!.MainPage!.DisplayAlert("No Camera", ":( No camera available.", "Ok");
            }

        }

        [RelayCommand]
        async Task DeleteSchedulePhoto(SchedulePicturesModel model)
        {
            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
                    return;
                }
                else
                {
                    if (model.Id == 0) //Id = 0 (Photo New)
                    {
                        LstNewPictures.Remove(model);
                        LstAllPictures.Remove(model);
                        ScheduleDetails.LstSchedulePictures.Remove(model);
                        LstATwoPictures.Remove(model);
                    }
                    else //Id != 0 (already Photo save)
                    {
                        UserDialogs.Instance.ShowLoading();


                        string UserToken = await _service.UserToken();
                        //var json = await Helpers.Utility.DeleteData("api/ImageMobile/DeleteOneImage", JsonConvert.SerializeObject(ModelWhitOutImageSource));
                        var json = await ORep.DeleteStrItemAsync(string.Format("api/ImageMobile/DeleteOneImage/{0}", model.Id), UserToken);

                        if (json != null && json != "api not responding")
                        {
                            LstAllPictures.Remove(model);
                            ScheduleDetails.LstSchedulePictures.Remove(model);
                        }
                        UserDialogs.Instance.HideHud();
                    }

                    List<SchedulePicturesModel> NewImg = ScheduleDetails.LstSchedulePictures.Where(x => x.Id == 0).ToList();
                    if (NewImg.Count > 0)
                    {
                        DoneFlag = true;
                    }

                    else
                    {
                        DoneFlag = false;
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        async Task CalcSchPhotoCount(int Count)
        {
            int Co = Count - 2;
            PhotosCount = 0;
            if (Co > 0)
            {
                PhotosCount = Co;
            }
        }

        [RelayCommand]
        async Task DonePictures(SchedulesModel model)
        {
            IsEnable = false;

            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
                    return;
                }
                else
                {
                    if (model != null)
                    {
                        await UploadPictures(model.LstSchedulePictures);

                        SetLstTwoSchedulePhotos(model.LstSchedulePictures);

                        //PhotosCount = model.LstSchedulePictures.Count-2;
                        await CalcSchPhotoCount(model.LstSchedulePictures.Count);

                        await App.Current!.MainPage!.DisplayAlert("FixPro", "Succes for add schedule pictures.", "Ok");

                        UserDialogs.Instance.ShowLoading();
                        var popupView = new SchedulesViewModel(model.Id, model.OneScheduleDate.Id);
                        var page = new Pages.SchedulePages.ScheduleDetailsPage();
                        page.BindingContext = popupView;
                        await App.Current!.MainPage!.Navigation.PushAsync(page);
                        App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);
                        App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);
                        UserDialogs.Instance.HideHud();

                        //await App.Current!.MainPage!.Navigation.PopAsync();
                    }
                    else
                    {
                        await App.Current!.MainPage!.DisplayAlert("Alert", "Please Choose Photos.", "Ok");
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
            }

            IsEnable = true;
        }

        [RelayCommand]
        public void SetLstTwoSchedulePhotos(List<SchedulePicturesModel> LstPhotos)
        {
            if (LstPhotos != null && LstPhotos.Count > 0)
            {
                ObservableCollection<SchedulePicturesModel> lstPictures = new ObservableCollection<SchedulePicturesModel>(LstPhotos);

                if (lstPictures.Count > 0)
                {
                    if (lstPictures.Count > 0 && lstPictures.Count < 2)
                    {
                        LstATwoPictures.Add(LstPhotos[0]);
                    }
                    else if (lstPictures.Count >= 2)
                    {
                        LstATwoPictures.Add(LstPhotos[0]);
                        LstATwoPictures.Add(LstPhotos[1]);
                    }
                    else
                    {
                        LstATwoPictures.Add(LstPhotos[0]);
                        LstATwoPictures.Add(LstPhotos[1]);
                    }

                }
            }
        }


        [RelayCommand]
        async Task SubmitSchInvoiceOrEstimate(SchedulesModel model)
        {
            IsEnable = false;

            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await App.Current!.MainPage!.DisplayAlert("Error", "No Internet connection!", "OK");
                    return;
                }
                else
                {
                    if (model != null)
                    {
                        if (model.InvoiceOrEstimate == 0)//Insert Estimate InvoiceOrEstimate == 0 (Schedule Page)
                        {
                            if (LstItemsEstimate.Count > 0)
                            {
                                string UserToken = await _service.UserToken();

                                if (LstEstimateSchaduleDatesActual.Count > 0)
                                {
                                    if (Pending == true || Accept == true || Declind == true)
                                    {

                                        OneEstimate.AccountId = model.AccountId;
                                        OneEstimate.BrancheId = model.BrancheId;
                                        OneEstimate.ScheduleId = model.Id;
                                        OneEstimate.EstimateDate = DateTime.Now;
                                        OneEstimate.CustomerId = model.CustomerId;
                                        OneEstimate.Total = SubTotalEst; //Total before discount and tax
                                        OneEstimate.TaxId = model.CustomerDTO.TaxId;
                                        OneEstimate.Tax = model.CustomerDTO.TaxDTO?.Rate;
                                        OneEstimate.Taxval = null;
                                        OneEstimate.SignatureDraw = SignatureImageByte64Estimate;
                                        //OneEstimate.Taxval = (model.CustomerDTO != null && model.CustomerDTO.MemeberType == false && model.CustomerDTO.TaxDTO != null) ? (SubTotal - (SubTotal * model.CustomerDTO.MemberDTO.MemberValue / 100) * model.CustomerDTO.TaxDTO.Rate / 100) : (model.CustomerDTO != null && model.CustomerDTO.TaxDTO != null && model.CustomerDTO.MemeberType == true && model.CustomerDTO.TaxDTO != null) ? ((SubTotal - model.CustomerDTO.Discount) * model.CustomerDTO.TaxDTO.Rate / 100) : 0;
                                        OneEstimate.MemberId = model.CustomerDTO.MemeberId;
                                        OneEstimate.Discount = Discount;
                                        //OneEstimate.DiscountAmountOrPercent = model.CustomerDTO.MemberDTO.MemberType == false ? "%" : "$";
                                        OneEstimate.DiscountAmountOrPercent = AmountOrPersent == false ? "%" : "$";
                                        OneEstimate.Net = NetEst;
                                        OneEstimate.Status = Accept == true ? 1 : Declind == true ? 2 : 0; //0 = Pending
                                        OneEstimate.SignaturePrintName = null;
                                        OneEstimate.Terms = null;
                                        OneEstimate.NotesForCustomer = model.CustomerDTO.Notes;
                                        OneEstimate.Notes = model.Notes;
                                        OneEstimate.Active = model.Active;
                                        OneEstimate.CreateUser = int.Parse(Helpers.Settings.UserIdGet);
                                        OneEstimate.CreateDate = DateTime.Now;

                                        foreach (ScheduleItemsServicesModel item in LstItemsEstimate)
                                        {
                                            EstimateItemServicesModel ObjItem = new EstimateItemServicesModel
                                            {
                                                Id = item.Id,
                                                AccountId = model.AccountId,
                                                BrancheId = model.BrancheId,
                                                //TaxId = model.CustomerDTO.TaxId,
                                                //Tax = model.CustomerDTO.TaxDTO.Rate,
                                                //Taxable = (model.CustomerDTO.TaxDTO.Rate == null || model.CustomerDTO.TaxDTO.Rate == 0) ? false : true,
                                                Taxable = true,
                                                //Unit = item.Unit,
                                                Price = item.CostRate,
                                                Quantity = item.Quantity,
                                                //Discountable = model.CustomerDTO.MemberDTO.MemberValue != null ? true : false,
                                                Discountable = true,
                                                ItemsServicesId = item.ItemsServicesId,
                                                ItemsServicesName = item.ItemsServicesName,
                                                CreateUser = int.Parse(Helpers.Settings.UserIdGet),
                                                CreateDate = DateTime.Now,
                                                Total = item.Quantity != null && item.Tax != null ? (item.CostRate * item.Quantity) + (item.CostRate * item.Quantity * item.Tax / 100) : item.Quantity == null && item.Tax != null ? item.CostRate + (item.CostRate * item.Quantity * item.Tax / 100) : item.Quantity != null && item.Tax == null ? item.CostRate * item.Quantity : item.CostRate,
                                                Active = model.Active,
                                            };
                                            OneEstimate.LstEstimateItemServices.Add(ObjItem);
                                        }

                                        OneEstimate.LstScdDate = LstEstimateSchaduleDatesActual.ToList();

                                        UserDialogs.Instance.ShowLoading();
                                        //var json = await Helpers.Utility.PostData("api/Estimates/PostEstimate", JsonConvert.SerializeObject(OneEstimate));
                                        var json = await ORep.PostDataAsync("api/Estimates/PostEstimate", OneEstimate, UserToken);
                                        UserDialogs.Instance.HideHud();

                                        if (json != "Bad Request" && json != "api not responding" && json.Contains("Already Exist For This Schedule Date#") != true)
                                        {
                                            //await App.Current!.MainPage!.DisplayAlert("FixPro", "Success Save Estimate.", "Ok");
                                            //Helpers.Messages.ShowSuccessSnackBar("Success Create Estimate.");
                                            var toast = Toast.Make("Success Create Estimate.", CommunityToolkit.Maui.Core.ToastDuration.Long, 15);
                                            await toast.Show();

                                            bool answer = await App.Current!.MainPage!.DisplayAlert("Question?", "Do you want to send an email to the customer?", "Yes", "No");

                                            if (answer)//Send Email
                                            {

                                                UserDialogs.Instance.ShowLoading();
                                                var jsonEmail = await ORep.PostStrAsync("api/Estimates/PostEstimateEmail", OneEstimate, UserToken);
                                                UserDialogs.Instance.HideHud();

                                                if (!string.IsNullOrEmpty(jsonEmail) && jsonEmail.Contains("Send Success") == true)
                                                {
                                                    //Helpers.Messages.ShowSuccessSnackBar("Success Send Email to Customer.");
                                                    var toast1 = Toast.Make("Success Send Email to Customer.", CommunityToolkit.Maui.Core.ToastDuration.Long, 15);
                                                    await toast.Show();
                                                    //await App.Current!.MainPage!.DisplayAlert("FixPro", "Email sent successfully to customer", "Ok");
                                                }
                                                else
                                                {
                                                    //Helpers.Messages.ShowSuccessSnackBar("Failed to send e-mail to the customer");

                                                    var toast1 = Toast.Make("Failed to send e-mail to the customer", CommunityToolkit.Maui.Core.ToastDuration.Long, 15);
                                                    await toast.Show();
                                                    //await App.Current!.MainPage!.DisplayAlert("FixPro", "Failed to send e-mail to the customer", "Ok");
                                                }
                                            }

                                            //await App.Current!.MainPage!.Navigation.PushAsync(new MainPage());
                                            var VM = new SchedulesViewModel(model.Id, model.ScheduleDateId);
                                            //var page = new NewSchedulePage();
                                            var page = new Pages.SchedulePages.ScheduleDetailsPage();
                                            page.BindingContext = VM;
                                            await App.Current!.MainPage!.Navigation.PushAsync(page);
                                            App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);
                                        }
                                        else
                                        {
                                            await App.Current!.MainPage!.DisplayAlert("Alert", json, "Ok");
                                        }

                                    }
                                    else
                                    {
                                        await App.Current!.MainPage!.DisplayAlert("Alert", "Please Choose Status for Estimate.", "Ok");
                                    }
                                }
                                else
                                {
                                    await App.Current!.MainPage!.DisplayAlert("Alert", "No schedule dates chosen for this estimate!", "Ok");
                                }
                            }
                            else
                            {
                                await App.Current!.MainPage!.DisplayAlert("Alert", "No item/service chosen for this estimate!", "Ok");
                            }

                        }
                        else //Insert Invoice InvoiceOrEstimate == 1 (Schedule Page)
                        {
                            if (LstItemsInvoice.Count > 0)
                            {
                                string UserToken = await _service.UserToken();

                                if (LstInvoiceSchaduleDatesActual.Count > 0)
                                {
                                    var CheckItemoutFalse = LstItemsInvoice.Where(m => m.Out == false).FirstOrDefault();
                                    if (CheckItemoutFalse != null)
                                    {
                                        OneInvoice.AccountId = model.AccountId;
                                        OneInvoice.BrancheId = model.BrancheId;
                                        OneInvoice.ContractId = model.ContractId;
                                        OneInvoice.ScheduleId = model.Id;
                                        OneInvoice.InvoiceDate = DateTime.Now;
                                        OneInvoice.CustomerId = model.CustomerId;
                                        OneInvoice.Total = SubTotal;
                                        OneInvoice.TaxId = model.CustomerDTO.TaxId;
                                        OneInvoice.Tax = model.CustomerDTO.TaxDTO?.Rate;
                                        //OneInvoice.Taxval = (SubTotal - (SubTotal * model.CustomerDTO.MemberDTO.MemberValue / 100)) * model.CustomerDTO.TaxDTO.Rate / 100;
                                        //OneInvoice.Taxval = (model.CustomerDTO != null && model.CustomerDTO.MemeberType == false && model.CustomerDTO.TaxDTO != null) ? (SubTotal - (SubTotal * model.CustomerDTO.MemberDTO.MemberValue / 100) * model.CustomerDTO.TaxDTO.Rate / 100) : (model.CustomerDTO != null && model.CustomerDTO.TaxDTO != null && model.CustomerDTO.MemeberType == true && model.CustomerDTO.TaxDTO != null) ? ((SubTotal - model.CustomerDTO.Discount) * model.CustomerDTO.TaxDTO.Rate / 100) : 0;
                                        OneInvoice.Taxval = null;
                                        OneInvoice.MemberId = model.CustomerDTO.MemeberId;
                                        OneInvoice.Discount = Discount;
                                        //OneInvoice.DiscountAmountOrPercent = model.CustomerDTO.MemberDTO.MemberType == false ? "%" : "$";
                                        OneInvoice.DiscountAmountOrPercent = AmountOrPersent == false ? "%" : "$";
                                        OneInvoice.Paid = 0;
                                        OneInvoice.Net = Net;
                                        OneInvoice.Status = 0; //Draft status if(1=partail & 2=paid)
                                        OneInvoice.Type = 2; //Installment Payment type
                                        OneInvoice.SignaturePrintName = null;
                                        OneInvoice.SignatureDraw = null;
                                        OneInvoice.Terms = null;
                                        OneInvoice.NotesForCustomer = model.CustomerDTO.Notes;
                                        OneInvoice.Notes = model.Notes;
                                        OneInvoice.Active = model.Active;
                                        OneInvoice.CreateUser = int.Parse(Helpers.Settings.UserIdGet);
                                        OneInvoice.CreateDate = DateTime.Now;

                                        foreach (ScheduleItemsServicesModel item in LstItemsInvoice)
                                        {
                                            InvoiceItemServicesModel ObjItem = new InvoiceItemServicesModel
                                            {
                                                Id = item.Id,
                                                AccountId = model.AccountId,
                                                BrancheId = model.BrancheId,
                                                ItemServiceDescription = item.ItemServiceDescription,
                                                //TaxId = model.CustomerDTO.TaxId,
                                                //Tax = model.CustomerDTO.TaxDTO.Rate,
                                                //Taxable = (model.CustomerDTO.TaxDTO.Rate == null || model.CustomerDTO.TaxDTO.Rate == 0) ? false : true,
                                                Taxable = true,
                                                //Unit = item.Unit,
                                                Price = item.CostRate,
                                                Quantity = item.Quantity,
                                                //Discountable = model.CustomerDTO.MemberDTO.MemberValue != null ? true : false,
                                                Discountable = true,
                                                ItemsServicesId = item.ItemsServicesId,
                                                ItemsServicesName = item.ItemsServicesName,
                                                CreateUser = int.Parse(Helpers.Settings.UserIdGet),
                                                CreateDate = DateTime.Now,
                                                SkipOfTotal = item.Out,
                                                Total = item.Quantity != null && item.Tax != null ? (item.CostRate * item.Quantity) + (item.CostRate * item.Quantity * item.Tax / 100) : item.Quantity == null && item.Tax != null ? item.CostRate + (item.CostRate * item.Quantity * item.Tax / 100) : item.Quantity != null && item.Tax == null ? item.CostRate * item.Quantity : item.CostRate,
                                                Active = model.Active,
                                            };
                                            OneInvoice.LstInvoiceItemServices.Add(ObjItem);
                                        }

                                        OneInvoice.LstScdDate = LstInvoiceSchaduleDatesActual.ToList();

                                        UserDialogs.Instance.ShowLoading();
                                        //var json = await Helpers.Utility.PostMData("api/Invoices/PostInvoice", JsonConvert.SerializeObject(OneInvoice));
                                        var json = await ORep.PostDataAsync("api/Invoices/PostInvoice", OneInvoice, UserToken);
                                        UserDialogs.Instance.HideHud();

                                        if (json != "Bad Request" && json != "api not responding" && json.Contains("Not_Enough") != true && json.Contains("This Invoice Already Exist") != true)
                                        {
                                            //await App.Current!.MainPage!.DisplayAlert("FixPro", "Success Create Invoice for this Job.", "Ok");
                                            //Helpers.Messages.ShowSuccessSnackBar("Success Create Invoice for this Job.");
                                            var toast = Toast.Make("Success Create Invoice for this Job.", CommunityToolkit.Maui.Core.ToastDuration.Long, 15);
                                            await toast.Show();

                                            bool answer = await App.Current!.MainPage!.DisplayAlert("Question?", "Do you want to send an email to the customer?", "Yes", "No");

                                            if (answer)//Send Email
                                            {
                                                UserDialogs.Instance.ShowLoading();
                                                var jsonEmail = await ORep.PostStrAsync("api/Invoices/PostInvoiceEmail", OneInvoice, UserToken);
                                                UserDialogs.Instance.HideHud();

                                                if (!string.IsNullOrEmpty(jsonEmail) && jsonEmail.Contains("Send Success") == true)
                                                {
                                                    //Helpers.Messages.ShowSuccessSnackBar("Success Send Email to Customer.");
                                                    var toast1 = Toast.Make("Success Send Email to Customer.", CommunityToolkit.Maui.Core.ToastDuration.Long, 15);
                                                    await toast.Show();
                                                    //await App.Current!.MainPage!.DisplayAlert("FixPro", "Email sent successfully to customer", "Ok");
                                                }
                                                else
                                                {
                                                    //Helpers.Messages.ShowSuccessSnackBar("Failed to send e-mail to the customer");
                                                    var toast1 = Toast.Make("Failed to send e-mail to the customer", CommunityToolkit.Maui.Core.ToastDuration.Long, 15);
                                                    await toast.Show();
                                                    //await App.Current!.MainPage!.DisplayAlert("FixPro", "Failed to send e-mail to the customer", "Ok");
                                                }
                                            }

                                            if (OneInvoice.Net > 0)
                                            {
                                                OneInvoice.Id = int.Parse(json.Replace("\"", "").Trim());
                                                var ViewModel = new SchedulesViewModel(OneInvoice, CustomerDetails);
                                                var page = new Pages.PopupPages.PaymentMethodsPopup();
                                                page.BindingContext = ViewModel;
                                                await MopupService.Instance.PushAsync(page);
                                                //await App.Current!.MainPage!.Navigation.PushAsync(new MainPage());
                                            }
                                            else
                                            {
                                                var VM = new SchedulesViewModel(model.Id, model.ScheduleDateId);
                                                //var page = new NewSchedulePage();
                                                var page = new ScheduleDetailsPage();
                                                page.BindingContext = VM;
                                                await App.Current!.MainPage!.Navigation.PushAsync(page);
                                                App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);
                                            }


                                        }
                                        else
                                        {
                                            await App.Current!.MainPage!.DisplayAlert("Alert", json, "Ok");
                                            //await App.Current!.MainPage!.Navigation.PushAsync(new MainPage());
                                        }
                                    }
                                    else
                                    {
                                        await App.Current!.MainPage!.DisplayAlert("Alert", "Please don’t check all the items/services out for this invoice", "Ok");
                                    }
                                }
                                else
                                {
                                    await App.Current!.MainPage!.DisplayAlert("Alert", "No schedule dates chosen for this invoice!", "Ok");
                                }

                            }
                            else
                            {
                                await App.Current!.MainPage!.DisplayAlert("Alert", "No item/service chosen for this invoice", "Ok");
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
            }

            IsEnable = true;
        }


        [RelayCommand]
        async Task SubmitCustInvoiceOrEstimate(CustomersModel model)
        {
            IsEnable = false;

            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await App.Current!.MainPage!.DisplayAlert("Error", "No Internet connection!", "OK");
                    return;
                }
                else
                {
                    string UserToken = await _service.UserToken();

                    if (model != null)
                    {
                        if (Controls.StaticMembers.WayAfterChooseCust == 1)//Insert Estimate WayAfterChooseCust == 0 (Customer Page)
                        {
                            if (LstItemsEstimate.Count > 0)
                            {
                                if (Pending == true || Accept == true || Declind == true)
                                {

                                    OneEstimate.AccountId = model.AccountId;
                                    OneEstimate.BrancheId = model.BrancheId;
                                    //OneEstimate.ScheduleId = model.Id;
                                    //OneEstimate.ScheduleDateId = model.OneScheduleDate.Id;
                                    OneEstimate.EstimateDate = DateTime.Now;
                                    OneEstimate.CustomerId = model.Id;
                                    OneEstimate.Total = SubTotalEst;
                                    OneEstimate.TaxId = model.TaxId;
                                    OneEstimate.Tax = model.TaxDTO?.Rate;
                                    //OneEstimate.Taxval = (SubTotal - (SubTotal * model.MemberDTO.MemberValue / 100)) * model.TaxDTO.Rate / 100;
                                    //OneEstimate.Taxval = (model.MemeberType == false && model.TaxDTO != null) ? (SubTotal - (SubTotal * model.MemberDTO.MemberValue / 100) * model.TaxDTO.Rate / 100) : (model.TaxDTO != null && model.MemeberType == true && model.TaxDTO != null) ? ((SubTotal - model.Discount) * model.TaxDTO.Rate / 100) : 0;
                                    OneEstimate.Taxval = null;
                                    OneEstimate.MemberId = model.MemeberId;
                                    OneEstimate.Discount = Discount;
                                    OneEstimate.SignatureDraw = SignatureImageByte64Estimate;
                                    //OneEstimate.DiscountAmountOrPercent = model.MemberDTO.MemberType == false ? "%" : "$";
                                    OneEstimate.DiscountAmountOrPercent = AmountOrPersent == false ? "%" : "$";
                                    OneEstimate.Net = NetEst;
                                    OneEstimate.Status = Accept == true ? 1 : Declind == true ? 2 : 0; //0 = Pending
                                    OneEstimate.SignaturePrintName = null;
                                    OneEstimate.Terms = null;
                                    OneEstimate.NotesForCustomer = model.Notes;
                                    //OneEstimate.Notes = model.Notes;
                                    OneEstimate.Active = model.Active;
                                    OneEstimate.CreateUser = int.Parse(Helpers.Settings.UserIdGet);
                                    OneEstimate.CreateDate = DateTime.Now;

                                    foreach (ScheduleItemsServicesModel item in LstItemsEstimate)
                                    {
                                        EstimateItemServicesModel ObjItem = new EstimateItemServicesModel
                                        {
                                            Id = item.Id,
                                            AccountId = model.AccountId,
                                            BrancheId = model.BrancheId,
                                            //TaxId = model.TaxId,
                                            //Tax = model.TaxDTO.Rate,
                                            //Taxable = (model.TaxDTO.Rate == null || model.TaxDTO.Rate == 0) ? false : true,
                                            Taxable = true,
                                            //Unit = item.Unit,
                                            Price = item.CostRate,
                                            Quantity = item.Quantity,
                                            //Discountable = model.MemberDTO.MemberValue != null ? true : false,
                                            Discountable = true,
                                            ItemsServicesId = item.ItemsServicesId,
                                            ItemsServicesName = item.ItemsServicesName,
                                            CreateUser = int.Parse(Helpers.Settings.UserIdGet),
                                            CreateDate = DateTime.Now,
                                            Total = item.Quantity != null && item.Tax != null ? (item.CostRate * item.Quantity) + (item.CostRate * item.Quantity * item.Tax / 100) : item.Quantity == null && item.Tax != null ? item.CostRate + (item.CostRate * item.Quantity * item.Tax / 100) : item.Quantity != null && item.Tax == null ? item.CostRate * item.Quantity : item.CostRate,
                                            Active = model.Active,
                                        };
                                        OneEstimate.LstEstimateItemServices.Add(ObjItem);
                                    }

                                    UserDialogs.Instance.ShowLoading();
                                    //var json = await Helpers.Utility.PostData("api/Estimates/PostEstimate", JsonConvert.SerializeObject(OneEstimate));
                                    var json = await ORep.PostDataAsync("api/Estimates/PostEstimate", OneEstimate, UserToken);
                                    UserDialogs.Instance.HideHud();

                                    if (json != "Bad Request" && json != "api not responding" && json.Contains("Already Exist For This Schedule Date#") != true)
                                    {
                                        //await App.Current!.MainPage!.DisplayAlert("FixPro", "Success Create Estimate for This Job.", "Ok");

                                        //Helpers.Messages.ShowSuccessSnackBar("Success Create Estimate for This Job.");
                                        var toast = Toast.Make("Success Create Estimate for This Job.", CommunityToolkit.Maui.Core.ToastDuration.Long, 15);
                                        await toast.Show();

                                        bool answer = await App.Current!.MainPage!.DisplayAlert("Question?", "Do you want to send an email to the customer?", "Yes", "No");

                                        if (answer)//Send Email
                                        {
                                            UserDialogs.Instance.ShowLoading();
                                            var jsonEmail = await ORep.PostStrAsync("api/Estimates/PostEstimateEmail", OneEstimate, UserToken);
                                            UserDialogs.Instance.HideHud();

                                            if (!string.IsNullOrEmpty(jsonEmail) && jsonEmail.Contains("Send Success") == true)
                                            {
                                                //Helpers.Messages.ShowSuccessSnackBar("Success Send Email to Customer.");
                                                var toast1 = Toast.Make("Success Send Email to Customer.", CommunityToolkit.Maui.Core.ToastDuration.Long, 15);
                                                await toast1.Show();
                                                //await App.Current!.MainPage!.DisplayAlert("FixPro", "Email sent successfully to customer", "Ok");
                                            }
                                            else
                                            {
                                                //Helpers.Messages.ShowSuccessSnackBar("Failed to send e-mail to the customer");
                                                var toast1 = Toast.Make("Failed to send e-mail to the customer", CommunityToolkit.Maui.Core.ToastDuration.Long, 15);
                                                await toast1.Show();
                                                //await App.Current!.MainPage!.DisplayAlert("FixPro", "Failed to send e-mail to the customer", "Ok");
                                            }
                                        }

                                        var ViewModel = new CustomersViewModel(CustomerDetails);
                                        var page = new Pages.CustomerPages.CustomersDetailsPage();
                                        page.BindingContext = ViewModel;
                                        await App.Current!.MainPage!.Navigation.PushAsync(page);

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
                                        //await App.Current!.MainPage!.DisplayAlert("Alert", "Faild Create Estimate.", "Ok");
                                    }


                                }
                                else
                                {
                                    await App.Current!.MainPage!.DisplayAlert("Alert", "Please Choose Status for Estimate.", "Ok");
                                }
                            }
                            else
                            {
                                await App.Current!.MainPage!.DisplayAlert("Alert", "No item/service chosen for this estimate!", "Ok");
                            }
                        }
                        else //Insert Invoice WayAfterChooseCust == 2 (Customer Page)
                        {
                            if (LstItemsInvoice.Count > 0)
                            {
                                var CheckItemoutFalse = LstItemsInvoice.Where(m => m.Out == false).FirstOrDefault();
                                if (CheckItemoutFalse != null)
                                {
                                    OneInvoice.AccountId = model.AccountId;
                                    OneInvoice.BrancheId = model.BrancheId;
                                    //OneInvoice.ContractId = model.ContractId;
                                    //OneInvoice.ScheduleDateId = model.OneScheduleDate.Id;
                                    //OneInvoice.ScheduleId = model.Id;
                                    OneInvoice.InvoiceDate = DateTime.Now;
                                    OneInvoice.CustomerId = model.Id;
                                    OneInvoice.Total = SubTotal;
                                    OneInvoice.TaxId = model.TaxId;
                                    OneInvoice.Tax = model.TaxDTO?.Rate;
                                    //OneInvoice.Taxval = (SubTotal - (SubTotal * model.MemberDTO.MemberValue / 100)) * model.TaxDTO.Rate / 100;
                                    //OneInvoice.Taxval = (model.MemeberType == false && model.TaxDTO != null) ? (SubTotal - (SubTotal * model.MemberDTO.MemberValue / 100) * model.TaxDTO.Rate / 100) : (model.TaxDTO != null && model.MemeberType == true && model.TaxDTO != null) ? ((SubTotal - model.Discount) * model.TaxDTO.Rate / 100) : 0;
                                    OneInvoice.Taxval = null;
                                    OneInvoice.MemberId = model.MemeberId;
                                    OneInvoice.Discount = Discount;
                                    OneInvoice.DiscountAmountOrPercent = AmountOrPersent == false ? "%" : "$";
                                    OneInvoice.Paid = 0;
                                    OneInvoice.Net = Net;
                                    OneInvoice.Status = 0; //Draft status if(1=partail & 2=paid)
                                    OneInvoice.Type = 2; //Installment Payment type
                                    OneInvoice.SignaturePrintName = null;
                                    OneInvoice.SignatureDraw = null;
                                    OneInvoice.Terms = null;
                                    OneInvoice.NotesForCustomer = model.Notes;
                                    //OneInvoice.Notes = model.Notes;
                                    OneInvoice.Active = model.Active;
                                    OneInvoice.CreateUser = int.Parse(Helpers.Settings.UserIdGet);
                                    OneInvoice.CreateDate = DateTime.Now;

                                    foreach (ScheduleItemsServicesModel item in LstItemsInvoice)
                                    {
                                        InvoiceItemServicesModel ObjItem = new InvoiceItemServicesModel
                                        {
                                            Id = item.Id,
                                            AccountId = model.AccountId,
                                            BrancheId = model.BrancheId,
                                            ItemServiceDescription = item.ItemServiceDescription,
                                            //TaxId = model.TaxId,
                                            //Tax = model.TaxDTO.Rate,
                                            //Taxable = (model.TaxDTO.Rate == null || model.TaxDTO.Rate == 0) ? false : true,
                                            Taxable = true,
                                            //Unit = item.Unit,
                                            Price = item.CostRate,
                                            Quantity = item.Quantity,
                                            //Discountable = model.MemberDTO.MemberValue != null ? true : false,
                                            Discountable = true,
                                            ItemsServicesId = item.ItemsServicesId,
                                            ItemsServicesName = item.ItemsServicesName,
                                            CreateUser = int.Parse(Helpers.Settings.UserIdGet),
                                            CreateDate = DateTime.Now,
                                            SkipOfTotal = item.Out,
                                            Total = item.Quantity != null && item.Tax != null ? (item.CostRate * item.Quantity) + (item.CostRate * item.Quantity * item.Tax / 100) : item.Quantity == null && item.Tax != null ? item.CostRate + (item.CostRate * item.Quantity * item.Tax / 100) : item.Quantity != null && item.Tax == null ? item.CostRate * item.Quantity : item.CostRate,
                                            Active = model.Active,
                                        };
                                        OneInvoice.LstInvoiceItemServices.Add(ObjItem);
                                    }

                                    UserDialogs.Instance.ShowLoading();
                                    //var json = await Helpers.Utility.PostMData("api/Invoices/PostInvoice", JsonConvert.SerializeObject(OneInvoice));
                                    var json = await ORep.PostDataAsync("api/Invoices/PostInvoice", OneInvoice, UserToken);
                                    UserDialogs.Instance.HideHud();

                                    if (json != "Bad Request" && json != "api not responding" && json.Contains("Not_Enough") != true && json.Contains("This Invoice Already Exist") != true)
                                    {
                                        //await App.Current!.MainPage!.DisplayAlert("FixPro", "Success Create Invoice for This Job.", "Ok");
                                        //Helpers.Messages.ShowSuccessSnackBar("Success Create Invoice for This Job.");
                                        var toast = Toast.Make("Success Create Invoice for This Job.", CommunityToolkit.Maui.Core.ToastDuration.Long, 15);
                                        await toast.Show();

                                        bool answer = await App.Current!.MainPage!.DisplayAlert("Question?", "Do you want to send an email to the customer?", "Yes", "No");

                                        if (answer)//Send Email
                                        {
                                            UserDialogs.Instance.ShowLoading();
                                            var jsonEmail = await ORep.PostStrAsync("api/Invoices/PostInvoiceEmail", OneInvoice, UserToken);
                                            UserDialogs.Instance.HideHud();

                                            if (!string.IsNullOrEmpty(jsonEmail) && jsonEmail.Contains("Send Success") == true)
                                            {
                                                //Helpers.Messages.ShowSuccessSnackBar("Success Send Email to Customer.");
                                                var toast1 = Toast.Make("Success Send Email to Customer.", CommunityToolkit.Maui.Core.ToastDuration.Long, 15);
                                                await toast1.Show();
                                                //await App.Current!.MainPage!.DisplayAlert("FixPro", "Email sent successfully to customer", "Ok");
                                            }
                                            else
                                            {
                                               // Helpers.Messages.ShowSuccessSnackBar("Failed to send e-mail to the customer");
                                                var toast1 = Toast.Make("Failed to send e-mail to the customer", CommunityToolkit.Maui.Core.ToastDuration.Long, 15);
                                                await toast1.Show();
                                                //await App.Current!.MainPage!.DisplayAlert("FixPro", "Failed to send e-mail to the customer", "Ok");
                                            }
                                        }

                                        if (OneInvoice.Net > 0)
                                        {
                                            OneInvoice.Id = int.Parse(json.Replace("\"", "").Trim());
                                            var ViewModel = new SchedulesViewModel(OneInvoice, CustomerDetails);
                                            var page = new Pages.PopupPages.PaymentMethodsPopup();
                                            page.BindingContext = ViewModel;
                                            await MopupService.Instance.PushAsync(page);
                                            //await App.Current!.MainPage!.Navigation.PushAsync(new MainPage());
                                            // App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);
                                        }
                                        else
                                        {
                                            var ViewModel = new CustomersViewModel(CustomerDetails);
                                            var page = new Pages.CustomerPages.CustomersDetailsPage();
                                            page.BindingContext = ViewModel;
                                            await App.Current!.MainPage!.Navigation.PushAsync(page);
                                            App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);
                                            App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);
                                        }

                                    }
                                    else
                                    {
                                        await App.Current!.MainPage!.DisplayAlert("Alert", json, "Ok");
                                        await App.Current!.MainPage!.Navigation.PushAsync(new MainPage());
                                    }
                                }
                                else
                                {
                                    await App.Current!.MainPage!.DisplayAlert("Alert", "Please don’t check all the items/services out for this invoice", "Ok");
                                }

                            }
                            else
                            {
                                await App.Current!.MainPage!.DisplayAlert("Alert", "No item/service chosen for this invoice", "Ok");
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
            }

            IsEnable = true;
        }


        [RelayCommand]
        async Task OpenCustomerDetails(CustomersModel model)
        {
            IsEnable = false;
            UserDialogs.Instance.ShowLoading();
            Controls.StaticMembers.WayCreateCust = 3;//From Schedule can edit customer and return schedule again
            Controls.StaticMembers.ScheduleIdStatic = ScheduleDetails.Id;
            Controls.StaticMembers.ScheduleDateIdStatic = ScheduleDetails.ScheduleDateId;
            var popupView = new CustomersViewModel(model);
            var page = new Pages.CustomerPages.CustomersDetailsPage();
            page.BindingContext = popupView;
            await App.Current!.MainPage!.Navigation.PushAsync(page);
            UserDialogs.Instance.HideHud();
            IsEnable = true;
        }

        [RelayCommand]
        async Task CreditPayment(InvoiceModel model)
        {
            IsEnable = false;
            UserDialogs.Instance.ShowLoading();
            Controls.StaticMembers.PayCashOrCredit = 2;
            await MopupService.Instance.PopAsync();
            var ViewModel = new PaymentsViewModel(model, CustomerDetails);
            var page = new Pages.CustomerPages.CashOrCreditPaymentPage();
            page.BindingContext = ViewModel;
            await App.Current!.MainPage!.Navigation.PushAsync(page);
            UserDialogs.Instance.HideHud();
            IsEnable = true;
        }

        [RelayCommand]
        async Task CashPayment(InvoiceModel model)
        {
            IsEnable = false;
            UserDialogs.Instance.ShowLoading();
            Controls.StaticMembers.PayCashOrCredit = 1;
            await MopupService.Instance.PopAsync();
            var ViewModel = new PaymentsViewModel(model, CustomerDetails);
            var page = new Pages.CustomerPages.CashOrCreditPaymentPage();
            page.BindingContext = ViewModel;
            await App.Current!.MainPage!.Navigation.PushAsync(page);
            UserDialogs.Instance.HideHud();
            IsEnable = true;
        }

        [RelayCommand]
        async Task OpenMaterialDetails(ScheduleItemsServicesModel model)
        {
            IsEnable = false;
            UserDialogs.Instance.ShowLoading();
            await App.Current!.MainPage!.Navigation.PushAsync(new Pages.SchedulePages.NewItemsServicesSchedulePage(model));
            UserDialogs.Instance.HideHud();
            IsEnable = true;
        }

        [RelayCommand]
        async Task OpenMaterialReceiptDetails(ScheduleMaterialReceiptModel model)
        {
            IsEnable = false;
            UserDialogs.Instance.ShowLoading();
            await App.Current!.MainPage!.Navigation.PushAsync(new Pages.SchedulePages.MaterialReceiptPage(model));
            UserDialogs.Instance.HideHud();
            IsEnable = true;
        }

        [RelayCommand]
        async Task OpenServiceDetails(ScheduleItemsServicesModel model)
        {
            IsEnable = false;
            UserDialogs.Instance.ShowLoading();
            await App.Current!.MainPage!.Navigation.PushAsync(new Pages.SchedulePages.ScheduleFreeServicesPage(model));
            UserDialogs.Instance.HideHud();
            IsEnable = true;
        }

        [RelayCommand]
        async Task OutScheduleImage(SchedulePicturesModel image)
        {
            IsEnable = false;
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                string UserToken = await _service.UserToken();
                var json = await ORep.PutStrAsync("api/Schedules/PutOutPicture", image, UserToken);
                if (json == "false")
                {
                    //Helpers.Messages.ShowSuccessSnackBar("Don't Show unchecked photos to customer");
                    var toast = Toast.Make("Don't Show unchecked photos to customer", CommunityToolkit.Maui.Core.ToastDuration.Long, 15);
                    await toast.Show();
                }
                else
                {
                    //Helpers.Messages.ShowSuccessSnackBar("Show checked photos to customer");
                    var toast = Toast.Make("Show checked photos to customer", CommunityToolkit.Maui.Core.ToastDuration.Long, 15);
                    await toast.Show();
                }
            }
            IsEnable = true;
        }

        [RelayCommand]
        async Task OpenFullScreenSchImage(string ImageName)
        {
            IsEnable = false;
            UserDialogs.Instance.ShowLoading();
            await App.Current!.MainPage!.Navigation.PushAsync(new FullScreenImagePage(ImageName));
            UserDialogs.Instance.HideHud();
            IsEnable = true;
        }

        [RelayCommand]
        async Task OpenFullScreenSchImageBeforeInsert(ImageSource ImageName)
        {
            IsEnable = false;
            UserDialogs.Instance.ShowLoading();
            await App.Current!.MainPage!.Navigation.PushAsync(new FullScreenImagePage(ImageName));
            UserDialogs.Instance.HideHud();
            IsEnable = true;
        }
    }
}
