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
	/// Logica di interazione per FilterWindow.xaml
	/// </summary>
	public partial class FilterWindow : Window
		{

		Filtro filtro;

		public FilterWindow(ref Filtro filtro)
			{
			InitializeComponent();
			
			this.filtro = filtro;

			foreach (Riga.Tipo x in Enum.GetValues(typeof(Riga.Tipo)))
				{
				cbTipo.Items.Add(x);
				}
			foreach (Filtro.Condizione x in Enum.GetValues(typeof(Filtro.Condizione)))
				{
				cbCondizione.Items.Add(x);
				}
			cbCondizione.SelectedItem = Filtro.Condizione.AND;
			cbTipo.SelectedItem = Riga.Tipo.P;
			chkVerificato.IsThreeState = true;
			chkConsuntivo.IsThreeState = true;
			btAnnulla.IsCancel = true;
			btOK.IsDefault = true;

			ClearWindow();

			SetWindow();
			}

		public void SetWindow()
			{
			if (filtro.Nota != "")
				tbNota.Text = filtro.Nota;
			if (filtro.DataDa != null)
				dtFrom.SelectedDate = filtro.DataDa;
			if (filtro.DataA != null)
				dtTo.SelectedDate = filtro.DataA;
			if (filtro.Descrizione != "")
				tbDescrizione.Text = filtro.Descrizione;
			if (filtro.ImportoMin != null)
				tbImportoMin.Text = ((decimal)filtro.ImportoMin).ToString();
			if (filtro.Consuntivo != null)
				chkConsuntivo.IsChecked = filtro.Consuntivo;
			else
				chkConsuntivo.IsChecked = null;
			
			if (filtro.Verificato != null)
				chkVerificato.IsChecked = filtro.Verificato;
			else
				chkVerificato.IsChecked = null;

#warning COMPLETARE con condizione AND / OR da aggiungere in Filtro !

			if (filtro.OperatoreLogico == Filtro.Condizione.AND)
				cbCondizione.SelectedItem = Filtro.Condizione.AND;
			else
				cbCondizione.SelectedItem = Filtro.Condizione.OR;
			
			}
		public void ClearWindow()
			{
			tbNota.Text = "";
			dtFrom.SelectedDate = null;
			dtTo.SelectedDate = null;
			tbDescrizione.Text = "";
			tbImportoMin.Text = "";
			tbImportoMax.Text = "";
			chkConsuntivo.IsChecked = null;
			chkVerificato.IsChecked = null;
			cbCondizione.SelectedItem = Filtro.Condizione.AND;
			}
		public void SetFilter()
			{
			if (tbNota.Text.Length > 0)
				filtro.Nota = tbNota.Text;
			else
				filtro.Nota = "";

			if (dtFrom.SelectedDate != null)
				filtro.DataDa = dtFrom.SelectedDate;
			else
				filtro.DataDa = null;

			if (dtTo.SelectedDate != null)
				filtro.DataA = dtTo.SelectedDate;
			else
				filtro.DataA = null;

			if (tbDescrizione.Text.Length > 0)
				filtro.Descrizione = tbDescrizione.Text;
			else
				filtro.Descrizione = "";

			if (tbImportoMin.Text.Length > 0)
				{
				filtro.ImportoMin = Math.Abs(Riga.String2DecimalOrZero(tbImportoMin.Text));
				}
			else
				filtro.ImportoMin = null;

			if (tbImportoMax.Text.Length > 0)
				{
				filtro.ImportoMax = Math.Abs(Riga.String2DecimalOrZero(tbImportoMax.Text));
				}
			else
				filtro.ImportoMax = null;


			if (chkConsuntivo.IsChecked.HasValue)
				{
				if (chkConsuntivo.IsChecked == true)
					filtro.Consuntivo = true;
				else
					filtro.Consuntivo = false;
				}
			else
				filtro.Consuntivo = null;

			if (chkVerificato.IsChecked.HasValue)
				{
				if (chkVerificato.IsChecked == true)
					filtro.Verificato = true;
				else
					filtro.Verificato = false;
				}
			else
				filtro.Verificato = null;



#warning COMPLETARE con condizione AND / OR da aggiungere in Filtro

			filtro.OperatoreLogico = (Filtro.Condizione)cbCondizione.SelectedIndex;
			
			}

		private void btOK_Click(object sender, RoutedEventArgs e)
			{
			SetFilter();
			this.DialogResult = true;
			Close();
			}

		private void btAnnulla_Click(object sender, RoutedEventArgs e)
			{
			this.DialogResult = false;
			Close();
			}

		private void btReset_Click(object sender, RoutedEventArgs e)
			{
			ClearWindow();
			}
		}
	}
