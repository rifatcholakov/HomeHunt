using HomeHunt.Data.Models;
using HomeHunt.Data.Models.Enums;

namespace HomeHunt.Data
{
    public static class SeedData
    {
        public static Location[] GetLocations()
        {
            return new Location[]
            {
                new Location { Id = 1, City = "Sofia", Neighborhood = "Lozenets", PostalCode = "1164" },
                new Location { Id = 2, City = "Sofia", Neighborhood = "Mladost", PostalCode = "1750" },
                new Location { Id = 3, City = "Sofia", Neighborhood = "Lyulin", PostalCode = "1360" },
                new Location { Id = 4, City = "Plovdiv", Neighborhood = "Kapana", PostalCode = "4000" },
                new Location { Id = 5, City = "Plovdiv", Neighborhood = "Trakia", PostalCode = "4023" },
                new Location { Id = 6, City = "Varna", Neighborhood = "Chaika", PostalCode = "9010" },
                new Location { Id = 7, City = "Varna", Neighborhood = "Briz", PostalCode = "9027" },
                new Location { Id = 8, City = "Burgas", Neighborhood = "Lazur", PostalCode = "8000" },
                new Location { Id = 9, City = "Blagoevgrad", Neighborhood = "Varosha", PostalCode = "2700" }
            };
        }

        public static Agent[] GetAgents()
        {
            return new Agent[]
            {
                new Agent { Id = 1, FirstName = "Ivan", LastName = "Petrov", Email = "ivan.petrov@example.com", Phone = "+359 888 100 001", AgencyName = "Sunrise Estates" },
                new Agent { Id = 2, FirstName = "Maria", LastName = "Georgieva", Email = "maria.georgieva@example.com", Phone = "+359 888 100 002", AgencyName = "Sunrise Estates" },
                new Agent { Id = 3, FirstName = "Georgi", LastName = "Dimitrov", Email = "georgi.dimitrov@example.com", Phone = "+359 888 100 003", AgencyName = "Balkan Homes" },
                new Agent { Id = 4, FirstName = "Elena", LastName = "Stoyanova", Email = "elena.stoyanova@example.com", Phone = "+359 888 100 004", AgencyName = "Balkan Homes" },
                new Agent { Id = 5, FirstName = "Nikolay", LastName = "Ivanov", Email = "nikolay.ivanov@example.com", Phone = "+359 888 100 005", AgencyName = "Black Sea Realty" },
                new Agent { Id = 6, FirstName = "Desislava", LastName = "Koleva", Email = "desislava.koleva@example.com", Phone = "+359 888 100 006", AgencyName = "Black Sea Realty" },
                new Agent { Id = 7, FirstName = "Stefan", LastName = "Hristov", Email = "stefan.hristov@example.com", Phone = "+359 888 100 007", AgencyName = "Rhodope Properties" }
            };
        }

