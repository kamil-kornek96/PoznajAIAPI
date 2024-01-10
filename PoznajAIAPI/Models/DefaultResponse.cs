namespace PoznajAI.Models
{
    public class DefaultResponse<T>
    {
        public DefaultResponse()
        {
            
        }
        public DefaultResponse(int status, string message, bool success, T? data)
        {
            Status = status; 
            Message = message; 
            Success = success; 
            Data = data;
        }

        public DefaultResponse(int status, string message, bool success)
        {
            Status = status;
            Message = message;
            Success = success;
        }
        public string Message { get; set; }
        public int Status { get; set; }
        public bool Success { get; set; }
        public T? Data { get; set; }
    }
}
