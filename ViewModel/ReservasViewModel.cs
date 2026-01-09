using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Models;

namespace ViewModel
{
    public class ReservasViewModel : BaseViewModel
    {
        private readonly ReservaRepository _repository;

        private ObservableCollection<Reserva> _reservas;
        public ObservableCollection<Reserva> Reservas
        {
            get => _reservas;
            set
            {
                _reservas = value;
                OnPropertyChanged(nameof(Reservas));
            }
        }

        private Reserva _selectedReserva;
        public Reserva SelectedReserva
        {
            get => _selectedReserva;
            set
            {
                _selectedReserva = value;
                OnPropertyChanged(nameof(SelectedReserva));
                OnPropertyChanged(nameof(IsEditingExistingReserva));
                OnPropertyChanged(nameof(IsFormEnabled));
                
                System.Windows.Input.CommandManager.InvalidateRequerySuggested();
            }
        }

        private ObservableCollection<Socio> _socios;
        public ObservableCollection<Socio> Socios
        {
            get => _socios;
            set
            {
                _socios = value;
                OnPropertyChanged(nameof(Socios));
            }
        }

        private ObservableCollection<Actividad> _actividades;
        public ObservableCollection<Actividad> Actividades
        {
            get => _actividades;
            set
            {
                _actividades = value;
                OnPropertyChanged(nameof(Actividades));
            }
        }

        public bool IsEditingExistingReserva => SelectedReserva != null && SelectedReserva.Id != 0;
        
        public bool IsFormEnabled => SelectedReserva != null;

        public ICommand AgregarCommand { get; }
        public ICommand EditarCommand { get; }
        public ICommand GuardarCommand { get; }
        public ICommand EliminarCommand { get; }
        public ICommand CancelarCommand { get; }

        public ReservasViewModel()
        {
            _repository = new ReservaRepository();
            Reservas = new ObservableCollection<Reserva>();
            Socios = new ObservableCollection<Socio>();
            Actividades = new ObservableCollection<Actividad>();

            AgregarCommand = new RelayCommand(Agregar);
            GuardarCommand = new RelayCommand(Guardar, () => SelectedReserva != null);
            EditarCommand = new RelayCommand(Editar, () => SelectedReserva != null);
            EliminarCommand = new RelayCommand(Eliminar, () => SelectedReserva != null);
            CancelarCommand = new RelayCommand(Cancelar);

            CargarDatos();
        }

        private void CargarDatos()
        {
            CargarReservas();
            CargarSocios();
            CargarActividades();
        }

        private void CargarReservas()
        {
            Reservas.Clear();
            var reservas = _repository.GetAll();
            foreach (var reserva in reservas)
            {
                Reservas.Add(reserva);
            }
        }

        private void CargarSocios()
        {
            Socios.Clear();
            var socios = _repository.GetSociosActivos();
            foreach (var socio in socios)
            {
                Socios.Add(socio);
            }
        }

        private void CargarActividades()
        {
            Actividades.Clear();
            var actividades = _repository.GetAllActividades();
            foreach (var actividad in actividades)
            {
                Actividades.Add(actividad);
            }
        }

        private void Agregar()
        {
            var nuevaReserva = new Reserva
            {
                SocioId = 0,
                ActividadId = 0,
                Fecha = DateTime.Today
            };
            Reservas.Add(nuevaReserva);
            SelectedReserva = nuevaReserva;
        }

        private void Guardar()
        {
            if (SelectedReserva == null) return;

            if (SelectedReserva.SocioId == 0)
            {
                System.Windows.MessageBox.Show("Debe seleccionar un socio.", "Validación", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            if (SelectedReserva.ActividadId == 0)
            {
                System.Windows.MessageBox.Show("Debe seleccionar una actividad.", "Validación", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            int? reservaIdExcluir = SelectedReserva.Id == 0 ? (int?)null : SelectedReserva.Id;
            if (!_repository.ValidarReserva(SelectedReserva.SocioId, SelectedReserva.ActividadId, SelectedReserva.Fecha, reservaIdExcluir))
            {
                System.Windows.MessageBox.Show(
                    "El socio ya tiene una reserva para este día. No se permite más de una reserva por día.", 
                    "Validación", 
                    System.Windows.MessageBoxButton.OK, 
                    System.Windows.MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (SelectedReserva.Id == 0)
                {
                    _repository.Add(SelectedReserva);
                }
                else
                {
                    _repository.Update(SelectedReserva);
                }
                
                CargarReservas();
                SelectedReserva = null;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al guardar: {ex.Message}", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void Editar()
        {
        }

        private void Eliminar()
        {
            if (SelectedReserva == null) return;

            var resultado = System.Windows.MessageBox.Show(
                "¿Está seguro que desea eliminar esta reserva?", 
                "Confirmar eliminación", 
                System.Windows.MessageBoxButton.YesNo, 
                System.Windows.MessageBoxImage.Question);

            if (resultado == System.Windows.MessageBoxResult.Yes)
            {
                _repository.Delete(SelectedReserva.Id);
                CargarReservas();
                SelectedReserva = null;
            }
        }

        private void Cancelar()
        {
            SelectedReserva = null;
            CargarReservas();
        }
    }
}
