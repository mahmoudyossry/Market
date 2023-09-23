using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Dto
{
    public class PermissionTreeDto
    {
        public long key { get; set; }
        public string Label { get; set; }
        public List<PermissionTreeDto> Children { get; set; }
    }
}
