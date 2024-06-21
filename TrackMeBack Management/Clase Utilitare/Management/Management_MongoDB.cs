using MongoDB.Bson;
using MongoDB.Driver;
using TrackMeBack_Management.Models.Autentificare;
using TrackMeBack_Management.Models.Utilizator;
using TrackMeBack_Management.Clase_Utilitare.Servicii;
using TrackMeBack_Management.Models.Harta;
using TrackMeBack_Management.Models.Discipline;
using System.Reflection.Emit;
using System.Drawing.Text;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using static QRCoder.PayloadGenerator;
using TrackMeBack_Management.Models.Orar;
using TrackMeBack_Management.Models.Prezente;

namespace TrackMeBack_Management.Clase_Utilitare.Management
{
    public static class Management_MongoDB
    {
        public static class Notare
        {
            public static int Notare_Student(Notare_Model notare)
            {
                var client = ConexiuneBD();
                var colectie = client.GetDatabase("Discipline").GetCollection<BsonDocument>("TB_Prezente_INFO");

                var filtru_marca = Builders<BsonDocument>.Filter.AnyEq("marca", notare.marca);
                var filtru_index = Builders<BsonDocument>.Filter.AnyEq("index", notare.index);

                var rezultat = colectie.Find(filtru_marca & filtru_index).FirstOrDefault().Add("nota",notare.nota);

                if (rezultat != null) { return 0; }
                else { return 1; }
            }
        }


        public static class Utilizator
        {
            public static int Editare_Cont_Administrare(string email, string actiune, string parola_noua)
            {
                var client = ConexiuneBD();
                var colectie = client.GetDatabase("Application").GetCollection<Administrator_Model>("TB_Administrare");
                var filtru = Builders<Administrator_Model>.Filter.Eq(f => f.email, email);

                if (email == "admin.admin@upt.ro" && actiune == "Stergere")
                {
                    return -3;  // Eroare stergere Admin
                }
                else
                {
                    switch (actiune)
                    {
                        case "Stergere":

                            var stergere = colectie.DeleteOne(filtru);
                            if (stergere != null)
                            {
                                return 0;
                            }
                            else return -2;//eroare stergere

                        case "Adaugare":

                            if (!Administrator_Existent(email))
                            {
                                Administrator_Model utilizator_nou = new Administrator_Model(email, parola_noua);
                                colectie.InsertOne(utilizator_nou);
                                return 0;
                            }
                            else return -1;

                        default:

                            string hash_parola_noua = Servicii_Criptare.Criptare(parola_noua);
                            var update = Builders<Administrator_Model>.Update.Set(u => u.parola, hash_parola_noua);
                            UpdateResult rezultat = colectie.UpdateOne(filtru, update);

                            if (rezultat != null)
                            {
                                return 0;
                            }
                            else return -2; //eroare resetare
                    }
                }
            }


            public static int Editare_Cont_Profesor(string marca, Profesor_Model profesor, string actiune)
            {
                var client = ConexiuneBD();
                var colectie = client.GetDatabase("Application").GetCollection<Profesor_Model>("TB_Profesor");

                var filtru = Builders<Profesor_Model>.Filter.Eq(f => f.marca, marca);

                switch (actiune)
                {
                    case "Stergere":

                        bool exista = colectie.Find(filtru).FirstOrDefault()!=null;

                        if (!exista)
                        {
                            return -3;
                        }

                        var stergere = colectie.DeleteOne(filtru);
                        if (stergere != null)
                        {
                            return 0;
                        }
                        else return -2; //eroare stergere

                    case "Adaugare":
                        if(!Profesor_Existent(profesor.email))
                        {
                            colectie.InsertOne(profesor);
                            return 0; 
                        }
                        else
                        {
                            return -1;  //utilizator existent
                        }

                    default:
                        var document_vechi = colectie.Find(filtru).FirstOrDefault();
                        profesor._id = document_vechi._id;
                        profesor.marca = document_vechi.marca;
                        var rezultat = colectie.ReplaceOne(filtru, profesor);
                        if (rezultat != null)
                        {
                            return 0;
                        }
                        else return -2; //eroare resetare
                }
            }


