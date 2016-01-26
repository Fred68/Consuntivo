using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Collections.ObjectModel;       // Per Observable collection

namespace WPF02
	{
	/// <summary>
	/// Classe base astratta con funzioni di conversione e altro
	/// </summary>
	public abstract class Riga
		{
		// Classe public perchè oggetto passato come argomento ad una window, se no errore:
		// <Accessibilità incoerente: il tipo parametro è meno accessibile del metodo>
		public static class Separatore
			{
			public const char list = ',';
			public const char field = ';';
			public const char line = '\n';
			public const char datetime = '/';
			public const char header = '@';
			public static char[] seps = new char[] { ' ', ',', ';', '/' };
			public static char[] vietati = new char[] {
												Separatore.field,
												Separatore.line,
												Separatore.header};
			}
		public enum Tipo { P, A, N };

		public const string strTrue =  "v";
		public const string strFalse = "x";

		protected int _id;									// Numero identificativo unico

		// Metodi da implementare nelle classi derivate
		public abstract bool FromString(string str);			// Importa i dati da una stringa
		public abstract override string ToString();				// Esporta i dati ina una stringa
		public abstract string ListProperties();				// Stringa con le proprietà
		public abstract string Intestazione();                  // Intestazione della sezione
		public abstract IEnumerable<string> Valori();			// Enumeratore dei valori 
		public abstract IEnumerable<string> Titoli();           // Enumeratore delle proprietà
		public abstract IEnumerable<TipoColonna> Tipi();		// Enumeratore dei tipi
		public abstract int Count();                            // Numero delle proprietà
	
		// Metodi statici
		public static string DateTime2String(DateTime dt)
			{
			StringBuilder strb = new StringBuilder();
			strb.Append(dt.Year.ToString() + Separatore.datetime);
			strb.Append(dt.Month.ToString() + Separatore.datetime);
			strb.Append(dt.Day.ToString());
			return strb.ToString();
			}
		public static DateTime String2DateTime(string d)
			{
			DateTime x = new DateTime();
			if (DateTime.TryParse(d, out x))
				x = DateTime.Parse(d);
			return x;
			}
		public static void String2ListInt(ref List<int> il, string str)
			{
			string[] lst;
			lst = str.Split(Separatore.seps);
			il.Clear();
			foreach (string st in lst)
			    {
			    int i;
			    if (int.TryParse(st, out i))
			        {
			        i = int.Parse(st);
			        il.Add(i);
			        }
			    }
			}
		public static string ListInt2String(List<int> il)
			{
			StringBuilder strb = new StringBuilder();
			bool first = true;
			foreach (int i in il)
				{
				if (!first)
					{
					strb.Append(Separatore.list);
					}
				strb.Append(i.ToString());
				first = false;
				}
			return strb.ToString();
			}
		public static int String2IntOrZero(string str)
			{
			int i = 0;
			if (int.TryParse(str, out i))
				{
				i = int.Parse(str);
				}
			return i;
			}
		public static decimal String2DecimalOrZero(string str, out bool result)
			{
			decimal i = 0m;
			result = false;
			if (decimal.TryParse(str,NumberStyles.Currency,CultureInfo.CurrentCulture, out i))
				{
				i = decimal.Parse(str, NumberStyles.Currency, CultureInfo.CurrentCulture);
				result = true;
				}
			return i;
			}
		public static decimal String2DecimalOrZero(string str)
			{
			bool nouse;
			return String2DecimalOrZero(str, out nouse);
			}
		public static string CleanString(string txt)
			{
			StringBuilder strb = new StringBuilder();
			if(txt!=null)
				foreach(char ch in txt)
					{
					if (!Separatore.vietati.Contains<char>(ch))
						strb.Append(ch);
					}
			return strb.ToString();
			}
		public static bool CheckUnique(IEnumerable<Riga> cl)	// Verifica unicità dei numeri identificativi
			{
			bool ok = true;
			foreach(Riga r in cl)
				{
				int cnt = 0;
				foreach(Riga t in cl)
					{
					if (t._id == r._id)
						cnt++;
					}
				if(cnt>1)
					{
					ok = false;
					break;
					}
				}
			return ok;
			}
		}
	}
