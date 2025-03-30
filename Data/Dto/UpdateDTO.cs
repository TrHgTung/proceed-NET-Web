namespace webapp.Dto
{
    // public class UpdateOrderDto
    // {
    //     public string OrderNo { get; set; }
        
    //     public decimal TotalAmount { get; set; }
    
    //     public string ItemID { get; set; }

    //     // public float Quantity { get; set; }
    //     public double Quantity { get; set; }
        
    //     // public float Price { get; set; }
    //     public double Price { get; set; }

    //     // public decimal Amount { get; set; }
    // }

    public class UpdateOrderDto // update 1 dong item
    {   
        public string ItemID { get; set; }

        public double Quantity { get; set; }
        
        public double Price { get; set; }

        // public string CustomerID { get; set; }
    }

    public class UpdateOMDto  // update lai toan bo item cua hoa don (OrderMaster)
    {   
        public string CustomerID { get; set; }
    }
}
