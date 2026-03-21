namespace StoreHub.API.Common.Model
{
    public class Sale
    {
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public decimal SalePrice { get; set; }
        public int Discount { get; set; }
        public int SaleStock { get; set; }
        public bool OnSale { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        // Navigation property (optional, for EF relationships)
        public Product Product { get; set; }
    }

    public class ProductWithSale
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public decimal SalePrice { get; set; }
        public int Discount { get; set; }
        public bool OnSale { get; set; }

        public string Description { get; set; }
        public string SKU { get; set; }
        public int CategoryId { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public int SaleStock { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }

    public class ProductWithSaleResponse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public IEnumerable<ProductWithSale> Products { get; set; }
    }
}
