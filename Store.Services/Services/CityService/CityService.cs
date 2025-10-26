using AutoMapper;
using Microsoft.Extensions.Logging;
using Store.Data.Entities.IdentityEntities;
using Store.Repositories.Interfaces;
using Store.Services.HandleResponse.CommonResponse;
using Store.Services.Services.CityService.Dtos;

namespace Store.Services.Services.CityService
{
    public class CityService : ICityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CityService> _logger;
        public CityService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<CityService> logger
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<CommonResponse<CityResultDto>> AddCityAsync(CityCreateDto dto)
        {
            var response = new CommonResponse<CityResultDto>();
            if (dto == null)
                return response.Fail("400", "Bad Request, dto is Null");

            try
            {
                var city = _mapper.Map<City>(dto);
                await _unitOfWork.Repository<City, int>().AddAsync(city);
                await _unitOfWork.CompleteAsync();
                return response.Success(_mapper.Map<CityResultDto>(city));
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }
        }

        public async Task<CommonResponse<bool>> DeleteCityAsync(int cityId)
        {
            var response = new CommonResponse<bool>();
            if (cityId <= 0)
                return response.Fail("400", "Invalid Data, City Id must be more than 0");
            try
            {
                var city = await _unitOfWork.Repository<City, int>().GetByIdAsync(cityId);
                if (city == null)
                    return response.Fail("404", "City Not Found");

                _unitOfWork.Repository<City,int>().Delete(city);
                await _unitOfWork.CompleteAsync();
                return response.Success(true);
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }
        }

        public async Task<CommonResponse<IReadOnlyList<CityResultDto>>> GetAllCityAsync()
        {
            var response = new CommonResponse<IReadOnlyList<CityResultDto>>();
            try
            {
                var cities = await _unitOfWork.Repository<City, int>().GetAllAsync();
                if (cities == null || !cities.Any())
                    return response.Fail("404", "Not Found Cities");

                var mappedCities = _mapper.Map<IReadOnlyList<CityResultDto>>(cities);
                return response.Success(mappedCities);
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }
        }

        public async Task<CommonResponse<CityResultDto>> GetCityByIdAsync(int cityId)
        {
            var response = new CommonResponse<CityResultDto>();
            if (cityId <= 0)
                return response.Fail("400", "Invalid Data, City Id must be more than 0");
            try
            {
                var city = await _unitOfWork.Repository<City, int>().GetByIdAsync(cityId);
                if (city == null)
                    return response.Fail("404", "Not Found City");

                var mappedCity = _mapper.Map<CityResultDto>(city);
                return response.Success(mappedCity);
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }
        }

        public async Task<CommonResponse<CityResultDto>> UpdateCityAsync(int cityId, CityUpdateDto dto)
        {
            var response = new CommonResponse<CityResultDto>();
            if (cityId <= 0)
                return response.Fail("400", "Invalid Data, City Id must be more than 0");
            if (dto == null)
                return response.Fail("400", "Invalid Data, Dto is Null");

            try
            {
                var city =await  _unitOfWork.Repository<City, int>().GetByIdAsync(cityId);
                if (city == null)
                    return response.Fail("404", "City Not Found");

                if (!string.IsNullOrEmpty(dto.Name))
                    city.Name = dto.Name;
                if(dto.EstimatedDeliveryDays > 0)
                    city.EstimatedDeliveryDays = dto.EstimatedDeliveryDays.Value;
                if(dto.DeliveryCost > 0)
                    city.DeliveryCost = dto.DeliveryCost.Value;

                _unitOfWork.Repository<City, int>().Update(city);
                await _unitOfWork.CompleteAsync();
                return response.Success(_mapper.Map<CityResultDto>(city));
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }
        }
    }
}
