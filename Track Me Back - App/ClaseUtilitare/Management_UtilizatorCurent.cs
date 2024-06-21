using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.Json;

namespace TrackMeBack_Management.Clase_Utilitare.Management
{
    public class Management_UtilizatorCurent
    {
        private static readonly string locatie_userSettings = "C:\\Users\\Adeus\\source\\repos\\Track Me Back - App\\Properties\\userSettings.json";
        private class UserSettings
        {
            public InfoStudent? Info_Student {  get; set; }  
            public InfoProfesor? Info_Profesor { get; set; }

            public void Resetare_Setari()
            {
                if (Info_Student != null)
                {
                    Info_Student.Resetare_Setari();
                }
                if(Info_Profesor != null)
                {
                    Info_Profesor.Resetare_Setari();
                }
                
            }
        }

        private class InfoProfesor
        {
            public string Nume { get; set; } = "-";
            public string Prenume { get; set; } = "-";
            public string Facultate { get; set; } = "-";
            public string Marca { get; set; } = "-";

            public void Resetare_Setari()
            {
                Nume = "-";
                Prenume = "-";
                Facultate = "-";
                Marca = "-";
            }
        }

        private class InfoStudent
        {
            public string Nume { get; set; } = "-";
            public string Prenume { get; set; } = "-";
            public string Facultate { get; set; } = "-";

            public string Specializare { get; set; } = "-";
            public string Marca { get; set; } = "-";

            public void Resetare_Setari()
            {
                Nume = "-";
                Prenume = "-";
                Facultate = "-";
                Specializare="-";
                Marca = "-";
            }
        }

        //  Setarea Informatiilor Despre Utilizator.

        public static int Set_Info_Profesor(string email)
        {
            UserSettings settings = Citire_UserSettings();

            var conexiune = ConexiuneBD();
            var filtru = Builders<BsonDocument>.Filter.Eq("email", email);
            var document = conexiune.GetDatabase("Application").GetCollection<BsonDocument>("TB_Profesor").Find(filtru).First();
            
            if (settings != null)
            {

                settings.Info_Profesor.Nume = document.GetValue("nume").AsString;
                settings.Info_Profesor.Prenume = document.GetValue("prenume").AsString;
                settings.Info_Profesor.Facultate = document.GetValue("facultate").AsString;
                settings.Info_Profesor.Marca = document.GetValue("marca").AsString;
                Salvare_UserSettings(settings);
                return 0;
            }
            else
            {
                return -1;  //  Eroare
            }

        }
        public static void Get_Info_Profesor(out string nume, out string marca, out string specializare)
        {
            UserSettings setari = Citire_UserSettings();
            nume = setari.Info_Profesor.Nume + " " + setari.Info_Profesor.Prenume;
            marca = setari.Info_Profesor.Marca;
            specializare = setari.Info_Profesor.Facultate;
        }


        public static int Set_Info_Student(string email)
        {
            UserSettings settings = Citire_UserSettings();

            var conexiune = ConexiuneBD();
            var filtru = Builders<BsonDocument>.Filter.Eq("email", email);
            var document = conexiune.GetDatabase("Application").GetCollection<BsonDocument>("TB_Student").Find(filtru).First();

            if (settings != null)
            {

                settings.Info_Student.Nume = document.GetValue("nume").AsString;
                settings.Info_Student.Prenume = document.GetValue("prenume").AsString;
                settings.Info_Student.Facultate = document.GetValue("facultate").AsString;
                settings.Info_Student.Specializare = document.GetValue("specializare").AsString;
                settings.Info_Student.Marca = document.GetValue("marca").AsString;
                Salvare_UserSettings(settings);
                return 0;
            }
            else
            {
                return -1;  //  Eroare
            }

        }
        public static void Get_Info_Student(out string nume, out string marca, out string specializare)
        {
            UserSettings setari = Citire_UserSettings();
            nume = setari.Info_Student.Nume + " " + setari.Info_Student.Prenume;
            marca = setari.Info_Student.Marca;
            specializare = setari.Info_Student.Specializare;
        }


        public static string Get_Marca_Profesor()
        {
            UserSettings setari = Citire_UserSettings();
            return setari.Info_Profesor.Marca;
        }

        public static string Get_Marca_Student()
        {
            UserSettings setari = Citire_UserSettings();
            return setari.Info_Student.Marca;
        }

        public static void Resetare_Utilizator()
        {
            UserSettings setari = Citire_UserSettings();
            setari.Resetare_Setari();
            Salvare_UserSettings(setari);
        }

        //  Prelucrarea fisierului JSON.
        private static void Salvare_UserSettings(UserSettings setari)
        {
            // Serializare : obj -> json
            string fisier_setari_modificat = JsonSerializer.Serialize(setari);
            //Suprascriere
            File.WriteAllText(locatie_userSettings, fisier_setari_modificat);
        }

        private static UserSettings Citire_UserSettings()
        {
            string fisier_json = File.ReadAllText(locatie_userSettings);

            // Deserializare : json : obj
           return JsonSerializer.Deserialize<UserSettings>(fisier_json);
        }

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
    }
}
