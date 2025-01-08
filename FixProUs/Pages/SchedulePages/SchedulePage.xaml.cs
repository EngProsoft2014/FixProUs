using Controls.UserDialogs.Maui;
using FixProUs.Models;
using FixProUs.ViewModels;
using Syncfusion.Maui.Calendar;
using Syncfusion.Maui.DataSource.Extensions;
using System.Collections.ObjectModel;

namespace FixProUs.Pages.SchedulePages;

public partial class SchedulePage : Controls.CustomsPage
{
    SchedulesViewModel ViewModel { get => BindingContext as SchedulesViewModel; set => BindingContext = value; }

    public SchedulePage()
    {
        InitializeComponent();

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        calendar.MonthView.SpecialDayPredicate = (date) =>
        {
            if (ViewModel.GroupedList.Count > 0)
            {
                foreach (var day in ViewModel.GroupedList)
                {
                    if (DateTime.Parse(day.StartDate) == date.Date)
                    {
                        TimeSpan Days = date.Date.Date - DateTime.Parse(day.StartDate).Date;

                        if (date.Date == DateTime.Parse(day.StartDate).AddDays(Math.Abs(Days.TotalDays)).Date)
                        {
                            CalendarIconDetails iconDetails = new CalendarIconDetails();
                            iconDetails.Icon = CalendarIcon.Dot;
                            iconDetails.Fill = Color.FromHex("#538dd4");
                            return iconDetails;
                        }
                    }
                }
            };

            return null;
        };
    }


    private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
    {
        await Navigation.PopAsync();

    }

    //private void actIndLoading_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    //{
    //    if (actIndLoading.IsRunning == true)
    //        this.IsEnabled = false;
    //    else
    //        this.IsEnabled = true;
    //}

    private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        try
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
                //return;
            }
            else
            {
                if (Controls.StartData.EmployeeDataStatic.ActiveCreateSchedule == true)
                {
                    Controls.StaticMembers.WayAfterChooseCust = 0; //Create New Schedule
                    await App.Current!.MainPage!.Navigation.PushAsync(new Pages.SchedulePages.ChooseCustomerPage());
                }
                else
                {
                    await App.Current!.MainPage!.DisplayAlert("Warning", "Sorry, You don't have access to create schedule", "OK");
                }
            }
        }
        catch (Exception)
        {
            await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
            //throw;
        }
    }

    private void swchCalenderView_Toggled(object sender, ToggledEventArgs e)
    {
        if (e.Value == false)
        {
            schedule.View = Syncfusion.Maui.Scheduler.SchedulerView.Week;
        }
        if (e.Value == true)
        {
            schedule.View = Syncfusion.Maui.Scheduler.SchedulerView.Day;
        }
    }


    private async void schedule_CellTapped(object sender, Syncfusion.Maui.Scheduler.SchedulerTappedEventArgs e)
    {
        UserDialogs.Instance.ShowLoading();
        try
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
                //return;
            }
            else
            {
                if (e.Appointments != null)
                {
                    SchedulesModel ScheduleId = e.Appointments.FirstOrDefault() as SchedulesModel;
                    var VM = new SchedulesViewModel(ScheduleId!.Id, ScheduleId.OneScheduleDate.Id);
                    var page = new ScheduleDetailsPage();
                    page.BindingContext = VM;
                    await App.Current!.MainPage!.Navigation.PushAsync(page);
                }
            }
        }
        catch (Exception)
        {
            await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
            //throw;
        }

        UserDialogs.Instance.HideHud();
    }

    private void calendar_SelectionChanged(object sender, Syncfusion.Maui.Calendar.CalendarSelectionChangedEventArgs e)
    {
        DateTime cc = Convert.ToDateTime(e.NewValue);
        string day = cc.ToString("yyyy-MM-dd");
        var Fird = ViewModel.LstSchedules.Where(x => x.StartDate == day).ToList();
        var Scon = Fird.OrderBy(o => o.From);
        colJobs.ItemsSource = new ObservableCollection<SchedulesModel>(Scon);
    }


    private void swchCalenderOrListView_Toggled(object sender, ToggledEventArgs e)
    {
        if (e.Value == false)
        {
            calendar.IsVisible = true;
            stkSwtScheduleView.IsVisible = false;
            schedule.IsVisible = false;
            colJobs.IsVisible = true;
        }
        if (e.Value == true)
        {
            schedule.IsVisible = true;
            calendar.IsVisible = false;
            stkSwtScheduleView.IsVisible = true;
            colJobs.IsVisible = false;
        }
    }

    //Search Btn
    private void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
    {
        calendar.IsVisible = false;
        stkSwtScheduleView.IsVisible = false;
        schedule.IsVisible = false;
        colJobs.IsVisible = false;
        stkListOrCalAndWeekOrDays.IsVisible = false;
        stkSearch.IsVisible = true;
        lblSearch.IsVisible = false;
        lblCalender.IsVisible = true;
        stkSearchItems.IsVisible = true;
        srchJobs.Text = "";
        colSearchJobs.IsVisible = false;
    }

    //Calendar Btn
    private void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
    {
        if (swchCalenderOrListView.IsToggled == true)
        {
            //Schedule
            schedule.IsVisible = true;
            calendar.IsVisible = false;
            colJobs.IsVisible = false;
            stkSwtScheduleView.IsVisible = true;
            colSearchJobs.IsVisible = false;
        }
        else
        {
            //Calendar
            calendar.IsVisible = true;
            schedule.IsVisible = false;
            colJobs.IsVisible = true;
            stkSwtScheduleView.IsVisible = false;
            colSearchJobs.IsVisible = false;
        }

        stkListOrCalAndWeekOrDays.IsVisible = true;
        stkSearch.IsVisible = false;
        lblSearch.IsVisible = true;
        lblCalender.IsVisible = false;
    }

    private void srchJobs_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.NewTextValue))
        {
            stkSearchItems.IsVisible = false;
            colSearchJobs.IsVisible = true;
        }
        else
        {
            stkSearchItems.IsVisible = true;
            colSearchJobs.IsVisible = false;
        }
    }


}