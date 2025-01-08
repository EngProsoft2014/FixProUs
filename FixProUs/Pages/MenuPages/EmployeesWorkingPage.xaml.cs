using FixProUs.ViewModels;

namespace FixProUs.Pages.MenuPages;

public partial class EmployeesWorkingPage : Controls.CustomsPage
{
    EmployeesViewModel ViewModel { get => BindingContext as EmployeesViewModel; set => BindingContext = value; }

    public EmployeesWorkingPage()
	{
		InitializeComponent();
	}

    private void srcBarEmployee_TextChanged(object sender, TextChangedEventArgs e)
    {
        lstEmployees.ItemsSource = ViewModel.LstWorkingEmployees.Where(x => (x.FirstName.ToLower() + " " + x.LastName.ToLower()).Contains(srcBarEmployee.Text.ToLower()));
    }

    //private void actIndLoading_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    //{
    //    if (actIndLoading.IsRunning == true)
    //        this.IsEnabled = false;
    //    else
    //        this.IsEnabled = true;
    //}
}