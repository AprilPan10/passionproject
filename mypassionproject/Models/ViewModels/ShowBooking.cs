using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mypassionproject.Models.ViewModels
{
    public class ShowBooking
    {
        public BookingDto booking { get; set; }
        //information about the team the player plays for
        public ClientDto client { get; set; }
    }
}