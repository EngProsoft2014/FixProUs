namespace FixProUs.Pages.SchedulePages;

public partial class CreateItemPage : Controls.CustomsPage
{
	public CreateItemPage()
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
        await App.Current.MainPage.Navigation.PopAsync();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await App.Current.MainPage.Navigation.PopAsync();
    }
}