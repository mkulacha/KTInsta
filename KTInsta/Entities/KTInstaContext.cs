using System.Data.Entity;
using KTInsta.Entities.Mapping;


namespace KTInsta.Entities
{
    internal class KTInstaContext : DbContext
    {

        public KTInstaContext()
            : base("name=KTInstaContext")
        {
            Database.CommandTimeout = 240;
        }

        static KTInstaContext()
        {
            Database.SetInitializer<KTInstaContext>(null);
        }

        public DbSet<KTInstaCards> KTInstaCards { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new KTInstaCardsMap());
        }
    }
}


