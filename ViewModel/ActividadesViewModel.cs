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
    public class ActividadesViewModel : BaseViewModel
    {
        private readonly ActividadRepository _repository;

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

        private Actividad _selectedActividad;
        public Actividad SelectedActividad
        {
            get => _selectedActividad;
            set
            {
                _selectedActividad = value;
                OnPropertyChanged(nameof(SelectedActividad));
                OnPropertyChanged(nameof(IsEditingExistingActividad));
                OnPropertyChanged(nameof(IsFormEnabled));
                
                System.Windows.Input.CommandManager.InvalidateRequerySuggested();
            }
        }

        public bool IsEditingExistingActividad => SelectedActividad != null && SelectedActividad.Id != 0;
        
        public bool IsFormEnabled => SelectedActividad != null;

        public ICommand AgregarCommand { get; }
        public ICommand GuardarCommand { get; }
        public ICommand EditarCommand { get; }
        public ICommand EliminarCommand { get; }
        public ICommand CancelarCommand { get; }

        public ActividadesViewModel()
        {
            _repository = new ActividadRepository();
            Actividades = new ObservableCollection<Actividad>();

            AgregarCommand = new RelayCommand(Agregar);
            GuardarCommand = new RelayCommand(Guardar, () => SelectedActividad != null);
            EditarCommand = new RelayCommand(Editar, () => SelectedActividad != null);
            EliminarCommand = new RelayCommand(Eliminar, () => SelectedActividad != null);
            CancelarCommand = new RelayCommand(Cancelar);

            CargarActividades();
        }

        private void CargarActividades()
        {
            Actividades.Clear();
            var actividades = _repository.GetAll();
            foreach (var actividad in actividades)
            {
                Actividades.Add(actividad);
            }
        }

        private void Agregar()
        {
            var nuevaActividad = new Actividad
            {
                Nombre = "",
                AforoMaximo = 0
            };
            Actividades.Add(nuevaActividad);
            SelectedActividad = nuevaActividad;
        }

        private void Guardar()
        {
            if (SelectedActividad == null) return;

            if (string.IsNullOrWhiteSpace(SelectedActividad.Nombre))
            {
                System.Windows.MessageBox.Show("El nombre es obligatorio.", "Validación", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            if (SelectedActividad.AforoMaximo <= 0)
            {
                System.Windows.MessageBox.Show("El aforo máximo debe ser mayor a 0.", "Validación", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (SelectedActividad.Id == 0)
                {
                    _repository.Add(SelectedActividad);
                }
                else
                {
                    _repository.Update(SelectedActividad);
                }
                
                CargarActividades();
                SelectedActividad = null;
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
            if (SelectedActividad == null) return;

            _repository.Delete(SelectedActividad.Id);
            CargarActividades();
            SelectedActividad = null;
        }

        private void Cancelar()
        {
            SelectedActividad = null;
            CargarActividades();
        }
    }
}
