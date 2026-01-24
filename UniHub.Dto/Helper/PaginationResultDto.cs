using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniHub.Dto
{
    public class PaginationResultDto<T>
    {
        public int TotalCount { get; set; }
        public int Count { get; set; }
        public IEnumerable<T> Result { get; set; } = Enumerable.Empty<T>();
    }
}
