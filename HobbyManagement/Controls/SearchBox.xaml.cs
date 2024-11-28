using HobbyManagement.Commands;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HobbyManagement.Controls
{
    /// <summary>
    /// Interaction logic for SearchBox.xaml
    /// </summary>
    public partial class SearchBox : UserControl
    {
        public static readonly DependencyProperty SearchTextProperty = 
            DependencyProperty.Register(nameof(SearchText), typeof(string), typeof(SearchBox), new PropertyMetadata(""));

        public SearchBox()
        {
            InitializeComponent();
        }

        public ICommand ClearSearchCommand => new RelayCommand(ClearSearch, () => true);

        public string SearchText
        {
            get
            {
                return (string)GetValue(SearchTextProperty);
            }

            set
            {
                SetValue(SearchTextProperty, value);
            }
        }

        private void ClearSearch()
        {
            SearchText = "";
        }
    }
}
