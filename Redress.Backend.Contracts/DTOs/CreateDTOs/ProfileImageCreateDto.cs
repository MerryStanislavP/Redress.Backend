using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redress.Backend.Contracts.DTOs.CreateDTOs
{
    public class ProfileImageCreateDto
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public Guid ProfileId { get; set; }
    }
}
