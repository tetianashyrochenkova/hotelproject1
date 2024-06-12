using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cours_Work.Models
{
    [Serializable]
    internal class Room
    {
        public int Number { get; set; }
        [Range(1, 3)]
        public int Class { get; set; }
        public int Capacity { get; set; }
        public bool IsOccupied { get; set; }

        public Room(int roomClass, int number ,int capacity)
        {
            Class = roomClass;
            Number = number;
            Capacity = capacity;
            IsOccupied = false;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("Room: "  + "\n");
            sb.Append("Number: " + Number + "\n");
            sb.Append("Class: " + Class + "\n");
            sb.Append("Capacity: " + Capacity + "\n");
            sb.Append("Occupied: " + IsOccupied + "\n");

            return sb.ToString();
        }
    }
}
