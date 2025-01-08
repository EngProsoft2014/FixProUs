using FixProUs.Models;
using FixProUs.ViewModels;
using Mopups.Services;
using System.Collections.ObjectModel;

namespace FixProUs.Pages.PopupPages;

public partial class ScheduleDatesPopup : Mopups.Pages.PopupPage
{
    public delegate void DatesDelegte(List<SchaduleDateModel> Dates);
    public event DatesDelegte DatesClose;

    SchedulesViewModel ViewModel = new SchedulesViewModel();

    public ScheduleDatesPopup()
	{
		InitializeComponent();
	}

    public ScheduleDatesPopup(ObservableCollection<SchaduleDateModel> LstDates)
    {
        InitializeComponent();
        lstDates.ItemsSource = ViewModel.LstEstimateSchaduleDates = ViewModel.LstInvoiceSchaduleDates = LstDates;
    }

    //private void actIndLoading_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    //{
    //    if (actIndLoading.IsRunning == true)
    //        this.IsEnabled = false;
    //    else
    //        this.IsEnabled = true;
    //}

    private async void Button_Clicked(object sender, EventArgs e)
    {
        this.IsEnabled = false;
        List<SchaduleDateModel> LstDates = new List<SchaduleDateModel>();
        LstDates = ViewModel.LstEstimateSchaduleDates.Where(x => x.IsChecked == true).ToList();

        if (LstDates != null)
        {
            DatesClose.Invoke(LstDates);
        }
        else
        {
            await App.Current.MainPage.DisplayAlert("Warning", "Please Choose Empolyee !!", "OK");
        }
        this.IsEnabled = true;
        await MopupService.Instance.PopAsync();
    }
}