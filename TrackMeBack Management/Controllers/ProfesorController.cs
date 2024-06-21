using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using TrackMeBack_Management.Clase_Utilitare.Management;
using TrackMeBack_Management.Models.Orar;
using TrackMeBack_Management.Models.Prezente;

namespace TrackMeBack_Management.Controllers
{
    public class ProfesorController : Controller
    {
        public IActionResult Home()
        {
            string materia = string.Empty;

            if(Redirect_Profesor(out materia))
            {
                return View();
            }
            else { return RedirectToAction("Nimic_de_Afisat"); }
            
        }

        public IActionResult Management_Studenti()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Management_Studenti(Notare_Model notare)
        {
            return View();
        }

        public IActionResult Nimic_de_Afisat() { return View(); }

        private static bool Redirect_Profesor(out string materia)
        {
            string marca = Management_UtilizatorCurent.Profesor.Marca();
            var client = Management_MongoDB.ConexiuneBD();

            var filtru_marca = Builders<BsonDocument>.Filter.Eq("marca",marca);

            var discipline_profesor = client.GetDatabase("Discipline").GetCollection<BsonDocument>("TB_INFO").Find(filtru_marca).ToList();

            foreach(var disciplina in discipline_profesor)
            {
                string disciplina_tabel = disciplina.GetValue("materie").AsString;
                var filtru_materie_activa = Builders<BsonDocument>.Filter.Eq("materia", disciplina_tabel);

                var discipline_active = client.GetDatabase("Discipline").GetCollection<BsonDocument>("TB_DISCIPLINE_ACTIVE").Find(filtru_marca).ToList();

                foreach(var disciplina_activa in discipline_active)
                {
                    if(disciplina_tabel==disciplina_activa.GetValue("materie").AsString)
                    {
                        materia = disciplina_activa.GetValue("materie").AsString;
                        return true;
                    }
                }
            }
            materia = null;
            return false;
        }

    }
}
