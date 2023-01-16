using System.Windows;
using System.Windows.Input;
using projectControl;

namespace BaseObjectsMVVM.WpfControls
{
    public class DataGrid:System.Windows.Controls.DataGrid
    {
        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            DoubleClickCommand?.Execute(null);
        }

        public RelayCommand DoubleClickCommand {
            get { return (RelayCommand)(GetValue(DoubleClickCommandProperty)); }
            set { SetValue(DoubleClickCommandProperty, value); }
        }
        public static DependencyProperty DoubleClickCommandProperty = DependencyProperty.Register("DoubleClickCommand", typeof(RelayCommand), typeof(DataGrid));

    }
}