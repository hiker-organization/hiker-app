namespace App_Hiker.Model.Api
{
    public class DataResponse<T> : MessageResponse
    {
        public T? data { get; set; } = default(T);
    }
}
