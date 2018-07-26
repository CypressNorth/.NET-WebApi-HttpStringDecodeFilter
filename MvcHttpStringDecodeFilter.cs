using System.Net;
using System.Reflection;
using System.Web.Mvc;

namespace CN.MVC.Filters
{
    public class MVCHttpStringDecodeFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            base.OnActionExecuting(actionContext);
            // If the request is either a PUT or POST, attempt to decode all strings
            if (actionContext.HttpContext.Request.HttpMethod.ToString() == WebRequestMethods.Http.Post
                || actionContext.HttpContext.Request.HttpMethod.ToString() == WebRequestMethods.Http.Put)
            {
                // For each of the items in the PUT/POST
                foreach (var item in actionContext.ActionParameters.Values)
                {
                    try
                    {
                        // Get the type of the object
                        Type type = item.GetType();

                        // For each property of this object, html decode it if it is of type string
                        //NOTE: this needs to be modified to account for nested/complex objects
                        foreach (PropertyInfo propertyInfo in type.GetProperties())
                        {
                            var prop = propertyInfo.GetValue(item);
                            if (prop != null && prop.GetType() == typeof(string))
                            {
                                propertyInfo.SetValue(item, WebUtility.HtmlDecode((string)prop));
                            }
                        }
                    }
                    catch { }
                }
            }
        }
    }
}