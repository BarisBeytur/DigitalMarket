
using Microsoft.AspNetCore.Http;

namespace DigitalMarket.Base.Session;

public interface ISessionContext
{
    public HttpContext HttpContext { get; set; }
    public Session Session { get; set; }
}