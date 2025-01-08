namespace FixProUs.Pages.PlansPages;

public partial class ChoosePlanPage : Controls.CustomsPage
{
	public ChoosePlanPage()
	{
		InitializeComponent();
	}

    private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

}