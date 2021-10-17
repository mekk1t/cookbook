using Microsoft.AspNetCore.Http;

namespace KP.Cookbook.UI.Models
{
    public class StepFormModel
    {
        public int StepOrder { get; set; }
        public IFormFile Picture { get; set; }
    }
}
