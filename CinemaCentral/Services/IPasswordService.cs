namespace CinemaCentral.Services;

public interface IPasswordService
{
    public byte[] CreateHash(byte[] password, byte[] salt);
    public byte[] CreateSalt();
    public bool VerifyHash(byte[] password, byte[] salt, byte[] hash);
}