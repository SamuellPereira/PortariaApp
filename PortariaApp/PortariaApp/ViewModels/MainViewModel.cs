using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using PortariaApp.Models;

namespace PortariaApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Visitante> Visitantes { get; set; } = new();
        public ObservableCollection<Visitante> VisitantesFiltrados { get; set; } = new();
        private Visitante _selecionado = new();
        private string _filtro = "";

        public Visitante VisitanteSelecionado 
        { 
            get => _selecionado; 
            set { _selecionado = value; OnPropertyChanged(); } 
        }
        public string Filtro 
        { 
            get => _filtro; 
            set { _filtro = value; OnPropertyChanged(); Filtrar(); } 
        }

        public ICommand NovoCommand { get; }
        public ICommand SalvarCommand { get; }
        public ICommand ExcluirCommand { get; }
        public ICommand LimparCommand { get; }

        private int _proximoId = 1;

        public MainViewModel()
        {
            NovoCommand = new RelayCommand(Novo);
            SalvarCommand = new RelayCommand(Salvar);
            ExcluirCommand = new RelayCommand(Excluir);
            LimparCommand = new RelayCommand(Limpar);

            Visitantes.Add(new Visitante 
            { 
                Id = _proximoId++, 
                Nome = "João Silva", 
                RG = "12.345.678-9", 
                CPF = "123.456.789-00",
                Filial = "Matriz",
                Telefone = "(11)99999-9999"
            });
            Filtrar();
        }

        private void Novo() => VisitanteSelecionado = new Visitante { Id = _proximoId++ };
        private void Salvar()
        {
            if (VisitanteSelecionado?.Nome?.Length > 0)
            {
                var existe = Visitantes.FirstOrDefault(v => v.Id == VisitanteSelecionado.Id);
                if (existe != null)
                {
                    var index = Visitantes.IndexOf(existe);
                    Visitantes[index] = VisitanteSelecionado;
                }
                else
                    Visitantes.Add(VisitanteSelecionado);
                Filtrar();
            }
        }
        private void Excluir() 
        {
            if (VisitanteSelecionado != null)
            {
                Visitantes.Remove(VisitanteSelecionado);
                Filtrar();
                VisitanteSelecionado = new Visitante();
            }
        }
        private void Limpar() => VisitanteSelecionado = new Visitante();

        private void Filtrar()
        {
            VisitantesFiltrados.Clear();
            foreach (var v in Visitantes)
            {
                if (string.IsNullOrEmpty(Filtro) || 
                    v.Nome.Contains(Filtro) || 
                    v.RG.Contains(Filtro))
                {
                    VisitantesFiltrados.Add(v);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly System.Action _execute;
        public RelayCommand(System.Action execute) => _execute = execute;
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => _execute();
        public event System.EventHandler CanExecuteChanged;
    }
}
