using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace mypassionproject.Models
{
    public class Pet
    {
        [Key]
        public int PetID { get; set; }
        public string PetName { get; set; }
        public DateTime PetDatebirth { get; set; }
        public string PetType { get; set; }
        public string PetBreed { get; set; }
        public string PetBio { get; set; }
        public bool PetHasPic { get; set; }
        public string PicExtension { get; set; }
        //a pet has one owner
        [ForeignKey("Client")]
        public int ClientID { get; set; }
        public virtual Client Client { get; set; }
    }
    public class PetDto
    {
        public int PetID { get; set; }
        [DisplayName("Name")]
        public string PetName { get; set; }
        [DisplayName("Date of Birth")]
        public DateTime PetDatebirth { get; set; }
        [DisplayName("Type")]
        public string PetType { get; set; }
        [DisplayName("Breed")]
        public string PetBreed { get; set; }
        [DisplayName("Bio")]
        public string PetBio { get; set; }
        public bool PetHasPic { get; set; }
        public string PicExtension { get; set; }
        public int ClientID { get; set; }

    }

}
