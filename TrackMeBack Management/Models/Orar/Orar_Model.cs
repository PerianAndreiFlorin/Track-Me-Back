using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Reflection.Emit;
using System.Runtime.InteropServices.ObjectiveC;
using TrackMeBack_Management.Clase_Utilitare.Management;

namespace TrackMeBack_Management.Models.Orar
{
    public class Orar_Model
    {
        [BsonId]
        ObjectId _id { get; set; }

        public string ziua {  get; set; } = string.Empty;

        public string ora_start {  get; set; } = string.Empty;   

        public string materia { get; set; } = string.Empty;

        public string specializare {  get; set; } = string.Empty;

        public int anul {  get; set; }

        public int grupa { get; set; }

        public string sala { get; set; }= string.Empty;

        public Orar_Model(string materia, string sala, string ziua, string ora_start, int grupa)
        {
            this.sala = sala;
            this.materia = materia;
            this.ziua = ziua.ToUpper();
            this.ora_start = ora_start;
            this.grupa = grupa;
            specializare = Return_Specializare(materia);
            anul = Return_Anul(materia);
        }

        private static string Tabel_Materie(string materia)
        {
            var conexiune = Management_MongoDB.ConexiuneBD();
            var baza_date = conexiune.GetDatabase("Discipline");

            //  Preluarea fiecărei colecții.
            var lista_colectii = baza_date.ListCollectionNames().ToList();
            foreach(var colectie in lista_colectii)
            {
                if (!colectie.Contains("DISCIPLINE") && !colectie.Contains("ORAR"))
                {
                    var colectie_bd = baza_date.GetCollection<BsonDocument>(colectie);
                    var filtru = Builders<BsonDocument>.Filter.AnyEq("materie", materia);

                    var rezultat = colectie_bd.Find(filtru).FirstOrDefault();
                    if (rezultat != null)
                    {
                        return colectie;
                    }
                }
            }
            return null;
        }

        private static string Return_Specializare(string materia)
        {
            var conexiune = Management_MongoDB.ConexiuneBD();
            var colectie = conexiune.GetDatabase("Discipline").GetCollection<BsonDocument>(Tabel_Materie(materia));

            var filtru = Builders<BsonDocument>.Filter.AnyEq("materie", materia);
            var rezultat = colectie.Find(filtru).FirstOrDefault();
            if (rezultat != null)
            {
                return rezultat.GetValue("specializare",defaultValue :"-").AsString;
            }else return "-";
        }

        private static int Return_Anul(string materia)
        {
            var conexiune = Management_MongoDB.ConexiuneBD();
            var colectie = conexiune.GetDatabase("Discipline").GetCollection<BsonDocument>(Tabel_Materie(materia));

            var filtru = Builders<BsonDocument>.Filter.AnyEq("materie", materia);
            var rezultat = colectie.Find(filtru).FirstOrDefault();
            if (rezultat != null)
            {
                return rezultat.GetValue("an", defaultValue: "-").AsInt32;
            }
            else return 0;
        }
    }
}
