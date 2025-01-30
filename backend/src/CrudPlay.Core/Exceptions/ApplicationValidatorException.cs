namespace CrudPlay.Core.Exceptions;

public class ApplicationValidatorException : Exception
{
    public ApplicationValidatorException(string message) : base(message) { }
}
