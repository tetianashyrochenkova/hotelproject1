using Cours_Work.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cours_Work.Repository
{
    [Serializable]
    internal class RoomsRepository
    {
        private const string _filePath = "rooms.json";
        private GuestsRepository _guestsRepository;
        private readonly DateOnly _today;

        public List<Room> Rooms { get; set; }

        public RoomsRepository()
        {
            _today = DateOnly.FromDateTime(DateTime.Today);
            Rooms = LoadRooms();
            _guestsRepository = new GuestsRepository();
            Reocupate();
        }

        private static List<Room> LoadRooms()
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                return JsonConvert.DeserializeObject<List<Room>>(json) ?? new List<Room>();
            }
            else
            {
                return new List<Room>();
            }
        }

        private void SaveRooms()
        {
            string json = JsonConvert.SerializeObject(Rooms);
            File.WriteAllText(_filePath, json);
        }

        public Room? GetRoom(int number)
        {
            return Rooms.FirstOrDefault(e => e.Number == number);
        }

        public string? AddRoom(Room newRoom)
        {
            if (Rooms.Any(e => e.Number == newRoom.Number))
            {
                return "Room with this number already exists";
            }
            Rooms.Add(newRoom);
            SaveRooms();
            return null;
        }

        public string? RemoveRoom(int number)
        {
            Room? room = GetRoom(number);
            if (room == null)
            {
                return "Room with this number doesn't exist";
            }
            Rooms.Remove(room);
            SaveRooms();
            return null;
        }

        public string? UpdateRoom(Room roomForUpdate)
        {
            Room? room = GetRoom(roomForUpdate.Number);
            if (room == null)
            {
                return "Room with this number doesn't exist";
            }
            room.Class = roomForUpdate.Class;
            room.Capacity = roomForUpdate.Capacity;
            room.IsOccupied = roomForUpdate.IsOccupied;
            SaveRooms();
            return null;
        }

        public List<Room> GetRooms(int? roomClass = null, int? capacity = null)
        {
            Reocupate();
            var rooms = Rooms;

            if (roomClass.HasValue && (roomClass.Value >= 1 && roomClass.Value <= 3))
            {
                rooms = rooms.Where(e => e.Class == roomClass.Value).ToList();
            }
            if (capacity.HasValue)
            {
                rooms = rooms.Where(e => e.Capacity == capacity.Value).ToList();
            }

            return rooms;
        }

        public List<Room> IsNotOc(int? roomClass = null, int? capacity = null)
        {
            Reocupate();
            var rooms = Rooms.Where(e => !e.IsOccupied).ToList();

            if (roomClass.HasValue && (roomClass.Value >= 1 && roomClass.Value <= 3))
            {
                rooms = rooms.Where(e => e.Class == roomClass.Value).ToList();
            }
            if (capacity.HasValue)
            {
                rooms = rooms.Where(e => e.Capacity == capacity.Value).ToList();
            }

            return rooms;
        }

        public void Ocupate(int roomNumb)
        {
            Room? room = GetRoom(roomNumb);
            if (room != null)
            {
                room.IsOccupied = true;
                SaveRooms();
            }
        }

        public void DeOcupate(int roomNumb)
        {
            Room? room = GetRoom(roomNumb);
            if (room != null)
            {
                room.IsOccupied = false;
                SaveRooms();
            }
        }

        private void Reocupate()
        {
            var yesterday = _today.AddDays(-1);

            foreach (Room room in Rooms)
            {
                Guest? guest = _guestsRepository.Guests.FirstOrDefault(e => e.RoomNumber == room.Number);

                if (guest != null)
                {
                    if (guest.DepartureDate == yesterday)
                    {
                        DeOcupate(room.Number);
                        _guestsRepository.RemoveGuest(guest.PassportNumber);
                    }
                    else if (guest.DepartureDate >= _today && !room.IsOccupied)
                    {
                        Ocupate(room.Number);
                    }
                }
                else if (room.IsOccupied)
                {
                    DeOcupate(room.Number);
                }
            }
        }

    }
}
