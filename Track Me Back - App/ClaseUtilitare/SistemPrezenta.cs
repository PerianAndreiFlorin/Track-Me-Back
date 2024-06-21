using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackMeBack_Management.Clase_Utilitare.Management;
using TrackMeBack_Mobile.ClaseUtilitare;

namespace Track_Me_Back___App.ClaseUtilitare
{
    internal class SistemPrezenta
    {
        public static int Verificare(string sala_curenta, string ziua, out string mesaj)
        {
            //------------------------------------------------------------------------------------- Variabile Student
            int grupa = 0, an = 0;
            string specializare = string.Empty;
            string marca = Management_UtilizatorCurent.Get_Marca_Student();
            Servicii_MongoDB.Info_Student(out specializare, out an, out grupa);

            List<string> lista_materii_student = Servicii_MongoDB.Listare_Discipline_Student();

            //------------------------------------------------------------------------------------- Setup Baza Date
            var client = Servicii_MongoDB.ConexiuneBD();
            var colectie = client.GetDatabase("Discipline").GetCollection<BsonDocument>("TB_DISCIPLINE_ACTIVE");
            var filtru_zi = Builders<BsonDocument>.Filter.AnyEq("ziua", ziua);
            var filtru_grupa = Builders<BsonDocument>.Filter.AnyEq("grupa", grupa);

            //------------------------------------------------------------------------------------- Preluare Date Mmongo
            var rezultat = colectie.Find(filtru_zi&filtru_grupa).FirstOrDefault();

            string materia_activa = "-";
            string sala_activa = "-";
            string tip_ora = "-";
            int index = 0;

            if (rezultat!=null)
            {
                materia_activa = rezultat.GetValue("materia").AsString;
                sala_activa = rezultat.GetValue("sala").AsString;
                tip_ora = rezultat.GetValue("tip").AsString;
                index = rezultat.GetValue("index").AsInt32;
            }
            

            if (lista_materii_student.Contains(materia_activa))
            {
                if(sala_curenta==sala_activa)
                {
                    string tabel = "TB_PREZENTE_" + specializare;
                    var colectie_prezente = client.GetDatabase("Discipline").GetCollection<Prezenta_Student>(tabel);

                    if(!Verificare_Prezenta(tabel,materia_activa,marca,tip_ora,index))
                    {
                        mesaj = "Prezență adăugată cu succes!";

                        Prezenta_Student prezenta = new Prezenta_Student(materia_activa, marca, tip_ora, index);
                        colectie_prezente.InsertOne(prezenta);
                        return 1;
                    }
                    else
                    {
                        mesaj = "Prezență deja adăugată!";
                        return 2;
                    }
                }
                else
                {
                    mesaj = "Sală necorespunzătoare!";
                    Directii_Sala.Directie(sala_curenta, sala_activa, out mesaj);
                    return 0;
                } 
            }
            else
            {
                mesaj = "Nu există materii active!";
                return -1;
            }
        }

        private class Prezenta_Student
        {
            [BsonId] 
            ObjectId _id;

            public string materia { get; set; } = string.Empty;

            public string marca_student {  get; set; } = string.Empty;

            public string tip {  get; set; } = string.Empty;

            public int index {  get; set; }

            public Prezenta_Student(string materia, string marca_student,string tip, int index)
            {
                this.materia = materia;
                this.marca_student = marca_student;
                this.tip = tip;
                this.index = index;
            }
        }

        private static bool Verificare_Prezenta(string tabel, string materie, string marca, string tip, int index)
        {
            var client = Servicii_MongoDB.ConexiuneBD();
            var colectie_prezente = client.GetDatabase("Discipline").GetCollection<BsonDocument>(tabel);
            var filtru_materie = Builders<BsonDocument>.Filter.AnyEq("materia", materie);
            var filtru_marca = Builders<BsonDocument>.Filter.AnyEq("marca_student", marca);
            var filtru_tip = Builders<BsonDocument>.Filter.AnyEq("tip", tip);
            var filtru_index = Builders<BsonDocument>.Filter.AnyEq("index", index);

            var rezultat = colectie_prezente.Find(filtru_marca & filtru_materie & filtru_tip & filtru_index).FirstOrDefault();

            return rezultat != null;
        }

    }
}
