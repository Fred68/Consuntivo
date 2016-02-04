
// #define DEBUGDATA

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
	/// Logica di interazione per OpEditWindow.xaml
	/// </summary>
	public partial class OpEditWindow : Window
		{
		Operazione op;
		Operazioni ops;

		static string SepNomiOps = "- ";

		public OpEditWindow(ref Operazione op, ref Operazioni ops)
			{
			InitializeComponent();
			this.op = op;
			this.ops = ops;
			this.btCancel.IsCancel = true;
			this.btOk.IsDefault = true;
			setText();
			}
		public void setText()
			{
			#if DEBUGDATA
			StringBuilder strb = new StringBuilder();
			strb.Append(op.ToString() + '\n');
			string[] cmp = op.ToString().Split(new char[] { Riga.Separatore.field });
			int ii = 0;
			foreach (string s in cmp)
				{
				strb.Append(ii.ToString() + ": " + s + '\n');
				ii++;
				}
			tbTesto.Text = strb.ToString();
			#endif
			tbNota.Text = op.nota;
			dpData.SelectedDate = op.getData(true);
			tbDescrizione.Text = op.descrizione;
			tbImporto.Text = op.importo.ToString("C");
			chkConsuntivo.IsChecked = op.consuntivo;
			chkVerificato.IsChecked = op.verificato;
			foreach (Operazione.Tipo tp in Enum.GetValues(typeof(Operazione.Tipo)))
				{
				cbTipo.Items.Add(tp);
				}
			cbTipo.SelectedValue = op.tipo;
			FillListOpStandard();
			tbConti.Text = op.conti;
			
			tbConti.IsReadOnly = true;
			cbOperazioniStandard.IsReadOnly = true;

			}
		void FillListOpStandard()
			{
			cbOperazioniStandard.Items.Clear();
			object selectedItem = null;
			foreach(OpStandard x in ops.OpStandard())
				{
				string item = x.numero + OpEditWindow.SepNomiOps + x.descrizione;
				cbOperazioniStandard.Items.Add(item);
				if (x.numero == op.tipostd)
					selectedItem = item;
				}
			if(selectedItem != null)
				cbOperazioniStandard.SelectedItem = selectedItem;
			}
		int FindOpStandardSelezionata()
			{
			int nsel = -1;
			if (cbOperazioniStandard.SelectedIndex != -1)
				{
				string str = cbOperazioniStandard.SelectedItem.ToString();
				int i = str.IndexOf(OpEditWindow.SepNomiOps);
				if (i != -1)
					{
					int tmp;
					string ns = str.Substring(0, i);
					if (int.TryParse(ns, out tmp))
						{
						nsel = int.Parse(ns);
						}
					}
				}
			return nsel;
			}
		private void btCancel_Click(object sender, RoutedEventArgs e)
			{
			Close();
			}

		private void btOk_Click(object sender, RoutedEventArgs e)
			{
			op.nota = tbNota.Text;
			DateTime tmp;
			if (dpData.SelectedDate != null)
				{
				tmp = (DateTime)dpData.SelectedDate;
				op.setData(tmp);
				}
			op.descrizione = tbDescrizione.Text;
			op.importo = Riga.String2DecimalOrZero(tbImporto.Text);
			op.consuntivo = ((bool)chkConsuntivo.IsChecked) ? true : false;
			op.verificato = ((bool)chkVerificato.IsChecked) ? true : false;
			op.tipo = (Operazione.Tipo)cbTipo.SelectedIndex;
			int opsSel = FindOpStandardSelezionata();
			if(opsSel != -1)
				op.tipostd = opsSel;
			op.conti = tbConti.Text;
			Close();
			}

		private void tbConti_GotFocus(object sender, RoutedEventArgs e)
			{
			List<CheckItem> lchk;
			ListWindow w = new ListWindow(ops, out lchk, op);
			bool? res = w.ShowDialog();
			if (res.HasValue && res.Value == true)
				{
				StringBuilder strb = new StringBuilder();
				bool first = true;
				foreach (CheckItem it in lchk)
					{
					if(it.check == true)
						{
						if (!first)
							{
							strb.Append(" ");
							}
						else
							{
							first = false;
							}
						if (it.sottrai == true)
							strb.Append("-");
						strb.Append(it.number.ToString());
						}
					}
				tbConti.Text = strb.ToString();
				//MessageBox.Show(strb.ToString());
				}
			Keyboard.ClearFocus();          // Toglie la selezione della casella
			}
		}
	}
