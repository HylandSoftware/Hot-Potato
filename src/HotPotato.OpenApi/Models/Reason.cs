namespace HotPotato.OpenApi.Models
{
    public enum Reason
    {
        Unknown,
        InvalidBody,
        InvalidHeaders,
        MissingHeaders,
        MissingPath,
        MissingMethod,
        MissingSpecBody,
        MissingStatusCode,
        UnexpectedBody
    }
}
