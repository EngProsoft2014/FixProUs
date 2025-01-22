using FixProUs.Models;
using FixProUs.ViewModels;
using Mopups.Services;
using System.Collections.ObjectModel;

namespace FixProUs.Pages.PopupPages;

public partial class EmployeesPopup : Mopups.Pages.PopupPage
{
    public delegate void EmployeesDelegte(List<EmployeeModel> Employees);
    public event EmployeesDelegte EmployeesClose;

    SchedulesViewModel ViewModel = new SchedulesViewModel();

    public EmployeesPopup()
	{
		InitializeComponent();
	}

    public EmployeesPopup(ObservableCollection<EmployeeModel> LstEmps)
    {
        InitializeComponent();
        lstEmployees.ItemsSource = ViewModel.LstEmpInOneCategory = LstEmps;
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
        this.IsEnabled = false;
        List<EmployeeModel> LstEmps = new List<EmployeeModel>();
        LstEmps = ViewModel.LstEmpInOneCategory.Where(x => x.IsChecked == true).ToList();

        if (LstEmps != null)
        {
            EmployeesClose.Invoke(LstEmps);
        }
        else
        {
            await App.Current!.MainPage!.DisplayAlert("Warning", "Please Choose Empolyee !!", "OK");
        }
        this.IsEnabled = true;
        await MopupService.Instance.PopAsync();
    }
}