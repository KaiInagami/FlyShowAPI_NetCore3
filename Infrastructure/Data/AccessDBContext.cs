using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

using MongoDB.Driver;
using System;

namespace Infrastructure.Data
{
    public class AccessDBContext : DbContext
    {
        #region NOSQL
        

        //public class TokenModel
        //{
        //    public string ObjectId { get; set; }
        //    [BsonElement("Token")]
        //    public string Token { get; set; }

        //    [BsonElement("Account")]
        //    public string Account { get; set; }

        //    [BsonElement("CurrentTime")]
        //    public string CurrentTime { get; set; }

        //    [BsonElement("CurrentTime")]
        //    public DateTime ExpireTime { get; set; }

        //    [BsonElement("IsValid")]
        //    public bool IsValid { get; set; }
        //}

        //public TokenModel GetTokenProperties(string token)
        //{
        //    MongoClient _client = new MongoClient("mongodb://localhost:27017");
        //    MongoServer _server = _client.GetServer();
        //    MongoDatabase _db = _server.GetDatabase("Tokens");

        //    var res = MongoDB.Driver.Builders.Query<TokenModel>.EQ(model => model.Token, token);
        //    return _db.GetCollection<TokenModel>("Tokens").FindOne(res);
        //}

        //public bool DisableTokenStatus(string token)
        //{
        //    MongoClient _client = new MongoClient("mongodb://localhost:27017");
        //    MongoServer _server = _client.GetServer();
        //    MongoDatabase _db = _server.GetDatabase("Tokens");

        //    var res = MongoDB.Driver.Builders.Query<TokenModel>.EQ(model => model.Token, token);
        //    var dbEntity = _db.GetCollection<TokenModel>("Tokens").FindOne(res);
        //    dbEntity.IsValid = false;
        //    var operation = MongoDB.Driver.Builders.Update<TokenModel>.Replace(dbEntity);
        //    var result = _db.GetCollection<TokenModel>("Tokens").Update(res, operation);
        //    return !result.HasLastErrorMessage;
        //}
        #endregion

        #region MSSQL
        public DbSet<Advertise> Advertise { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<SpecialContractStore> SpecialContractStore { get; set; }
        public DbSet<SysUser> SysUser { get; set; }
        public DbSet<User> User { get; set; }

        public AccessDBContext(DbContextOptions<AccessDBContext> options) : base(options) 
        {   
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                var connectionString = "Server=localhost;Database=VegetableStore;Trusted_Connection=True;MultipleActiveResultSets=true;";

                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Advertise>().Property(x => x.ID).IsRequired();
            modelBuilder.Entity<Advertise>().Property(x => x.ResourceUrl).IsRequired().HasMaxLength(500);
            modelBuilder.Entity<Advertise>().Property(x => x.Link).HasMaxLength(500);
            modelBuilder.Entity<Advertise>().Property(x => x.StartDate).IsRequired();
            modelBuilder.Entity<Advertise>().Property(x => x.EndDate).IsRequired();
            modelBuilder.Entity<Advertise>().ToTable("Advertise");

            modelBuilder.Entity<Product>().Property(x => x.ID).IsRequired();
            modelBuilder.Entity<Product>().Property(x => x.SerialNo).IsRequired().HasMaxLength(15);
            modelBuilder.Entity<Product>().Property(x => x.Image).HasMaxLength(500);
            modelBuilder.Entity<Product>().Property(x => x.Price);
            modelBuilder.Entity<Product>().Property(x => x.Unit).HasMaxLength(50);
            modelBuilder.Entity<Product>().Property(x => x.Inventory);
            modelBuilder.Entity<Product>().Property(x => x.Remark).HasMaxLength(1000);
            modelBuilder.Entity<Product>().Property(x => x.Area).HasMaxLength(50);
            modelBuilder.Entity<Product>().Property(x => x.CreateDate);
            modelBuilder.Entity<Product>().Property(x => x.IsInStock);
            modelBuilder.Entity<Product>().Property(x => x.Type);
            modelBuilder.Entity<Product>().Property(x => x.YouTubeUrl).HasMaxLength(500);
            modelBuilder.Entity<Product>().ToTable("Product");

            modelBuilder.Entity<SpecialContractStore>().Property(x => x.ID).IsRequired();
            modelBuilder.Entity<SpecialContractStore>().Property(x => x.Name).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<SpecialContractStore>().ToTable("SpecialContractStore");

            modelBuilder.Entity<SysUser>().Property(x => x.ID).IsRequired();
            modelBuilder.Entity<SysUser>().Property(x => x.Name).HasMaxLength(50);
            modelBuilder.Entity<SysUser>().Property(x => x.Account).HasMaxLength(50);
            modelBuilder.Entity<SysUser>().Property(x => x.Password).HasMaxLength(100);
            modelBuilder.Entity<SysUser>().ToTable("SysUser");

            modelBuilder.Entity<User>().Property(x => x.ID).IsRequired();
            modelBuilder.Entity<User>().Property(x => x.Name).HasMaxLength(50);
            modelBuilder.Entity<User>().Property(x => x.Gender);
            modelBuilder.Entity<User>().Property(x => x.Phone).HasMaxLength(50);
            modelBuilder.Entity<User>().Property(x => x.Birthday);
            modelBuilder.Entity<User>().Property(x => x.Email).HasMaxLength(500);
            modelBuilder.Entity<User>().Property(x => x.Address).HasMaxLength(500);
            modelBuilder.Entity<User>().Property(x => x.Password).HasMaxLength(500);
            modelBuilder.Entity<User>().Property(x => x.Priority);
            modelBuilder.Entity<User>().ToTable("User");
        }
        #endregion
    }
}
