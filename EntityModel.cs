using System;
using System.Windows;

namespace BaseObjectsMVVM
{
    /// <summary>
    /// Класс описывающий любую сущность 
    /// </summary>
    public abstract class EntityModel
    {
        public virtual int? ItemId { get; set; }
    }
}