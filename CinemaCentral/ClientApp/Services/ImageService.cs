using ImageMagick;

namespace CinemaCentral.ClientApp.Services;

public class ImageService : IImageService
{
    private readonly HttpClient _httpClient;
    
    public ImageService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task Resize(string uri, string destination, int width, int height)
    {
        var stream = await _httpClient.GetStreamAsync(uri);
        using var image = new MagickImage(stream);
        
        var size = new MagickGeometry(width, height)
        {
            IgnoreAspectRatio = true
        };
        image.Resize(size);
        
        if (File.Exists(destination))
            File.Delete(destination);
        await image.WriteAsync(destination);
    }
}