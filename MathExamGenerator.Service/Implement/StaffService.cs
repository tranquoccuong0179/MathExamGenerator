using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Enum;
using MathExamGenerator.Model.Exceptions;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.Staff;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Account;
using MathExamGenerator.Model.Payload.Response.Staff;
using MathExamGenerator.Model.Utils;
using MathExamGenerator.Repository.Interface;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MathExamGenerator.Service.Implement;

public class StaffService : BaseService<StaffService>, IStaffService
{
    private readonly IUploadService _uploadService;
    public StaffService(IUnitOfWork<MathExamGeneratorContext> unitOfWork, 
                        ILogger<StaffService> logger, 
                        IMapper mapper, 
                        IHttpContextAccessor httpContextAccessor,
                        IUploadService uploadService) : base(unitOfWork, logger, mapper, httpContextAccessor)
    {
        _uploadService = uploadService;
    }

    public async Task<BaseResponse<RegisterResponse>> RegisterStaff(RegisterStaffRequest request)
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
            throw new Exception("Một lỗi đã xảy ra trong quá trình đăng ký tài khoản nhân viên");
        }

        return new BaseResponse<RegisterResponse>()
        {
            Status = StatusCodes.Status200OK.ToString(),
            Message = "Đăng kí nhân viên thành công",
            Data = _mapper.Map<RegisterResponse>(account)
        };
    }

    public async Task<BaseResponse<IPaginate<GetStaffResponse>>> GetAllStaff(int page, int size)
    {
        if (page < 1 || size < 1)
        {
            throw new BadHttpRequestException("Số trang và số lượng trong trang phải lớn hơn hoặc bằng 1");
        }
        
        var staffs = await _unitOfWork.GetRepository<Account>().GetPagingListAsync(
            selector: s => new GetStaffResponse()
            {
                Id = s.Id,
                UserName = s.UserName,
                Email = s.Email,
                Phone = s.Phone,
                AvatarUrl = s.AvatarUrl,
                FullName = s.FullName,
                Gender = s.Gender,
                DateOfBirth = s.DateOfBirth,
            },
            predicate: s => s.Role.Equals(RoleEnum.STAFF.GetDescriptionFromEnum()) && s.IsActive == true,
            page: page,
            size: size);

        return new BaseResponse<IPaginate<GetStaffResponse>>()
        {
            Status = StatusCodes.Status200OK.ToString(),
            Message = "Lấy danh sách nhân viên thành công",
            Data = staffs
        };
    }

    public async Task<BaseResponse<GetStaffResponse>> GetStaff(Guid id)
    {
        var staff = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
            selector: s => new GetStaffResponse()
            {
                Id = s.Id,
                UserName = s.UserName,
                Email = s.Email,
                Phone = s.Phone,
                AvatarUrl = s.AvatarUrl,
                FullName = s.FullName,
                Gender = s.Gender,
                DateOfBirth = s.DateOfBirth,
            },
            predicate: s => s.Id.Equals(id) && s.Role.Equals(RoleEnum.STAFF.GetDescriptionFromEnum()) && s.IsActive == true) 
                    ?? throw new NotFoundException("Không tìm thấy thông tin nhân viên");

        return new BaseResponse<GetStaffResponse>()
        {
            Status = StatusCodes.Status200OK.ToString(),
            Message = "Lấy thông tin nhân viên thành công",
            Data = staff
        };
    }

    public async Task<BaseResponse<bool>> DeleteStaff(Guid id)
    {
        var staff = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
            predicate: s => s.Id.Equals(id) && s.Role.Equals(RoleEnum.STAFF.GetDescriptionFromEnum()) && s.IsActive == true) 
                    ?? throw new NotFoundException("Không tìm thấy thông tin nhân viên");
        
        staff.IsActive = false;
        staff.DeleteAt = TimeUtil.GetCurrentSEATime();
        
        _unitOfWork.GetRepository<Account>().UpdateAsync(staff);
        
        var isSuccess = await _unitOfWork.CommitAsync() > 0;

        if (!isSuccess)
        {
            throw new Exception("Một lỗi đã xảy ra trong quá trình xóa tài khoản nhân viên");
        }

        return new BaseResponse<bool>()
        {
            Status = StatusCodes.Status200OK.ToString(),
            Message = "Xóa nhân viên thành công",
            Data = isSuccess
        };
    }

    public async Task<BaseResponse<GetStaffResponse>> GetProfileStaff()
    {
        Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);

        var staff = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
            selector: s => new GetStaffResponse()
            {
                Id = s.Id,
                UserName = s.UserName,
                Email = s.Email,
                Phone = s.Phone,
                AvatarUrl = s.AvatarUrl,
                FullName = s.FullName,
                Gender = s.Gender,
                DateOfBirth = s.DateOfBirth,
                
            },      
            predicate: s => s.Id.Equals(accountId) && s.Role.Equals(RoleEnum.STAFF.GetDescriptionFromEnum()) && s.IsActive == true) 
                    ?? throw new NotFoundException("Không tìm thấy thông tin nhân viên");

        return new BaseResponse<GetStaffResponse>()
        {
            Status = StatusCodes.Status200OK.ToString(),
            Message = "Lấy thông tin hồ sơ nhân viên thành công",
            Data = staff
        };
    }

    public async Task<BaseResponse<GetStaffResponse>> UpdateStaff(UpdateStaffRequest request)
    {
        Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);

        var staff = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
            predicate: s => s.Id.Equals(accountId) && s.Role.Equals(RoleEnum.STAFF.GetDescriptionFromEnum()) && s.IsActive == true) 
                    ?? throw new NotFoundException("Không tìm thấy thông tin nhân viên");
        
        staff.FullName = request.FullName ?? staff.FullName;
        staff.Gender = request.Gender?.GetDescriptionFromEnum() ?? staff.Gender;
        staff.Phone = request.Phone ?? staff.Phone;
        staff.DateOfBirth = request.DateOfBirth ?? staff.DateOfBirth;
        staff.UpdateAt = TimeUtil.GetCurrentSEATime();
        
        _unitOfWork.GetRepository<Account>().UpdateAsync(staff);
        
        var isSuccess = await _unitOfWork.CommitAsync() > 0;

        if (!isSuccess)
        {
            throw new Exception("Một lỗi đã xảy ra trong quá trình cập nhật tài khoản nhân viên");
        }

        return new BaseResponse<GetStaffResponse>()
        {
            Status = StatusCodes.Status200OK.ToString(),
            Message = "Cập nhật thành công",
            Data = new GetStaffResponse()
            {
                Id = staff.Id,
                UserName = staff.UserName,
                Email = staff.Email,
                Phone = staff.Phone,
                AvatarUrl = staff.AvatarUrl,
                FullName = staff.FullName,
                Gender = staff.Gender,
                DateOfBirth = staff.DateOfBirth,
            }
        };
    }
}