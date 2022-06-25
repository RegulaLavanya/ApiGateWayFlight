using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementService.Models
{
    public class Flights
    {
        public int Id { get; set; }

        public string FlightName { get; set; }

        public int AirlineId { get; set; }

        [ForeignKey("AirlineId")]
        public Airlines Airlines { get; set; }
    }
}
