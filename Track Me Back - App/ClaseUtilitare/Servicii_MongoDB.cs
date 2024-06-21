using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackMeBack_Management.Clase_Utilitare.Management;

namespace TrackMeBack_Mobile.ClaseUtilitare
{
    internal static class Servicii_MongoDB
    {
        public static class Autentificare
        {
            public static bool Acces(string email, string parola)
            {
                var client = ConexiuneBD();
                var colectie = client.GetDatabase("Application").GetCollection<BsonDocument>(Tabel_Utilizator(email));
                var filtru = Builders<BsonDocument>.Filter.AnyEq("email", email);
                var rezultat = colectie.Find(filtru).FirstOrDefault();

                string hash_parola = Servicii_Criptare.Criptare(parola);
                if (rezultat != null && rezultat.ContainsValue(hash_parola))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        //Listare
        public static List<Orar_Model> Listare_Orar_Profesor()
        {
            List<Orar_Model> lista_orar = new List<Orar_Model>();
            List<string> discipline_profesor = Listare_Discipline_Profesor();
            //  Conectare MongoBD.
            var conexiune = ConexiuneBD();
            var baza_date = conexiune.GetDatabase("Discipline");

            var lista_colectii = baza_date.ListCollectionNames().ToList();
            foreach (var colectie in lista_colectii)
            {
                if (colectie.Contains("TB_ORAR"))
                {
                    var colectie_bd = baza_date.GetCollection<Orar_Model>(colectie).Find(new BsonDocument()).ToList();

                    foreach (var orar in colectie_bd)
                    {
                        foreach (string disciplina in discipline_profesor) 
                        {
                            if (orar.materia == disciplina)
                            {
                                lista_orar.Add(orar);
                            }
                        }
                    }
                }
            }
            return lista_orar;
        }


        public static List<string> Listare_Discipline_Profesor()
        {
            List<string> lista_discipline = new List<string>();

            var conexiune = ConexiuneBD();
            var colectie = conexiune.GetDatabase("Discipline").GetCollection<BsonDocument>("TB_INFO");
            var rezultat = colectie.Find(new BsonDocument()).ToList();

            foreach (var disciplina in rezultat)
            {

                if(disciplina.ContainsValue(Management_UtilizatorCurent.Get_Marca_Profesor()))
                {
                    lista_discipline.Add(disciplina.GetValue("materie").AsString);
                }
                
            }
            return lista_discipline;
        }

        public static List<string> Listare_Discipline_Student()
        {
            List<string> lista_discipline = new List<string>();
            string specializare = string.Empty;
            int anul = 0, grupa = 0;

            Info_Student(out specializare, out anul, out grupa);

            var conexiune = ConexiuneBD();
            string tabel = "TB_" + specializare;
            var colectie = conexiune.GetDatabase("Discipline").GetCollection<BsonDocument>(tabel);
            var filtru_an = Builders<BsonDocument>.Filter.AnyEq("an", anul);
            var rezultat = colectie.Find(filtru_an).ToList();

            foreach (var disciplina in rezultat)
            {

                    lista_discipline.Add(disciplina.GetValue("materie").AsString);

            }
            return lista_discipline;
        }

        public static void Info_Student(out string specializarea, out int anul, out int grupa)
        {
            string marca = Management_UtilizatorCurent.Get_Marca_Student();

            var conexiune = ConexiuneBD();
            var colectie = conexiune.GetDatabase("Application").GetCollection<BsonDocument>("TB_Student");
            var filtru_marca = Builders<BsonDocument>.Filter.Eq("marca", marca);

            var rezultat = colectie.Find(filtru_marca).FirstOrDefault();

            anul = rezultat.GetValue("an_studiu").AsInt32;
            grupa = rezultat.GetValue("grupa").AsInt32;
            specializarea = rezultat.GetValue("specializare").AsString;
        }

        // Funcții Ajutătoare
        public static MongoClient ConexiuneBD()
        {

            string connectionUri = "mongodb+srv://Admin_UPT:TrackMeBackUPT2024@trackmeback.5wpb9ih.mongodb.net/?retryWrites=true&w=majority&appName=TrackMeBack";

            var settings = MongoClientSettings.FromConnectionString(connectionUri);

            // Server API versiunea V1 ( doc. MongoDB )
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            // Client nou - conexiune la Server.
            var client = new MongoClient(settings);

            return client;

        }

        public static string Tabel_Utilizator(string email)
        {
            if (email.Contains("student"))
            {
                return "TB_Student";
            }
            else return "TB_Profesor";
        }


        //Model
        public class Orar_Model
        {
            [BsonId]
            ObjectId _id { get; set; }

            public string ziua { get; set; } = string.Empty;

            public string ora_start { get; set; } = string.Empty;

            public string materia { get; set; } = string.Empty;

            public string specializare { get; set; } = string.Empty;

            public int anul { get; set; }

            public int grupa { get; set; }

            public string sala { get; set; } = string.Empty;
        }
    }
}
