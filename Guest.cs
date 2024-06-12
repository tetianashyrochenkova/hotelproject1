using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cours_Work.Models
{
    [Serializable]
    internal class Guest
    {
        public string PassportNumber { get; set; }
        public string Name {  get; set; }
        public string Surname { get; set; }
        public DateOnly ArrivalDate { get; set; }
        public DateOnly DepartureDate { get; set; }
        public int RoomNumber { get; set; }
        public List<Service> Services { get; set; } = new List<Service>();

        public Guest(string passport, string name, string surname, DateOnly arrival, DateOnly departure, int roomNumb)
        {
            PassportNumber = passport;
            Name = name;
            Surname = surname;
            ArrivalDate = arrival;
            DepartureDate = departure;
            RoomNumber = roomNumb;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("Guest: " + "\n");
            sb.Append("Passport: " + PassportNumber+ "\n");
            sb.Append("Name: " + Name+ "\n");
            sb.Append("Surname: " + Surname + "\n");
            sb.Append("Arrival Date: " + ArrivalDate + "\n"); 
            sb.Append("Departure Date: " + DepartureDate + "\n");
            sb.Append("Room: " + RoomNumber + "\n");

            return sb.ToString();
        }
    }
}
