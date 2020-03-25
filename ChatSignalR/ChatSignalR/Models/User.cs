using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ChatSignalR.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string Nickname { get; set; }
        
        public int? RoleId { get; set; } = 2;
        public Role Role { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}
