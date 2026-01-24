using Mapster;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniHub.Domain.Entities;
using UniHub.Domain.Interface;
using UniHub.Dto;
using UniHub.Infrastructure;

namespace UniHub.Service.Services
{
    public class SettingService:ISettingService
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

            return new BaseResponse<string>(true);
        }

        public async Task<BaseResponse<IEnumerable<SettingDto>>> GetSettingByNameAsync(string name)
        {
            var settings = (await _settingRepository.GetAllAsync()).ToList();

            var ooo = _context.Settings.ToList(); // Data is now in memory (IEnumerable)
            var result = await settings.AsQueryable().ToListAsync(); // BOOM!

            return new BaseResponse<IEnumerable<SettingDto>>(settings.Adapt<List<SettingDto>>());
        }
    }
}
