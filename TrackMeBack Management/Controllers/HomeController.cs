using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TrackMeBack_Management.Clase_Utilitare.Management;
using TrackMeBack_Management.Models;
using TrackMeBack_Management.Models.Autentificare;
using TrackMeBack_Management.Clase_Utilitare.Servicii;

namespace TrackMeBack_Management.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //  Autentificarea.
        public IActionResult Autentificare()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Autentificare(Autentificare_Model date_utilizator)
        {
            if (ModelState.IsValid)
            {
                //  Verificrea datelor de Autentificare.
                bool utilizator_existent = Management_MongoDB.Autentificare.Acces(date_utilizator.email, date_utilizator.parola);

                if (utilizator_existent)
                {
                    //  Repartizarea Utilizatorului în functie de rol.
                    switch (Management_UtilizatorCurent.Nume_Email(date_utilizator.email))
                    {
                        case "Admin":
                            return RedirectToAction("Home", "Administrator");

                        case "Secretariat":
                            Management_UtilizatorCurent.Set_Secretariat(date_utilizator.email);
                            return RedirectToAction("Home", "Secretariat");

                        default:
                            Management_UtilizatorCurent.Set_Info_Prof(date_utilizator.email);
                            return RedirectToAction("Home", "Profesor");
                    }
                }
                else 
                {
                    //  Anunțarea Greșelilor de autentificare.
                    TempData["Mesaj_Eroare"] = "Email-ul sau Parola sunt incorecte!";
                    return View(); 
                }
                
            }
            else
            {
               Management_MongoDB.Raport_Erori.Raportare("ModelState - Invalid : HomeController -> Autentificare");
               return RedirectToAction("Error");
            }
            
        }

        //  Resetarea Parolei.
        public IActionResult Resetare_Parola()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Resetare_Parola(Resetare_Parola_Model date_utilizator)
        {
            if (date_utilizator.cod_reset == "-" || Management_MongoDB.Autentificare.Verificare_Cerere(date_utilizator.email) == false)
            {
                ViewData["cod_trimis"] = true;
                TempData["Mesaj_Avertisment"] = "Codul de resetare a fost trimis!";
                Management_MongoDB.Autentificare.Cerere_Resetare_Parola(date_utilizator.email);
                return View();
            }
            else if (ModelState.IsValid)
            {
                int cod_eroare = Management_MongoDB.Autentificare.Resetare_Parola(date_utilizator.email, date_utilizator.parola_nou, date_utilizator.cod_reset);

                switch (cod_eroare)
                {
                    default:
                        TempData["Mesaj_Info"] = "Parola a fost resetată cu succes!";

                        Servicii_Trimitere_Email.Email_Informare_Parola(date_utilizator.email,date_utilizator.parola_nou);

                        return RedirectToAction("Autentificare");
                    case -1:
                        TempData["Mesaj_Eroare"] = "Codul introdus este greșit!";
                        return View();
                    case -2:
                        Management_MongoDB.Raport_Erori.Raportare("Eroare - Resetare_Parola : Management_MongoDB -> Resetare_Parola");
                        return RedirectToAction("Error");
                }
            }
            else
            {
                Management_MongoDB.Raport_Erori.Raportare("ModelState - Invalid : HomeController -> Resetare_Parola");
                return RedirectToAction("Error");
            }
        }

        //  Pagina de Erori.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
