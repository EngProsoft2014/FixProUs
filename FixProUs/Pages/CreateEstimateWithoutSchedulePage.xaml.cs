using CommunityToolkit.Maui.Alerts;
using FFImageLoading;
using FixProUs.ViewModels;
using Syncfusion.Maui.Core.Carousel;
using Syncfusion.Maui.Core.Internals;

namespace FixProUs.Pages;

public partial class CreateEstimateWithoutSchedulePage : Controls.CustomsPage
{
    SchedulesViewModel ViewModel { get => BindingContext as SchedulesViewModel; set => BindingContext = value; }

    public CreateEstimateWithoutSchedulePage()
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

    private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
    {
        stkEditDiscount.IsVisible = true;
    }

    private void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
    {
        stkEditDiscount.IsVisible = false;
    }

    private void entryDiscount_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (e.NewTextValue != null && e.NewTextValue != "")
        {
            pnkSave.IsVisible = true;
        }
        else
        {
            pnkSave.IsVisible = false;
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        ViewModel.TotalInvoice(ViewModel.ScheduleDetails, ViewModel.CustomerDetails);
    }

    private void ShowSignatureBtn_Clicked(object sender, EventArgs e)
    {
        ShowSignatureBtn.IsVisible = false;
        CloseSignatureBtn.IsVisible = true;
        frmSignature.IsVisible = true;
    }

    private void CloseSignatureBtn_Clicked(object sender, EventArgs e)
    {
        CloseSignatureBtn.IsVisible = false;
        ShowSignatureBtn.IsVisible = true;
        frmSignature.IsVisible = false;
    }


    private void Button_Clicked_Clear(object sender, EventArgs e)
    {
        DrawBoard.Lines.Clear();
        ViewModel.SignatureImageByte64Estimate = "";
    }

    private async void Button_Clicked_Save(object sender, EventArgs e)
    {
        var stream = await DrawBoard.GetImageStream(300, 300);
        ViewModel.SignatureImageByte64Estimate = Convert.ToBase64String(Helpers.Utility.ReadToEnd(stream));
        var toast = Toast.Make("Success for save your signature", CommunityToolkit.Maui.Core.ToastDuration.Long, 15);
        await toast.Show();
    }

    private void TapGestureRecognizer_Tapped_Pending(object sender, TappedEventArgs e)
    {
        if (chkPending.IsChecked == false)
        {
            chkPending.IsChecked = true;
            chkDeclind.Color = Color.FromHex("#333");
            chkDeclind.IsChecked = false;
            chkPending.Color = Color.FromHex("#b66dff");
            chkAccept.Color = Color.FromHex("#333");
            chkAccept.IsChecked = false;
            //colRequests.ItemsSource = ViewModel.Responses.Where(a => a.TotalPriceAgentAccept > 0);
        }
        else
        {
            if (chkDeclind.IsChecked == true || chkAccept.IsChecked == true)
            {
                chkPending.IsChecked = false;
            }
            else
            {
                chkPending.IsChecked = true;
            }
        }
    }

    private void TapGestureRecognizer_Tapped_Accept(object sender, TappedEventArgs e)
    {
        if (chkAccept.IsChecked == false)
        {
            chkAccept.IsChecked = true;
            chkDeclind.Color = Color.FromHex("#333");
            chkDeclind.IsChecked = false;
            chkPending.Color = Color.FromHex("#333");
            chkPending.IsChecked = false;
            chkAccept.Color = Color.FromHex("#b66dff");
        }
        else
        {
            if (chkDeclind.IsChecked == true || chkPending.IsChecked == true)
            {
                chkAccept.IsChecked = false;
            }
            else
            {
                chkAccept.IsChecked = true;
            }
        }
    }

    private void TapGestureRecognizer_Tapped_Declind(object sender, TappedEventArgs e)
    {
        if (chkDeclind.IsChecked == false)
        {
            chkDeclind.IsChecked = true;
            chkDeclind.Color = Color.FromHex("#b66dff");
            chkPending.Color = Color.FromHex("#333");
            chkPending.IsChecked = false;
            chkAccept.Color = Color.FromHex("#333");
            chkAccept.IsChecked = false;
        }
        else
        {
            if (chkPending.IsChecked == true || chkAccept.IsChecked == true)
            {
                chkDeclind.IsChecked = false;
            }
            else
            {
                chkDeclind.IsChecked = true;
            }
        }
    }




    ////Save Credit
    //private async void Button_Clicked_1(object sender, EventArgs e)
    //{

    //    ImageSource? source =  signaturePadEstimate.ToImageSource();

    //    if (source != null)
    //    {

    //        StreamImageSource streamImageSource = (StreamImageSource)source;

    //        System.Threading.CancellationToken cancellationToken = System.Threading.CancellationToken.None;

    //        Task<Stream> image = streamImageSource.Stream(cancellationToken);

    //        Stream stream = image.Result;

    //        byte[] bytes = new byte[stream.Length];

    //        stream.Read(bytes, 0, bytes.Length);

    //        string fileName = Path.GetRandomFileName() + ".png";
    //        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), fileName);

    //        using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
    //        {

    //            fileStream.Seek(0, SeekOrigin.End);

    //            await fileStream.WriteAsync(bytes, 0, bytes.Length);


    //            ViewModel.SignatureImageByte64Estimate = Convert.ToBase64String(bytes);

    //            //Helpers.Messages.ShowSuccessSnackBar("Success for save your signature");
    //            var toast = Toast.Make("Success for save your signature", CommunityToolkit.Maui.Core.ToastDuration.Long, 15);
    //            await toast.Show();
    //        }

    //    }

    //}


    ////Clear Credit
    //private void Button_Clicked_2(object sender, EventArgs e)
    //{
    //    signaturePadEstimate.Clear();
    //}


}