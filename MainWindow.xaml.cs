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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;       // Per Observable collection
using System.Windows.Markup;                // Per XMLlanguage
using System.Globalization;                 // Per Culture info
// using System.Windows.Documents;



namespace WPF02
    {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
        {
		static string SepNomiConti = "- ";

		Operazioni operazioni;

		public MainWindow()
            {
			this.Language = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag);
			InitializeComponent();
			operazioni = new Operazioni();
			UpdateTitle();
			UpdateLblStatus();
			}

        void OpenWithDialog()
			{
			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
			dlg.DefaultExt = ".txt";
			dlg.Filter = "Text documents (.txt)|*.txt";
			Nullable<bool> rt = dlg.ShowDialog();
			if (rt == true)
				{
				if (MessageBox.Show("Tutti i dati non salvati verranno persi.\nProcedere ?", "Nuovo archivio", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
					{
					operazioni.New();
					AfterOpenSave(false, operazioni.Open(dlg.FileName));
					}
				}
			}
		void SaveAsWithDialog()
			{
			Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
			dlg.DefaultExt = ".txt";
			dlg.Filter = "Text documents (.txt)|*.txt";
			Nullable<bool> rt = dlg.ShowDialog();
			if (rt == true)
				{
				AfterOpenSave(true, operazioni.Save(dlg.FileName));
				}
			}
		void SaveWithDialog()
			{
			if (operazioni.Filename.Length > 0)
				{
				AfterOpenSave(true, operazioni.Save(operazioni.Filename));
				}
			else
				SaveAsWithDialog();
			}
		void AfterOpenSave(bool bSave, bool ok = true)
			{
			string str1 = (bSave ? "\nsalvato." : "\ncaricato");
			string str2 = (bSave ? "Errore salvataggio" : "Errore caricamento");
			if (ok)
				{
				dgOperazioni.CommitEdit();
				dgOperazioni.CancelEdit();
				dgConti.CommitEdit();
				dgConti.CancelEdit();
				dgOpStandard.CommitEdit();
				dgOpStandard.CancelEdit();
				dgOperazioni.Items.Refresh();
				dgConti.Items.Refresh();
				dgOpStandard.Items.Refresh();
				UpdateLstConti();
				UpdateTitle();
				UpdateLblStatus();
				operazioni.Check();
				this.InvalidateVisual();
				}
			else
				MessageBox.Show(str2);
			}
		void UpdateLstConti()
			{
			if(!operazioni.contiUpdated)
				{
				//lstConti.ItemsSource = operazioni.conti.ToList();
				FillLstConti();
				operazioni.contiUpdated = true;
				}
			}
		void FillLstConti()
			{
			lstConti.Items.Clear();
			foreach(Conto c in operazioni.conti)
				{
				lstConti.Items.Add(c.numero + MainWindow.SepNomiConti + c.descrizione);
				}
			}
		void UpdateTitle()
			{
			if(operazioni.Filename.Length>0)
				this.Title = "Consuntivo " + operazioni.Filename;
			else
				this.Title = "Consuntivo " + "<nessun file>";
			}
		FlowDocument FillFlowDocument(ref FlowDocument doc, Size pageSize)   // Per stampa
			{
			doc.ColumnWidth = doc.MaxPageWidth = pageSize.Width;
			doc.MaxPageHeight = pageSize.Height;
			
			// Stampa le Operazioni
			Operazione nouse = new Operazione();
			Table tOp = new Table();
			int ncol = nouse.Count();
			
			foreach (TipoColonna str in nouse.Tipi())
				{
				TableColumn tc = new TableColumn();
				tc.Width = new GridLength(str.larghezzaColonna, GridUnitType.Pixel);
				tOp.Columns.Add(tc);
				}

			var rg = new TableRowGroup();
			
			TableRow rowint = new TableRow();
			rowint.Background = Brushes.Transparent;
			rowint.FontSize = 12;
			rowint.FontWeight = FontWeights.Bold;
			foreach (string str in nouse.Titoli())
				{
				TableCell tc = new TableCell( new Paragraph(new Run(str)));
				tc.BorderBrush = Brushes.Black;
				tc.BorderThickness = new Thickness(0, 1, 0, 1);
				rowint.Cells.Add(tc);
				}
			rg.Rows.Add(rowint);

			TableRow rowtyp = new TableRow();
			rowtyp.Background = Brushes.Transparent;
			rowtyp.FontSize = 12;
			rowtyp.FontWeight = FontWeights.Bold;
			#if false
			foreach (TipoColonna str in nouse.Tipi())
				{
				TableCell tc = new TableCell(new Paragraph(new Run(str.tipo)));
				tc.BorderBrush = Brushes.Black;
				tc.BorderThickness = new Thickness(0, 1, 0, 1);
				rowtyp.Cells.Add(tc);
				}
			#endif
			rg.Rows.Add(rowtyp);

			foreach (Operazione op in operazioni.operazioni)
				{
				TableRow row = new TableRow();
				row.Background = Brushes.Transparent;
				row.FontSize = 10;
				row.FontWeight = FontWeights.Normal;
				foreach (string str in op.Valori())
					{
					TableCell tc = new TableCell(new Paragraph(new Run(str)));
					tc.BorderBrush = Brushes.Black;
					tc.BorderThickness = new Thickness(0, 0, 0, 0.5);
					row.Cells.Add(tc);
					//row.Cells.Add(new TableCell(new Paragraph(new Run(str))));
					}
				rg.Rows.Add(row);
				}
			tOp.RowGroups.Add(rg);
			doc.Blocks.Add(tOp);

			// Interruzione di pagine
			Section section = new Section();
			section.BreakPageBefore = true;
			doc.Blocks.Add(section);

			// Conti
			Conto nouseC = new Conto();
			Table tCn = new Table();
			int ncolC = nouseC.Count();
			for (int i = 0; i < ncolC; i++)
				{
				TableColumn tc = new TableColumn();
				tc.Width = new GridLength(70, GridUnitType.Pixel);
				tCn.Columns.Add(tc);
				}
			var rgC = new TableRowGroup();

			TableRow rowintC = new TableRow();
			rowintC.Background = Brushes.Transparent;
			rowintC.FontSize = 12;
			rowintC.FontWeight = FontWeights.Bold;
			foreach (string str in nouseC.Titoli())
				{
				TableCell tc = new TableCell(new Paragraph(new Run(str)));
				tc.BorderBrush = Brushes.Black;
				tc.BorderThickness = new Thickness(0, 1, 0, 1);
				rowintC.Cells.Add(tc);
				}
			rgC.Rows.Add(rowintC);

			TableRow rowtypC = new TableRow();
			rowtypC.Background = Brushes.Transparent;
			rowtypC.FontSize = 12;
			rowtypC.FontWeight = FontWeights.Bold;
			#if false
			foreach (TipoColonna str in nouseC.Tipi())
				{
				TableCell tc = new TableCell(new Paragraph(new Run(str.tipo)));
				tc.BorderBrush = Brushes.Black;
				tc.BorderThickness = new Thickness(0, 1, 0, 1);
				rowtypC.Cells.Add(tc);
				}
			#endif
			rgC.Rows.Add(rowtypC);

			foreach (Conto op in operazioni.conti)
				{
				TableRow row = new TableRow();
				row.Background = Brushes.Transparent;
				row.FontSize = 10;
				row.FontWeight = FontWeights.Normal;
				foreach (string str in op.Valori())
					{
					TableCell tc = new TableCell(new Paragraph(new Run(str)));
					tc.BorderBrush = Brushes.Black;
					tc.BorderThickness = new Thickness(0, 0, 0, 0.5);
					row.Cells.Add(tc);
					}
				rgC.Rows.Add(row);
				}
			tCn.RowGroups.Add(rgC);

			doc.Blocks.Add(tCn);
			
			// Interruzione di pagina
			Section section2 = new Section();
			section2.BreakPageBefore = true;
			doc.Blocks.Add(section2);

			// Op standard
			OpStandard nouseS = new OpStandard();
			Table tSn = new Table();
			int ncolS = nouseS.Count();
			for (int i = 0; i < ncolS; i++)
				{
				TableColumn tc = new TableColumn();
				tc.Width = new GridLength(70, GridUnitType.Pixel);
				tSn.Columns.Add(tc);
				}
			var rgS = new TableRowGroup();

			TableRow rowintS = new TableRow();
			rowintS.Background = Brushes.Transparent;
			rowintS.FontSize = 12;
			rowintS.FontWeight = FontWeights.Bold;
			foreach (string str in nouseS.Titoli())
				{
				TableCell tc = new TableCell(new Paragraph(new Run(str)));
				tc.BorderBrush = Brushes.Black;
				tc.BorderThickness = new Thickness(0, 1, 0, 1);
				rowintS.Cells.Add(tc);
				}
			rgS.Rows.Add(rowintS);

			TableRow rowtypS = new TableRow();
			rowtypS.Background = Brushes.Transparent;
			rowtypS.FontSize = 12;
			rowtypS.FontWeight = FontWeights.Bold;
			#if false
			foreach (TipoColonna str in nouseS.Tipi())
				{
				TableCell tc = new TableCell(new Paragraph(new Run(str.tipo)));
				tc.BorderBrush = Brushes.Black;
				tc.BorderThickness = new Thickness(0, 1, 0, 1);
				rowtypS.Cells.Add(tc);
				}
			#endif
			rgS.Rows.Add(rowtypS);

			foreach (OpStandard op in operazioni.opStandard)
				{
				TableRow row = new TableRow();
				row.Background = Brushes.Transparent;
				row.FontSize = 10;
				row.FontWeight = FontWeights.Normal;
				foreach (string str in op.Valori())
					{
					TableCell tc = new TableCell(new Paragraph(new Run(str)));
					tc.BorderBrush = Brushes.Black;
					tc.BorderThickness = new Thickness(0, 0, 0, 0.5);
					row.Cells.Add(tc);
					}
				rgS.Rows.Add(row);
				}
			tSn.RowGroups.Add(rgS);

			doc.Blocks.Add(tSn);


			StringBuilder strb = new StringBuilder();
			 
			foreach (List<Consuntivo> lc in operazioni.ListeConsuntivi())
				{
				strb.Append("---\n");
				foreach (Consuntivo cons in lc)
					{
#warning da completare, previa verifica accessibilità 
					strb.Append(cons.ToString() + '\n');
					}
				}
			MessageBox.Show(strb.ToString());

			return doc;
			}
		private void ModificaElementoSelezionato()
			{
			int i = dgOperazioni.SelectedIndex;
			if ((i > -1) && (i < dgOperazioni.Items.Count))
				{
				try
					{
					Operazione op = (Operazione)dgOperazioni.SelectedItem;
					if (op != null)
						{
						OpEditWindow oped = new OpEditWindow(ref op, ref operazioni);
						bool? ret = oped.ShowDialog();
							{
							if((ret.HasValue) && (ret.Value== true))
								{
								dgOperazioni.CommitEdit();
								dgOperazioni.CancelEdit();
								dgOperazioni.Items.Refresh();
								}
							}
						}
					else
						MessageBox.Show("selezione errata");
					}
				catch (Exception ex)
					{
					MessageBox.Show(ex.ToString());
					}
				}
			}
		string ViewErrors()
			{
			StringBuilder strb = new StringBuilder();
			int n = operazioni.ErroriCount();
			if (n > 0)
				{
				foreach (string str in operazioni.Errori())
					{
					strb.Append(str+"\n");
					}
				}
			else
				{
				strb.Append("Nessun errore!");
				}
			return strb.ToString();
			}
		int FindContoConsSelezionato()
			{
			int nsel = -1;
			if (lstConti.SelectedIndex != -1)
				{
				string str = lstConti.SelectedItem.ToString();
				int i = str.IndexOf(MainWindow.SepNomiConti);
				if(i!=-1)
					{
					int tmp;
					string ns = str.Substring(0, i);
					if(int.TryParse(ns, out tmp))
						{
						nsel = int.Parse(ns);
						}
					}
				}
			return nsel;
			}
		private void dgOperazioni_Loaded(object sender, RoutedEventArgs e)
            {
			operazioni.setDatiGrid(this.dgOperazioni, this.dgConti, this.dgOpStandard, this.dgConsuntivi);
			this.lstConti.IsReadOnly = true;
			Filtra.Content = "Applica filtro";
			}
        private void buttonVediProprieta_Click(object sender, RoutedEventArgs e)
            {
            MessageBox.Show(new Operazione().ListProperties());
            }
		private void listaOperazioni_Click(object sender, RoutedEventArgs e)
			{
			MessageBox.Show(operazioni.ListaOperazioni());
			}
		private void Window_Loaded(object sender, RoutedEventArgs e)
			{
			operazioni.azioneStatus = UpdateLblStatus;
			}
		private void VediMsg_Click(object sender, RoutedEventArgs e)
			{
			StringBuilder strb = new StringBuilder();
			strb.Append("Azioni eseguite:\n");
			foreach (string str in operazioni.Messaggi())
				{
				strb.Append(str + '\n');
				}
			MessageBox.Show(strb.ToString());
			}
		private void Filtra_Click(object sender, RoutedEventArgs e)
			{
			if (!operazioni.filtro.IsAttivo)
				{
				operazioni.ApplicaFiltro();
				Filtra.Content = "Annulla filtro";
				}
			else
				{
				operazioni.CancellaFiltro();
				Filtra.Content = "Applica filtro";
				}
			}
		private void editSelected_Click(object sender, RoutedEventArgs e)
			{
			ModificaElementoSelezionato();
			dgOperazioni.CommitEdit();
			dgOperazioni.Items.Refresh();
			dgOperazioni.InvalidateVisual();
			}
		private void dgOperazioni_MouseDoubleClick(object sender, MouseButtonEventArgs e)
			{
			ModificaElementoSelezionato();
			dgOperazioni.CancelEdit();
			}
		private void Inserisci_Click(object sender, RoutedEventArgs e)
			{
			Operazione op = (Operazione)dgOperazioni.SelectedItem;
			if (!operazioni.OpInserisciAt(op))
				MessageBox.Show("Errore inserimento");
			}
		private void New_Click(object sender, RoutedEventArgs e)
			{
			if(MessageBox.Show("Tutti i dati non salvati verranno persi.\nProcedere ?","Nuovo archivio",MessageBoxButton.YesNo)==MessageBoxResult.Yes)
				{
				operazioni.New();
				UpdateTitle();
				UpdateLblStatus();
				}
			}
		private void Open_Click(object sender, RoutedEventArgs e)
			{
			OpenWithDialog();
			}
		private void Save_Click(object sender, RoutedEventArgs e)
			{
			SaveWithDialog();
			}
		private void SaveAs_Click(object sender, RoutedEventArgs e)
			{
			SaveAsWithDialog();
			}
		private void Close_Click(object sender, RoutedEventArgs e)
			{
			Close();
			}
		private void View_Click(object sender, RoutedEventArgs e)
			{
			StringBuilder strb = new StringBuilder();
			strb.Append("Filename: " + operazioni.Filename + '\n');
			strb.Append(operazioni.ListaOperazioni());
			strb.Append(operazioni.ListaConti());
			strb.Append(operazioni.ListaOpStandard());
			MessageBox.Show(strb.ToString());
			}
		private void EnableLog_Click(object sender, RoutedEventArgs e)
			{
			operazioni.LogAbilitato = true;
			}
		private void DisableLog_Click(object sender, RoutedEventArgs e)
			{
			operazioni.LogAbilitato = false;
			}
		private void ClearLog_Click(object sender, RoutedEventArgs e)
			{
			operazioni.CancellaLog();
			}
		private void ViewLog_Click(object sender, RoutedEventArgs e)
			{
			MessageBox.Show(operazioni.MsgList());
			}
		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
			{
			if(MessageBox.Show("Quit ?","Please confirm...",MessageBoxButton.YesNo)==MessageBoxResult.No)
				e.Cancel = true;
			}
		private void buttonPausa_Click(object sender, RoutedEventArgs e)
			{
			MessageBox.Show("Pausa");
			if (System.Diagnostics.Debugger.IsAttached)
				System.Diagnostics.Debugger.Break();
			}
		private void dgOperazioni_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
			{
			
			}
		private void dgOperazioni_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
			{
			//if(e.PropertyType == typeof(System.DateTime))
			//	{
			//	(e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";
			//	}
			if (e.PropertyType == typeof(System.Decimal))
				{
				(e.Column as DataGridTextColumn).Binding.StringFormat = "C";
				#warning AGGIUNGERE ALLINEAMENTO A DESTRA
				}

			}
		private void Print_Click(object sender, RoutedEventArgs e)
			{
			PrintDialog printDlg = new PrintDialog();
			if (printDlg.ShowDialog() == true)
				{
				Size pageSize = new Size(printDlg.PrintableAreaWidth, printDlg.PrintableAreaHeight);
				FlowDocument doc = new FlowDocument();
				doc.ColumnWidth = doc.MaxPageWidth = pageSize.Width;
				doc.MaxPageHeight = pageSize.Height;
				FillFlowDocument(ref doc, pageSize);
				doc.Name = "FlowDoc";
				IDocumentPaginatorSource idpSource = doc;
				string prName = "Consuntivo" + (operazioni.Filename.Length>0 ? " - "+ operazioni.Filename : "");
				printDlg.PrintDocument(idpSource.DocumentPaginator, prName);
				}

			//Operazione op = new Operazione("nota..", DateTime.Now, "Nuova operazione", 1000, Operazione.Tipo.A, 1, "1,2,3");
			//StringBuilder strb = new StringBuilder();
			//foreach (string p in op.Titoli())
			//	{
			//	strb.Append(p + Operazione.Separatore.line);
			//	}
			//for(int x=0;x<2;x++)
			//	foreach (string p in op.Valori())
			//		{
			//		strb.Append(p + Operazione.Separatore.line);
			//		}
			//MessageBox.Show(strb.ToString());


			//MessageBoxResult mbr =	MessageBox.Show("Print semplice ?","Si`: semplice, no: complesso",MessageBoxButton.YesNoCancel);
			//switch(mbr)
			//	{
			//	case MessageBoxResult.Yes:
			//		{
			//		PrintDialog printDlg = new PrintDialog();
			//		if (printDlg.ShowDialog() == true)
			//			{
			//			printDlg.PrintVisual(dgOperazioni, "Operazioni");
			//			}
			//		}
			//		break;
			//	case MessageBoxResult.No:
			//			{
			//			PrintDialog printDlg = new PrintDialog();
			//			if (printDlg.ShowDialog() == true)
			//				{
			//				FlowDocument doc = CreateFlowDocument();
			//				doc.Name = "FlowDoc";
			//				IDocumentPaginatorSource idpSource = doc;
			//				printDlg.PrintDocument(idpSource.DocumentPaginator, "OperazioniComplesso");
			//				}
			//		}
			//		break;
			//	default:
			//		break;
			//	}
			
			}
		private void lstConti_GotFocus(object sender, RoutedEventArgs e)
			{
			UpdateLstConti();
			}
		private void btVisualizzaCons_Click(object sender, RoutedEventArgs e)
			{
			string msg = "";
			int contosel = FindContoConsSelezionato();
			if(contosel != -1)
				{
				if (operazioni.Check())
					{
					// msg = "Conto selezionato: " + contosel.ToString();
					Conto tmp = this.operazioni.FindConto(contosel);
					// tmp = this.operazioni.
					// string tmp = contosel.ToString + MainWindow.SepNomiConti + 
					lblContoCons.Content = tmp.numero.ToString() + MainWindow.SepNomiConti + tmp.descrizione;

					operazioni.setConsuntivoGrid(dgConsuntivi, contosel);
					
					}
				else
					{
					msg = ("Impossibile generare il consuntivo.\nCorreggere gli errori.");
					}
				}
			else
				{
				msg = "Nessun conto selezionato.";
				}
			dgOperazioni.CommitEdit();
			dgOperazioni.CancelEdit();
			dgOperazioni.Items.Refresh();
			if(msg.Length > 0)	MessageBox.Show(msg);
			}
		private void lstConti_SelectionChanged(object sender, SelectionChangedEventArgs e)
			{
			string msg = "";
			int contosel = FindContoConsSelezionato();
			if (contosel != -1)
				{
				if (operazioni.Check())
					{
					// msg = "Conto selezionato: " + contosel.ToString();
					Conto tmp = this.operazioni.FindConto(contosel);
					// tmp = this.operazioni.
					// string tmp = contosel.ToString + MainWindow.SepNomiConti + 
					lblContoCons.Content = tmp.numero.ToString() + MainWindow.SepNomiConti + tmp.descrizione;

					operazioni.setConsuntivoGrid(dgConsuntivi, contosel);

					}
				else
					{
					msg = ("Impossibile generare il consuntivo.\nCorreggere gli errori.");
					}
				}
			else
				{
				msg = "Nessun conto selezionato.";
				}
			dgOperazioni.CommitEdit();
			dgOperazioni.CancelEdit();
			dgOperazioni.Items.Refresh();
			if (msg.Length > 0) MessageBox.Show(msg);
			}
		private void Check_Click(object sender, RoutedEventArgs e)
			{
			operazioni.Check();
			}
		private void ViewError_Click(object sender, RoutedEventArgs e)
			{
			// operazioni.Check();
			MessageBox.Show(ViewErrors());
			}
		public void UpdateLblStatus()
			{
			if (operazioni.status)
				{
				this.imageOk.Visibility = Visibility.Visible;
				this.imageErr.Visibility = Visibility.Hidden;
				this.lblStatus.Content = "ok";
				}
			else
				{
				this.imageOk.Visibility = Visibility.Hidden;
				this.imageErr.Visibility = Visibility.Visible;
				this.lblStatus.Content = "Errori";
				}
			}
		private void imageErr_MouseDown(object sender, MouseButtonEventArgs e)
			{
			if((e.ChangedButton == MouseButton.Left)&&(e.ClickCount==2))
				{
				MessageBox.Show(ViewErrors());
				}
			}
		private void Genera_Click(object sender, RoutedEventArgs e)
			{
			operazioni.EspandeOpStandard();
			if (operazioni.GeneraConsuntivi() == true)
				MessageBox.Show("Generati consuntivi");
			dgOperazioni.CommitEdit();
			dgOperazioni.CancelEdit();
			dgOperazioni.Items.Refresh();
			dgConsuntivi.Items.Refresh();
			}

		private void Set_Filter(object sender, RoutedEventArgs e)
			{
			FilterWindow w = new FilterWindow(ref operazioni.filtro);
			if (w.ShowDialog() == true)
				{
				//MessageBox.Show("Impostato filtro");
				}

			}
		}
    }
