using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Models
{
   public class ApiTable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<ApiTableSchema> Schema { get; set; }

        public DateTime Created { get; set; }
    }
}
