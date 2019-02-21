using HotPotato.Validators;
using System.Collections.Generic;

namespace HotPotato.Results
{
    public static class ResultsMethods
    {
        public static List<ValidationError> GetInvalidReasons(Result result)
        {
            InvalidResult invResult = (InvalidResult)result;
            return invResult.Reasons;
        }
    }
}
