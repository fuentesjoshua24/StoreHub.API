namespace StoreHub.API.Common.Model
{
    public class Promotion
    {
        public int PromotionId { get; set; }
        //[Required(ErrorMessage = "UserName is mandatory field.")]
        public string PromotionName { get; set; }
        //[Required]
        //[RegularExpression("^[0-9a-zA-Z]+([._+-]?[0-9a-zA-Z]+)*@[0-9a-zA-Z]+.[a-zA-Z]{2,4}([.][a-zA-Z]{2,3})?$")] //, ErrorMessage = 'EmailAddr is not in correct format.')
        public string Description { get; set; }
        public string Brand { get; set; }
        public string SKU { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
    public class PromotionResponse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; internal set; }
        public IEnumerable<Promotion> Promotions { get; set; }
    }
}
