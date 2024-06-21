using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using TrackMeBack_Management.Clase_Utilitare.Management;
using TrackMeBack_Management.Clase_Utilitare.Servicii;
using TrackMeBack_Management.Models.Autentificare;

namespace TrackMeBack_Management.Models.Harta
{
    public class Sala_Model
    {
        [Key]
        [Required]
        public BsonObjectId _id { get; set; }


        [Required]
        [RegularExpression("[A-Z]*", ErrorMessage = "Numele conține doar litere!")]
        public string cladire { get; set; } = string.Empty;


        [Required]
        [RegularExpression("[A-Z]", ErrorMessage = "Corpul conține doar o litere!")]
        public string corp { get; set; } = string.Empty;


        [Required]
        public string index { get; set; } = string.Empty;


        //  Stocarea codului x64 pentru imaginea QR.
        [Required]
        public string cod_QR { get; set; } = string.Empty;


        public Sala_Model(string cladire, string corp, string index)
        {
            this.cladire = cladire;
            if (corp == null)
            {
                this.corp = "-";
            }
            else
            {
                this.corp = corp;
            }
            this.index = index;
            cod_QR = Servicii_QR.Generare_QR(cladire,corp,index);
        }
    }
}
