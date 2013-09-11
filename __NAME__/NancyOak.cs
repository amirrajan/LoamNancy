using System;
using System.Collections.Generic;
using System.Linq;
using Nancy;
using Oak;

namespace __NAME__
{
    public static class NancyOak
    {
        public static object Dto(this NancyModule module)
        {
            var dictionary = (module.Request.Form as IDictionary<string, object>)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => ((DynamicDictionaryValue)kvp.Value).Value);

            return (dictionary).ToPrototype();
        }

        public static Response Json(object o)
        {
            var response = (Response)DynamicToJson.Convert(o);
            response.ContentType = "application/json";
            return response;
        }

        public static Response OK(this NancyModule module)
        {
            return new Response { StatusCode = HttpStatusCode.OK };
        }
    }
}
