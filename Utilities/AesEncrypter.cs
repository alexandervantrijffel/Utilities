using System;
using System.IO;
using System.Security.Cryptography;

namespace Structura.SharedComponents.Utilities
{
	[Serializable]
	public class CryptedDataWrapper
	{
		public byte[] CipherBytes { get; set; }

		public byte[] Salt { get; }

		public CryptedDataWrapper()
		{
			Salt = new byte[16];
			var rnd = new Random((int)DateTime.Now.Ticks);
			rnd.NextBytes(Salt);
		}

		public CryptedDataWrapper(byte[] salt)
		{
			Salt = salt;
		}
	}

	public class AesEncrypter
	{
		private readonly string _password;

		public AesEncrypter(string password)
		{
			_password = password;
		}

		public CryptedDataWrapper Encrypt<T>(T data) where T : class
		{
			var wrapper = new CryptedDataWrapper();
			using (var aes = InitAes(wrapper.Salt))
			{
				using (var ms = new MemoryStream())
				{
					using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
					{
						var unencrypted = ConvertByteArray.ObjectToByteArray(data);
						cs.Write(unencrypted, 0, unencrypted.Length);
					}
					wrapper.CipherBytes = ms.ToArray();
				}
			}
			return wrapper;
		}

		public T Decrypt<T>(CryptedDataWrapper cryptedDataWrapper) where T : class
		{
			using (var aes = InitAes(cryptedDataWrapper.Salt))
			{
				using (MemoryStream ms = new MemoryStream())
				{
					using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
					{
						cs.Write(cryptedDataWrapper.CipherBytes, 0, cryptedDataWrapper.CipherBytes.Length);
					}
					var plainBytes = ms.ToArray();
					return ConvertByteArray.ByteArrayToObject<T>(plainBytes);
				}
			}
		}

		private AesManaged InitAes(byte[] salt)
		{
			const int rfc2898KeygenIterations = 100;
			const int aesKeySizeInBits = 128;
			var aes = new AesManaged();
			aes.Padding = PaddingMode.PKCS7;
			aes.KeySize = aesKeySizeInBits;
			var keyStrengthInBytes = aes.KeySize / 8;
			var rfc2898 = new Rfc2898DeriveBytes(_password, salt, rfc2898KeygenIterations);
			aes.Key = rfc2898.GetBytes(keyStrengthInBytes);
			aes.IV = rfc2898.GetBytes(keyStrengthInBytes);
			return aes;
		}
	}
}