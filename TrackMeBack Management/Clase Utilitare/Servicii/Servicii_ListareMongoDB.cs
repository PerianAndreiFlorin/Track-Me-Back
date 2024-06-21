using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Drawing.Drawing2D;
using System.Reflection.Emit;
using TrackMeBack_Management.Clase_Utilitare.Management;
using TrackMeBack_Management.Models.Discipline;
using TrackMeBack_Management.Models.Orar;
using TrackMeBack_Management.Models.Utilizator;
using static QRCoder.PayloadGenerator;

namespace TrackMeBack_Management.Clase_Utilitare.Servicii
{
    public static class Servicii_ListareMongoDB
    {

        public static List<string> Listare_TB_Administrare()
        {
            List<string> lista = new List<string>();

            var conexiune = Management_MongoDB.ConexiuneBD();
            var colectie = conexiune.GetDatabase("Application").GetCollection<Administrator_Model>("TB_Administrare");
            var rezultat = colectie.Find(new BsonDocument()).ToList();

            foreach (var utilizator in rezultat)
            {
                lista.Add(utilizator.email.ToString());
            }

            return lista; 
        }

        public static List<List<string>> Listare_Harta_UPT()
        {
            List<List<string>> lista_sali_string = new List<List<string>>();

            //  Conectare MongoBD.
            var conexiune = Management_MongoDB.ConexiuneBD();
            var baza_date = conexiune.GetDatabase("HartaCampusUPT");

            //  Preluarea fiecărei colecții.
            var lista_colectii = baza_date.ListCollectionNames().ToList();
            
            //  Iterație prin fiecare colecție.
            foreach(var colectie in lista_colectii)
            {
                var documente = baza_date.GetCollection<BsonDocument>(colectie).Find(new BsonDocument()).ToList();

                //  Iterație prin fiecare sală din tabel ( colecție ).
                foreach (var document in documente)
                {
                    string cladire = document.GetValue("cladire", defaultValue: "-").AsString;
                    string corp = document.GetValue("corp", defaultValue: "-").AsString;
                    string index = document.GetValue("index", defaultValue: "-").AsString;
                    string qr = document.GetValue("cod_QR", defaultValue: "-").AsString;

                    List<string> document_lista = new List<string>();
                    document_lista.Add(cladire);
                    document_lista.Add(corp);
                    document_lista.Add(index);
                    if(qr!="-")
                    {
                        document_lista.Add("data:image/png;base64," + qr);
                    }
                    else { document_lista.Add(qr); }
                    

                    lista_sali_string.Add(document_lista);
                }
            }

            return lista_sali_string;
        }

        public static List<string> Listare_Coduri_Sali()
        {
            List<string> lista_sali = new List<string>();

            //  Conectare MongoBD.
            var conexiune = Management_MongoDB.ConexiuneBD();
            var baza_date = conexiune.GetDatabase("HartaCampusUPT");

            //  Preluarea fiecărei colecții.
            var lista_colectii = baza_date.ListCollectionNames().ToList();

            //  Iterație prin fiecare colecție.
            foreach (var colectie in lista_colectii)
            {
                var documente = baza_date.GetCollection<BsonDocument>(colectie).Find(new BsonDocument()).ToList();

                //  Iterație prin fiecare sală din tabel ( colecție ).
                foreach (var document in documente)
                {
                    string cladire = document.GetValue("cladire", defaultValue: "-").AsString;
                    string corp = document.GetValue("corp", defaultValue: "-").AsString;
                    string index = document.GetValue("index", defaultValue: "-").AsString;

                    string cod_sala = cladire;
                    if(corp!="-")
                    {
                        cod_sala += "_" + corp + "_" + index;
                    }
                    else { cod_sala += "_" + index; }

                    lista_sali.Add(cod_sala);
                }
            }

            return lista_sali;
        }

        public static List<string> Listare_Cladiri_UPT()
        {
            List<string> lista_cladiri = new List<string>();
            //  Conectare MongoBD.
            var conexiune = Management_MongoDB.ConexiuneBD();
            var baza_date = conexiune.GetDatabase("HartaCampusUPT");

            //  Preluarea fiecărei colecții.
            var lista_colectii = baza_date.ListCollectionNames().ToList();

            foreach (string tabel in lista_colectii) 
            {
                if(tabel !="TB_INFORMATIONAL")
                {
                    int index = tabel.IndexOf("_") + 1;
                    string cladire = tabel.Remove(0, index);
                    lista_cladiri.Add(cladire);
                }
            }
            return lista_cladiri;
        }

        public static List<List<string>> Listare_Orar()
        {
            List<List<string>> lista_orar = new List<List<string>>();
            //  Conectare MongoBD.
            var conexiune = Management_MongoDB.ConexiuneBD();
            var baza_date = conexiune.GetDatabase("Discipline");

            var lista_colectii = baza_date.ListCollectionNames().ToList();
            foreach (var colectie in lista_colectii)
            {
                if (colectie.Contains("TB_ORAR"))
                {
                    var colectie_bd = baza_date.GetCollection<Orar_Model>(colectie).Find(new BsonDocument()).ToList();

                    foreach(var orar in colectie_bd)
                    {
                        List<string> lista = new List<string>();
                        lista.Add(orar.materia);
                        lista.Add(orar.specializare);
                        lista.Add(orar.ziua);
                        lista.Add(orar.ora_start);
                        lista.Add(orar.sala);
                        lista.Add(orar.anul.ToString());
                        lista.Add(orar.grupa.ToString());
                        lista_orar.Add(lista);
                    }
                }
            }
            return lista_orar;
        }

