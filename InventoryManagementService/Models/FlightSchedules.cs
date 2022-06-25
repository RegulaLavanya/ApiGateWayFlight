using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementService.Models
{
    public class FlightSchedules
    {
        public int ID { get; set; }

        public int FlightId { get; set; }

        [ForeignKey("FlightId")]
        public Flights Flights { get; set; }

        public int AirLineId { get; set; }
        [ForeignKey("AirLineId")]
        public Airlines Airlines { get; set; }

        public string FromPlace { get; set; }

        public string ToPlace { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }
        public int TotalSeats { get; set; }

        public int BusinessClassSeats { get; set; }

        public int NonBusinessClassSeats { get; set; }

        public decimal BusinessTicketCost { get; set; }

        public decimal NonBusinessTicketCost { get; set; }

        public string MealOption { get; set; }

        public int IsActive { get; set; }
    }
}
