using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
//using static System.Net.Mime.MediaTypeNames;
using System.Windows;


namespace ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private BaseViewModel _currentViewModel;
        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public ICommand ShowReservasCommand { get; }
        public ICommand ShowActividadesCommand { get; }
        public ICommand ShowSociosCommand { get; }
        public ICommand SalirCommand { get; }



        public MainViewModel()
        {
            ShowReservasCommand = new RelayCommand(() =>
                CurrentViewModel = new ReservasViewModel());

            ShowActividadesCommand = new RelayCommand(() =>
                CurrentViewModel = new ActividadesViewModel());

            ShowSociosCommand = new RelayCommand(() =>
                CurrentViewModel = new SociosViewModel());

            SalirCommand = new RelayCommand(() =>
                         Application.Current.Shutdown());

            // Vista inicial
            CurrentViewModel = new ReservasViewModel();
        }
    }

}
