using Backend.Db_tables;

namespace Backend.Data
{
    public static class DbDataInit
    {
        public static void DataInint(ApplicationDBContext context)
        {
            context.Database.EnsureCreated();

            // Add Rollen
            if (!context.Rollen.Any())
            {
                var rollen = new Rolle[]
            {
                    new Rolle { Name = "Administrator" },
                    new Rolle { Name = "Manager"},
                    new Rolle { Name = "Benutzer"}
            };
                context.Rollen.AddRange(rollen);
                context.SaveChanges();
            }

            // Add Standorte
            if (!context.Standorte.Any())
            {
                var standort = new Standort[]
                {
                    new Standort { Ort = "Rostock" },
                    new Standort { Ort = "Berlin" },
                    new Standort { Ort = "Köln"}
                };
                context.Standorte.AddRange(standort);
                context.SaveChanges();
            }

            // Add Users
            if (!context.Users.Any())
            {
                var user = new User[]
                {
                    new User { Vorname = "Natalie", Nachname = "Derk", IdRolle = 1 },
                    new User { Vorname = "Max", Nachname = "Mustermann", IdRolle = 2},
                    new User { Vorname = "Anna", Nachname = "Müller", IdRolle = 3}
                };
                context.Users.AddRange(user);
                context.SaveChanges();
            }

            // Add Cars
            if (!context.Fahrzeuge.Any())
            {
                var standortList = context.Standorte.ToList();
                var car = new Fahrzeuge[]
                {
                    new Fahrzeuge { Marke = "BMW", Farbe = "rot", Typ = "Limousine", IdOrt = standortList[0].IdOrt },
                    new Fahrzeuge { Marke = "VW", Farbe = "blau", Typ = "Limousine", IdOrt = standortList[1].IdOrt },
                    new Fahrzeuge { Marke = "Toyta", Farbe = "weiß", Typ = "PKW", IdOrt = standortList[2].IdOrt }

                };
                context.Fahrzeuge.AddRange(car);
                context.SaveChanges();
            }
        }



    }
}