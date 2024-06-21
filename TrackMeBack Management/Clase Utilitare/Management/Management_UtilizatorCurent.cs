using MongoDB.Driver;
using System.Text.Json;
using TrackMeBack_Management.Models.Utilizator;

namespace TrackMeBack_Management.Clase_Utilitare.Management
{
    public class Management_UtilizatorCurent
    {
        private static readonly string locatie_userSettings = "Properties\\userSettings.json";
        private class UserSettings
        {
            public bool Administrator { get; set; } = false;
            public string Dep_Secretariat { get; set; } = "-";
            public InfoProfesor? Info_Profesor { get; set; }

            public void Resetare_Setari()
            {
                Administrator = false;
                Dep_Secretariat = "-";
                Info_Profesor.Resetare_Setari();
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

        //  Setarea Informatiilor Despre Utilizator.

        public static int Set_Administrator()
        {
            UserSettings settings = Citire_UserSettings();

            if (settings != null)
            {
                settings.Administrator=true;
                Salvare_UserSettings(settings);
                return 0;
            }
            else
            {
                return -1;  //  Eroare.
            }

        }

        public static int Set_Secretariat(string email)
        {
            UserSettings setari = Citire_UserSettings();

            if (setari != null)
            {
                setari.Dep_Secretariat = Prenume_Email(email).ToUpper();
                Salvare_UserSettings(setari);
                return 0;
            }
            else
            {
                return -1;  //  Eroare.
            }

        }

        public static int Set_Info_Prof(string email)
        {
            UserSettings settings = Citire_UserSettings();

            var conexiune = Management_MongoDB.ConexiuneBD();
            var filtru = Builders<Profesor_Model>.Filter.Eq(f => f.email, email);
            var document = conexiune.GetDatabase("Application").GetCollection<Profesor_Model>("TB_Profesor").Find(filtru).First();
            
            if (settings != null)
            {

                settings.Info_Profesor.Nume = document.nume;
                settings.Info_Profesor.Prenume = document.prenume;
                settings.Info_Profesor.Facultate = document.facultate;
                settings.Info_Profesor.Marca = document.marca;
                Salvare_UserSettings(settings);
                return 0;
            }
            else
            {
                return -1;  //  Eroare
            }

        }

        public static class Profesor
        {
            public static string Marca()
            {
                UserSettings setari = Citire_UserSettings();
                return setari.Info_Profesor.Marca;
            }
        }


        public static void Resetare_Utilizator()
        {
            UserSettings setari = new UserSettings();
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

        public static string Departament_Secretariat()
        {
           UserSettings setari = Citire_UserSettings();

            return setari.Dep_Secretariat.ToUpper();
        }

        //  Preluarea Numelui si Prenumelui din Email
        public static string Prenume_Email(string email)             // Preluarea Prenumelui : prenume.
        {
            int PozitieFinal = email.IndexOf('.');
            string Prenume = email.Substring(0, PozitieFinal);

            Prenume = char.ToUpper(Prenume[0]) + Prenume.Substring(1);

            return Prenume;
        }

        public static string Nume_Email(string email)                //	Preluarea Numelui : .nume@
        {
            int PozitieInceput = email.IndexOf('.') + 1;
            int PozitieFinal = email.IndexOf('@') - PozitieInceput;
            string Nume = email.Substring(PozitieInceput, PozitieFinal);

            Nume = char.ToUpper(Nume[0]) + Nume.Substring(1);
            return Nume;
        }
    }
}
