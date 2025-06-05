using HMS.DesktopClient.Services;
using HMS.Shared.DTOs.Patient;
using HMS.Shared.Entities;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace HMS.DesktopClient.ViewModels
{
    public class PatientViewModel : INotifyPropertyChanged
    {
        private readonly PatientService _patientService;
        private int _userId;
        private string _name = "";
        private string _email = "";
        private string _cnp = "";
        private string _phoneNumber = "";
        private string _bloodType = "";
        private string _emergencyContact = "";
        private string _allergies = "";
        private float _weight;
        private float _height;
        private DateTime _birthDate;
        private string _address = "";
        private string _password = "";

        // Keep a reference to the original patient data
        public PatientDto OriginalPatient { get; private set; }

        public PatientViewModel(PatientService patientService, int userId)
        {
            _patientService = patientService;
            _userId = userId;

            // Initialize with empty patient data
            OriginalPatient = new PatientDto();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int UserId
        {
            get => _userId;
            set
            {
                if (_userId != value)
                {
                    _userId = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged();
                }
            }
        }

        public string CNP
        {
            get => _cnp;
            set
            {
                if (_cnp != value)
                {
                    _cnp = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (_phoneNumber != value)
                {
                    _phoneNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        public string BloodType
        {
            get => _bloodType;
            set
            {
                if (_bloodType != value)
                {
                    _bloodType = value;
                    OnPropertyChanged();
                }
            }
        }

        public string EmergencyContact
        {
            get => _emergencyContact;
            set
            {
                if (_emergencyContact != value)
                {
                    _emergencyContact = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Allergies
        {
            get => _allergies;
            set
            {
                if (_allergies != value)
                {
                    _allergies = value;
                    OnPropertyChanged();
                }
            }
        }

        public float Weight
        {
            get => _weight;
            set
            {
                if (_weight != value)
                {
                    _weight = value;
                    OnPropertyChanged();
                }
            }
        }

        public float Height
        {
            get => _height;
            set
            {
                if (_height != value)
                {
                    _height = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime BirthDate
        {
            get => _birthDate;
            set
            {
                if (_birthDate != value)
                {
                    _birthDate = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(BirthDateFormatted));
                }
            }
        }

        public string BirthDateFormatted => BirthDate.ToString("dd/MM/yyyy");

        public string Address
        {
            get => _address;
            set
            {
                if (_address != value)
                {
                    _address = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged();
                }
            }
        }

        public async Task LoadPatientData()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"Loading patient data for user ID: {_userId}");
                var patient = await _patientService.GetPatientById(_userId);
                OriginalPatient = patient;

                // Add debug logging for properties
                System.Diagnostics.Debug.WriteLine($"Patient data loaded: Name={patient.Name}, Email={patient.Email}");

                // Update all view model properties with proper notification
                UserId = _userId;
                Name = patient.Name;
                Email = patient.Email;
                CNP = patient.CNP;
                PhoneNumber = patient.PhoneNumber;
                BloodType = patient.BloodType;
                EmergencyContact = patient.EmergencyContact;
                Allergies = patient.Allergies;
                Weight = patient.Weight;
                Height = patient.Height;
                BirthDate = patient.BirthDate;
                Address = patient.Address;
                Password = ""; // Don't load password, only for updates

                // Force UI refresh by raising PropertyChanged for all properties
                OnPropertyChanged(string.Empty);

                System.Diagnostics.Debug.WriteLine("All patient properties updated in ViewModel");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading patient data: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateEmergencyContact()
        {
            try
            {
                var patient = CreatePatientForUpdate();
                patient.EmergencyContact = EmergencyContact;
                return await _patientService.UpdatePatient(patient);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating emergency contact: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateWeight()
        {
            try
            {
                var patient = CreatePatientForUpdate();
                patient.Weight = Weight;
                return await _patientService.UpdatePatient(patient);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating weight: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateHeight()
        {
            try
            {
                var patient = CreatePatientForUpdate();
                patient.Height = Height;
                return await _patientService.UpdatePatient(patient);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating height: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdatePassword()
        {
            try
            {
                var patient = CreatePatientForUpdate();
                patient.Password = Password;
                return await _patientService.UpdatePatient(patient);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating password: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateName()
        {
            try
            {
                var patient = CreatePatientForUpdate();
                patient.Name = Name;
                return await _patientService.UpdatePatient(patient);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating name: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdatePatientData(bool updateName = false, bool updateEmergencyContact = false,
            bool updateWeight = false, bool updateHeight = false, bool updatePassword = false)
        {
            try
            {
                // Create patient object with original data
                var patient = new Patient
                {
                    Id = UserId,
                    Email = OriginalPatient.Email,
                    // Don't set password by default - we'll set it only if updating
                    Password = updatePassword ? Password : OriginalPatient.Password,
                    // Update only the fields that have changed
                    Name = updateName ? Name : OriginalPatient.Name,
                    CNP = OriginalPatient.CNP,
                    PhoneNumber = OriginalPatient.PhoneNumber,
                    Role = OriginalPatient.Role,
                    BloodType = Enum.Parse<HMS.Shared.Enums.BloodType>(OriginalPatient.BloodType),
                    EmergencyContact = updateEmergencyContact ? EmergencyContact : OriginalPatient.EmergencyContact,
                    Allergies = OriginalPatient.Allergies,
                    Weight = updateWeight ? Weight : OriginalPatient.Weight,
                    Height = updateHeight ? Height : OriginalPatient.Height,
                    BirthDate = OriginalPatient.BirthDate,
                    Address = OriginalPatient.Address
                };

                // Log what we're updating
                System.Diagnostics.Debug.WriteLine($"Updating patient #{UserId} with changes:" +
                                                   $"{(updateName ? " Name," : "")}" +
                                                   $"{(updateEmergencyContact ? " EmergencyContact," : "")}" +
                                                   $"{(updateWeight ? " Weight," : "")}" +
                                                   $"{(updateHeight ? " Height," : "")}" +
                                                   $"{(updatePassword ? " Password" : "")}");

                return await _patientService.UpdatePatient(patient);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating patient data: {ex.Message}");
                throw;
            }
        }

        private Patient CreatePatientForUpdate()
        {
            // Create a new Patient with the original data to update specific fields
            return new Patient
            {
                Id = UserId,
                Email = Email,
                Password = Password, // Use the current password property, not from OriginalPatient since it doesn't exist
                Name = OriginalPatient.Name, // Will be overridden for name update
                CNP = CNP,
                PhoneNumber = PhoneNumber,
                Role = OriginalPatient.Role, // Direct assignment since it's already UserRole enum type
                BloodType = Enum.Parse<HMS.Shared.Enums.BloodType>(BloodType),
                EmergencyContact = OriginalPatient.EmergencyContact, // Will be overridden for emergency contact update
                Allergies = Allergies,
                Weight = OriginalPatient.Weight, // Will be overridden for weight update
                Height = OriginalPatient.Height, // Will be overridden for height update
                BirthDate = BirthDate,
                Address = Address
            };
        }
    }
}