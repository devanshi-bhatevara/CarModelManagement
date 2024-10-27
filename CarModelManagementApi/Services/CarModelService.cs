using CarModelManagementApi.Data;
using CarModelManagementApi.Dtos;

namespace CarModelManagementApi.Services
{
    public class CarModelService : ICarModelService
    {
        private readonly ICarModelRepository _carModelRepository;

        public CarModelService(ICarModelRepository carModelRepository)
        {
            _carModelRepository = carModelRepository;
        }

        public async Task<ServiceResponse<IEnumerable<CarModel>>> GetAllCarModels(string? search)
        {
            var response = new ServiceResponse<IEnumerable<CarModel>>();
            var carModels = await _carModelRepository.GetAllCarModels(search);
            if (carModels != null && carModels.Any())
            {
                response.Data = carModels;
            }
            else
            {
                response.Success = false;
                response.Message = "No record found";
            }
            return response;
        }

        public async Task<ServiceResponse<string>> AddCarModelWithImages(AddDto request)
        {
            var response = new ServiceResponse<string>();

            var result = await _carModelRepository.AddCarModelWithImages(request);

            if (result)
            {
                response.Success = true;
                response.Message = "Car model saved successfully";
            }
            else
            {
                response.Success = false;
                response.Message = "Car model already exists!";
            }
            return response;
        }

        public async Task<ServiceResponse<CarModelDto>> GetCarModelById(int carModelId)
        {
            var response = new ServiceResponse<CarModelDto>();
            var carModelDto = await _carModelRepository.GetCarModelById(carModelId);

            if (carModelDto != null)
            {
                response.Success = true;
                response.Data = carModelDto;
            }
            else
            {
                response.Success = false;
                response.Message = "Car model not found";
            }
            return response;
        }

        public async Task<ServiceResponse<string>> UpdateCarModel(CarModelDto request)
        {
            var response = new ServiceResponse<string>();
            var result = await _carModelRepository.UpdateCarModel(request);

            if (result)
            {
                response.Success = true;
                response.Message = "Data updated successfully";
            }
            else
            {
                response.Success = false;
                response.Message = "Already exists";
            }
            return response;
        }

        public async Task<ServiceResponse<string>> DeleteCarModel(int carModelId)
        {
            var response = new ServiceResponse<string>();
            var result = await _carModelRepository.DeleteCarModel(carModelId);

            if (result)
            {
                response.Success = true;
                response.Message = "Car model deleted successfully";
            }
            else
            {
                response.Success = false;
                response.Message = "Something went wrong";
            }
            return response;
        }

    }
}
