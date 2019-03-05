namespace HotPotato.OpenApi.Enums
{
    public class ReasonEnum
    {
        public enum Reason
        {
            Unknown,
            ValidBody,
            InvalidBody,
            ValidHeaders,
            InvalidHeaders,
            MissingHeaders,
            MissingPath,
            MissingMethod,
            MissingStatusCode,
            ExpectedNoBody,
            UnexpectedBody
        }
    }
}
