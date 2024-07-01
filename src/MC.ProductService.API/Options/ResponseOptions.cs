namespace MC.ProductService.API.Options
{
    /// <summary>
    /// Defines a generic interface for API responses that encapsulate the primary data in a 'data' property.
    /// This interface follows the JSON API specification for top-level documents as defined at https://jsonapi.org/format/#document-top-level.
    /// </summary>
    /// <typeparam name="T">The type of data being returned in the API response.</typeparam>
    public interface IActionDataResponse<T>
    {
        //
        // Resumen:
        //     Gets or sets the data.
        //
        // Comentarios:
        //     A convention defined by JSONAPI: https://jsonapi.org/format/#document-top-level
        T Data { get; set; }
    }

    /// <summary>
    /// Represents a generic API response structure that encapsulates the returned data within a data property.
    /// This class is intended to provide a consistent response format across different endpoints.
    /// </summary>
    /// <typeparam name="T">The type of the data being encapsulated in the API response.</typeparam>
    public class ActionDataResponse<T> : IActionDataResponse<T>
    {
        public T Data { get; set; }

        public ActionDataResponse(T data)
        {
            Data = data;
        }

        // Implement other properties and methods from IActionResponse if any
    }

    /// <summary>
    /// Represents a structured response for API requests that fail validation.
    /// </summary>
    public class ErrorResponse
    {
        public string? Type { get; set; }
        public string? Title { get; set; }
        public int? Status { get; set; }
        public string? TraceId { get; set; }
    }

    /// <summary>
    /// It provides detailed information about the nature of the errors encountered during the validation process.
    /// </summary>
    public class ValidationErrorResponse : ErrorResponse
    {
        public Dictionary<string, string[]>? Errors { get; set; }
    }
}
