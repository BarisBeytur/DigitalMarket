using DigitalMarket.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalMarket.Data.Domain
{
    [Table("Category", Schema = "dbo")]
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Tags { get; set; }

        public virtual ICollection<Product> Products { get; set; }

    }
}
