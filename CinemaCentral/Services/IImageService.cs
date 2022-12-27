namespace CinemaCentral.Services;

public interface IImageService
{
    public Task Resize(string uri, Stream destination, int width, int height);
}