using Mopups.Services;

namespace FixProUs.Pages.PopupPages;

public partial class ChangeAccountPhotoPupop : Mopups.Pages.PopupPage
{
	public ChangeAccountPhotoPupop()
	{
		InitializeComponent();
	}

    private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        await MopupService.Instance.PopAsync();
    }
}