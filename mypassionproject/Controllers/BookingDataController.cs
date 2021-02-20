using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using mypassionproject.Models;
using System.Diagnostics;

namespace mypassionproject.Controllers
{
    public class BookingDataController : ApiController
    {
        //This variable is our database access point
        private ApplicationDbContext db = new ApplicationDbContext();

        //This code is mostly scaffolded from the base models and database context
        //New > WebAPIController with Entity Framework Read/Write Actions
        //Choose model "Booking"
        //Choose context "Application Data Context"
        //Note: The base scaffolded code needs many improvements for a fully
        //functioning MVP.

        /// <summary>
        /// Gets a list or bookings in the database alongside a status code (200 OK).
        /// </summary>
        /// <returns>A list of booking, including booking id, description, date, and clientid</returns>
        /// <example>
        /// GET: api/BookingData/GetBookings
        /// </example>
        [ResponseType(typeof(IEnumerable<BookingDto>))]
        public IHttpActionResult GetBookings()
        {
            List<Booking> Bookings = db.Bookings.ToList();
            List<BookingDto> BookingDtos = new List<BookingDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var Booking in Bookings)
            {
                BookingDto NewBooking = new BookingDto
                {
                    BookingID = Booking.BookingID,
                    BookingPrice = Booking.BookingPrice,
                    BookingDes = Booking.BookingDes,
                    BookingDate = Booking.BookingDate,
                    ClientID = Booking.ClientID
                };
                BookingDtos.Add(NewBooking);
            }

            return Ok(BookingDtos);
        }

        /// <summary>
        /// Finds a particular booking in the database with a 200 status code. If the player is not found, return 404.
        /// </summary>
        /// <param name="id">The booking id</param>
        /// <returns>Information about the booking, including booking id, description, date, and clientid</returns>
        // <example>
        // GET: api/BookingData/FindBooking/5
        // </example>
        [HttpGet]
        [ResponseType(typeof(BookingDto))]
        public IHttpActionResult FindBooking(int id)
        {
            //Find the data
            Booking Booking = db.Bookings.Find(id);
            //if not found, return 404 status code.
            if (Booking == null)
            {
                return NotFound();
            }

            //put into a 'friendly object format'
            BookingDto BookingDto = new BookingDto
            {
                BookingID = Booking.BookingID,
                BookingPrice = Booking.BookingPrice,
                BookingDes = Booking.BookingDes,
                BookingDate = Booking.BookingDate,
                ClientID = Booking.ClientID
            };


            //pass along data as 200 status code OK response
            return Ok(BookingDto);
        }

        /// <summary>
        /// Finds a particular Client in the database given a booking id with a 200 status code. If the Client is not found, return 404.
        /// </summary>
        /// <param name="id">The booking id</param>
        /// <returns>Information about the booking, including booking id, description, date, and clientid</returns>
        // <example>
        // GET: api/ClientData/FindClientForBooking/5
        // </example>
        [HttpGet]
        [ResponseType(typeof(ClientDto))]
        public IHttpActionResult FindClientForBooking(int id)
        {
            //Finds the first clients which has any bookings
            //that match the input bookingid
            Client Client = db.Clients
                .Where(c => c.Bookings.Any(b => b.BookingID == id))
                .FirstOrDefault();
            //if not found, return 404 status code.
            if (Client == null)
            {
                return NotFound();
            }

            //put into a 'friendly object format'
            ClientDto ClientDto = new ClientDto
            {
                ClientID = Client.ClientID,
                ClientFirstName = Client.ClientFirstName,
                ClientLastName = Client.ClientLastName,
                ClientPhone = Client.ClientPhone,
                ClientEmail = Client.ClientEmail,
                ClientAddress = Client.ClientAddress
            };


            //pass along data as 200 status code OK response
            return Ok(ClientDto);
        }

        /// <summary>
        /// Updates a booking in the database given information about the booking.
        /// </summary>
        /// <param name="id">The booking id</param>
        /// <param name="booking">A booking object. Received as POST data.</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/BookingData/UpdateBooking/5
        /// FORM DATA: Booking JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateBooking(int id, [FromBody] Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != booking.BookingID)
            {
                return BadRequest();
            }

            db.Entry(booking).State = EntityState.Modified;

            try
            {
                db.SaveChanges();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds a booking to the database.
        /// </summary>
        /// <param name="booking">A booking object. Sent as POST form data.</param>
        /// <returns>status code 200 if successful. 400 if unsuccessful</returns>
        /// <example>
        /// POST: api/BookingData/AddBooking
        ///  FORM DATA: Booking JSON Object
        /// </example>
        [ResponseType(typeof(Booking))]
        [HttpPost]
        public IHttpActionResult AddBooking([FromBody] Booking booking)
        {
            //Will Validate according to data annotations specified on model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Bookings.Add(booking);
            db.SaveChanges();

            return Ok(booking.BookingID);
        }
        /// <summary>
        /// Deletes a booking in the database
        /// </summary>
        /// <param name="id">The id of the booking to delete.</param>
        /// <returns>200 if successful. 404 if not successful.</returns>
        /// <example>
        /// POST: api/BookingData/DeleteBooking/5
        /// </example>
        [HttpPost]
        public IHttpActionResult DeleteBooking(int id)
        {
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return NotFound();
            }

            db.Bookings.Remove(booking);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        /// <summary>
        /// Finds a booking in the system. Internal use only.
        /// </summary>
        /// <param name="id">The booking id</param>
        /// <returns>TRUE if the booking exists, false otherwise.</returns>
        private bool BookingExists(int id)
        {
            return db.Bookings.Count(e => e.BookingID == id) > 0;
        }
    }
}