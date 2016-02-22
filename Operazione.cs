using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;            // Per property info

namespace WPF02
    {
#warning VEDERE SE IMPOSTARE IL FLAG modificato=true AD OGNI SET
	public class Operazione : Riga, IComparable
        {
		List<int> cnt;
		DateTime dt;
		public const int CAMPI = 9;

		#region PROPERTIES
		public string nota { get; set; }
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
        public int tipostd { get; set; }
        public string conti
            {
            get { return getConti(); }
            set { setConti(value); }
            }
		#endregion
		public static Dictionary<string, TipoColonna> larghezze;

		static Operazione()
			{
			larghezze = new Dictionary<string, TipoColonna>();
			foreach (PropertyInfo prop in typeof(Operazione).GetProperties())
				{
				larghezze.Add(prop.Name, new TipoColonna(prop.PropertyType.ToString(), 0));
				}
			larghezze["nota"].larghezzaColonna = 50;
			larghezze["data"].larghezzaColonna = 70;
			larghezze["descrizione"].larghezzaColonna = 130;
			larghezze["importo"].larghezzaColonna = 100;
			larghezze["consuntivo"].larghezzaColonna = 20;
			larghezze["verificato"].larghezzaColonna = 20;
			larghezze["tipo"].larghezzaColonna = 20;
			larghezze["tipostd"].larghezzaColonna = 20;
			larghezze["conti"].larghezzaColonna = 50;
			}
		public Operazione()
            {
			cnt = new List<int>();
			nota = "";
            dt = System.DateTime.Now;

			descrizione = "-";
            importo = 0;
            tipo = Tipo.P;
            tipostd = 0;
            }
        public Operazione(string nota, DateTime data, string descrizione, int importo, Tipo tipo, uint tipost, string conti)
            {
			cnt = new List<int>();
			this.nota = nota;
            this.dt = data;
            this.descrizione = descrizione;
            this.importo = importo;
            this.tipo = tipo;
            this.tipostd = tipostd;
			this.conti = conti;
            }
		public override string Intestazione()
			{
			return Operazione.Separatore.header.ToString()+"OPERAZIONI";
			}
		public override string ToString()
			{
			StringBuilder strb = new StringBuilder();
			strb.Append(Operazione.CleanString(nota) + Separatore.field);
			strb.Append(data.ToString() + Separatore.field);
			strb.Append(Operazione.CleanString(descrizione) + Separatore.field);
			strb.Append(importo.ToString(/*"C"*/) + Separatore.field);
			strb.Append((consuntivo ? Operazione.strTrue : Operazione.strFalse) + Separatore.field);
			strb.Append((verificato ? Operazione.strTrue : Operazione.strFalse) + Separatore.field);
			strb.Append(tipo.ToString() + Separatore.field);
			strb.Append(tipostd.ToString() + Separatore.field);
			strb.Append(conti);
			return strb.ToString();
			}
		public override bool FromString(string str)
			{
			bool ok = false;
			bool[] conv = new bool[CAMPI];
			for (int i= 0; i < conv.Length; i++) conv[i] = true;
			string[] cmp = str.ToString().Split(new char[] { Separatore.field });
			if (cmp.Length == CAMPI)
				{
				Operazione tmp = new Operazione();
				tmp.nota = cmp[0];
				tmp.data = cmp[1];
				tmp.descrizione = cmp[2];
				tmp.importo = Operazione.String2DecimalOrZero(cmp[3], out conv[3]);
				tmp.consuntivo = ((cmp[4] == Operazione.strTrue.ToString()) ? true : false);
				tmp.verificato = ((cmp[5] == Operazione.strTrue.ToString()) ? true : false);
				foreach (Operazione.Tipo tp in Enum.GetValues(typeof(Operazione.Tipo)))
					{
					if(cmp[6] == tp.ToString())
						{
						tmp.tipo = tp;
						conv[6] = true;
						break;
						}
					conv[6] = false;
					}
				tmp.tipostd = Operazione.String2IntOrZero(cmp[7]);
				tmp.conti = cmp[8];
				ok = true;
				if (Array.Exists(conv, el => el == false))
					{
					ok = false;
					}
				if (ok)
					{
					this.nota = tmp.nota;
					this.data = tmp.data;
					this.descrizione= tmp.descrizione;
					this.importo= tmp.importo ;
					this.consuntivo= tmp.consuntivo ;
					this.verificato= tmp.verificato ;
					this.tipo= tmp.tipo;
					this.tipostd= tmp.tipostd;
					this.conti= tmp.conti;
					}
				}
			return ok;
			}
		public override string ListProperties()		// Dialogo con lista delle proprietà
            {
            StringBuilder strb = new StringBuilder();
            foreach(PropertyInfo prop in typeof(Operazione).GetProperties())
                {
                strb.Append(prop.Name + '\t' + prop.PropertyType.ToString() + System.Environment.NewLine);
                }
            return strb.ToString();
            }
		public override int Count()
			{
			int i = 0;
			foreach (PropertyInfo prop in typeof(Operazione).GetProperties())
				i++;
			return i;
			}
		public override IEnumerable<string> Valori()           // I valori (in stringa)
			{
			foreach (PropertyInfo prop in typeof(Operazione).GetProperties())
				{
				object obj = prop.GetValue(this);
				string ret;
				switch(prop.PropertyType.ToString())
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
			foreach (PropertyInfo prop in typeof(Operazione).GetProperties())
				{
				yield return prop.Name as string;
				}
			yield break;
			}
		public override IEnumerable<TipoColonna> Tipi()
			{
			foreach (PropertyInfo prop in typeof(Operazione).GetProperties())
				{
				TipoColonna tipo = new TipoColonna(prop.PropertyType.ToString(), larghezze[prop.Name].larghezzaColonna);
				yield return tipo as TipoColonna;
				}
			yield break;
			}
		public IEnumerable<int> Conti()				// Restituisce i numeri dei conti
			{
			foreach (int c in cnt)
				yield return c;
			yield break;
			}
		public int CompareTo(object obj)			// Per interfaccia IComparable
			{
			Operazione op = obj as Operazione;
			if (op == null)
				throw new ArgumentException("obj non è una Operazione");
			return this.dt.CompareTo(op.dt);
			}
		string getConti()							// Ottiene la string con la lista dei conti
            {
			return ListInt2String(cnt);
            }
        void setConti(string str)					// Imposta la la lista dei conti leggendo la stringa
            {
			String2ListInt(ref cnt, str);
            }	
		string getData()                            // Restituisce la data
			{	// Data trasformata in stringa e ordinata come tale, perciò scelto formato YYYY/MM/DD 
			return Operazione.DateTime2String(dt);
			}
		public DateTime getData(bool unused)
			{
			return dt;
			}
		void setData(string d)						// Imposta la data
			{
			dt = String2DateTime(d);
			}
		public void setData(DateTime dt)
			{
			this.dt = dt;
			}
		public bool ContieneConto(int nc)
			{
			bool found = false;
			foreach(int c in cnt)
				{
				if(c == nc)
					{
					found = true;
					break;
					}
				}
			return found;
			}
		}
    

    }
