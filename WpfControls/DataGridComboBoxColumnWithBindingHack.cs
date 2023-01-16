using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace BaseObjectsMVVM.WpfControls
{
    public class DataGridComboBoxColumnWithBindingHack : DataGridComboBoxColumn
    {
        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            FrameworkElement element = base.GenerateEditingElement(cell, dataItem);
            //       cell.LostFocus += cell_LostFocus;
            CopyItemsSource(element);
            return element;
        }

        //void cell_LostFocus(object sender, RoutedEventArgs e)
        //{
        //}

        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            FrameworkElement element = base.GenerateElement(cell, dataItem);
            //  element.LostFocus += cell_LostFocus;
            //     ((System.Windows.Controls.ComboBox)element).SelectionChanged += DataGridComboBoxColumnWithBindingHack_SelectionChanged;
            CopyItemsSource(element);
            return element;
        }

        //void DataGridComboBoxColumnWithBindingHack_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{

        //}

        private void CopyItemsSource(FrameworkElement element)
        {
            BindingOperations.SetBinding(element, ItemsControl.ItemsSourceProperty, BindingOperations.GetBinding(this, ItemsControl.ItemsSourceProperty));
    
        }

    }
}