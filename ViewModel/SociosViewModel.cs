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
    public class SociosViewModel : BaseViewModel
    {
        private readonly SocioRepository _repository;

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

        private Socio _selectedSocio;
        public Socio SelectedSocio
        {
            get => _selectedSocio;
            set
            {
                _selectedSocio = value;
                OnPropertyChanged(nameof(SelectedSocio));
                OnPropertyChanged(nameof(IsEditingExistingSocio));
                OnPropertyChanged(nameof(IsFormEnabled));
                
                // Forzar la reevaluación de CanExecute en todos los comandos
                System.Windows.Input.CommandManager.InvalidateRequerySuggested();
            }
        }

        public bool IsEditingExistingSocio => SelectedSocio != null && SelectedSocio.Id != 0;
        
        public bool IsFormEnabled => SelectedSocio != null;

        public ICommand AgregarSocCommand { get; }
        public ICommand GuardarSocCommand { get; }
        public ICommand EditarSocCommand { get; }
        public ICommand EliminarSocCommand { get; }
        public ICommand CancelarSocCommand { get; }

        public SociosViewModel()
        {
            _repository = new SocioRepository();
            Socios = new ObservableCollection<Socio>();

            AgregarSocCommand = new RelayCommand(Agregar);
            GuardarSocCommand = new RelayCommand(Guardar, () => SelectedSocio != null);
            EditarSocCommand = new RelayCommand(Editar, () => SelectedSocio != null);
            EliminarSocCommand = new RelayCommand(Eliminar, () => SelectedSocio != null);
            CancelarSocCommand = new RelayCommand(Cancelar);

            CargarSocios();
        }

        private void CargarSocios()
        {
            Socios.Clear();
            var socios = _repository.GetAll();
            foreach (var socio in socios)
            {
                Socios.Add(socio);
            }
        }

        private void Agregar()
        {
            var nuevoSocio = new Socio
            {
                Nombre = "",
                Email = "",
                Activo = true
            };
            Socios.Add(nuevoSocio);
            SelectedSocio = nuevoSocio;
        }

        private void Guardar()
        {
            if (SelectedSocio == null) return;

            // Validación básica
            if (string.IsNullOrWhiteSpace(SelectedSocio.Nombre))
            {
                System.Windows.MessageBox.Show("El nombre es obligatorio.", "Validación", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(SelectedSocio.Email))
            {
                System.Windows.MessageBox.Show("El email es obligatorio.", "Validación", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (SelectedSocio.Id == 0)
                {
                    // Nuevo socio
                    _repository.Add(SelectedSocio);
                }
                else
                {
                    // Socio existente
                    _repository.Update(SelectedSocio);
                }
                
                CargarSocios();
                SelectedSocio = null;
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
            if (SelectedSocio == null) return;

            _repository.Delete(SelectedSocio.Id);
            CargarSocios();
            SelectedSocio = null;
        }

        private void Cancelar()
        {
            SelectedSocio = null;
            CargarSocios();
        }
    }
}
