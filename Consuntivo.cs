using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;            // Per property info


namespace WPF02
	{
	public class Consuntivo : Riga
		{
		// public enum Tipo { P, A, N };
		DateTime dt;
		public const int CAMPI = 7;

		#region PROPERTIES
		public string data
			{
			get { return getData(); }
			set { setData(value); }
			}
		public override string descrizione
			{
			get { return _des; }
			set { _des = value; }
			}
		public decimal importo { get; set; }
		public bool consuntivo { get; set; }
		public bool verificato { get; set; }
		public Tipo tipo { get; set; }
		public decimal totale { get; set; }
		#endregion
		public static Dictionary<string, TipoColonna> larghezze;

		static Consuntivo()
			{
			larghezze = new Dictionary<string, TipoColonna>();
			foreach (PropertyInfo prop in typeof(Consuntivo).GetProperties())
				{
				larghezze.Add(prop.Name, new TipoColonna(prop.PropertyType.ToString(), 0));
				}
			larghezze["data"].larghezzaColonna = 70;
			larghezze["descrizione"].larghezzaColonna = 130;
			larghezze["importo"].larghezzaColonna = 100;
			larghezze["consuntivo"].larghezzaColonna = 20;
			larghezze["verificato"].larghezzaColonna = 20;
			larghezze["tipo"].larghezzaColonna = 20;
			larghezze["totale"].larghezzaColonna = 100;
			}
		public Consuntivo()
			{
			dt = System.DateTime.Now;
			descrizione = "-";
			importo = 0;
			totale = 0;
			tipo = Tipo.P;
			}
		public Consuntivo(DateTime data, string descrizione, decimal importo, Tipo tipo, decimal totale, bool consuntivo, bool verificato)
			{
			this.dt = data;
			this.descrizione = descrizione;
			this.importo = importo;
			this.tipo = tipo;
			this.totale = totale;
			this.consuntivo = consuntivo;
			this.verificato = verificato;
			}
		public override string Intestazione()
			{
			return Consuntivo.Separatore.header.ToString() + "CONSUNTIVI";
			}
		public override string ToString()
			{
			StringBuilder strb = new StringBuilder();
			strb.Append(data.ToString() + Separatore.field);
			strb.Append(Consuntivo.CleanString(descrizione) + Separatore.field);
			strb.Append(importo.ToString(/*"C"*/) + Separatore.field);
			strb.Append((consuntivo ? Consuntivo.strTrue : Consuntivo.strFalse) + Separatore.field);
			strb.Append((verificato ? Consuntivo.strTrue : Consuntivo.strFalse) + Separatore.field);
			strb.Append(tipo.ToString() + Separatore.field);
			strb.Append(totale.ToString(/*"C"*/));
			return strb.ToString();
			}
		public override bool FromString(string str)
			{
			bool ok = false;
			bool[] conv = new bool[CAMPI];
			for (int i = 0; i < conv.Length; i++) conv[i] = true;
			string[] cmp = str.ToString().Split(new char[] { Separatore.field });
			if (cmp.Length == CAMPI)
				{
				Consuntivo tmp = new Consuntivo();
				tmp.data = cmp[0];
				tmp.descrizione = cmp[1];
				tmp.importo = Consuntivo.String2DecimalOrZero(cmp[2], out conv[2]);
				tmp.consuntivo = ((cmp[3] == Consuntivo.strTrue.ToString()) ? true : false);
				tmp.verificato = ((cmp[4] == Consuntivo.strTrue.ToString()) ? true : false);
				foreach (Consuntivo.Tipo tp in Enum.GetValues(typeof(Consuntivo.Tipo)))
					{
					if (cmp[5] == tp.ToString())
						{
						tmp.tipo = tp;
						conv[5] = true;
						break;
						}
					conv[5] = false;
					}
				tmp.totale = Consuntivo.String2DecimalOrZero(cmp[6], out conv[6]);
				ok = true;
				if (Array.Exists(conv, el => el == false))
					{
					ok = false;
					}
				if (ok)
					{
					this.data = tmp.data;
					this.descrizione = tmp.descrizione;
					this.importo = tmp.importo;
					this.consuntivo = tmp.consuntivo;
					this.verificato = tmp.verificato;
					this.tipo = tmp.tipo;
					this.totale = tmp.totale;
					}
				}
			return ok;
			}
		public override string ListProperties()     // Dialogo con lista delle proprietà
			{
			StringBuilder strb = new StringBuilder();
			foreach (PropertyInfo prop in typeof(Consuntivo).GetProperties())
				{
				strb.Append(prop.Name + '\t' + prop.PropertyType.ToString() + System.Environment.NewLine);
				}
			return strb.ToString();
			}
		public override int Count()
			{
			int i = 0;
			foreach (PropertyInfo prop in typeof(Consuntivo).GetProperties())
				i++;
			return i;
			}
		public override IEnumerable<string> Valori()           // I valori (in stringa)
			{
			foreach (PropertyInfo prop in typeof(Consuntivo).GetProperties())
				{
				object obj = prop.GetValue(this);
				string ret;
				switch (prop.PropertyType.ToString())
					{
					case "System.Decimal":
							{
							ret = ((Decimal)obj).ToString("C");
							}
						break;
					case "System.Boolean":
							{
							ret = ((bool)obj) ? "v" : "";
							}
						break;
					default:
							{
							ret = obj.ToString();
							}
						break;
					}
				yield return ret as string;
				}
			yield break;
			}
		public override IEnumerable<string> Titoli()
			{
			foreach (PropertyInfo prop in typeof(Consuntivo).GetProperties())
				{
				yield return prop.Name as string;
				}
			yield break;
			}
		public override IEnumerable<TipoColonna> Tipi()
			{
			foreach (PropertyInfo prop in typeof(Consuntivo).GetProperties())
				{
				TipoColonna tipo = new TipoColonna(prop.PropertyType.ToString(), larghezze[prop.Name].larghezzaColonna);
				yield return tipo as TipoColonna;
				}
			yield break;
			}


		string getData()                            // Restituisce la data
			{   // Data trasformata in stringa e ordinata come tale, perciò scelto formato YYYY/MM/DD 
			return Consuntivo.DateTime2String(dt);
			}
		public DateTime getData(bool unused)
			{
			return dt;
			}
		void setData(string d)                      // Imposta la data
			{
			dt = String2DateTime(d);
			}
		public void setData(DateTime dt)
			{
			this.dt = dt;
			}

		}
	}
