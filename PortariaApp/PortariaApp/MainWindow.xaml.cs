using System.Windows;
using PortariaApp.ViewModels;

namespace PortariaApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}
