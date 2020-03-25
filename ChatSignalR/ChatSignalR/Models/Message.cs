using System;
using System.ComponentModel.DataAnnotations;

namespace ChatSignalR.Models
{
    public class Message
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public int? UserID { get; set; }
        public virtual User Sender { get; set; }

        public Message()
        {
            Date = DateTime.Now;
        }
    }
}