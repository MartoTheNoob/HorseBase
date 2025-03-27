using HorseBase.Models;

namespace HorseBase.Data.DbInitializer
{
    public static class SeedHorses
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (context.horses.Any())
            {
                return; // Database has been seeded
            }

            // Seed Horses
            var horses = new List<Horse> {

             new Horse { Number = 1, BirhtYear = new DateTime(2012, 12, 4), Breed = context.breeds.Where(x => x.Name == "Mustang").FirstOrDefault(), Gender = "Male", Height = 130, Price = 50 },
             new Horse { Number = 2, BirhtYear = new DateTime(2013, 11, 4), Breed = context.breeds.Where(x => x.Name == "Quarter Horse").FirstOrDefault(), Gender = "Female", Height = 135, Price = 55 },
             new Horse { Number = 3, BirhtYear = new DateTime(2015, 8, 4), Breed = context.breeds.Where(x => x.Name == "Appaloosa").FirstOrDefault(), Gender = "Male", Height = 125, Price = 60 },
             new Horse { Number = 4, BirhtYear = new DateTime(2013, 3, 4), Breed = context.breeds.Where(x => x.Name == "PaintHorse").FirstOrDefault(), Gender = "Female", Height = 150, Price = 45 },
             new Horse { Number = 5, BirhtYear = new DateTime(2014, 5, 4), Breed = context.breeds.Where(x => x.Name == "Warmbloods").FirstOrDefault(), Gender = "Female", Height = 145, Price = 50 },

             new Horse { Number = 6, BirhtYear = new DateTime(2016, 7, 12), Breed = context.breeds.Where(x => x.Name == "Mustang").FirstOrDefault(), Gender = "Male", Height = 132, Price = 52 },
             new Horse { Number = 7, BirhtYear = new DateTime(2018, 9, 15), Breed = context.breeds.Where(x => x.Name == "Quarter Horse").FirstOrDefault(), Gender = "Female", Height = 140, Price = 58 },
             new Horse { Number = 8, BirhtYear = new DateTime(2017, 4, 20), Breed = context.breeds.Where(x => x.Name == "Appaloosa").FirstOrDefault(), Gender = "Male", Height = 128, Price = 62 },
             new Horse { Number = 9, BirhtYear = new DateTime(2019, 1, 5), Breed = context.breeds.Where(x => x.Name == "PaintHorse").FirstOrDefault(), Gender = "Female", Height = 148, Price = 48 },
             new Horse { Number = 10, BirhtYear = new DateTime(2014, 11, 30), Breed = context.breeds.Where(x => x.Name == "Warmbloods").FirstOrDefault(), Gender = "Male", Height = 142, Price = 53 },

             new Horse { Number = 11, BirhtYear = new DateTime(2015, 6, 18), Breed = context.breeds.Where(x => x.Name == "Mustang").FirstOrDefault(), Gender = "Female", Height = 134, Price = 51 },
             new Horse { Number = 12, BirhtYear = new DateTime(2016, 3, 22), Breed = context.breeds.Where(x => x.Name == "Quarter Horse").FirstOrDefault(), Gender = "Male", Height = 137, Price = 56 },
             new Horse { Number = 13, BirhtYear = new DateTime(2018, 8, 10), Breed = context.breeds.Where(x => x.Name == "Appaloosa").FirstOrDefault(), Gender = "Female", Height = 127, Price = 61 },
             new Horse { Number = 14, BirhtYear = new DateTime(2017, 12, 25), Breed = context.breeds.Where(x => x.Name == "PaintHorse").FirstOrDefault(), Gender = "Male", Height = 149, Price = 47 },
             new Horse { Number = 15, BirhtYear = new DateTime(2013, 2, 14), Breed = context.breeds.Where(x => x.Name == "Warmbloods").FirstOrDefault(), Gender = "Female", Height = 144, Price = 54 },

             new Horse { Number = 16, BirhtYear = new DateTime(2019, 5, 1), Breed = context.breeds.Where(x => x.Name == "Mustang").FirstOrDefault(), Gender = "Male", Height = 131, Price = 50 },
             new Horse { Number = 17, BirhtYear = new DateTime(2016, 10, 28), Breed = context.breeds.Where(x => x.Name == "Quarter Horse").FirstOrDefault(), Gender = "Female", Height = 138, Price = 57 },
             new Horse { Number = 18, BirhtYear = new DateTime(2015, 7, 19), Breed = context.breeds.Where(x => x.Name == "Appaloosa").FirstOrDefault(), Gender = "Male", Height = 126, Price = 63 },
             new Horse { Number = 19, BirhtYear = new DateTime(2018, 4, 11), Breed = context.breeds.Where(x => x.Name == "PaintHorse").FirstOrDefault(), Gender = "Female", Height = 147, Price = 46 },
             new Horse { Number = 20, BirhtYear = new DateTime(2014, 9, 3), Breed = context.breeds.Where(x => x.Name == "Warmbloods").FirstOrDefault(), Gender = "Male", Height = 143, Price = 55 },

             new Horse { Number = 21, BirhtYear = new DateTime(2017, 2, 23), Breed = context.breeds.Where(x => x.Name == "Mustang").FirstOrDefault(), Gender = "Female", Height = 133, Price = 52 },
             new Horse { Number = 22, BirhtYear = new DateTime(2019, 11, 17), Breed = context.breeds.Where(x => x.Name == "Quarter Horse").FirstOrDefault(), Gender = "Male", Height = 139, Price = 59 },
             new Horse { Number = 23, BirhtYear = new DateTime(2016, 1, 29), Breed = context.breeds.Where(x => x.Name == "Appaloosa").FirstOrDefault(), Gender = "Female", Height = 129, Price = 64 },
             new Horse { Number = 24, BirhtYear = new DateTime(2015, 3, 8), Breed = context.breeds.Where(x => x.Name == "PaintHorse").FirstOrDefault(), Gender = "Male", Height = 146, Price = 49 },
             new Horse { Number = 25, BirhtYear = new DateTime(2018, 6, 21), Breed = context.breeds.Where(x => x.Name == "Warmbloods").FirstOrDefault(), Gender = "Female", Height = 141, Price = 56 }
            };
            context.horses.AddRange(horses);
            context.SaveChanges();
        }
    }
}
