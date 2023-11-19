using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace toDoApp_Server.Models
{
    public class toDoAppModel
    {
        public int id  { get; set; }

        [Required]
        public string? task { get; set; }
        [Required]
        public DateTime expDate { get; set; }
        [Required]
        public string? notes { get; set; }

    }
}
