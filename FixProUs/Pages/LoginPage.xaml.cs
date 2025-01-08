namespace FixProUs.Pages;

public partial class LoginPage : Controls.CustomsPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

    protected override bool OnBackButtonPressed()
    {
        Device.BeginInvokeOnMainThread(async () =>
        {
            var exit = false;
            exit = await this.DisplayAlert("FIXPRO", "Do you want to exit the program?", "Ok", "I want to stay").ConfigureAwait(false);
            if (exit)
            {
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            }
        });
        return true;
    }

    private void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
    {
        entPassword.IsPassword = (entPassword.IsPassword == true) ? false : true;
    }

    //private void actIndLoading_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    //{
    //    if (actIndLoading.IsRunning == true)
    //        this.IsEnabled = false;
    //    else
    //        this.IsEnabled = true;
    //}
}