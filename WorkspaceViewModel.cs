using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using projectControl;

namespace BaseObjectsMVVM
{
    /// <summary>
    /// Класс описывающий ViewModel рабочей страницы
    /// </summary>
    public abstract class WorkspaceViewModel : BaseViewModel
    {
        /// <summary>
        /// Frame в котором эта страница отображается
        /// </summary>
        public Frame MainFrame;

        /// <summary>
        /// страница которая вызвала данную страницу
        /// </summary>
        public WorkspaceViewModel Parent;


        public WorkspaceViewModel(Frame mainFrame, WorkspaceViewModel parent)
        {
            Parent = parent;
            MainFrame = mainFrame;
            ChildPageDisposeEvent += OnChildPageDisposeEvent;
        }

        #region Commands

        #region GoBack

        private RelayCommand _goBackCommand;

        /// <summary>
        /// Команда возврата к предыдущей странице
        /// </summary>
        public RelayCommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new RelayCommand(obj => GoBack()));

        /// <summary>
        /// Возврат к предыдущей странице
        /// </summary>
        public virtual void GoBack()
        {
            ChildPageDisposeEvent?.Invoke();
            MainFrame.NavigationService.GoBack();
        }

        #endregion

        #region Update

        private RelayCommand _updateCommand;

        /// <summary>
        /// Команда обновления содержимого окна
        /// </summary>
        public RelayCommand UpdateCommand =>
            _updateCommand ?? (_updateCommand = new RelayCommand(obj => UpdateViewModel()));

        /// <summary>
        /// обновление содержимого окна
        /// </summary>
        public virtual void UpdateViewModel()
        {
            MessageBox.Show("Обновление содержимого окна - нет реализации");
        }

        #endregion

        #region Save

        private RelayCommand _saveCommand;

        /// <summary>
        /// Команда сохранения содержимого окна
        /// </summary>
        public RelayCommand SaveCommand => _saveCommand ?? (_saveCommand = new RelayCommand(obj => SaveViewModel()));

        /// <summary>
        /// Сохранение содержимого окна
        /// </summary>
        public virtual void SaveViewModel()
        {
            MessageBox.Show("Сохранение содержимого окна - нет реализации");
        }

        #endregion

        #endregion

        /// <summary>
        /// метод перехода к следующему окну
        /// </summary>
        /// <param name="newWVM"></param>
        public virtual void Navigate(Page newWVM)
        {
            MainFrame.Navigate(newWVM);
        }

        #region OnChildPageDispose

        /// <summary>
        /// Действия при разрушении дочернего окна
        /// </summary>
        public virtual void OnChildPageDisposeEvent()
        {
            Parent.UpdateCommand.Execute(null);
        }

        public delegate void ChildPageDisposeEventDelegate();

        public event ChildPageDisposeEventDelegate ChildPageDisposeEvent;

        #endregion
    }
}