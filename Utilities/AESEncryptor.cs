using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Structura.SharedComponents.Utilities
{
    public interface IAesEncryptor
    {
        void LoadKeyFile();
        string Encrypt(string plainText);
        string Decrypt(string encryptedText);
        string IVAsString { get; set; }
    }

    public class AesEncryptor : IAesEncryptor
    {
        private readonly ISettingsRetriever _settingsRetriever;

        public AesEncryptor(ISettingsRetriever settingsRetriever)
        {
            _settingsRetriever = settingsRetriever;
        }

        private RijndaelManaged _rijndael = null;
        public byte[] Key { get; set; }
        public byte[] IV { get; set; }
        public string IVAsString
        {
            get
            {
                return IV == null ? null : Convert.ToBase64String(IV);
            }
            set { IV = Convert.FromBase64String(value); }
        }

        public AesEncryptor()
        {
            _rijndael = new RijndaelManaged {Mode = CipherMode.CBC};
        }

        public void LoadKeyFile()
        {
            LoadKeyFile(_settingsRetriever.Get<string>("EncryptionKeyFile"));
        }

        public void LoadKeyFile(string xmlFile)
        {
            var doc = new XmlDocument();
            doc.Load(xmlFile);
            var keyNode = doc.SelectSingleNode("/Ff/Key");
            Key = Convert.FromBase64String(keyNode.InnerText);
        }

        public string Encrypt(string plainText)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(plainText);
            if (IV == null)
            {
                _rijndael.GenerateIV();
                IV = _rijndael.IV;
            }
            return Convert.ToBase64String(Encrypt(Key, IV, bytes));

        }

        public string Decrypt(string encryptedText)
        {
            Check.RequireNotNull<ArgumentException,string>(IV, "IV cannot be null");
            return Encoding.Unicode.GetString(Decrypt(Key, IV, Convert.FromBase64String(encryptedText)));
        }

        public byte[] Encrypt(byte[] decrypted)
        {
            return Encrypt(Key, IV, decrypted);
        }

        public byte[] Decrypt(byte[] encrypted)
        {
            return Decrypt(Key, IV, encrypted);
        }


        /// <summary>
        /// Encrypts a value with Rijndael encrption
        /// </summary>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <param name="decrypted"></param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] key, byte[] iv, byte[] decrypted)
        {
            Check.Require(key != null, "Key cannot be null, did you execute LoadKeyFile()?");
            byte[] encrypted = null;

            if (_rijndael != null)
            {
                // encrypt the data
                var msEncrypt = new MemoryStream();
                CryptoStream csEncrypt = new CryptoStream(msEncrypt, _rijndael.CreateEncryptor(key, iv), CryptoStreamMode.Write);

                csEncrypt.Write(decrypted, 0, decrypted.Length);
                csEncrypt.FlushFinalBlock();
                encrypted = msEncrypt.ToArray();
                msEncrypt.Close();
            }
            return encrypted;
        }

        /// <summary>
        /// Decrypts a value with Rijndael encrption
        /// </summary>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <param name="encrypted"></param>
        /// <returns></returns>
        public byte[] Decrypt(byte[] key, byte[] iv, byte[] encrypted)
        {
            Check.Require(key != null, "Key cannot be null, did you execute LoadKeyFile()?");
            byte[] temp;
            byte[] decrypted = null;

            if (_rijndael != null)
            {
                var msDecrypt = new MemoryStream(encrypted);
                var csDecrypt = new CryptoStream(msDecrypt, _rijndael.CreateDecryptor(key, iv), CryptoStreamMode.Read);

                temp = new byte[encrypted.Length];
                var bytesRead = csDecrypt.Read(temp, 0, temp.Length);
                decrypted = new byte[bytesRead];
                Array.Copy(temp, decrypted, bytesRead);
                msDecrypt.Dispose();
            }
            return decrypted;
        }
    }
}
