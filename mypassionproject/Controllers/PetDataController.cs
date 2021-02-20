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
    public class PetDataController : ApiController
    {
        //This variable is our database access point
        private ApplicationDbContext db = new ApplicationDbContext();
        //This code is mostly scaffolded from the base models and database context
        //New > WebAPIController with Entity Framework Read/Write Actions
        //Choose model "Pet"
        //Choose context "Application Data Context"
        //Note: The base scaffolded code needs many improvements for a fully
        //functioning MVP.

        // <summary>
        /// Gets a list or pets in the database alongside a status code (200 OK).
        /// </summary>
        /// <returns>A list of pets including their ID, name, age, bio, type, breed and clientid.</returns>
        /// <example>
        // GET: api/PetData
        /// </example>
        [ResponseType(typeof(IEnumerable<PetDto>))]
        public IHttpActionResult GetPets()
        {
            List<Pet> Pets = db.Pets.ToList();
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
        /// Finds a particular player in the database with a 200 status code. If the player is not found, return 404.
        /// </summary>
        /// <param name="id">The pet id</param>
        /// <returns>Information about the pets including their ID, name, age, bio, type, breed and clientid.</returns>
        // <example>
        // GET: api/PetData/FindPet/5
        // </example>
        [HttpGet]
        [ResponseType(typeof(PetDto))]
        public IHttpActionResult FindPet(int id)
        {
            //Find the data
            Pet Pet = db.Pets.Find(id);
            //if not found, return 404 status code.
            if (Pet == null)
            {
                return NotFound();
            }

            //put into a 'friendly object format'
            PetDto PetDto = new PetDto
            {
                PetID = Pet.PetID,
                PetName = Pet.PetName,
                PetDatebirth = Pet.PetDatebirth,
                PetType = Pet.PetType,
                PetBreed = Pet.PetBreed,
                PetBio = Pet.PetBio,
                ClientID = Pet.ClientID
            };


            //pass along data as 200 status code OK response
            return Ok(PetDto);
        }
        /// <summary>
        /// Finds a particular Client in the database given a pet id with a 200 status code. If the Client is not found, return 404.
        /// </summary>
        /// <param name="id">The pet id</param>
        /// <returns>Information about the pets including their ID, name, age, bio, type, breed and clientid.</returns>
        // <example>
        // GET: api/ClientData/FindClientForPet/5
        // </example>
        [HttpGet]
        [ResponseType(typeof(ClientDto))]
        public IHttpActionResult FindClientForPet(int id)
        {
            //Finds the first client which has any pets
            //that match the input petid
            Client Client = db.Clients
                .Where(c => c.Pets.Any(p => p.PetID == id))
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
        /// Updates a pet in the database given information about the pet.
        /// </summary>
        /// <param name="id">The pet id</param>
        /// <param name="pet">A player object. Received as POST data.</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/PetData/UpdatePet/5
        /// FORM DATA: Player JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdatePet(int id, [FromBody] Pet pet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pet.PetID)
            {
                return BadRequest();
            }

            db.Entry(pet).State = EntityState.Modified;

            try
            {
                db.SaveChanges();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PetExists(id))
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
        /// Adds a pet to the database.
        /// </summary>
        /// <param name="pet">A pet object. Sent as POST form data.</param>
        /// <returns>status code 200 if successful. 400 if unsuccessful</returns>
        /// <example>
        /// POST: api/PetData/AddPet
        ///  FORM DATA: Pet JSON Object
        /// </example>
        [ResponseType(typeof(Pet))]
        [HttpPost]
        public IHttpActionResult AddPet([FromBody] Pet pet)
        {
            //Will Validate according to data annotations specified on model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Pets.Add(pet);
            db.SaveChanges();

            return Ok(pet.PetID);
        }
        /// <summary>
        /// Deletes a pet in the database
        /// </summary>
        /// <param name="id">The id of the pet to delete.</param>
        /// <returns>200 if successful. 404 if not successful.</returns>
        /// <example>
        // DELETE: api/PetData/DeletePet/5
        /// </example>
        [HttpPost]

        public IHttpActionResult DeletePet(int id)
        {
            Pet pet = db.Pets.Find(id);
            if (pet == null)
            {
                return NotFound();
            }

            db.Pets.Remove(pet);
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
        /// Finds a pet in the system. Internal use only.
        /// </summary>
        /// <param name="id">The pet id</param>
        /// <returns>TRUE if the pet exists, false otherwise.</returns>
        private bool PetExists(int id)
        {
            return db.Pets.Count(e => e.PetID == id) > 0;
        }
    }
}