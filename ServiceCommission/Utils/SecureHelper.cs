using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCommission.Utils
{
	public static class SecureHelper
	{
		private const string MascaraSalt = @"{0}{1}";

		/// <summary>
		/// Gera o hash SHA256 de uma string
		/// </summary>
		public static string GerarHash(string salt, string value)
		{
			var builder = new StringBuilder();

			using (var sha256 = SHA256.Create())
			{
				var encoding = Encoding.UTF8;
				var input = String.Format(MascaraSalt, value, salt.ToLower());
				var computed = sha256.ComputeHash(encoding.GetBytes(input));

				foreach (var item in computed)
					builder.Append(item.ToString("X2"));
			}

			return builder.ToString();
		}
	}
}
