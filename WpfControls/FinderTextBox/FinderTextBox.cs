using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace BaseObjectsMVVM.WpfControls
{
    public partial class FinderTextBox : UserControl, INotifyPropertyChanged
    
    {
        public static readonly DependencyProperty ItemsSourceProperty;
        public static readonly DependencyProperty ItemsSourceFiltredProperty;
        public static readonly DependencyProperty SelectedItemProperty;
        public static readonly DependencyProperty IsOpenProperty;
        public static readonly DependencyProperty TextProperty;

        public FinderTextBox()
        {
            InitializeComponent();
            DataContext = this;
        }
        static FinderTextBox()
        {
            ItemsSourceProperty = DependencyProperty.Register(
                "ItemsSourcee",
                typeof(IEnumerable),
                typeof(FinderTextBox),
                new FrameworkPropertyMetadata(
                    new ObservableCollection<ICanUseFinderTextBox>(),
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.AffectsRender));
            ItemsSourceFiltredProperty = DependencyProperty.Register(
                "ItemsSourceFiltred",
                typeof(IEnumerable),
                typeof(FinderTextBox),
                new FrameworkPropertyMetadata(
                    new ObservableCollection<ICanUseFinderTextBox>(),
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.AffectsRender));
            SelectedItemProperty = DependencyProperty.Register(
                "SelectedItemm",
                typeof(ICanUseFinderTextBox),
                typeof(FinderTextBox),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.AffectsRender));
            IsOpenProperty = DependencyProperty.Register(
                "IsOpen",
                typeof(bool),
                typeof(FinderTextBox),
                new FrameworkPropertyMetadata(
                    false,
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.AffectsRender));
            TextProperty = DependencyProperty.Register(
                "Text",
                typeof(string),
                typeof(FinderTextBox),
                new FrameworkPropertyMetadata(
                    String.Empty,
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.AffectsRender));
        }
        
        public string Text
        {
            get => (string) GetValue(TextProperty);
            set
            {
                SetValue(TextProperty, value);
                if (String.IsNullOrEmpty(Text))
                {
                    IsOpen = false;
                }
                else
                {
                    IsOpen = true;
                    Regex regex = new Regex((Text).ToLower());
                    var a2 = ItemsSourcee.Cast<ICanUseFinderTextBox>()
                        .Where(qwe =>
                        {
                            MatchCollection matches = regex.Matches(qwe.DescrForFind.ToLower());
                            return matches.Count > 0;
                        });
                    var a1 = new ObservableCollection<ICanUseFinderTextBox>(a2);
                    ItemsSourceFiltred =  new ObservableCollection<ICanUseFinderTextBox>(a1);
                }
                OnPropertyChanged(()=>IsOpen);
            }
        }
        
        public bool IsOpen
        {
            get => (bool) GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }

        public IEnumerable ItemsSourceFiltred
        {
            get => (IEnumerable) GetValue(ItemsSourceFiltredProperty);
            set => SetValue(ItemsSourceFiltredProperty, value);
        }
        public IEnumerable ItemsSourcee
        {
            get => (IEnumerable) GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }
        
        
        public ICanUseFinderTextBox SelectedItemm
        {
            get => (ICanUseFinderTextBox) GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

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