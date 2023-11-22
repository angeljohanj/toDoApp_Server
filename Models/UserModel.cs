using System.ComponentModel.DataAnnotations;

namespace toDoApp_Server.Models
{
    public class UserModel
    {
        public int? id { get; set; }
        [Required]
        public string? username { get; set; }
        [Required]
        public string? password { get; set; }
    }
}
