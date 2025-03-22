using FixProUs.ViewModels;
using Mopups.Services;

namespace FixProUs.Pages.CallPages;

public partial class NewCallPage : Controls.CustomsPage
{
    CallsViewModel ViewModel { get => BindingContext as CallsViewModel; set => BindingContext = value; }

    private bool _isHandlingFocusPhone;
    private bool _isHandlingFocusCustName;

    public NewCallPage()
    {
        InitializeComponent();
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();


        if (string.IsNullOrEmpty(entryNameGet.Text))
        {
            entryNameGet.Text = "Please Choose Customer";
        }
    }

    private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        await App.Current!.MainPage!.Navigation.PopAsync();
    }

    //private void actIndLoading_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    //{
    //    if (actIndLoading.IsRunning == true)
    //        this.IsEnabled = false;
    //    else
    //        this.IsEnabled = true;
    //}

    private async void entryPhone_Focused(object sender, FocusEventArgs e)
    {



    }

    private void entryPhone_Unfocused(object sender, FocusEventArgs e)
    {

        ViewModel.IsEnable = false;
        var entry = (Entry)sender;
        entry.IsReadOnly = false;
        ViewModel.IsEnable = true;
        _isHandlingFocusPhone = false;
    }

    private async void TapGestureRecognizer_Tapped_Phone(object sender, TappedEventArgs e)
    {
        if (_isHandlingFocusPhone) return;
        _isHandlingFocusPhone = true;

        ViewModel.IsEnable = false;

        entryPhone.IsReadOnly = true;

        try
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
                return;
            }
            else
            {
                var popupView = new Pages.CallPages.SearchCustomerPopup();

                popupView.DidClose += (str) =>
                {
                    entryPhone.Text = ViewModel.OneCall.PhoneNum = str.Phone1;
                    ViewModel.OneCall.CustomerId = str.Id;
                    ViewModel.OneCall.CustomerName = str.FirstName + " " + str.LastName;

                    if (!string.IsNullOrEmpty(str.FirstName) && !string.IsNullOrEmpty(str.LastName))
                    {
                        stkCustNameGet.IsVisible = true;
                        entryNameGet.Text = str.FirstName + " " + str.LastName;
                        entryNameGet.IsEnabled = false;
                    }
                    else
                    {
                        ViewModel.OneCall.CustomerName = entryNameGet.Text = "";
                        entryNameGet.Text = "Enter customer";
                        entryNameGet.IsEnabled = true;
                    }
                };

                await MopupService.Instance.PushAsync(popupView);
            }
        }
        catch (Exception)
        {
            await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
            throw;
        }

        ViewModel.IsEnable = true;

        _isHandlingFocusPhone = false;

        entryPhone.IsReadOnly = false;

    }

    private async void TapGestureRecognizer_Tapped_CustName(object sender, EventArgs e)
    {
        if (ViewModel?.OneCall?.Id != 0 && ViewModel?.OneCall?.ScheduleDateId == 0 || ViewModel?.OneCall?.ScheduleDateId == null)
        {
            if (_isHandlingFocusCustName) return;
            _isHandlingFocusCustName = true;

            entryNameGet.IsEnabled = false;

            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await App.Current!.MainPage!.DisplayAlert("Error", "No Internet connection!", "OK");
                    return;
                }
                else
                {
                    if ((ViewModel?.OneCall?.CustomerId == 0 || ViewModel?.OneCall?.CustomerId == null) && entryPhone.Text != null)
                    {
                        var popupView = new Pages.CallPages.SearchCustomerPopup();

                        popupView.DidClose += (str) =>
                        {
                            //ViewModel.OneCall.PhoneNum = str.Phone1;
                            ViewModel!.OneCall.CustomerId = str.Id;
                            ViewModel.OneCall.CustomerName = str.FirstName + " " + str.LastName;

                            if (!string.IsNullOrEmpty(str.FirstName) && !string.IsNullOrEmpty(str.LastName))
                            {
                                stkCustNameGet.IsVisible = true;
                                entryNameGet.Text = str.FirstName + " " + str.LastName;
                            }
                            else
                            {
                                entryNameGet.Text = "Please Choose Customer";
                            }
                        };

                        await MopupService.Instance.PushAsync(popupView);
                    }
                }
            }

            catch (Exception)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", "No Internet connection!", "OK");
                throw;
            }

            entryNameGet.IsEnabled = true;

            _isHandlingFocusCustName = false;
        }

    }





    //private async void entryNameGet_Focused(object sender, FocusEventArgs e)
    //{
    //    if (e.IsFocused == true)
    //    {
    //        ViewModel.IsEnable = false;
    //        var entry = (Entry)sender;
    //        entry.IsEnabled = false;

    //        try
    //        {
    //            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
    //            {
    //                await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
    //                return;
    //            }
    //            else
    //            {

    //                if (ViewModel.OneCall.CustomerId == null)
    //                {
    //                    var popupView = new Pages.CallPages.SearchCustomerPopup();

    //                    popupView.DidClose += (str) =>
    //                    {
    //                        //ViewModel.OneCall.PhoneNum = str.Phone1;
    //                        ViewModel.OneCall.CustomerId = str.Id;
    //                        ViewModel.OneCall.CustomerName = str.FirstName + " " + str.LastName;

    //                        if (!string.IsNullOrEmpty(str.FirstName) && !string.IsNullOrEmpty(str.LastName))
    //                        {
    //                            stkCustNameGet.IsVisible = true;
    //                            entryNameGet.Text = str.FirstName + " " + str.LastName;
    //                        }
    //                        else
    //                        {
    //                            entryNameGet.Placeholder = "Please Choose Customer";
    //                        }
    //                    };

    //                    await MopupService.Instance.PushAsync(popupView);
    //                }

    //                ViewModel.IsEnable = true;
    //            }
    //        }

    //        catch (Exception)
    //        {
    //            await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
    //            throw;
    //        }
    //    }

    //}

    //private void entryNameGet_Unfocused(object sender, FocusEventArgs e)
    //{
    //    ViewModel.IsEnable = false;
    //    var entry = (Entry)sender;
    //    entry.IsEnabled = true;
    //    ViewModel.IsEnable = true;
    //}
}