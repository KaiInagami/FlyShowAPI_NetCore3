using Infrastructure.Data;
using Infrastructure.Entities;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Infrastructure.Repositories
{
    public class TokenRepository //: ITokenRepository
    {
        private readonly IMongoCollection<TokenModel> _tokens;

        public TokenRepository(IMonoConnectionSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.Database);

            _tokens = database.GetCollection<TokenModel>(settings.Table);
        }

        public List<TokenModel> GetTokenList() =>
            _tokens.Find(tok => true).ToList();

        public object GetOneToken(string token) =>
          _tokens.Find(tok => tok.Token == token).FirstOrDefault();

        public void RemoveToken(string token) =>
            _tokens.DeleteOne(tok => tok.Token == token);

        //public void Update(string token, Book bookIn) =>
        //    _tokens.ReplaceOne(book => book.Id == id, bookIn);

        //public Book Get(string id) =>
        //    _books.Find<Book>(book => book.Id == id).FirstOrDefault();

        //public Book Create(Book book)
        //{
        //    _books.InsertOne(book);
        //    return book;
        //}



        //public void Remove(Book bookIn) =>
        //    _books.DeleteOne(book => book.Id == bookIn.Id);


        //public TokenModel GetTokenPropertiesByToken(string token)
        //{
        //    var result = new TokenModel();

        //    try
        //    {
        //        result = _dbContext.GetTokenProperties(token);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("MongoDB run faild");
        //    }
        //    return result;
        //}

        //public bool DisableTokenStatus(string token)
        //{
        //    try
        //    {
        //       return _dbContext.DisableTokenStatus(token);
        //    }
        //    catch (Exception)
        //    {
        //        throw new Exception("MongoDB run faild");
        //    }
        //}
    }
}
