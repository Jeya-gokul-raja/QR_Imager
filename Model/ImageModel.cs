namespace Backend.Model
{


    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class ImageModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string ImageUrl { get; set; }
    }

}