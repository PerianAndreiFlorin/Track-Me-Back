using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrackMeBack_Management.Clase_Utilitare.Management;
using TrackMeBack_Mobile.ClaseUtilitare;

namespace Track_Me_Back___App.ClaseUtilitare
{
    internal class Sistem_Start_Ora
    {
        public static void Adaugare_Disciplina_Activa(string sala, out string mesaj)
        {
            string ziua = DateTime.Today.DayOfWeek.ToString().ToUpper();
            string ora_curenta = DateTime.Now.ToString("HH:mm");

            //------------------------------------------------------------------------------------- Setup Baza Date Orar
            var client = Servicii_MongoDB.ConexiuneBD();
            var colectie_orar = client.GetDatabase("Discipline").GetCollection<BsonDocument>("TB_ORAR_INFO");
            var filtru_ziua = Builders<BsonDocument>.Filter.AnyEq("ziua", ziua);
            var rezultat = colectie_orar.Find(filtru_ziua).ToList();

            //------------------------------------------------------------------------------------- Preluare Materii Profesor
            List<string> discipline_profesor = Servicii_MongoDB.Listare_Discipline_Profesor();

            //------------------------------------------------------------------------------------- Cautare
            foreach (var document in rezultat) 
            {
                string ora_orar = document.GetValue("ora_start",defaultValue:"00:00").AsString;
                if(Verificare_Ora(ora_orar,ora_curenta))
                {
                    string materie_orar = document.GetValue("materia",defaultValue:" ").AsString;
                    foreach(var disciplina_profesor in discipline_profesor)
                    {
                        if(discipline_profesor.Contains(materie_orar))
                        {
                            string mesaj_sala = string.Empty;
                            string sala_orar = document.GetValue("sala", defaultValue: " ").AsString;
                            if (sala==sala_orar)
                            {
                                int index = Calcul_Index_Disciplina(materie_orar);
                                int grupa = document.GetValue("grupa", defaultValue: 1).AsInt32;
                                string tip = Verificare_Tip_Materie(Management_UtilizatorCurent.Get_Marca_Profesor(), materie_orar);
                                //------------------------------------------------------------------------------------- Adaugare Materie Activa
                                var colectie_discipline_active = client.GetDatabase("Discipline").GetCollection<Disciplina_Activa>("TB_DISCIPLINE_ACTIVE");
                                Disciplina_Activa disciplina = new Disciplina_Activa(materie_orar, tip, grupa, index, ziua, sala_orar);

                                colectie_discipline_active.InsertOne(disciplina);
                                mesaj = "Ora a început!";
                                return;
                            }
                            else
                            {
                                Directii_Sala.Directie(sala,sala_orar, out mesaj_sala);
                                mesaj = mesaj_sala;
                                return;
                            }
                        }
                    }
                }
            }

            mesaj = "Nu aveți ore disponibile momentan!";
        }

        private class Disciplina_Activa
        {
            [BsonId]
            public ObjectId _id { get; set; }

            public DateTime data { get; set; }

            public string materia { get; set; } = string.Empty;

            public string tip { get; set; } = string.Empty;

            public int grupa { get; set; }

            public int index { get; set; }

            public string ziua { get; set; } = string.Empty;

            public string sala { get; set; } = string.Empty;

            public Disciplina_Activa(string materia, string tip, int grupa, int index, string ziua, string sala)
            {
                this.materia = materia;
                this.tip = tip;
                this.grupa = grupa;
                this.index = index;
                this.sala = sala;
                this.ziua = ziua;
                this.sala = sala;
                data = DateTime.UtcNow;
            }
        }

        private static int Calcul_Index_Disciplina(string materia)
        {
            int index = -1;
            var conexiune = Servicii_MongoDB.ConexiuneBD();
            var colectie = conexiune.GetDatabase("Discipline").GetCollection<Disciplina_Activa>("TB_PREZENTE_INFO");
            var filtru_materia = Builders<Disciplina_Activa>.Filter.Eq(f => f.materia, materia);
            var rezultat = colectie.Find(filtru_materia).ToList();

            if(rezultat!=null)
            {
                foreach (var materie in rezultat)
                {
                    if (materie.index > index)
                    {
                        index = materie.index;
                    }
                }
            }
            
            index++;

            return index;
        }

        private static bool Verificare_Ora(string ora_orar, string ora_curenta)
        {
            DateTime ora_orar_timp = DateTime.ParseExact(ora_orar,"HH:mm",null);
            DateTime ora_curenta_timp = DateTime.ParseExact(ora_curenta, "HH:mm", null);

            double diferenta_minute = (ora_orar_timp-ora_curenta_timp).TotalMinutes;

            int diferenta_max = 10;
            return Math.Abs(diferenta_minute)<=diferenta_max;
        }

        private static string Verificare_Tip_Materie(string marca, string materie)
        {
            var client = Servicii_MongoDB.ConexiuneBD();
            var colectie = client.GetDatabase("Discipline").GetCollection<BsonDocument>("TB_INFO");

            var filtru_materie = Builders<BsonDocument>.Filter.AnyEq("materie", materie);
            var rezultat = colectie.Find(filtru_materie).FirstOrDefault();

            string tip = "Laborator";
            foreach (var detaliu in rezultat) 
            { 
                if (detaliu.Value == marca && detaliu.Name == "profesor_curs")
                {
                    tip = "Curs";
                }
            }

            return tip; 
        }
    }  
}
