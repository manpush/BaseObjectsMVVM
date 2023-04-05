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
    /// <summary>
    /// класс описывающий работу со списком моделей
    /// </summary>
    /// <typeparam name="EVM">ViewModel</typeparam>
    /// <typeparam name="EM">Model</typeparam>
    /// <typeparam name="MSql">ModelSql</typeparam>
    public abstract class DictionaryViewModel<EVM, EM, MSql> : BaseViewModel
        where EM : EntityModel, new()
        where EVM : EntityViewModel<EM, MSql>, new()
        where MSql : ModelSql<EM>, new()

    {
        /// <summary>
        /// конструктор с указанием родителя
        /// </summary>
        /// <param name="parent">ViewModel страницы на которой будет отображаться список</param>
        public DictionaryViewModel(WorkspaceViewModel parent)
        {
            Parent = parent;
        }

        #region Properties

        #region Parent

        /// <summary>
        /// ViewModel страницы на которой будет отображаться список
        /// </summary>
        public WorkspaceViewModel Parent { get; set; }

        #endregion
        #region Items

        private ObservableCollection<EVM> _items;
        /// <summary>
        /// Коллекция для биндинка к DataGrid.
        /// Коллекция ViewModel-ей для отображения
        /// </summary>
        public ObservableCollection<EVM> Items
        {
            get => _items ?? (_items = new ObservableCollection<EVM>());
            set
            {
                _items = value;
                OnPropertyChanged(() => Items);
            }
        }

        #endregion

        #region SelectedItem

        private EVM _selectedItem;
        /// <summary>
        /// Возвращает выделенный элемент коллекции
        /// </summary>
        public EVM SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(() => SelectedItem);
            }
        }
        
        #endregion

        #region SelectedItemIndeex
        /// <summary>
        /// Возвращает индекс выделенного элемента коллекции
        /// </summary>
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

        #endregion

        #region CanSave
        /// <summary>
        /// Атрибут отражающий возможность выполнения команды сохранить
        /// true в случае когда хоть у одного элемента в списке CanSave==true
        /// </summary>
        public override bool CanSave
        {
            get { return Items.FirstOrDefault(i => i.CanSave) != null; }
        }

        #endregion
        
        #endregion

        #region Methods
        
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
        /// <summary>
        /// редактирование элемента. Обычно перегружается поумолчанию
        /// </summary>
        /// <param name="item"></param>
        public virtual void EditItem(EVM item)
        {
            EditItem();
        }

        public virtual void EditItem()
        {
            MessageBox.Show("Нет реализации редактирования элемента");
        }

        #endregion

        #region Commands

        #region OpenItem
        
        private RelayCommand _openItemCommand;
        /// <summary>
        /// команда открытия карточки выбранного элемента
        /// TODO: добавить проверку на множественный выбор, в этом случае ничего не делать
        /// </summary>
        public RelayCommand OpenItemCommand =>
            _openItemCommand ?? (_openItemCommand = new RelayCommand(obj => OpenItem(SelectedItem)));
        /// <summary>
        /// открытие карточки выбранного элемента
        /// </summary>
        /// <param name="item">VM элемента</param>
        public virtual void OpenItem(EVM item)
        {
            SelectItem(item); //чтобы при закрытии карточки выбор находмлся на изменённом элементе 
        }
        #endregion

        #region DeleteSelectedItem

        private RelayCommand _deleteSelectedItemCommand;

        public RelayCommand DeleteSelectedItemCommand =>
            _deleteSelectedItemCommand ?? (_deleteSelectedItemCommand =
                new RelayCommand(obj => DeleteItem(SelectedItem), o => SelectedItem != null));
        public virtual void DeleteItem(EVM selectedItem)
        {
            selectedItem.DeleteItemCommand.Execute(null);
            LoadItems();
        }
        #endregion

        #region Update

        private RelayCommand _updateCommand;
        /// <summary>
        /// команда загрузки списка из хранилища
        /// TODO: сделать сохранение выбранного элемента после обновления списка
        /// </summary>
        public RelayCommand UpdateCommand => _updateCommand ?? (_updateCommand = new RelayCommand(obj => LoadItems()));
        /// <summary>
        /// Подгрузка элементов в список
        /// </summary>
        public virtual void LoadItems()
        {
            MessageBox.Show("Нет реализации загрузки элементов");
        }
        
        #endregion


        #region Save
        
        private RelayCommand _saveCommand;
        /// <summary>
        /// команда сохранения всех изменённых элементов списка
        /// </summary>
        public RelayCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new RelayCommand(obj => SaveItems(), a => CanSave));
        /// <summary>
        /// сохранение всех изменённых элементов списка
        /// </summary>
        public virtual void SaveItems()
        {
            Items.Where(i => i.CanSave).ToList().ForEach(a => a.SaveItem());
            LoadItems();
        }
        
        #endregion
        
        #region GroupItems
        
        private RelayCommand _groupItemsCommand;
        /// <summary>
        /// Команда объединения записей (спицифичный механизм, возможно ему не место здесь :))
        /// </summary>
        public RelayCommand GroupItemsCommand => _groupItemsCommand ?? 
                                                 (_groupItemsCommand = new RelayCommand(obj => GroupItems()));

        public int ItemIdForGroup1 { get; set; }
        public int ItemIdForGroup2 { get; set; }
        /// <summary>
        /// Метод объединяющий ItemIdForGroup1 и ItemIdForGroup2
        /// </summary>
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
        
        #endregion
        
        #region CreateItem

        private RelayCommand _createItemCommand;
        /// <summary>
        /// Команда создания объекта списка
        /// </summary>
        public RelayCommand CreateItemCommand => _createItemCommand ?? (_createItemCommand =
            new RelayCommand(obj => CreateItem(
                    new EVM()
                    {
                        ItemId = Items.Max(p => p.ItemId) + 1,
                        Status = SaveStatuses.New
                    }
                )
            ));
        /// <summary>
        /// Создание объекта списка
        /// </summary>
        /// <param name="item"></param>
        public virtual void CreateItem(EVM item)
        {
            Items.Add(item);
            OpenItem(Items.Last());
        }

        #endregion
        
        #endregion
    }
}