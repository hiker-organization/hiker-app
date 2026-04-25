namespace App_Hiker.Model
{
    public class DataResponse<T> : MessageResponse
    {
        public T? data { get; set; } = default(T);
    }
}
