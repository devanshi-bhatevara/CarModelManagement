using CarModelManagementApi.Dtos;

namespace CarModelManagementApi.Data
{
    public interface ICarModelRepository
    {
        Task<IEnumerable<CarModel>> GetAllCarModels(string? search);

        Task<bool> AddCarModelWithImages(AddDto request);

        Task<bool> CarModelExists(string modelCode);

        Task<CarModelDto> GetCarModelById(int carModelId);

        Task<bool> UpdateCarModel(CarModelDto request);

        Task<bool> IsCarModelExists(string modelCode, int carModelId);


        Task<bool> DeleteCarModel(int carModelId);
    }
}
