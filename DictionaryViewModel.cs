using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using projectControl;

namespace BaseObjectsMVVM
{
    public abstract class DictionaryViewModel<EVM, EM, MSql> : BaseViewModel
        where EM : EntityModel, new()
        where EVM : EntityViewModel<EM, MSql>, new()
        where MSql : ModelSql<EM>, new()

    {
        public DictionaryViewModel(WorkspaceViewModel parent)
        {
            Parent = parent;
        }

        public DictionaryViewModel()
        {
        }

        public WorkspaceViewModel Parent { get; set; }
        private ObservableCollection<EVM> _items;

        public ObservableCollection<EVM> Items
        {
            get => _items ?? (_items = new ObservableCollection<EVM>());
            set
            {
                _items = value;
                OnPropertyChanged(() => Items);
            }
        }

        private EVM _selectedItem;

        public EVM SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(() => SelectedItem);
            }
        }
        
        /// <summary>
        /// Выделяет элемент в интерфейсе.
        /// Сбрасывая при этом все остальные выделения.
        /// </summary>
        /// <param name="itemVm">Запись, которую необходимо выделить</param>
        protected void SelectItem(EVM itemVm)
        {
            if (itemVm == null)
                return;
            ICollectionView view = CollectionViewSource.GetDefaultView(Items);
            view?.MoveCurrentTo(itemVm);
        }

        public int? SelectedItemIndex
        {
            get => Items.IndexOf(SelectedItem);
            set
            {
                if (Items.Count > value && value != -1)
                    SelectedItem = Items[(int) value];
                OnPropertyChanged(() => SelectedItem);
                OnPropertyChanged(() => SelectedItemIndex);
            }
        }

        public virtual void EditItem(EVM item)
        {
            EditItem();
        }

        public virtual void EditItem()
        {
            MessageBox.Show("Нет реализации редактирования элемента");
        }

        public virtual void LoadItems()
        {
            MessageBox.Show("Нет реализации загрузки элементов");
        }

        private RelayCommand _openItemCommand;

        public RelayCommand OpenItemCommand =>
            _openItemCommand ?? (_openItemCommand = new RelayCommand(obj => OpenItem(SelectedItem)));

        public int ItemIdForGroup1 { get; set; }
        public int ItemIdForGroup2 { get; set; }
        

        private RelayCommand _deleteSelectedItemCommand;

        public RelayCommand DeleteSelectedItemCommand =>
            _deleteSelectedItemCommand ?? (_deleteSelectedItemCommand =
                new RelayCommand(obj => DeleteItem(SelectedItem), o => SelectedItem != null));

        private RelayCommand _updateCommand;
        public RelayCommand UpdateCommand => _updateCommand ?? (_updateCommand = new RelayCommand(obj => LoadItems()));
        
        public override bool CanSave
        {
            get { return Items.FirstOrDefault(i => i.CanSave) != null; }
        }

        private RelayCommand _saveCommand;

        public RelayCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new RelayCommand(obj => SaveItems(), a => CanSave));

        public virtual void SaveItems()
        {
            Items.Where(i => i.CanSave).ToList().ForEach(a => a.SaveItem());
            LoadItems();
        }

        public virtual void OpenItem(EVM item)
        {
            SelectItem(item);
        }

        public virtual void DeleteItem(EVM selectedItem)
        {
            selectedItem.DeleteItemCommand.Execute(null);
            LoadItems();
        }


        public virtual void CreateItem(EVM item)
        {
            Items.Add(item);
            OpenItem(Items.Last());
        }

        private RelayCommand _createItemCommand;

        public RelayCommand CreateItemCommand => _createItemCommand ?? (_createItemCommand =
            new RelayCommand(obj => CreateItem(
                    new EVM()
                    {
                        ItemId = Items.Max(p => p.ItemId) + 1,
                        Status = SaveStatuses.New
                    }
                )
            ));
        private RelayCommand _groupItems;
        public RelayCommand GroupItemsCommand => _groupItems ?? 
                                                 (_groupItems = new RelayCommand(obj => GroupItems()));


        public virtual void GroupItems()
        {
            try
            {
                var mSql = new MSql();
                mSql.GroupItems(ItemIdForGroup1, ItemIdForGroup2);
            }
            catch (Exception e)
            {
                MessageBox.Show("err " + e.Message);
            }
        }
    }
}