using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF02
	{
	public class TipoColonna
		{
		public string tipo;
		public double larghezzaColonna;
		public TipoColonna()
			{
			tipo = "";
			larghezzaColonna = 0.0;
			}
		public TipoColonna(string tipo, double larghezzaColonna)
			{
			this.tipo = tipo;
			this.larghezzaColonna = larghezzaColonna;
			}
		}
	}
