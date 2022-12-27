using ImageMagick;

namespace CinemaCentral.Services;

public class ImageService : IImageService
{
    private readonly HttpClient _httpClient;
    
    public ImageService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task Resize(string uri, Stream destination, int width, int height)
    {
        var stream = await _httpClient.GetStreamAsync(uri);
        using var image = new MagickImage(stream);
        
        var size = new MagickGeometry(width, height)
        {
            IgnoreAspectRatio = true
        };
        image.Resize(size);
        
        await image.WriteAsync(destination);
    }
}