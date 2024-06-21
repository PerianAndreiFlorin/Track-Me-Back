using System.ComponentModel.DataAnnotations;

namespace TrackMeBack_Management.Models.Autentificare
{
    public class Editare_Admin_Model
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Introduceți Email")]
        [RegularExpression("[A-z]+[\\.][A-z]+[\\@][u][p][t][\\.]ro$", ErrorMessage = "Format : nume.prenume@upt.ro")]
        public string email { get; set; } = string.Empty;


        [Display(Name = "Parola Nouă")]
        [Required(ErrorMessage = "Introduceți noua Parolă")]
        [DataType(DataType.Password)]
        [StringLength(25, MinimumLength = 10, ErrorMessage = "Parolă invalidă!")]
        public string parola_nou { get; set; } = string.Empty;


        //  Selecția acțiunii
        [Display(Name = "Acțiune")]
        [Required]
        public string actiune { get; set; } = string.Empty;
    }
}
