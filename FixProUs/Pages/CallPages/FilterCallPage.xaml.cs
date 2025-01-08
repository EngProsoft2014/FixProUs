using FixProUs.ViewModels;
using GoogleApi.Entities.Maps.AerialView.Common.Enums;

namespace FixProUs.Pages.CallPages;

public partial class FilterCallPage : Controls.CustomsPage
{
    FilterCallsViewModel ViewModel { get => BindingContext as FilterCallsViewModel; set => BindingContext = value; }

    public FilterCallPage()
	{
		InitializeComponent();
	}

    private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        await App.Current.MainPage.Navigation.PopAsync();
    }

    //private void actIndLoading_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    //{
    //    if (actIndLoading.IsRunning == true)
    //        this.IsEnabled = false;
    //    else
    //        this.IsEnabled = true;
    //}

    private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e) //Reset Filter Calls
    {

        pkrStartDt.Date = ViewModel.StartDate = DateTime.Now;
        ViewModel.OneFilter.StartDate = string.Empty;
        pkrEndDt.Date = ViewModel.EndDate = DateTime.Now;
        ViewModel.OneFilter.EndDate = string.Empty;

        swtDate.IsToggled = false;

        entryPhone.Text = ViewModel.OneFilter.PhoneNum = string.Empty;
        entryJob.Text = ViewModel.OneFilter.ScheduleTitle = string.Empty;
        pkrReason.SelectedItem = null;
        ViewModel.OneFilter.ReasonName = string.Empty;
        pkrCampaign.SelectedItem = null;
        ViewModel.OneFilter.CampaignName = string.Empty;
        pkrEmployee.SelectedItem = null;
        ViewModel.OneFilter.EmployeeName = string.Empty;

        Controls.StaticMembers.FilterCallModel = ViewModel.OneFilter;
    }
}