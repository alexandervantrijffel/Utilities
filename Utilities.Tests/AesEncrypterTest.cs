using System;
using System.Runtime.Serialization;
using Shouldly;
using Xunit;

namespace Structura.Shared.Utilities.Tests
{
	public class AesEncrypterTest
	{
		/// <summary>
		/// Encrypts An EncryptData object with a DateTime and decr
		/// And unen
		/// </summary>
		[Fact]
		public void Decrypted_datetime_should_match_original_datetime()
		{
			var data = new EncryptData { Date = DateTime.Now.Subtract(TimeSpan.FromDays(5)), SecretText = "I am a fan of Chopin." };
			const string password = "VerySecret!";
			var crypted = new AesEncrypter(password).Encrypt(data);
			var unencrypted = new AesEncrypter(password).Decrypt<EncryptData>(crypted);
			unencrypted.Date.Ticks.ShouldBe(data.Date.Ticks);
			unencrypted.SecretText.ShouldBe(data.SecretText);
		}

		[Fact]
		public void Encrypted_data_should_not_match_original()
		{
			var data = new EncryptData { Date = DateTime.Now.Subtract(TimeSpan.FromDays(5)), SecretText = "I am a fan of Chopin." };
			const string password = "VerySecret!";
			var crypted = new AesEncrypter(password).Encrypt(data);
		    var msg =
		        Should.Throw<SerializationException>(
		            () => ConvertByteArray.ByteArrayToObject<EncryptData>(crypted.CipherBytes)).Message;
            (msg.Contains("The input stream is not a valid binary format") || msg.Contains("does not contain a valid BinaryHeader")).ShouldBeTrue();
		}
	}
}