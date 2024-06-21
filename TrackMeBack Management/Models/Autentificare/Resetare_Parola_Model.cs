using System.ComponentModel.DataAnnotations;

namespace TrackMeBack_Management.Models.Autentificare
{
    public class Resetare_Parola_Model
    {
        [Key]
        [Required]
        public int _id { get; set; }


        [Display(Name = "Email")]
        [Required(ErrorMessage = "Introduceți Email")]
        [RegularExpression("[A-z]+[\\.][A-z]+[\\@][u][p][t][\\.]ro$", ErrorMessage = "Format : nume.prenume@upt.ro")]
        public string email { get; set; } = string.Empty;


        [Display(Name = "Parola Nouă")]
        [Required(ErrorMessage = "Introduceți noua Parolă")]
        [DataType(DataType.Password)]
        [StringLength(25, MinimumLength = 10, ErrorMessage = "Parolă invalidă!")]
        public string parola_nou { get; set; } = string.Empty;


        [Display(Name = "Cod")]
        [Required(ErrorMessage = "Introduceți Codul")]
        [RegularExpression("[A-Z0-9]{10}", ErrorMessage = "Cod invalid!")]
        public string cod_reset { get; set; } = "-";
    }
}
