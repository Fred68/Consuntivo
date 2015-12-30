
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

		public OpEditWindow(ref Operazione op)
			{
			InitializeComponent();
			this.op = op;
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
			tbOperazioneStandard.Text = op.tipostd.ToString();
			tbConti.Text = op.conti;
			}
		private void btCancel_Click(object sender, RoutedEventArgs e)
			{
			Close();
			}

		private void btOk_Click(object sender, RoutedEventArgs e)
			{
			op.nota = tbNota.Text;
			op.setData(dpData.DisplayDate);
			op.descrizione = tbDescrizione.Text;
			op.importo = Riga.String2DecimalOrZero(tbImporto.Text);
			op.consuntivo = ((bool)chkConsuntivo.IsChecked) ? true : false;
			op.verificato = ((bool)chkVerificato.IsChecked) ? true : false;
			op.tipo = (Operazione.Tipo)cbTipo.SelectedIndex;
			op.tipostd = Riga.String2IntOrZero(tbOperazioneStandard.Text);
			op.conti = tbConti.Text;
		   Close();
			}
		}
	}
