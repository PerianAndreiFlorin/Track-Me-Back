using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackMeBack_Mobile.ClaseUtilitare;

namespace Track_Me_Back___App.ClaseUtilitare
{
    class Directii_Sala
    {
        public static void Directie(string sala_curenta, string sala_orar, out string mesaj)
        {
            string cladire_curenta,corp_curent,index_curent;
            string cladire_orar,corp_orar,index_orar;

            Separare_Info_Sala(sala_curenta, out cladire_curenta, out corp_curent, out index_curent);
            Separare_Info_Sala(sala_orar, out cladire_orar, out corp_orar, out index_orar);

            if(cladire_orar == cladire_curenta)
            {
                if (corp_orar == corp_curent)
                {
                    if (index_orar == index_curent)
                    {
                        mesaj = "Ai ajuns la destinație!";
                    }
                    else
                    {
                        mesaj = "Clădirea este în apropiere, la acest etaj!";
                    }
                }
                else
                {
                    mesaj = "Trebuie să vă deplasați spre corpul: " + corp_orar;
                }
            }
            else
            {
                mesaj = "GoogleMaps";
                string link = Link_Cladire(cladire_orar);
                Google_Maps(link);
            }
        }

        private static void Separare_Info_Sala(string sala,out string cladire, out string corp, out string index)
        {
            int index_prim=0, index_doi=0;
            int counter = 0;
            foreach (char c in sala) 
            { 
                counter++;
                if (c == '_')
                {
                    if(index_prim == 0)
                    {
                        index_prim=counter;
                    }
                    else { index_doi = counter; }
                }
            }
            cladire = sala.Substring(0,index_prim-1);

            if(index_doi == 0)
            {
                corp = "-";
                index = sala.Substring(index_prim);
            }
            else
            {
                corp = sala.Substring(index_prim, (index_doi - 1) - index_prim);
                index = sala.Substring(index_doi);
            }
        }


        private static async void Google_Maps(string link)
        {
            try
            {
                Uri uri = new Uri(link);
                await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                // Eroare de la browser
            }
        }


        public static string Link_Cladire(string cladire)
        {
            var client = Servicii_MongoDB.ConexiuneBD();
            string tabel = "TB_"+cladire;
            var colectie = client.GetDatabase("HartaCampusUPT").GetCollection<BsonDocument>(tabel);
            var filtru_id = Builders<BsonDocument>.Filter.Eq("_id", 0);
            var rezultat = colectie.Find(filtru_id).FirstOrDefault();

            return rezultat.GetValue("locatie").AsString;
        }
    }

}
