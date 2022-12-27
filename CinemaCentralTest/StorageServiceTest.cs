using CinemaCentral.Services;

namespace CinemaCentralTest;

public class StorageServiceTest
{
    private StorageService _storageService = new();
    
    [SetUp]
    public void SetUp()
    {
        _storageService = new StorageService();
    }
    
    [Test]
    public async Task StoresObject()
    {
        const string path = "assets/DummyFile.txt";
        await using var file = File.OpenRead(path);
        var destination = "wwwroot" + await _storageService.StoreObject(file, ".txt");

        var content1 = await File.ReadAllBytesAsync(path);
        var content2 = await File.ReadAllBytesAsync(destination);
        Assert.That(content2, Is.EqualTo(content1));
    }

    [TearDown]
    public void TearDown()
    {
        Directory.Delete("wwwroot", true);
    }
}