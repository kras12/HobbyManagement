using HobbyManagement.Viewmodels;
using System.Windows;

namespace HobbyManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="hobbyManagerViewModel">Injected hobby manager view model.</param>
        public MainWindow(IHobbyManagerViewModel hobbyManagerViewModel)
        {
            InitializeComponent();
            DataContext = hobbyManagerViewModel;
        }
    }
}