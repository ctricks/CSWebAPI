using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SampleWebAPI.Models;

namespace SampleWebAPI.Services
{
    public class UsersService
    {
        private readonly IMongoCollection<Users>? _usersCollection;

        public UsersService(IOptions<UserBoxDatabaseSettings> userBoxDatabaseSettings)
        {
            var mongoClient = new MongoClient(
               userBoxDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                userBoxDatabaseSettings.Value.DatabaseName
                );
            _usersCollection = mongoDatabase.GetCollection<Users>(
                userBoxDatabaseSettings.Value.UsersCollectionName);
        }
        public async Task<List<Users>> GetUsersAsync() =>
            await _usersCollection.Find(_ => true).ToListAsync();

        public async Task<Users?> GetUserAsync(string username) =>
            await _usersCollection.Find(x => x.UserName == username).FirstOrDefaultAsync();

        public async Task<Users?> GetUserIDAsync(string id) =>
            await _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Users newUsers) =>
            await _usersCollection.InsertOneAsync(newUsers);

        public async Task UpdateAsync(string id, Users updatedUser) =>
            await _usersCollection.ReplaceOneAsync(x => x.Id == id, updatedUser);

        public async Task RemoveAsync(string id) =>
            await _usersCollection.DeleteOneAsync(x => x.Id == id);
    }
}
