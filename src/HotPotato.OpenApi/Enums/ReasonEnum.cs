namespace HotPotato.OpenApi.Enums
{
    public class ReasonEnum
    {
        public enum Reason
        {
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
