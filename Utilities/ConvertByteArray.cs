using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Structura.SharedComponents.Utilities
{
	public static class ConvertByteArray
	{
		public static byte[] ObjectToByteArray(object source)
		{
			var formatter = new BinaryFormatter();
			using (var stream = new MemoryStream())
			{
				formatter.Serialize(stream, source);
				return stream.ToArray();
			}
		}
		public static T ByteArrayToObject<T>(byte[] arrBytes) where T : class
		{
			using (var memStream = new MemoryStream())
			{
				var binForm = new BinaryFormatter();
				memStream.Write(arrBytes, 0, arrBytes.Length);
				memStream.Seek(0, SeekOrigin.Begin);
				return binForm.Deserialize(memStream) as T;
			}
		}
	}
}