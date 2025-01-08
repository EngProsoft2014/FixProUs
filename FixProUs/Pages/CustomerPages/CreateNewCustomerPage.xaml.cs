using FixProUs.ViewModels;

namespace FixProUs.Pages.CustomerPages;

public partial class CreateNewCustomerPage : Controls.CustomsPage
{
    CustomersViewModel ViewModel { get => BindingContext as CustomersViewModel; set => BindingContext = value; }

    public CreateNewCustomerPage()
	{
		InitializeComponent();
	}

    private void rdBtnYes_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value == true)
        {
            ViewModel.CustomerDetails.Discount = null;
            stkMember.IsVisible = true;
            stkDiscount.IsVisible = false;
        }
        else
        {
            ViewModel.CustomerDetails.MemberDTO = null;
            stkMember.IsVisible = false;
            stkDiscount.IsVisible = true;
        }
    }

    private void rdBtnNo_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value == true)
        {
            ViewModel.CustomerDetails.MemberDTO = null;
            stkMember.IsVisible = false;
            stkDiscount.IsVisible = true;
        }
        else
        {
            ViewModel.CustomerDetails.Discount = null;
            stkMember.IsVisible = true;
            stkDiscount.IsVisible = false;
        }
    }

    private void swtTaxable_Toggled(object sender, ToggledEventArgs e)
    {
        if (e.Value == true)
        {
            stkTexable.IsVisible = true;
        }
        else
        {
            stkTexable.IsVisible = false;
        }
    }

    private void Picker_SelectedIndexChanged_1(object sender, EventArgs e)
    {
        var selectedOption = (sender as Picker).SelectedItem;
        ViewModel?.ChooseCustomerCategoryCommand.Execute(selectedOption);
    }

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedOption = (sender as Picker).SelectedItem;
        ViewModel?.ChooseCustomerMemberShipCommand.Execute(selectedOption);
    }

    private void Picker_SelectedIndexChanged_2(object sender, EventArgs e)
    {
        var selectedOption = (sender as Picker).SelectedItem;
        ViewModel?.ChooseCustomerTaxCommand.Execute(selectedOption);
    }

    private void Picker_SelectedIndexChanged_3(object sender, EventArgs e)
    {
        var selectedOption = (sender as Picker).SelectedItem;
        ViewModel?.ChooseCustomerCampaignCommand.Execute(selectedOption);
    }

    //Address
    private void Entry_Focused(object sender, FocusEventArgs e)
    {
        var selectedOption = (sender as Entry).Text;
        ViewModel?.SelecteAddressCommand.Execute(selectedOption);
    }

}