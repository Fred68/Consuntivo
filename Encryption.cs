using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security.Cryptography;						// Per AES (chiave simmetrica)
using System.IO;										// Per memory stream

namespace WPF02
	{
	/// <summary>
	/// Classe per critptare e decriptare delle stringhe con Rijndael (AES con chieve e blocchi a 32 caratteri = 256 bit)
	/// L'IV in base64 è anteposto al messaggio criptato, con Padding PKCS7 e concatenamento CBC, e convertito in base64
	/// La chiave viene generata dalla password (usando salt e iterazioni fisse)
	/// </summary>
	class Encryption
		{
		int keySize;
		int blockSize;
		PaddingMode paddingMode;
		CipherMode cipherMode;
		RijndaelManaged aes;
		int IVbase64size;
		List<string> errors;

		private byte[] salt; // = new byte[] { 10, 20, 30, 40, 50, 60, 70, 80 };
		private const int SALTSIZE = 8;
		private const byte SALTZERO = 0x20;
		/// <summary>
		/// Costruttore della classe per crittografare
		/// </summary>
		public Encryption(string strSalt = "")
			{
			keySize = 32;									// 32 caratteri (256 bit)
			blockSize = 32;									// 32 caratteri (256 bit)
			paddingMode = PaddingMode.PKCS7;				// Completa con numero progressivo
			cipherMode = CipherMode.CBC;					// Combina blocco precedente crittografato al successivo (contro blocchi identici)
			IVbase64size = IVbase64length();                // Imposta la lunghezza dell'IV in base64
			errors = new List<string>();                    // Lista dei messaggi di errore o altro
			SetSalt(strSalt);
			}
		/// <summary>
		/// Imposta il salt partendo dai caratteri Unicode UTF-8 di una stringa
		/// Legge solo i primi 8 caratteri e li converte
		/// in byte, se possibile, se no in 0x20
		/// </summary>
		/// <param name="strSalt"></param>
		private void SetSalt(string strSalt)
			{
			salt = new byte[SALTSIZE];
			byte b;
			for (int i = 0; i < SALTSIZE; i++)
				{
				b = SALTZERO;
				if (i < strSalt.Length)
					{
					char ch = strSalt[i];
					try
						{
						b = Convert.ToByte(ch);
						}
					catch (OverflowException)
						{ b = SALTZERO; }
					}
				salt[i] = b;
				}
			}
		/// <summary>
		/// Restituisce il salt convertito in caratteri Unicode
		/// </summary>
		/// <returns>Stringa con il salt</returns>
		private string GetSalt()
			{
			StringBuilder strb = new StringBuilder();
			foreach(byte b in salt)
				{
				strb.Append(Convert.ToChar(b));
				}
			return strb.ToString();
			}
		public string Salt
			{
			get { return GetSalt(); }
			set { SetSalt(value);  }
			}
		/// <summary>
		/// Cripta la stringa, inserendo l'IV in base64 all'inizio
		/// seguito dal messaggio crittografato in base64
		/// </summary>
		/// <param name="txt">Stringa da crittografare</param>
		/// <param name="password">Stringa con password di lunghezza arbitraria</param>
		/// <returns>Stringa criptata e convertita in base64</returns>
		public string Encrypt(string txt, string password)
			{
			string enc = "";
			CreateAes();
			aes.GenerateIV();
			aes.Key = CreateKey(password);
			string iv = Convert.ToBase64String(aes.IV);
			ICryptoTransform encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
			byte[] xBuff = null;
			try
				{
				using (MemoryStream ms = new MemoryStream())
					{
					using (CryptoStream cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
						{
						byte[] xXml = Encoding.UTF8.GetBytes(txt);
						cs.Write(xXml, 0, xXml.Length);
						}
					xBuff = ms.ToArray();
					}
				enc = Convert.ToBase64String(xBuff);
				}
			catch (Exception ex)
				{
				errors.Add("ENC:" + ex.Message);
				enc = "";
				}
			return iv + enc;
			}
		/// <summary>
		/// Decripta la stringa in base64, usando la password
		/// </summary>
		/// <param name="txt">Messaggio criptato in base64</param>
		/// <param name="password">Password usata per criptare il messaggio</param>
		/// <returns>La stringa del messaggio originario</returns>
		public string Decrypt(string txt, string password)
			{
			string dec = "";
			string msg;
			string iv;
			CreateAes();
			aes.Key = CreateKey(password);
			iv = GetIVheader(txt, out msg);

			aes.IV = Convert.FromBase64String(iv);
			ICryptoTransform decrypt = aes.CreateDecryptor();
			byte[] xBuff = null;
			try
				{
				using (MemoryStream ms = new MemoryStream())
					{
					using (CryptoStream cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
						{
						byte[] xXml = Convert.FromBase64String(msg);
						cs.Write(xXml, 0, xXml.Length);
						}
					xBuff = ms.ToArray();
					}
				dec = Encoding.UTF8.GetString(xBuff);
				}
			catch(Exception ex)
				{
				errors.Add("DEC:"+ex.Message);
				dec = "";
				}
			return dec;
			}
		/// <summary>
		/// Cancella la lista dei messaggi di errore
		/// </summary>
		public void ClearErrors()
			{
			errors.Clear();
			}
		/// <summary>
		/// Restituisce una stringa contenente i messaggi di errore della lista
		/// </summary>
		/// <returns></returns>
		public string Errors()
			{
			StringBuilder strb = new StringBuilder();
			foreach (string str in errors)
				strb.Append(str + '\n');
			return strb.ToString();
			}
		private byte[] CreateKey(string passwd, int keybytes = 32)		// Crea la chiave, da stringa arbitaria. Salt fisso
			{
			const int iter = 301;
			Rfc2898DeriveBytes keyGen = new Rfc2898DeriveBytes(passwd, salt, iter);
			return keyGen.GetBytes(keybytes);
			}
		private string GetIVheader(string txt, out string msg)		// Taglia iv dallínizio di msg e lo restituisce.
			{
			string iv;
			try
				{
				iv = txt.Substring(0, IVbase64size);
				msg = txt.Substring(IVbase64size);
				}
			catch
				{
				iv = "";
				msg = "";
				}
			return iv;
			}
		private int IVbase64length()								// Genera IV casuale, lo converte in base64 e ne restituisce la lunghezza 
			{
			CreateAes();
			aes.GenerateIV();
			string iv = Convert.ToBase64String(aes.IV);
			return iv.Length;
			}
		private void CreateAes()									// Crea e inizializza il criptatore
			{
			aes = new RijndaelManaged();
			aes.KeySize = keySize * 8;
			aes.BlockSize = blockSize * 8;
			aes.Padding = paddingMode;
			aes.Mode = cipherMode;
			}
		}
	}
