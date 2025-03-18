using FixProUs.ViewModels;
using Mopups.Services;
using Syncfusion.Maui.Inputs;
using System.Threading.Tasks;

namespace FixProUs.Pages.SchedulePages;

public partial class ScheduleJobDetailsPage : Controls.CustomsPage
{
    SchedulesViewModel ViewModel { get => BindingContext as SchedulesViewModel; set => BindingContext = value; }

    public ScheduleJobDetailsPage()
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

    //Cancel
    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private void SfComboBox_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    {
        var selectedOption = (sender as SfComboBox)!.SelectedItem;
        ViewModel.SelectedEmpCategoryCommand.Execute(selectedOption);
    }

    //Not Serviced
    private void Button_Clicked_1(object sender, EventArgs e)
    {
        stkStEdTime.IsVisible = false;
        stkSpHourMin.IsVisible = false;
        stkButtons.IsVisible = false;
        stkResponNotServ.IsVisible = true;
        stkResponButtons.IsVisible = true;

        stkAddJob.IsVisible = false;
        stkAddJobDateButtons.IsVisible = false;
        stkMain.IsVisible = false;
    }

    //Back Btn (Not Serviced)
    private void Button_Clicked_2(object sender, EventArgs e)
    {
        stkStEdTime.IsVisible = true;
        stkSpHourMin.IsVisible = true;
        stkButtons.IsVisible = true;
        stkResponNotServ.IsVisible = false;
        stkResponButtons.IsVisible = false;

        stkMain.IsVisible = true;
    }

    //NO Btn (New Visit)
    private void Button_Clicked_3(object sender, EventArgs e)
    {
        stkStEdTime.IsVisible = true;
        stkSpHourMin.IsVisible = true;
        stkButtons.IsVisible = true;
        stkDate.IsVisible = true;
        stkAddJob.IsVisible = false;
        stkAddJobDateButtons.IsVisible = false;

        stkMain.IsVisible = true;
    }

    //New Visit
    private void Button_Clicked_4(object sender, EventArgs e)
    {

        stkReopenButtons.IsVisible = false;
        stkResponButtons.IsVisible = false;
        stkResponNotServ.IsVisible = false;
        stkMain.IsVisible = false;

        stkStEdTime.IsVisible = false;
        stkSpHourMin.IsVisible = false;
        stkButtons.IsVisible = false;
        stkDate.IsVisible = false;
        stkAddJob.IsVisible = true;
        stkAddJobDateButtons.IsVisible = true;
    }


    private async void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        await Navigation.PopAsync();
    }

    ////Not Serviced
    //private void Button_Clicked_1(object sender, EventArgs e)
    //{
    //    stkStEdTime.IsVisible = false;
    //    stkSpHourMin.IsVisible = false;
    //    stkButtons.IsVisible = false;
    //    stkResponNotServ.IsVisible = true;
    //    stkResponButtons.IsVisible = true;

    //    stkAddJobDate.IsVisible = false;
    //    stkAddJobTimes.IsVisible = false;
    //    stkAddJobEmployee.IsVisible = false;
    //    stkAddJobDateButtons.IsVisible = false;
    //    stkMain.IsVisible = false;
    //}

    ////Back Btn (Not Serviced)
    //private void Button_Clicked_2(object sender, EventArgs e)
    //{
    //    stkStEdTime.IsVisible = true;
    //    stkSpHourMin.IsVisible = true;
    //    stkButtons.IsVisible = true;
    //    stkResponNotServ.IsVisible = false;
    //    stkResponButtons.IsVisible = false;

    //    stkMain.IsVisible = true;
    //}

    ////NO Btn (New Visit)
    //private void Button_Clicked_3(object sender, EventArgs e)
    //{
    //    stkStEdTime.IsVisible = true;
    //    stkSpHourMin.IsVisible = true;
    //    stkButtons.IsVisible = true;
    //    stkDate.IsVisible = true;
    //    stkAddJobDate.IsVisible = false;
    //    stkAddJobTimes.IsVisible = false;
    //    stkAddJobEmployee.IsVisible = false;
    //    stkAddJobDateButtons.IsVisible = false;

    //    stkMain.IsVisible = true;
    //}

    ////New Visit
    //private void Button_Clicked_4(object sender, EventArgs e)
    //{

    //    stkReopenButtons.IsVisible = false;
    //    stkResponButtons.IsVisible = false;
    //    stkResponNotServ.IsVisible = false;
    //    stkMain.IsVisible = false;

    //    stkStEdTime.IsVisible = false;
    //    stkSpHourMin.IsVisible = false;
    //    stkButtons.IsVisible = false;
    //    stkDate.IsVisible = false;
    //    stkAddJobDate.IsVisible = true;
    //    stkAddJobTimes.IsVisible = true;
    //    stkAddJobEmployee.IsVisible = true;
    //    stkAddJobDateButtons.IsVisible = true;
    //}

}