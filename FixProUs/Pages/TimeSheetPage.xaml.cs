using FixProUs.ViewModels;

namespace FixProUs.Pages;

public partial class TimeSheetPage : Controls.CustomsPage
{
    TimeSheetViewModel ViewModel { get => BindingContext as TimeSheetViewModel; set => BindingContext = value; }

    public TimeSheetPage()
    {
        InitializeComponent();

        if (lstEmployeesIn.ItemsSource.Equals(0))
        {
            stkNoData.IsVisible = true;
        }

    }


    //private void actIndLoading_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    //{
    //    if (actIndLoading.IsRunning == true)
    //        this.IsEnabled = false;
    //    else
    //        this.IsEnabled = true;
    //}

    //Employees In
    private void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
    {
        lstEmployeesOut.IsVisible = false;
        lstEmployeesIn.IsVisible = true;
        yumCheckIn.BackgroundColor = Color.FromHex("#b66dff");
        lblCheckIn.TextColor = Colors.White;

        yumCheckOut.BackgroundColor = Color.FromHex("#eedeff");
        lblCheckOut.TextColor = Colors.Black;

        if (ViewModel.LstEmployeesIn.Count == 0)
        {
            stkNoData.IsVisible = true;
        }
        else
        {
            stkNoData.IsVisible = false;
        }
    }

    //Employees Out
    private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        lstEmployeesIn.IsVisible = false;
        lstEmployeesOut.IsVisible = true;

        yumCheckIn.BackgroundColor = Color.FromHex("#eedeff");
        lblCheckIn.TextColor = Colors.Black;

        yumCheckOut.BackgroundColor = Color.FromHex("#b66dff");
        lblCheckOut.TextColor = Colors.White;

        if (ViewModel.LstEmployeesOut.Count == 0)
        {
            stkNoData.IsVisible = true;
        }
        else
        {
            stkNoData.IsVisible = false;
        }
    }


    private void srcBarEmployee_TextChanged(object sender, TextChangedEventArgs e)
    {
        lstEmployeesIn.ItemsSource = ViewModel.LstEmployeesIn.Where(x => (x.EmployeeName).Contains(srcBarEmployee.Text));
        lstEmployeesOut.ItemsSource = ViewModel.LstEmployeesOut.Where(x => (x.EmployeeName).Contains(srcBarEmployee.Text));
    }


    private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}