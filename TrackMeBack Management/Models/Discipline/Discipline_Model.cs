using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TrackMeBack_Management.Models.Discipline
{
    public class Discipline_Model
    {
        [BsonId]
        public BsonObjectId _id { get; set; }
        public string materie { get; set; } = string.Empty;


        public string specializare { get; set; } = string.Empty;


        public int an { get; set; }


        public int semestru { get; set; }


        public string profesor_curs { get; set; } = string.Empty;


        public string profesor_laborator { get; set; } = string.Empty;


        public string? profesor_laborator_2 { get; set; }

        public Discipline_Model(string materie, string specializare,int an, int semestru, string prof_c,string prof_l,string prof_l2)
        {
            this.materie = materie;
            this.specializare = specializare;
            this.semestru = semestru;
            profesor_curs = prof_c;
            profesor_laborator = prof_l;
            profesor_laborator_2 = prof_l2;
            this.an=an;
        }

    }
}
