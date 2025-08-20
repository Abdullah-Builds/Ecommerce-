using System.ComponentModel.DataAnnotations;

namespace MyApp.Models
{
    public class LoginModel
    {
        public int Id { get; set; }

        [StringLength(30)]
        public string FirstName { get; set; }

        [StringLength(30)]
        public string LastName { get; set; }

        [StringLength(30)]
        public string Email { get; set; }

        [StringLength(9)]
        public string Password { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }


        [StringLength(12)]
        public string IP { get; set; }
    }
}
