﻿using System;
using System.Text;
using System.Reflection;            // Per property info
using System.Collections.Generic;

namespace WPF02
	{
	public class Conto : Riga
		{
		public const int CAMPI = 3;

		#region PROPERTIES
		public int numero { get; set; }
		public string descrizione { get; set; }
		public string nota { get; set; }
		#endregion
		public static Dictionary<string, TipoColonna> larghezze;

		static Conto()
			{
			larghezze = new Dictionary<string, TipoColonna>();
			foreach (PropertyInfo prop in typeof(Conto).GetProperties())
				{
				larghezze.Add(prop.Name, new TipoColonna(prop.PropertyType.ToString(), 0));
				}
			larghezze["numero"].larghezzaColonna = 50;
			larghezze["descrizione"].larghezzaColonna = 130;
			larghezze["nota"].larghezzaColonna = 130;
			}
		public Conto()
			{
			numero = 0;
			descrizione = "-";
			}
		public Conto(int numero, string descrizione, string nota)
			{
			this.numero = numero;
			this.descrizione = descrizione;
			this.nota = nota;
			}
		public override string ToString()
			{
			StringBuilder strb = new StringBuilder();
			strb.Append(numero.ToString() + Separatore.field);
			strb.Append(Conto.CleanString(descrizione) + Separatore.field);
			strb.Append(Conto.CleanString(nota));
			return strb.ToString();
			}
		public override bool FromString(string str)
			{
			bool ok = false;
			string[] cmp = str.ToString().Split(new char[] { Separatore.field });
			if (cmp.Length == CAMPI)
				{
				Conto tmp = new Conto();
				tmp.numero = Conto.String2IntOrZero(cmp[0]);
				tmp.descrizione = cmp[1];
				tmp.nota = cmp[2];
				ok = true;
				if (ok)
					{
					this.numero = tmp.numero;
					this.descrizione = tmp.descrizione;
					this.nota = tmp.nota;
					}
				}
			return ok;
			}
		public override string ListProperties()           // Dialogo con lista delle proprietà
			{
			StringBuilder strb = new StringBuilder();
			foreach (PropertyInfo prop in typeof(Operazione).GetProperties())
				{
				strb.Append(prop.Name + '\t' + prop.PropertyType.ToString() + System.Environment.NewLine);
				}
			return strb.ToString();
			}
		public override int Count()
			{
			int i = 0;
			foreach (PropertyInfo prop in typeof(Conto).GetProperties())
				i++;
			return i;
			}
		public override string Intestazione()
			{
			return Conto.Separatore.header.ToString() + "CONTI";
			}
		public override IEnumerable<string> Valori()           // I valori (in stringa)
			{
			foreach (PropertyInfo prop in typeof(Conto).GetProperties())
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
			foreach (PropertyInfo prop in typeof(Conto).GetProperties())
				{
				yield return prop.Name as string;
				}
			yield break;
			}
		public override IEnumerable<TipoColonna> Tipi()
			{
			foreach (PropertyInfo prop in typeof(Conto).GetProperties())
				{
				TipoColonna tipo = new TipoColonna(prop.PropertyType.ToString(), larghezze[prop.Name].larghezzaColonna);
				yield return tipo as TipoColonna;
				}
			yield break;
			}
		}
	}
