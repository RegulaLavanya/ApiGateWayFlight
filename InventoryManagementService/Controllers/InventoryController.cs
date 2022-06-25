using InventoryManagementService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementService.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        public AppDbContext _context;

        public InventoryController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Fetches all airlines data
        /// </summary>
        /// <returns>Airline data</returns>
        [HttpGet("api/v1.0/flight/GetAirlines")]
        public ActionResult<Airlines> GetAirlines()
        {
            try
            {
                var fschedule = _context.Airlines.Where(x => Equals(x.IsActive, 1)).ToList();

                if (fschedule != null)
                    
                    return Ok(fschedule);
                
                else
                    return NotFound("No data!");

            }
            catch (Exception ex)
            {
                return BadRequest("Exception in GetAirlines " + ex);
            }

        }

       // [HttpGet("api/v1.0/flight/GetFlightSchedules")]
        //public ActionResult<Airlines> GetFlightSchedules()
        //{
        //    try
        //    {
        //        // var fschedule = new Airlines();
        //        var fschedule = _context.FlightSchedules.ToList();
        //        var flights = _context.Flights.ToList();

        //        if (fschedule != null)
        //        {
        //            return Ok(new { fschedule, flights });
        //        }
        //        else
        //            return NotFound("No data!");

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest("Exception in GetFlightSchedules " + ex);
        //    }

        //}

        /// <summary>
        /// Gets all schedules of the airlines
        /// </summary>
        /// <returns>List of schedules</returns>
        [HttpGet("api/v1.0/flight/GetSchedule")]
        public ActionResult<List<FlightSchedules>> GetSchedule()
        {
            try
            {
                var result = from flightSchedules in _context.FlightSchedules where flightSchedules.StartDateTime >DateTime.Now
                             join airlines in _context.Airlines on flightSchedules.AirLineId equals airlines.Id
                                                                   //  airlines.IsActive=1
                             join flights in _context.Flights on flightSchedules.FlightId equals flights.Id
                             select new
                             {
                                 FlightScheduleId= flightSchedules.ID,
                                 FlightName= flights.FlightName,
                                 AirLineId = flightSchedules.AirLineId ,
                                 AirlineName = airlines.AirlineName,
                                 FlightId=flightSchedules.FlightId,
                                 FromPlace = flightSchedules.FromPlace,
                                 ToPlace = flightSchedules.ToPlace,
                                 StartDateTime = flightSchedules.StartDateTime,
                                 EndDateTime = flightSchedules.EndDateTime,
                                 BusinessTicketCost = flightSchedules.BusinessTicketCost,
                                 NonBusinessTicketCost = flightSchedules.NonBusinessTicketCost

                             };
                // var fschedule = new FlightSchedules();
             //   var fschedule = _context.FlightSchedules.Where(x => x.StartDateTime >= DateTime.Now ).ToList();

                if (result != null)
                {
                    return Ok(result);
                }
                else
                    return NotFound("no schedules available");

            }
            catch (Exception ex)
            {
                return BadRequest("Exception in GetSchedule" + ex);
            }

        }

        /// <summary>
        /// Blocks the airline
        /// </summary>
        /// <param name="airline">airline info</param>
        /// <returns></returns>
        [HttpPost("api/v1.0/flight/block")]
        public ActionResult<FlightSchedules> Block(Airlines airline)
        {
            try
            {
                var fschedule = _context.Airlines.Where(x => Equals(x.Id, airline.Id)).FirstOrDefault();

                if (fschedule != null)
                {
                    fschedule.IsActive = 0;
                    _context.SaveChanges();
                    return Ok(fschedule);
                }
                else
                    return NotFound("No Airline!");

            }
            catch (Exception ex)
            {
                return BadRequest("Exception in blocking airline" + ex);
            }

        }
       
        /// <summary>
        /// Creates new airline
        /// </summary>
        /// <param name="airline">Airline details</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/v1.0/flight/airline/register")]
        public  ActionResult<Airlines> RegisterAirline([FromBody] Airlines airline)
        {
            try
            {
                var airlineData = new Airlines();

                var result = _context.Airlines.Where(x => string.Equals(x.AirlineName.ToLower(), airline.AirlineName.ToLower())).FirstOrDefault();
                if (result != null)
                    return NotFound("airline exsist please give different name!");
                else
                {

                    airlineData.AirlineName = airline.AirlineName;
                    airlineData.IsActive = 1;

                    _context.Airlines.Add(airlineData);
                     _context.SaveChanges();

                    return Ok(airlineData);
                }

            }
            catch (Exception ex)
            {
                return BadRequest("Exception in register airline method" + ex.ToString());
            }
        }

        /// <summary>
        /// Creates flight for an airline
        /// </summary>
        /// <param name="Flight">Flight details</param>
        /// <returns></returns>
        [HttpPost("api/v1.0/flight/register")]
        public ActionResult<Flights> RegisterFlight([FromBody]Flights Flight)
        {
            try
            {
                var airlineData = new Flights();

                var result = _context.Flights.Where(x => string.Equals(x.FlightName, Flight.FlightName.ToLower())
                                                    && Equals(x.AirlineId, Flight.AirlineId)).FirstOrDefault();
                if (result != null)
                    return NotFound("Flight exsist for airline please give different name!");
                else
                {
                    airlineData.FlightName = Flight.FlightName;
                    airlineData.AirlineId = Flight.AirlineId;
                  
                    _context.Flights.Add(airlineData);
                    _context.SaveChanges();

                    return Ok(airlineData);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Exception in register airline method" + ex.ToString());
            }
        }

        /// <summary>
        /// Adds schedule for flight
        /// </summary>
        /// <param name="schedules">schedule details</param>
        /// <returns>Flight schedule</returns>
        [HttpPost]
        [Route("api/v1.0/flight/airline/inventory/add")]
        public ActionResult<FlightSchedules> AddSchedule([FromBody] FlightSchedules schedules)
        {
            var flightSchedule = new FlightSchedules();
            flightSchedule.FlightId = schedules.FlightId;
            flightSchedule.AirLineId = schedules.AirLineId;
            flightSchedule.FromPlace = schedules.FromPlace;
            flightSchedule.ToPlace = schedules.ToPlace;
            flightSchedule.StartDateTime = schedules.StartDateTime;
            flightSchedule.EndDateTime = schedules.EndDateTime;
            flightSchedule.TotalSeats = schedules.TotalSeats;
            flightSchedule.BusinessClassSeats = schedules.BusinessClassSeats;
            flightSchedule.NonBusinessClassSeats = schedules.NonBusinessClassSeats;
            flightSchedule.BusinessTicketCost = schedules.BusinessTicketCost;
            flightSchedule.NonBusinessTicketCost = schedules.NonBusinessTicketCost;
            flightSchedule.MealOption = schedules.MealOption;
            flightSchedule.IsActive = 1;
            try
            {
                _context.FlightSchedules.Add(flightSchedule);
                _context.SaveChanges();
                return Ok(flightSchedule);
            }
            catch (Exception ex)
            {
                return BadRequest("Exception in Inventory method" + ex.ToString());
            }
        }

        /// <summary>
        /// Retrieves flight schedules based on search data
        /// </summary>
        /// <param name="schedules">search data</param>
        /// <returns>Available Flights</returns>
        [HttpPost("api/v1.0/flight/search")]

        public ActionResult<List<FlightSchedules>> FlightSearch([FromBody] FlightSchedules schedules)
        {
            try
            {
                var searchData = _context.FlightSchedules.Where(x => 
                (x.StartDateTime >= schedules.StartDateTime && x.EndDateTime <= schedules.EndDateTime) &&
                (x.FromPlace.ToLower() == schedules.FromPlace.ToLower())
                && x.ToPlace.ToLower() == schedules.ToPlace.ToLower() && x.IsActive == 1).ToList();
                if (searchData != null && searchData.Count>0)
                    return Ok(searchData);
                else
                    return NotFound("No Flights!");
            }
            catch (Exception ex)
            {
                return BadRequest("Exception in flight search" + ex.ToString());
            }
        }
    }
}