        public static Property[] GetProperties()
        {
            return new Property[]
            {
                new Property
                {
                    Id = 1,
                    Title = "Bright two-bedroom apartment in Lozenets",
                    Description = "Renovated apartment close to the park with a large living room, a balcony and a parking spot in the garage.",
                    Price = 185000,
                    Area = 82,
                    Bedrooms = 2,
                    Bathrooms = 1,
                    PropertyType = PropertyType.Apartment,
                    ListingType = ListingType.Sale,
                    Status = PropertyStatus.Available,
                    LocationId = 1,
                    AgentId = 1,
                    ImageUrl = "https://images.unsplash.com/photo-1522708323590-d24dbb6b0267?auto=format&fit=crop&w=800&q=70",
                    CreatedOn = new DateTime(2026, 10, 1).AddDays(-2)
                },
                new Property
                {
                    Id = 2,
                    Title = "Modern studio near the metro",
                    Description = "Fully furnished studio with a small kitchen and a new bathroom. A five minute walk to the metro station.",
                    Price = 650,
                    Area = 36,
                    Bedrooms = 1,
                    Bathrooms = 1,
                    PropertyType = PropertyType.Studio,
                    ListingType = ListingType.Rent,
                    Status = PropertyStatus.Available,
                    LocationId = 2,
                    AgentId = 1,
                    ImageUrl = "https://images.unsplash.com/photo-1493809842364-78817add7ffb?auto=format&fit=crop&w=800&q=70",
                    CreatedOn = new DateTime(2026, 10, 1).AddDays(-1)
                },
                new Property
                {
                    Id = 3,
                    Title = "Three-bedroom apartment in Mladost",
                    Description = "Large apartment on the fifth floor with a lift, two terraces and a view of Vitosha mountain.",
                    Price = 158000,
                    Area = 105,
                    Bedrooms = 3,
                    Bathrooms = 2,
                    PropertyType = PropertyType.Apartment,
                    ListingType = ListingType.Sale,
                    Status = PropertyStatus.Reserved,
                    LocationId = 2,
                    AgentId = 2,
                    CreatedOn = new DateTime(2026, 10, 1).AddDays(-9)
                },
                new Property
                {
                    Id = 4,
                    Title = "Office space in Business Park",
                    Description = "Open space office with two meeting rooms, air conditioning and four parking places.",
                    Price = 2400,
                    Area = 160,
                    Bedrooms = 0,
                    Bathrooms = 2,
                    PropertyType = PropertyType.Office,
                    ListingType = ListingType.Rent,
                    Status = PropertyStatus.Available,
                    LocationId = 2,
                    AgentId = 3,
                    ImageUrl = "https://images.unsplash.com/photo-1497366216548-37526070297c?auto=format&fit=crop&w=800&q=70",
                    CreatedOn = new DateTime(2026, 10, 1).AddDays(-12)
                },
                new Property
                {
                    Id = 5,
                    Title = "Two-bedroom apartment for rent in Lyulin",
                    Description = "Unfurnished apartment with central heating, ready to move in. Pets are allowed.",
                    Price = 520,
                    Area = 70,
                    Bedrooms = 2,
                    Bathrooms = 1,
                    PropertyType = PropertyType.Apartment,
                    ListingType = ListingType.Rent,
                    Status = PropertyStatus.Rented,
                    LocationId = 3,
                    AgentId = 4,
                    ImageUrl = "https://images.unsplash.com/photo-1554995207-c18c203602cb?auto=format&fit=crop&w=800&q=70",
                    CreatedOn = new DateTime(2026, 10, 1).AddDays(-20)
                },
                new Property
                {
                    Id = 6,
                    Title = "Loft in the heart of Kapana",
                    Description = "Creative loft in the old creative quarter of Plovdiv with high ceilings, exposed bricks and a lot of natural light.",
                    Price = 139000,
                    Area = 78,
                    Bedrooms = 1,
                    Bathrooms = 1,
                    PropertyType = PropertyType.Apartment,
                    ListingType = ListingType.Sale,
                    Status = PropertyStatus.Available,
                    LocationId = 4,
                    AgentId = 4,
                    ImageUrl = "https://images.unsplash.com/photo-1536376072261-38c75010e6c9?auto=format&fit=crop&w=800&q=70",
                    CreatedOn = new DateTime(2026, 10, 1).AddDays(-3)
                },
                new Property
                {
                    Id = 7,
                    Title = "Small shop on the main street",
                    Description = "Ground floor shop with a big window on a busy pedestrian street. Suitable for a cafe or a boutique.",
                    Price = 1100,
                    Area = 45,
                    Bedrooms = 0,
                    Bathrooms = 1,
                    PropertyType = PropertyType.Commercial,
                    ListingType = ListingType.Rent,
                    Status = PropertyStatus.Available,
                    LocationId = 4,
                    AgentId = 3,
                    ImageUrl = "https://images.unsplash.com/photo-1441986300917-64674bd600d8?auto=format&fit=crop&w=800&q=70",
                    CreatedOn = new DateTime(2026, 10, 1).AddDays(-6)
                },
                new Property
                {
                    Id = 8,
                    Title = "Sea view apartment in Chaika",
                    Description = "Two-bedroom apartment with a big terrace and a view of the sea. The building has an elevator and a guarded parking.",
                    Price = 245000,
                    Area = 95,
                    Bedrooms = 2,
                    Bathrooms = 2,
                    PropertyType = PropertyType.Apartment,
                    ListingType = ListingType.Sale,
                    Status = PropertyStatus.Available,
                    LocationId = 6,
                    AgentId = 5,
                    ImageUrl = "https://images.unsplash.com/photo-1560448204-e02f11c3d0e2?auto=format&fit=crop&w=800&q=70",
                    CreatedOn = new DateTime(2026, 10, 1).AddDays(-1)
                },
                new Property
                {
                    Id = 9,
                    Title = "Luxury house in Briz",
                    Description = "Modern house with a swimming pool, three bathrooms and a smart heating system. Quiet area near the forest.",
                    Price = 560000,
                    Area = 310,
                    Bedrooms = 5,
                    Bathrooms = 4,
                    PropertyType = PropertyType.House,
                    ListingType = ListingType.Sale,
                    Status = PropertyStatus.Reserved,
                    LocationId = 7,
                    AgentId = 5,
                    ImageUrl = "https://images.unsplash.com/photo-1613977257363-707ba9348227?auto=format&fit=crop&w=800&q=70",
                    CreatedOn = new DateTime(2026, 10, 1).AddDays(-7)
                },
                new Property
                {
                    Id = 10,
                    Title = "Apartment in Lazur near the park",
                    Description = "Two-bedroom apartment next to the Sea Garden, on the third floor of a well maintained building.",
                    Price = 128000,
                    Area = 76,
                    Bedrooms = 2,
                    Bathrooms = 1,
                    PropertyType = PropertyType.Apartment,
                    ListingType = ListingType.Sale,
                    Status = PropertyStatus.Sold,
                    LocationId = 8,
                    AgentId = 6,
                    ImageUrl = "https://images.unsplash.com/photo-1460317442991-0ec209397118?auto=format&fit=crop&w=800&q=70",
                    CreatedOn = new DateTime(2026, 10, 1).AddDays(-30)
                },
                new Property
                {
                    Id = 11,
                    Title = "Building plot in Briz",
                    Description = "Flat plot of land in a developing area with electricity and water at the border. Permission for a house up to three floors.",
                    Price = 95000,
                    Area = 600,
                    Bedrooms = 0,
                    Bathrooms = 0,
                    PropertyType = PropertyType.Land,
                    ListingType = ListingType.Sale,
                    Status = PropertyStatus.Available,
                    LocationId = 7,
                    AgentId = 5,
                    CreatedOn = new DateTime(2026, 10, 1).AddDays(-18)
                },
                new Property
                {
                    Id = 12,
                    Title = "House with mountain view in Varosha",
                    Description = "Old house in good condition, with a big yard and a view of Pirin mountain. Needs a small renovation.",
                    Price = 175000,
                    Area = 150,
                    Bedrooms = 3,
                    Bathrooms = 2,
                    PropertyType = PropertyType.House,
                    ListingType = ListingType.Sale,
                    Status = PropertyStatus.Available,
                    LocationId = 9,
                    AgentId = 7,
                    ImageUrl = "https://images.unsplash.com/photo-1464146072230-91cabc968266?auto=format&fit=crop&w=800&q=70",
                    CreatedOn = new DateTime(2026, 10, 1).AddDays(-13)
                }
            };
        }
    }
}
