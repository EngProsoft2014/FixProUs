using FixProUs.ViewModels;

namespace FixProUs.Pages.SchedulePages;

public partial class ChooseCustomerPage : Controls.CustomsPage
{
    CustomersViewModel ViewModel { get => BindingContext as CustomersViewModel; set => BindingContext = value; }
    public ChooseCustomerPage()
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

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await App.Current!.MainPage!.Navigation.PushAsync(new Pages.SchedulePages.NewSchedulePage());
    }

    private void srchPhone_TextChanged(object sender, TextChangedEventArgs e)
    {
        //lstCustomers.ItemsSource = ViewModel.LstCustomers.Where(x => (x.Phone1).Contains(srchPhone.Text));
        lstCustomers.ItemsSource = ViewModel.LstCustomers.Where(x => (x.Phone1).Contains(srchPhoneOrAddress.Text) || (x.Address.ToLower()).Contains(srchPhoneOrAddress.Text.ToLower())
        || (x.FirstName.ToLower() + x.LastName.ToLower()).Contains(srchPhoneOrAddress.Text.ToLower()));
    }

}