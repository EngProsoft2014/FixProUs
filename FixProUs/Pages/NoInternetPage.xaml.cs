using Controls.UserDialogs.Maui;
using System.Xml.Linq;

namespace FixProUs.Pages;

public partial class NoInternetPage : Controls.CustomsPage
{
    public Page Name = new Page();

    public NoInternetPage()
	{
		InitializeComponent();
	}

    public NoInternetPage(Page PageName)
    {
        InitializeComponent();

        if (Helpers.Settings.UserNameGet != "" && Helpers.Settings.PasswordGet != "")
        {
            Name = PageName;
        }
        else
        {
            Name = new LoginPage();
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
    }

    protected override bool OnBackButtonPressed()
    {
        //Device.BeginInvokeOnMainThread(async () =>
        //{
        //    var exit = false;
        //    exit = await this.DisplayAlert("FixPro", "Do you want to exit the program?", "Ok", "I want to stay").ConfigureAwait(false);
        //    if (exit)
        //    {
        //        System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
        //    }
        //});

        // Run the async code on the UI thread
        Dispatcher.Dispatch(async() =>
        {
            var exit = false;
            exit = await this.DisplayAlert("FixPro", "Do you want to exit the program?", "Ok", "I want to stay").ConfigureAwait(false);
            if (exit)
            {
                Application.Current.Quit();
            }
        });
        return true;
    }



    public async Task GoAfterConnected()
    {
        UserDialogs.Instance.ShowLoading();
        //await App.Current!.MainPage!.Navigation.PushAsync(Name);

        await App.Current!.MainPage!.Navigation.PopAsync();
        //App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);

        UserDialogs.Instance.HideHud();
    }

    async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
    {
        if (e.NetworkAccess != NetworkAccess.Internet)
        {
            // Connection to internet is Not available

        }
        else
        {
            // Connection to internet is available
            await GoAfterConnected();
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            // Connection to internet is Not available

        }
        else
        {
            // Connection to internet is available
            await GoAfterConnected();
        }
    }
}