using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;

namespace CeciGoogleFirebase.Domain.DTO.Commons
{
    public class ResultDataResponse <TData>
    {
        /// <summary>
        /// Total pages
        /// </summary>
        public int TotalPages { get; set; }
        
        /// <summary>
        /// Total items
        /// </summary>
        public int TotalItems { get; set; }
        
        /// <summary>
        /// Data
        /// </summary>
        public TData Data { get; set; }
        
        private Exception _exception { get; set; }
        
        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        
        [JsonIgnore]
        public Exception Exception
        {
            get
            {
                return _exception;
            }
            set
            {
                if (value != null)
                {
                    StatusCode = HttpStatusCode.InternalServerError;
                    _exception = value;
                }
            }
        }
    }
}
