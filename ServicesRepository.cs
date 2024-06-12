using System.Collections.Generic;
using System.Linq;
using Cours_Work.Models;
using Newtonsoft.Json;
using System.IO;

namespace Cours_Work.Repository
{
    internal class ServicesRepository
    {
        private const string _filePath = "services.json";
        private readonly GuestsRepository _guestRepository;

        public List<Service> Services { get; set; }

        public ServicesRepository()
        {
            Services = LoadServices();
            _guestRepository = new GuestsRepository();
        }

        private static List<Service> LoadServices()
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                return JsonConvert.DeserializeObject<List<Service>>(json) ?? new List<Service>();
            }
            else
            {
                return new List<Service>();
            }
        }

        private void SaveServices()
        {
            string json = JsonConvert.SerializeObject(Services);
            File.WriteAllText(_filePath, json);
        }

        public void AddService(Service service)
        {
            Services.Add(service);
            SaveServices();
        }

        public void RemoveService(string serviceName)
        {
            var service = Services.FirstOrDefault(s => s.Name == serviceName);
            if (service != null)
            {
                Services.Remove(service);
                SaveServices();
            }
        }

        public void AddServiceToGuest(Guest guest, Service service)
        {
            if (guest != null && service!=null)
            {
                guest.Services.Add(service);
                _guestRepository.UpdateGuest(guest);
            }
        }
    }
}
