namespace HotPotato.Results
{
    public class BodyValidResult : Result
    {
        public override string Message { get; }
        public BodyValidResult(string content)
        {
            Message = Messages.BodyValid(content);
        }
    }
}
