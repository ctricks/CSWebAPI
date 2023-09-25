using CSWebAPI.Helpers;
using CSWebAPI.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using SampleWebAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SampleWebAPI.Services
{
    public class UsersService
    {
        private readonly IMongoCollection<Users>? _usersCollection;

        private readonly AppSettings _appSettings;

        public UsersService(IOptions<UserBoxDatabaseSettings> userBoxDatabaseSettings, IOptions<AppSettings> appSettings)
        {
            var mongoClient = new MongoClient(
               userBoxDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                userBoxDatabaseSettings.Value.DatabaseName
                );
            _usersCollection = mongoDatabase.GetCollection<Users>(
                userBoxDatabaseSettings.Value.UsersCollectionName);

            _appSettings = appSettings.Value;
        }

        public AuthenticateResponse CreateToken(Users user)
        {
            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        private string generateJwtToken(Users user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
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
