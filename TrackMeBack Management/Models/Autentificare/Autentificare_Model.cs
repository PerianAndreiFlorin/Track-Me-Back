using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TrackMeBack_Management.Models.Autentificare
{
    public class Autentificare_Model
    {
        [Key]
        public int _id { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Introduceți Email")]
        [RegularExpression("[A-z]+[\\.][A-z]+[\\@]upt[\\.]ro$", ErrorMessage = "Format : nume.prenume@upt.ro")]
        public string email { get; set; } = string.Empty;


        [Display(Name = "Parola")]
        [Required(ErrorMessage = "Introduceți Parola")]
        [DataType(DataType.Password)]
        [StringLength(25, MinimumLength = 10, ErrorMessage = "Parolă invalidă!")]
        public string parola { get; set; } = string.Empty;
    }
}
