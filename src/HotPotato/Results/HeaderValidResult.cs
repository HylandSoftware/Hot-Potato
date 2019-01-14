namespace HotPotato.Results
{
    public class HeaderValidResult : Result
    {
        public override string Message { get; }

        public HeaderValidResult(string key, string value)
        {
            Message = Messages.HeaderValueValid(key, value);
        }
    }
}
