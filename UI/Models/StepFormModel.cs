using Microsoft.AspNetCore.Http;

namespace KitProjects.Cookbook.UI.Models
{
    public class StepFormModel
    {
        public int StepOrder { get; set; }
        public IFormFile Picture { get; set; }
    }
}
