namespace RecognizerGateway.Shared;

public class Error : IEquatable<Error>
{
    public static readonly Error None = new Error(string.Empty, string.Empty);
    public static readonly Error NullValue = new Error("Error.NullValue", "The specified result value is null.");


    public string Code { get; }
    public string Message { get; }

    public Error(string code, string message)
    {
        Code = code;
        Message = message;    
    }


    public virtual bool Equals(Error? other)
    {
        if(other is null){
            return false;
        }

        return Code == other.Code && Message == other.Message;
    }

    public override bool Equals(object? obj) => obj is Error err && Equals(err);

    public static bool operator ==(Error? err1, Error err2){
        if(err1 is null && err2 is null){
            return true;
        }

        if(err1 is null || err2 is null){
            return false;
        }

        return err1.Equals(err2);
    }

    public static bool operator !=(Error? err1, Error err2) => !(err1==err2);

    public override int GetHashCode() => HashCode.Combine(Code, Message);
    public override string ToString() => Code;

}