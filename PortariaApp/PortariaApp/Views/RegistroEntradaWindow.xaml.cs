using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PortariaApp.Models;
using PortariaApp.ViewModels;

namespace PortariaApp.Views
{
    public partial class RegistroEntradaWindow : Window
    {
        private MainViewModel _mainViewModel;

        public RegistroEntradaWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            _mainViewModel = viewModel;
            CarregarVisitantes();
        }

        private void CarregarVisitantes()
        {
            DataGridVisitantes.ItemsSource = _mainViewModel.Visitantes;
        }

        private void TxtPesquisar_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string filtro = TxtPesquisar.Text?.ToLower() ?? "";
            var filtrados = _mainViewModel.Visitantes
                .Where(v => v.Nome.ToLower().Contains(filtro) || v.RG.ToLower().Contains(filtro))
                .ToList();
            DataGridVisitantes.ItemsSource = filtrados;
        }

        private void DataGridVisitantes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridVisitantes.SelectedItem is Visitante visitante)
            {
                TxtVisitanteNome.Text = $"👤 {visitante.Nome} | 📄 RG: {visitante.RG}";
            }
        }

        private void BtnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            var selecionado = DataGridVisitantes.SelectedItem as Visitante;
            if (selecionado == null)
            {
                MessageBox.Show("❌ Selecione um visitante!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtEmpresa.Text))
            {
                MessageBox.Show("❌ Informe a empresa!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // ATUALIZA DADOS
            selecionado.FilialOrigem = (ComboFilial.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "";
            selecionado.EmpresaMandou = TxtEmpresa.Text;
            selecionado.HoraEntrada = System.DateTime.Now; // Garante hora atual

            MessageBox.Show($"✅ ENTRADA REGISTRADA!\n\n👤 {selecionado.Nome}\n🏢 {selecionado.FilialOrigem}\n🏭 {selecionado.EmpresaMandou}\n⏰ {selecionado.HoraEntrada:HH:mm:ss}", 
                "Sucesso!", MessageBoxButton.OK, MessageBoxImage.Information);

            DialogResult = true;
            Close();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
