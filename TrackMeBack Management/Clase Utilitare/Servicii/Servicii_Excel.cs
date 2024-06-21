using ExcelDataReader;
using MimeKit.Tnef;
using System.Text;
using TrackMeBack_Management.Models.Discipline;
using TrackMeBack_Management.Models.Harta;
using TrackMeBack_Management.Models.Orar;
using TrackMeBack_Management.Models.Utilizator;

namespace TrackMeBack_Management.Clase_Utilitare.Servicii
{
    public class Servicii_Excel
    {
        public static List<List<object>> Citire(string fisier)
        {
            //Stocarea Datelor din Excel.
            List<List<object>> date_excel = new List<List<object>>();

            //Deschiderea fisierului Excel in modul de citire.
            var stream = File.Open(fisier, FileMode.Open, FileAccess.Read);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var reader = ExcelReaderFactory.CreateReader(stream, new ExcelReaderConfiguration
            {
                FallbackEncoding = Encoding.GetEncoding(1252),
            });

            //Preluarea fiecărui rând din fișier.
            do
            {
                while (reader.Read())
                {
                    List<object> date_rand = new List<object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        date_rand.Add(reader.GetValue(i));
                    }
                    date_excel.Add(date_rand);
                }
            } while (reader.NextResult());

            reader.Close();
            stream.Close();

            return date_excel;
        }

        public static List<Sala_Model> Serializare_Sala()
        {

            List<Sala_Model> lista_model = new List<Sala_Model>();

            List<List<object>> date_excel = Citire("Models\\Fisiere_Excel\\Excel_Harta.xlsx");
            
            if (date_excel[0].Contains("CLADIRE") && date_excel[0].Contains("CORP") && date_excel[0].Contains("INDEX"))
            {
                date_excel.RemoveAt(0);

                foreach (var sala in date_excel)
                {
                    if (sala[0]!=null && sala[2]!=null)
                    {
                        if (sala[1] == null)
                        {
                            Sala_Model model = new Sala_Model(sala[0].ToString().ToUpper(), null, sala[2].ToString().ToUpper());
                            lista_model.Add(model);
                        }
                        else
                        {
                            Sala_Model model = new Sala_Model(sala[0].ToString().ToUpper(), sala[1].ToString().ToUpper(), sala[2].ToString().ToUpper());
                            lista_model.Add(model);
                        }
                    }
                }
                return lista_model;
            }
            else
            {
                return null;
            }
        }

        public static List<Discipline_Model> Serializare_Disciplina()
        {
            List<Discipline_Model> lista_model = new List<Discipline_Model>();

            List<List<object>> date_excel = Citire("Models\\Fisiere_Excel\\Excel_Discipline.xlsx");

            if (date_excel[0].Contains("MATERIA") && date_excel[0].Contains("SPECIALIZAREA") && date_excel[0].Contains("ANUL") && date_excel[0].Contains("SEMESTRUL") && date_excel[0].Contains("PROF CURS") && date_excel[0].Contains("PROF LAB"))
            {
                date_excel.RemoveAt(0);

                foreach (var disciplina in date_excel)
                {
                    bool verificare_valori = true;
                    for (int i = 0; i < 6;i++)
                    {
                        if (disciplina[i]==null)
                        {
                            verificare_valori = false;
                        }
                    }
                    if(verificare_valori)
                    {
                        double an = (double)disciplina[2];
                        double semestru = (double)disciplina[3];
                        if (disciplina[6] == null)
                        {
                            Discipline_Model model = new Discipline_Model(disciplina[0].ToString(), disciplina[1].ToString(),(int)an, (int)semestru, disciplina[4].ToString(), disciplina[5].ToString(), null);
                            lista_model.Add(model);
                        }
                        else
                        {
                            Discipline_Model model = new Discipline_Model(disciplina[0].ToString(), disciplina[1].ToString(), (int)an, (int)semestru, disciplina[4].ToString(), disciplina[5].ToString(), disciplina[6].ToString());
                            lista_model.Add(model);
                        }
                    }    
                }
                return lista_model;
            }
            else
            {
                return null;
            }
        }

