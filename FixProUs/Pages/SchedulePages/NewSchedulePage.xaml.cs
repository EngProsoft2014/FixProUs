using FixProUs.Models;
using FixProUs.ViewModels;
using Microsoft.Maui.Maps;
using Syncfusion.Maui.Inputs;
using System.Collections.ObjectModel;

namespace FixProUs.Pages.SchedulePages;

public partial class NewSchedulePage : Controls.CustomsPage
{
    SchedulesViewModel ViewModel { get => BindingContext as SchedulesViewModel; set => BindingContext = value; }

    public NewSchedulePage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (ViewModel?.CustomerDetails != null)
        {
            if (!string.IsNullOrEmpty(ViewModel.CustomerDetails.locationlatitude) && !string.IsNullOrEmpty(ViewModel.CustomerDetails.locationlongitude))
            {
                ObservableCollection<CustomersModel> LstCust = new ObservableCollection<CustomersModel>();
                LstCust.Add(ViewModel.CustomerDetails);

                map.ItemsSource = LstCust;
                map.Pins.FirstOrDefault().Label = ViewModel.CustomerDetails.Address;

                map.MoveToRegion(MapSpan.FromCenterAndRadius(
                new Location(double.Parse(ViewModel.CustomerDetails.locationlatitude), double.Parse(ViewModel.CustomerDetails.locationlongitude)), Distance.FromMiles(2)));
            }
        }

    }

    private async void TapGestureRecognizer_Tapped1(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
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
        await App.Current.MainPage.Navigation.PopAsync();
    }

    private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        await App.Current.MainPage.Navigation.PopAsync();
    }


    private async void map_MapClicked(object sender, Microsoft.Maui.Controls.Maps.MapClickedEventArgs e)
    {
        if (ViewModel?.ScheduleDetails?.Id != 0)
        {
            var location = new Location(double.Parse(ViewModel?.CustomerDetails?.locationlatitude), double.Parse(ViewModel?.CustomerDetails?.locationlongitude));

            //var location = new Location(31.199629, 29.918674);

            var options = new MapLaunchOptions { NavigationMode = NavigationMode.Driving };

            await Map.OpenAsync(location, options);
        }

    }


    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedOption = (sender as Picker).SelectedItem;
        if (selectedOption != null)
        {
            ViewModel.SelectedEmpCategoryCommand.Execute(selectedOption);
        }
    }


    private async void Button_Clicked_2(object sender, EventArgs e)
    {
        //ViewModel.ScheduleDetails.GetPictures = true; //In Get Pictures Case Only
        var popupView = new SchedulesViewModel(ViewModel.ScheduleDetails);
        var page = new Pages.SchedulePages.SchedulePicturesPage();
        page.BindingContext = popupView;
        await App.Current.MainPage.Navigation.PushAsync(page);
        //await App.Current.MainPage.Navigation.PushAsync(new SchedulePicturesPage());
    }


}