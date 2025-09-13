using System.Security.Cryptography;
using System.Text;

namespace yourself_demoAPI.Data.Helpers
{
	public static class OtpHelper
	{
		public static string GenerateCode()
		{
			using var rng = RandomNumberGenerator.Create();
			var bytes = new byte[4];
			rng.GetBytes(bytes);
			var num = Math.Abs(BitConverter.ToInt32(bytes, 0)) % 10000;

			return num.ToString("D4");
		}

		public static string HashCode(string code , string secret) 
		{ 
			var key = Encoding.UTF8.GetBytes(secret);
			using var hmac = new HMACSHA256(key);
			var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(code));	
			return Convert.ToBase64String(hash);
		}

		public static bool VerifyCode (string input, string stored, string secret)
		{
			var expected = HashCode(input, secret);
			return CryptographicOperations.FixedTimeEquals (
				Convert.FromBase64String(expected),
				Convert.FromBase64String(stored));
		}
	}
}
