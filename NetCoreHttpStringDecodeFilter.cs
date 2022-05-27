/* Credit: @JesperHenriksen-Groupcare */
using System;
using System.Net;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CN.NetCore.Filters
{
    public class HttpStringDecodeFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            base.OnActionExecuting(actionContext);
            
            // If the request is either a PUT or POST, attempt to decode all strings
            if (actionContext.HttpContext.Request.Method.ToString() == System.Net.WebRequestMethods.Http.Post
                || actionContext.HttpContext.Request.Method.ToString() == System.Net.WebRequestMethods.Http.Put)
            {
                // For each of the items in the PUT/POST
                foreach (var item in actionContext.ActionArguments.Values)
                {
                    try {
                        // Get the type of the object
                        Type type = item.GetType();
                        // For each property of this object, html decode it if it is of type string
                        foreach (PropertyInfo propertyInfo in type.GetProperties())
                        {
                            var prop = propertyInfo.GetValue(item);
                            if (prop != null && prop.GetType() == typeof(string))
                            {
                                propertyInfo.SetValue(item, System.Uri.UnescapeDataString((string)prop));
                            }
                        }
                    } catch {}
                }
            }
        }
    }
}
