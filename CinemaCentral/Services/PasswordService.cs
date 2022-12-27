using System.Security.Cryptography;
using Konscious.Security.Cryptography;

namespace CinemaCentral.Services;

public class PasswordService : IPasswordService
{
    public byte[] CreateHash(byte[] password, byte[] salt)
    {
        using var argon2 = new Argon2id(password);
        argon2.DegreeOfParallelism = 16;
        argon2.MemorySize = 8192;
        argon2.Iterations = 40;
        argon2.Salt = salt;

        return argon2.GetBytes(128);
    }

    public byte[] CreateSalt() => RandomNumberGenerator.GetBytes(32);

    public bool VerifyHash(byte[] password, byte[] salt, byte[] hash) => CreateHash(password, salt).SequenceEqual(hash);
}