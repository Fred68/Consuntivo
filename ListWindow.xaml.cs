using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPF02
	{
	/// <summary>
	/// Logica di interazione per ListWindow.xaml
	/// </summary>
	public partial class ListWindow : Window
		{
		List<CheckItem> lchk;
		public ListWindow(Operazioni operazioni, out List<CheckItem> lst, Operazione op)
			{
			InitializeComponent();
			lchk = new List<CheckItem>();
			foreach (Conto x in operazioni.Conti())
				{
				bool set = false;
				bool sottrai = false;
				foreach (int nc in op.Conti())
					{
					if (Math.Abs(nc) == x.numero)
						{
						set = true;
						sottrai = (nc > 0) ? false : true;
						}
					}
				CheckItem tmp = new CheckItem(set, x.numero, sottrai, x.descrizione);
				lchk.Add(tmp);
				}
			dataGrid.AutoGenerateColumns = true;
			dataGrid.CanUserAddRows = false;
			dataGrid.CanUserDeleteRows = false;
			dataGrid.ItemsSource = lchk;
			lst = lchk;
			}
		private void btCancel_Click(object sender, RoutedEventArgs e)
			{
			this.DialogResult = false;
			Close();
			}
		private void btOK_Click(object sender, RoutedEventArgs e)
			{
			this.DialogResult = true;
			Close();
			}
		}
	}
