using System;
using System.Data;
using System.Linq.Expressions;
using System.Windows;
using projectControl;

namespace BaseObjectsMVVM
{
    /// <summary>
    /// Класс описывающий ViewModel сущности
    /// </summary>
    /// <typeparam name="EM">Model</typeparam>
    /// <typeparam name="TSql">ModelSql</typeparam>
    public abstract class EntityViewModel<EM, TSql> : BaseViewModel
        where TSql : ModelSql<EM>, new()
        where EM : EntityModel, new()
    {
        #region Constructors

        /// <summary>
        /// Конструктор без аргументов (иначе ошибка CS0310)
        /// </summary>
        public EntityViewModel()
        {
        }

        /// <summary>
        /// конструктор создания VM "из данных"
        /// </summary>
        public EntityViewModel(DataRow row)
        {
            Item = new EM();
            ParseArguments(row);
        }

        /// <summary>
        /// конструктор для создания vm по id и с присвоением статуса
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="status"></param>
        public EntityViewModel(int? itemId, SaveStatuses status)
        {
            if (itemId != null)
            {
                _item = new EM();
                var TSql = new TSql();
                var adapter = TSql.LoadItem((int)itemId);
                DataTable data = new DataTable();
                adapter.Fill(data);

                if (data.Rows.Count > 0)
                {
                    ParseArguments(data.Rows[0]);
                }
            }
            else
            {
                _item = new EM();
            }

            switch (status)
            {
                case SaveStatuses.Unchanged:
                    MarkAsUnchanged();
                    break;
                case SaveStatuses.New:
                    MarkAsNew();
                    break;
            }
        }

        #endregion

        #region Atributes

        /// <summary>
        /// Модель сущности
        /// </summary>
        public EM Item
        {
            get => _item ?? (_item = new EM());
            set
            {
                _item = value;
                OnPropertyChanged(() => Item);
            }
        }

        private EM _item;

        /// <summary>
        ///  id сущности
        /// </summary>
        public virtual int? ItemId
        {
            get => Item.ItemId;
            set => Item.ItemId = value;
        }

        #endregion

        #region Commands

        #region SaveItem

        private RelayCommand _saveItemCommand;

        /// <summary>
        /// Команда сохранения объекта
        /// </summary>
        public RelayCommand SaveItemCommand =>
            _saveItemCommand ?? (_saveItemCommand = new RelayCommand(obj => SaveItem(), _ => CanSave));

        /// <summary>
        /// сохранениу объекта
        /// </summary>
        /// <returns>
        /// TODO: Результат сохранения
        /// </returns>
        public virtual int? SaveItem()
        {
            var a = new TSql();
            switch (Status)
            {
                case SaveStatuses.Changed:
                    a.Update(Item);
                    MarkAsUnchanged();
                    return null;
                case SaveStatuses.New:
                    MarkAsUnchanged();
                    return a.Create(Item);
            }

            return null;
        }

        #endregion

        #region DeleteItem

        private RelayCommand _deleteItemCommand;

        /// <summary>
        /// Команда удаления объекта
        /// </summary>
        public RelayCommand DeleteItemCommand =>
            _deleteItemCommand ?? (_deleteItemCommand = new RelayCommand(obj => DeleteItem()));

        /// <summary>
        /// Удаление объекта
        /// </summary>
        public virtual void DeleteItem()
        {
            var a = new TSql();
            a.Delete(Item);
        }

        #endregion

        #region UpdateItem

        private RelayCommand _updateItemCommand;

        /// <summary>
        /// Команда записи изменений объекта
        /// </summary>
        public RelayCommand UpdateItemCommand =>
            _updateItemCommand ?? (_updateItemCommand = new RelayCommand(obj => UpdateItem()));

        /// <summary>
        /// Запись изменений объекта
        /// </summary>
        public virtual void UpdateItem()
        {
            var a = new TSql();
            a.LoadItem((int)Item.ItemId);
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Обозначить как изменённый
        /// </summary>
        public override void MarkAsChanged()
        {
            base.MarkAsChanged();
            OnPropertyChanged(() => SaveItemCommand);
        }

        /// <summary>
        /// Обозначить как не изменённый
        /// </summary>
        public override void MarkAsUnchanged()
        {
            base.MarkAsUnchanged();
            OnPropertyChanged(() => SaveItemCommand);
        }

        /// <summary>
        /// подгрузить информацию об объекте
        /// </summary>
        /// <param name="itemId"></param>
        public virtual void LoadItem(int itemId)
        {
            MessageBox.Show("Загрузка объекта - нет реализации.");
        }

        /// <summary>
        /// метод парсинга данных по полям модели, полученных из базы.
        /// </summary>
        /// <param name="row">строка из базы</param>
        public abstract void ParseArguments(DataRow row);

        /// <summary>
        /// Метод для изменения полей объекта.
        /// По сути делает следующее: "При изменениии поля, помечать объект как изменённый"
        /// </summary>
        /// <param name="propertyExpr"></param>
        /// <param name="val"></param>
        /// <param name="prop"></param>
        /// <typeparam name="TPropertyType"></typeparam>
        public virtual void SetPropertyValue<TPropertyType>(Expression<Func<TPropertyType>> propertyExpr,
            TPropertyType val, ref TPropertyType prop)
        {
            if (!Equals(val, prop))
            {
                MarkAsChanged();
                prop = val;
            }

            base.OnPropertyChanged(propertyExpr);
        }

        #endregion
    }
}