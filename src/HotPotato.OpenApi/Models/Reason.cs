namespace HotPotato.OpenApi.Models
{
    public enum Reason
    {
        Unknown,
        InvalidBody,
        InvalidHeaders,
        MissingBody,
        MissingHeaders,
        MissingMethod,
        MissingPath,
        MissingContentType,
        MissingStatusCode,
        UnexpectedBody
    }
}
