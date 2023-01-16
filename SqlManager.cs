using System.Linq;
using System.Data.SQLite;
using System.Windows;
using System;

namespace BaseObjectsMVVM
{
    public class SqlManager
    {
        public string ConnectionString;
        public SQLiteConnection Connection;

        public void Connect()
        {
            try
            {
                Connection = new SQLiteConnection(ConnectionString);
                Connection.Open();
                Connection.Close();
            } catch (Exception e)
            {
                MessageBox.Show("err: "+e.Message);
            }
        }

        public SQLiteDataReader Query(string sqlExpression)
        {
            SQLiteCommand command = new SQLiteCommand(sqlExpression, MainStaticObject.SqlManager.Connection);
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows) // если есть данные
                {
                    return reader;
                }
                return null;
            }
        }
    }
}