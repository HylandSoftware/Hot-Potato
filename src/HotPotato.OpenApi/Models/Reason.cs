namespace HotPotato.OpenApi.Models
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
