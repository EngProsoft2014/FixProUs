
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Controls.UserDialogs.Maui;
using FixProUs.Models;
using Mopups.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;


namespace FixProUs.ViewModels
{
    public partial class TimeSheetViewModel : BaseViewModel
    {

        readonly Services.Data.ServicesService _service = new Services.Data.ServicesService();

        [ObservableProperty]
        ObservableCollection<CheckInOutModel> lstEmployeesIn;

        [ObservableProperty]
        ObservableCollection<CheckInOutModel> lstEmployeesOut;

        [ObservableProperty]
        bool isRefresh;

        [ObservableProperty]
        string date;

        [ObservableProperty]
        string numIn;

        [ObservableProperty]
        string numOut;

        DateTime dateDF;

        Helpers.GenericRepository ORep = new Helpers.GenericRepository();

        public TimeSheetViewModel()
        {
            Init();
        }

        void Init()
        {
            if (Controls.StaticMembers.SelectedDate.ToString("MM-dd-yyyy") == DateTime.Now.ToString("MM-dd-yyyy"))
            {
                dateDF = DateTime.Now;
                Date = "Today";
            }
            else
            {
                dateDF = Controls.StaticMembers.SelectedDate;
                Date = Controls.StaticMembers.SelectedDate.ToString("MM-dd-yyyy");
            }

            LstEmployeesIn = new ObservableCollection<CheckInOutModel>();
            LstEmployeesOut = new ObservableCollection<CheckInOutModel>();

            GetCheckInOutEmployees(dateDF.ToString("MM-dd-yyyy"));
        }

        public async void GetCheckInOutEmployees(string date)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                UserDialogs.Instance.ShowLoading();
                string UserToken = await _service.UserToken();
                var json = await ORep.GetAsync<List<CheckInOutModel>>("api/TimeSheet/GetCheckInOut?" + "date=" + date + "&" + "userId=" + Helpers.Settings.UserIdGet + "&" + "userRole=" + Controls.StartData.EmployeeDataStatic.UserRole.ToString(), UserToken);

                if (json != null)
                {
                    LstEmployeesIn = new ObservableCollection<CheckInOutModel>(json.Where(x => x.HoursTo == "" || x.HoursTo == null).OrderBy(x => x.EmployeeName).ToList());
                    LstEmployeesOut = new ObservableCollection<CheckInOutModel>(json.Where(x => x.HoursTo != "" && x.HoursTo != null).OrderBy(x => x.EmployeeName).ToList());

                    NumIn = LstEmployeesIn.Count.ToString();
                    NumOut = LstEmployeesOut.Count.ToString();
                }
                else
                {
                    NumIn = "0";
                    NumOut = "0";
                }

