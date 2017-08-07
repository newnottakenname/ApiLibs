using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ApiLibs.General;
using System.Net.Http;
using Newtonsoft.Json;

namespace ApiLibs
{
    public abstract class Service
    {
        internal HttpClient Client;
        private readonly List<Param> _standardParameter = new List<Param>();
        private readonly List<Param> _standardHeader = new List<Param>();

        public Service(string hostUrl)
        {
            Client = new HttpClient { BaseAddress = new Uri(hostUrl) };
        }

        internal void AddStandardParameter(Param p)
        {
            _standardParameter.Add(p);
        }

        internal void AddStandardHeader(Param p)
        {
            _standardHeader.Add(p);
        }

        internal void AddStandardHeader(string name, string content)
        {
            AddStandardHeader(new Param(name, content));
        }

        internal void RemoveStandardHeader(string name)
        {
            _standardHeader.RemoveAll(p => p.Name == name);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("SonarLint", "S2228:Console logging should not be used", Justification = "I can")]
        internal void UpdateParameterIfExists(Param p)
        {
            foreach (Param para in _standardParameter)
            {
                if (para.Name == p.Name)
                {
                    Console.WriteLine(para.Name + " was: " + para.Value + " is: " + p.Value);
                    para.Value = p.Value;
                }
            }
        }

        internal void UpdateHeaderIfExists(string name, string value)
        {
            UpdateHeaderIfExists(new Param(name, value));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("SonarLint", "S2228:Console logging should not be used", Justification = "I can")]
        internal void UpdateHeaderIfExists(Param p)
        {
            foreach (Param para in _standardHeader)
            {
                if (para.Name == p.Name)
                {
                    Console.WriteLine(para.Name + " was: " + para.Value + " is: " + p.Value);
                    para.Value = p.Value;

                }
            }
        }

        

        internal async Task<T> MakeRequest<T>(string url, Call call = Call.GET, List<Param> parameters = null, List<Param> header = null, object content = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return Convert<T>(await HandleRequest(url, call, parameters, header, content, statusCode));
        }

        internal virtual async Task<string> HandleRequest(string url, Call call = Call.GET, List<Param> parameters = null, List < Param> headers = null, object content = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            headers = headers ?? new List<Param>();
            parameters = parameters ?? new List<Param>();

            
            
            parameters.AddRange(_standardParameter);
            var encoded =
                new FormUrlEncodedContent(
                    parameters.FindAll(i => !(i is OParam) || i.Value != null).Select(i => new KeyValuePair<string, string>(i.Name, i.Value)));

            HttpRequestMessage request = null;

            if (call == Call.POST)
            {
                request = new HttpRequestMessage(Convert(call), url) {Content = encoded};
            }
            else
            {
                request = new HttpRequestMessage(Convert(call), url + "?" + encoded.ReadAsStringAsync().Result);
            }

            if (content != null)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };
                request.Content = new StringContent(JsonConvert.SerializeObject(content, settings));
                //parameters.Add(new Param("application/json", JsonConvert.SerializeObject(content, settings)));
                //headers.Add(new Param("Content-Type", "application/json"));
            }

            //Add all headers
            headers.ForEach(p => request.Headers.Add(p.Name, p.Value));
            _standardHeader.ForEach(p => request.Headers.Add(p.Name, p.Value));

            return await ExcecuteRequest(request, statusCode);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("SonarLint", "S2228:Console logging should not be used", Justification = "I can")]
        internal async Task<string> ExcecuteRequest(HttpRequestMessage request, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            Debug.Assert(Client != null, "Client != null");
            HttpResponseMessage resp = await Client.SendAsync(request);
            string responseContent = await resp.Content.ReadAsStringAsync();
            if (resp.StatusCode != statusCode)
            {
                //No internet
                /*if (resp.E != null)
                {
                    if (resp.ErrorException is System.Net.WebException)
                    {

                        throw new NoInternetException(resp.ErrorException);
                    }

                    throw resp.ErrorException;
                }*/
                RequestException toThrow;
                switch (resp.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        toThrow = new PageNotFoundException(resp, null, resp.StatusCode, responseContent);
                        break;
                    case HttpStatusCode.Unauthorized:
                        toThrow = new UnAuthorizedException(resp, null, resp.StatusCode, responseContent);
                        break;
                    case HttpStatusCode.BadRequest:
                        toThrow = new BadRequestException(resp, null, resp.StatusCode, responseContent);
                        break;
                    default:
                        toThrow = new RequestException(resp, null, resp.StatusCode, responseContent);
                        break;

                }
                Console.WriteLine("--Exception Log---");
                Console.WriteLine("URL:\n" +  resp.RequestMessage.RequestUri);
                Console.WriteLine("Status Code:\n" + toThrow.StatusCode);
                Console.WriteLine("Response Message:\n" + toThrow.Content);
                Console.WriteLine("Full StackTrace:\n" + toThrow.StackTrace);
                Console.WriteLine("---END---\n");
                throw toThrow;
            }
            return responseContent;
        }

        internal T Convert<T>(HttpResponseMessage resp)
        {
            return Convert<T>(resp.Content.ReadAsStringAsync().Result);
        }

        internal T Convert<T>(string text)
        {
            T returnObj = JsonConvert.DeserializeObject<T>(text);
            if (returnObj is ObjectSearcher)
            {
                //Enable better OOP
                (returnObj as ObjectSearcher).Search(this);
            }
            return returnObj;
        }

        private HttpMethod Convert(Call m)
        {
            switch (m)
            {
                case Call.POST:
                    return HttpMethod.Post;
                case Call.PATCH:
                    return new HttpMethod("PATCH");
                case Call.DELETE:
                    return HttpMethod.Delete;
                case Call.PUT:
                    return HttpMethod.Put;
                default:
                    return HttpMethod.Get;
            }
        }

        internal void SetBaseUrl(string baseurl)
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri(baseurl);
        }

        internal void Print(HttpResponseMessage resp)
        {
            Console.WriteLine(resp.Content);
        }

        
    }
}

enum Call
{
    POST,
    GET,
    PATCH,
    DELETE,
    PUT
}
