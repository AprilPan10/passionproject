using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mypassionproject.Models.ViewModels
{
    public class UpdateBooking
    {
        //Information about the booking
        public BookingDto booking { get; set; }
        //Needed for a dropdownlist which presents the pet with a choice of clients to belong to
        
        public IEnumerable<ClientDto> allclients { get; set; }
    }
}