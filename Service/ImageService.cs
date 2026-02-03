namespace Backend.Service
{
    using Backend.Model;
    using MongoDB.Driver;

    public class ImageService
    {
        private readonly IMongoCollection<ImageModel> _collection;

        public ImageService(IMongoClient client, IConfiguration config)
        {
            var db = client.GetDatabase(config["MongoDB:DatabaseName"]);
            _collection = db.GetCollection<ImageModel>("Images");
        }

        public async Task InsertAsync(ImageModel image) 
        {
            await _collection.InsertOneAsync(image);
        }

        public async Task UpdateImageUrlAsync(string id, string imageUrl)
        {
            var filter = Builders<ImageModel>.Filter.Eq(x => x.Id, id);
            var update = Builders<ImageModel>.Update.Set(x => x.ImageUrl, imageUrl);

            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task<ImageModel> GetByIdAsync(string id)
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }
    }

}
