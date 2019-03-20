using NSwag;

namespace HotPotato.OpenApi.Validators
{
    internal interface IValidationProvider
    {
        SwaggerDocument specDoc { get; set; }
        SwaggerPathItem specPath { get; set; }
        SwaggerOperation specMeth { get; set; }
        SwaggerResponse specResp { get; set; }
    }
}
