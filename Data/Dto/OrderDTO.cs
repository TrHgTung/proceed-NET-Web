namespace webapp.Dto
{
    public class OrderDto
    {
        public string ItemID { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public decimal Amount { get; set; }
        public string CustomerID { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
