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
    public class ClientDataController : ApiController
    {
        //This variable is our database access point
        private ApplicationDbContext db = new ApplicationDbContext();
        //This code is mostly scaffolded from the base models and database context
        //New > WebAPIController with Entity Framework Read/Write Actions
        //Choose model "Client"
        //Choose context "Application Data Context"
        //Note: The base scaffolded code needs many improvements for a fully
        //functioning MVP.

        // <summary>
        /// Gets a list or Clients in the database alongside a status code (200 OK).
        /// </summary>
        /// <returns>A list of Clients including their ID, name, email,phone number, and address.</returns>
        /// <example>
        // GET: api/ClientData
        /// </example>
        [ResponseType(typeof(IEnumerable<ClientDto>))]
        public IHttpActionResult GetClients()
        {
            List<Client> Clients = db.Clients.ToList();
            List<ClientDto> ClientDtos = new List<ClientDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var Client in Clients)
            {
                ClientDto NewClient = new ClientDto
                {
                    ClientID = Client.ClientID,
                    ClientFirstName = Client.ClientFirstName,
                    ClientLastName = Client.ClientLastName,
                    ClientPhone = Client.ClientPhone,
                    ClientEmail = Client.ClientEmail,
                    ClientAddress = Client.ClientAddress
                };
                ClientDtos.Add(NewClient);
            }

            return Ok(ClientDtos);
        }
        /// <summary>
        /// Gets a list of pets in the database alongside a status code (200 OK).
        /// </summary>
        /// <param name="id">The input clientid</param>
        /// <returns>A list of pets associated with the client</returns>
        /// <example>
        // GET: api/ClientData/GetPetsForClient
        /// </example>

        [ResponseType(typeof(IEnumerable<PetDto>))]

        public IHttpActionResult GetPetsForClient(int id)
        {
            List<Pet> Pets = db.Pets.Where(p => p.ClientID == id)
                 .ToList();
            List<PetDto> PetDtos = new List<PetDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var Pet in Pets)
            {
                PetDto NewPet = new PetDto
                {
                    PetID = Pet.PetID,
                    PetName = Pet.PetName,
                    PetDatebirth = Pet.PetDatebirth,
                    PetType = Pet.PetType,
                    PetBreed = Pet.PetBreed,
                    PetBio = Pet.PetBio,
                    ClientID = Pet.ClientID
                };
                PetDtos.Add(NewPet);
            }


            return Ok(PetDtos);
        }
        /// <summary>
        /// Gets a list or Bookings in the database alongside a status code (200 OK).
        /// </summary>
        /// <param name="id">The input clientid</param>
        /// <returns>A list of Bookings including their ID, name, and URL.</returns>
        /// <example>
        // GET: api/BookingData/GetBookingsForClient
        /// </example>
        [ResponseType(typeof(IEnumerable<BookingDto>))]
        public IHttpActionResult GetBookingsForClient(int id)
        {
            List<Booking> Bookings = db.Bookings.Where(b => b.ClientID == id)
              .ToList();
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
        /// Finds a particular Client in the database with a 200 status code. If the Team is not found, return 404.
        /// </summary>
        /// <param name="id">The Client id</param>
        /// <returns>Information about the Team, including Team id, bio, first and last name, and teamid</returns>
        // <example>
        // GET: api/ClientData/FindClient/5
        // </example>
        [HttpGet]
        [ResponseType(typeof(ClientDto))]
        public IHttpActionResult FindClient(int id)
        {//Find the data
            Client Client = db.Clients.Find(id);
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
        /// Updates a Client in the database given information about the Client.
        /// </summary>
        /// <param name="id">The Client id</param>
        /// <param name="Team">A Client object. Received as POST data.</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/ClientData/UpdateClient/5
        /// FORM DATA: Client JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateClient(int id, [FromBody] Client Client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Client.ClientID)
            {
                return BadRequest();
            }

            db.Entry(Client).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
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
        /// Adds a Client to the database.
        /// </summary>
        /// <param name="Client">A Client object. Sent as POST form data.</param>
        /// <returns>status code 200 if successful. 400 if unsuccessful</returns>
        /// <example>
        /// POST: api/ClientData/AddClient
        ///  FORM DATA: Client JSON Object
        /// </example>
        [ResponseType(typeof(Client))]
        [HttpPost]
        public IHttpActionResult AddClient([FromBody] Client Client)
        {
            //Will Validate according to data annotations specified on model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Clients.Add(Client);
            db.SaveChanges();

            return Ok(Client.ClientID);
        }

        // <summary>
        /// Deletes a Client in the database
        /// </summary>
        /// <param name="id">The id of the Client to delete.</param>
        /// <returns>200 if successful. 404 if not successful.</returns>
        /// <example>
        /// POST: api/ClientData/DeleteClient/5
        /// </example>
        [HttpPost]
        public IHttpActionResult DeleteClient(int id)
        {
            Client Client = db.Clients.Find(id);
            if (Client == null)
            {
                return NotFound();
            }

            db.Clients.Remove(Client);
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
        /// Finds a Client in the system. Internal use only.
        /// </summary>
        /// <param name="id">The Client id</param>
        /// <returns>TRUE if the Client exists, false otherwise.</returns>
        private bool ClientExists(int id)
        {
            return db.Clients.Count(e => e.ClientID == id) > 0;
        }
    }
}