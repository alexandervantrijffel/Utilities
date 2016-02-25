using System;
using System.Runtime.Serialization;
using FluentAssertions;
using Xunit;

namespace Structura.SharedComponents.Utilities.Tests
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
			unencrypted.Date.Ticks.Should().Be(data.Date.Ticks);
			unencrypted.SecretText.Should().Be(data.SecretText);
		}

		[Fact]
		public void Encrypted_data_should_not_match_original()
		{
			var data = new EncryptData { Date = DateTime.Now.Subtract(TimeSpan.FromDays(5)), SecretText = "I am a fan of Chopin." };
			const string password = "VerySecret!";
			var crypted = new AesEncrypter(password).Encrypt(data);
			Action a = () => ConvertByteArray.ByteArrayToObject<EncryptData>(crypted.CipherBytes);
			var message = a.ShouldThrow<SerializationException>().And.Message;
			(message.Contains("The input stream is not a valid binary format") || message.Contains("does not contain a valid BinaryHeader"))
				.Should().BeTrue();
		}
	}
}