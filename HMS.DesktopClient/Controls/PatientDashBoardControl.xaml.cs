using HMS.DesktopClient.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HMS.DesktopClient.Controls
{
    public sealed partial class PatientDashboardControl : UserControl
    {
        private PatientViewModel? _patientViewModel;
        public event Action? LogoutButtonClicked;

        public PatientDashboardControl()
        {
            this.InitializeComponent();
        }

        public PatientDashboardControl(PatientViewModel patientViewModel)
        {
            InitializeComponent();
            this._patientViewModel = patientViewModel;
            DataContext = this._patientViewModel;

            // Add logging to confirm if the DataContext is set correctly
            Debug.WriteLine($"DataContext set to: {patientViewModel?.GetType().Name}");
        }

        public void NotificationsButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement notification functionality
            ShowInfoDialog("Notifications", "Notification feature will be implemented soon.");
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LogoutButtonClicked?.Invoke();
        }

        private async void OnUpdateButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this._patientViewModel == null)
                    throw new Exception("Patient is not initialized");

                // Validate required fields
                if (string.IsNullOrWhiteSpace(_patientViewModel.Name))
                {
                    await ShowErrorDialog("Validation Error", "Name cannot be empty.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(_patientViewModel.EmergencyContact))
                {
                    await ShowErrorDialog("Validation Error", "Emergency contact cannot be empty.");
                    return;
                }

                if (_patientViewModel.Weight <= 0)
                {
                    await ShowErrorDialog("Validation Error", "Weight must be a positive value.");
                    return;
                }

                if (_patientViewModel.Height <= 0)
                {
                    await ShowErrorDialog("Validation Error", "Height must be a positive value.");
                    return;
                }

                // Check if password is provided - if yes, validate it
                bool passwordChanged = false;
                if (!string.IsNullOrWhiteSpace(_patientViewModel.Password))
                {
                    if (_patientViewModel.Password.Length < 6)
                    {
                        await ShowErrorDialog("Validation Error", "Password must be at least 6 characters long.");
                        return;
                    }
                    passwordChanged = true;
                }

                // Track which fields have changed
                bool emergencyContactChanged = _patientViewModel.EmergencyContact != _patientViewModel.OriginalPatient.EmergencyContact;
                bool weightChanged = Math.Abs(_patientViewModel.Weight - _patientViewModel.OriginalPatient.Weight) > 0.01f;
                bool heightChanged = Math.Abs(_patientViewModel.Height - _patientViewModel.OriginalPatient.Height) > 0.01f;
                bool nameChanged = _patientViewModel.Name != _patientViewModel.OriginalPatient.Name;

                // Only update if at least one field has changed
                bool hasChanges = emergencyContactChanged || weightChanged || heightChanged || nameChanged || passwordChanged;

                if (hasChanges)
                {
                    // Perform the update with all changed fields
                    bool updateSuccessful = await _patientViewModel.UpdatePatientData(
                        updateName: nameChanged,
                        updateEmergencyContact: emergencyContactChanged,
                        updateWeight: weightChanged,
                        updateHeight: heightChanged,
                        updatePassword: passwordChanged
                    );

                    if (updateSuccessful)
                    {
                        string message = passwordChanged ?
                            "Your information and password have been updated successfully. Please log in again with your new password." :
                            "Your information has been updated successfully.";

                        await ShowSuccessDialog("Update Successful", message);

                        if (passwordChanged)
                        {
                            // Logout if the password was changed
                            LogoutButtonClicked?.Invoke();
                        }
                        else
                        {
                            // Refresh patient data
                            await this._patientViewModel.LoadPatientData();
                        }
                    }
                    else
                    {
                        await ShowErrorDialog("Update Failed", "Failed to update your information.");
                    }
                }
                else
                {
                    await ShowInfoDialog("No Changes", "No changes were made.");
                }
            }
            catch (Exception ex)
            {
                await ShowErrorDialog("Update Failed", $"An error occurred: {ex.Message}");
            }
        }

        private async Task ShowSuccessDialog(string title, string message)
        {
            var successDialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            await successDialog.ShowAsync();
        }

        private async Task ShowErrorDialog(string title, string message)
        {
            var errorDialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            await errorDialog.ShowAsync();
        }

        private async Task ShowInfoDialog(string title, string message)
        {
            var infoDialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            await infoDialog.ShowAsync();
        }
    }
}