            public static int Editare_Cont_Student(string marca, Student_Model student, string actiune)
            {
                var client = ConexiuneBD();
                var colectie = client.GetDatabase("Application").GetCollection<Student_Model>("TB_Student");

                var filtru = Builders<Student_Model>.Filter.Eq(f => f.marca, marca);

                switch (actiune)
                {
                    case "Stergere":

                        bool exista = colectie.Find(filtru).FirstOrDefault() != null;

                        if (!exista)
                        {
                            return -3;
                        }

                        var stergere = colectie.DeleteOne(filtru);
                        if (stergere != null)
                        {
                            return 0;
                        }
                        else return -2; //eroare stergere

                    case "Adaugare":
                        if (!Student_Existent(student.email))
                        {
                            colectie.InsertOne(student);
                            return 0;
                        }
                        else
                        {
                            return -1;  //utilizator existent
                        }

                    default:
                        exista = colectie.Find(filtru).FirstOrDefault() != null;

                        if (!exista)
                        {
                            return -3;
                        }

                        var id_vechi = colectie.Find(filtru).FirstOrDefault();
                        student._id = id_vechi._id;
                        var rezultat = colectie.ReplaceOne(filtru, student);
                        if (rezultat != null)
                        {
                            return 0;
                        }
                        else return -2; //eroare resetare
                }
            }


            public static bool Profesor_Existent(string email)
            {
                var client = ConexiuneBD();
                var colectie = client.GetDatabase("Application").GetCollection<BsonDocument>("TB_Profesor");
                var filtru = Builders<BsonDocument>.Filter.AnyEq("email",email);
                var profesor = colectie.Find(filtru).FirstOrDefault();

                if (profesor == null)
                {
                    return false;
                }
                else { return true; }
                
            }


            public static bool Student_Existent(string email)
            {
                var client = ConexiuneBD();
                var colectie = client.GetDatabase("Application").GetCollection<BsonDocument>("TB_Student");
                var filtru = Builders<BsonDocument>.Filter.AnyEq("email", email);
                var profesor = colectie.Find(filtru).FirstOrDefault();

                if (profesor == null)
                {
                    return false;
                }
                else { return true; }

            }


            public static bool Administrator_Existent(string email)
            {
                var client = ConexiuneBD();
                var colectie = client.GetDatabase("Application").GetCollection<BsonDocument>("TB_Administrare");
                var filtru = Builders<BsonDocument>.Filter.AnyEq("email", email);
                var administrator = colectie.Find(filtru).FirstOrDefault();

                if (administrator == null)
                {
                    return false;
                }
                else { return true; }
            }
        }


        public static class Harta_UPT
        {
            public static int Editare_Sala(Sala_Model sala, string actiune)
            {

                var client = ConexiuneBD();
                string tabel = "TB_" + sala.cladire;

                var colectie = client.GetDatabase("HartaCampusUPT").GetCollection<Sala_Model>(tabel);

                switch (actiune)
                {
                    case "Stergere":

                        var filtru_cladire = Builders<Sala_Model>.Filter.Eq(f => f.cladire, sala.cladire);
                        var filtru_corp = Builders<Sala_Model>.Filter.Eq(f => f.corp, sala.corp);
                        var filtru_index = Builders<Sala_Model>.Filter.Eq(f => f.index, sala.index);

                        if(colectie.Find(filtru_cladire& filtru_corp & filtru_index).FirstOrDefault()==null)
                        {
                            return -2;
                        }

                        var stergere = colectie.DeleteOne(filtru_cladire & filtru_corp & filtru_index);
                        if (stergere != null)
                        {
                            return 0;
                        }
                        else return -3; //   eroare selecție

                    case "Adaugare":
                        if (!Sala_Existenta(sala, tabel))
                        {
                            colectie.InsertOne(sala);
                        }
                        else
                        {
                            return -1;  // sala exista
                        }
                        return 0;

                    default:
                        return -3; //   eroare selecție
                }
            }


            public static bool Sala_Existenta(Sala_Model sala, string tabel)
            {
                var client = ConexiuneBD();
                var colectie = client.GetDatabase("HartaCampusUPT").GetCollection<Sala_Model>(tabel);
                var filtru = Builders<Sala_Model>.Filter.Eq(f => f.index, sala.index);
                var lista_sala = colectie.Find(filtru).ToList();

                foreach (Sala_Model sala_index in lista_sala)
                {
                    if (sala_index.corp == sala.corp)
                    {
                        return true;
                    }
                }

                return false;
            }
        }


