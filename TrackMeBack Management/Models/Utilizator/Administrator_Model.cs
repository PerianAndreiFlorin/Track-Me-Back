using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using TrackMeBack_Management.Clase_Utilitare.Management;
using TrackMeBack_Management.Clase_Utilitare.Servicii;
using TrackMeBack_Management.Models.Autentificare;

namespace TrackMeBack_Management.Models.Utilizator
{
    public class Administrator_Model
    {
        [Key]
        [Required]
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

        public Administrator_Model(string email, string parola)
        {
            this.email = email;
            this.parola = Servicii_Criptare.Criptare(parola);
            _id = Calcul_Id();
        }

        private int Calcul_Id()
        {
            var client = Management_MongoDB.ConexiuneBD();
            var colectie = client.GetDatabase("Application").GetCollection<Administrator_Model>("TB_Administrare");
            return (int)colectie.Count(new BsonDocument())+1;
        }
    }
}
