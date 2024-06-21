using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TrackMeBack_Management.Clase_Utilitare.Management;
using TrackMeBack_Management.Models;
using TrackMeBack_Management.Models.Harta;
using TrackMeBack_Management.Models.Orar;
using TrackMeBack_Management.Models.Utilizator;

namespace TrackMeBack_Management.Controllers
{
    public class SecretariatController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }

        public IActionResult Orar_UPT()
        { 
            return View(); 
        }

        [HttpPost]
        public IActionResult Orar_UPT(Editare_Orar_Model orar)
        {
            if(ModelState.IsValid)
            {
                Orar_Model orar_nou = new Orar_Model(orar.materia,orar.sala,orar.ziua,orar.ora_start,orar.grupa);
                int cod_eroare_manual = Management_MongoDB.Orar_UPT.Editare(orar_nou,orar.actiune);
                switch (cod_eroare_manual)
                {
                    default:
                        TempData["Mesaj_Info"] = "Orar modificat cu succes!";
                        return View();
                    case -1:
                        TempData["Mesaj_Eroare"] = "Orar existent!";
                        return View();
                    case -2:
                        TempData["Mesaj_Eroare"] = "Orar inexistent!";
                        return View();
                    case -3:
                        TempData["Mesaj_Eroare"] = "Ceva nu a funcționat corespunzător!";
                        return View();
                }
            }
            else
            {
                Management_MongoDB.Raport_Erori.Raportare("ModelState - Invalid : SecretariatController -> Orar_UPT");
                return RedirectToAction("Error");
            }
        }


        public IActionResult Student_UPT()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Student_UPT(Editare_Student_Model student)
        {
            if (ModelState.IsValid)
            {

                switch (Verificare_Model_Student(student))
                {
                    default:
                        Student_Model student_nou = new Student_Model(student.nume, student.prenume, student.grupa, student.an_Studiu, student.facultate, student.specializare, student.parola);
                        int cod_editare = Management_MongoDB.Utilizator.Editare_Cont_Student(student.marca, student_nou, student.actiune);
                        switch (cod_editare)
                        {
                            default:
                                TempData["Mesaj_Info"] = "Date adăugate/modificate cu succes!";
                                return View();
                            case -1:
                                TempData["Mesaj_Eroare"] = "Utilizatorul există deja!";
                                return View();
                            case -2:
                                Management_MongoDB.Raport_Erori.Raportare("Eroare : Management_MongoDB -> Editare_Cont_Student() : Cod eroare -2");
                                return RedirectToAction("Error");
                        }
                    case 0:
                        Student_Model student_blank = new Student_Model("Blank", "Blank", 1, 3, "AC", "SPEC","PAR");
                        int cod_stergere = Management_MongoDB.Utilizator.Editare_Cont_Student(student.marca, student_blank, student.actiune);
                        switch (cod_stergere)
                        {
                            default:
                                TempData["Mesaj_Info"] = "Date șterse cu succes!";
                                return View();
                            case -2:
                                Management_MongoDB.Raport_Erori.Raportare("Eroare : Management_MongoDB -> Editare_Cont_Student() : Cod eroare -2");
                                return RedirectToAction("Error");
                            case -3:
                                TempData["Mesaj_Eroare"] = "Utilizatorul nu există!";
                                return View();
                        }
                    case -1:
                        TempData["Mesaj_Eroare"] = "Pentru Adăugare/Editare toate datele trebuie completate!";
                        return View();
                    case -2:
                        TempData["Mesaj_Eroare"] = "Pentru Ștergere Marca trebuie selectată";
                        return View();
                }
            }
            else
            {
                Management_MongoDB.Raport_Erori.Raportare("ModelState - Invalid : SecretariatController -> Student_UPT");
                return RedirectToAction("Error");
            }
        }


        public IActionResult Adaugare_Student_Excel()
        {
            int cod_eroare_excel = Management_MongoDB.Adaugare_Excel.Adaugare_Student();
            if (cod_eroare_excel == 0)
            {
                TempData["Mesaj_Info"] = "Date Studenți modificate cu succes!";
                return RedirectToAction("Student_UPT");
            }
            TempData["Mesaj_Eroare"] = "Ceva nu a funcționat corespunzător!";
            return RedirectToAction("Student_UPT");
        }

        //  Pagina de Erori.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        private static int Verificare_Model_Student(Editare_Student_Model student)
        {
            switch (student.actiune)
            {
                default:
                    if (student.nume == null || student.prenume == null || student.specializare == null || student.parola == null) return -1;
                    break;
                case "Stergere":
                    if (student.marca == null) return -2; else return 0;
            }

            return 1;
        }

        public IActionResult Adaugare_Orar_Excel()
        {
            int cod_eroare_excel = Management_MongoDB.Adaugare_Excel.Adaugare_Orar();
            if (cod_eroare_excel == 0)
            {
                TempData["Mesaj_Info"] = "Date Orar modificate cu succes!";
                return RedirectToAction("Orar_UPT");
            }
            TempData["Mesaj_Eroare"] = "Ceva nu a funcționat corespunzător!";
            return RedirectToAction("Orar_UPT");
        }
    }
}
