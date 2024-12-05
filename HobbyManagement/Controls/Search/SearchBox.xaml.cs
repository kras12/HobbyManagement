using HobbyManagement.Commands;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HobbyManagement.Controls.Search
{
    /// <summary>
    /// Interaction logic for SearchBox.xaml
    /// </summary>
    public partial class SearchBox : UserControl
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchBox()
        {
            InitializeComponent();
        }

        #endregion

        #region DependencyProperties

        /// <summary>
        /// Dependency property controlled by property <see cref="SearchText"/>.
        /// </summary>
        public static readonly DependencyProperty SearchTextProperty = 
            DependencyProperty.Register(nameof(SearchText), typeof(string), typeof(SearchBox), new PropertyMetadata(""));

        #endregion

        #region Properties

        /// <summary>
        /// A command to clear the search input field. 
        /// </summary>
        public ICommand ClearSearchCommand => new RelayCommand(ClearSearch);

        /// <summary>
        /// The search text. 
        /// </summary>
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

        #endregion

        #region Methods

        /// <summary>
        /// Clears the search input field. 
        /// </summary>
        private void ClearSearch()
        {
            SearchText = "";
        }

        #endregion
    }
}
