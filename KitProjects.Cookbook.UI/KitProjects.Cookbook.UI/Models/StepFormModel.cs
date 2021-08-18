using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitProjects.Cookbook.UI.Models
{
    public class StepFormModel
    {
        public long StepId { get; set; }
        public IFormFile Picture { get; set; }
    }
}
