﻿using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.ExamMatrix;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.ExamMatrix;
using MathExamGenerator.Model.Utils;
using MathExamGenerator.Repository.Interface;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Service.Implement
{
    public class ExamMatrixService : BaseService<ExamMatrixService>, IExamMatrixService
    {
        public ExamMatrixService(IUnitOfWork<MathExamGeneratorContext> unitOfWork, ILogger<ExamMatrixService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor): base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse<GetExamMatrixResponse>> CreateExamMatrix(CreateExamMatrixWithStructureRequest request)
        {
            if (request == null || request.Sections == null || !request.Sections.Any())
            {
                return new BaseResponse<GetExamMatrixResponse>
                {
                    Status = StatusCodes.Status400BadRequest.ToString(),
                    Message = "Dữ liệu tạo ma trận không hợp lệ hoặc không có section nào."
                };
            }

            foreach (var sectionReq in request.Sections)
            {
                if (sectionReq.TotalQuestions <= 0 || sectionReq.TotalScore <= 0)
                {
                    return new BaseResponse<GetExamMatrixResponse>
                    {
                        Status = StatusCodes.Status400BadRequest.ToString(),
                        Message = $"Section '{sectionReq.SectionName}' có tổng số câu hoặc tổng điểm không hợp lệ."
                    };
                }

                if (sectionReq.Details == null || !sectionReq.Details.Any())
                {
                    return new BaseResponse<GetExamMatrixResponse>
                    {
                        Status = StatusCodes.Status400BadRequest.ToString(),
                        Message = $"Section '{sectionReq.SectionName}' không có chi tiết section."
                    };
                }

                int totalQuestionCount = sectionReq.Details.Sum(d => d.QuestionCount);
                double totalScore = sectionReq.Details.Sum(d => d.QuestionCount * d.ScorePerQuestion);

                if (totalQuestionCount != sectionReq.TotalQuestions)
                {
                    return new BaseResponse<GetExamMatrixResponse>
                    {
                        Status = StatusCodes.Status400BadRequest.ToString(),
                        Message = $"Tổng số câu của các detail không bằng TotalQuestions trong section '{sectionReq.SectionName}'."
                    };
                }

                if (totalScore != sectionReq.TotalScore)
                {
                    return new BaseResponse<GetExamMatrixResponse>
                    {
                        Status = StatusCodes.Status400BadRequest.ToString(),
                        Message = $"Tổng điểm các detail không bằng TotalScore trong section '{sectionReq.SectionName}'."
                    };
                }

                foreach (var detail in sectionReq.Details)
                {
                    if(detail.BookChapterId.HasValue && detail.BookTopicId.HasValue)
                    {
                        return new BaseResponse<GetExamMatrixResponse>
                        {
                            Status = StatusCodes.Status400BadRequest.ToString(),
                            Message = $"Chi tiết trong section '{sectionReq.SectionName}' có cùng lúc cả BookChapterId và BookTopicId. Chỉ chọn 1 trong 2."
                        };
                    }

                    if (!detail.BookChapterId.HasValue && !detail.BookTopicId.HasValue)
                    {
                        return new BaseResponse<GetExamMatrixResponse>
                        {
                            Status = StatusCodes.Status400BadRequest.ToString(),
                            Message = $"Chi tiết trong section '{sectionReq.SectionName}' không có BookChapterId hoặc BookTopicId. Phải có 1 trong 2."
                        };
                    }

                    if (detail.QuestionCount <= 0 || detail.ScorePerQuestion <= 0)
                    {
                        return new BaseResponse<GetExamMatrixResponse>
                        {
                            Status = StatusCodes.Status400BadRequest.ToString(),
                            Message = $"Chi tiết trong section '{sectionReq.SectionName}' có giá trị QuestionCount hoặc ScorePerQuestion không hợp lệ."
                        };
                    }
                }
            }

            var subject = await _unitOfWork.GetRepository<Subject>().SingleOrDefaultAsync(
                predicate: x => x.IsActive == true);

            if (subject == null)
            {
                return new BaseResponse<GetExamMatrixResponse>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không có môn học được lưu trữ cả.",
                };
            }

            var matrix = _mapper.Map<ExamMatrix>(request);
            matrix.SubjectId = subject.Id;
            await _unitOfWork.GetRepository<ExamMatrix>().InsertAsync(matrix);

            foreach (var sectionReq in request.Sections)
            {
                var section = _mapper.Map<MatrixSection>(sectionReq);
                section.ExamMatrixId = matrix.Id;
                await _unitOfWork.GetRepository<MatrixSection>().InsertAsync(section);

                foreach (var detailReq in sectionReq.Details)
                {
                    if (detailReq.BookChapterId.HasValue)
                    {
                        var bookChapter = await _unitOfWork.GetRepository<BookChapter>().SingleOrDefaultAsync(
                            predicate: x => x.Id == detailReq.BookChapterId && x.IsActive == true);

                        if (bookChapter == null)
                        {
                            return new BaseResponse<GetExamMatrixResponse>
                            {
                                Status = StatusCodes.Status404NotFound.ToString(),
                                Message = $"Không tìm thấy BookChapter trong 1 Detail của section '{sectionReq.SectionName}'.",
                            };
                        }
                    }

                    if (detailReq.BookTopicId.HasValue)
                    {
                        var bookTopic = await _unitOfWork.GetRepository<BookTopic>().SingleOrDefaultAsync(
                            predicate: x => x.Id == detailReq.BookTopicId && x.IsActive == true);

                        if (bookTopic == null)
                        {
                            return new BaseResponse<GetExamMatrixResponse>
                            {
                                Status = StatusCodes.Status404NotFound.ToString(),
                                Message = $"Không tìm thấy BookTopic trong 1 Detail của section '{sectionReq.SectionName}'.",
                            };
                        }
                    }

                    var detail = _mapper.Map<MatrixSectionDetail>(detailReq);
                    detail.MatrixSectionId = section.Id;
                    await _unitOfWork.GetRepository<MatrixSectionDetail>().InsertAsync(detail);
                }
            }

            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccessfully)
            {
                return new BaseResponse<GetExamMatrixResponse>
                {
                    Status = StatusCodes.Status500InternalServerError.ToString(),
                    Message = "Một lỗi đã xảy ra trong quá trình tạo ma trận",
                };
            }

            return new BaseResponse<GetExamMatrixResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Tạo ma trận thành công.",
                Data = _mapper.Map<GetExamMatrixResponse>(matrix)
            };
        }

        public async Task<BaseResponse<bool>> DeleteExamMatrix(Guid id)
        {
            var matrix = await _unitOfWork.GetRepository<ExamMatrix>().SingleOrDefaultAsync(
                predicate: em => em.Id == id && em.IsActive == true);

            if (matrix == null)
            {
                return new BaseResponse<bool>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy ma trận để xoá.",
                    Data = false
                };
            }

            var isBeingUsed = await _unitOfWork.GetRepository<Exam>().SingleOrDefaultAsync(
                predicate: x => x.ExamMatrixId == id && x.IsActive == true);

            if (isBeingUsed != null)
            {
                return new BaseResponse<bool>
                {
                    Status = StatusCodes.Status400BadRequest.ToString(),
                    Message = "Không thể xoá ma trận vì vẫn còn đề thi đang sử dụng nó.",
                    Data = false
                };
            }

            matrix.IsActive = false;
            matrix.UpdateAt = TimeUtil.GetCurrentSEATime();
            matrix.DeleteAt = TimeUtil.GetCurrentSEATime();
            _unitOfWork.GetRepository<ExamMatrix>().UpdateAsync(matrix);

            var sections = await _unitOfWork.GetRepository<MatrixSection>().GetListAsync(
                predicate: x => x.ExamMatrixId == id && x.IsActive == true);

            foreach (var section in sections)
            {
                section.IsActive = false;
                section.UpdateAt = TimeUtil.GetCurrentSEATime();
                section.DeleteAt = TimeUtil.GetCurrentSEATime();
                _unitOfWork.GetRepository<MatrixSection>().UpdateAsync(section);

                var details = await _unitOfWork.GetRepository<MatrixSectionDetail>().GetListAsync(
                    predicate: d => d.MatrixSectionId == section.Id && d.IsActive == true);

                foreach (var detail in details)
                {
                    detail.IsActive = false;
                    detail.UpdateAt = TimeUtil.GetCurrentSEATime();
                    detail.DeleteAt = TimeUtil.GetCurrentSEATime();
                    _unitOfWork.GetRepository<MatrixSectionDetail>().UpdateAsync(detail);
                }
            }

            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccessfully)
            {
                return new BaseResponse<bool>
                {
                    Status = StatusCodes.Status500InternalServerError.ToString(),
                    Message = "Một lỗi xảy ra trong quá trình xóa ma trận.",
                    Data = isSuccessfully
                };
            }

            return new BaseResponse<bool>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Xoá ma trận thành công.",
                Data = isSuccessfully
            };
        }

        public async Task<BaseResponse<IPaginate<GetExamMatrixResponse>>> GetAllExamMatrix(int page, int size)
        {
            if (page < 1 || size < 1)
            {
                return new BaseResponse<IPaginate<GetExamMatrixResponse>>
                {
                    Status = StatusCodes.Status400BadRequest.ToString(),
                    Message = "Trang hoặc kích thước không hợp lệ.",
                    Data = null
                };
            }

            var result = await _unitOfWork.GetRepository<ExamMatrix>().GetPagingListAsync(
                selector: x => _mapper.Map<GetExamMatrixResponse>(x),
                page: page,
                size: size,
                orderBy: o => o.OrderByDescending(x => x.CreateAt),
                predicate: x => x.IsActive == true,
                include: x => x.Include(e => e.Subject));

            return new BaseResponse<IPaginate<GetExamMatrixResponse>>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy danh sách ma trận đề thi thành công.",
                Data = result
            };
        }

        public async Task<BaseResponse<GetExamMatrixResponse>> GetById(Guid id)
        {
            var result = await _unitOfWork.GetRepository<ExamMatrix>().SingleOrDefaultAsync(
                predicate: x => x.IsActive == true && x.Id == id,
                include: x => x.Include(e => e.Subject));

            if (result == null)
            {
                return new BaseResponse<GetExamMatrixResponse>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy ma trận đề thi."
                };
            }

            return new BaseResponse<GetExamMatrixResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy ma trận đề thi thành công.",
                Data = _mapper.Map<GetExamMatrixResponse>(result)
            };
        }

        public async Task<BaseResponse<ExamMatrixStructureResponse>> GetMatrixStructure(Guid id)
        {
            var matrix = await _unitOfWork.GetRepository<ExamMatrix>().SingleOrDefaultAsync(
                predicate: x => x.Id == id && x.IsActive == true,
                include: m => m.Include(x => x.MatrixSections)
                               .ThenInclude(s => s.MatrixSectionDetails));

            if (matrix == null)
            {
                return new BaseResponse<ExamMatrixStructureResponse>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy ma trận.",
                };
            }

            var response = _mapper.Map<ExamMatrixStructureResponse>(matrix);
            return new BaseResponse<ExamMatrixStructureResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy cấu trúc ma trận thành công.",
                Data = response
            };
        }

        public async Task<BaseResponse<bool>> UpdateExamMatrix(Guid id, UpdateExamMatrixWithStructureRequest request)
        {
            var matrixRepo = _unitOfWork.GetRepository<ExamMatrix>();
            var matrix = await matrixRepo.SingleOrDefaultAsync(
                predicate: x => x.Id == id && x.IsActive == true);

            if (matrix == null)
            {
                return new BaseResponse<bool>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy ma trận để cập nhật."
                };
            }

            var examUsingMatrix = await _unitOfWork.GetRepository<Exam>().SingleOrDefaultAsync(
                predicate: x => x.ExamMatrixId == id && x.IsActive == true);

            if (examUsingMatrix != null)
            {
                return new BaseResponse<bool>
                {
                    Status = StatusCodes.Status400BadRequest.ToString(),
                    Message = "Không thể cập nhật ma trận vì đang được sử dụng trong đề thi."
                };
            }

            if (request == null || request.Sections == null || !request.Sections.Any())
            {
                return new BaseResponse<bool>
                {
                    Status = StatusCodes.Status400BadRequest.ToString(),
                    Message = "Dữ liệu cập nhật ma trận không hợp lệ hoặc không có section nào."
                };
            }

            foreach (var sectionReq in request.Sections)
            {
                if (sectionReq.TotalQuestions <= 0 || sectionReq.TotalScore <= 0)
                {
                    return new BaseResponse<bool>
                    {
                        Status = StatusCodes.Status400BadRequest.ToString(),
                        Message = $"Section '{sectionReq.SectionName}' có tổng số câu hoặc tổng điểm không hợp lệ."
                    };
                }

                if (sectionReq.Details == null || !sectionReq.Details.Any())
                {
                    return new BaseResponse<bool>
                    {
                        Status = StatusCodes.Status400BadRequest.ToString(),
                        Message = $"Section '{sectionReq.SectionName}' không có chi tiết section."
                    };
                }

                int totalQuestionCount = sectionReq.Details.Sum(d => d.QuestionCount);
                double totalScore = sectionReq.Details.Sum(d => d.QuestionCount * d.ScorePerQuestion);

                if (totalQuestionCount != sectionReq.TotalQuestions)
                {
                    return new BaseResponse<bool>
                    {
                        Status = StatusCodes.Status400BadRequest.ToString(),
                        Message = $"Tổng số câu của các detail không bằng TotalQuestions trong section '{sectionReq.SectionName}'."
                    };
                }

                if (totalScore != sectionReq.TotalScore)
                {
                    return new BaseResponse<bool>
                    {
                        Status = StatusCodes.Status400BadRequest.ToString(),
                        Message = $"Tổng điểm các detail không bằng TotalScore trong section '{sectionReq.SectionName}'."
                    };
                }

                foreach (var detail in sectionReq.Details)
                {
                    if (detail.QuestionCount <= 0 || detail.ScorePerQuestion <= 0)
                    {
                        return new BaseResponse<bool>
                        {
                            Status = StatusCodes.Status400BadRequest.ToString(),
                            Message = $"Chi tiết trong section '{sectionReq.SectionName}' có giá trị QuestionCount hoặc ScorePerQuestion không hợp lệ."
                        };
                    }

                    var topic = await _unitOfWork.GetRepository<BookTopic>().SingleOrDefaultAsync(
                        predicate: t => t.Id == detail.BookTopicId && t.BookChapterId == detail.BookChapterId && t.IsActive == true);

                    if (topic == null)
                    {
                        return new BaseResponse<bool>
                        {
                            Status = StatusCodes.Status400BadRequest.ToString(),
                            Message = $"Topic không hợp lệ hoặc không thuộc chapter trong section '{sectionReq.SectionName}'."
                        };
                    }

                    var chapter = await _unitOfWork.GetRepository<BookChapter>().SingleOrDefaultAsync(
                        predicate: c => c.Id == detail.BookChapterId && c.IsActive == true,
                        include: c => c.Include(b => b.SubjectBook));

                    if (chapter == null || chapter.SubjectBook?.SubjectId != request.SubjectId)
                    {
                        return new BaseResponse<bool>
                        {
                            Status = StatusCodes.Status400BadRequest.ToString(),
                            Message = $"Chapter không thuộc đúng môn học trong section '{sectionReq.SectionName}'."
                        };
                    }
                }
            }

            // Cập nhật matrix
            matrix.Name = request.Name ?? matrix.Name;
            matrix.Grade = request.Grade ?? matrix.Grade;
            matrix.Description = request.Description ?? matrix.Description;
            matrix.IsActive = true;
            matrix.UpdateAt = TimeUtil.GetCurrentSEATime();
            matrixRepo.UpdateAsync(matrix);

            // Xoá sections & details cũ
            var sectionRepo = _unitOfWork.GetRepository<MatrixSection>();
            var detailRepo = _unitOfWork.GetRepository<MatrixSectionDetail>();

            var oldSections = await sectionRepo.GetListAsync(
                predicate: s => s.ExamMatrixId == matrix.Id);
            foreach (var sec in oldSections)
            {
                var details = await detailRepo.GetListAsync(
                    predicate: d => d.MatrixSectionId == sec.Id);
                foreach (var d in details)
                {
                    detailRepo.DeleteAsync(d);
                }
                sectionRepo.DeleteAsync(sec);
            }

            // Thêm section & detail mới
            foreach (var sectionReq in request.Sections)
            {
                var section = _mapper.Map<MatrixSection>(sectionReq);
                section.Id = Guid.NewGuid(); 
                section.ExamMatrixId = matrix.Id;
                await sectionRepo.InsertAsync(section);

                foreach (var detailReq in sectionReq.Details)
                {
                    var detail = _mapper.Map<MatrixSectionDetail>(detailReq);
                    detail.Id = Guid.NewGuid(); 
                    detail.MatrixSectionId = section.Id;
                    await detailRepo.InsertAsync(detail);
                }
            }

            bool isSuccess = await _unitOfWork.CommitAsync() > 0;

            return new BaseResponse<bool>
            {
                Status = isSuccess ? StatusCodes.Status200OK.ToString() : StatusCodes.Status500InternalServerError.ToString(),
                Message = isSuccess ? "Cập nhật ma trận thành công." : "Cập nhật ma trận thất bại.",
                Data = isSuccess
            };
        }

    }
}
    