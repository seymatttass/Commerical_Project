using System.ComponentModel.DataAnnotations;

namespace Users.API.DTOS.Users
{
    public class UpdateUserDTO
    {
        [Required]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Username { get; set; }

        [Required, MaxLength(50)]
        public string Password { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required, MaxLength(50)]
        public string Surname { get; set; }

        [Required, MaxLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        public DateTime? Birthdate { get; set; }

        [MaxLength(15)]
        public string TelNo { get; set; }
    }
}
