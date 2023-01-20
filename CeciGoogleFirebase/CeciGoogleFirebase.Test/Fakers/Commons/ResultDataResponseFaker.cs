using Bogus;
using CeciGoogleFirebase.Domain.DTO.Commons;
using CeciGoogleFirebase.Domain.DTO.User;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CeciGoogleFirebase.Test.Fakers.Commons
{
    public static class ResultDataResponseFaker
    {
        public static Faker<ResultDataResponse<TData>> ResultDataResponse<TData>(TData data, HttpStatusCode httpStatusCode)
        {
            return new Faker<ResultDataResponse<TData>>()
                .CustomInstantiator(p => new ResultDataResponse<TData>
                {
                    Data = data,
                    StatusCode = httpStatusCode,
                    TotalItems = p.Random.Int(),
                    TotalPages = p.Random.Int()
                });
        }
    }
}
