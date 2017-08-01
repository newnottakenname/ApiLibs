using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibs.General
{
    public class RequestException : InvalidOperationException
    {
        public readonly string ResponseUri;
        public readonly HttpStatusCode StatusCode;
        public readonly string Content;
        public HttpResponseMessage Response { get; set; }

        public RequestException(HttpResponseMessage response, string responseUri, HttpStatusCode statusCode, string content) : base("A problem occured while trying to access " + responseUri + ". Statuscode: " + statusCode + "\n" + content)
        {
            Response = response;
            ResponseUri = responseUri;
            StatusCode = statusCode;
            Content = content;
        }

    }

    public class NoInternetException : InvalidOperationException
    {
        public NoInternetException(Exception inner) : base(inner.Message, inner) { }
    }

    public class PageNotFoundException : RequestException
    {
        public PageNotFoundException(HttpResponseMessage response, string responseUri, HttpStatusCode statusCode, string content) : base(response, responseUri, statusCode, content)
        {
        }
    }

    public class UnAuthorizedException : RequestException
    {
        public UnAuthorizedException(HttpResponseMessage response, string responseUri, HttpStatusCode statusCode, string content) : base(response, responseUri, statusCode, content)
        {
        }
    }

    public class BadRequestException : RequestException
    {
        public BadRequestException(HttpResponseMessage response, string responseUri, HttpStatusCode statusCode, string content) : base(response, responseUri, statusCode, content)
        {
        }
    }
}
