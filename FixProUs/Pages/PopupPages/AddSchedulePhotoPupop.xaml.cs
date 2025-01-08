using Mopups.Services;

namespace FixProUs.Pages.PopupPages;

public partial class AddSchedulePhotoPupop : Mopups.Pages.PopupPage
{
	public AddSchedulePhotoPupop()
	{
		InitializeComponent();
	}

    private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        await MopupService.Instance.PopAsync();
    }
}