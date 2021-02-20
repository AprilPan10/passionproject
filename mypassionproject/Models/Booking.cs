using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mypassionproject.Models
{
    public class Booking
    {
        [Key]
        public int BookingID { get; set; }
        public int BookingPrice { get; set; }
        public string BookingDes { get; set; }
        public DateTime BookingDate { get; set; }
        //a booking have one client
        [ForeignKey("Client")]
        public int ClientID { get; set; }
        public virtual Client Client { get; set; }
    }
    public class BookingDto
    {
        public int BookingID { get; set; }
        [DisplayName("Price")]
        public int BookingPrice { get; set; }
        [DisplayName("Description")]
        public string BookingDes { get; set; }
        [DisplayName("Booking Date")]
        public DateTime BookingDate { get; set; }
        public int ClientID { get; set; }

    }
}