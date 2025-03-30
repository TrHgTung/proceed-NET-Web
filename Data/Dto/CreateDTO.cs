namespace webapp.Dto
{
    public class CreateDTO
    {

        public class OrderDetail
        {
            // public Guid OrderMasterID { get; set; }
            public string ItemID { get; set; }
            public int Quantity { get; set; }
            public float Price { get; set; }
        }

        public class OrderMaster
        {
            // public string ItemID { get; set; }
            // public int Quantity { get; set; }
            // public float Price { get; set; }
            // public decimal Amount { get; set; }
            public string CustomerID { get; set; }
            // public decimal TotalAmount { get; set; }
        }
    }
}
