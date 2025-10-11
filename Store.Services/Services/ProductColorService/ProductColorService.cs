using AutoMapper;
using Microsoft.Extensions.Logging;
using Store.Data.Entities.ProductEntities;
using Store.Repositories.Interfaces;
using Store.Services.HandleResponse.CommonResponse;
using Store.Services.Services.ProductColorService.Dtos;


namespace Store.Services.Services.ProductColorService
{
    public class ProductColorService : IProductColorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public ProductColorService(
            IUnitOfWork unitOfWork ,
            IMapper mapper,
            ILogger<ProductColorService> logger
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<CommonResponse<ColorResultDto>> AddColorAsync(ColorCreateDto dto)
        {
            var response = new CommonResponse<ColorResultDto>();
            ProductColor color; 
            if(dto == null)
                return response.Fail("400", "Not Valid input Data");
            try
            {
                color = _mapper.Map<ProductColor>(dto);
                await _unitOfWork.Repository<ProductColor , int>().AddAsync(color);
                await _unitOfWork.CompleteAsync();
                var mappedColor = _mapper.Map<ColorResultDto>(color);
                return response.Success(mappedColor); 
            }
            catch (Exception err)
            {
                _logger.LogError(err, "Error occurred while adding color: {ColorName}", dto.ColorName);
                throw;
            }
        }

        public async Task<CommonResponse<bool>> DeleteColorAsync(int colorId)
        {
            var response = new CommonResponse<bool>();
            if (colorId <= 0)
                return response.Fail("400", "Invalid Data, Color Id must be greater than 0");
            try
            {
                var color = await _unitOfWork.Repository<ProductColor, int>().GetByIdAsync(colorId);
                if(color is null)
                    return response.Fail("404", "Color Not Found");

                _unitOfWork.Repository<ProductColor , int>().Delete(color);
                await _unitOfWork.CompleteAsync();
                return response.Success(true);
            }
            catch (Exception err)
            {
                _logger.LogError(err, "[ProductColorService.DeleteColorAsync] Error for ColorId: {ColorId}", colorId);
                throw;
            }
        }

        public async Task<CommonResponse<IReadOnlyList<ColorResultDto>>> GetAllColorAsync()
        {
            var response = new CommonResponse<IReadOnlyList<ColorResultDto>>();
            IReadOnlyList<ProductColor> colors;
            try
            {
                colors = await _unitOfWork.Repository<ProductColor, int>().GetAllAsync();
                if(!colors.Any())
                    return response.Fail("404", "No Colors Found");
                var mappedColorProducts = _mapper.Map<IReadOnlyList<ColorResultDto>>(colors);
                return response.Success(mappedColorProducts);
            }
            catch (Exception err)
            {
                _logger.LogError(err, "[ProductColorService.GetAllColorAsync] Error");
                throw;
            }      
        }

        public async Task<CommonResponse<ColorResultDto>> GetColorByIdAsync(int colorId)
        {
            var response = new CommonResponse<ColorResultDto>();
            ProductColor color;
            if (colorId <= 0)
                return response.Fail("400", "Invalid Data, Color Id must be greater than 0");

            try
            {
                color = await _unitOfWork.Repository<ProductColor, int>().GetByIdAsync(colorId);
                if(color == null)
                    return response.Fail("404", "Color Not Found");
                var mappedColor = _mapper.Map<ColorResultDto>(color);
                return response.Success(mappedColor);
            }
            catch (Exception err)
            {
                _logger.LogError(err, "[ProductColorService.GetColorByIdAsync] Error for ColorId: {ColorId}", colorId);
                throw;
            }  
        }

        public async Task<CommonResponse<ColorResultDto>> UpdateColorAsync(int colorId, ColorUpdateDto dto)
        {
            var response = new CommonResponse<ColorResultDto>();
            if (colorId <= 0)
                return response.Fail("400", "Invalid Data, Color Id must be greater than 0");
            if (dto == null)
                return response.Fail("400", "Not Valid Data, Color not found, cannot update Color");

            try
            {
                var color = await _unitOfWork.Repository<ProductColor, int>().GetByIdAsync(colorId);
                if (color == null)
                    return response.Fail("404", "Color Not Found");

                if (!string.IsNullOrEmpty(dto.ColorName))
                    color.ColorName = dto.ColorName;

                _unitOfWork.Repository<ProductColor, int>().Update(color);
                await _unitOfWork.CompleteAsync();

                var mappedColor = _mapper.Map<ColorResultDto>(color);
                return response.Success(mappedColor);
            }
            catch (Exception err)
            {
                _logger.LogError(err, "[ProductColorService.UpdateColorAsync] Error for ColorId: {ColorId}", colorId);
                _logger.LogError(err.Message);
                throw;
            }
        }
    }
}
