using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace BaseObjectsMVVM
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public virtual void MarkAsChanged()
        {
            if (Status == SaveStatuses.Unchanged)
            {
                Status = SaveStatuses.Changed;
                OnPropertyChanged(() => Status);
                OnPropertyChanged(() => CanSave);
            }
        }
        
        public virtual void MarkAsNew()
        {
            Status = SaveStatuses.New;
            OnPropertyChanged(() => Status);
            OnPropertyChanged(()=> CanSave);
        }

        public virtual bool CanSave {
            get =>  Status == SaveStatuses.Changed ||
                    Status == SaveStatuses.New;
        }

        public virtual void MarkAsUnchanged()
        {
                Status = SaveStatuses.Unchanged;
                OnPropertyChanged(() => Status);

        }
        public virtual SaveStatuses Status { get; set; } = SaveStatuses.Unchanged;

        public virtual void OnPropertyChanged<TPropertyType>(Expression<Func<TPropertyType>> propertyExpr)
        {
            string propertyName = GetPropertySymbol(propertyExpr);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public static string GetPropertySymbol<TResult>(Expression<Func<TResult>> expr)
        {
            if (expr.Body is UnaryExpression)
                return ((MemberExpression)((UnaryExpression)expr.Body).Operand).Member.Name;
            return ((MemberExpression)expr.Body).Member.Name;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        
        
    }
}