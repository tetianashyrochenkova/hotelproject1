using Cours_Work.Models;
using Cours_Work.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Cours_Work
{
    internal class App
    {
        private RoomsRepository _rooms;
        private GuestsRepository _guests;
        private ServicesRepository _services;

        public App()
        {
            _guests = new GuestsRepository();
            _rooms = new RoomsRepository();
            _services = new ServicesRepository();
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Hotel Management System");
                Console.WriteLine("1. View all guests");
                Console.WriteLine("2. View all rooms");
                Console.WriteLine("3. View free rooms");
                Console.WriteLine("4. Add new guest");
                Console.WriteLine("5. Add new room");
                Console.WriteLine("6. Delay Check-out");
                Console.WriteLine("7. Check-out guests");
                Console.WriteLine("8. Search guests");
                Console.WriteLine("9. Early checkout");
                Console.WriteLine("10. Update guest information");
                Console.WriteLine("11. Update room information");
                Console.WriteLine("12. Manage services");
                Console.WriteLine("13. Add service to guest's bill"); 
                Console.WriteLine("14. View guest's services"); 

                Console.WriteLine("0. Exit");
                Console.WriteLine();
                Console.Write("Choose an option: ");
                var choice = Console.ReadLine().Trim();

                switch (choice)
                {
                    case "1":
                        GetGuests();
                        break;
                    case "2":
                        GetRooms();
                        break;
                    case "3":
                        GetFreeRooms();
                        break;
                    case "4":
                        CheckInGuest();
                        break;
                    case "5":
                        AddNewRoom();
                        break;
                    case "6":
                        DelayCheckout();
                        break;
                    case "7":
                        CheckOutGuests();
                        break;
                    case "8":
                        SearchGuests();
                        break;
                    case "9":
                        EarlyCheckout();
                        break;
                    case "10":
                        UpdateGuestInformation();
                        break;
                    case "11":
                        UpdateRoomInformation();
                        break;
                    case "12":
                        ManageServices();
                        break;
                    case "13":
                        AddServiceToGuestBill();
                        break;
                    case "14":
                        ViewGuestServices();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private void GetGuests()
        {
            if (!_guests.Guests.Any())
            {
                Console.WriteLine("No guests found.");
            }
            else
            {
                 
                foreach (Guest g in _guests.Guests)
                {
                    Console.WriteLine(g.ToString());
                } 
            }
        }

        private void GetRooms()
        {
            int roomClass;
            Console.WriteLine("\nEnter new room class (leave blank with - to don't select):");
            while (true)
            {
                var roomClassInput = GetInput();
                if (roomClassInput == null) return;

                if (string.IsNullOrEmpty(roomClassInput) || roomClassInput == "-")
                {
                    roomClass = 0;
                    break;
                }
                else if (int.TryParse(roomClassInput, out  roomClass) && roomClass >= 1 && roomClass <= 3)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Room class must be between 1 and 3. Please try again.");
                }

            }

            int capacity;
            while (true)
            {
                Console.WriteLine("\nEnter new room capacity  (leave blank with - to don't select):");
                string capacityInput = GetInput();
                if (capacityInput == null) return;

                if (string.IsNullOrEmpty(capacityInput) || capacityInput == "-")
                {
                    capacity =0;
                    break;
                }
                else if (int.TryParse(capacityInput, out capacity) && capacity > 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Capacity must be a positive integer. Please try again.");
                }
            }

            int? cap;
            if(capacity == 0)
            {
                cap = null;
            }
            else
            {
                cap = capacity;
            }

            int? roomc;
            if (roomClass == 0)
            {
                roomc = null;
            }
            else
            {
                roomc = roomClass;
            }

            var rooms = _rooms.GetRooms(roomc, cap);

            if (!rooms.Any())
            {
                Console.WriteLine("No rooms found.");
                return;
            }

             
            foreach (Room room in rooms)
            {
                Console.WriteLine(room);
            }
             
        }

        private void GetFreeRooms()
        {
            int roomClass;
            Console.WriteLine("\nEnter new room class (leave blank with - to don't select):");
            while (true)
            {
                var roomClassInput = GetInput();
                if (roomClassInput == null) return;

                if (string.IsNullOrEmpty(roomClassInput) || roomClassInput == "-")
                {
                    roomClass = 0;
                    break;
                }
                else if (int.TryParse(roomClassInput, out roomClass) && roomClass >= 1 && roomClass <= 3)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Room class must be between 1 and 3. Please try again.");
                }

            }

            int capacity;
            while (true)
            {
                Console.WriteLine("\nEnter new room capacity  (leave blank with - to don't select):");
                string capacityInput = GetInput();
                if (capacityInput == null) return;

                if (string.IsNullOrEmpty(capacityInput) || capacityInput == "-")
                {
                    capacity = 0;
                    break;
                }
                else if (int.TryParse(capacityInput, out capacity) && capacity > 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Capacity must be a positive integer. Please try again.");
                }
            }

            int? cap;
            if (capacity == 0)
            {
                cap = null;
            }
            else
            {
                cap = capacity;
            }

            int? roomc;
            if (roomClass == 0)
            {
                roomc = null;
            }
            else
            {
                roomc = roomClass;
            }

            var freeRooms = _rooms.IsNotOc(roomc, cap);

            if (!freeRooms.Any())
            {
                Console.WriteLine("No free rooms found.");
                return;
            }

             
            foreach (Room room in freeRooms)
            {
                Console.WriteLine(room);
            }
             
        }

        private void CheckInGuest()
        {
            while (true)
            {
                 
                Console.WriteLine("Enter data about new guest:");

                string passport;
                while (true)
                {
                    Console.Write("\nEnter passport number: ");
                    passport = GetInput();
                    if (passport == null) return;

                    if (string.IsNullOrEmpty(passport))
                    {
                        Console.WriteLine("passport number cannot be empty.");
                        continue;
                    }
                    if (!_guests.GuestExist(passport))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Guest with this passport already exists.");
                    }
                }

                string name;
                while (true)
                {
                    Console.Write("\nEnter name: ");
                    name = GetInput();
                    if (name == null) return;
                    if (string.IsNullOrEmpty(name))
                    {
                        Console.WriteLine("Name cannot be empty.");
                        continue;
                    }
                    if (passport == null) return;
                    break;
                }

                string surname;
                while (true)
                {
                    Console.Write("\nEnter surname: ");
                    surname = GetInput();
                    if (surname == null) return;
                    if (string.IsNullOrEmpty(surname))
                    {
                        Console.WriteLine("Surname cannot be empty.");
                        continue;
                    }

                    break;
                }

                DateOnly arrival = DateOnly.FromDateTime(DateTime.Today);

                DateOnly departure;
                while (true)
                {
                    Console.Write("\nEnter date of departure (YYYY-MM-DD): ");
                    var input = GetInput();
                    if (input == null) return;
                    if (string.IsNullOrEmpty(input))
                    {
                        Console.WriteLine("Date of departure cannot be empty.");
                        continue;
                    }

                    if (DateOnly.TryParse(input, out departure))
                    {
                        if (departure <= arrival)
                        {
                            Console.WriteLine("Date of departure must be later than date of arrival.");
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                       

                        Console.WriteLine("Wrong date format.");
                    }
                }

                int roomClass;
                while (true)
                {
                    Console.Write("\nEnter room class (1-3): ");
                    var input = GetInput();
                    if (input == null) return;
                    if (string.IsNullOrEmpty(passport))
                    {
                        Console.WriteLine("Passport number cannot be empty.");
                        continue;
                    }
                    if (int.TryParse(input, out roomClass) && roomClass >= 1 && roomClass <= 3)
                    {
                        break;
                    }
                    else
                    {
                       
                        Console.WriteLine("Room class must be between 1 and 3.");
                    }
                }

                int capacity;
                while (true)
                {
                    Console.Write("\nEnter room capacity: ");
                    var input = GetInput();
                    if (input == null) return;
                    if (string.IsNullOrEmpty(input))
                    {
                        Console.WriteLine("Room capacity cannot be empty.");
                        continue;
                    }
                    if (int.TryParse(input, out capacity) && capacity > 0)
                    {
                        break;
                    }
                    else
                    {
                       
                        Console.WriteLine("Capacity must be a positive integer.");
                    }
                }

                var freeRooms = _rooms.IsNotOc(roomClass, capacity);
                if (!freeRooms.Any())
                {
                    Console.WriteLine("No available rooms match the criteria. Please try again.");
                    continue;
                }

                Console.WriteLine("\nAvailable rooms:");
                foreach (var room in freeRooms)
                {
                    Console.WriteLine(room);
                }

                int roomNumber;
                while (true)
                {
                    Console.Write("\nEnter room number from the available rooms: ");
                    var input = GetInput();
                    if (input == null) return;
                    if (string.IsNullOrEmpty(input))
                    {
                        Console.WriteLine("Room number cannot be empty.");
                        continue;
                    }
                    if (int.TryParse(input, out roomNumber) && freeRooms.Any(r => r.Number == roomNumber))
                    {
                        break;
                    }
                    else
                    {
                       
                        Console.WriteLine("Invalid room number. Please select a room from the available rooms.");
                    }
                }

                Guest newGuest = new Guest(passport, name, surname, arrival, departure, roomNumber);

                Console.WriteLine(newGuest);

                Console.WriteLine("Add guest? (Y/N)");
                var ans = Console.ReadKey().KeyChar;
                if (ans == 'Y' || ans == 'y')
                {
                    _guests.AddGuest(newGuest);
                    _rooms.Ocupate(newGuest.RoomNumber);
                    GenerateReceipt(newGuest);
                }
                 
                break;
            }
        }

        private void GenerateReceipt(Guest guest)
        {
            Console.WriteLine($"\nReceipt:");
            Console.WriteLine($"Guest: {guest.Name} {guest.Surname}");
            Console.WriteLine($"Passport: {guest.PassportNumber}");
            Console.WriteLine($"Room Number: {guest.RoomNumber}");
            Console.WriteLine($"Arrival Date: {guest.ArrivalDate}");
            Console.WriteLine($"Departure Date: {guest.DepartureDate}");

            var room = _rooms.GetRoom(guest.RoomNumber);
            double price;
            if (room.Class == 1)
            {
                price = 500;
            }
            else if (room.Class == 2)
            {
                price = 300;
            }
            else
            {
                price = 150;
            }

            var countOfDays = (guest.DepartureDate.ToDateTime(TimeOnly.MinValue) - guest.ArrivalDate.ToDateTime(TimeOnly.MinValue)).Days;
            price *= countOfDays * room.Capacity;

            Console.WriteLine($"Count of days in hotel: {countOfDays}");
            Console.WriteLine($"Bill: {price}");

        }

        private void AddNewRoom()
        {
            while (true)
            {
                 
                Console.WriteLine("Enter data about new room:");

                int roomClass;
                while (true)
                {
                    Console.Write("\nEnter room class: ");
                    var input = GetInput();
                    if (input == null) return;
                    if (string.IsNullOrEmpty(input))
                    {
                        Console.WriteLine("Room class cannot be empty.");
                        continue;
                    }
                    if (int.TryParse(input, out roomClass))
                    {
                        if (roomClass < 1 || roomClass > 3)
                        {
                           
                            Console.WriteLine("Room class must be between 1 and 3.");
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Wrong room class format.");
                    }
                }

                int number;
                while (true)
                {
                    Console.Write("\nEnter room number: ");
                    var input = GetInput();
                    if (input == null) return;
                    if (string.IsNullOrEmpty(input))
                    {
                        Console.WriteLine("Rooom number cannot be empty.");
                        continue;
                    }
                    if (int.TryParse(input, out number))
                    {
                        if (_rooms.GetRoom(number) != null)
                        {
                           
                            Console.WriteLine("This room already exists.");
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                       
                        Console.WriteLine("Wrong room number format.");
                    }
                }

                int capacity;
                while (true)
                {
                    Console.Write("\nEnter room capacity: ");
                    var input = GetInput();
                    if (input == null) return;
                    if (string.IsNullOrEmpty(input))
                    {
                        Console.WriteLine("Room capacity cannot be empty.");
                        continue;
                    }
                    if (int.TryParse(input, out capacity))
                    {
                        break;
                    }
                    else
                    {
                       
                        Console.WriteLine("Wrong capacity format.");
                    }
                }

                Room newRoom = new Room(roomClass, number, capacity);

                Console.WriteLine(newRoom);

                Console.WriteLine("Add room? (Y/N)");
                var ans = Console.ReadKey().KeyChar;
                if (ans == 'Y' || ans == 'y')
                {
                    _rooms.AddRoom(newRoom);
                }
                 
                break;
            }
        }

        private void CheckOutGuests()
        {
            var day = DateOnly.FromDateTime(DateTime.Today);

            var guestsLeavingToday = _guests.Guests.Where(g => g.DepartureDate == day).ToList();

            if (!guestsLeavingToday.Any())
            {
                Console.WriteLine("No guests are scheduled to leave today.");
                return;
            }

            Console.WriteLine("Guests checking out today:");
            foreach (var guest in guestsLeavingToday)
            {
                Console.WriteLine(guest);
            }
        }

        private void SearchGuests()
        {
             
            Console.WriteLine("Search guests by:");

            Console.Write("\nPassport (leave blank - to skip): ");
            string? passport = GetInput();
            if (passport == null) return;
            if (!string.IsNullOrEmpty(passport) && passport == "-" )
            {
                passport = null;
            }

            Console.Write("\nName (leave blank - to skip): ");
            string name = GetInput();
            if (name == null) return;
            if (!string.IsNullOrEmpty(name) && name == "-")
            {
                name = null;
            }


            Console.Write("\nSurname (leave blank - to skip): ");
            string surname = GetInput();
            if (surname == null) return;
            if (!string.IsNullOrEmpty(surname) && surname == "-")
            {
                surname = null;
            }


            var foundGuests = _guests.FindGuests(passport, name, surname);

            if (!foundGuests.Any())
            {
                Console.WriteLine("No guests found matching the criteria.");
            }
            else
            {
                Console.WriteLine("Found guests:");
                foreach (var guest in foundGuests)
                {
                    Console.WriteLine(guest);
                }
            }

             
        }
        private void GenerateRefundReceipt(Guest guest, DateOnly previousDepartureDate, DateOnly newDepartureDate)
        {
            Console.WriteLine($"\nRefund Receipt:");
            Console.WriteLine($"Guest: {guest.Name} {guest.Surname}");
            Console.WriteLine($"Passport: {guest.PassportNumber}");
            Console.WriteLine($"Room Number: {guest.RoomNumber}");
            Console.WriteLine($"Previous Departure Date: {previousDepartureDate}");
            Console.WriteLine($"New Departure Date: {newDepartureDate}");

            var room = _rooms.GetRoom(guest.RoomNumber);
            double pricePerDay;
            if (room.Class == 1)
            {
                pricePerDay = 500;
            }
            else if (room.Class == 2)
            {
                pricePerDay = 300;
            }
            else
            {
                pricePerDay = 150;
            }

            int refundedDays = (previousDepartureDate.ToDateTime(TimeOnly.MinValue) - newDepartureDate.ToDateTime(TimeOnly.MinValue)).Days;
            double refundAmount = pricePerDay * refundedDays * room.Capacity;

            Console.WriteLine($"Refunded Days: {refundedDays}");
            Console.WriteLine($"Refund Amount: {refundAmount}");

        }

        private void EarlyCheckout()
        {
            Guest? guest;
            while (true)
            {
                Console.WriteLine("Enter passport number of the guest checking out early:");
                var passport = GetInput();
                if (passport == null) return;
                if (string.IsNullOrEmpty(passport))
                {
                    Console.WriteLine("Passport number cannot be empty.");
                    continue;
                }
                guest = _guests.Guests.FirstOrDefault(g => g.PassportNumber == passport);
                if (guest == null)
                {
                    Console.WriteLine("Guest with this passport does not exist.");
                    continue;
                }

                break;
            }

            DateOnly newDepartureDate;
            while (true)
            {
                Console.Write("\nEnter new departure date (YYYY-MM-DD): ");
                var input = GetInput();
                if (input == null) return;
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Departure day cannot be empty.");
                    continue;
                }
                if (DateOnly.TryParse(input, out newDepartureDate))
                {
                   
                    if (newDepartureDate <= guest.ArrivalDate)
                    {
                        Console.WriteLine("New departure date cannot be earlier than arrival date.");
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                   
                    Console.WriteLine("Invalid date format.");
                }
            }

            var previousDepartureDate = guest.DepartureDate;
            guest.DepartureDate = newDepartureDate;
            _guests.UpdateGuest(guest);

            GenerateRefundReceipt(guest, previousDepartureDate, newDepartureDate);
            Console.WriteLine($"Guest {guest.Name} {guest.Surname} checked out early. New departure date: {guest.DepartureDate}");
             
        }

        private void GenerateDelayReceipt(Guest guest, DateOnly previousDepartureDate, DateOnly newDepartureDate)
        {
            Console.WriteLine($"\nDelay Receipt:");
            Console.WriteLine($"Guest: {guest.Name} {guest.Surname}");
            Console.WriteLine($"Passport: {guest.PassportNumber}");
            Console.WriteLine($"Room Number: {guest.RoomNumber}");
            Console.WriteLine($"Previous Departure Date: {previousDepartureDate}");
            Console.WriteLine($"New Departure Date: {newDepartureDate}");

            var room = _rooms.GetRoom(guest.RoomNumber);
            double pricePerDay;
            if (room.Class == 1)
            {
                pricePerDay = 500;
            }
            else if (room.Class == 2)
            {
                pricePerDay = 300;
            }
            else
            {
                pricePerDay = 150;
            }

            int additionalDays = (newDepartureDate.ToDateTime(TimeOnly.MinValue) - previousDepartureDate.ToDateTime(TimeOnly.MinValue)).Days;
            double additionalCost = pricePerDay * additionalDays * room.Capacity;

            Console.WriteLine($"Additional Days: {additionalDays}");
            Console.WriteLine($"Additional Cost: {additionalCost}");

        }
        private void DelayCheckout()
        {
            Guest? guest;
            while (true)
            {
                Console.WriteLine("Enter passport number of the guest for delaying checkout:");
                var passport = GetInput();
                if (passport == null) return;
                if (string.IsNullOrEmpty(passport))
                {
                    Console.WriteLine("Passport number cannot be empty.");
                    continue;
                }
                guest = _guests.Guests.FirstOrDefault(g => g.PassportNumber == passport);
                if (guest == null)
                {
                    Console.WriteLine("Guest with this passport does not exist.");
                    continue;
                }

                break;
            }
            DateOnly newDepartureDate;
            while (true)
            {
                Console.Write("\nEnter new departure date (YYYY-MM-DD): ");
                var input = GetInput();
                if (input == null) return;
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Departure date cannot be empty.");
                    continue;
                }
                if (DateOnly.TryParse(input, out newDepartureDate))
                {
                   
                    if (newDepartureDate <= guest.DepartureDate)
                    {
                        Console.WriteLine("New departure date must be later than the current departure date.");
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                   
                    Console.WriteLine("Invalid date format.");
                }
            }

            var previousDepartureDate = guest.DepartureDate;
            guest.DepartureDate = newDepartureDate;
            _guests.UpdateGuest(guest);

            GenerateDelayReceipt(guest, previousDepartureDate, newDepartureDate);
            Console.WriteLine($"Guest {guest.Name} {guest.Surname} checkout delayed. New departure date: {guest.DepartureDate}");
             
        }

        private void UpdateGuestInformation()
        {
            Guest? guest;
            while (true)
            {
                Console.WriteLine("Enter passport number of the guest to update:");
                var passport = GetInput();
                if (passport == null) return;
                if (string.IsNullOrEmpty(passport))
                {
                    Console.WriteLine("Passport number cannot be empty.");
                    continue;
                }
                guest = _guests.Guests.FirstOrDefault(g => g.PassportNumber == passport);
                if (guest == null)
                {
                    Console.WriteLine("Guest with this passport does not exist.");
                    continue;
                }

                break;
            }

            Console.WriteLine("\nEnter new name (leave blank with - to keep current):");
            string name = GetInput();
            if (name == null) return;

            if (!string.IsNullOrEmpty(name) && name!="-")
            {
                guest.Name = name;
            }

            Console.WriteLine("\nEnter new surname  (leave blank with - to keep current):");
            string surname = GetInput();
            if (surname == null) return;

            if (!string.IsNullOrEmpty(surname) && surname != "-")
            {
                guest.Surname = surname;
            }

            _guests.UpdateGuest(guest);

            Console.WriteLine("Guest information updated successfully.");
             
        }

        private void UpdateRoomInformation()
        {
            Room? room;
            while (true)
            {
                Console.WriteLine("Enter room number of the room to update:");
                var input = GetInput();
                if (input == null) return;
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Room number cannot be empty.");
                    continue;
                }
                if (int.TryParse(input, out int roomNumber))
                {

                    room = _rooms.GetRoom(roomNumber);
                    if (room == null)
                    {
                        Console.WriteLine("Room with this number does not exist. Please try again.");
                    }
                    else if (room.IsOccupied)
                    {
                        Console.WriteLine("Room is currently occupied and cannot be updated. Please try again.");
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                   
                    Console.WriteLine("Invalid room number format. Please try again.");
                }
            }

            Console.WriteLine("\nEnter new room class (leave blank with - to keep current):");
            while (true)
            {
                string roomClassInput = GetInput();
                if (roomClassInput == null) return;

                if (string.IsNullOrEmpty(roomClassInput) || roomClassInput == "-")
                {
                    break;
                }
                else if (int.TryParse(roomClassInput, out int roomClass) && roomClass >= 1 && roomClass <= 3)
                {
                    room.Class = roomClass;
                    break;
                }
                else
                {
                    Console.WriteLine("Room class must be between 1 and 3. Please try again.");
                }
            }

            while (true)
            {
                Console.WriteLine("\nEnter new room capacity  (leave blank with - to keep current):");
                string capacityInput = GetInput();
                if (capacityInput == null) return;

                if (string.IsNullOrEmpty(capacityInput) || capacityInput == "-")
                {
                    break;
                }
                else if (int.TryParse(capacityInput, out int capacity) && capacity > 0)
                {
                    room.Capacity = capacity;
                    break;
                }
                else
                {
                    Console.WriteLine("Capacity must be a positive integer. Please try again.");
                }
            }

            _rooms.UpdateRoom(room);

            Console.WriteLine("Room information updated successfully.");
             
        }

        private void ManageServices()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("\nService Management");
                Console.WriteLine("1. View all services");
                Console.WriteLine("2. Add new service");
                Console.WriteLine("3. Remove service");
                Console.WriteLine("0. Back to main menu");
                Console.Write("Choose an option: ");
                var choice = GetInput();
                if (choice == null) return;

                switch (choice)
                {
                    case "1":
                        ViewAllServices();
                        break;
                    case "2":
                        AddNewService();
                        break;
                    case "3":
                        RemoveService();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private void ViewAllServices()
        {
            var services = _services.Services;
            if (!services.Any())
            {
                Console.WriteLine("No services available.");
                return;
            }

            Console.WriteLine("Available services:");
            foreach (var service in services)
            {
                Console.WriteLine(service);
            }
        }

        private void AddNewService()
        {
            string name;
            while (true)
            {
                Console.Write("\nEnter service name: ");
                name = GetInput();
                if (name == null) return;

                if (string.IsNullOrEmpty(name))
                {
                    Console.WriteLine("Service name cannot be empty.");
                    continue;
                }
                if(_services.Services.Any(e=>e.Name == name))
                {
                    Console.WriteLine("Service with this name already exist.");
                    continue;
                }
                break;
            }

            double price;
            while (true)
            {
                Console.Write("Enter service price: ");
                var input = GetInput();
                if (input == null) return;
                if (string.IsNullOrEmpty(name))
                {
                    Console.WriteLine("Service price cannot be empty.");
                    continue;
                }
                if (!double.TryParse(input, out price) || price < 0)
                {
                   
                    Console.WriteLine("Invalid price. Please enter a positive number.");

                }
                break;
            }

            var newService = new Service(name, price);
            _services.AddService(newService);
            Console.WriteLine("Service added successfully.");
        }

        private void RemoveService()
        {

            Service? service;
            while (true)
            {
                Console.Write("\nEnter service name to remove:"); 
                var name = GetInput();
                if (name == null) return;
                if (string.IsNullOrEmpty(name))
                {
                    Console.WriteLine("Service name cannot be empty.");
                    continue;
                }
                service = _services.Services.FirstOrDefault(el => el.Name == name);
                if (service == null)
                {
                    Console.WriteLine("Service not found.");
                    continue;
                }

                break;
            }

            _services.RemoveService(service.Name);
            Console.WriteLine("Service removed successfully.");
        }

        private void AddServiceToGuestBill()
        {
            Guest? guest;
            while (true)
            {
                Console.WriteLine("Enter passport number of the guest:");
                var passport = GetInput();
                if (passport == null) return;
                if (string.IsNullOrEmpty(passport))
                {
                    Console.WriteLine("Passport number cannot be empty.");
                    continue;
                }
                guest = _guests.Guests.FirstOrDefault(g => g.PassportNumber == passport);
                if (guest == null)
                {
                    Console.WriteLine("Guest with this passport does not exist.");
                    continue;
                }

                break;
            }

            Service? service;
            while (true)
            {
                Console.Write("\nEnter service name :");
                var name = GetInput();
                if (name == null) return;
                if (string.IsNullOrEmpty(name))
                {
                    Console.WriteLine("Service name cannot be empty.");
                    continue;
                }
                service = _services.Services.FirstOrDefault(el => el.Name == name);
                if (service == null)
                {
                    Console.WriteLine("Service not found.");
                    continue;
                }

                break;
            }

            _services.AddServiceToGuest(guest, service);
           
        }

        private void ViewGuestServices()
        {
            Guest? guest;
            while (true)
            {
                Console.WriteLine("Enter passport number of the guest:");
                var passport = GetInput();
                if (passport == null) return;

                if (string.IsNullOrEmpty(passport))
                {
                    Console.WriteLine("Passport number cannot be empty.");
                    continue;
                }
                guest = _guests.Guests.FirstOrDefault(g => g.PassportNumber == passport);
                if (guest == null)
                {
                    Console.WriteLine("Guest with this passport does not exist.");
                    continue;
                }

                break;
            }
            if ( guest.Services==null || guest.Services.Count == 0)
            {
                Console.WriteLine("This guest doesn't use any services");
                return;
            }

            Console.WriteLine("Services for guest:");
            foreach (var service in guest.Services)
            {
                Console.WriteLine(service.ToString());
            }
        }

        private string? GetInput()
        {
            if ( Console.ReadKey(true).Key == ConsoleKey.Escape)
            {
                Console.WriteLine("return back");
                Console.WriteLine();
                return null;
            }
            var input = Console.ReadLine();

            return input.Trim();
        }

    }
}
