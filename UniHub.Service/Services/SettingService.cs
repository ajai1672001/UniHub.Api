using Mapster;
using System;
using System.Collections.Generic;
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
        public SettingService(
            IRepository<Setting> settingRepository, 
            IUnitOfWork unitOfWork)
        {
            _settingRepository = settingRepository;
            _unitOfWork = unitOfWork;
        }



        public async Task SaveSettingAsync(SettingDto dto)
        {
            var setting = dto.Adapt<Setting>();

            await _settingRepository.InsertAsync(setting);

            await _unitOfWork.CommitAsync();
        }
    }
}
