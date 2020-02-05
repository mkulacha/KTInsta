using System.Data.Entity.ModelConfiguration;

namespace KTInsta.Entities.Mapping
{
    public class KTInstaCardsMap : EntityTypeConfiguration<KTInstaCards>
    {
        public KTInstaCardsMap()
        {
            this.ToTable("KTInstaCards");
            this.Property(t => t.CardId).HasColumnName("CardId");
            this.Property(t => t.CardHeadLine).HasColumnName("CardHeadLine");
            this.Property(t => t.CardText).HasColumnName("CardText");
            this.Property(t => t.CardType).HasColumnName("CardType");
            this.Property(t => t.DateCreated).HasColumnName("DateCreated");
            Ignore(e => e.DateCreated);
        }

    }
}


       
