using WEB_253502_Chertok.Domain.Entities;

namespace WEB_253502_Chertok.API.Data
{
	public class DbInitializer
	{
		public static async Task SeedData(WebApplication app)
		{
			var baseUrl = app.Configuration.GetSection("baseUrl").Value;

			List<Category> Categories = new List<Category>
			{
				new Category { Name = "Phone", NormalizedName = "phone" },
				new Category { Name = "Headphones", NormalizedName = "headphones" },
				new Category { Name = "Laptop", NormalizedName = "laptop" },
				new Category { Name = "TV", NormalizedName = "tv" },
			};
			List<Product> Products = new List<Product>
			{
				new Product
				{
					Id = 1,
					Name = "Xiaomi Redmi Note 13",
					Description = "Android, экран 6.67\" AMOLED (1080x2400) 120 Гц, Qualcomm Snapdragon 685, ОЗУ 8 ГБ, память 256 ГБ, поддержка карт памяти, камера 108 Мп, аккумулятор 5000 мАч, 2 SIM (nano-SIM), влагозащита IP54",
					Price = 800,
					Category = Categories[0],
					CategoryId = 1,
					Image = $"{baseUrl}/images/phone1.png"
				},
				new Product
				{
					Id = 2,
					Name = "Apple iPhone 15",
					Description = "Apple iOS, экран 6.1\" OLED (1179x2556) 60 Гц, Apple A16 Bionic, ОЗУ 6 ГБ, память 128 ГБ, камера 48 Мп, аккумулятор 3349 мАч, 1 SIM (nano-SIM/eSIM), влагозащита IP68",
					Price = 3500,
					Category = Categories[0],
					CategoryId = 1,
					Image = $"{baseUrl}/images/phone2.png"
				},
				new Product
				{
					Id = 3,
					Name = "HONOR Earbuds X6",
					Description = "беспроводные наушники с микрофоном, вставные, портативные, полностью беспроводные (TWS), Bluetooth 5.3, 20-20000 Гц, нет, быстрая зарядка, время работы 9 ч, с кейсом 40 ч",
					Price = 85,
					Category = Categories[1],
					CategoryId = 2,
					Image = $"{baseUrl}/images/headphones1.png"
				},
				new Product
				{
					Id = 4,
					Name = "Apple AirPods Pro 2",
					Description = "беспроводные наушники с микрофоном, внутриканальные, портативные, полностью беспроводные (TWS), Bluetooth 5.3, нет, быстрая зарядка, время работы 6 ч, с кейсом 30 ч, активное шумоподавление",
					Price = 880,
					Category = Categories[1],
					CategoryId = 2,
					Image = $"{baseUrl}/images/headphones2.png"
				},
				new Product
				{
					Id = 5,
					Name = "ASUS Vivobook Go 15",
					Description = "15.6\" 1920 x 1080, IPS, 60 Гц, AMD Ryzen 5 7520U, 16 ГБ LPDDR5, SSD 512 ГБ, видеокарта встроенная, без ОС, цвет крышки серебристый, аккумулятор 42 Вт·ч",
					Price = 1770,
					Category = Categories[2],
					CategoryId = 3,
					Image = $"{baseUrl}/images/laptop1.png"
				},
				new Product
				{
					Id = 6,
					Name = "Xiaomi RedmiBook Pro 15",
					Description = "15.6\" 3200 x 2000, IPS, 120 Гц, AMD Ryzen 7 7840HS, 16 ГБ LPDDR5, SSD 512 ГБ, видеокарта встроенная, цвет крышки серый, аккумулятор 72 Вт·ч",
					Price = 3000,
					Category = Categories[2],
					CategoryId = 3,
					Image = $"{baseUrl}/images/laptop2.png"
				},
				new Product
				{
					Id = 7,
					Name = "Xiaomi TV A 55\"",
					Description = "55\" 3840x2160 (4K UHD), матрица IPS, частота матрицы 60 Гц, Smart TV (Android TV), HDR10, HLG, DTS:HD, DTS:X, Wi-Fi, смарт пульт",
					Price = 1700,
					Category = Categories[3],
					CategoryId = 4,
					Image = $"{baseUrl}/images/tv1.png"
				},
				new Product
				{
					Id = 8,
					Name = "LG OLED C4 OLED55C4RLA",
					Description = "55\" 3840x2160 (4K UHD), матрица OLED (WOLED), частота матрицы 144 Гц, Smart TV (LG webOS), HDR10, Dolby Vision, Dolby Vision IQ, HLG, DTS:X, Dolby Atmos, AirPlay, Wi-Fi, смарт пульт",
					Price = 5000,
					Category = Categories[3],
					CategoryId = 4,
					Image = $"{baseUrl}/images/tv2.png"
				}
			};

			using var scope = app.Services.CreateScope();

			var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

			await context.Categories.AddRangeAsync(Categories);
			await context.Products.AddRangeAsync(Products);

			await context.SaveChangesAsync();
		}
	}
}
