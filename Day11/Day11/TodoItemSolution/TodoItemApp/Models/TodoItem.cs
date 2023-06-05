using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TodoItemApp.Models
{
    public class TodoItem
    {
        public int Id { get; set; }

        [Column("Title", TypeName = "Varchar(100)")]
        public string? Title { get; set; }

        public string TodoDate { get; set; }
        public int IsComplete { get; set; }
    }
}
