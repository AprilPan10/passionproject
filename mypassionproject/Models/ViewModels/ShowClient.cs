using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mypassionproject.Models.ViewModels
{
    public class ShowClient
    {
        //Information about the client
        public ClientDto client { get; set; }

        //Information about all pets on that client
        public IEnumerable<PetDto> clientpets { get; set; }

        //Information about all bookings for that client
        public IEnumerable<BookingDto> clientbookings { get; set; }
    }
}