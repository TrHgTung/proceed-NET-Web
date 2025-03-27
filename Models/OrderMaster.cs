using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapp.Models{
    [Table("tblOrderMaster")]
    public class OrderMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid OrderMasterID { get; set; }

        [MaxLength(20)]
        public DateTime OrderDate { get; set; }

        public string OrderNo { get; set; }

        [MaxLength(255)]
        public string CustomerID { get; set; }

        public decimal TotalAmount { get; set; }
        
        public string DivSubID { get; set; }

    }
}