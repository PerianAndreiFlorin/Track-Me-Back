using System.ComponentModel.DataAnnotations;

namespace TrackMeBack_Management.Models.Utilizator
{
    public class Editare_Profesor_Model
    {

        public string? marca { get; set; } = string.Empty;


        [Display(Name = "Nume")]
        [RegularExpression("[A-Z]{1}[a-z]*", ErrorMessage = "Nume incorect!")]
        public string? nume { get; set; } = string.Empty;


        [Display(Name = "Prenume")]
        [RegularExpression("[A-Z][a-z]*[-][A-Z][a-z]*|[A-Z][a-z]* [A-Z][a-z]*|[A-Z][a-z]*", ErrorMessage = "Prenume incorect!")]
        public string? prenume { get; set; } = string.Empty;

        [Display(Name = "Facultatea")]
        [RegularExpression("AC|CT|IEE|ETcTI|ARH|MPT|CIIM", ErrorMessage = "Facultăți: AC | CT | IEE | ETcTI | ARH | MPT | CIIM")]
        public string? facultate { get; set; } = string.Empty;

        [Display(Name = "Parola")]
        [DataType(DataType.Password)]
        [StringLength(25, MinimumLength = 10, ErrorMessage = "Parolă invalidă!")]
        public string? parola { get; set; } = string.Empty;

        //  Selecția acțiunii
        [Display(Name = "Acțiune")]
        [Required]
        public string actiune { get; set; } = string.Empty;
    }
}
