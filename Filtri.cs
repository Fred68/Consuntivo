using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StringExtension;

// Attenzione: il file contiene più classi collegate

namespace WPF02
	{
	public class Filtro
		{

		public enum Condizione {  AND, OR};

		string nota;
		DateTime?[] data;
		string descrizione;
		decimal?[] importo;
		bool? consuntivo;
		bool? verificato;
		Riga.Tipo? tipo;

		Condizione operatoreLogico;

		List<bool> test;

		#region PROPRIETA
		public string Nota
			{
			get { return nota; }
			set { nota = value; }
			}
		public DateTime? DataDa
			{
			get { return data[0]; }
			set { data[0] = value; }
			}
		public DateTime? DataA
			{
			get { return data[1]; }
			set { data[1] = value; }
			}
		public string strDataDa
			{
			get { return Data2String(data[0]); }
			set { data[0] = String2Date(value); }
			}
		public string strDataA
			{
			get { return Data2String(data[1]); }
			set { data[1] = String2Date(value); }
			}
		public string Descrizione
			{
			get { return descrizione; }
			set { descrizione = value; }
			}
		public decimal? ImportoMin
			{
			get { return importo[0]; }
			set { importo[0] = value; }
			}
		public decimal? ImportoMax
			{
			get { return importo[1]; }
			set { importo[1] = value; }
			}
		public bool? Consuntivo
			{
			get { return consuntivo; }
			set { consuntivo = value; }
			}
		public bool? Verificato
			{
			get { return verificato; }
			set { verificato = value; }
			}
		public Riga.Tipo? Tipo
			{
			get { return tipo; }
			set { tipo = value; }
			}

		public Condizione OperatoreLogico
			{
			get { return operatoreLogico; }
			set { operatoreLogico = value; }
			}
		public bool IsAttivo;
		#endregion

		private string Data2String(DateTime? dt)
			{
			string tmp;
			if (dt == null)
				tmp = string.Empty;
			else
				tmp = Riga.DateTime2String((DateTime)dt);
			return tmp;
			}
		private DateTime? String2Date(string strDate)
			{
			DateTime? tmp;
			if (strDate == "")
				tmp = null;
			else
				tmp = Riga.String2DateTime(strDate);
			return tmp;
			}
		private void ClearTest()
			{
			test.Clear();
			}
		public Filtro()
			{
			data = new DateTime?[2];
			importo = new decimal?[2];
			test = new List<bool>();
			Clear();
			}		
		public void Clear()
			{
			nota = descrizione = string.Empty;
			data[0] = data[1] = null;
			importo[0] = importo[1] = null;
			consuntivo = verificato = null;
			tipo = null;
			IsAttivo = false;
			operatoreLogico = Condizione.AND;
			ClearTest();
			}
		public bool Check(Operazione op)
			{
			bool ok = false;
			ClearTest();
			if (nota != "")				test.Add(op.nota.ContainsWithWildcards(nota));
			if (DataDa != null)			test.Add((op.getData(true) >= data[0]));
			if (DataA != null)			test.Add((op.getData(true) <= data[1]));
			if (descrizione != "")		test.Add(op.descrizione.ContainsWithWildcards(descrizione));
			if (ImportoMin != null)		test.Add((Math.Abs(op.importo) >= importo[0]));
			if (ImportoMax != null)		test.Add((Math.Abs(op.importo) <= importo[1]));
			if (Consuntivo != null)		test.Add((op.consuntivo == consuntivo));
			if (Verificato != null)		test.Add((op.verificato == verificato));
			if (Tipo != null)			test.Add((op.tipo == tipo));

			switch(OperatoreLogico)
				{
				case Condizione.AND:
					ok = true;
					foreach (bool x in test)
						ok = ok && x;
					break;
				case Condizione.OR:
					ok = false;
					foreach (bool x in test)
						ok = ok || x;
					break;
				default:
					ok = false;
					break;
				}
			

			return ok;
			}
						
		}
	}
