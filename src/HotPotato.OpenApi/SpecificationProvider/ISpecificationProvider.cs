using NSwag;

namespace HotPotato.OpenApi.SpecificationProvider
{
    public interface ISpecificationProvider
    {
        SwaggerDocument GetSpecDocument();
    }
}
