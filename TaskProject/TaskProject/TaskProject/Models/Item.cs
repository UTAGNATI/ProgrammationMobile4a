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

        public string Text { get; set; }
        public string Description { get; set; }
		public bool Completed { get; set; }
    }
}