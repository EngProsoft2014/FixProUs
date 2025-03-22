using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Controls.UserDialogs.Maui;
using FixProUs.Models;
using FixProUs.Pages;
using Stripe;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixProUs.ViewModels
{
    public partial class PlansViewModel : BaseViewModel
    {
        readonly Services.Data.ServicesService _service = new Services.Data.ServicesService();
        Helpers.GenericRepository ORep = new Helpers.GenericRepository();

        [ObservableProperty]
        PlansModel onePlan;
        [ObservableProperty]
        ObservableCollection<PlansModel> lstPricing;
        [ObservableProperty]
        StripeAccountModel stripeModel;
        [ObservableProperty]
        string cardNumber;
        [ObservableProperty]
        string holderName;
        [ObservableProperty]
        string expirationDate;
        [ObservableProperty]
        string cvv;
        [ObservableProperty]
        bool isMonthly;
        [ObservableProperty]
        bool isYearly = true;


        public PlansViewModel() 
        {
            LstPricing = new ObservableCollection<PlansModel>();
            OnePlan = new PlansModel();
            Init();
        }

        public PlansViewModel(PlansModel model)
        {
            OnePlan = model;
        }

        async void Init()
        {
            await GetPlans();
        }

        async Task GetPlans()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                UserDialogs.Instance.ShowLoading();

                var json = await ORep.GetAsync<ObservableCollection<PlansModel>>("api/Plans/GetPlans");

                if (json != null && json.Count != 0)
                {
                    LstPricing = json;
                }

                UserDialogs.Instance.HideHud();
            }
        }

        [RelayCommand]
        async Task SelectedPlan(PlansModel model)
        {
            var ViewModel = new PlansViewModel(model);
            var page = new Pages.PlansPages.PlanPaymentPage();
            page.BindingContext = ViewModel;
            await App.Current!.MainPage!.Navigation.PushAsync(page);
        }

        [RelayCommand]
        async Task CreditPayNow(PlansModel model)
        {
            IsEnable = false;

            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await App.Current!.MainPage!.DisplayAlert("Error", "No Internet connection!", "OK");
                    return;
                }
                else
                {
                    UserDialogs.Instance.ShowLoading();
                    OnePlan = model;

                    if (string.IsNullOrEmpty(CardNumber))
                    {
                        await App.Current!.MainPage!.DisplayAlert("Alert", $"Please Complete This Field Required : Card Number.", "Ok");
                    }
                    else if (string.IsNullOrEmpty(HolderName))
                    {
                        await App.Current!.MainPage!.DisplayAlert("Alert", $"Please Complete This Field Required : Holder Name.", "Ok");
                    }
                    else if (string.IsNullOrEmpty(ExpirationDate))
                    {
                        await App.Current!.MainPage!.DisplayAlert("Alert", $"Please Complete This Field Required : Expiration Date.", "Ok");
                    }
                    else if (string.IsNullOrEmpty(Cvv))
                    {
                        await App.Current!.MainPage!.DisplayAlert("Alert", $"Please Complete This Field Required : CVV.", "Ok");
                    }
                    else
                    {
                        if ((IsYearly == true && IsMonthly == false) || (IsYearly == false && IsMonthly == true))
                        {
                            if (IsYearly == true && IsMonthly == false)
                            {
                                //await PayViaStripe(model.AnnualPrice);
                                await PayForTest(model, true);
                            }
                            else
                            {
                                //await PayViaStripe(model.MonthlyPrice);
                                await PayForTest(model, false);
                            }
                        }
                        else
                        {
                            await App.Current!.MainPage!.DisplayAlert("Error", "Please Select Billing Cycle", "OK");
                        }
                    }

                    UserDialogs.Instance.HideHud();
                }
            }
            catch (Exception)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", "Please complete all the payment fields", "OK");
            }

            IsEnable = true;
        }

        public async Task InitiolizModel(PlansModel model)
        {
            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await App.Current!.MainPage!.DisplayAlert("Error", "No Internet connection!", "OK");
                    return;
                }
                else
                {

                    //OnePayment.AccountId = model.AccountId;
                    //OnePayment.BrancheId = model.BrancheId;
                    //OnePayment.CustomerId = model.CustomerId;
                    ////OnePayment.ContractId = model.ContractId;
                    //OnePayment.InvoiceId = model.Id;
                    ////OnePayment.ExpensesId = model.ExpensesId;
                    //OnePayment.PaymentDate = DateTime.Now;
                    //OnePayment.SignatureDraw = SignatureImageByte64;

                    //OnePayment.Amount = OnePayment.Amount == null ? model.Net : OnePayment.Amount;
                    //OnePayment.OverAmount = model.OverAmount;

                    //OnePayment.IncreaseDecrease = model.IncreaseDecrease;
                    //OnePayment.TransactionID = model.TransactionID;
                    //OnePayment.CheckNumber = model.CheckNumber;
                    //OnePayment.BankName = model.BankName;
                    //OnePayment.AccountNumber = model.AccountNumber;
                    //OnePayment.Notes = model.Notes;
                    //OnePayment.Active = model.Active;
                    //OnePayment.CreateUser = model.CreateUser;
                    //OnePayment.CreateDate = model.CreateDate;

                    //UserDialogs.Instance.ShowLoading();
                    ////var json = await Helpers.Utility.PostData("api/Payments/InsertPayment", JsonConvert.SerializeObject(OnePayment));
                    //var json = await ORep.PostDataAsync("api/Payments/InsertPayment", OnePayment, UserToken);
                    //UserDialogs.Instance.HideLoading();

                    //if (json != null && json != "api not responding")
                    //{
                    //    await App.Current!.MainPage!.DisplayAlert("FixPro", "Payment completed successfully.", "Ok");
                    //    await App.Current!.MainPage!.Navigation.PushAsync(new MainPage());
                    //}
                    //else
                    //{
                    //    await App.Current!.MainPage!.DisplayAlert("FixPro", "Payment not accepted", "Ok");
                    //}


                }
            }
            catch (Exception ex)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
            }

        }

        async Task GetSkretKey(int? AccountId, int? BranchId)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                UserDialogs.Instance.ShowLoading();

                var json = await ORep.GetAsync<StripeAccountModel>(string.Format("api/Login/GetStripeAccount?" + "AccountId=" + AccountId + "&" + "BranchId=" + BranchId));

                if (json != null)
                {
                    StripeModel = json;
                }

                UserDialogs.Instance.HideHud();
            }
        }

        public async Task PayViaStripe(string Price)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                await GetSkretKey(int.Parse(Helpers.Settings.AccountIdGet), int.Parse(Helpers.Settings.BranchIdGet));

                StripeConfiguration.ApiKey = StripeModel.SecretKey;

                //StripeConfiguration.ApiKey = "sk_test_IHINMHgrNTLUWqh3IcTcMdNB";

                // step 2: Assign card to token object
                var stripeCard = new TokenCreateOptions
                {
                    Card = new TokenCardOptions
                    {
                        Number = CardNumber,
                        Name = HolderName,
                        ExpMonth = ExpirationDate.Split('/')[0],
                        ExpYear = ExpirationDate.Split('/')[1],
                        Cvc = Cvv,
                    }
                };

                Stripe.TokenService service = new Stripe.TokenService();
                Stripe.Token newToken = service.Create(stripeCard);

                // step 3: assign the token to the source
                var option = new SourceCreateOptions
                {
                    Type = SourceType.Card,
                    Currency = "USD",
                    Token = newToken.Id
                };

                var sourceService = new SourceService();
                Stripe.Source source = sourceService.Create(option);

                // step 4: create customer
                CustomerCreateOptions customer = new CustomerCreateOptions
                {
                    Name = Helpers.Settings.UserFristNameGet + "" + Helpers.Settings.UserLastNameGet,
                    Email = Helpers.Settings.EmailGet,
                    Description = OnePlan.Name,
                };

                var customerService = new CustomerService();
                var cust = customerService.Create(customer);

                // step 5: charge option
                var chargeoption = new ChargeCreateOptions
                {
                    Amount = Convert.ToInt64(decimal.Parse(Price) * 100),
                    Currency = "USD",
                    ReceiptEmail = Helpers.Settings.EmailGet,
                    Customer = cust.Id,
                    Source = source.Id
                };

                // step 6: charge the customer
                var chargeService = new ChargeService();
                Charge charge = chargeService.Create(chargeoption);
                if (charge.Status == "succeeded")
                {
                    // success
                    await InitiolizModel(OnePlan);
                }
                else
                {
                    // failed
                    await App.Current!.MainPage!.DisplayAlert("Alert", "Job payment failed!", "Ok");
                }
            }

        }



        public async Task PayForTest(PlansModel model, bool PriceType)
        {
            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await App.Current!.MainPage!.DisplayAlert("Error", "No Internet connection!", "OK");
                    return;
                }
                else
                {

                    string AnnualMonthly = "";
                    string Amount = "";
                    if (PriceType == true)
                    {
                        AnnualMonthly = "Annual";
                        Amount = model.AnnualPrice;
                    }
                    else
                    {
                        AnnualMonthly = "Monthly";
                        Amount = model.MonthlyPrice;
                    }

                    UserDialogs.Instance.ShowLoading();

                    var json = await ORep.GetAsync<string>("api/Login/GetAccountToPayUpgrade?Email=" + Helpers.Settings.EmailGet + "&Plan=" + model.Id + "&AnnualMonthly=" + AnnualMonthly + "&Amount=" + Amount + "&TransactionId=" + "12225" + "&OrderIdMySql=&UserIdMysql=");

                    UserDialogs.Instance.HideHud();

                    if (json == null || json == "")
                    {
                        await App.Current!.MainPage!.DisplayAlert("FixPro", "Payment completed successfully.", "Ok");
                        await App.Current!.MainPage!.Navigation.PushAsync(new LoginPage());
                    }
                    else
                    {
                        await App.Current!.MainPage!.DisplayAlert("FixPro", "Payment not accepted", "Ok");
                    }

                }
            }
            catch (Exception ex)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
            }

        }
    }
}
