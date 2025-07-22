using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.Package;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Package;
using MathExamGenerator.Model.Utils;
using MathExamGenerator.Repository.Interface;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Service.Implement
{
    public class PackageService : BaseService<PackageService>, IPackageService
    {
        public PackageService(
              IUnitOfWork<MathExamGeneratorContext> unitOfWork,
              ILogger<PackageService> logger,
              IMapper mapper,
              IHttpContextAccessor httpContextAccessor
          ) : base(unitOfWork, logger, mapper, httpContextAccessor) { }


        public async Task<BaseResponse<GetPackageResponse>> Create(CreatePackageRequest request)
        {
            var package = _mapper.Map<Package>(request);

            await _unitOfWork.GetRepository<Package>().InsertAsync(package);
            var result = await _unitOfWork.CommitAsync();
            if (result < 0)
            {
                throw new Exception("Tạo gói thất bại");
            }

            var response = _mapper.Map<GetPackageResponse>(package);
            response.IsActive = request.IsActive;
            return new BaseResponse<GetPackageResponse>
            {
                Status = StatusCodes.Status201Created.ToString(),
                Message = "Tạo gói thành công",
                Data = response
            };
        }

        public async Task<BaseResponse<bool>> Delete(Guid id)
        {
            var package = await _unitOfWork.GetRepository<Package>().SingleOrDefaultAsync(
             predicate: p => p.Id == id && p.DeleteAt == null
             );

            if (package == null)
            {
                return new BaseResponse<bool>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Gói không tồn tại",
                    Data = false
                };
            }
            package.DeleteAt = TimeUtil.GetCurrentSEATime();
            _unitOfWork.GetRepository<Package>().DeleteAsync(package);
            var result = await _unitOfWork.CommitAsync();
            if (result < 0)
            {
                throw new Exception("Xóa gói thất bại");
            }

            return new BaseResponse<bool>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Xóa gói thành công",
                Data = true
            };
        }

        public async Task<BaseResponse<IPaginate<GetPackageResponse>>> GetActive(int page, int size)
        {
            var packages = await _unitOfWork.GetRepository<Package>()
                    .GetPagingListAsync(
                        predicate: p => p.IsActive == true,
                        selector: p => new GetPackageResponse
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Price = p.Price,
                            CreateAt = p.CreateAt
                        },
                        orderBy: q => q.OrderByDescending(p => p.CreateAt),
                        page: page,
                        size: size
                    );

            return new BaseResponse<IPaginate<GetPackageResponse>>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy danh sách gói đang hoạt động thành công",
                Data = packages
            };
        }

        public async Task<BaseResponse<IPaginate<GetPackageResponse>>> GetAll(int page, int size)
            {
                var packages = await _unitOfWork.GetRepository<Package>()
                    .GetPagingListAsync(
                        selector: p => new GetPackageResponse
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Price = p.Price,
                            CreateAt = p.CreateAt
                        },
                        orderBy: q => q.OrderByDescending(p => p.CreateAt),
                        page: page,
                        size: size
                    );

                return new BaseResponse<IPaginate<GetPackageResponse>>
                {
                    Status = StatusCodes.Status200OK.ToString(),
                    Message = "Lấy danh sách gói thành công",
                    Data = packages
                };
            }

        public async Task<BaseResponse<GetPackageResponse>> GetById(Guid id)
        {
            var package = await _unitOfWork.GetRepository<Package>().SingleOrDefaultAsync(
                predicate: p => p.Id == id && p.DeleteAt == null
                );

            if (package == null)
            {
                return new BaseResponse<GetPackageResponse>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy gói",
                    Data = null
                };
            }

            var data = _mapper.Map<GetPackageResponse>(package);
            return new BaseResponse<GetPackageResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy thông tin gói thành công",
                Data = data
            };
        }


        public async Task<BaseResponse<GetPackageResponse>> Update(Guid id, UpdatePackageRequest request)
        {
            var package = await _unitOfWork.GetRepository<Package>().SingleOrDefaultAsync(
               predicate: p => p.Id == id && p.DeleteAt == null
               );

            if (package == null)
            {
                return new BaseResponse<GetPackageResponse>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Gói không tồn tại",
                    Data = null
                };
            }
            _mapper.Map(request, package);
            package.UpdateAt = TimeUtil.GetCurrentSEATime();
            _unitOfWork.GetRepository<Package>().UpdateAsync(package);
            var result = await _unitOfWork.CommitAsync();
            if (result < 0)
            {
                throw new Exception("Cập nhật gói thất bại");
            }
            var response = _mapper.Map<GetPackageResponse>(package);
            response.IsActive = request.IsActive;
            return new BaseResponse<GetPackageResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Cập nhật gói thành công",
                Data = response
            };


        }
    }
}
