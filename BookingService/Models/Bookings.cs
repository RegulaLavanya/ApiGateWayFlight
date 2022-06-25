using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookingService.Models
{
    public class Bookings
    {
        public int ID { get; set; }

        public string UserName { get; set; }

        public string EmailId { get; set; }

        public int NoOfSeats { get; set; }

        public string DetailsOfPassenger { get; set; }

        public string MealOption { get; set; }
        public string SeatNUmbers { get; set; }
        public decimal Price { get; set; }

        public string PNR { get; set; }

        public int ScheduleId { get; set; }

        public string FromPlace { get; set; }

        public string ToPlace { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public int AirLineId { get; set; }
        public int FlightId { get; set; }


    }
}
