using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF02
	{
	public class CheckItem
		{

		#region PROPRIETA
		public bool check { get; set; }
		public bool sottrai { get; set; }
		public int number { get; }
		public string descrizione { get; }
		#endregion

		public CheckItem()
			{
			this.check = false;
			this.number = 0;
			this.descrizione = "-";
			this.sottrai = true;
			}
		public CheckItem(bool check, int number, bool segno, string descrizione)
			{
			this.check = check;
			this.number = number;
			this.descrizione = descrizione;
			this.sottrai = segno;
			}
		}		
	}
