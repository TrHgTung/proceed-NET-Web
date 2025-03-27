using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapp.Models{
    [Table("tblOrderDetail")]
    public class OrderDetail
    {
        [Key]
        public int RowDetailID { get; set; }

        public int OrderMasterID { get; set; }

        [MaxLength(20)]
        public string LineNumber { get; set; }

        public string ItemID { get; set; }

        public int Quantity { get; set; }

        public float Price { get; set; }
        
        public int Amount { get; set; }

    }
}