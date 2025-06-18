using Microsoft.AspNetCore.Identity;

namespace CareerBuilderX.Models
{
    public class Person:IdentityUser
    {
        public string Fname { get; set; }

        public string LName { get; set; }
    }
}
