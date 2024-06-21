using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using TrackMeBack_Management.Clase_Utilitare.Servicii;

namespace TrackMeBack_Management.Models.Utilizator
{
    public class Profesor_Model
    {
        [BsonId]
        public ObjectId _id {  get; set; }
        

        [Key]
        public string marca { get; set; } = string.Empty;


        [Display(Name = "Nume")]
        [Required(ErrorMessage = "Introduceți Numele")]
        [RegularExpression("[A-Z]{1}[a-z]*", ErrorMessage = "Nume incorect!")]
        public string nume { get; set; } = string.Empty;


        [Display(Name = "Prenume")]
        [Required(ErrorMessage = "Introduceți Prenumele")]
        [RegularExpression("[A-Z][a-z]*[-][A-Z][a-z]*|[A-Z][a-z]* [A-Z][a-z]*|[A-Z][a-z]*", ErrorMessage = "Prenume incorect!")]
        public string prenume { get; set; } = string.Empty;


        [Display(Name = "Email")]
        [Required(ErrorMessage = "Introduceți Email")]
        [RegularExpression("[a-z]+[\\.][a-z]+[\\@]upt[\\.]ro$", ErrorMessage = "Format : nume.prenume@upt.ro")]
        public string email { get; set; } = string.Empty;

        [Display(Name = "Parola")]
        [Required(ErrorMessage = "Introduceți Parola")]
        [DataType(DataType.Password)]
        [StringLength(25, MinimumLength = 10, ErrorMessage = "Parolă invalidă!")]
        public string parola { get; set; } = string.Empty;


        [Display(Name = "Facultatea")]
        [Required(ErrorMessage = "Introduceți Facultatea")]
        [RegularExpression("AC|CT|IEE|ETcTI|ARH|MPT|CIIM", ErrorMessage = "Facultăți: AC | CT | IEE | ETcTI | ARH | MPT | CIIM")]
        public string facultate { get; set; } = string.Empty;

        public Profesor_Model(string nume, string prenume, string parola, string facultate)
        {
            marca = Generare_Marca(nume, prenume, facultate);
            this.nume = nume;
            this.prenume = prenume;
            email = nume.ToLower() + '.' + Prim_Prenume(prenume).ToLower() + "@upt.ro";
            this.parola = Servicii_Criptare.Criptare(parola);
            this.facultate = facultate;
        }

        private static string Generare_Marca(string nume, string prenume, string facultate)
        {
            string marca = string.Empty;
            marca += facultate.ToUpper();
            marca += nume[0];
            marca += Initiale_Prenume(prenume);
            marca += Generare_Cod_Marca();
            return marca;
        }

        private static string Generare_Cod_Marca()
        {
            string cod_resetare = string.Empty;
            Random numar_random = new Random();

            for (int i = 0; i < 5; i++)
            {
                cod_resetare += numar_random.Next(0, 9);
            }
            return cod_resetare;
        }

        private static string Initiale_Prenume(string prenume)
        {
            string initiale = string.Empty;
            if (prenume.Contains('-'))
            {
                initiale += prenume[0];

                int pozitie_separator = prenume.IndexOf('-');
                initiale += prenume[pozitie_separator + 1];
                initiale.ToUpper();

                return initiale;
            }
            else if (prenume.Contains(' '))
            {
                initiale += prenume[0];

                int pozitie_separator = prenume.IndexOf(' ');
                initiale += prenume[pozitie_separator + 1];
                initiale.ToUpper();

                return initiale;
            }
            else
            {
                initiale += prenume[0];
                initiale.ToUpper();
                return initiale;
            }
        }

        private static string Prim_Prenume(string prenume)
        {
            string prim_prenume = string.Empty;
            if (prenume.Contains('-'))
            {
                int pozitie_separator = prenume.IndexOf('-');
                prim_prenume = prenume.Substring(0, pozitie_separator);

                return prim_prenume;
            }
            else if (prenume.Contains(' '))
            {
                int pozitie_separator = prenume.IndexOf(' ');
                prim_prenume = prenume.Substring(0, pozitie_separator);

                return prim_prenume;
            }
            else
            {
                return prenume;
            }
        }

    }
}
