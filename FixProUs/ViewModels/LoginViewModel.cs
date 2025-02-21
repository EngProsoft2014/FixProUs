
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows.Input;
using Akavache;
using System.Reactive.Linq;
using FixProUs.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Controls.UserDialogs.Maui;
using FixProUs.Services.Data;
using FixProUs.Pages;

namespace FixProUs.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {

        readonly Services.Data.ServicesService _service = new Services.Data.ServicesService();

        [ObservableProperty]
        EmployeeModel loginModel;

        Helpers.GenericRepository ORep = new Helpers.GenericRepository();

        public LoginViewModel()
        {

            if (Helpers.Settings.UserIdGet != "0" && Helpers.Settings.UserNameGet != "")
            {
                LoginModel = new EmployeeModel()
                {
                    Id = int.Parse(Helpers.Settings.UserIdGet),
                    UserName = Helpers.Settings.UserNameGet,
                    FirstName = Helpers.Settings.UserFristNameGet,
                    Password = Helpers.Settings.PasswordGet,
                    EmailUserName = Helpers.Settings.EmailGet,
                    Phone1 = Helpers.Settings.PhoneGet,
                    BrancheId = int.Parse(Helpers.Settings.BranchIdGet),
                    BranchName = Helpers.Settings.BranchNameGet,
                    CreateDate = Helpers.Settings.CreateDateGet == "" ? DateTime.Now : DateTime.Parse(Helpers.Settings.CreateDateGet),
                    OneSignalPlayerId = Helpers.Settings.PlayerIdGet,
                };
            }
            else
            {
                LoginModel = new EmployeeModel();
            }
        }

