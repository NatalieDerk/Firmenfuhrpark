using Backend.Db_tables;

namespace Backend.Data
{
    public static class DbRolleInit
    {
        public static void AddRolle(ApplicationDBContext context)
        {
            context.Database.EnsureCreated();

            if (context.Rollen.Any())
                return;

            var rollen = new Rolle[]
            {
                    new Rolle { Name = "Administrator" },
                    new Rolle { Name = "Manager"},
                    new Rolle { Name = "Benutzer"}
            };

            foreach (var r in rollen)
                context.Rollen.Add(r);

            context.SaveChanges();
        }
    }
}