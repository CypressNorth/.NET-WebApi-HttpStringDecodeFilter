.NET-WebApi-HttpStringDecodeFilter
==================================

An Http ActionFilter that can be used to intercept all incoming PUT and POST requests, and automatically decode all string properties of the object that is sent in the request.

Example
==================================
Suppose you have a model with many string properties:

```C#
public class ShippingInfo
{
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string street { get; set; }
    public string state { get; set; }
    public string city { get; set; }
    public string zipcode { get;set; }
}
```

Now suppose this information gets sent to the server via an AJAX PUT or POST call. In order to ensure none of the values are HTML encoded, you would have to parse each of them out manually. In your WebAPI Controller, you might have something like this:

```C#
public async Task<HttpResponseMessage> Post([FromBody]ShippingInfo info)
{
  info.firstName = WebUtility.HtmlDecode(info.firstName);
  info.lastName = WebUtility.HtmlDecode(info.lastName);
  info.street = WebUtility.HtmlDecode(info.street);
  info.state = WebUtility.HtmlDecode(info.state);
  info.city = WebUtility.HtmlDecode(info.city);
  info.zipcode = WebUtility.HtmlDecode(info.zipcode);
  
   ... whatever else you want to do ...
}
```

If you have a project with numerious API calls, dealing with this throughout the span of a project might become a hassle.

Solution
==================================
We created a simple System.Web.Http.Filters.ActionFilterAttribute that inspects all of the properties of any model that is sent to the server via a PUT or POST request, checks to see if the property is of type string, and if so, html decodes it.

To use this action filter on all methods inside a WebAPI controller, simply decorate your class with this method.

```C#
[HttpStringDecodeFilter]
public class AddressController : ApiController
{
    // GET
    public async Task<HttpResponseMessage> Get(int id)
    {
      ...
    }
    
    // POST
    public async Task<HttpResponseMessage> Post([FromBody]ShippingInfo info)
    {
      ...
    }
    
    // PUT
    public async Task<HttpResponseMessage> Put([FromBody]ShippingInfo info)
    {
      ...
    }
}
```

To use this action filter on a specific method, you can decorate that method with the attribute:

```C#
public class AddressController : ApiController
{
    // GET
    public async Task<HttpResponseMessage> Get(int id)
    {
      ...
    }
    
    // POST
    [HttpStringDecodeFilter]
    public async Task<HttpResponseMessage> Post([FromBody]ShippingInfo info)
    {
      ...
    }
}
```
