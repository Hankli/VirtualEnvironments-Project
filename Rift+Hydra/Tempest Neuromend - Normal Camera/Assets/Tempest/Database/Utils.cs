using System;
using System.IO;
using System.Text;
using System.Security;
using System.Security.Cryptography;


using UnityEngine;
namespace Tempest
{
	namespace Utils
	{
		public class Encryptor
		{
			public static string Encrypt(string text, out string key, out string iv)
			{
				byte[] keyBytes = null;
				byte[] ivBytes = null;
				
				string encryptedText = Encrypt (text, out keyBytes, out ivBytes);
				
				key = Convert.ToBase64String (keyBytes);
				iv = Convert.ToBase64String (ivBytes);
				
				return encryptedText;
			}
			
			public static string Encrypt(string text, out byte[] key, out byte[] iv)
			{
				using(RijndaelManaged rij = new RijndaelManaged())
				{
					rij.GenerateKey();
					rij.GenerateIV();
					
					key = rij.Key;
					iv = rij.IV;
					
					byte[] plainTextBytes = Encoding.UTF8.GetBytes(text);
					ICryptoTransform encryptor = rij.CreateEncryptor(rij.Key, rij.IV);
					
					byte[] cipherTextBytes;
					
					using (MemoryStream memoryStream = new MemoryStream())
					{
						using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
						{
							cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
							cryptoStream.FlushFinalBlock();
							cipherTextBytes = memoryStream.ToArray();
							cryptoStream.Close();
						}
						memoryStream.Close();
					}
					return Convert.ToBase64String(cipherTextBytes);
				}
			}

			public static string Decrypt(string text, string key, string iv)
			{
				byte[] keyBytes = Convert.FromBase64String(key);
				byte[] ivBytes = Convert.FromBase64String (iv);
				
				return Decrypt (text, keyBytes, ivBytes);
			}

			public static string Decrypt(string encryptedText, byte[] key, byte[] iv)
			{		
				byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
				ICryptoTransform decryptor = new RijndaelManaged().CreateDecryptor(key, iv);
		
				using(MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
				{
					using(CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
					{
						byte[] plainTextBytes = new byte[cipherTextBytes.Length];
							
						int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
						memoryStream.Close();
						cryptoStream.Close();
							
						return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
					}
				}
			}
		}
	}
}
