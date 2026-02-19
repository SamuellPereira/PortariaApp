using System.ComponentModel;
using System.Runtime.CompilerServices;
using System;

namespace PortariaApp.Models
{
    public class Visitante : INotifyPropertyChanged
    {
        private int _id;
        private string _nome = "", _rg = "", _cpf = "", _telefone = "";
        private string _dataNasc = "", _placa = "", _fotoPath = "";
        private string _filial = "", _endereco = "", _bairro = "", _cep = "";
        private int _tipo;
        private DateTime _horaEntrada = DateTime.Now;
        private DateTime? _horaSaida;
        private bool _saidaMarcada;
        private string _filialOrigem = "";
        private string _empresaMandou = "";

        public int Id { get => _id; set { _id = value; OnPropertyChanged(); } }
        public string Nome { get => _nome; set { _nome = value ?? ""; OnPropertyChanged(); } }
        public string RG { get => _rg; set { _rg = value ?? ""; OnPropertyChanged(); } }
        public string CPF { get => _cpf; set { _cpf = value ?? ""; OnPropertyChanged(); } }
        public string Telefone { get => _telefone; set { _telefone = value ?? ""; OnPropertyChanged(); } }
        public string DataNasc { get => _dataNasc; set { _dataNasc = value ?? ""; OnPropertyChanged(); } }
        public string Placa { get => _placa; set { _placa = value ?? ""; OnPropertyChanged(); } }
        public string FotoPath { get => _fotoPath; set { _fotoPath = value ?? ""; OnPropertyChanged(); } }
        public string Filial { get => _filial; set { _filial = value ?? ""; OnPropertyChanged(); } }
        public string Endereco { get => _endereco; set { _endereco = value ?? ""; OnPropertyChanged(); } }
        public string Bairro { get => _bairro; set { _bairro = value ?? ""; OnPropertyChanged(); } }
        public string CEP { get => _cep; set { _cep = value ?? ""; OnPropertyChanged(); } }
        public int Tipo { get => _tipo; set { _tipo = value; OnPropertyChanged(); } }
        public DateTime HoraEntrada { get => _horaEntrada; set { _horaEntrada = value; OnPropertyChanged(); } }
        public DateTime? HoraSaida { get => _horaSaida; set { _horaSaida = value; OnPropertyChanged(); } }
        public bool SaidaMarcada { get => _saidaMarcada; set { _saidaMarcada = value; OnPropertyChanged(); } }
        public string FilialOrigem { get => _filialOrigem; set { _filialOrigem = value ?? ""; OnPropertyChanged(); } }
        public string EmpresaMandou { get => _empresaMandou; set { _empresaMandou = value ?? ""; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
