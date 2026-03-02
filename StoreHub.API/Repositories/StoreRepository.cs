using MySql.Data.MySqlClient;
using StoreHub.API.Common.Model;
using StoreHub.API.CommonUtility;

namespace StoreHub.API.Repositories
{
    public class StoreRepository : IStoreRepository
    {
        private readonly IConfiguration _configuration;
        public readonly ILogger<StoreRepository> _logger;

        //Copilot
        private readonly string _connectionString;

        private readonly MySqlConnection _MySqlConnection;

        public StoreRepository(IConfiguration configuration, ILogger<StoreRepository> logger)
        {

            _configuration = configuration;

            //Copilot
            _connectionString = _configuration.GetConnectionString("MySqlDBConnectionString")
                                ?? _configuration["ConnectionStrings:MySqlDBConnectionString"]
                                ?? throw new InvalidOperationException("Connection string 'MySqlDBConnectionString' not configured.");

            _logger = logger;
            // initialize shared connection using resolved connection string
            _MySqlConnection = new MySqlConnection(_connectionString);
        }

        public async Task<Response> AddProduct(AddProduct request)
        {
            _logger.LogInformation("AddProduct is Calling in Repository");
            var response = new Response();

            // Validate SQL
            string sqlText;
            try
            {
                sqlText = SqlQueries.AddProduct;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"SQL load failed: {ex.Message}";
                return response;
            }

            if (string.IsNullOrWhiteSpace(sqlText))
            {
                response.IsSuccess = false;
                response.Message = "AddProduct SQL is not configured.";
                return response;
            }

            try
            {
                if (_MySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _MySqlConnection.OpenAsync();
                }

                using (MySqlCommand sqlCommand = new MySqlCommand(sqlText, _MySqlConnection))
                {
                    try
                    {
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        sqlCommand.CommandTimeout = 180;

                        // Parameters for AddProduct (product fields)
                        sqlCommand.Parameters.AddWithValue("@ProductName", request.ProductName ?? string.Empty);
                        sqlCommand.Parameters.AddWithValue("@Description", request.Description ?? string.Empty);
                        sqlCommand.Parameters.AddWithValue("@Brand", request.Brand ?? string.Empty);
                        //sqlCommand.Parameters.AddWithValue("@SKU", request.SKU ?? string.Empty);
                        sqlCommand.Parameters.AddWithValue("@Price", request.Price);
                        sqlCommand.Parameters.AddWithValue("@Stock", request.Stock);
                        sqlCommand.Parameters.AddWithValue("@CategoryId", request.CategoryId);
                        sqlCommand.Parameters.AddWithValue("@ImageUrl", request.ImageUrl ?? string.Empty);
                        //sqlCommand.Parameters.AddWithValue("@IsActive", request.IsActive);
                        //sqlCommand.Parameters.AddWithValue("@CreatedDate", request.CreatedDate);
                        //sqlCommand.Parameters.AddWithValue("@UpdatedDate", request.UpdatedDate);


                        int status = await sqlCommand.ExecuteNonQueryAsync();
                        if (status <= 0)
                        {
                            response.IsSuccess = false;
                            response.Message = "Query not executed.";
                            _logger.LogError("Error occur: Query not executed.");
                            return response;
                        }

                        response.IsSuccess = true;
                        response.Message = "Product added successfully.";
                    }
                    catch (Exception ex)
                    {
                        response.IsSuccess = false;
                        response.Message = ex.Message;
                        _logger.LogError($"Error occur: {ex.Message}");
                        return response;
                    }
                    finally
                    {
                        //if (_MySqlConnection.State == System.Data.ConnectionState.Open)
                        //{
                        await _MySqlConnection.CloseAsync();
                        await _MySqlConnection.DisposeAsync();
                        //}
                    }                
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                _logger.LogError($"Error occur: {ex.Message}");
                return response;
            }
            return response;
        }

        public async Task<Response> GetProduct()
        {
            _logger.LogInformation("GetProduct is Calling in Repository");
            Response response = new Response();
            response.IsSuccess = true;
            response.Message = "Product is fetched successfully.";

            // Validate SQL
            string sqlText;
            try
            {
                sqlText = SqlQueries.GetProduct;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"SQL load failed: {ex.Message}";
                return response;
            }

            if (string.IsNullOrWhiteSpace(sqlText))
            {
                response.IsSuccess = false;
                response.Message = "GetProduct SQL is not configured.";
                return response;
            }

            try
            {
                if (_MySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _MySqlConnection.OpenAsync();
                }

                using (MySqlCommand sqlCommand = new MySqlCommand(sqlText, _MySqlConnection))
                {
                    try
                    {
                        // CommandText already set by constructor above; set CommandType instead
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        sqlCommand.CommandTimeout = 180;

                        // No parameters for GetProduct (returns all active products)
                        using var dataReader = await sqlCommand.ExecuteReaderAsync();
                        {
                            if (dataReader.HasRows)
                            {
                                response.Products = new List<Product>();
                                while (await dataReader.ReadAsync())
                                {
                                    Product getProduct = new Product();
                                    getProduct.ProductId = dataReader["ProductId"] != DBNull.Value ? Convert.ToInt32(dataReader["ProductId"]) : 0;
                                    getProduct.ProductName = dataReader["ProductName"] != DBNull.Value ? Convert.ToString(dataReader["ProductName"]) : string.Empty;
                                    getProduct.Description = dataReader["Description"] != DBNull.Value ? Convert.ToString(dataReader["Description"]) : string.Empty;
                                    getProduct.Brand = dataReader["Brand"] != DBNull.Value ? Convert.ToString(dataReader["Brand"]) : string.Empty;
                                    getProduct.SKU = dataReader["SKU"] != DBNull.Value ? Convert.ToString(dataReader["SKU"]) : string.Empty;
                                    getProduct.Price = dataReader["Price"] != DBNull.Value ? Convert.ToDecimal(dataReader["Price"]) : 0m;
                                    getProduct.Stock = dataReader["Stock"] != DBNull.Value ? Convert.ToInt32(dataReader["Stock"]) : 0;
                                    getProduct.CategoryId = dataReader["CategoryId"] != DBNull.Value ? Convert.ToInt32(dataReader["CategoryId"]) : 0;
                                    getProduct.ImageUrl = dataReader["ImageUrl"] != DBNull.Value ? Convert.ToString(dataReader["ImageUrl"]) : string.Empty;
                                    getProduct.IsActive = dataReader["IsActive"] != DBNull.Value ? Convert.ToBoolean(dataReader["IsActive"]) : false;
                                    getProduct.CreatedDate = dataReader["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dataReader["CreatedDate"]) : DateTime.MinValue;
                                    getProduct.UpdatedDate = dataReader["UpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dataReader["UpdatedDate"]) : DateTime.MinValue;

                                    response.Products.Add(getProduct);
                                }
                            }
                            else
                            {
                                response.IsSuccess = true;
                                response.Message = "No products found.";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        response.IsSuccess = false;
                        response.Message = $"SQL load failed: {ex.Message}";
                        _logger.LogError("Error occur: Query not executed.");
                        return response;
                    }
                    finally
                    {
                        await sqlCommand.DisposeAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                _logger.LogError($"Error occur: {ex.Message}");
            }

            return response;
        }

        public async Task<Response> GetProductById(GetProductById request)
        {
            _logger.LogInformation("GetProductById is Calling in Repository");
            Response response = new Response();
            response.IsSuccess = true;
            response.Message = "Product is fetched successfully.";

            // Validate SQL
            string sqlText;
            try
            {
                sqlText = SqlQueries.GetProductById;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"SQL load failed: {ex.Message}";
                return response;
            }

            if (string.IsNullOrWhiteSpace(sqlText))
            {
                response.IsSuccess = false;
                response.Message = "GetProduct SQL is not configured.";
                return response;
            }

            try
            {
                if (_MySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _MySqlConnection.OpenAsync();
                }

                using (MySqlCommand sqlCommand = new MySqlCommand(sqlText, _MySqlConnection))
                {
                    try
                    {
                        // CommandText already set by constructor above; set CommandType instead
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        sqlCommand.CommandTimeout = 180;
                        // Add parameter for ProductId so the query returns the specific product
                        sqlCommand.Parameters.AddWithValue("@ProductId", request.ProductId);

                        using var dataReader = await sqlCommand.ExecuteReaderAsync();
                        {
                            if (dataReader.HasRows)
                            {
                                response.Products = new List<Product>();
                                while (await dataReader.ReadAsync())
                                {
                                    Product getProduct = new Product();
                                    getProduct.ProductId = dataReader["ProductId"] != DBNull.Value ? Convert.ToInt32(dataReader["ProductId"]) : 0;
                                    getProduct.ProductName = dataReader["ProductName"] != DBNull.Value ? Convert.ToString(dataReader["ProductName"]) : string.Empty;
                                    getProduct.Description = dataReader["Description"] != DBNull.Value ? Convert.ToString(dataReader["Description"]) : string.Empty;
                                    getProduct.Brand = dataReader["Brand"] != DBNull.Value ? Convert.ToString(dataReader["Brand"]) : string.Empty;
                                    getProduct.SKU = dataReader["SKU"] != DBNull.Value ? Convert.ToString(dataReader["SKU"]) : string.Empty;
                                    getProduct.Price = dataReader["Price"] != DBNull.Value ? Convert.ToDecimal(dataReader["Price"]) : 0m;
                                    getProduct.Stock = dataReader["Stock"] != DBNull.Value ? Convert.ToInt32(dataReader["Stock"]) : 0;
                                    getProduct.CategoryId = dataReader["CategoryId"] != DBNull.Value ? Convert.ToInt32(dataReader["CategoryId"]) : 0;
                                    getProduct.ImageUrl = dataReader["ImageUrl"] != DBNull.Value ? Convert.ToString(dataReader["ImageUrl"]) : string.Empty;
                                    getProduct.IsActive = dataReader["IsActive"] != DBNull.Value ? Convert.ToBoolean(dataReader["IsActive"]) : false;
                                    getProduct.CreatedDate = dataReader["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dataReader["CreatedDate"]) : DateTime.MinValue;
                                    getProduct.UpdatedDate = dataReader["UpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dataReader["UpdatedDate"]) : DateTime.MinValue;

                                    response.Products.Add(getProduct);
                                }
                            }
                            else
                            {
                                response.IsSuccess = true;
                                response.Message = "No products found.";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        response.IsSuccess = false;
                        response.Message = $"SQL load failed: {ex.Message}";
                        _logger.LogError("Error occur: Query not executed.");
                        return response;
                    }
                    finally
                    {
                        await sqlCommand.DisposeAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                _logger.LogError($"Error occur: {ex.Message}");
            }

            return response;
        }

        public async Task<Response> UpdateProduct(UpdateProduct request)
        {
            _logger.LogInformation("UpdateProduct is Calling in Repository");
            var response = new Response();

            // Validate SQL
            string sqlText;
            try
            {
                sqlText = SqlQueries.UpdateProduct;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"SQL load failed: {ex.Message}";
                return response;
            }

            if (string.IsNullOrWhiteSpace(sqlText))
            {
                response.IsSuccess = false;
                response.Message = "AddProduct SQL is not configured.";
                return response;
            }

            try
            {
                if (_MySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _MySqlConnection.OpenAsync();
                }

                using (MySqlCommand sqlCommand = new MySqlCommand(sqlText, _MySqlConnection))
                {
                    try
                    {
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        sqlCommand.CommandTimeout = 180;

                        // Parameters for AddProduct (product fields)
                        sqlCommand.Parameters.AddWithValue("@ProductId", request.ProductId);
                        sqlCommand.Parameters.AddWithValue("@ProductName", request.ProductName);
                        sqlCommand.Parameters.AddWithValue("@Description", request.Description);
                        sqlCommand.Parameters.AddWithValue("@Brand", request.Brand);
                        //sqlCommand.Parameters.AddWithValue("@SKU", request.SKU);
                        sqlCommand.Parameters.AddWithValue("@Price", request.Price);
                        sqlCommand.Parameters.AddWithValue("@Stock", request.Stock);
                        sqlCommand.Parameters.AddWithValue("@CategoryId", request.CategoryId);
                        sqlCommand.Parameters.AddWithValue("@ImageUrl", request.ImageUrl);
                        sqlCommand.Parameters.AddWithValue("@IsActive", request.IsActive);
                        //sqlCommand.Parameters.AddWithValue("@CreatedDate", request.CreatedDate);
                        //sqlCommand.Parameters.AddWithValue("@UpdatedDate", request.UpdatedDate);


                        int status = await sqlCommand.ExecuteNonQueryAsync();
                        if (status <= 0)
                        {
                            response.IsSuccess = false;
                            response.Message = "Query not executed.";
                            _logger.LogError("Error occur: Query not executed.");
                            return response;
                        }

                        response.IsSuccess = true;
                        response.Message = "UpdateProduct added successfully.";

                    }
                    catch (Exception ex)
                    {
                        response.IsSuccess = false;
                        response.Message = ex.Message;
                        _logger.LogError($"Error occur: {ex.Message}");
                        return response;
                    }
                    finally
                    {
                        //if (_MySqlConnection.State == System.Data.ConnectionState.Open)
                        //{
                        await _MySqlConnection.CloseAsync();
                        await _MySqlConnection.DisposeAsync();
                        //}
                    }     
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                _logger.LogError($"Error occur: {ex.Message}");
                return response;
            }
            return response;
        }

        public async Task<Response> InactiveProduct(InactiveProduct request)
        {
            _logger.LogInformation("InactiveProduct is Calling in Repository");
            Response response = new Response();
            response.IsSuccess = true;
            response.Message = "InactiveProduct successfully.";

            // Validate SQL
            string sqlText;
            try
            {
                sqlText = SqlQueries.InactiveProduct;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"SQL load failed: {ex.Message}";
                return response;
            }

            if (string.IsNullOrWhiteSpace(sqlText))
            {
                response.IsSuccess = false;
                response.Message = "DeleteProduct SQL is not configured.";
                return response;
            }

            try
            {

                if (_MySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _MySqlConnection.OpenAsync();
                }

                using (MySqlCommand sqlCommand = new MySqlCommand(sqlText, _MySqlConnection))
                {                   
                    try
                    {
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        sqlCommand.CommandTimeout = 180;
                        // Parameters for AddProduct (product fields)
                        sqlCommand.Parameters.AddWithValue("@ProductId", request.ProductId);
                        sqlCommand.Parameters.AddWithValue("@IsActive", request.IsActive);

                        int status = await sqlCommand.ExecuteNonQueryAsync();
                        if (status <= 0)
                        {
                            response.IsSuccess = false;
                            response.Message = "Query not executed.";
                            _logger.LogError("Error occur: Query not executed.");
                            return response;
                        }

                        response.IsSuccess = true;
                        response.Message = "InactiveProduct successfully.";
                    }
                    catch (Exception ex)
                    {
                        response.IsSuccess = false;
                        response.Message = ex.Message;
                        _logger.LogError($"Error occur: {ex.Message}");
                        
                    }
                    finally
                    {
                        //if (_MySqlConnection.State == System.Data.ConnectionState.Open)
                        //{
                        await _MySqlConnection.CloseAsync();
                        await _MySqlConnection.DisposeAsync();
                        //}
                    }
                }           
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                _logger.LogError($"Error occur: {ex.Message}");
                return response;
            }
            return response;
        }

        public async Task<Response> DeleteProduct(DeleteProduct request)
        {
            _logger.LogInformation("DeleteProduct is Calling in Repository");
            Response response = new Response();
            response.IsSuccess = true;
            response.Message = "DeleteProduct successfully.";

            // Validate SQL
            string sqlText;
            try
            {
                sqlText = SqlQueries.DeleteProduct;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"SQL load failed: {ex.Message}";
                return response;
            }

            if (string.IsNullOrWhiteSpace(sqlText))
            {
                response.IsSuccess = false;
                response.Message = "DeleteProduct SQL is not configured.";
                return response;
            }

            try
            {

                if (_MySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _MySqlConnection.OpenAsync();
                }

                using (MySqlCommand sqlCommand = new MySqlCommand(sqlText, _MySqlConnection))
                {
                    try
                    {
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        sqlCommand.CommandTimeout = 180;
                        // Parameters for AddProduct (product fields)
                        sqlCommand.Parameters.AddWithValue("@ProductId", request.ProductId);

                        int status = await sqlCommand.ExecuteNonQueryAsync();
                        if (status <= 0)
                        {
                            response.IsSuccess = false;
                            response.Message = "Query not executed.";
                            _logger.LogError("Error occur: Query not executed.");
                            return response;
                        }

                        response.IsSuccess = true;
                        response.Message = "InactiveProduct successfully.";                      
                    }
                    catch (Exception ex)
                    {
                        response.IsSuccess = false;
                        response.Message = ex.Message;
                        _logger.LogError($"Error occur: {ex.Message}");
                        return response;
                    }
                    finally
                    {
                        //if (_MySqlConnection.State == System.Data.ConnectionState.Open)
                        //{
                        await _MySqlConnection.CloseAsync();
                        await _MySqlConnection.DisposeAsync();
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                _logger.LogError($"Error occur: {ex.Message}");
                return response;
            }
            return response;
        }

    }
}
