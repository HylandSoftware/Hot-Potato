using HotPotato.OpenApi.SpecificationProvider;
using NSwag;

namespace HotPotato.OpenApi.Validators
{
    internal class ValidationProvider : IValidationProvider
    {
        public SwaggerDocument specDoc { get; set; }
        public SwaggerPathItem specPath { get; set; }
        public SwaggerOperation specMeth { get; set; }
        public SwaggerResponse specResp { get; set; }
        public ValidationProvider(ISpecificationProvider docPro)
        {
            specDoc = docPro.GetSpecDocument();
        }
    }
}
