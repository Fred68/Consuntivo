using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF02
	{
	class datetimeString : Riga
		{
		DateTime dt;
		public DateTime data
			{
			get { return dt; }
			set { dt = value; }
			}
		public datetimeString()
			{
			dt = System.DateTime.Now; 
			}
		public datetimeString(DateTime datetime)
			{
			dt = datetime;
			}
		public override string ToString()
			{
			//return dt.ToString("d");
			return getData();
			}

		static public implicit operator DateTime(datetimeString value)
			{
			return value.dt;
			}
		static public implicit operator datetimeString(DateTime value)
			{
			return new datetimeString(value);
			}

		string getData()                            // Restituisce la data (in stringa)
			{
			return dt.ToString("d");
			}
		void setData(string d)                      // Imposta la data (da stringa)
			{
			dt = String2DateTime(d);
			}


		}
	}
