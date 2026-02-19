#nullable disable warnings

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using PortariaApp.Models;
using System.Runtime.CompilerServices;
using System.Windows;

namespace PortariaApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Visitante> Visitantes { get; set; } = new();
        public ObservableCollection<Visitante> VisitantesFiltrados { get; set; } = new();
        public ObservableCollection<Visitante> VisitantesPendentes { get; set; } = new();
        
        private Visitante _selecionado = new();
        private string _filtro = "";
        private int _proximoId = 1;
        private bool _isEditMode = false;

        public Visitante VisitanteSelecionado 
        { get => _selecionado; set { _selecionado = value ?? new(); OnPropertyChanged(); } }
        
        public string Filtro 
        { get => _filtro; set { _filtro = value ?? ""; OnPropertyChanged(); Filtrar(); } }

        public bool IsEditMode 
        { get => _isEditMode; set { _isEditMode = value; OnPropertyChanged(); } }

        public ICommand NovoCommand { get; }
        public ICommand SalvarCommand { get; }
        public ICommand ExcluirCommand { get; }
        public ICommand LimparCommand { get; }
        public ICommand AtualizarListaCommand { get; }
        public ICommand EditarCommand { get; }
        public ICommand RegistrarEntradaCommand { get; }

        public MainViewModel()
        {
            NovoCommand = new RelayCommand(_ => Novo());
            SalvarCommand = new RelayCommand(_ => Salvar(), _ => CanSalvar());
            ExcluirCommand = new RelayCommand(_ => Excluir(), _ => CanExcluir());
            LimparCommand = new RelayCommand(_ => Limpar());
            AtualizarListaCommand = new RelayCommand(_ => AtualizarLista());
            EditarCommand = new RelayCommand(_ => AlternarEdicao());
            RegistrarEntradaCommand = new RelayCommand(_ => AbrirRegistroEntrada());

            var joao = new Visitante 
            { 
                Id = _proximoId++, Nome = "João Silva", RG = "12.345.678-9",
                Telefone = "(11)99999-9999", Filial = "Matriz", HoraEntrada = DateTime.Now
            };
            Visitantes.Add(joao);
            
            AtualizarLista();
            Filtrar();
        }

        private void Novo() 
        { VisitanteSelecionado = new Visitante { Id = _proximoId++, HoraEntrada = DateTime.Now }; IsEditMode = true; }

        private bool CanSalvar() => !string.IsNullOrEmpty(VisitanteSelecionado?.Nome);
        private bool CanExcluir() => VisitanteSelecionado?.Id > 0;

        private void Salvar()
        {
            if (!string.IsNullOrEmpty(VisitanteSelecionado?.Nome))
            {
                var existe = Visitantes.FirstOrDefault(v => v.Id == VisitanteSelecionado.Id);
                if (existe != null) Visitantes[Visitantes.IndexOf(existe)] = VisitanteSelecionado;
                else Visitantes.Add(VisitanteSelecionado);
                
                IsEditMode = false; AtualizarLista(); Filtrar();
            }
        }

        private void Excluir() 
        {
            if (VisitanteSelecionado != null && Visitantes.Contains(VisitanteSelecionado))
            {
                Visitantes.Remove(VisitanteSelecionado);
                AtualizarLista(); Filtrar();
                VisitanteSelecionado = new Visitante(); IsEditMode = false;
            }
        }

        private void Limpar() { VisitanteSelecionado = new Visitante(); IsEditMode = false; }

        private void AlternarEdicao()
        {
            if (IsEditMode) Salvar();
            else { if (VisitanteSelecionado.Id == 0) Novo(); IsEditMode = true; }
        }

        private void AbrirRegistroEntrada()
        {
            try
            {
                var window = new Views.RegistroEntradaWindow(this);
                window.ShowDialog();
                AtualizarLista(); Filtrar();
            }
            catch { /* Ignora se RegistroEntradaWindow não existe ainda */ }
        }

        private void AtualizarLista()
        {
            VisitantesPendentes.Clear();
            foreach (var v in Visitantes.Where(v => !v.SaidaMarcada))
                VisitantesPendentes.Add(v);
            OnPropertyChanged(nameof(VisitantesPendentes));
        }

        private void Filtrar()
        {
            VisitantesFiltrados.Clear();
            var filtro = Filtro.ToLower();
            foreach (var v in Visitantes)
                if (string.IsNullOrEmpty(filtro) || v.Nome.ToLower().Contains(filtro) || v.RG.ToLower().Contains(filtro))
                    VisitantesFiltrados.Add(v);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public RelayCommand(Action execute) : this(o => execute(), null) { }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;
        public void Execute(object? parameter) => _execute(parameter!);
    }
}