        public static class Orar_UPT
        {
            public static int Editare(Orar_Model orar, string actiune)
            {
                var client = ConexiuneBD();
                string tabel = "TB_ORAR_"+ orar.specializare;
                var colectie = client.GetDatabase("Discipline").GetCollection<Orar_Model>(tabel);

                switch (actiune)
                {
                    case "Stergere":

                        var filtru_materie = Builders<Orar_Model>.Filter.Eq(f => f.materia, orar.materia);
                        var filtru_grupa = Builders<Orar_Model>.Filter.Eq(f => f.grupa, orar.grupa);
                        var filtru_ora = Builders<Orar_Model>.Filter.Eq(f => f.ora_start, orar.ora_start);
                        var filtru_zi = Builders<Orar_Model>.Filter.Eq(f => f.ziua, orar.ziua);

                        if (colectie.Find(filtru_materie & filtru_grupa & filtru_ora).FirstOrDefault() == null)
                        {
                            return -2;
                        }

                        var stergere = colectie.DeleteOne(filtru_materie & filtru_grupa & filtru_ora);
                        if (stergere != null)
                        {
                            return 0;
                        }
                        else return -3; //   eroare selecție

                    case "Adaugare":
                        if (!Orar_Existent(orar))
                        {
                            colectie.InsertOne(orar);
                        }
                        else
                        {
                            return -1;  // orarul exista
                        }
                        return 0;

                    default:
                        return -3; //   eroare selecție
                }
            }

            public static bool Orar_Existent(Orar_Model orar)
            {
                var client = ConexiuneBD();
                string tabel = "TB_ORAR_" + orar.specializare;
                var colectie = client.GetDatabase("Discipline").GetCollection<Orar_Model>(tabel);

                var filtru_cladire = Builders<Orar_Model>.Filter.Eq(f => f.materia, orar.materia);
                var filtru_corp = Builders<Orar_Model>.Filter.Eq(f => f.grupa, orar.grupa);
                var filtru_index = Builders<Orar_Model>.Filter.Eq(f => f.ora_start, orar.ora_start);

                if (colectie.Find(filtru_cladire & filtru_corp & filtru_index).FirstOrDefault() != null)
                {
                    return true;
                }
                return false;
            }
        }


        public static class Discipline_UPT
        {
            
            public static int Editare_Disciplina(Discipline_Model disciplina, string actiune)
            {
                var client = ConexiuneBD();
                string tabel = "TB_" + disciplina.specializare.ToString().ToUpper();
                var colectie = client.GetDatabase("Discipline").GetCollection<Discipline_Model>(tabel);

                switch (actiune)
                {
                    case "Stergere":

                        var filtru = Builders<Discipline_Model>.Filter.Eq(f => f.materie, disciplina.materie);

                        bool exista = colectie.Find(filtru).FirstOrDefault()!=null;

                        if(!exista)
                        {
                            return -3;  //  nu exista
                        }

                        var stergere = colectie.DeleteOne(filtru);
                        if (stergere != null)
                        {
                            return 0;
                        }
                        else return -2; //   eroare selecție

                    case "Adaugare":
                        if (disciplina.semestru < (disciplina.an+disciplina.an-1) || disciplina.semestru > (disciplina.an + disciplina.an))
                        {
                            return -3; //   semestru invalid
                        }

                        if (!Disciplina_Existenta(disciplina.materie, disciplina.specializare))
                        {
                            colectie.InsertOne(disciplina);
                        }
                        else return -1; //Disciplina existenta
                        return 0;

                    default:
                        var filtru_edit = Builders<Discipline_Model>.Filter.Eq(f => f.materie, disciplina.materie);

                        var obiect_curent = colectie.Find(filtru_edit).FirstOrDefault();
                        disciplina._id = obiect_curent._id;

                        if (disciplina.semestru < (disciplina.an + disciplina.an - 1) || disciplina.semestru > (disciplina.an + disciplina.an))
                        {
                            return -3; //   semestru invalid
                        }

                        var rezultat = colectie.ReplaceOne(filtru_edit, disciplina);
                        if (rezultat != null)
                        {
                            return 0;
                        }
                        return -2; //   eroare selecție
                }

            }

