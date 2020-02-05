using System;
using System.ComponentModel.DataAnnotations;

namespace KTInsta.Entities
{
    public class KTInstaCards :BaseModel
    {
        [Key]
        public int CardId { get; set; }
        public string CardHeadLine { get; set; }
        public string CardText { get; set; }
        public int CardType { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
