namespace ATM.Core
{
    public sealed record Error(string Code, int HttpStatusCode, string? Message = null);
}
