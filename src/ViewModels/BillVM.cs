﻿using LiteDB;
using MahApps.Metro.Controls.Dialogs;
using Phony.Kernel;
using Phony.Models;
using Phony.Views;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Phony.ViewModels
{

    public class BillVM : BindableBase
    {
        long _searchSelectedValue;
        decimal _itemChildItemPrice;
        decimal _itemChildItemQTYExist;
        decimal _itemChildItemQTYSell;
        decimal _serviceChildServiceBalance;
        decimal _serviceChildServiceCost;
        decimal _childDiscount;
        decimal _billTotal;
        decimal _billTotalAfterEachDiscount;
        decimal _billDiscount;
        decimal _billTotalAfterDiscount;
        decimal _billClientPayment;
        decimal _billClientPaymentChange;
        long _currentBillNo;
        string _searchText;
        string _itemChildItemName;
        string _serviceChildServiceName;
        string _serviceChildNotes;
        string _itemChildNotes;
        bool _byItem;
        bool _byCard;
        bool _byService;
        bool _byName;
        bool _byShopCode;
        bool _byBarCode;
        bool _isAddItemChildOpen;
        bool _isAddServiceChildOpen;
        bool _isAddBillNote;
        bool _isSearchDropDownOpen;
        Client _selectedClient;
        Item _selectedItem;
        Service _selectedService;
        BillItemMove _dataGridSelectedBillItemMove;
        BillServiceMove _dataGridSelectedBillServiceMove;

        Visibility _billClientPaymentChangeVisible;

        List<Client> _clients;
        List<Item> _items;
        List<Service> _services;
        List<User> _users;

        ObservableCollection<object> _searchItems;
        ObservableCollection<BillItemMove> _billItemsMoves;
        ObservableCollection<BillServiceMove> _billServicesMoves;

        public long SearchSelectedValue
        {
            get => _searchSelectedValue;
            set
            {
                if (value != _searchSelectedValue)
                {
                    _searchSelectedValue = value;
                    RaisePropertyChanged();
                }
            }
        }

        public decimal ItemChildItemPrice
        {
            get => _itemChildItemPrice;
            set
            {
                if (value != _itemChildItemPrice)
                {
                    _itemChildItemPrice = value;
                    RaisePropertyChanged();
                }
            }
        }

        public decimal ItemChildItemQTYSell
        {
            get => _itemChildItemQTYSell;
            set
            {
                if (value != _itemChildItemQTYSell)
                {
                    _itemChildItemQTYSell = value;
                    RaisePropertyChanged();
                }
            }
        }

        public decimal ItemChildItemQTYExist
        {
            get => _itemChildItemQTYExist;
            set
            {
                if (value != _itemChildItemQTYExist)
                {
                    _itemChildItemQTYExist = value;
                    RaisePropertyChanged();
                }
            }
        }

        public decimal ServiceChildServiceBalance
        {
            get => _serviceChildServiceBalance;
            set
            {
                if (value != _serviceChildServiceBalance)
                {
                    _serviceChildServiceBalance = value;
                    RaisePropertyChanged();
                }
            }
        }

        public decimal ServiceChildServiceCost
        {
            get => _serviceChildServiceCost;
            set
            {
                if (value != _serviceChildServiceCost)
                {
                    _serviceChildServiceCost = value;
                    RaisePropertyChanged();
                }
            }
        }

        public decimal ChildDiscount
        {
            get => _childDiscount;
            set
            {
                if (value != _childDiscount)
                {
                    _childDiscount = value;
                    RaisePropertyChanged();
                }
            }
        }

        public decimal BillTotal
        {
            get => _billTotal;
            set
            {
                if (value != _billTotal)
                {
                    _billTotal = Math.Round(value, 2);
                    RaisePropertyChanged();
                }
            }
        }

        public decimal BillTotalAfterEachDiscount
        {
            get => _billTotalAfterEachDiscount;
            set
            {
                if (value != _billTotalAfterEachDiscount)
                {
                    _billTotalAfterEachDiscount = Math.Round(value, 2);
                    RaisePropertyChanged();
                }
            }
        }

        public decimal BillDiscount
        {
            get => _billDiscount;
            set
            {
                if (value != _billDiscount)
                {
                    _billDiscount = value;
                    if (_billDiscount > 0)
                    {
                        BillTotalAfterDiscount = Math.Round(BillTotalAfterEachDiscount - (BillTotalAfterEachDiscount * (_billDiscount / 100)), 2);
                    }
                    else
                    {
                        BillTotalAfterDiscount = Math.Round(BillTotalAfterEachDiscount, 2);
                    }
                    RaisePropertyChanged();
                }
            }
        }

        public decimal BillTotalAfterDiscount
        {
            get => _billTotalAfterDiscount;
            set
            {
                if (value != _billTotalAfterDiscount)
                {
                    _billTotalAfterDiscount = Math.Round(value, 2);
                    RaisePropertyChanged();
                }
            }
        }

        public decimal BillClientPayment
        {
            get => _billClientPayment;
            set
            {
                if (value != _billClientPayment)
                {
                    _billClientPayment = value;
                    if (_billClientPayment > BillTotalAfterDiscount)
                    {
                        BillClientPaymentChange = Math.Round(_billClientPayment - BillTotalAfterDiscount, 2);
                        BillClientPaymentChangeVisible = Visibility.Visible;
                    }
                    else
                    {
                        BillClientPaymentChange = 0;
                        BillClientPaymentChangeVisible = Visibility.Collapsed;
                    }
                    RaisePropertyChanged();
                }
            }
        }

        public decimal BillClientPaymentChange
        {
            get => _billClientPaymentChange;
            set
            {
                if (value != _billClientPaymentChange)
                {
                    _billClientPaymentChange = Math.Round(value, 2);
                    RaisePropertyChanged();
                }
            }
        }

        public long CurrentBillNo
        {
            get => _currentBillNo;
            set
            {
                if (value != _currentBillNo)
                {
                    _currentBillNo = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (value != _searchText)
                {
                    _searchText = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string ItemChildItemName
        {
            get => _itemChildItemName;
            set
            {
                if (value != _itemChildItemName)
                {
                    _itemChildItemName = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string ServiceChildServiceName
        {
            get => _serviceChildServiceName;
            set
            {
                if (value != _serviceChildServiceName)
                {
                    _serviceChildServiceName = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string ServiceChildNotes
        {
            get => _serviceChildNotes;
            set
            {
                if (value != _serviceChildNotes)
                {
                    _serviceChildNotes = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string ItemChildNotes
        {
            get => _itemChildNotes;
            set
            {
                if (value != _itemChildNotes)
                {
                    _itemChildNotes = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool ByItem
        {
            get => _byItem;
            set
            {
                if (value != _byItem)
                {
                    _byItem = value;
                    SearchItems = new ObservableCollection<object>();
                    SearchSelectedValue = 0;
                    RaisePropertyChanged();
                }
            }
        }

        public bool ByCard
        {
            get => _byCard;
            set
            {
                if (value != _byCard)
                {
                    _byCard = value;
                    SearchItems = new ObservableCollection<object>();
                    SearchSelectedValue = 0;
                    RaisePropertyChanged();
                }
            }
        }

        public bool ByService
        {
            get => _byService;
            set
            {
                if (value != _byService)
                {
                    _byService = value;
                    SearchItems = new ObservableCollection<object>();
                    SearchSelectedValue = 0;
                    RaisePropertyChanged();
                }
            }
        }

        public bool ByName
        {
            get => _byName;
            set
            {
                if (value != _byName)
                {
                    _byName = value;
                    SearchItems = new ObservableCollection<object>();
                    SearchSelectedValue = 0;
                    RaisePropertyChanged();
                }
            }
        }

        public bool ByShopCode
        {
            get => _byShopCode;
            set
            {
                if (value != _byShopCode)
                {
                    _byShopCode = value;
                    SearchItems = new ObservableCollection<object>();
                    SearchSelectedValue = 0;
                    RaisePropertyChanged();
                }
            }
        }

        public bool ByBarCode
        {
            get => _byBarCode;
            set
            {
                if (value != _byBarCode)
                {
                    _byBarCode = value;
                    SearchItems = new ObservableCollection<object>();
                    SearchSelectedValue = 0;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsAddItemChildOpen
        {
            get => _isAddItemChildOpen;
            set
            {
                if (value != _isAddItemChildOpen)
                {
                    _isAddItemChildOpen = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsAddServiceChildOpen
        {
            get => _isAddServiceChildOpen;
            set
            {
                if (value != _isAddServiceChildOpen)
                {
                    _isAddServiceChildOpen = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsAddBillNote
        {
            get => _isAddBillNote;
            set
            {
                if (value != _isAddBillNote)
                {
                    _isAddBillNote = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsSearchDropDownOpen
        {
            get => _isSearchDropDownOpen;
            set
            {
                if (value != _isSearchDropDownOpen)
                {
                    _isSearchDropDownOpen = value;
                    RaisePropertyChanged();
                }
            }
        }

        public Client SelectedClient
        {
            get => _selectedClient;
            set
            {
                if (value != _selectedClient)
                {
                    _selectedClient = value;
                    RaisePropertyChanged();
                }
            }
        }

        public Item SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (value != _selectedItem)
                {
                    _selectedItem = value;
                }
            }
        }

        public Service SelectedService
        {
            get => _selectedService;
            set
            {
                if (value != _selectedService)
                {
                    _selectedService = value;
                    RaisePropertyChanged();
                }
            }
        }

        public BillItemMove DataGridSelectedBillItemMove
        {
            get => _dataGridSelectedBillItemMove;
            set
            {
                if (value != _dataGridSelectedBillItemMove)
                {
                    _dataGridSelectedBillItemMove = value;
                    RaisePropertyChanged();
                }
            }
        }

        public BillServiceMove DataGridSelectedBillServiceMove
        {
            get => _dataGridSelectedBillServiceMove;
            set
            {
                if (value != _dataGridSelectedBillServiceMove)
                {
                    _dataGridSelectedBillServiceMove = value;
                    RaisePropertyChanged();
                }
            }
        }

        public Visibility BillClientPaymentChangeVisible
        {
            get => _billClientPaymentChangeVisible;
            set
            {
                if (value != _billClientPaymentChangeVisible)
                {
                    _billClientPaymentChangeVisible = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<object> SearchItems
        {
            get => _searchItems;
            set
            {
                if (value != _searchItems)
                {
                    _searchItems = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<BillItemMove> BillItemsMoves
        {
            get => _billItemsMoves;
            set
            {
                if (value != _billItemsMoves)
                {
                    _billItemsMoves = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<BillServiceMove> BillServicesMoves
        {
            get => _billServicesMoves;
            set
            {
                if (value != _billServicesMoves)
                {
                    _billServicesMoves = value;
                    RaisePropertyChanged();
                }
            }
        }

        public List<Client> Clients
        {
            get => _clients;
            set
            {
                if (value != _clients)
                {
                    _clients = value;
                    RaisePropertyChanged();
                }
            }
        }

        public List<Item> Items
        {
            get => _items;
            set
            {
                if (value != _items)
                {
                    _items = value;
                    RaisePropertyChanged();
                }
            }
        }

        public List<Service> Services
        {
            get => _services;
            set
            {
                if (value != _services)
                {
                    _services = value;
                    RaisePropertyChanged();
                }
            }
        }

        public List<User> Users
        {
            get => _users;
            set
            {
                if (value != _users)
                {
                    _users = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ICommand Search { get; set; }
        public ICommand AddBillMove { get; set; }
        public ICommand AddItemToBill { get; set; }
        public ICommand AddServiceToBill { get; set; }
        public ICommand DeleteBillMove { get; set; }
        public ICommand RedoBill { get; set; }
        public ICommand SaveBill { get; set; }
        public ICommand SaveAndShow { get; set; }

        Users.LoginVM CurrentUser = new Users.LoginVM();

        Bills Message = Application.Current.Windows.OfType<Bills>().FirstOrDefault();

        public BillVM()
        {
            LoadCommands();
            ByName = true;
            ByItem = true;
            BillClientPaymentChangeVisible = Visibility.Collapsed;
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
            {
                Clients = new List<Client>(db.GetCollection<Client>(DBCollections.Clients.ToString()).FindAll());
                Items = new List<Item>(db.GetCollection<Item>(DBCollections.Items.ToString()).FindAll());
                Services = new List<Service>(db.GetCollection<Service>(DBCollections.Services.ToString()).FindAll());
                Users = new List<User>(db.GetCollection<User>(DBCollections.Users.ToString()).FindAll());
            }
            BillItemsMoves = new ObservableCollection<BillItemMove>();
            BillServicesMoves = new ObservableCollection<BillServiceMove>();
            NewBillNo();
        }

        public void LoadCommands()
        {
            Search = new DelegateCommand(DoSearch, CanSearch);
            AddBillMove = new DelegateCommand(DoAddBillMove, CanAddBillMove);
            AddItemToBill = new DelegateCommand(DoAddItemToBill, CanAddItemToBill);
            AddServiceToBill = new DelegateCommand(DoAddServiceToBill, CanAddServiceToBill);
            DeleteBillMove = new DelegateCommand(DoDeleteBillMove, CanDeleteBillMove);
            RedoBill = new DelegateCommand(DoRedoBill, CanRedoBill);
            SaveBill = new DelegateCommand(DoSaveBill, CanSaveBill);
            SaveAndShow = new DelegateCommand(DoSaveAndShow, CanSaveAndShow);
        }

        async Task<long> SaveBillNoAsync()
        {
            if (BillClientPayment < BillTotalAfterDiscount)
            {
                if (SelectedClient.Id == 1)
                {
                    await Message.ShowMessageAsync("خطأ", "لا يمكن عمل فاتورة اجل لهذا العميل اختار عميل اخر او اضف عميل جديد");
                    return 0;
                }
                var result = await Message.ShowMessageAsync("اجل", $"هل انت متاكد من تسجيل الفاتورة كاجل؟", MessageDialogStyle.AffirmativeAndNegative);
                if (result != MessageDialogResult.Affirmative)
                {
                    return 0;
                }
            }
            string billNote = null;
            if (IsAddBillNote)
            {
                billNote = await Message.ShowInputAsync("ملاحظة", $"اكتب اى شئ ليتم طباعته مع الفاتورة");
            }
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
            {
                try
                {
                    var bi = new Bill
                    {
                        Client = db.GetCollection<Client>(DBCollections.Clients.ToString()).FindById(SelectedClient.Id),
                        Store = db.GetCollection<Store>(DBCollections.Stores.ToString()).FindById(1),
                        Discount = BillDiscount,
                        TotalAfterDiscounts = BillTotalAfterDiscount,
                        TotalPayed = BillClientPayment,
                        Notes = billNote,
                        CreateDate = DateTime.Now,
                        Creator = db.GetCollection<User>(DBCollections.Users.ToString()).FindById(CurrentUser.Id),
                        Editor = null,
                        EditDate = null
                    };
                    db.GetCollection<Bill>(DBCollections.Bills.ToString()).Insert(bi);
                    foreach (var item in BillItemsMoves)
                    {
                        item.Bill = bi;
                        db.GetCollection<BillItemMove>(DBCollections.BillsItemsMoves.ToString()).Insert(item);
                        var i = db.GetCollection<Item>(DBCollections.Items.ToString()).FindById(item.Item.Id);
                        i.QTY -= item.QTY;
                        db.GetCollection<Item>(DBCollections.Items.ToString()).Update(i);
                    }
                    foreach (var service in BillServicesMoves)
                    {
                        service.Bill = bi;
                        db.GetCollection<BillServiceMove>(DBCollections.BillsServicesMoves.ToString()).Insert(service);
                        var s = db.GetCollection<Service>(DBCollections.Services.ToString()).FindById(service.Service.Id);
                        s.Balance -= service.ServicePayment;
                        db.GetCollection<Service>(DBCollections.Services.ToString()).Update(s);
                    }
                    if (BillClientPayment < BillTotalAfterDiscount)
                    {
                        var c = db.GetCollection<Client>(DBCollections.Clients.ToString()).FindById(SelectedClient.Id);
                        c.Balance += BillTotalAfterDiscount - BillClientPayment;
                        db.GetCollection<Client>(DBCollections.Clients.ToString()).Update(c);
                    }
                    db.GetCollection<TreasuryMove>(DBCollections.TreasuriesMoves.ToString()).Insert(new TreasuryMove
                    {
                        Treasury = db.GetCollection<Treasury>(DBCollections.Treasuries.ToString()).FindById(1),
                        Debit = BillClientPayment,
                        Credit = BillClientPaymentChange,
                        Notes = $"فاتورة رقم {bi.Id}",
                        CreateDate = DateTime.Now,
                        Creator = db.GetCollection<User>(DBCollections.Users.ToString()).FindById(CurrentUser.Id),
                        EditDate = null,
                        Editor = null
                    });
                    Clear();
                    CurrentBillNo = bi.Id + 1;
                    return bi.Id;
                }
                catch (Exception ex)
                {
                    await Core.SaveExceptionAsync(ex);
                    return -1;
                }
            }
        }

        private bool CanSaveAndShow()
        {
            return CanSaveBill();
        }

        private async void DoSaveAndShow()
        {
            var i = await SaveBillNoAsync();
            if (i > 0)
            {
                await Message.ShowMessageAsync("تم الحفظ", $"تم حفظ الفاتورة بالرقم {i} بنجاح و سيتم عرضها للطباعه الان");
                new SalesBillsViewer(i).Show();
            }
            else if (i < 0)
            {
                await Message.ShowMessageAsync("خطا", "حدث خطا اثناء حفظ الفاتورة");
            }
        }

        private bool CanSaveBill()
        {
            if (SelectedClient == null)
            {
                return false;
            }
            if ((BillItemsMoves.Count > 0 || BillServicesMoves.Count > 0) && SelectedClient.Id > 0)
            {
                return true;
            }
            return false;
        }

        private async void DoSaveBill()
        {
            var i = await SaveBillNoAsync();
            if (i > 0)
            {
                await Message.ShowMessageAsync("تم الحفظ", $"تم حفظ الفاتورة بالرقم {i}");
            }
            else if (i < 0)
            {
                await Message.ShowMessageAsync("خطا", "حدث خطا اثناء حفظ الفاتورة");
            }
        }

        private bool CanRedoBill()
        {
            return true;
        }

        private void DoRedoBill()
        {
            Clear();
        }

        private bool CanDeleteBillMove()
        {
            if ((DataGridSelectedBillItemMove == null && ByItem) || (DataGridSelectedBillServiceMove == null && ByService))
            {
                return false;
            }
            return true;
        }

        private void DoDeleteBillMove()
        {
            if (ByItem)
            {
                var ItemToQtyPrice = DataGridSelectedBillItemMove.ItemPrice * DataGridSelectedBillItemMove.QTY;
                BillTotal -= ItemToQtyPrice;
                BillTotalAfterEachDiscount -= ItemToQtyPrice - (ItemToQtyPrice * (DataGridSelectedBillItemMove.Discount / 100));
                BillTotalAfterDiscount = BillTotalAfterEachDiscount - (BillTotalAfterEachDiscount * (BillDiscount / 100));
                BillItemsMoves.Remove(DataGridSelectedBillItemMove);
            }
            if (ByService)
            {
                BillTotal -= DataGridSelectedBillServiceMove.ServicePayment;
                BillTotalAfterEachDiscount -= DataGridSelectedBillServiceMove.ServicePayment - (DataGridSelectedBillServiceMove.ServicePayment * (DataGridSelectedBillServiceMove.Discount / 100));
                BillTotalAfterDiscount = BillTotalAfterEachDiscount - (BillTotalAfterEachDiscount * (BillDiscount / 100));
                BillServicesMoves.Remove(DataGridSelectedBillServiceMove);
            }
            DataGridSelectedBillItemMove = null;
            DataGridSelectedBillServiceMove = null;
        }

        void Clear()
        {
            SelectedClient = null;
            BillItemsMoves = new ObservableCollection<BillItemMove>();
            BillServicesMoves = new ObservableCollection<BillServiceMove>();
            BillTotal = 0;
            BillTotalAfterEachDiscount = 0;
            BillDiscount = 0;
            BillTotalAfterDiscount = 0;
            BillClientPayment = 0;
            SearchSelectedValue = 0;
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
            {
                Clients = new List<Client>(db.GetCollection<Client>(DBCollections.Clients.ToString()).FindAll());
                Items = new List<Item>(db.GetCollection<Item>(DBCollections.Items.ToString()).FindAll());
                Services = new List<Service>(db.GetCollection<Service>(DBCollections.Services.ToString()).FindAll());
                SearchSelectedValue = 0;
            }
        }

        void ClearChild()
        {
            if (IsAddItemChildOpen)
            {
                IsAddItemChildOpen = false;
                ItemChildItemName = null;
                ItemChildItemPrice = 0;
                ItemChildItemQTYExist = 0;
                ItemChildItemQTYSell = 0;
                SelectedItem = null;
                ItemChildNotes = null;
            }
            if (IsAddServiceChildOpen)
            {
                IsAddServiceChildOpen = false;
                ServiceChildServiceName = null;
                ServiceChildServiceBalance = 0;
                ServiceChildServiceCost = 0;
                SelectedService = null;
                ServiceChildNotes = null;
            }
            ChildDiscount = 0;
        }

        private bool CanAddServiceToBill()
        {
            if (ServiceChildServiceCost > 0)
            {
                return true;
            }
            return false;
        }

        private void DoAddServiceToBill()
        {
            decimal balanceNeeded = 0;
            foreach (var item in BillServicesMoves)
            {
                if (item.Service.Id == SearchSelectedValue)
                {
                    balanceNeeded += item.ServicePayment;
                }
            }
            balanceNeeded += ServiceChildServiceCost;
            if (SelectedService.Balance >= balanceNeeded)
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
                {
                    BillServicesMoves.Add(new BillServiceMove
                    {
                        Bill = db.GetCollection<Bill>(DBCollections.Bills.ToString()).FindById(CurrentBillNo),
                        Service = db.GetCollection<Service>(DBCollections.Services.ToString()).FindById(SearchSelectedValue),
                        Balance = ServiceChildServiceBalance,
                        ServicePayment = ServiceChildServiceCost,
                        Discount = ChildDiscount,
                        Notes = ServiceChildNotes,
                        Creator = db.GetCollection<User>(DBCollections.Users.ToString()).FindById(CurrentUser.Id),
                        CreateDate = DateTime.Now,
                        Editor = null,
                        EditDate = null
                    });
                }
                BillTotal += ServiceChildServiceCost;
                BillTotalAfterEachDiscount += ServiceChildServiceCost - (ServiceChildServiceCost * (ChildDiscount / 100));
                if (BillDiscount > 0)
                {
                    BillTotalAfterDiscount = BillTotalAfterEachDiscount - (BillTotalAfterEachDiscount * (BillDiscount / 100));
                }
                else
                {
                    BillTotalAfterDiscount = BillTotalAfterEachDiscount;
                }
                ClearChild();
            }
            else
            {
                Message.ShowMessageAsync("رصيد غير كافى", "الرصيد فى الخدمه لا يكفى لتسجيل العمليه");
            }
        }

        private bool CanAddItemToBill()
        {
            if (ItemChildItemQTYSell > 0)
            {
                return true;
            }
            return false;
        }

        private void DoAddItemToBill()
        {
            decimal QTYNeeded = 0;
            foreach (var item in BillItemsMoves)
            {
                if (item.Item.Id == SearchSelectedValue)
                {
                    QTYNeeded += item.QTY;
                }
            }
            QTYNeeded += ItemChildItemQTYSell;
            if (SelectedItem.QTY >= QTYNeeded)
            {
                var ItemToQtyPrice = SelectedItem.RetailPrice * ItemChildItemQTYSell;
                using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
                {
                    BillItemsMoves.Add(new BillItemMove
                    {
                        Bill = db.GetCollection<Bill>(DBCollections.Bills.ToString()).FindById(CurrentBillNo),
                        Item = db.GetCollection<Item>(DBCollections.Items.ToString()).FindById(SearchSelectedValue),
                        QTY = ItemChildItemQTYSell,
                        ItemPrice = SelectedItem.RetailPrice,
                        Discount = ChildDiscount,
                        Notes = ItemChildNotes,
                        Creator = db.GetCollection<User>(DBCollections.Users.ToString()).FindById(CurrentUser.Id),
                        CreateDate = DateTime.Now,
                        Editor = null,
                        EditDate = null
                    });
                }
                BillTotal += ItemToQtyPrice;
                BillTotalAfterEachDiscount += ItemToQtyPrice - (ItemToQtyPrice * (ChildDiscount / 100));
                if (BillDiscount > 0)
                {
                    BillTotalAfterDiscount = BillTotalAfterEachDiscount - (BillTotalAfterEachDiscount * (BillDiscount / 100));
                }
                else
                {
                    BillTotalAfterDiscount = BillTotalAfterEachDiscount;
                }
                ClearChild();
            }
            else
            {
                Message.ShowMessageAsync("الكمية لا تكفى", "الكمية الخاصه بالصنف اقل من المراد بيعه");
            }
        }

        private bool CanAddBillMove()
        {
            if (SearchSelectedValue > 0)
            {
                return true;
            }
            return false;
        }

        private void DoAddBillMove()
        {
            if (ByService)
            {
                SelectedService = Services.FirstOrDefault(f => f.Id == SearchSelectedValue);
                IsAddServiceChildOpen = true;
                ServiceChildServiceName = SelectedService.Name;
            }
            else
            {
                SelectedItem = Items.FirstOrDefault(f => f.Id == SearchSelectedValue);
                IsAddItemChildOpen = true;
                ItemChildItemName = SelectedItem.Name;
                ItemChildItemPrice = SelectedItem.RetailPrice;
                ItemChildItemQTYExist = SelectedItem.QTY;
            }
        }

        private bool CanSearch()
        {
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                return true;
            }
            return false;
        }

        private void DoSearch()
        {
            try
            {
                if (ByItem)
                {
                    if (ByName)
                    {
                        SearchItems = new ObservableCollection<object>(Items.Where(i => i.Group == ItemGroup.Other && i.Name.Contains(SearchText)));
                        if (SearchItems.Count > 0)
                        {
                            IsSearchDropDownOpen = true;
                        }
                        else
                        {
                            SearchSelectedValue = 0;
                            BespokeFusion.MaterialMessageBox.ShowError("لم يستطع ايجاد صنف بهذا الاسم");
                        }
                    }
                    else if (ByBarCode)
                    {
                        SearchItems = new ObservableCollection<object>(Items.Where(i => i.Group == ItemGroup.Other && i.Barcode == SearchText));
                        if (SearchItems.Count > 0)
                        {
                            SearchSelectedValue = Items.SingleOrDefault(i => i.Group == ItemGroup.Other && i.Barcode == SearchText).Id;
                            IsSearchDropDownOpen = true;
                        }
                        else
                        {
                            SearchSelectedValue = 0;
                            BespokeFusion.MaterialMessageBox.ShowError("لم يستطع ايجاد صنف بهذا الباركود");
                        }
                    }
                    else
                    {
                        SearchItems = new ObservableCollection<object>(Items.Where(i => i.Group == ItemGroup.Other && i.Shopcode == SearchText));
                        if (SearchItems.Count > 0)
                        {
                            SearchSelectedValue = Items.SingleOrDefault(i => i.Group == ItemGroup.Other && i.Shopcode == SearchText).Id;
                            IsSearchDropDownOpen = true;
                        }
                        else
                        {
                            SearchSelectedValue = 0;
                            BespokeFusion.MaterialMessageBox.ShowError("لم يستطع ايجاد صنف بكود المحل هذا");
                        }
                    }
                }
                else if (ByCard)
                {
                    if (ByName)
                    {
                        SearchItems = new ObservableCollection<object>(Items.Where(i => i.Group == ItemGroup.Card && i.Name.Contains(SearchText)));
                        if (SearchItems.Count > 0)
                        {
                            IsSearchDropDownOpen = true;
                        }
                        else
                        {
                            SearchSelectedValue = 0;
                            BespokeFusion.MaterialMessageBox.ShowError("لم يستطع ايجاد كارت شحن بهذا الاسم");
                        }
                    }
                    else if (ByBarCode)
                    {
                        SearchItems = new ObservableCollection<object>(Items.Where(i => i.Group == ItemGroup.Card && i.Barcode == SearchText));
                        if (SearchItems.Count > 0)
                        {
                            SearchSelectedValue = Items.SingleOrDefault(i => i.Group == ItemGroup.Card && i.Barcode == SearchText).Id;
                            IsSearchDropDownOpen = true;
                        }
                        else
                        {
                            SearchSelectedValue = 0;
                            BespokeFusion.MaterialMessageBox.ShowError("لم يستطع ايجاد كارت شحن بهذا الباركود");
                        }
                    }
                    else
                    {
                        SearchItems = new ObservableCollection<object>(Items.Where(i => i.Group == ItemGroup.Card && i.Shopcode == SearchText));
                        if (SearchItems.Count > 0)
                        {
                            SearchSelectedValue = Items.SingleOrDefault(i => i.Group == ItemGroup.Card && i.Shopcode == SearchText).Id;
                            IsSearchDropDownOpen = true;
                        }
                        else
                        {
                            SearchSelectedValue = 0;
                            BespokeFusion.MaterialMessageBox.ShowError("لم يستطع ايجاد كارت شحن بكود المحل هذا");
                        }
                    }
                }
                else
                {
                    SearchItems = new ObservableCollection<object>(Services.Where(s => s.Name.Contains(SearchText)));
                    if (SearchItems.Count > 0)
                    {
                        IsSearchDropDownOpen = true;
                    }
                    else
                    {
                        SearchSelectedValue = 0;
                        BespokeFusion.MaterialMessageBox.ShowError("لم يستطع ايجاد خدمه بهذا الاسم");
                    }
                }
            }
            catch (Exception ex)
            {
                SearchSelectedValue = 0;
                Core.SaveException(ex);
            }
        }

        void NewBillNo()
        {
            try
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
                {
                    CurrentBillNo = ++db.GetCollection<Client>(DBCollections.Clients.ToString()).FindAll().LastOrDefault().Id;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                CurrentBillNo = 1;
            }
        }
    }
}