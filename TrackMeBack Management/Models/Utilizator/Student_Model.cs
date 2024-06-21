using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.IdGenerators;
using TrackMeBack_Management.Clase_Utilitare.Management;
using TrackMeBack_Management.Clase_Utilitare.Servicii;

namespace TrackMeBack_Management.Models.Utilizator
{
    public class Student_Model
    {
        [BsonId]
        public ObjectId _id { get; set; }

        public string marca { get; set; } = string.Empty;


        public string nume { get; set; } = string.Empty;


        public string prenume { get; set; } = string.Empty;


        public int grupa { get; set; }


        public int an_studiu { get; set; }


        public string facultate { get; set; } = string.Empty ;


        public string specializare { get; set; } = string.Empty;


        public string email { get; set; } = string.Empty;


        public string parola { get; set; } = string.Empty;

        public Student_Model(string nume, string prenume, int grupa,int an_studiu, string facultate, string specializare, string parola)
        {
            this.nume = nume;
            this.prenume = prenume;
            this.grupa = grupa;
            this.an_studiu = an_studiu;
            this.facultate = facultate;
            this.specializare = specializare;
            email = nume.ToLower()+ '.'+ Retur_Prenume(prenume).ToLower() + "@student.upt.ro";
            this.parola = Servicii_Criptare.Criptare(parola);
            marca = Retur_Marca(specializare,nume);
        }

        private static string Retur_Marca(string specializare, string nume)
        {

                string marca = string.Empty;
                marca += specializare.Substring(0,2).ToUpper();
                marca += nume[0];

            var client = Management_MongoDB.ConexiuneBD();
            var colectie = client.GetDatabase("Application").GetCollection<BsonDocument>("TB_Student");

            int numar_studenti = (int)colectie.Count(new BsonDocument());
            marca += numar_studenti;

            if(marca.Length < 5)
            {
                marca = marca.Insert(3, "0");
            }

                return marca;
            
        }

        private static string Retur_Prenume(string prenume)
        {

            if (prenume.Contains('-'))
            {
                int ultim_index = prenume.IndexOf("-");
                prenume = prenume.Substring(0,ultim_index);
            }
            else if (prenume.Contains(' '))
            {
                int ultim_index = prenume.IndexOf(" ");
                prenume = prenume.Substring(0, ultim_index);
            }
            else { return prenume; }

            return prenume;
        }
    }
}
