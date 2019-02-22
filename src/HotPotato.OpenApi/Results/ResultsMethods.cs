using HotPotato.OpenApi.Validators;
using System.Collections.Generic;

namespace HotPotato.OpenApi.Results
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
