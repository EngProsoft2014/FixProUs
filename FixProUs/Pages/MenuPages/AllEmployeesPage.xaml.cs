using FixProUs.ViewModels;

namespace FixProUs.Pages.MenuPages;

public partial class AllEmployeesPage : Controls.CustomsPage
{
    EmployeesViewModel ViewModel { get => BindingContext as EmployeesViewModel; set => BindingContext = value; }

    public AllEmployeesPage()
	{
		InitializeComponent();
	}

    protected override bool OnBackButtonPressed()
    {
        Device.BeginInvokeOnMainThread(async () =>
        {
            await Navigation.PushAsync(new MainPage());
            App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);
        });
        return true;
    }

    private async void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
        App.Current!.MainPage!.Navigation.RemovePage(App.Current!.MainPage!.Navigation.NavigationStack[App.Current!.MainPage!.Navigation.NavigationStack.Count - 2]);
    }

    private void srcBarEmployee_TextChanged(object sender, TextChangedEventArgs e)
    {
        lstEmployees.ItemsSource = ViewModel.LstEmployees.Where(x => (x.FirstName.ToLower() + " " + x.LastName.ToLower()).Contains(srcBarEmployee.Text.ToLower()));
    }

    private async void lstEmployees_QueryItemSize(object sender, Syncfusion.Maui.ListView.QueryItemSizeEventArgs e)
    {
        if (ViewModel.LstEmployees.Count == 0)
            return;

        //hit bottom!
        if (e.ItemIndex == ViewModel.LstEmployees.Count - 1)
        {
            if (ViewModel.PageNumber <= ViewModel.TotalPage)
            {
                await ViewModel.GetEmployees();
            }
        }
    }

    //private void actIndLoading_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    //{
    //    if (actIndLoading.IsRunning == true)
    //        this.IsEnabled = false;
    //    else
    //        this.IsEnabled = true;
    //}

    //private async void lstEmployees_ItemAppearing(object sender, ItemVisibilityEventArgs e)
    //{
    //    if (ViewModel.LstEmployees.Count == 0)
    //        return;

    //    //hit bottom!
    //    if (e.Item == ViewModel.LstEmployees[ViewModel.LstEmployees.Count - 1])
    //    {
    //        if (ViewModel.PageNumber <= ViewModel.TotalPage)
    //        {
    //            await ViewModel.GetEmployees();
    //        }
    //    }
    //}


}