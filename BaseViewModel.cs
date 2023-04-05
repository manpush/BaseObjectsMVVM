using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace BaseObjectsMVVM
{
    /// <summary>
    /// Класс описывающий механизмы ViewModel
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Механизм маркировки ViewModel как изменённый.
        /// Пример использования: в карточке объекта при изменении какого-либо атрибута
        /// </summary>
        public virtual void MarkAsChanged()
        {
            if (Status == SaveStatuses.Unchanged)
            {
                Status = SaveStatuses.Changed;
                OnPropertyChanged(() => Status);
                OnPropertyChanged(() => CanSave);
            }
        }
        /// <summary>
        /// Механизм маркировки ViewModel как новый.
        /// Пример использования: в карточке объекта при создании объекта
        /// </summary>
        public virtual void MarkAsNew()
        {
            Status = SaveStatuses.New;
            OnPropertyChanged(() => Status);
            OnPropertyChanged(()=> CanSave);
        }
        /// <summary>
        /// поле отображающее возможность сохранить объект
        /// </summary>
        public virtual bool CanSave {
            get =>  Status == SaveStatuses.Changed ||
                    Status == SaveStatuses.New;
        }
        /// <summary>
        /// Механизм маркировки ViewModel как неизменённый.
        /// Пример использования: в карточке объекта при открытии существующего объекта
        /// </summary>
        public virtual void MarkAsUnchanged()
        {
                Status = SaveStatuses.Unchanged;
                OnPropertyChanged(() => Status);

        }
        /// <summary>
        /// Поле статуса модели. По умолч - SaveStatuses.Unchanged
        /// </summary>
        public virtual SaveStatuses Status { get; set; } = SaveStatuses.Unchanged;

        /// <summary>
        /// Механизм "пинания" View при изенении ViewModel
        /// </summary>
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