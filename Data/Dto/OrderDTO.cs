namespace webapp.Dto
{
    public class OrderDto
    {
        // public class OrderDetailsDto
        // {
            public string ItemID { get; set; }

            public int Quantity { get; set; }

            public float Price { get; set; }
            
            public int Amount { get; set; }
        // }

        // public class OrderMasterDto
        // {
            public string CustomerID { get; set; }

            public float TotalAmount { get; set; }
        // }
    }
}
