using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogs.Catalogs.Application.DTO
{
    public class CatalogResponse
    {
        public Guid CatalogId { get; set; }
        public string Name { get; set; }
    }
}
