using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TrackMeBack_Management.Models.Prezente
{
    public class Notare_Model
    {
        [BsonId] 
        public ObjectId Id { get; set; }

        public int nota { get; set; }

        public string marca { get; set; } = string.Empty;

        public int index { get; set; }

    }
}