                UserDialogs.Instance.HideHud();
            }
        }

        [RelayCommand]
        void RefreshLstEmployees()
        {
            IsRefresh = true;

            GetCheckInOutEmployees(dateDF.ToString("MM-dd-yyyy"));

            IsRefresh = false;
        }

        [RelayCommand]
        void NextDay()
        {
            IsBusy = true;
            dateDF = Controls.StaticMembers.SelectedDate;
            Date = dateDF.AddDays(1).ToString("MM-dd-yyyy") == DateTime.Now.ToString("MM-dd-yyyy") ? "Today" : dateDF.AddDays(1).ToString("MM-dd-yyyy");
            Controls.StaticMembers.SelectedDate = dateDF = dateDF.AddDays(1);
            RefreshLstEmployees();
            IsBusy = false;
        }

        [RelayCommand]
        void BackDay()
        {
            IsBusy = true;
            dateDF = Controls.StaticMembers.SelectedDate;
            Date = dateDF.AddDays(-1).ToString("MM-dd-yyyy") == DateTime.Now.ToString("MM-dd-yyyy") ? "Today" : dateDF.AddDays(-1).ToString("MM-dd-yyyy");
            Controls.StaticMembers.SelectedDate = dateDF = dateDF.AddDays(-1);
            RefreshLstEmployees();
            IsBusy = false;
        }

        [RelayCommand]
        async Task SelectedDate()
        {
            IsBusy = true;
            var popupView = new Pages.PopupPages.DatePopup();
            popupView.RangeClose += (calendar) =>
            {
                UserDialogs.Instance.ShowLoading();

                GetCheckInOutEmployees(calendar.StartDate.Value.ToString("MM-dd-yyyy"));

                Date = calendar.StartDate.Value.ToString("MM-dd-yyyy");

                UserDialogs.Instance.HideHud();
            };

            await MopupService.Instance.PushAsync(popupView);
            IsBusy = false;
        }

        [RelayCommand]
        async Task SelectedTimeIn(CheckInOutModel model)
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
                    var popupView = new Pages.PopupPages.CheckoutPopup(model.Id, model.HoursFrom);
                    popupView.TimeDidClose += async (time) =>
                    {
                        UserDialogs.Instance.ShowLoading();
                        string UserToken = await _service.UserToken();
                        model.HoursFrom = string.Format(time.ToString(@"hh\:mm"));
                        model.Active = true;

                        var json = await ORep.PutAsync(string.Format("api/TimeSheet/PutCheckInOut/{0}", model.EmployeeId), model, UserToken);

                        if (json != null)
                        {
                            await App.Current!.MainPage!.DisplayAlert("FixPro", "Succes Check In Time.", "Ok");
                            await App.Current.MainPage.Navigation.PushAsync(new Pages.TimeSheetPage());
                            App.Current.MainPage.Navigation.RemovePage(App.Current.MainPage.Navigation.NavigationStack[App.Current.MainPage.Navigation.NavigationStack.Count - 2]);
                        }
                        UserDialogs.Instance.HideHud();
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
        async Task SelectedTimeOut(CheckInOutModel model)
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
                    var popupView = new Pages.PopupPages.CheckoutPopup(model.HoursFrom);
                    popupView.TimeDidClose += async (time) =>
                    {
                        UserDialogs.Instance.ShowLoading();

                        if (time > TimeSpan.Parse(model.HoursFrom))
                        {
                            string UserToken = await _service.UserToken();
                            //model.HoursTo = string.Format("{0:hh:mm}", time.ToString());
                            model.HoursTo = string.Format(time.ToString(@"hh\:mm"));
                            model.DurationHours = (time - TimeSpan.Parse(model.HoursFrom)).Hours.ToString();
                            model.DurationMinutes = (time - TimeSpan.Parse(model.HoursFrom)).Minutes.ToString();

                            await ORep.PutAsync(string.Format("api/TimeSheet/PutCheckInOut/{0}", model.EmployeeId), model, UserToken);

                        }
                        else
                        {
                            await App.Current!.MainPage!.DisplayAlert("Alert", "Please Choose Time After Check In Time.", "Ok");
                        }

                        UserDialogs.Instance.HideHud();
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
        async Task SelectedTimeMyStart(CheckInOutModel model)
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

                    model.HoursFrom = string.Format(DateTime.Now.ToString(@"hh\:mm"));
                    model.Date = Controls.StaticMembers.SelectedDate.ToString("yyyy-MM-dd");
                    model.CreateDate = DateTime.Now;
                    model.CreateUser = int.Parse(Helpers.Settings.UserIdGet);
                    model.SheetColor = "#26cc8a";
                    model.Active = true;

                    var json = await ORep.PostAsync("api/TimeSheet/PostCheckInOut", model, UserToken);

                    //if (json != null && json != "api not responding")
                    if (json != null && json.EmployeeId != 0 && json.EmployeeId != null)
                    {
                        await App.Current!.MainPage!.DisplayAlert("FixPro", "Succes Check In Time.", "Ok");
                        await App.Current.MainPage.Navigation.PushAsync(new Pages.TimeSheetPage());
                        App.Current.MainPage.Navigation.RemovePage(App.Current.MainPage.Navigation.NavigationStack[App.Current.MainPage.Navigation.NavigationStack.Count - 2]);
                    }
                    else
                    {
                        await App.Current!.MainPage!.DisplayAlert("FixPro", "Failed Check In Time.", "Ok");
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
        async Task SelectedTimeMyEnd(CheckInOutModel model)
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

                    TimeSpan time = DateTime.Now.TimeOfDay;

                    if (time > TimeSpan.Parse(model.HoursFrom))
                    {
                        string UserToken = await _service.UserToken();

                        model.HoursTo = string.Format(time.ToString(@"hh\:mm"));
                        model.DurationHours = (time - TimeSpan.Parse(model.HoursFrom)).Hours.ToString();
                        model.DurationMinutes = (time - TimeSpan.Parse(model.HoursFrom)).Minutes.ToString();

                        var json = await ORep.PutAsync(string.Format("api/TimeSheet/PutCheckInOut/{0}", model.EmployeeId), model, UserToken);

                        //if (json != null && json != "api not responding")
                        if (json != null)
                        {
                            await App.Current!.MainPage!.DisplayAlert("FixPro", "Succes Check Out Time.", "Ok");
                            await App.Current.MainPage.Navigation.PushAsync(new Pages.TimeSheetPage());
                            App.Current.MainPage.Navigation.RemovePage(App.Current.MainPage.Navigation.NavigationStack[App.Current.MainPage.Navigation.NavigationStack.Count - 2]);
                        }
                    }
                    else
                    {
                        await App.Current!.MainPage!.DisplayAlert("Alert", "Please Choose Time After Check In Time.", "Ok");
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

    }
}
