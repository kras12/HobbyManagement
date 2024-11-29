using HobbyManagement.Viewmodels;
using System.Windows;

namespace HobbyManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IHobbyManagerViewModel _hobbyManagerViewModel;

        public MainWindow(IHobbyManagerViewModel hobbyManagerViewModel)
        {
            InitializeComponent();
            _hobbyManagerViewModel = hobbyManagerViewModel;
            DataContext = hobbyManagerViewModel;
        }
    }
}