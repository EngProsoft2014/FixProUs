namespace FixProUs.Pages;

public partial class NotificationsPage : Controls.CustomsPage
{
	public NotificationsPage()
	{
		InitializeComponent();
	}

    private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        await App.Current.MainPage.Navigation.PopAsync();
        await App.Current.MainPage.Navigation.PushAsync(new MainPage());
    }

    protected override bool OnBackButtonPressed()
    {
        App.Current.MainPage.Navigation.PopAsync();
        App.Current.MainPage.Navigation.PushAsync(new MainPage());
        return true;
    }
}