using BookingService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {

        public AppDbContext _context;
       // private readonly IMessageProducer _messageProducer;
        public BookingController(AppDbContext context)
            //, IMessageProducer messageProducer)
        {
            _context = context;
           // _messageProducer = messageProducer;
        }

        /// <summary>
        /// Booking Flight
        /// </summary>
        /// <param name="value">Booking details</param>
        /// <returns>Booking confirmation with PNR</returns>
        [HttpPost("api/v1.0/flight/booking")]
        public ActionResult<Bookings> BookFlight(Bookings value)
        {
            try
            {
                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                var stringChars = new char[8];
                var random = new Random();

                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }

                var finalString = new String(stringChars);

                var bookData = new Bookings();
                bookData.UserName = value.UserName;
                bookData.EmailId = value.EmailId;
                bookData.NoOfSeats = value.NoOfSeats;
                bookData.DetailsOfPassenger = value.DetailsOfPassenger;
                bookData.MealOption = value.MealOption;
                bookData.SeatNUmbers = value.SeatNUmbers;
                bookData.Price = value.Price;
                bookData.PNR = finalString;
                bookData.ScheduleId = value.ScheduleId;
                bookData.FlightName = value.FlightName;
                bookData.AirLineId = value.AirLineId;
               // bookData.FlightId = value.FlightId;
                bookData.FromPlace = value.FromPlace;
                bookData.ToPlace = value.ToPlace;
                bookData.StartDateTime = value.StartDateTime;
                bookData.EndDateTime = value.EndDateTime;

               //var scheduledata = new FlightSchedules();
               //scheduledata = _context.FlightSchedules.Where(x => Int64.Equals(x.ID, scheduleid)).FirstOrDefault();
                _context.Bookings.Add(bookData);
                _context.SaveChanges();
                return Ok( bookData);
                //return Ok(new { bookData, scheduledata });
            }
            catch (Exception ex)
            {
                return BadRequest("Exception in booking ticket" + ex.ToString());
            }
        }

        /// <summary>
        /// Retrieving ticket details based on PNR
        /// </summary>
        /// <param name="pnr"></param>
        /// <returns></returns>
        [HttpGet("api/v1.0/flight/ticket/{pnr}")]
        public ActionResult<Bookings> TrackTicket(string pnr)
        {
            try
            {
                var bookingDetails = _context.Bookings.Where(x => string.Equals(x.PNR, pnr)).FirstOrDefault();
                if (bookingDetails != null)
                    return Ok(bookingDetails);
                else
                    return NotFound("Booking details not found with PNR");
            }
            catch(Exception ex)
            {
                return BadRequest("EXxception in cancel" + ex.ToString());
            }
        }

        /// <summary>
        /// Retrieving booking history based on email
        /// </summary>
        /// <param name="emailid">Email</param>
        /// <returns>List of tickets(booking history)</returns>
        [HttpGet("api/v1.0/booking/history/{emailid}")]
        public async Task<ActionResult<List<Bookings>>> History(string emailid)
        {
            try
            {
                var bookingDetails = _context.Bookings.Where(x => string.Equals(x.EmailId, emailid)).ToList();
                if (bookingDetails != null)
                    return Ok(bookingDetails);
                else
                    return NotFound("Booking history not found with email!");
            }
            catch(Exception ex)
            {
                return BadRequest("Exception in Bookinghistory using email" + ex.ToString());
            }
        }

        /// <summary>
        /// Cancel ticket
        /// </summary>
        /// <param name="pnr">Ticket PNR</param>
        /// <returns></returns>
        [HttpDelete("api/v1.0/booking/cancel/{pnr}")]
        public ActionResult<Bookings> CancelBooking(string pnr)
        {
            try
            {
                //var boo && EF.Functions.DateDiffDay(DateTime.Now, x.StartDateTime) == 1
                var bookingDetails = _context.Bookings.Where(x => 
                string.Equals(x.PNR, pnr) ).FirstOrDefault();
                if (bookingDetails != null)
                {
                    _context.Bookings.Remove(bookingDetails);
                    _context.SaveChanges();
                    return Ok(bookingDetails);
                }
                else
                    return NotFound("PNR not found");
            }
            catch (Exception ex)
            {
                return BadRequest("Exception in CancelBooking" + ex.ToString());
            }

        }
    }

}
