namespace HotPotato.Results
{
    public abstract class Result
    {
        public abstract string Message { get; }
        public override string ToString() => Message;
    }
}
