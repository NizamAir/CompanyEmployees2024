using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects.StatisticsDTOs
{
    public record ProductShiftStatisticDto
    {
        public string ProductName { get; set; }
        public int Shifts { get; set; }
    }
}
