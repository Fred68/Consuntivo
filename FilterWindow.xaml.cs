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
			//if(filtro.Consuntivo != null)
			//	chkConsuntivo.S

#warning COMPLETARE !!!

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
			if (tbDescrizione.Text.Length > 0)
				filtro.Descrizione = tbDescrizione.Text;
			else
				filtro.Descrizione = "";
			
			#warning COMPLETARE !!!
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
