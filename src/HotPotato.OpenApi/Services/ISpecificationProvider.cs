using NSwag;

namespace HotPotato.OpenApi.Services
{
    public interface ISpecificationProvider
    {
        SwaggerDocument GetSpecDocument();
    }
}
