using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AnimeNotepad.Services;

public static class UwuFileService
{
    private static readonly byte[] Magic = Encoding.ASCII.GetBytes("ANIMEUWU");
    private const byte FormatVersion = 1;
    private const int SaltSize = 16;
    private const int NonceSize = 12;
    private const int TagSize = 16;
    private const int Iterations = 600_000;

    public static byte[] Encrypt(string content, string password)
    {
        ValidatePassword(password);
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] nonce = RandomNumberGenerator.GetBytes(NonceSize);
        byte[] plaintext = Encoding.UTF8.GetBytes(content);
        byte[] ciphertext = new byte[plaintext.Length];
        byte[] tag = new byte[TagSize];

        byte[] key = DeriveKey(password, salt);
        using var aes = new AesGcm(key, TagSize);
        aes.Encrypt(nonce, plaintext, ciphertext, tag, Magic);

        using var output = new MemoryStream();
        output.Write(Magic);
        output.WriteByte(FormatVersion);
        output.Write(salt);
        output.Write(nonce);
        output.Write(tag);
        output.Write(ciphertext);
        return output.ToArray();
    }

    public static string Decrypt(Stream input, string password)
    {
        ValidatePassword(password);
        using var data = new MemoryStream();
        input.CopyTo(data);
        byte[] payload = data.ToArray();
        int headerSize = Magic.Length + 1 + SaltSize + NonceSize + TagSize;
        if (payload.Length < headerSize || !payload.AsSpan(0, Magic.Length).SequenceEqual(Magic) || payload[Magic.Length] != FormatVersion)
        {
            throw new InvalidDataException("El archivo no tiene un formato .uwu válido.");
        }

        int offset = Magic.Length + 1;
        byte[] salt = payload[offset..(offset + SaltSize)];
        offset += SaltSize;
        byte[] nonce = payload[offset..(offset + NonceSize)];
        offset += NonceSize;
        byte[] tag = payload[offset..(offset + TagSize)];
        offset += TagSize;
        byte[] ciphertext = payload[offset..];
        byte[] plaintext = new byte[ciphertext.Length];

        try
        {
            byte[] key = DeriveKey(password, salt);
            using var aes = new AesGcm(key, TagSize);
            aes.Decrypt(nonce, ciphertext, tag, plaintext, Magic);
            return Encoding.UTF8.GetString(plaintext);
        }
        catch (CryptographicException)
        {
            throw new UnauthorizedAccessException("La contraseña no es correcta o el archivo fue alterado.");
        }
    }

    private static byte[] DeriveKey(string password, byte[] salt)
        => Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, 32);

    private static void ValidatePassword(string password)
    {
        if (string.IsNullOrEmpty(password)) throw new ArgumentException("La contraseña no puede estar vacía.", nameof(password));
    }
}