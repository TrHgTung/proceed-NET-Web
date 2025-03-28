namespace webapp.Dto
{
    public class UpdateOrderDto
    {
        public string OrderNo { get; set; }
        
        public decimal TotalAmount { get; set; }
    
        public string ItemID { get; set; }

        // public float Quantity { get; set; }
        public double Quantity { get; set; }
        
        // public float Price { get; set; }
        public double Price { get; set; }

        // public decimal Amount { get; set; }
    }
}
