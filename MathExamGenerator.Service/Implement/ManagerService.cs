using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Enum;
using MathExamGenerator.Model.Exceptions;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.Manager;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Account;
using MathExamGenerator.Model.Payload.Response.Manager;
using MathExamGenerator.Model.Utils;
using MathExamGenerator.Repository.Interface;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MathExamGenerator.Service.Implement;

public class ManagerService : BaseService<ManagerService>, IManagerService
{
    private readonly IUploadService _uploadService;
    public ManagerService(IUnitOfWork<MathExamGeneratorContext> unitOfWork, 
                          ILogger<ManagerService> logger, 
                          IMapper mapper, 
                          IHttpContextAccessor httpContextAccessor,
                          IUploadService uploadService) : base(unitOfWork, logger, mapper, httpContextAccessor)
    {
        _uploadService = uploadService;
    }

    public async Task<BaseResponse<RegisterResponse>> RegisterManager(RegisterManagerRequest request)
    {
        var accountList = await _unitOfWork.GetRepository<Account>().GetListAsync();
        if (accountList.Any(a => a.UserName.Equals(request.UserName)))
        {
            throw new BadHttpRequestException("Tên đăng nhập đã tồn tại");
        }

        if (accountList.Any(a => a.Email.Equals(request.Email)))
        {
            throw new BadHttpRequestException("Email đã tồn tại");
        }

        if (accountList.Any(a => a.Phone.Equals(request.Phone)))
        {
            throw new BadHttpRequestException("Số điện thoại đã tồn tại");
        }

        var account = _mapper.Map<Account>(request);

        account.AvatarUrl = await _uploadService.UploadImage(request.AvatarUrl);

        await _unitOfWork.GetRepository<Account>().InsertAsync(account);

        var isSuccess = await _unitOfWork.CommitAsync() > 0;

        if (!isSuccess)
        {
            throw new Exception("Một lỗi đã xảy ra trong quá trình đăng ký tài khoản");
        }

        return new BaseResponse<RegisterResponse>
        {
            Status = StatusCodes.Status200OK.ToString(),
            Message = "Tạo tài khoản thành công",
            Data = _mapper.Map<RegisterResponse>(account)
        };
    }

    public async Task<BaseResponse<IPaginate<GetManagerResponse>>> GetAllManager(int page, int size)
    {
        if (page < 1 || size < 1)
        {
            throw new BadHttpRequestException("Số trang và số lượng trong trang phải lớn hơn hoặc bằng 1");
        }
        
        var response = await _unitOfWork.GetRepository<Account>().GetPagingListAsync(
            selector: m => new GetManagerResponse
            {
                Id = m.Id,
                Email = m.Email,
                Phone = m.Phone,
                AvatarUrl = m.AvatarUrl,
                DateOfBirth = m.DateOfBirth,
                FullName = m.FullName,
                Gender = m.Gender,
                UserName = m.UserName,
            },
            predicate: m => m.Role.Equals(RoleEnum.MANAGER.GetDescriptionFromEnum()) && m.IsActive == true,
            orderBy: m => m.OrderByDescending(m => m.CreateAt),
            page: page,
            size: size);

        return new BaseResponse<IPaginate<GetManagerResponse>>()
        {
            Status = StatusCodes.Status200OK.ToString(),
            Message = "Lấy danh sách thông tin thành viên quản lí thành công",
            Data = response
        };
    }

    public async Task<BaseResponse<GetManagerResponse>> GetManager(Guid id)
    {
        var response = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
            selector: m => new GetManagerResponse
            {
                Id = m.Id,
                Email = m.Email,
                Phone = m.Phone,
                AvatarUrl = m.AvatarUrl,
                DateOfBirth = m.DateOfBirth,
                FullName = m.FullName,
                Gender = m.Gender,
                UserName = m.UserName
            },
            predicate: m =>
                m.Id.Equals(id) && m.Role.Equals(RoleEnum.MANAGER.GetDescriptionFromEnum()) && m.IsActive == true)?? 
                       throw new NotFoundException("Không tìm thấy quản lí");

        return new BaseResponse<GetManagerResponse>()
        {
            Status = StatusCodes.Status200OK.ToString(),
            Message = "Lấy thông tin thành viên quản lí thành công",
            Data = response
        };
    }

    public async Task<BaseResponse<bool>> DeleteManager(Guid id)
    {
        var manager = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
            predicate: m => 
                m.Id.Equals(id) && m.Role.Equals(RoleEnum.MANAGER.GetDescriptionFromEnum()) && m.IsActive == true) ?? 
                      throw new NotFoundException("Không tìm thấy quản lí");
        
        manager.IsActive = false;
        manager.DeleteAt = TimeUtil.GetCurrentSEATime();
        _unitOfWork.GetRepository<Account>().UpdateAsync(manager);
        
        var isSuccess = await _unitOfWork.CommitAsync() > 0;

        if (!isSuccess)
        {
            throw new Exception("Một lỗi đã xảy ra trong quá trình xóa quản lí");
        }

        return new BaseResponse<bool>()
        {
            Status = StatusCodes.Status200OK.ToString(),
            Message = "Xóa quản lí thành công",
            Data = isSuccess
        };
    }
}