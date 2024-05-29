using System;
using System.ComponentModel.DataAnnotations;

namespace KFHRBackEnd.Models.Entites.Request
{
    public class RegisterRequest
    {

       
       
        public string Email { get; set; }

       
        public string Password { get; set; }

        

        public string Name { get; set; }

        
        public DateTime DOB { get; set; }

        
        public Gender Gender { get; set; }

       
        public string ProfilePicURL { get; set; }

     
       
        public bool IsAdmin { get; set; }
    }
}

