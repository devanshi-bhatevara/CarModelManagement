using CarModelManagementApi.Dtos;
using CarModelManagementApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace CarModelManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarModelController : ControllerBase
    {
        private readonly ICarModelService _carModelService;
        private readonly IConfiguration _config;

        public CarModelController(ICarModelService carModelService)
        {
            _carModelService = carModelService;
        }

        [HttpGet("GetAllCarModels")]
        public async Task<IActionResult> GetAllCarModels(string? search)
        {
            var response = await _carModelService.GetAllCarModels(search);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost("AddCarModelWithImages")]
        public async Task<IActionResult> AddCarModel(AddDto addDto)
        {
            var response = await _carModelService.AddCarModelWithImages(addDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetCarModelById/{carModelId}")]
        public async Task<IActionResult> GetCarModelById(int carModelId)
        {
            var response = await _carModelService.GetCarModelById(carModelId);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpPut("UpdateCarModel")]
        public async Task<IActionResult> UpdateCarModel(CarModelDto request)
        {
            var response = await _carModelService.UpdateCarModel(request);
            if (response.Success)
            {
                return Ok(response); 
            }
            return BadRequest(response);
        }

        [HttpDelete("DeleteCarModel/{carModelId}")]
        public async Task<IActionResult> DeleteCarModel(int carModelId)
        {
            var response = await _carModelService.DeleteCarModel(carModelId);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);

        }



    }
}
