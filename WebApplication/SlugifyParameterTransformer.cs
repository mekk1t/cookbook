using Microsoft.AspNetCore.Routing;

namespace KitProjects.MasterChef.WebApplication
{
    public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        public string TransformOutbound(object value) => value?.ToString().ToLower();
    }
}
