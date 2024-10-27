using CarModelManagementApi.Dtos;

namespace CarModelManagementApi.Services
{
    public interface ICarModelService
    {
        Task<ServiceResponse<IEnumerable<CarModel>>> GetAllCarModels(string? search);

        Task<ServiceResponse<string>> AddCarModelWithImages(AddDto request);

        Task<ServiceResponse<CarModelDto>> GetCarModelById(int carModelId);

        Task<ServiceResponse<string>> UpdateCarModel(CarModelDto request);

        Task<ServiceResponse<string>> DeleteCarModel(int carModelId);
    }
}
