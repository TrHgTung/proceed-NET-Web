using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapp.Models{
    [Table("tblItemList")]
    public class ItemList
    {
        [Key]
        public string ItemID { get; set; }

        [MaxLength(255)]
        public string ItemName { get; set; }

        [MaxLength(10)]
        public string InvUnitOfMeasr { get; set; }

    }
}