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
    public abstract class  EntityViewModel<EM, TSql> : BaseViewModel 
        where TSql:ModelSql<EM>, new() 
        where EM : EntityModel, new()
    {
        #region Constructors
        /// <summary>
        /// Конструктор без аргументов (иначе ошибка CS0310)
        /// </summary>
        public EntityViewModel(){}
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
        
        public EM Item {
            get => _item;
            set
            {
                _item = value;
                OnPropertyChanged(()=>Item);
            }
        }
        private EM _item;

        private RelayCommand _saveItemCommand;
        public RelayCommand SaveItemCommand => _saveItemCommand ?? (_saveItemCommand = new RelayCommand( obj => SaveItem(), _=>CanSave));

        private RelayCommand _deleteItemCommand;
        public RelayCommand DeleteItemCommand => _deleteItemCommand ?? (_deleteItemCommand = new RelayCommand( obj => DeleteItem()));
        
        private RelayCommand _updateItemCommand;
        public RelayCommand UpdateItemCommand => _updateItemCommand ?? (_updateItemCommand = new RelayCommand( obj => UpdateItem()));

        
        public override void MarkAsChanged()
        {
            base.MarkAsChanged();
            OnPropertyChanged(()=>SaveItemCommand);
        }
        public override void MarkAsUnchanged()
        {
            base.MarkAsUnchanged();
            OnPropertyChanged(()=>SaveItemCommand);
        }

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

        public virtual void DeleteItem()
        {
            var a = new TSql();
            a.Delete(Item);
        }
        
        public virtual void UpdateItem()
        {
            var a = new TSql();
            a.LoadItem((int) Item.ItemId);
        }
        

        public virtual void LoadItem(int itemId)
        {
            MessageBox.Show("Загрузка объекта - нет реализации.");
        }

        public abstract void ParseArguments(DataRow row);
        public virtual void SetPropertyValue<TPropertyType>(Expression<Func<TPropertyType>> propertyExpr, TPropertyType val, ref TPropertyType prop)
        {
            if (!Equals(val, prop))
            {
                MarkAsChanged();
                prop = val;
            }

            base.OnPropertyChanged(propertyExpr);
        }

        public virtual int? ItemId
        {
            get => Item.ItemId;
            set => Item.ItemId = value;
        }
    }
}