using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapp.Models{
    [Table("tblOrderMaster")]
    public class OrderMaster
    {
        [Key]
        public int OrderMasterID { get; set; }

        [MaxLength(20)]
        public string OrderDate { get; set; }

        public int OrderNo { get; set; }

        [MaxLength(255)]
        public string CustomerID { get; set; }

        public float TotalAmount { get; set; }
        
        public string DivSubID { get; set; }

    }
}