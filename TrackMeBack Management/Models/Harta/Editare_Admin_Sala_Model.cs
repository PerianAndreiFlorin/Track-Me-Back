using System.ComponentModel.DataAnnotations;

namespace TrackMeBack_Management.Models.Harta
{
    public class Editare_Admin_Sala_Model
    {
        //  Structura unei săli.
        [Display(Name = "Clădire")]
        [Required(ErrorMessage = "Introduceți numele Clădirii!")]
        [RegularExpression("ELECTRO|MEC|SPM|ASPC", ErrorMessage = "Numele conține doar litere!")]
        public string cladire { get; set; } = string.Empty;


        //  Corpul cladirii este optional.
        [Display(Name = "Corpul Clădirii")]
        [RegularExpression("[A-Z]", ErrorMessage = "Corpul conține doar o litere!")]
        public string? corp { get; set; } = string.Empty;


        //  Indexul va fi folosit si pentru locatiile speciale: Secretariat / Decanat / Lift.
        [Display(Name = "Indexul Sălii")]
        [Required(ErrorMessage = "Introduceți indexul Sălii!")]
        public string index { get; set; } = string.Empty;


        //  Selecția acțiunii
        [Display(Name = "Acțiune")]
        [Required]
        public string actiune { get; set; } = string.Empty;

    }
}
