using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Security;
using System.Security.Cryptography;
using System.Collections.Generic;

using UnityEngine;
namespace Tempest
{
	namespace Utils
	{
		public class Encryptor
		{
			public static string Encrypt(string text, out string salt)
			{
				byte[] saltBytes = null;
				string encryptedText = Encrypt (text, out saltBytes);
				salt = Convert.ToBase64String (saltBytes);

				return encryptedText;
			}
			
			public static string Encrypt(string text, out byte[] salt)
			{
				using(RijndaelManaged rij = new RijndaelManaged())
				{
					rij.GenerateKey();
					rij.GenerateIV();
					
					salt = rij.Key;
					
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
		
					byte[] encryptedBytes = new byte[rij.IV.Length + cipherTextBytes.Length];

					Buffer.BlockCopy(rij.IV, 0, encryptedBytes, 0, rij.IV.Length);
					Buffer.BlockCopy(cipherTextBytes, 0, encryptedBytes, rij.IV.Length, cipherTextBytes.Length);

					return Convert.ToBase64String (encryptedBytes);
				}
			}

			public static string Decrypt(string encryptedText, string salt)
			{
				return Decrypt (encryptedText, Convert.FromBase64String(salt));
			}

			public static string Decrypt(string encryptedText, byte[] salt)
			{		
				if(encryptedText.Length > 16)
				{
					byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
					byte[] iv = new byte[16];
					byte[] cipherTextBytes = new byte[encryptedBytes.Length - iv.Length];

					Buffer.BlockCopy (encryptedBytes, 0, iv, 0, iv.Length);
					Buffer.BlockCopy (encryptedBytes, iv.Length, cipherTextBytes, 0, cipherTextBytes.Length); 

					ICryptoTransform decryptor = new RijndaelManaged().CreateDecryptor(salt, iv);
			
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
				return null;
			}
		}
	}
}
