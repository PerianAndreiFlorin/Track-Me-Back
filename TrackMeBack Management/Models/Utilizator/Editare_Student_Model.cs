using System.ComponentModel.DataAnnotations;

namespace TrackMeBack_Management.Models.Utilizator
{
    public class Editare_Student_Model
    {

        public string? marca { get; set; } = string.Empty;

        public string actiune { get; set; } = string.Empty;

        [Display(Name = "Nume")]
        [RegularExpression("[A-Z]{1}[a-z]*", ErrorMessage = "Nume incorect!")]
        public string? nume { get; set; } = string.Empty;


        [Display(Name = "Prenume")]
        [RegularExpression("[A-Z][a-z]*[-][A-Z][a-z]*|[A-Z][a-z]* [A-Z][a-z]*|[A-Z][a-z]*", ErrorMessage = "Prenume incorect!")]
        public string? prenume { get; set; } = string.Empty;


        [Display(Name = "Grupa")]
        [RegularExpression("[1-3]{1}", ErrorMessage = "Grupa trebuie să fie în intervalul 1-3")]
        public int grupa { get; set; }

        [Display(Name = "Facultatea")]
        public string? facultate { get; set; } = string.Empty;

        [Display(Name = "Anul")]
        [RegularExpression("[1-4]{1}", ErrorMessage = "Grupa trebuie să fie în intervalul 1-4")]
        public int an_Studiu { get; set; }


        [Display(Name = "Specializarea")]
        [RegularExpression("INFO|CTI|IS", ErrorMessage = "Specializări: INFO | CTI | IS")]
        public string? specializare { get; set; } = string.Empty;


        public string email { get; set; } = string.Empty;


        [Display(Name = "Parola")]
        [DataType(DataType.Password)]
        [StringLength(25, MinimumLength = 10, ErrorMessage = "Parolă invalidă!")]
        public string? parola { get; set; } = string.Empty;

        
    }
}
