using System.ComponentModel.DataAnnotations;

namespace TrackMeBack_Management.Models.Discipline
{
    public class Editare_Discipline_Model
    {

        //  Denumirea Materiei.
        [Display(Name = "Materia")]
        [Required(ErrorMessage = "Introduceți numele materiei!")]
        [RegularExpression("[A-z ăâîșțĂÂÎȘȚ]*", ErrorMessage = "Materia conține doar litere!")]
        public string materie { get; set; } = string.Empty;


        //  Denumirea Materiei.
        [Display(Name = "Specializarea")]
        [Required(ErrorMessage = "Introduceți numele specializării!")]
        [RegularExpression("[A-z]*", ErrorMessage = "Specializarea conține doar litere!")]
        public string specializare { get; set; } = string.Empty;


        public int an {  get; set; }

        //  Denumirea Materiei.
        [Display(Name = "Semestrul")]
        [Required(ErrorMessage = "Introduceți semestrul!")]
        public int semestru { get; set; }


        //  Lista Profesorilor
        [Display(Name = "Profesor Curs")]
        [Required(ErrorMessage = "Introduceți profesorul!")]
        public string? profesor_curs { get; set; } = string.Empty;


        [Display(Name = "Profesor Laborator")]
        [Required(ErrorMessage = "Introduceți profesorul!")]
        public string? profesor_laborator { get; set; } = string.Empty;


        [Display(Name = "Profesor Laborator 2")]
        [Required(ErrorMessage = "Introduceți profesorul!")]
        public string? profesor_laborator_2 { get; set; }


        //  Selecția acțiunii
        [Display(Name = "Acțiune")]
        [Required]
        public string actiune { get; set; } = string.Empty;
    }
}
