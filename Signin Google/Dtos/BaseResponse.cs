namespace Signin_Google.Dtos;

public class BaseResponse<T>
{
    public T Data { get; set; }
    public List<string> Errors { get; set; }

    public BaseResponse(T data, List<string> errors = null)
    {
        Data = data;
        Errors = errors ?? new List<string>();
    }
}
