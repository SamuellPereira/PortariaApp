using System;
using System.Windows;
using System.Windows.Threading;
using PortariaApp.ViewModels;

namespace PortariaApp
{
    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel;
        private DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            DataContext = _viewModel;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (s, e) => Desenvolvido.Text = $"Desenvolvido por Samuel | {DateTime.Now:HH:mm:ss}";
            _timer.Start();
        }

        // ← ADICIONE ESTA PROPRIEDADE!
        public MainViewModel ViewModel => _viewModel;

        protected override void OnClosed(EventArgs e)
        {
            _timer?.Stop();
            base.OnClosed(e);
        }
    }
}
