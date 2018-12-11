using SQLite;
using System;
using System.ComponentModel.DataAnnotations;

namespace TaskProject.Models
{
    [Table("Items")]
    public class Item
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Text { get; set; } //Title
        public string Company { get; set; }
        public string Description { get; set; }
        public string DeadLine { get; set; }
        public int NbOfParticipants { get; set; }
        public string Participants { get; set; }
        public int Priority { get; set; } //sort by priority
        public bool Completed { get; set; }
    }
}