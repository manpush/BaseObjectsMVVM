using System.Data.SqlClient;
using System.Data.SQLite;
using System.Windows;

namespace BaseObjectsMVVM
{
    /// <summary>
    /// Класс описывающий механизмы взаимодействия модели с местом хранения данных
    /// TODO: переписать для возможности реализации любого места хранения (не только SQLite)
    /// </summary>
    /// <typeparam name="EM"></typeparam>
    public abstract class ModelSql<EM> where EM : BaseObjectsMVVM.EntityModel
    {
        public virtual void Update(EM item)
        {
            MessageBox.Show("Сохранение объекта - нет реализации.");
        }

        public virtual int? Create(EM item)
        {
            MessageBox.Show("Создание объекта - нет реализации.");
            return null;
        }

        public virtual void GroupItems(int id1, int id2)
        {
            MessageBox.Show("Объединение объектов - нет реализации.");
        }

        public virtual void Delete(EM item)
        {
            MessageBox.Show("Улаление объекта - нет реализации.");
        }

        public virtual SQLiteDataAdapter LoadItems()
        {
            MessageBox.Show("Загрузка объекта - нет реализации.");
            return null;
        }

        public virtual SQLiteDataAdapter LoadItem(int itemId)
        {
            MessageBox.Show("Загрузка объекта - нет реализации.");
            return null;
        }
    }
}