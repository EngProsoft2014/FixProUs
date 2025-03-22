using Controls.UserDialogs.Maui;
using FixProUs.ViewModels;

namespace FixProUs.Pages.SchedulePages;

public partial class SchedulePicturesPage : Controls.CustomsPage
{
    SchedulesViewModel ViewModel { get => BindingContext as SchedulesViewModel; set => BindingContext = value; }
    public SchedulePicturesPage()
    {
        InitializeComponent();
    }

    [Obsolete]
    protected override bool OnBackButtonPressed()
    {
        Device.BeginInvokeOnMainThread(async () =>
        {
            ViewModel.IsEnable = false;
            UserDialogs.Instance.ShowLoading();
            var popupView = new SchedulesViewModel(ViewModel.ScheduleDetails.Id, ViewModel.ScheduleDetails.OneScheduleDate.Id);
            var page = new Pages.SchedulePages.NewSchedulePage();
            page.BindingContext = popupView;
            await App.Current!.MainPage!.Navigation.PushAsync(page);
            App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);
            App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);
            UserDialogs.Instance.HideHud();
            ViewModel.IsEnable = true;
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
}