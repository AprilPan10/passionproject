using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mypassionproject.Models.ViewModels
{
    public class ShowPet
    {
        public PetDto pet { get; set; }
        //information about the team the player plays for
        public ClientDto client { get; set; }
    }
}