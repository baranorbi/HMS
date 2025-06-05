using HMS.Shared.DTOs.Patient;
using HMS.Shared.Entities;
using HMS.Shared.Proxies.Implementations;
using HMS.Shared.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HMS.DesktopClient.Services
{
    public class PatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public static PatientService CreateWithToken(string token)
        {
            var httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5203/api/") };
            var patientProxy = new PatientProxy(httpClient, token);
            return new PatientService(patientProxy);
        }

        public async Task<PatientDto> GetPatientById(int id)
        {
            try
            {
                return await _patientRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get patient data: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<PatientDto>> GetAllPatients()
        {
            try
            {
                return await _patientRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get all patients: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdatePatient(Patient patient)
        {
            try
            {
                return await _patientRepository.UpdateAsync(patient);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update patient: {ex.Message}", ex);
            }
        }

        public async Task<Patient> AddPatient(Patient patient)
        {
            try
            {
                return await _patientRepository.AddAsync(patient);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to add patient: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeletePatient(int id)
        {
            try
            {
                return await _patientRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete patient: {ex.Message}", ex);
            }
        }
    }
}