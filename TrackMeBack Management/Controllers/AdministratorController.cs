using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TrackMeBack_Management.Clase_Utilitare.Management;
using TrackMeBack_Management.Clase_Utilitare.Servicii;
using TrackMeBack_Management.Models;
using TrackMeBack_Management.Models.Autentificare;
using TrackMeBack_Management.Models.Discipline;
using TrackMeBack_Management.Models.Harta;
using TrackMeBack_Management.Models.Utilizator;

namespace TrackMeBack_Management.Controllers
{
    public class AdministratorController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }

        // ------------------------------------- Administrarea Conturilor
        public IActionResult Conturi_Admin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Conturi_Admin(Editare_Admin_Model date_autentificare)
        {
            if (ModelState.IsValid)
            {
                int cod_eroare = Management_MongoDB.Utilizator.Editare_Cont_Administrare(date_autentificare.email,date_autentificare.actiune, date_autentificare.parola_nou);
                switch (cod_eroare)
                {
                    default:
                        TempData["Mesaj_Info"] = "Date modificate cu succes!";
                        Servicii_Trimitere_Email.Email_Informare_Parola(date_autentificare.email,date_autentificare.parola_nou);
                        return View();
                    case -1:
                        TempData["Mesaj_Eroare"] = "Utilizator existent!";
                        return View();
                    case -2:
                        Management_MongoDB.Raport_Erori.Raportare("Eroare - AdministratorController : Management_MongoDB -> Resetare_Parola_Admin");
                        return RedirectToAction("Error");
                    case -3:
                        TempData["Mesaj_Eroare"] = "Nu se poate șterge contul Administratorului!";
                        return View();
                }
            }
            else
            {
                Management_MongoDB.Raport_Erori.Raportare("ModelState - Invalid : AdministratorController -> Conturi_Admin");
                return RedirectToAction("Error");
            }
        }


        // ------------------------------------- Administrarea Materiilor
        public IActionResult Discipline_UPT()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Discipline_UPT(Editare_Discipline_Model disciplina)
        {
            if(ModelState.IsValid)
            {
                switch (Verificare_Model_Disciplina(disciplina))
                {
                    default:
                        Discipline_Model disciplina_noua = new Discipline_Model(disciplina.materie,disciplina.specializare,disciplina.an,disciplina.semestru,disciplina.profesor_curs,disciplina.profesor_laborator,disciplina.profesor_laborator_2);
                        int cod_editare = Management_MongoDB.Discipline_UPT.Editare_Disciplina(disciplina_noua,disciplina.actiune);
                        switch (cod_editare)
                        {
                            default:
                                TempData["Mesaj_Info"] = "Date adăugate/modificate cu succes!";
                                return View();
                            case -1:
                                TempData["Mesaj_Eroare"] = "Disciplina există deja!";
                                return View();
                            case -2:
                                Management_MongoDB.Raport_Erori.Raportare("Eroare : Management_MongoDB -> Editare_Disciplina() : Cod eroare -2");
                                return RedirectToAction("Error");
                            case -3:
                                TempData["Mesaj_Eroare"] = "Semestru invalid!";
                                return View();
                        }
                    case 0:
                        Discipline_Model disciplina_gol = new Discipline_Model(disciplina.materie, disciplina.specializare,1, 1, "Blank", "Blank", "Blank");
                        int cod_stergere = Management_MongoDB.Discipline_UPT.Editare_Disciplina(disciplina_gol,disciplina.actiune);
                        switch (cod_stergere)
                        {
                            default:
                                TempData["Mesaj_Info"] = "Date șterse cu succes!";
                                return View();
                            case -2:
                                Management_MongoDB.Raport_Erori.Raportare("Eroare : Management_MongoDB -> Editare_Disciplina() : Cod eroare -2");
                                return RedirectToAction("Error");
                            case -3:
                                TempData["Mesaj_Eroare"] = "Disciplina nu există!";
                                return View();
                        }
                    case -1:
                        TempData["Mesaj_Eroare"] = "Pentru Adăugare/Editare toate datele trebuie completate!";
                        return View();
                    case -2:
                        TempData["Mesaj_Eroare"] = "Pentru Ștergere doar Materia trebuie selectată";
                        return View();
                }
            }
            else
            {
                Management_MongoDB.Raport_Erori.Raportare("ModelState - Invalid : AdministratorController -> Discipline_UPT");
                return RedirectToAction("Error");
            }
        }


        // ------------------------------------- Administrarea Profesorilor
        public IActionResult Profesori_UPT()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Profesori_UPT(Editare_Profesor_Model profesor)
        {
            if(ModelState.IsValid)
            {

                switch (Verificare_Model_Profesor(profesor))
                {
                    default:
                        Profesor_Model profesor_nou = new Profesor_Model(profesor.nume, profesor.prenume, profesor.parola, profesor.facultate);
                        int cod_editare = Management_MongoDB.Utilizator.Editare_Cont_Profesor(profesor.marca, profesor_nou, profesor.actiune);
                        switch (cod_editare)
                        {
                            default:
                                TempData["Mesaj_Info"] = "Date adăugate/modificate cu succes!";
                                return View();
                            case -1:
                                TempData["Mesaj_Eroare"] = "Utilizatorul există deja!";
                                return View();
                            case -2:
                                Management_MongoDB.Raport_Erori.Raportare("Eroare : Management_MongoDB -> Editare_Cont_Profesor() : Cod eroare -2");
                                return RedirectToAction("Error");
                        }
                    case 0:
                        Profesor_Model profesor_blank = new Profesor_Model("Blank", "Blank", "Parola_Blank_123", "AC");
                        int cod_stergere = Management_MongoDB.Utilizator.Editare_Cont_Profesor(profesor.marca, profesor_blank, profesor.actiune);
                        switch (cod_stergere)
                        {
                            default:
                                TempData["Mesaj_Info"] = "Date șterse cu succes!";
                                return View();
                            case -2:
                                Management_MongoDB.Raport_Erori.Raportare("Eroare : Management_MongoDB -> Editare_Cont_Profesor() : Cod eroare -2");
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
                Management_MongoDB.Raport_Erori.Raportare("ModelState - Invalid : AdministratorController -> Profesori_UPT");
                return RedirectToAction("Error");
            }
        }


        // ------------------------------------- Administrarea Clădirilor
        public IActionResult Harta_UPT()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Harta_UPT(Editare_Admin_Sala_Model sala_edit)
        {
            if (ModelState.IsValid)
            {
                    Sala_Model sala = new Sala_Model(sala_edit.cladire, sala_edit.corp, sala_edit.index);
                    int cod_eroare_manual = Management_MongoDB.Harta_UPT.Editare_Sala(sala, sala_edit.actiune);
                switch(cod_eroare_manual)
                {
                    default:
                        TempData["Mesaj_Info"] = "Sală modificată cu succes!";
                        return View();
                    case -1:
                        TempData["Mesaj_Eroare"] = "Sală existentă!";
                        return View();
                    case -2:
                        TempData["Mesaj_Eroare"] = "Sală inexistentă!";
                        return View();
                    case -3:
                        TempData["Mesaj_Eroare"] = "Ceva nu a funcționat corespunzător!";
                        return View();
                }
            }
            else 
            {
                Management_MongoDB.Raport_Erori.Raportare("ModelState - Invalid : AdministratorController -> Harta_UPT");
                return RedirectToAction("Error");
            }
        }


        public IActionResult Adaugare_Sala_Excel()
        {
            int cod_eroare_excel = Management_MongoDB.Adaugare_Excel.Adaugare_Sala();
            if (cod_eroare_excel == 0)
            {
                TempData["Mesaj_Info"] = "Sală modificată cu succes!";
                return RedirectToAction("Harta_UPT");
            }
            TempData["Mesaj_Eroare"] = "Ceva nu a funcționat corespunzător!";
            return RedirectToAction("Harta_UPT");
        }


        public IActionResult Adaugare_Profesor_Excel()
        {
            int cod_eroare_excel = Management_MongoDB.Adaugare_Excel.Adaugare_Profesor();
            if (cod_eroare_excel == 0)
            {
                TempData["Mesaj_Info"] = "Date Profesori modificate cu succes!";
                return RedirectToAction("Profesori_UPT");
            }
            TempData["Mesaj_Eroare"] = "Ceva nu a funcționat corespunzător!";
            return RedirectToAction("Profesori_UPT");
        }


        public IActionResult Adaugare_Disciplina_Excel()
        {
            int cod_eroare_excel = Management_MongoDB.Adaugare_Excel.Adaugare_Disciplina();
            if (cod_eroare_excel == 0)
            {
                TempData["Mesaj_Info"] = "Date Discipline modificate cu succes!";
                return RedirectToAction("Discipline_UPT");
            }
            TempData["Mesaj_Eroare"] = "Ceva nu a funcționat corespunzător!";
            return RedirectToAction("Discipline_UPT");
        }


        //  Pagina de Erori.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //  Funcții utilitare:
        private static int Verificare_Model_Profesor(Editare_Profesor_Model profesor)
        {
            switch (profesor.actiune) 
            {
                default:
                    if (profesor.nume == null || profesor.prenume == null || profesor.facultate == null || profesor.parola == null) return -1;
                    break;
                case "Stergere":
                    if (profesor.marca == null) return -2;else return 0 ;
            }

            return 1;
        }

        public static int Verificare_Model_Disciplina(Editare_Discipline_Model disciplina) 
        {
            switch (disciplina.actiune) 
            { 
                default :
                    if (disciplina.materie==null || disciplina.semestru==null || disciplina.specializare==null || disciplina.profesor_curs==null || disciplina.profesor_laborator==null) return -1;
                        break;
                case "Stergere": if (disciplina.materie == null || disciplina.specializare==null) return -2;else return 0 ;
            }
            return 1;
        }
    }
}
