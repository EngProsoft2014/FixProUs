using Mopups.Services;

namespace FixProUs.Pages.PopupPages;

public partial class CheckoutPopup : Mopups.Pages.PopupPage
{
    public delegate void TimeDelegte(TimeSpan time);
    public event TimeDelegte TimeDidClose;

    public CheckoutPopup()
	{
		InitializeComponent();
	}

    public CheckoutPopup(int id, string time)
    {
        InitializeComponent();
        btnCheck.Text = "Check In";

        if (time != null && time != "")
        {
            timeCheckOut.Time = TimeSpan.Parse(time);
        }
    }

    public CheckoutPopup(string time)
    {
        InitializeComponent();

        btnCheck.Text = "Check Out";

        if (time != null && time != "")
        {
            timeCheckOut.Time = TimeSpan.Parse(time);
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await MopupService.Instance.PopAsync();
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        if (timeCheckOut.Time != null)
        {
            TimeDidClose?.Invoke(timeCheckOut.Time);
        }
        else
        {
            await App.Current.MainPage.DisplayAlert("Alert", "Please select a time", "Ok");
        }
        await MopupService.Instance.PopAsync();
        await App.Current.MainPage.Navigation.PushAsync(new TimeSheetPage());
        App.Current.MainPage.Navigation.RemovePage(App.Current.MainPage.Navigation.NavigationStack[App.Current.MainPage.Navigation.NavigationStack.Count - 2]);
    }
}