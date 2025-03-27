using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapp.Models{
    [Table("tblCustomerList")]
    public class CustomerList
    {
        [Key]
        public string CustomerID { get; set; }

        [MaxLength(255)]
        public string CustomerName { get; set; }

        [MaxLength(30)]
        public string TaxCode { get; set; }

        [MaxLength(255)]
        public string Address { get; set; }
    }
}