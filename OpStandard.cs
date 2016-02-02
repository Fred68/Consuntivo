using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;            // Per property info

namespace WPF02
	{
	public class OpStandard : Riga
		{
		public const int CAMPI = 3;

		#region PROPERTIES
		public int numero
			{
			get {return _id; }
			set { _id = value; }
			}
		public override string descrizione
			{
			get { return _des; }
			set { _des = value; }
			}
		public string conti
			{
			get { return getConti(); }
			set { setConti(value); }
			}
		#endregion
		public static Dictionary<string, TipoColonna> larghezze;

		List<int> cnt;

		static OpStandard()
			{
			larghezze = new Dictionary<string, TipoColonna>();
			foreach (PropertyInfo prop in typeof(OpStandard).GetProperties())
				{
				larghezze.Add(prop.Name, new TipoColonna(prop.PropertyType.ToString(), 0));
				}
			larghezze["numero"].larghezzaColonna = 50;
			larghezze["descrizione"].larghezzaColonna = 150;
			larghezze["conti"].larghezzaColonna = 100;
			}
		public OpStandard()
			{
			cnt = new List<int>();
			numero = 0;
			descrizione = "-";
			}
		public OpStandard(int numero, string descrizione, string conti)
			{
			cnt = new List<int>();
			this.numero = numero;
			this.descrizione = descrizione;
			this.conti = conti;
			}
		public override string ToString()
			{
			StringBuilder strb = new StringBuilder();
			strb.Append(numero.ToString() + Separatore.field);
			strb.Append(OpStandard.CleanString(descrizione) + Separatore.field);
			strb.Append(conti);
			return strb.ToString();
			}
		public override bool FromString(string str)
			{
			bool ok = false;
			string[] cmp = str.ToString().Split(new char[] { Separatore.field });
			if (cmp.Length == CAMPI)
				{
				OpStandard tmp = new OpStandard();
				tmp.numero = OpStandard.String2IntOrZero(cmp[0]);
				tmp.descrizione = cmp[1];
				tmp.conti = cmp[2];
				ok = true;
				if (ok)
					{
					this.numero = tmp.numero;
					this.descrizione = tmp.descrizione;
					this.conti = tmp.conti;
					}
				}
			return ok;

			}
		public override string ListProperties()           // Dialogo con lista delle proprietà
			{
			StringBuilder strb = new StringBuilder();
			foreach (PropertyInfo prop in typeof(OpStandard).GetProperties())
				{
				strb.Append(prop.Name + '\t' + prop.PropertyType.ToString() + System.Environment.NewLine);
				}
			return strb.ToString();
			}
		public override int Count()
			{
			int i = 0;
			foreach (PropertyInfo prop in typeof(OpStandard).GetProperties())
				i++;
			return i;
			}
		public override IEnumerable<string> Valori()      // I valori (in stringhe)
			{
			foreach (PropertyInfo prop in typeof(OpStandard).GetProperties())
				{
				yield return prop.GetValue(this) as string;
				}
			}
		string getConti()                           // Ottiene la string con la lista dei conti
			{
			return ListInt2String(cnt);
			}
		void setConti(string str)                   // Imposta la la lista dei conti leggendo la stringa
			{
			String2ListInt(ref cnt, str);
			}
		public override string Intestazione()
			{
			return Operazione.Separatore.header.ToString() + "OPSTANDARD";
			}
		public override IEnumerable<string> Titoli()
			{
			foreach (PropertyInfo prop in typeof(OpStandard).GetProperties())
				{
				yield return prop.Name as string;
				}
			yield break;
			}
		public override IEnumerable<TipoColonna> Tipi()
			{
			foreach (PropertyInfo prop in typeof(OpStandard).GetProperties())
				{
				TipoColonna tipo = new TipoColonna(prop.PropertyType.ToString(), larghezze[prop.Name].larghezzaColonna);
				yield return tipo as TipoColonna;
				}
			yield break;
			}
		public IEnumerable<int> Conti()             // Restituisce i numeri dei conti
			{
			foreach (int c in cnt)
				yield return c;
			yield break;
			}
		}
	}