        public static List<List<string>> Listare_Discipline_UPT(string specializare)
        {
            string tabel = "TB_" + specializare.ToUpper();
            List<List<string>> lista_discipline = new List<List<string>>();

            var conexiune = Management_MongoDB.ConexiuneBD();
            var colectie = conexiune.GetDatabase("Discipline").GetCollection<Discipline_Model>(tabel);
            var rezultat = colectie.Find(new BsonDocument()).ToList();

            foreach (var disciplina in rezultat)
            {
                List<string> lista = new List<string>();
                lista.Add(disciplina.materie);
                lista.Add(disciplina.specializare);
                lista.Add(disciplina.an.ToString());
                lista.Add(disciplina.semestru.ToString());
                lista.Add(disciplina.profesor_curs);
                lista.Add(disciplina.profesor_laborator);
                lista.Add(disciplina.profesor_laborator_2);

                lista_discipline.Add(lista);
            }
            return lista_discipline;
        }

        public static List<string> Listare_Materii_UPT(string specializare)
        {
            string tabel = "TB_" + specializare.ToUpper();
            List<string> lista_discipline = new List<string>();

            var conexiune = Management_MongoDB.ConexiuneBD();
            var colectie = conexiune.GetDatabase("Discipline").GetCollection<Discipline_Model>(tabel);
            var rezultat = colectie.Find(new BsonDocument()).ToList();

            foreach (var disciplina in rezultat)
            {
                lista_discipline.Add(disciplina.materie);
            }
            return lista_discipline;
        }

        public static List<List<string>> Listare_Utilizator(string tip_utilizator)
        {
            List<List<string>> lista_profesori=new List<List<string>>();

            var conexiune = Management_MongoDB.ConexiuneBD();
            string tabel= "TB_";

            switch (tip_utilizator)
            {
                case "profesor":
                    tabel += "Profesor";
                    break;
                case "student":
                    tabel += "Student";
                    break;
                    default:
                        List<string> lista_eroare = new List<string>();
                        lista_eroare.Add("Eroare la încărcarea documentelor!");
                        lista_profesori.Add(lista_eroare);
                        return lista_profesori;
            }

            var documente = conexiune.GetDatabase("Application").GetCollection<BsonDocument>(tabel).Find(new BsonDocument()).ToList();

            foreach (var document in documente)
            {
                List<string> string_doc = new List<string>();
                string marca = document.GetValue("marca", defaultValue: "-").AsString;
                string nume = document.GetValue("nume", defaultValue: "-").AsString;
                string prenume = document.GetValue("prenume", defaultValue: "-").AsString;
                string email = document.GetValue("email", defaultValue: "-").AsString;

                string_doc.Add(marca);
                string_doc.Add(nume);
                string_doc.Add(prenume);
                string_doc.Add(email);

                lista_profesori.Add(string_doc);
            }

            return lista_profesori;
        }

        public static List<List<string>> Listare_Profesor_Facultate(string facultate)
        {
            List<List<string>> lista_profesori = new List<List<string>>();

            var conexiune = Management_MongoDB.ConexiuneBD();
            var documente = conexiune.GetDatabase("Application").GetCollection<BsonDocument>("TB_Profesor").Find(new BsonDocument()).ToList();

            foreach (var document in documente)
            {
                if(document.GetValue("facultate",defaultValue:"-")=="AC")
                {
                    List<string> string_doc = new List<string>();
                    string marca = document.GetValue("marca", defaultValue: "-").AsString;
                    string nume = document.GetValue("nume", defaultValue: "-").AsString;
                    string prenume = document.GetValue("prenume", defaultValue: "-").AsString;

                    string_doc.Add(marca);
                    string_doc.Add(nume);
                    string_doc.Add(prenume);

                    lista_profesori.Add(string_doc);
                }
            }

            return lista_profesori ;
        }

        public static List<List<string>> Listare_Student_Facultate(string facultate)
        {
            List<List<string>> lista_profesori = new List<List<string>>();

            var conexiune = Management_MongoDB.ConexiuneBD();
            var documente = conexiune.GetDatabase("Application").GetCollection<BsonDocument>("TB_Student").Find(new BsonDocument()).ToList();

            foreach (var document in documente)
            {
                if (document.GetValue("facultate", defaultValue: "-") == "AC")
                {
                    List<string> string_doc = new List<string>();
                    string marca = document.GetValue("marca", defaultValue: "-").AsString;
                    string nume = document.GetValue("nume", defaultValue: "-").AsString;
                    string prenume = document.GetValue("prenume", defaultValue: "-").AsString;
                    string specializare = document.GetValue("specializare", defaultValue: "-").AsString;
                    string an = document.GetValue("an", defaultValue: "-").AsString;
                    string email = document.GetValue("email", defaultValue: "-").AsString;

                    string_doc.Add(marca);
                    string_doc.Add(nume);
                    string_doc.Add(prenume);
                    string_doc.Add(specializare);
                    string_doc.Add(an);
                    string_doc.Add(email);

                    lista_profesori.Add(string_doc);
                }
            }

            return lista_profesori;
        }

    }
}
