using CarModelManagementApi.Dtos;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;

namespace CarModelManagementApi.Data
{
    public class CarModelRepository : ICarModelRepository
    {
        private readonly string _connectionString;

        public CarModelRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<CarModel>> GetAllCarModels(string? search)
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
            SELECT cm.*, cmi.ImageData, cmi.ImageName, cmi.ImageType
            FROM CarModel cm
            LEFT JOIN CarModelImage cmi ON cm.CarModelId = cmi.CarModelId
            WHERE cm.IsActive = 1";

                if (!string.IsNullOrEmpty(search))
                {
                    query += " AND (cm.ModelName LIKE @Search OR cm.ModelCode LIKE @Search) ";
                }

                query += " ORDER BY cm.DateOfManufacturing DESC";

                Console.WriteLine($"Generated Query: {query}");


                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (!string.IsNullOrEmpty(search))
                    {
                        command.Parameters.AddWithValue("@Search", "%" + search + "%");
                    }

                    await connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        var carModels = new List<CarModel>();

                        while (await reader.ReadAsync())
                        {
                            var carModelId = Convert.ToInt32(reader["CarModelId"]); // Adjust according to your model properties

                            var existingCarModel = carModels.FirstOrDefault(cm => cm.CarModelId == carModelId);

                            if (existingCarModel == null)
                            {
                                var carModel = new CarModel
                                {
                                    CarModelId = carModelId,
                                    Brand = reader["Brand"].ToString(),
                                    Class = reader["Class"].ToString(),
                                    ModelName = reader["ModelName"].ToString(),
                                    ModelCode = reader["ModelCode"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    Features = reader["Features"].ToString(),
                                    Price = Convert.ToDecimal(reader["Price"]),
                                    DateOfManufacturing = Convert.ToDateTime(reader["DateOfManufacturing"]),
                                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                                    SortOrder = Convert.ToInt32(reader["SortOrder"]),
                                    Images = new List<CarImageDto>() // Assuming ImageModel contains ImageData, ImageName, and ImageType
                                };

                                if (!reader.IsDBNull(reader.GetOrdinal("ImageData")))
                                {
                                    carModel.Images.Add(new CarImageDto
                                    {
                                        ImageData = (byte[])reader["ImageData"],
                                        ImageName = reader["ImageName"].ToString(),
                                        ImageType = reader["ImageType"].ToString()
                                    });
                                }

                                carModels.Add(carModel);
                            }
                            else
                            {
                                if (!reader.IsDBNull(reader.GetOrdinal("ImageData")))
                                {
                                    existingCarModel.Images.Add(new CarImageDto
                                    {
                                        ImageData = (byte[])reader["ImageData"],
                                        ImageName = reader["ImageName"].ToString(),
                                        ImageType = reader["ImageType"].ToString()
                                    });
                                }
                            }
                        }
                        return carModels;
                    }
                }
            }
        }

        public async Task<bool> AddCarModelWithImages(AddDto request)
        {
            if (await CarModelExists(request.ModelCode))
            {
                return false;
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("AddCarModelWithImages", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add parameters for CarModel fields
                    command.Parameters.AddWithValue("@Brand", request.Brand);
                    command.Parameters.AddWithValue("@Class", request.Class);
                    command.Parameters.AddWithValue("@ModelName", request.ModelName);
                    command.Parameters.AddWithValue("@ModelCode", request.ModelCode);
                    command.Parameters.AddWithValue("@Description", request.Description);
                    command.Parameters.AddWithValue("@Features", request.Features);
                    command.Parameters.AddWithValue("@Price", request.Price);
                    command.Parameters.AddWithValue("@DateOfManufacturing", request.DateOfManufacturing);
                    command.Parameters.AddWithValue("@IsActive", true);
                    command.Parameters.AddWithValue("@SortOrder", request.SortOrder);

                    // Map image data to TVP
                    var tvpTable = new DataTable();
                    tvpTable.Columns.Add("ImageData", typeof(byte[]));
                    tvpTable.Columns.Add("ImageName", typeof(string));
                    tvpTable.Columns.Add("ImageType", typeof(string));

                    foreach (var image in request.Images)
                    {
                        tvpTable.Rows.Add(image.ImageData, image.ImageName, image.ImageType);
                    }

                    var imagesParameter = command.Parameters.AddWithValue("@Images", tvpTable);
                    imagesParameter.SqlDbType = SqlDbType.Structured;
                    imagesParameter.TypeName = "dbo.TVP_CarModelImage"; // Adjust schema if needed

                    connection.Open();
                    var result = await command.ExecuteNonQueryAsync();
                    return result > 0;
                }
            }
        }

        public async Task<bool> CarModelExists(string modelCode)
        {

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(*) FROM CarModel WHERE ModelCode = @ModelCode";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ModelCode", modelCode);
                    await connection.OpenAsync();
                    int count = (int)await command.ExecuteScalarAsync();
                    return count > 0; 
                }
            }
        }

        public async Task<CarModelDto> GetCarModelById(int carModelId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT CarModelId, Brand, Class, ModelName, ModelCode, Description, Features, Price, DateOfManufacturing " +
                   "FROM CarModel WHERE CarModelId = @CarModelId AND IsActive = 1"; // Soft delete condition

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CarModelId", carModelId);

                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new CarModelDto
                            {
                                CarModelId = (int)reader["CarModelId"],
                                Brand = reader["Brand"].ToString(),
                                Class = reader["Class"].ToString(),
                                ModelName = reader["ModelName"].ToString(),
                                ModelCode = reader["ModelCode"].ToString(),
                                Description = reader["Description"].ToString(),
                                Features = reader["Features"].ToString(),
                                Price = (decimal)reader["Price"],
                                DateOfManufacturing = (DateTime)reader["DateOfManufacturing"]
                            };
                        }
                        return null; // No record found
                    }
                }
            }
        }

        public async Task<bool> UpdateCarModel(CarModelDto request)
        {
            if (await IsCarModelExists(request.ModelCode, request.CarModelId))
            {
                return false; 
            }
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var query = "UPDATE CarModel SET Brand = @Brand, Class = @Class, ModelName = @ModelName, " +
                            "ModelCode = @ModelCode, Description = @Description, Features = @Features, " +
                            "Price = @Price, DateOfManufacturing = @DateOfManufacturing, IsActive = 1 " +
                            "WHERE CarModelId = @CarModelId"; // No condition for soft delete

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CarModelId", request.CarModelId);
                    command.Parameters.AddWithValue("@Brand", request.Brand);
                    command.Parameters.AddWithValue("@Class", request.Class);
                    command.Parameters.AddWithValue("@ModelName", request.ModelName);
                    command.Parameters.AddWithValue("@ModelCode", request.ModelCode);
                    command.Parameters.AddWithValue("@Description", request.Description);
                    command.Parameters.AddWithValue("@Features", request.Features);
                    command.Parameters.AddWithValue("@Price", request.Price);
                    command.Parameters.AddWithValue("@DateOfManufacturing", request.DateOfManufacturing);
                    connection.Open();
                    var result = await command.ExecuteNonQueryAsync();
                    return result > 0;
                }
            }
        }

        public async Task<bool> IsCarModelExists(string modelCode, int carModelId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT COUNT(1) FROM CarModel WHERE ModelCode = @ModelCode AND CarModelId != @CarModelId AND IsActive = 1";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ModelCode", modelCode);
                    command.Parameters.AddWithValue("@CarModelId", carModelId);

                    connection.Open();
                    var exists = (int)await command.ExecuteScalarAsync() > 0;
                    return exists;
                }
            }
        }


        public async Task<bool> DeleteCarModel(int carModelId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var query = "UPDATE CarModel SET IsActive = 0 WHERE CarModelId = @CarModelId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CarModelId", carModelId);
                    connection.Open();
                    var result = await command.ExecuteNonQueryAsync();
                    return result > 0;
                }
            }
        }





    }
}