        [RelayCommand]
        async Task ClickLogin(EmployeeModel model)
        {
            IsBusy = true;

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                UserDialogs.Instance.ShowLoading();

                LoginModel = model;

                if (string.IsNullOrEmpty(LoginModel.UserName))
                {
                    await App.Current!.MainPage!.DisplayAlert("Alert", $"Please Complete This Field Required : User Name.", "Ok");
                }
                else if (string.IsNullOrEmpty(LoginModel.Password))
                {
                    await App.Current!.MainPage!.DisplayAlert("Alert", $"Please Complete This Field Required : Password.", "Ok");
                }
                else
                {

                    //var MLogin = await ORep.GetAsync<EmployeeModel>("api/Login/GetLogin?" + "UserName=" + model.UserName + "&" + "Password=" + model.Password);
                    var MLogin = await ORep.GetLoginAsync<EmployeeModel>("api/Login/GetLogin?" + "UserName=" + model.UserName + "&" + "Password=" + model.Password + "&" + "PlayerId=" + Helpers.Settings.PlayerIdGet);

                    if (MLogin.EmployeeStatus?.Contains("Try Again") == true)//UserName Or Password is Wrong
                    {
                        await App.Current!.MainPage!.DisplayAlert("Alert", "Wrong Username Or Password.", "Ok");
                    }
                    else if (MLogin.EmployeeStatus?.Contains("Account Is Expired") == true)
                    {
                        Preferences.Default.Set(Helpers.Settings.Email, MLogin.EmailUserName);
                        bool answer = await App.Current!.MainPage!.DisplayAlert("Question?", "Account expired, Do you want to buy a pricing plan?", "Yes", "No");
                        if (answer)
                        {
                            var ViewModel = new PlansViewModel();
                            var page = new Pages.PlansPages.ChoosePlanPage();
                            page.BindingContext = ViewModel;
                            await App.Current!.MainPage!.Navigation.PushAsync(page);
                        }
                    }
                    else
                    {

                        if (MLogin.ActiveAccount == true)
                        {
                            if (MLogin.ActiveMobileLogin == true)
                            {
                                
                                Preferences.Default.Set(Helpers.Settings.UserId, MLogin.Id.ToString()); 
                                Helpers.Settings.UserIdGet = !string.IsNullOrEmpty(MLogin.Id.ToString()) ? MLogin.Id.ToString() : "" ;

                                Preferences.Default.Set(Helpers.Settings.AccountId, MLogin.AccountId.ToString());
                                Helpers.Settings.AccountIdGet = !string.IsNullOrEmpty(MLogin.AccountId.ToString()) ? MLogin.AccountId.ToString() : "";

                                Preferences.Default.Set(Helpers.Settings.UserName, MLogin.UserName);
                                Helpers.Settings.UserNameGet = !string.IsNullOrEmpty(MLogin.UserName.ToString()) ? MLogin.UserName.ToString() : "";

                                Preferences.Default.Set(Helpers.Settings.Password, MLogin.Password);
                                Helpers.Settings.PasswordGet = !string.IsNullOrEmpty(MLogin.Password.ToString()) ? MLogin.Password.ToString() : "";

                                Preferences.Default.Set(Helpers.Settings.AccountName, MLogin.AccountName);
                                Helpers.Settings.AccountNameGet = !string.IsNullOrEmpty(MLogin.AccountName.ToString()) ? MLogin.AccountName.ToString() : "";

                                Preferences.Default.Set(Helpers.Settings.AccountDayExpired, DateTime.Now.ToString());
                                Helpers.Settings.AccountDayExpiredGet = DateTime.Now.ToString();

                                Preferences.Default.Set(Helpers.Settings.AccountNameWithSpace, MLogin.CompanyNameWithSpace.ToString());
                                Helpers.Settings.AccountNameWithSpaceGet = !string.IsNullOrEmpty(MLogin.CompanyNameWithSpace.ToString()) ? MLogin.CompanyNameWithSpace.ToString() : "";

                                Preferences.Default.Set(Helpers.Settings.UserFristName, MLogin.FirstName);
                                Helpers.Settings.UserFristNameGet = !string.IsNullOrEmpty(MLogin.FirstName.ToString()) ? MLogin.FirstName.ToString() : "";

                                Preferences.Default.Set(Helpers.Settings.UserLastName, MLogin.LastName);
                                Helpers.Settings.UserLastNameGet = !string.IsNullOrEmpty(MLogin.LastName.ToString()) ? MLogin.LastName.ToString() : "";

                                Preferences.Default.Set(Helpers.Settings.Email, MLogin.EmailUserName);
                                Helpers.Settings.EmailGet = !string.IsNullOrEmpty(MLogin.EmailUserName.ToString()) ? MLogin.EmailUserName.ToString() : "";

                                Preferences.Default.Set(Helpers.Settings.Phone, MLogin.Phone1);
                                Helpers.Settings.PhoneGet = !string.IsNullOrEmpty(MLogin.Phone1.ToString()) ? MLogin.Phone1.ToString() : "";

                                Preferences.Default.Set(Helpers.Settings.CreateDate, MLogin.CreateDate.ToString());
                                Helpers.Settings.CreateDateGet = !string.IsNullOrEmpty(MLogin.CreateDate.ToString()) ? MLogin.CreateDate.ToString() : "";

                                Preferences.Default.Set(Helpers.Settings.AccountId, MLogin.AccountId.ToString());
                                Helpers.Settings.AccountIdGet = !string.IsNullOrEmpty(MLogin.AccountId.ToString()) ? MLogin.AccountId.ToString() : "";

                                Preferences.Default.Set(Helpers.Settings.BranchId, MLogin.BrancheId.ToString());
                                Helpers.Settings.BranchIdGet = !string.IsNullOrEmpty(MLogin.BrancheId.ToString()) ? MLogin.BrancheId.ToString() : "";

                                Preferences.Default.Set(Helpers.Settings.BranchName, MLogin.BranchName);
                                Helpers.Settings.BranchNameGet = !string.IsNullOrEmpty(MLogin.BranchName.ToString()) ? MLogin.BranchName.ToString() : "";

                                Preferences.Default.Set(Helpers.Settings.UserRole, MLogin.UserRole.ToString());
                                Helpers.Settings.UserRoleGet = !string.IsNullOrEmpty(MLogin.UserRole.ToString()) ? MLogin.UserRole.ToString() : "";

                                Preferences.Default.Set(Helpers.Settings.UserEmployees, MLogin.Employees);
                                Helpers.Settings.UserEmployeesGet = !string.IsNullOrEmpty(MLogin.Employees) ? MLogin.Employees.ToString() : "";

                                Preferences.Default.Set(Helpers.Settings.TypeTrackingSch_Invo, MLogin.TypeTrackingSch_Invo.ToString());
                                Helpers.Settings.TypeTrackingSch_InvoGet = !string.IsNullOrEmpty(MLogin.TypeTrackingSch_Invo.ToString()) ? MLogin.TypeTrackingSch_Invo.ToString() : "";

                                Helpers.Utility.ServerUrl = !string.IsNullOrEmpty(MLogin.AccountSubdomainApiURL) ? MLogin.AccountSubdomainApiURL : Helpers.Utility.ServerUrl;

                                await BlobCache.LocalMachine.InsertObject(ServicesService.UserTokenServiceKey, MLogin.GernToken, DateTimeOffset.Now.AddHours(24));

                                Preferences.Default.Set(Helpers.Settings.UserPricture, (Helpers.Utility.PathServerProfileImages + Helpers.Settings.AccountNameGet + "/" + MLogin.Picture));
                                Helpers.Settings.UserPrictureGet = !string.IsNullOrEmpty(MLogin.Picture) ? (Helpers.Utility.PathServerProfileImages + Helpers.Settings.AccountNameGet + "/" + MLogin.Picture) : "";

                                await App.Current!.MainPage!.Navigation.PushAsync(new MainPage());
  
                            }
                            else
                            {
                                await App.Current!.MainPage!.DisplayAlert("Alert", "Sorry, This Username is not Mobile Access", "Ok");
                            }
                        }
                        else
                        {
                            await App.Current!.MainPage!.DisplayAlert("Alert", "Sorry, Your Account is not active", "Ok");
                        }
                    }
                }

                UserDialogs.Instance.HideHud();
            }

            IsBusy = false;
        }

    }
}
