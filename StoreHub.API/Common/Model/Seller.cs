namespace StoreHub.API.Common.Model
{
    public class Seller
    {
        public int SellerId { get; set; }
        public int UserId { get; set; }
        public string SellerName { get; set; }
        public string SellerAddress { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        // Used to hide extra mapping
        [System.Text.Json.Serialization.JsonIgnore]
        public User User { get; set; }
    }

    public class SellerResponse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; internal set; }
        public IEnumerable<Seller> Sellers { get; set; }
    }

    public class SellerMapping
    {
        public int SellerMappingId { get; set; }
        public int SellerId { get; set; }
        public int ProductId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public Seller Seller { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public Product Product { get; set; }
    }
}
