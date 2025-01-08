
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Controls.UserDialogs.Maui;
using FixProUs.Models;
using FixProUs.Pages;
using FixProUs.ViewModels;
using GoogleApi.Entities.Maps.StreetView.Request.Enums;
using GoogleApi.Entities.Translate.Common.Enums;
using Newtonsoft.Json;
using Stripe;
using Stripe.Issuing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Dynamic;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Input;
using Twilio.Jwt.AccessToken;


namespace FixProUs.ViewModels
{
    public partial class PaymentsViewModel : BaseViewModel
    {

        readonly Services.Data.ServicesService _service = new Services.Data.ServicesService();

        [ObservableProperty]
        InvoiceModel oneInvoice;

        [ObservableProperty]
        PaymentsModel onePayment;

        [ObservableProperty]
        CustomersModel customerDetails;

        [ObservableProperty]
        int cashOrCredit;

        [ObservableProperty]
        string cardNumber;

        [ObservableProperty]
        string holderName;

        [ObservableProperty]
        string expirationDate;

        [ObservableProperty]
        string signatureImageByte64;

        [ObservableProperty]
        string cvv;

        [ObservableProperty]
        StripeAccountModel stripeModel;

        Helpers.GenericRepository ORep = new Helpers.GenericRepository();


        //public ICommand ScanCommand
        //{
        //    get
        //    {
        //        return new Command(async () =>
        //        {
        //            try
        //            {
        //                Card = await CrossPayCards.Current.ScanAsync().ConfigureAwait(true);
        //            }
        //            catch (Exception ex)
        //            {
        //                Debug.WriteLine(ex);
        //            }
        //        });
        //    }
        //}

        //private PayCard _card;
        //public PayCard Card
        //{
        //    set
        //    {
        //        if (_card != value)
        //        {
        //            _card = value;

        //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Card)));
        //        }
        //    }
        //    get 
        //    {
        //        CardNumber = _card.CardNumber;
        //        HolderName = _card.HolderName;
        //        ExpirationDate = _card.ExpirationDate;   
        //        return _card;
        //    }
        //}

        public PaymentsViewModel()
        {

        }

        public PaymentsViewModel(InvoiceModel model, CustomersModel Cust)
        {
            CashOrCredit = Controls.StaticMembers.PayCashOrCredit; //Show Credit Or Cash in View
            OnePayment = new PaymentsModel();
            OneInvoice = new InvoiceModel();
            StripeModel = new StripeAccountModel();

            OneInvoice = model;
            CustomerDetails = Cust;
        }

        [RelayCommand]
        async void CashPayNow(InvoiceModel model)
        {
            IsBusy = true;
            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
                    return;
                }
                else
                {
                    UserDialogs.Instance.ShowLoading();
                    OnePayment.Type = 1;
                    OnePayment.Method = 1; //Cash

                    await InitiolizModel(model);
                    UserDialogs.Instance.HideHud();
                }
            }
            catch (Exception ex)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
            }
            IsBusy = false;
        }

        [RelayCommand]
        async void CreditPayNow(InvoiceModel model)
        {
            IsBusy = true;

            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
                    return;
                }
                else
                {
                    UserDialogs.Instance.ShowLoading();
                    OneInvoice = model;

                    OnePayment.Type = 0;
                    OnePayment.Method = 3; //Debit Card

                    await PayViaStripe();
                    UserDialogs.Instance.HideHud();
                }
            }
            catch (Exception)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", "Please Complate All informations !!!", "OK");
            }

            IsBusy= false;
        }

        public async Task InitiolizModel(InvoiceModel model)
        {
            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
                    return;
                }
                else
                {
                    if(!string.IsNullOrEmpty(SignatureImageByte64))
                    {
                        if (OnePayment.Amount <= model.Net || OnePayment.Amount == null)
                        {
                            string UserToken = await _service.UserToken();

                            OnePayment.AccountId = model.AccountId;
                            OnePayment.BrancheId = model.BrancheId;
                            OnePayment.CustomerId = model.CustomerId;
                            //OnePayment.ContractId = model.ContractId;
                            OnePayment.InvoiceId = model.Id;
                            //OnePayment.ExpensesId = model.ExpensesId;
                            OnePayment.PaymentDate = DateTime.Now;
                            OnePayment.SignatureDraw = SignatureImageByte64;

                            OnePayment.Amount = OnePayment.Amount == null ? model.Net : OnePayment.Amount;
                            //OnePayment.OverAmount = model.OverAmount;

                            //OnePayment.IncreaseDecrease = model.IncreaseDecrease;
                            //OnePayment.TransactionID = model.TransactionID;
                            //OnePayment.CheckNumber = model.CheckNumber;
                            //OnePayment.BankName = model.BankName;
                            //OnePayment.AccountNumber = model.AccountNumber;
                            OnePayment.Notes = model.Notes;
                            OnePayment.Active = model.Active;
                            OnePayment.CreateUser = model.CreateUser;
                            OnePayment.CreateDate = model.CreateDate;

                            UserDialogs.Instance.ShowLoading();
                            var json = await ORep.PostDataAsync("api/Payments/InsertPayment", OnePayment, UserToken);
                            UserDialogs.Instance.HideHud();

                            if (json != null && json != "api not responding")
                            {
                                await App.Current.MainPage.DisplayAlert("FixPro", "Succes Payment for This Job.", "Ok");
                                await App.Current.MainPage.Navigation.PushAsync(new MainPage());
                            }
                            else
                            {
                                await App.Current.MainPage.DisplayAlert("FixPro", "Field Payment for This Job.", "Ok");
                            }
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("FixPro", "Please Enter Right Amount.", "Ok");
                        }
                    }   
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("FixPro", "Please Enter Signature.", "Ok");
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
            }
            
        }

        async Task GetSkretKey(int? BranchId)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                UserDialogs.Instance.ShowLoading();
                string UserToken = await _service.UserToken();

                var json = await ORep.GetAsync<StripeAccountModel>(string.Format("api/Payments/GetStripeAccount?" + "BranchId=" + BranchId), UserToken);

                if (json != null)
                {
                    StripeModel = json;
                }

                UserDialogs.Instance.HideHud();
            }         
        }

        public async Task PayViaStripe()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                await GetSkretKey(OneInvoice.BrancheId);

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
                        Cvc = cvv,
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
                    Name = CustomerDetails.FirstName + "" + CustomerDetails.LastName,
                    Email = CustomerDetails.Email,
                    Description = OneInvoice.ScheduleName,
                    Address = new AddressOptions { City = CustomerDetails.City, Country = CustomerDetails.Country, Line1 = CustomerDetails.Address, Line2 = "", PostalCode = CustomerDetails.PostalcodeZIP, State = CustomerDetails.State }
                };

                var customerService = new CustomerService();
                var cust = customerService.Create(customer);

                // step 5: charge option
                var chargeoption = new ChargeCreateOptions
                {
                    Amount = Convert.ToInt64(OneInvoice.Total * 100),
                    Currency = "USD",
                    ReceiptEmail = CustomerDetails.Email,
                    Customer = cust.Id,
                    Source = source.Id
                };

                // step 6: charge the customer
                var chargeService = new ChargeService();
                Charge charge = chargeService.Create(chargeoption);
                if (charge.Status == "succeeded")
                {
                    // success
                    await InitiolizModel(OneInvoice);
                }
                else
                {
                    // failed
                    await App.Current!.MainPage!.DisplayAlert("Alert", "failed Payment for This Job.", "Ok");
                }
            }
               
        }
    }
}
