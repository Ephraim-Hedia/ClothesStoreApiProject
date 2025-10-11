using AutoMapper;
using Microsoft.Extensions.Logging;
using Store.Data.Entities.ProductEntities;
using Store.Repositories.Interfaces;
using Store.Services.HandleResponse.CommonResponse;
using Store.Services.Services.ProductSizeService.Dtos;

namespace Store.Services.Services.ProductSizeService
{
    public class ProductSizeService : IProductSizeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper; 
        private ILogger<ProductSizeService> _logger;
        public ProductSizeService
            (
                IUnitOfWork unitOfWork ,
                IMapper mapper , 
                ILogger<ProductSizeService> logger
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<CommonResponse<SizeResultDto>> AddSizeAsync(SizeCreateDto dto)
        {
            var response = new CommonResponse<SizeResultDto>();
            ProductSize size;
            if(dto == null)
                return response.Fail("400" , "Invalid Data, Size Data is Null");
            
            try
            {
                size = _mapper.Map<ProductSize>(dto);
                await _unitOfWork.Repository<ProductSize ,int>().AddAsync(size);
                await _unitOfWork.CompleteAsync();
                var mappedSize = _mapper.Map<SizeResultDto>(size);
                return response.Success(mappedSize);
            }
            catch (Exception err)
            {
                _logger.LogError(err, "Error occurred while adding Size: {SizeName}", dto.Name);
                throw;
            }        
        }

        public async Task<CommonResponse<bool>> DeleteSizeAsync(int sizeId)
        {
            var response = new CommonResponse<bool>();
            ProductSize size;
            if (sizeId <= 0)
                return response.Fail("400", "Invalid Data, Size Id must be greater than 0");
            try
            {
                size = await _unitOfWork.Repository<ProductSize, int>().GetByIdAsync(sizeId);
                if(size  == null)
                    return response.Fail("404", "Size Not Found");
                _unitOfWork.Repository<ProductSize,int>().Delete(size);
                await _unitOfWork.CompleteAsync();            
                return response.Success(true);
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }            
        }

        public async Task<CommonResponse<IReadOnlyList<SizeResultDto>>> GetAllSizeAsync()
        {
            var response = new CommonResponse<IReadOnlyList<SizeResultDto>>();
            IReadOnlyList<ProductSize> sizes;
            try
            {
                sizes = await _unitOfWork.Repository<ProductSize, int>().GetAllAsync();
                if (!sizes.Any())
                    return response.Fail("404", "No Sizes Found");
                var mappedSizes = _mapper.Map<IReadOnlyList<SizeResultDto>>(sizes);
                return response.Success(mappedSizes);
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }
        }

        public async Task<CommonResponse<SizeResultDto>> GetSizeByIdAsync(int sizeId)
        {
            var response = new CommonResponse<SizeResultDto>();
            ProductSize size;
            if (sizeId <= 0)
                return response.Fail("400", "Invalid Data, Size Id must be greater than 0");

            try
            {
                size = await _unitOfWork.Repository<ProductSize, int>().GetByIdAsync(sizeId);
                if(size == null)
                    return response.Fail("404", "Size Not Found");
                var mappedSize = _mapper.Map<SizeResultDto>(size);
                return response.Success(mappedSize);
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }
            
        }

        public async Task<CommonResponse<SizeResultDto>> UpdateSizeAsync(int sizeId, SizeUpdateDto dto)
        {
            var response = new CommonResponse<SizeResultDto>();
            ProductSize size;
            if (sizeId <= 0)
                return response.Fail("400", "Invalid Data, Size Id must be greater than 0");
            if (dto == null)
                return response.Fail("400", "Not Valid Input, Size Data is Null");
            try
            {
                size = await _unitOfWork.Repository<ProductSize, int>().GetByIdAsync(sizeId);
                if (size == null)
                    return response.Fail("404", "Size Not Found");
                
                if(!string.IsNullOrEmpty(dto.Name))
                    size.Name = dto.Name;
                if(!string.IsNullOrEmpty(dto.Description))
                    size.Description = dto.Description;

                _unitOfWork.Repository<ProductSize, int>().Update(size);
                await _unitOfWork.CompleteAsync();
                var mappedSize = _mapper.Map<SizeResultDto>(size);
                return response.Success(mappedSize);
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                throw;
            }
        }
    }
}
