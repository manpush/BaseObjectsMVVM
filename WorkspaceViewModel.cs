using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using projectControl;

namespace BaseObjectsMVVM
{
    public abstract class WorkspaceViewModel : BaseViewModel
    {
        public Frame MainFrame;
        public WorkspaceViewModel Parent;
       
        
        public WorkspaceViewModel(Frame mainFrame, WorkspaceViewModel parent)
        {
            Parent = parent;
            MainFrame = mainFrame;
            ChildPageDisposeEvent += OnChildPageDisposeEvent;
        }
        
            
        private RelayCommand _goBackCommand;
        public RelayCommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new RelayCommand( obj => GoBack()));
        public virtual void GoBack()
        {
            ChildPageDisposeEvent?.Invoke();
            MainFrame.NavigationService.GoBack();
        }
        
        private RelayCommand _updateCommand;
        public RelayCommand UpdateCommand => _updateCommand ?? (_updateCommand = new RelayCommand( obj => UpdateViewModel()));

        public virtual void UpdateViewModel()
        {
            MessageBox.Show("Обновление информации - нет реализации");
        }
        
        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand => _saveCommand ?? (_saveCommand = new RelayCommand( obj => SaveViewModel()));
        public virtual void SaveViewModel()
        {
            MessageBox.Show("Сохранение информации - нет реализации");
        }
        
        

        public virtual void Navigate(Page newWVM)
        {
            MainFrame.Navigate(newWVM);
            
        }

        public virtual void OnChildPageDisposeEvent()
        {
            Parent.UpdateCommand.Execute(null);
        }
        public delegate void ChildPageDisposeEventDelegate();
        public event ChildPageDisposeEventDelegate ChildPageDisposeEvent ;
    }
}