            public static bool Disciplina_Existenta(string materie, string specializare)
            {
                var client = ConexiuneBD();
                var filtru = Builders<Discipline_Model>.Filter.Eq(f => f.materie, materie);
                string tabel = "TB_" + specializare.ToUpper();

                var rezultat = client.GetDatabase("Discipline").GetCollection<Discipline_Model>("TB_INFO").Find(filtru).FirstOrDefault();

                if (rezultat == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }


        public static class Adaugare_Excel
        {
            //  Harta Campus UPT
            public static int Adaugare_Sala()
            {
                string tabel_curent = string.Empty;
                string tabel_precedent = string.Empty;
                List<Sala_Model> lista_sala = Servicii_Excel.Serializare_Sala();

                if (lista_sala != null)
                {
                    //  Setup
                    tabel_curent = "TB_" + lista_sala[0].cladire;
                    tabel_precedent = tabel_curent;

                    var client = ConexiuneBD();
                    var colectie = client.GetDatabase("HartaCampusUPT").GetCollection<Sala_Model>(tabel_curent);

                    foreach (var sala in lista_sala)
                    {
                        tabel_curent = "TB_" + sala.cladire;
                        if (!Harta_UPT.Sala_Existenta(sala, tabel_curent))
                        {
                            if (tabel_curent == tabel_precedent)
                            {
                                colectie.InsertOne(sala);
                            }
                            else
                            {
                                colectie = client.GetDatabase("HartaCampusUPT").GetCollection<Sala_Model>(tabel_curent);
                                colectie.InsertOne(sala);
                            }
                            tabel_precedent = tabel_curent;
                        }
                    }
                    return 0;   //  Succes
                }
                else
                {
                    Raport_Erori.Raportare("Management MongoDB : Adaugare Excel -> Eroare Serializare Sala");
                    return -1;  //  Eroare Serializare_Sala
                }
            }

            public static int Adaugare_Profesor()
            {
                List<Profesor_Model> lista_profesor = Servicii_Excel.Serializare_Profesor();

                if (lista_profesor != null)
                {
                    var client = ConexiuneBD();
                    var colectie = client.GetDatabase("Application").GetCollection<Profesor_Model>("TB_Profesor");

                    foreach(var profesor in lista_profesor)
                    {
                        if(!Utilizator.Profesor_Existent(profesor.email))
                        {
                            colectie.InsertOne(profesor);
                        }
                    }
                    return 0;
                }
                else
                {
                    Raport_Erori.Raportare("Management MongoDB : Adaugare Excel -> Eroare Serializare Profesor");
                    return -1;  // Eroare Serializare_Profesor
                }
            }

            public static int Adaugare_Student()
            {
                List<Student_Model> lista_student = Servicii_Excel.Serializare_Student();

                if (lista_student != null)
                {
                    var client = ConexiuneBD();
                    var colectie = client.GetDatabase("Application").GetCollection<Student_Model>("TB_Student");

                    foreach (var student in lista_student)
                    {
                        if (!Utilizator.Student_Existent(student.email))
                        {
                            colectie.InsertOne(student);
                        }
                    }
                    return 0;
                }
                else
                {
                    Raport_Erori.Raportare("Management MongoDB : Adaugare Excel -> Eroare Serializare Profesor");
                    return -1;  // Eroare Serializare_Profesor
                }
            }

            public static int Adaugare_Disciplina()
            {
                List<Discipline_Model> lista_discipline = Servicii_Excel.Serializare_Disciplina();

                if (lista_discipline != null)
                {
                    var client = ConexiuneBD();
                    string tabel = "TB_" + lista_discipline[1].specializare;
                    var colectie = client.GetDatabase("Discipline").GetCollection<Discipline_Model>(tabel);

                    foreach (var disciplina in lista_discipline)
                    {
                        if (disciplina.semestru == (disciplina.an + disciplina.an - 1) || disciplina.semestru == (disciplina.an + disciplina.an))
                        {
                            if (!Discipline_UPT.Disciplina_Existenta(disciplina.materie, tabel))
                            {
                                colectie.InsertOne(disciplina);
                            }
                        } 
                    }
                    return 0;
                }
                else
                {
                    return -1;  // Eroare Serializare_Profesor
                }
            }

            public static int Adaugare_Orar()
            {
                List<Orar_Model> lista_orar = Servicii_Excel.Serializare_Orar();

                if (lista_orar != null)
                {
                    var client = ConexiuneBD();
                    var lista_tabel = client.GetDatabase("Discipline").ListCollectionNames().ToList();

                    foreach (var orar in lista_orar)
                    {
                        string tabel = "TB_ORAR_" + orar.specializare;
                        var colectie = client.GetDatabase("Discipline").GetCollection<Orar_Model>(tabel);
                        if (!Orar_UPT.Orar_Existent(orar))
                        {
                                colectie.InsertOne(orar);
                        }
                    }
                    return 0;
                }
                else
                {
                    return -1;  // Eroare Serializare_Profesor
                }
            }
        }


        public static class Autentificare
        {
            //  Accesul în Aplicație.
            public static bool Acces(string email, string parola)
            {
                var client = ConexiuneBD();
                var colectie = client.GetDatabase("Application").GetCollection<BsonDocument>(Retur_Tabel(email));

                var filtru = Builders<BsonDocument>.Filter.AnyEq("email",email);
                var rezultat = colectie.Find(filtru).FirstOrDefault().GetValue("parola");

                string hash_parola = Servicii_Criptare.Criptare(parola);
                if (rezultat == hash_parola)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }


            //  Resetarea Parolei.
            public static int Resetare_Parola(string email, string parola_noua, string cod)
            {

                if (Acces_Resetare(email, cod))
                {
                    var client = ConexiuneBD();
                    var colectie = client.GetDatabase("Application").GetCollection<BsonDocument>(Retur_Tabel(email));

                    string hash_parola_noua = Servicii_Criptare.Criptare(parola_noua);

                    var filtru = Builders<BsonDocument>.Filter.Eq("email", email);
                    var update = Builders<BsonDocument>.Update.Set("parola", hash_parola_noua);

                    UpdateResult rezultat = colectie.UpdateOne(filtru, update);
                    if (rezultat != null)
                    {
                        return 0;
                    }
                    else return -2; //eroare resetare
                }
                else return -1;  //eroare access
            }

            private static bool Acces_Resetare(string email, string cod_resetare)
            {
                var client = ConexiuneBD();

                //  Datele ce urmeaza a fi schimbate. ( Folosind cod Resetare )
                var filtru = Builders<Model_Cerere_Resetare>.Filter.Eq(f => f.email, email);
                var rezultat = client.GetDatabase("Application").GetCollection<Model_Cerere_Resetare>("TB_Cod_Resetare").Find(filtru).FirstOrDefault();

                if (rezultat.cod_resetare == cod_resetare)
                {
                    return true;
                }
                else return false;
            }

            public static bool Verificare_Cerere(string email)
            {
                var client = ConexiuneBD();
                var filtru = Builders<Model_Cerere_Resetare>.Filter.Eq(f => f.email, email);
                var rezultat = client.GetDatabase("Application").GetCollection<Model_Cerere_Resetare>("TB_Cod_Resetare").Find(filtru).FirstOrDefault();

                if (rezultat == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            public static int Cerere_Resetare_Parola(string email)
            {
                if (!Verificare_Cerere(email))
                {
                    var client = ConexiuneBD();
                    Model_Cerere_Resetare cerere = new Model_Cerere_Resetare(email);

                    var colectie = client.GetDatabase("Application").GetCollection<Model_Cerere_Resetare>("TB_Cod_Resetare");
                    colectie.InsertOne(cerere);

                    Servicii_Trimitere_Email.Email_Resetare_Parola(cerere.email, cerere.cod_resetare, cerere.data_request);
                    return 1;
                }
                return 0;
            }


            private class Model_Cerere_Resetare
            {
                public DateTime data_request;
                public string email;
                public string cod_resetare;
                public Model_Cerere_Resetare(string email)
                {
                    this.email = email;
                    cod_resetare = Generare_Cod();
                    data_request = DateTime.UtcNow;
                }

                private static string Generare_Cod()
                {
                    string cod_resetare = string.Empty;
                    Random random = new Random();

                    for (int i = 0; i < 10; i++)
                    {
                        if (random.Next(0, 2) == 1)
                        {
                            cod_resetare += random.Next(0, 9);
                        }
                        else
                        {
                            cod_resetare += Convert.ToChar(random.Next(65, 91));
                        }
                    }
                    return cod_resetare;
                }
            }

        }


        public static class Raport_Erori
        {
            public static void Raportare(string mesaj_eroare)
            {
                var client = ConexiuneBD();
                var colectie = client.GetDatabase("Application").GetCollection<BsonDocument>("TB_Error_Logs");

                var eroare = new BsonDocument
                {
                    {"data",DateTime.Now },
                    {"eroare",mesaj_eroare }
                };

                colectie.InsertOne(eroare);
            }
        }


        //  Functii Utilitare.
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


        private static string Retur_Tabel(string email)
        {
            if (Management_UtilizatorCurent.Nume_Email(email) == "Admin" || Management_UtilizatorCurent.Nume_Email(email) == "Secretariat")
            {
                return "TB_Administrare";
            }
            else
            {
                return "TB_Profesor";
            }
        }
    }
}
