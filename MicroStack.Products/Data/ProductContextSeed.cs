using MicroStack.Products.Entities;
using MongoDB.Driver;

namespace MicroStack.Products.Data
{
    public class ProductContextSeed
    {
        public static void SeedData(IMongoCollection<Product> product)
        {
            bool existProduct = product.Find(p => true).Any();
            if (!existProduct)
            {
                product.InsertManyAsync(GetConfigureProducts());
            }
        }
        private static IEnumerable<Product> GetConfigureProducts()
        {
            var products = new List<Product>
            {
                new Product
                {
                    Name = "Apple iPhone 15 Pro",
                    Category = "Smartphone",
                    Summary = "Son teknoloji akıllı telefon",
                    Description = "Apple'ın A17 Pro çipiyle gelen, titanyum kasaya sahip, güçlü kamerası olan üst seviye akıllı telefon.",
                    ImageFile = "iphone15pro.png",
                    Price = 49999.99M
                },
                new Product
                {
                    Name = "Samsung Galaxy Tab S9",
                    Category = "Tablet",
                    Summary = "Yüksek performanslı Android tablet",
                    Description = "Snapdragon 8 Gen 2 ile güçlendirilmiş, AMOLED ekranlı, suya dayanıklı Android tablet.",
                    ImageFile = "galaxytabs9.png",
                    Price = 27999.99M
                },
                new Product
                {
                    Name = "Dell XPS 13 Plus",
                    Category = "Laptop",
                    Summary = "Şık ve güçlü ultrabook",
                    Description = "Intel i7 işlemcili, 16GB RAM ve 1TB SSD ile gelen, ince ve hafif Windows laptop.",
                    ImageFile = "dellxps13plus.png",
                    Price = 54999.00M
                },
                new Product
                {
                    Name = "Sony WH-1000XM5",
                    Category = "Headphones",
                    Summary = "Gelişmiş gürültü engelleyici kulaklık",
                    Description = "Sony'nin amiral gemisi kablosuz kulaklığı, uzun batarya ömrü ve üstün ses kalitesi ile öne çıkar.",
                    ImageFile = "sonywh1000xm5.png",
                    Price = 12999.50M
                },
                new Product
                {
                    Name = "Logitech MX Master 3S",
                    Category = "Mouse",
                    Summary = "Profesyonel kablosuz mouse",
                    Description = "Sessiz tıklama özelliği ve akıcı kaydırma tekerleği ile verimlilik odaklı bir mouse.",
                    ImageFile = "mxmaster3s.png",
                    Price = 2499.90M
                }
            };

            return products;
        }
    }
}
