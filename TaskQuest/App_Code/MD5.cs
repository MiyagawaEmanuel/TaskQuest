using System.Security.Cryptography;
using System.Text;

namespace TaskQuest.App_Code
{
	public static class Hash
	{
		public static string @String(string s)
		{
			StringBuilder sb = new StringBuilder();
	        MD5 md5 = MD5.Create();

	        byte[] entrada = Encoding.ASCII.GetBytes(s);
	        byte[] hash = md5.ComputeHash(entrada);

	        for (int i = 0; i < hash.Length; i++)
	            sb.Append(hash[i].ToString("X2"));
	       
	        return sb.ToString();
		}
	}
} 
