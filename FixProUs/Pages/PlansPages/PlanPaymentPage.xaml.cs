using FixProUs.ViewModels;
using Syncfusion.Maui.Core.Carousel;

namespace FixProUs.Pages.PlansPages;

public partial class PlanPaymentPage : Controls.CustomsPage
{
    PlansViewModel ViewModel { get => BindingContext as PlansViewModel; set => BindingContext = value; }

    public PlanPaymentPage()
    {
        InitializeComponent();

        //if (ViewModel != null && ViewModel.IsYearly == true && ViewModel.IsMonthly == false)
        //{
        //    pnkYearlyMethod.BackgroundColor = Color.FromHex("#d4f8ff");
        //    pnkMonthlyMethod.BackgroundColor = Color.FromHex("#ffffff");
        //    rdMonthly.IsChecked = false;
        //    rdYearly.IsChecked = true;
        //}
        //else
        //{
        //    pnkYearlyMethod.BackgroundColor = Color.FromHex("#ffffff");
        //    pnkMonthlyMethod.BackgroundColor = Color.FromHex("#d4f8ff");
        //    rdMonthly.IsChecked = true;
        //    rdYearly.IsChecked = false;
        //}
    }

    private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    //private void rdYearly_CheckedChanged(object sender, CheckedChangedEventArgs e)
    //{
    //    if (e.Value)
    //    {
    //        pnkYearlyMethod.BackgroundColor = Color.FromHex("#d4f8ff");
    //        pnkMonthlyMethod.BackgroundColor = Color.FromHex("#ffffff");
    //        rdMonthly.IsChecked = false;
    //        ViewModel.IsMonthly = false;
    //        rdYearly.IsChecked = true;
    //        ViewModel.IsYearly = true;
    //    }
    //    else
    //    {
    //        pnkYearlyMethod.BackgroundColor = Color.FromHex("#ffffff");
    //        pnkMonthlyMethod.BackgroundColor = Color.FromHex("#d4f8ff");
    //        rdMonthly.IsChecked = true;
    //        ViewModel.IsMonthly = true;
    //        rdYearly.IsChecked = false;
    //        ViewModel.IsYearly = false;
    //    }

    //}

    //private void rdMonthly_CheckedChanged(object sender, CheckedChangedEventArgs e)
    //{

    //    if (e.Value) 
    //    {
    //        pnkYearlyMethod.BackgroundColor = Color.FromHex("#ffffff");
    //        pnkMonthlyMethod.BackgroundColor = Color.FromHex("#d4f8ff");
    //        rdMonthly.IsChecked = true;
    //        ViewModel.IsMonthly = true;
    //        rdYearly.IsChecked = false;
    //        ViewModel.IsYearly = false;
    //    }
    //    else
    //    {
    //        pnkYearlyMethod.BackgroundColor = Color.FromHex("#d4f8ff");
    //        pnkMonthlyMethod.BackgroundColor = Color.FromHex("#ffffff");
    //        rdMonthly.IsChecked = false;
    //        ViewModel.IsMonthly = false;
    //        rdYearly.IsChecked = true;
    //        ViewModel.IsYearly = true;
    //    }

    //}

    private void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        if (rdYearly.IsChecked == false && rdMonthly.IsChecked == true)
        {
            pnkYearlyMethod.BackgroundColor = Color.FromHex("#d4f8ff");
            pnkMonthlyMethod.BackgroundColor = Color.FromHex("#ffffff");
            rdMonthly.IsChecked = false;
            ViewModel.IsMonthly = false;
            rdYearly.IsChecked = true;
            ViewModel.IsYearly = true;
        }
        else
        {
            pnkYearlyMethod.BackgroundColor = Color.FromHex("#ffffff");
            pnkMonthlyMethod.BackgroundColor = Color.FromHex("#d4f8ff");
            rdMonthly.IsChecked = true;
            ViewModel.IsMonthly = true;
            rdYearly.IsChecked = false;
            ViewModel.IsYearly = false;
        }
    }

    private void TapGestureRecognizer_Tapped_2(object sender, TappedEventArgs e)
    {
        if (rdYearly.IsChecked == true && rdMonthly.IsChecked == false)
        {
            pnkYearlyMethod.BackgroundColor = Color.FromHex("#ffffff");
            pnkMonthlyMethod.BackgroundColor = Color.FromHex("#d4f8ff");
            rdMonthly.IsChecked = true;
            ViewModel.IsMonthly = true;
            rdYearly.IsChecked = false;
            ViewModel.IsYearly = false;
        }
        else
        {
            pnkYearlyMethod.BackgroundColor = Color.FromHex("#d4f8ff");
            pnkMonthlyMethod.BackgroundColor = Color.FromHex("#ffffff");
            rdMonthly.IsChecked = false;
            ViewModel.IsMonthly = false;
            rdYearly.IsChecked = true;
            ViewModel.IsYearly = true;
        }
    }
}