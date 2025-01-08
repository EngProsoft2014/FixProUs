namespace FixProUs.Pages.CustomerPages;

public partial class CustomersDetailsPage : Controls.CustomsPage
{
	public CustomersDetailsPage()
	{
		InitializeComponent();
	}

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await App.Current!.MainPage!.Navigation.PopAsync();
    }

    //private void actIndLoading_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    //{
    //    if (actIndLoading.IsRunning == true)
    //        this.IsEnabled = false;
    //    else
    //        this.IsEnabled = true;
    //}
}