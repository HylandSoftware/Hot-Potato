using NSwag;

namespace HotPotato.OpenApi.Models
{
    public interface IDocumentProvider
    {
        SwaggerDocument GetSpecDocument();
    }
}
