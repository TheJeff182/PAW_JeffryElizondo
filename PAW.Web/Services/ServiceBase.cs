namespace PAW.Web.Services;

public abstract class ServiceBase
{
    protected string BaseUrl { get; set; } = "http://localhost:5146";

    protected string SetPathUrl(string name) => $"{BaseUrl}/{name}";
}
