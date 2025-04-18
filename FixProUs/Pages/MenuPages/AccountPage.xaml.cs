using FixProUs.ViewModels;
using Mopups.Pages;
using Mopups.Services;

namespace FixProUs.Pages.MenuPages;

public partial class AccountPage : Controls.CustomsPage
{
    AccountViewModel ViewModel { get => BindingContext as AccountViewModel; set => BindingContext = value; }

    public AccountPage()
	{
		InitializeComponent();
	}

    [Obsolete]
    protected override bool OnBackButtonPressed()
    {
        Device.BeginInvokeOnMainThread(async () =>
        {
            await Navigation.PushAsync(new MainPage());
        });
        return true;
    }

    //private void actIndLoading_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    //{
    //    if (actIndLoading.IsRunning == true)
    //        this.IsEnabled = false;
    //    else
    //        this.IsEnabled = true;
    //}

    private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
    {
        await MopupService.Instance.PushAsync(new PopupPages.ChangeAccountPhotoPupop());
    }

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedOption = (sender as Picker)!.SelectedItem;
        ViewModel.SelectBranchCommand.Execute(selectedOption);
    }

    private async void TapGestureRecognizer_Tapped_2(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }
}