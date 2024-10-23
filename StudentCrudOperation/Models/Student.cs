using System.ComponentModel.DataAnnotations.Schema;

namespace StudentCrudOperation.Models
{
    public class Student
    {
        public int Id { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int STD { get; set; }
        public DateTime DOB { get; set; }
        public DateTime CreateDate { get; set; }
        public string? ImageFile { get; set; }

        [NotMapped]
        public IFormFile? ImageUpload { get; set; }

    }
}
