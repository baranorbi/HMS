using HMS.DesktopClient.Controls;
using HMS.DesktopClient.Services;
using HMS.DesktopClient.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace HMS.DesktopClient.Views.Patient
{
    public sealed partial class PatientHomePage : Window
    {
        private PatientViewModel _patientViewModel;

        public PatientHomePage()
        {
            this.InitializeComponent();

            // Setup the patient service and view model
            var patientService = PatientService.CreateWithToken(App.CurrentUser.Token);
            _patientViewModel = new PatientViewModel(patientService, App.CurrentUser.Id);

            // Load patient data
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            try
            {
                await _patientViewModel.LoadPatientData();

                // Initialize the dashboard control
                var dashboardControl = new PatientDashboardControl(_patientViewModel);
                dashboardControl.LogoutButtonClicked += HandleLogoutRequested;

                // Add the control to the main frame
                MainFrame.Content = dashboardControl;
            }
            catch (Exception ex)
            {
                ShowErrorDialog("Error", $"Failed to load patient data: {ex.Message}");
            }
        }

        private void HandleLogoutRequested()
        {
            // Clear current user and token
            App.CurrentUser = null;

            // Navigate back to login page
            var loginPage = new LoginPage();
            loginPage.Activate();
            this.Close();
        }

        private async void Doctors_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Doctors",
                Content = "Doctors button clicked.",
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };
            await dialog.ShowAsync();
        }

        private async void MedicalRecords_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Medical Records",
                Content = "Medical Records button clicked.",
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };
            await dialog.ShowAsync();
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            // Create a new instance of PatientProfilePage and pass the patient view model
            var profilePage = new PatientProfilePage(_patientViewModel);
            MainFrame.Navigate(typeof(PatientProfilePage), profilePage);
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            // Reset to dashboard control
            var dashboardControl = new PatientDashboardControl(_patientViewModel);
            dashboardControl.LogoutButtonClicked += HandleLogoutRequested;
            MainFrame.Content = dashboardControl;
        }

        private async void ShowErrorDialog(string title, string message)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };
            await dialog.ShowAsync();
        }
    }
}