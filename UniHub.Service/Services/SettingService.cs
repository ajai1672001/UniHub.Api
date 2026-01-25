using Mapster;
using Microsoft.EntityFrameworkCore;
using UniHub.Domain.Entities;
using UniHub.Domain.Interface;
using UniHub.Dto;
using UniHub.Infrastructure;

namespace UniHub.Service.Services
{
    public class SettingService : ISettingService
    {
        private readonly IRepository<Setting> _settingRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;

        public SettingService(
            IRepository<Setting> settingRepository,
            IUnitOfWork unitOfWork,
            ApplicationDbContext context)
        {
            _settingRepository = settingRepository;
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<BaseResponse<string>> SaveSettingAsync(SettingDto dto)
        {
            var setting = dto.Adapt<Setting>();

            await _settingRepository.InsertAsync(setting);

            await _unitOfWork.CommitAsync();

            return  BaseResponse<string>.Success();
        }

        public async Task<BaseResponse<IEnumerable<SettingDto>>> GetSettingByNameAsync(string name)
        {
            var settings = (await _settingRepository.GetAllAsync()).ToList();

            return BaseResponse<IEnumerable<SettingDto>>.Success(settings.Adapt<List<SettingDto>>());
        }
    }
}