using HotPotato.Core.Http;
using HotPotato.OpenApi.Results;
using System.Collections.Generic;

namespace HotPotato.OpenApi.Validators
{
    public interface IHeaderValidator
    {
        ICollection<Result> Validate(HttpHeaders headers);
    }
}
