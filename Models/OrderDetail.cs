using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapp.Models{
    [Table("tblOrderDetail")]
    public class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid RowDetailID { get; set; } // OrderDetail khoa chinh

        public Guid OrderMasterID { get; set; }

        [MaxLength(20)]
        public int LineNumber { get; set; }

        public string ItemID { get; set; }

        public float Quantity { get; set; }

        public float Price { get; set; }
        
        public decimal Amount { get; set; }

    }
}