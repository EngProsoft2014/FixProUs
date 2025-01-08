using FixProUs.ViewModels;
using Mopups.Services;

namespace FixProUs.Pages.PopupPages;

public partial class PaymentMethodsPopup : Mopups.Pages.PopupPage
{
    SchedulesViewModel ViewModel { get => BindingContext as SchedulesViewModel; set => BindingContext = value; }

    public PaymentMethodsPopup()
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

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await MopupService.Instance.PopAsync();
        await App.Current.MainPage.Navigation.PushAsync(new MainPage());
    }
}