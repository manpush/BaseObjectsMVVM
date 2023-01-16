using System;
using System.Data;
using System.Linq.Expressions;
using System.Windows;
using projectControl;

namespace BaseObjectsMVVM
{
    public abstract class  EntityViewModel<T, TSql> : BaseViewModel 
        where TSql:ModelSql<T>, new() 
        where T : EntityModel, new()
    {
        public EntityViewModel()
        {
            _item = new T();
        }
        public EntityViewModel(int? itemId, SaveStatuses status)
        {
            if (itemId != null)
            {
                _item = new T();
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
                _item = new T();
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
        public T Item {
            get => _item;
            set
            {
                _item = value;
                OnPropertyChanged(()=>Item);
            }
        }
        private T _item;

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
        public virtual void MarkAsUnchanged()
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