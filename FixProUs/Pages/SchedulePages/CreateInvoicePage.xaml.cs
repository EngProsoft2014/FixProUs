using FixProUs.ViewModels;

namespace FixProUs.Pages.SchedulePages;

public partial class CreateInvoicePage : Controls.CustomsPage
{
    SchedulesViewModel ViewModel { get => BindingContext as SchedulesViewModel; set => BindingContext = value; }

    public CreateInvoicePage()
    {
        InitializeComponent();
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
        await Navigation.PopAsync();
    }

    private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
    {
        stkEditDiscount.IsVisible = true;
    }

    private void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
    {
        stkEditDiscount.IsVisible = false;
    }

    private void entryDiscount_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (e.NewTextValue != null && e.NewTextValue != "")
        {
            pnkSave.IsVisible = true;
        }
        else
        {
            pnkSave.IsVisible = false;
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        ViewModel.TotalInvoice(ViewModel.ScheduleDetails, ViewModel.CustomerDetails);
    }
}