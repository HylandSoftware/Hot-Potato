using NSwag;

namespace HotPotato.OpenApi.Services
{
    public interface IDocumentProvider
    {
        SwaggerDocument GetSpecDocument();
    }
}
