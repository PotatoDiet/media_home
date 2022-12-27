namespace CinemaCentral.Services;

public class StorageService : IStorageService
{
    private const string Root = "wwwroot/objects";

    public StorageService()
    {
        Directory.CreateDirectory(Root);
    }
    
    public async Task<string> StoreObject(Stream stream, string extension)
    {
        var id = Guid.NewGuid().ToString();
        var filename = Path.ChangeExtension(id, extension);
        var destination = Path.Combine(Root, filename);

        await using var destinationStream = File.Create(destination);
        await stream.CopyToAsync(destinationStream);
        
        return destination.Replace("wwwroot", "");
    }
}