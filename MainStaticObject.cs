using System.Data.SqlClient;
using System.Windows.Controls;
using System.Data.SQLite;

namespace BaseObjectsMVVM
{
    public static class MainStaticObject
    {
        public static Frame MainFrame { get; set; }
        public static string SqlConnectionString { get; set; }
        public static SQLiteConnection SqlConnection { get; set; }
        public static SqlManager SqlManager { get; set; }
    }
}