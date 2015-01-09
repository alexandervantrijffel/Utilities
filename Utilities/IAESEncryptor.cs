namespace SharedUtilities
{
    public interface IAesEncryptor
    {
        void LoadKeyFile();
        string Encrypt(string plainText);
        string Decrypt(string encryptedText);
        string IVAsString { get; set; }
    }
}
