using FixProUs.Models;
using FixProUs.ViewModels;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace FixProUs.Pages.MenuPages;

public partial class TrckingMapPage : Controls.CustomsPage
{
    EmployeesViewModel ViewModel { get => BindingContext as EmployeesViewModel; set => BindingContext = value; }

    public TrckingMapPage()
	{
		InitializeComponent();
	}

    public TrckingMapPage(DataMapsModel dataMap)
    {
        InitializeComponent();

        Pin pin = new Pin
        {
            Label = dataMap.Id.ToString(),
            //Address = "The city with a boardwalk",
            Type = PinType.Place,
            Location = new Location(dataMap.MPosition)
        };
        myMap.Pins.Add(pin);

        myMap.MoveToRegion(MapSpan.FromCenterAndRadius(
               new Location(double.Parse(dataMap.Lat), double.Parse(dataMap.Long)), Distance.FromMeters(200)));
    }

    private void myMap_MapClicked(object sender, MapClickedEventArgs e)
    {
        myMap.MoveToRegion(MapSpan.FromCenterAndRadius(
            new Location(double.Parse(ViewModel.LastListmap.LastOrDefault().Lat), double.Parse(ViewModel.LastListmap.LastOrDefault().Long)), Distance.FromMeters(100)));
    }

}