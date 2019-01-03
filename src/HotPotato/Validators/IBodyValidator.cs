using HotPotato.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotPotato.Validators
{
    public interface IBodyValidator
    {
        Result Validate(string content);
    }
}
