using System.Collections.Generic;

namespace ChatSignalR.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? OwnerID{ get; set; }
        public User Owner{ get; set; }
    }
}