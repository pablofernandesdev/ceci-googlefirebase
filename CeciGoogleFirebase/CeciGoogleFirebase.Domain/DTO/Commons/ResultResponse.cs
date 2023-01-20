using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CeciGoogleFirebase.Domain.DTO.Commons
{
    public class ResultResponse
    {
        public string Message { get; set; }

        public string Details { get; set; }

        public bool IsSuccessStatusCode => (int)StatusCode >= 200 && (int)StatusCode <= 299;

        private Exception _exception { get; set; }

        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        
        [JsonIgnore]
        public Exception Exception
        {
            get {
                return _exception;            
            }
            set {
                if (value != null)
                {
                    StatusCode = HttpStatusCode.InternalServerError;
                    Details = value.Message;
                    _exception = value;
                }
            }
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }

    public class ResultResponse<TData> : ResultResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TData Data { get; set; }
    }
}
