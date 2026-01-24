using UniHub.Domain.Interface;
using UniHub.Dto;

namespace UniHub.Service.Services
{
    public class ReportService : IReportService
    {
        private readonly Random _random = new Random();

        public List<StoreItemReportDto> GetRandomMostCommonItemsByStore()
        {
            string[] Items = { "printer paper", "laptop", "pens", "envelopes", "binder", "notepad", "backpack" };
            string[] Stores = { "Austin", "Denver", "London", "New York", "San Diego", "Seattle" };

            var result = Stores.Select(store => new StoreItemReportDto
            {
                StoreLocation = store,
                Items = Items.ToDictionary(
                    item => item,
                    item => _random.Next(100, 2200)
                )
            }).ToList();

            return result;
        }
    }
}