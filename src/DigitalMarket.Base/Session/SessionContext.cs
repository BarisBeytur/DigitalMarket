using Microsoft.AspNetCore.Http;

namespace DigitalMarket.Base.Session;

public class SessionContext : ISessionContext
{
    public HttpContext HttpContext { get; set; }
    public Session Session { get; set; }
}