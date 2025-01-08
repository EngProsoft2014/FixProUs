using CommunityToolkit.Maui.Alerts;
using FixProUs.ViewModels;

namespace FixProUs.Pages.CustomerPages;

public partial class CashOrCreditPaymentPage : Controls.CustomsPage
{
    PaymentsViewModel ViewModel { get => BindingContext as PaymentsViewModel; set => BindingContext = value; }

    public CashOrCreditPaymentPage()
	{
		InitializeComponent();
	}

    private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        if (ViewModel != null && ViewModel.OneInvoice.Id == 0)
        {
            await App.Current!.MainPage!.Navigation.PushAsync(new MainPage());
        }
        else
        {
            await Navigation.PopAsync();
        }
    }

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.NewTextValue) != true)
        {
            ViewModel.OnePayment.Amount = decimal.Parse(e.NewTextValue);
            btnPayCredit.Text = string.Format("Pay USD ${0}", e.NewTextValue);
            btnPayCash.Text = string.Format("Pay USD ${0}", e.NewTextValue);
            lblPayCash.Text = e.NewTextValue;
        }
        else
        {
            ViewModel.OnePayment.Amount = ViewModel.OneInvoice.Net;
            btnPayCredit.Text = string.Format("Pay USD ${0}", ViewModel.OneInvoice.Net);
            btnPayCash.Text = string.Format("Pay USD ${0}", ViewModel.OneInvoice.Net);
            lblPayCash.Text = ViewModel.OneInvoice.Net.ToString();
        }
    }

    private void swtPayCredit_Toggled(object sender, ToggledEventArgs e)
    {
        if (e.Value == true)
        {
            ViewModel.OnePayment.Amount = ViewModel.OneInvoice.Net;
            btnPayCredit.Text = string.Format("Pay USD ${0}", ViewModel.OneInvoice.Net);
            btnPayCash.Text = string.Format("Pay USD ${0}", ViewModel.OneInvoice.Net);
            lblPayCash.Text = ViewModel.OneInvoice.Net.ToString();
        }
        else
        {
            if (string.IsNullOrEmpty(entryNewAmount.Text) != true)
            {
                ViewModel.OnePayment.Amount = decimal.Parse(entryNewAmount.Text);
                btnPayCredit.Text = string.Format("Pay USD ${0}", entryNewAmount.Text);
            }
            else if (string.IsNullOrEmpty(entryCashNewAmount.Text) != true)
            {
                ViewModel.OnePayment.Amount = decimal.Parse(entryCashNewAmount.Text);
                btnPayCash.Text = string.Format("Pay USD ${0}", entryCashNewAmount.Text);
                lblPayCash.Text = entryCashNewAmount.Text;
            }
            else
            {
                ViewModel.OnePayment.Amount = ViewModel.OneInvoice.Net;
                btnPayCredit.Text = string.Format("Pay USD ${0}", ViewModel.OneInvoice.Net);
                btnPayCash.Text = string.Format("Pay USD ${0}", ViewModel.OneInvoice.Net);
                lblPayCash.Text = ViewModel.OneInvoice.Net.ToString();
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


    //Save Credit
    private void Button_Clicked_Clear_Credit(object sender, EventArgs e)
    {
        DrawBoardCredit.Lines.Clear();
        ViewModel.SignatureImageByte64 = "";
    }

    private async void Button_Clicked_Save_Credit(object sender, EventArgs e)
    {
        var stream = await DrawBoardCredit.GetImageStream(300, 300);
        ViewModel.SignatureImageByte64 = Convert.ToBase64String(Helpers.Utility.ReadToEnd(stream));
        var toast = Toast.Make("Success for save your signature", CommunityToolkit.Maui.Core.ToastDuration.Long, 15);
        await toast.Show();
    }

    //private async void Button_Clicked(object sender, EventArgs e)
    //{

    //    ImageSource? source = signaturePadCredit.ToImageSource();

    //    // Generate random file name
    //    string fileName = Path.GetRandomFileName() + ".png";

    //    string folderName = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    //    string path = Path.Combine(folderName, fileName);

    //    byte[] bytes = File.ReadAllBytes(path);

    //    ViewModel.SignatureImageByte64 = Convert.ToBase64String(bytes);

    //    //Helpers.Messages.ShowSuccessSnackBar("Success for save your signature");
    //    var toast = Toast.Make("Success for save your signature", CommunityToolkit.Maui.Core.ToastDuration.Long, 15);
    //    await toast.Show();
    //}

    ////Clear Credit
    //private void Button_Clicked_1(object sender, EventArgs e)
    //{
    //    signaturePadCredit.Clear();
    //}

    //Save Cash




    //Cash

    private void Button_Clicked_Clear_Cash(object sender, EventArgs e)
    {
        DrawBoardCash.Lines.Clear();
        ViewModel.SignatureImageByte64 = "";
    }

    private async void Button_Clicked_Save_Cash(object sender, EventArgs e)
    {
        var stream = await DrawBoardCash.GetImageStream(300, 300);
        ViewModel.SignatureImageByte64 = Convert.ToBase64String(Helpers.Utility.ReadToEnd(stream));
        var toast = Toast.Make("Success for save your signature", CommunityToolkit.Maui.Core.ToastDuration.Long, 15);
        await toast.Show();
    }

    //private async void Button_Clicked_2(object sender, EventArgs e)
    //{

    //    ImageSource? source = signaturePadCash.ToImageSource();

    //    // Generate random file name
    //    string fileName = Path.GetRandomFileName() + ".png";

    //    string folderName = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    //    string path = Path.Combine(folderName, fileName);

    //    byte[] bytes = File.ReadAllBytes(path);

    //    ViewModel.SignatureImageByte64 = Convert.ToBase64String(bytes);

    //    //Helpers.Messages.ShowSuccessSnackBar("Success for save your signature");
    //    var toast = Toast.Make("Success for save your signature", CommunityToolkit.Maui.Core.ToastDuration.Long, 15);
    //    await toast.Show();
    //}

    ////Clear Cash
    //private void Button_Clicked_3(object sender, EventArgs e)
    //{
    //    signaturePadCash.Clear();
    //}
}