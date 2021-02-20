using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace mypassionproject.Models
{
    public class Client
    {
        [Key]
        public int ClientID { get; set; }

        public string ClientFirstName { get; set; }
        public string ClientLastName { get; set; }
        public string ClientPhone { get; set; }
        public string ClientEmail { get; set; }
        public string ClientAddress { get; set; }

        //a client have many pets
        public ICollection<Pet> Pets { get; set; }
        //a client have many bookings
        public ICollection<Booking> Bookings { get; set; }
    }
    public class ClientDto
    {
        public int ClientID { get; set; }
        [DisplayName("First Name")]
        public string ClientFirstName { get; set; }
        [DisplayName("Last Name")]
        public string ClientLastName { get; set; }
        [DisplayName("Phone number")]
        public string ClientPhone { get; set; }
        [DisplayName("Email")]
        public string ClientEmail { get; set; }
        [DisplayName("Address")]
        public string ClientAddress { get; set; }

    }
}