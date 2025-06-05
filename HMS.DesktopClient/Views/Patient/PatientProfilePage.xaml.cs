using HMS.DesktopClient.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;

namespace HMS.DesktopClient.Views.Patient
{
    public sealed partial class PatientProfilePage : Page
    {
        private PatientViewModel _patientViewModel;

        public PatientProfilePage()
        {
            this.InitializeComponent();
        }

        public PatientProfilePage(PatientViewModel patientViewModel)
        {
            InitializeComponent();
            this._patientViewModel = patientViewModel;
            this.DataContext = _patientViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is PatientViewModel viewModel)
            {
                _patientViewModel = viewModel;
                DataContext = _patientViewModel;
            }
        }
    }
}