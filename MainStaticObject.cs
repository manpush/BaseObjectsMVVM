using System.Data.SqlClient;
using System.Windows.Controls;
using System.Data.SQLite;

namespace BaseObjectsMVVM
{
    public static class MainStaticObject
    {
        /// <summary>
        /// Главный фрейм приложения
        /// </summary>
        public static Frame MainFrame { get; set; }

        /// <summary>
        /// Строка подключения.
        /// Возможно этому здесь не место ))
        /// </summary>
        public static string SqlConnectionString { get; set; }

        /// <summary>
        /// Подключение к базе. Возможно этому здесь не место ))
        /// </summary>
        public static SQLiteConnection SqlConnection { get; set; }

        /// <summary>
        /// менеджер бд. Возможно этому здесь не место ))
        /// </summary>
        public static SqlManager SqlManager { get; set; }
    }
}