        public static List<Orar_Model> Serializare_Orar()
        {
            List<Orar_Model> lista_model = new List<Orar_Model>();

            List<List<object>> date_excel = Citire("Models\\Fisiere_Excel\\Excel_Orar.xlsx");

            if (date_excel[0].Contains("MATERIA") && date_excel[0].Contains("SALA") && date_excel[0].Contains("ZIUA") && date_excel[0].Contains("ORA_START") && date_excel[0].Contains("GRUPA"))
            {
                date_excel.RemoveAt(0);

                foreach (var orar in date_excel)
                {
                    bool verificare_valori = true;
                    for (int i = 0; i < 5; i++)
                    {
                        if (orar[i] == null)
                        {
                            verificare_valori = false;
                        }
                    }
                    if (verificare_valori)
                    {
                        double grupa = (double)orar[4];

                            Orar_Model model = new Orar_Model(orar[0].ToString(), orar[1].ToString(), orar[2].ToString(), orar[3].ToString(),(int)grupa);
                            lista_model.Add(model);
                    }
                }
                return lista_model;
            }
            else
            {
                return null;
            }
        }

        public static List<Profesor_Model> Serializare_Profesor()
        {
            List<Profesor_Model> lista_prof = new List<Profesor_Model>();

            List<List<object>> date_excel = Citire("Models\\Fisiere_Excel\\Excel_Profesor.xlsx");
            if (date_excel[0].Contains("NUME") && date_excel[0].Contains("PRENUME") && date_excel[0].Contains("PAROLA") && date_excel[0].Contains("FACULTATE"))
            {
                date_excel.RemoveAt(0);

                foreach (var profesor in date_excel)
                {
                    if (profesor[0]!=null && profesor[1] != null && profesor[2] != null && profesor[3] != null)
                    {
                        Profesor_Model profesor_nou = new Profesor_Model(profesor[0].ToString(), profesor[1].ToString(), profesor[2].ToString(), profesor[3].ToString().ToUpper());
                        lista_prof.Add(profesor_nou);
                    }
                }
                return lista_prof;
            }
            else
            {
                return null;
            }
        }

        public static List<Student_Model> Serializare_Student()
        {
            List<Student_Model> lista_student = new List<Student_Model>();

            List<List<object>> date_excel = Citire("Models\\Fisiere_Excel\\Excel_Student.xlsx");
            if (date_excel[0].Contains("NUME") && date_excel[0].Contains("PRENUME") && date_excel[0].Contains("PAROLA") && date_excel[0].Contains("GRUPA") && date_excel[0].Contains("AN") && date_excel[0].Contains("SPECIALIZARE") && date_excel[0].Contains("FACULTATE"))
            {
                date_excel.RemoveAt(0);

                int counter = 0;
                foreach (var student in date_excel)
                {
                    if (student[0] != null && student[1] != null && student[2] != null && student[3] != null && student[4] != null && student[5] != null && student[6] != null)
                    { 
                        int grupa = Convert.ToInt32(student[2]);
                        int anul = Convert.ToInt32(student[3]);
                        Student_Model student_nou = new Student_Model(student[0].ToString(), student[1].ToString(),grupa, anul,student[4].ToString().ToUpper(),student[5].ToString().ToUpper(), student[6].ToString());
                        
                        if(counter<=9)
                        {
                            student_nou.marca = student_nou.marca.Remove(student_nou.marca.Length - 1);
                            student_nou.marca = student_nou.marca + counter;
                        }
                        else
                        {
                            student_nou.marca = student_nou.marca.Remove(student_nou.marca.Length - 2);
                            student_nou.marca = student_nou.marca + counter;
                        }
                        counter++;

                        lista_student.Add(student_nou);
                    }
                }
                return lista_student;
            }
            else
            {
                return null;
            }
        }
    }
}
