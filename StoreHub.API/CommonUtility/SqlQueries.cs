namespace StoreHub.API.CommonUtility
{
    public class SqlQueries
    {
        private static readonly IConfiguration _configuration;
        private static readonly string[] _searchedPaths;

        static SqlQueries()
        {
            string baseDir = AppContext.BaseDirectory ?? Directory.GetCurrentDirectory();
            var candidates = new[]
            {
                Path.Combine(baseDir, "SqlQueries.xml"),
                Path.Combine(baseDir, "CommonUtility", "SqlQueries.xml"),
                Path.Combine(baseDir, "CommonUtility\\SqlQueries.xml"),
                Path.Combine(Directory.GetCurrentDirectory(), "SqlQueries.xml"),
                Path.Combine(Directory.GetCurrentDirectory(), "CommonUtility", "SqlQueries.xml")
            }.Distinct().ToArray();

            _searchedPaths = candidates;

            var found = candidates.FirstOrDefault(File.Exists);
            if (found == null)
                throw new InvalidOperationException("SqlQueries.xml not found. Searched: " + string.Join("; ", candidates));

            _configuration = new ConfigurationBuilder()
                .AddXmlFile(found, optional: false, reloadOnChange: true)
                .Build();
        }

        public static string AddProduct
        {
            get
            {
                var sql = _configuration["Queries:AddProduct"]?.Trim();
                if (string.IsNullOrWhiteSpace(sql))
                {
                    throw new InvalidOperationException("SQL query 'Queries:AddProduct' not found or empty in SqlQueries.xml. Searched: " + string.Join("; ", _searchedPaths));
                }
                return sql;
            }
        }

        public static string GetProduct
        {
            get
            {
                var sql = _configuration["Queries:GetProduct"];
                if (string.IsNullOrWhiteSpace(sql))
                {
                    throw new InvalidOperationException("SQL query 'Queries:GetProduct' not found or empty in SqlQueries.xml. Searched: " + string.Join("; ", _searchedPaths));
                }
                return sql;
            }
        }

        public static string GetProductById
        {
            get
            {
                var sql = _configuration["Queries:GetProductById"];
                if (string.IsNullOrWhiteSpace(sql))
                {
                    throw new InvalidOperationException("SQL query 'Queries:GetProductById' not found or empty in SqlQueries.xml. Searched: " + string.Join("; ", _searchedPaths));
                }
                return sql;
            }
        }

        public static string UpdateProduct
        {
            get
            {
                var sql = _configuration["Queries:UpdateProduct"];
                if (string.IsNullOrWhiteSpace(sql))
                {
                    throw new InvalidOperationException("SQL query 'Queries:UpdateProduct' not found or empty in SqlQueries.xml. Searched: " + string.Join("; ", _searchedPaths));
                }
                return sql;
            }
        }

        public static string DeleteProduct
        {
            get
            {
                var sql = _configuration["Queries:InactiveProduct"];
                if (string.IsNullOrWhiteSpace(sql))
                {
                    throw new InvalidOperationException("SQL query 'Queries:InactiveProduct' not found or empty in SqlQueries.xml. Searched: " + string.Join("; ", _searchedPaths));
                }
                return sql;
            }
        }

        public static string ProductById
        {
            get
            {
                var sql = _configuration["Queries:GetProductById"];
                if (string.IsNullOrWhiteSpace(sql))
                {
                    throw new InvalidOperationException("SQL query 'Queries:GetProductById' not found or empty in SqlQueries.xml. Searched: " + string.Join("; ", _searchedPaths));
                }
                return sql;
            }
        }
    }
}
