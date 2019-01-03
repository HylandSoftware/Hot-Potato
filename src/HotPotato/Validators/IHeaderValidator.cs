using HotPotato.Http;
using HotPotato.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotPotato.Validators
{
    public interface IHeaderValidator
    {
        ICollection<Result> Validate(HttpHeaders headers);
    }
}
