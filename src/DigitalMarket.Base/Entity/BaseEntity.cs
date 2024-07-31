using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalMarket.Base.Entity
{
    public class BaseEntity
    {
        [System.ComponentModel.DataAnnotations.Key]
        public long Id { get; set; }
        public string InsertUser { get; set; }
        public DateTime InsertDate { get; set; }
        public bool IsActive { get; set; }
    }
}
