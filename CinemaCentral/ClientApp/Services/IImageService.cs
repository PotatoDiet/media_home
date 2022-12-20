namespace CinemaCentral.ClientApp.Services;

public interface IImageService
{
    public Task Resize(string uri, string destination, int width, int height);
}