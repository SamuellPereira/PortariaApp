using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PortariaApp.Models;
using PortariaApp.ViewModels;

namespace PortariaApp.Views
{
    public partial class SaidaWindow : Window
    {
        private MainViewModel _mainViewModel;

        public SaidaWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainViewModel = mainWindow.ViewModel;
            Loaded += SaidaWindow_Loaded;
        }

        private void SaidaWindow_Loaded(object sender, RoutedEventArgs e)
        {
            AtualizarLista();
        }

        private void AtualizarLista()
        {
            var pendentes = _mainViewModel.Visitantes
                .Where(v => !v.SaidaMarcada)
                .ToList();
            
            // CORRIGIDO: Encontra o DataGrid pelo nome
            var dataGrid = (DataGrid)FindName("DataGridVisitantes");
            if (dataGrid != null)
                dataGrid.ItemsSource = pendentes;
        }

        private void ButtonMarcarSaida_Click(object sender, RoutedEventArgs e)
        {
            var dataGrid = (DataGrid)FindName("DataGridVisitantes");
            if (dataGrid?.SelectedItem is Visitante visitante)
            {
                visitante.HoraSaida = DateTime.Now;
                visitante.SaidaMarcada = true;
                AtualizarLista();
                MessageBox.Show($"✅ SAÍDA MARCADA!\n\n👤 {visitante.Nome}\n🚗 Placa: {visitante.Placa}\n🕐 Hora Saída: {visitante.HoraSaida:HH:mm:ss}", "Sucesso!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("❌ Selecione um visitante!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void FecharBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
