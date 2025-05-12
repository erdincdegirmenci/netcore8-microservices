using MicroStack.Sourcing.Entitites;
using MongoDB.Driver;

namespace MicroStack.Sourcing.Data
{
    public class SourcingContextSeed
    {
        public static void SeedData(IMongoCollection<Auction> auctionCollection)
        {
            bool exist = auctionCollection.Find(p => true).Any();
            if (!exist)
            {
                auctionCollection.InsertManyAsync(GetConfigureAuctions());
            }
        }
        private static IEnumerable<Auction> GetConfigureAuctions()
        {
            var auctions = new List<Auction>
            {
                new Auction
                {
                    Name = "Auction 1",
                    Description = "Elektronik ürünler için ilkbahar kampanyası",
                    ProductId = "P1001",
                    Quantity = 150,
                    StartedAt = new DateTime(2025, 3, 5),
                    FinishedAt = new DateTime(2025, 3, 20),
                    CreatedAt = DateTime.Now,
                    Status = 1,
                    IncludedSellers = new List<string> { "SellerX", "SellerY" }
                },
                new Auction
                {
                    Name = "Auction 2",
                    Description = "Yaz sezonu başlangıç indirimi",
                    ProductId = "P1002",
                    Quantity = 75,
                    StartedAt = new DateTime(2025, 6, 10),
                    FinishedAt = new DateTime(2025, 6, 25),
                    CreatedAt = DateTime.Now,
                    Status = 1,
                    IncludedSellers = new List<string> { "SellerA" }
                },
                new Auction
                {
                    Name = "Auction 3",
                    Description = "Yeni ürün tanıtımı için özel teklif",
                    ProductId = "P1003",
                    Quantity = 200,
                    StartedAt = new DateTime(2025, 5, 1),
                    FinishedAt = new DateTime(2025, 5, 7),
                    CreatedAt = DateTime.Now,
                    Status = 0,
                    IncludedSellers = new List<string> { "SellerB", "SellerC" }
                },
                new Auction
                {
                    Name = "Auction 4",
                    Description = "Sonbahar fırsat kampanyası",
                    ProductId = "P1004",
                    Quantity = 90,
                    StartedAt = new DateTime(2025, 9, 15),
                    FinishedAt = new DateTime(2025, 10, 5),
                    CreatedAt = DateTime.Now,
                    Status = 1,
                    IncludedSellers = new List<string> { "SellerD" }
                },
                new Auction
                {
                    Name = "Auction 5",
                    Description = "Yıl sonu özel kampanya",
                    ProductId = "P1005",
                    Quantity = 110,
                    StartedAt = new DateTime(2025, 12, 10),
                    FinishedAt = new DateTime(2025, 12, 31),
                    CreatedAt = DateTime.Now,
                    Status = 2,
                    IncludedSellers = new List<string> { "SellerE", "SellerF" }
                }
            };

            return auctions;
        }
    }

}