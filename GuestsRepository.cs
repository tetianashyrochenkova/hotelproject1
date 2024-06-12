using Cours_Work.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cours_Work.Repository
{
    internal class GuestsRepository
    {
        private const string _filePath = "guests.json";

        public List<Guest> Guests { get; set; }

        public GuestsRepository()
        {
            Guests = LoadGuests();
        }

        private static List<Guest> LoadGuests()
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                return JsonConvert.DeserializeObject<List<Guest>>(json) ?? new List<Guest>();
            }
            else
            {
                return new List<Guest>();
            }
        }

        private void SaveGuests()
        {
            string json = JsonConvert.SerializeObject(Guests);
            File.WriteAllText(_filePath, json);
        }

        public List<Guest> FindGuests(string? passport, string? name, string? surname)
        {
            var guests = Guests;

            if (!string.IsNullOrEmpty(passport))
            {
                guests = guests.Where(g => g.PassportNumber.Contains(passport)).ToList();
            }
            if (!string.IsNullOrEmpty(name))
            {
                guests = guests.Where(g => g.Name.Contains(name)).ToList();
            }
            if (!string.IsNullOrEmpty(surname))
            {
                guests = guests.Where(g => g.Surname.Contains(surname)).ToList();
            }

            return guests;
        }

        public string? AddGuest(Guest newGuest)
        {
            if (!GuestExist(newGuest.PassportNumber))
            {
                Guests.Add(newGuest);
                SaveGuests();
                return null;
            }
            return "Guest with this passport already exists";
        }

        public bool GuestExist(string passport)
        {
            return Guests.Any(e => e.PassportNumber == passport);
        }

        public string? RemoveGuest(string passportNumber)
        {
            var guest = Guests.FirstOrDefault(e => e.PassportNumber == passportNumber);
            if (guest == null)
            {
                return "Guest with this passport doesn't exist";
            }

            Guests.Remove(guest);
            SaveGuests();
            return null;
        }

        public string? UpdateGuest(Guest updatedGuest)
        {
            var guest = Guests.FirstOrDefault(e => e.PassportNumber == updatedGuest.PassportNumber);
            if (guest == null)
            {
                return "Guest with this passport doesn't exist";
            }

            guest.Name = updatedGuest.Name;
            guest.Surname = updatedGuest.Surname;
            guest.ArrivalDate = updatedGuest.ArrivalDate;
            guest.DepartureDate = updatedGuest.DepartureDate;
            guest.RoomNumber = updatedGuest.RoomNumber;

            SaveGuests();
            return null;
        }
    }
}
