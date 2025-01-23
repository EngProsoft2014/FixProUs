
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Controls.UserDialogs.Maui;
using FixProUs.Models;
using GoogleApi.Entities.Translate.Common.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace FixProUs.ViewModels
{
    public partial class CreateItemViewModel : BaseViewModel
    {

        readonly Services.Data.ServicesService _service = new Services.Data.ServicesService();

        [ObservableProperty]
        ItemsServicesModel itemDetails;

        [ObservableProperty]
        ItemsServicesCategoryModel oneItemsServicesCategory;

        [ObservableProperty]
        ItemsServicesSubCategoryModel oneItemsServicesSubCategory;

        [ObservableProperty]
        ItemsServicesTypes oneItemsServicesType;

        [ObservableProperty]
        ObservableCollection<ItemsServicesCategoryModel> lstItemsServicesCategories;

        [ObservableProperty]
        ObservableCollection<ItemsServicesSubCategoryModel> lstItemsServicesSubCategories;

        [ObservableProperty]
        ObservableCollection<ItemsServicesTypes> lstItemsServicesTypes;


        Helpers.GenericRepository ORep = new Helpers.GenericRepository();

        public delegate void AddItemDelegte(ItemsServicesModel item);
        public event AddItemDelegte AddItemClose;


        public CreateItemViewModel()
        {
            Init();
        }

        async void Init()
        {
            ItemDetails = new ItemsServicesModel();
            OneItemsServicesCategory = new ItemsServicesCategoryModel();
            OneItemsServicesSubCategory = new ItemsServicesSubCategoryModel();
            LstItemsServicesCategories = new ObservableCollection<ItemsServicesCategoryModel>();
            LstItemsServicesSubCategories = new ObservableCollection<ItemsServicesSubCategoryModel>();
            LstItemsServicesTypes = new ObservableCollection<ItemsServicesTypes>();
            OneItemsServicesType = new ItemsServicesTypes();


            LstItemsServicesTypes.Add(new ItemsServicesTypes() { Id = 1, Name = "Service" });
            LstItemsServicesTypes.Add(new ItemsServicesTypes() { Id = 2, Name = "Item" });
            LstItemsServicesTypes.Add(new ItemsServicesTypes() { Id = 3, Name = "Lebor" });
            LstItemsServicesTypes.Add(new ItemsServicesTypes() { Id = 4, Name = "Other" });

            await GetItemsServicesCategories();
            await GetItemsServicesSubCategories();
        }

        async Task GetItemsServicesCategories()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                UserDialogs.Instance.ShowLoading();
                string UserToken = await _service.UserToken();
                var json = await ORep.GetAsync<ObservableCollection<ItemsServicesCategoryModel>>(string.Format("api/Schedules/GetItemsServicesCategories?" + "AccountId=" + Helpers.Settings.AccountIdGet), UserToken);

                if (json != null)
                {
                    LstItemsServicesCategories = json;
                }

                UserDialogs.Instance.HideHud();
            }
        }

        async Task GetItemsServicesSubCategories()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                UserDialogs.Instance.ShowLoading();
                string UserToken = await _service.UserToken();
                var json = await ORep.GetAsync<ObservableCollection<ItemsServicesSubCategoryModel>>(string.Format("api/Schedules/GetItemsServicesSubCategories?" + "AccountId=" + Helpers.Settings.AccountIdGet), UserToken);

                if (json != null)
                {
                    LstItemsServicesSubCategories = json;
                }

                UserDialogs.Instance.HideHud();
            }
        }

        [RelayCommand]
        async void AddNewItemService(ItemsServicesModel item)
        {
            IsBusy = true;
            UserDialogs.Instance.ShowLoading();

            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await App.Current!.MainPage!.DisplayAlert("Error", "No Internet Avialable !!!", "OK");
                    return;
                }
                else
                {
                    if (string.IsNullOrEmpty(item.Name))
                    {
                        await App.Current!.MainPage!.DisplayAlert("Alert", $"Please Complete This Field Required : Name.", "Ok");
                    }
                    else if (OneItemsServicesType.Id == 0 || OneItemsServicesType == null)
                    {
                        await App.Current!.MainPage!.DisplayAlert("Alert", $"Please Complete This Field Required : Type.", "Ok");
                    }
                    else if (OneItemsServicesCategory.Id == 0 || OneItemsServicesCategory == null)
                    {
                        await App.Current!.MainPage!.DisplayAlert("Alert", $"Please Complete This Field Required : Category.", "Ok");
                    }
                    else if (OneItemsServicesSubCategory.Id == 0 || OneItemsServicesSubCategory == null)
                    {
                        await App.Current!.MainPage!.DisplayAlert("Alert", $"Please Complete This Field Required : Sub Category.", "Ok");
                    }
                    else if (item.QTYTime == 0 || item.QTYTime == null)
                    {
                        await App.Current!.MainPage!.DisplayAlert("Alert", $"Please Complete This Field Required : QTY.", "Ok");
                    }
                    else if (item.CostperUnit == 0 || item.CostperUnit == null)
                    {
                        await App.Current!.MainPage!.DisplayAlert("Alert", $"Please Complete This Field Required : Price.", "Ok");
                    }
                    else
                    {
                        if (ItemDetails != null)
                        {
                            string UserToken = await _service.UserToken();

                            ItemDetails.AccountId = int.Parse(Helpers.Settings.AccountIdGet);
                            ItemDetails.BrancheId = int.Parse(Helpers.Settings.BranchIdGet);
                            ItemDetails.CategoryId = OneItemsServicesCategory.Id;
                            ItemDetails.SubCategoryId = OneItemsServicesSubCategory.Id;
                            ItemDetails.Type = OneItemsServicesType.Id;
                            ItemDetails.InventoryItem = OneItemsServicesType.Id == 2 ? true : false;
                            ItemDetails.Active = true;
                            ItemDetails.CreateUser = int.Parse(Helpers.Settings.UserIdGet);
                            ItemDetails.CreateDate = DateTime.Now;

                            UserDialogs.Instance.ShowLoading();
                            var Json = await ORep.PostStrAsync("api/Schedules/PostAddItemService", ItemDetails, UserToken);
                            UserDialogs.Instance.HideHud();

                            if (Json != "")
                            {
                                await App.Current!.MainPage!.DisplayAlert("FixPro", "Succes for Add Item/Service.", "Ok");

                                var ReturnItem = JsonConvert.DeserializeObject<ItemsServicesModel>(Json);
                                AddItemClose.Invoke(ReturnItem);
                            }
                            else
                            {
                                await App.Current!.MainPage!.DisplayAlert("FixPro", "Failed for Add Item/Service.", "Ok");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
            }

            UserDialogs.Instance.HideHud();
            IsBusy = false;
        }
    }
}
