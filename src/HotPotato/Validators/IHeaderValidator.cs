using HotPotato.Core.Http;
using HotPotato.Results;
using System.Collections.Generic;

namespace HotPotato.Validators
{
    public interface IHeaderValidator
    {
        ICollection<Result> Validate(HttpHeaders headers);
    }
}
