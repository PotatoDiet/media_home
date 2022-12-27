namespace CinemaCentral.Services;

public interface IStorageService
{
    public Task<string> StoreObject(Stream stream, string extension);
}