namespace PoznajAI.Models
{
    public class DefaultResponse<T>
    {
        public DefaultResponse()
        {

        }
        public DefaultResponse(int status, string message, bool success, T data)
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

        /// <summary>
        /// Message that informs about error that occur
        /// </summary>
        /// <example>User not found</example>
        public string Message { get; set; }
        /// <summary>
        /// Status code
        /// </summary>
        /// <example>404</example>
        public int Status { get; set; }
        /// <summary>
        /// If succes API cal
        /// </summary>
        /// <example>false</example>
        public bool Success { get; set; }
        /// <summary>
        /// Data (optional). This field accepts an object.
        /// </summary>
        public T Data { get; set; }
    }
}
