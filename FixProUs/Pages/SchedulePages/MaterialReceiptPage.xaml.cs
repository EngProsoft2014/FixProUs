using FixProUs.Models;
using FixProUs.Pages.PopupPages;
using FixProUs.ViewModels;

namespace FixProUs.Pages.SchedulePages;

public partial class MaterialReceiptPage : Controls.CustomsPage
{

    ScheduleMaterialReceiptViewModel ViewModel { get => BindingContext as ScheduleMaterialReceiptViewModel; set => BindingContext = value; }

    public MaterialReceiptPage()
    {
        InitializeComponent();
    }

    public MaterialReceiptPage(ScheduleMaterialReceiptModel model, ScheduleMaterialReceiptViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
        //ViewModel.OneSupplier.Id = ;
        //ViewModel.OneSupplier.FirstName = model.SupplierName;

        //comxLstSuppliers.Text = model.SupplierName;
        comxLstSuppliers.SelectedItem = ViewModel.OneSupplier;
        entryCost.Text = model.Cost.ToString();
        edtNotes.Text = model.Notes;
        imgReceipt.Source = ImageSource.FromUri(new Uri(model.ReceiptPhotoView));

        comxLstSuppliers.IsEnabled = false;
        entryCost.IsReadOnly = true;
        edtNotes.IsReadOnly = true;
        stkImgBtns.IsEnabled = false;
        stkBtns.IsVisible = false;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
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
        await App.Current!.MainPage!.Navigation.PopAsync();
    }


    private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        await App.Current!.MainPage!.Navigation.PopAsync();
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        await App.Current!.MainPage!.Navigation.PushAsync(new AddSchedulePhotoPupop());
    }

    private void comxLstSuppliers_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedOption = (sender as Picker).SelectedItem;
        ViewModel?.SelectSupplierCommand.Execute(selectedOption);
    }
}