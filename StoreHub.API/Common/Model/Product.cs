namespace StoreHub.API.Common.Model
{
    public class AddProduct
    {
        //public int ProductId { get; set; }
        //[Required(ErrorMessage = "UserName is mandatory field.")]
        public string ProductName { get; set; }
        //[Required]
        //[RegularExpression("^[0-9a-zA-Z]+([._+-]?[0-9a-zA-Z]+)*@[0-9a-zA-Z]+.[a-zA-Z]{2,4}([.][a-zA-Z]{2,3})?$")] //, ErrorMessage = 'EmailAddr is not in correct format.')
        public string Description { get; set; }
        public string Brand { get; set; }
        //public string SKU { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public string ImageUrl { get; set; }
        //public bool IsActive { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
    }

    public class UpdateProduct
    {       
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
    }

    public class GetProductById
    {
        public int ProductId { get; set; }
    }

    public class DeleteProduct
    {
        public int ProductId { get; set; }
        public bool IsActive { get; set; }
    }

    public class Product
    {
        public int ProductId { get; set; }
        //[Required(ErrorMessage = "UserName is mandatory field.")]
        public string ProductName { get; set; }
        //[Required]
        //[RegularExpression("^[0-9a-zA-Z]+([._+-]?[0-9a-zA-Z]+)*@[0-9a-zA-Z]+.[a-zA-Z]{2,4}([.][a-zA-Z]{2,3})?$")] //, ErrorMessage = 'EmailAddr is not in correct format.')
        public string Description { get; set; }
        public string Brand { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class Response
    {
        public string Message { get; set; }
        public bool IsSuccess { get; internal set; }
        public List<Product> Products { get; set; }
    }
}
