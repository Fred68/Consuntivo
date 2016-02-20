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



namespace WPF02
    {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
        {
		static string SepNomiConti = "- ";

		int headerSize = 10;
		int textSize = 10;

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
		#region PRINT
		FlowDocument FillFlowDocument(ref FlowDocument doc, Size pageSize)   // Per stampa
			{
			doc.ColumnWidth = doc.MaxPageWidth = pageSize.Width;
			doc.MaxPageHeight = pageSize.Height;

			Section[] sz = new Section[4];


			sz[0] = CreateSectionWithPageBreak();
			PrintTesto(ref sz[0], "OPERAZIONI");                              // Stampa le Operazioni (filtrate)
			Table tOp = new Table();
			Operazione opEmpty = new Operazione();
			TableRowGroup rgOp = new TableRowGroup();
			PrintTitoliTabella(ref tOp, ref rgOp, opEmpty);
			foreach (Operazione op in operazioni.operazioni)
				{
				PrintRigaTabella(ref tOp, ref rgOp, op);
				}
			tOp.RowGroups.Add(rgOp);
			sz[0].Blocks.Add(tOp);
			doc.Blocks.Add(sz[0]);
			
			sz[1] = CreateSectionWithPageBreak();
			PrintTesto(ref sz[1], "CONTI");                                   // Stampa i Conti
			Table tCn = new Table();
			Conto cnEmpty = new Conto();
			TableRowGroup rgCn = new TableRowGroup();
			PrintTitoliTabella(ref tCn, ref rgCn, cnEmpty);
			foreach (Conto cn in operazioni.conti)
				{
				PrintRigaTabella(ref tCn, ref rgCn, cn);
				}
			tCn.RowGroups.Add(rgCn);
			sz[1].Blocks.Add(tCn);
			doc.Blocks.Add(sz[1]);

			sz[2] = CreateSectionWithPageBreak();
			PrintTesto(ref sz[2], "OPERAZIONI STANDARD");                     // Stampa le Operazioni Standard
			Table tOs = new Table();
			OpStandard osEmpty = new OpStandard();
			TableRowGroup rgOs = new TableRowGroup();
			PrintTitoliTabella(ref tOs, ref rgOs, osEmpty);
			foreach (OpStandard os in operazioni.opStandard)
				{
				PrintRigaTabella(ref tOs, ref rgOs, os);
				}
			tOs.RowGroups.Add(rgOs);
			sz[2].Blocks.Add(tOs);
			doc.Blocks.Add(sz[2]);

			#region DA SCRIVERE

			sz[3] = CreateSectionWithPageBreak(true);
			foreach (Conto cnt in operazioni.Conti())
				{
				PrintTesto(ref sz[3], "CONSUNTIVO[" + cnt.numero.ToString() + "]: " + cnt.descrizione);
				List<Consuntivo> lc = operazioni.FindListaConsuntivi(cnt.numero);
				if (lc != null)
					{
					Table tCns = new Table();
					Consuntivo cnsEmpty = new Consuntivo();
					TableRowGroup rgCns = new TableRowGroup();
					PrintTitoliTabella(ref tCns, ref rgCns, cnsEmpty);
					foreach (Consuntivo cns in lc)
						{
						PrintRigaTabella(ref tCns, ref rgCns, cns);
						}
					tCns.RowGroups.Add(rgCns);
					sz[3].Blocks.Add(tCns);
					}
				else
					MessageBox.Show("Nessun consuntivo ["+cnt.numero.ToString()+"]");
				}
			doc.Blocks.Add(sz[3]);
#if false
			PrintPageBreak(ref doc);

			foreach (Conto cnt in operazioni.Conti())
				{
				PrintTesto(ref doc, "CONSUNTIVO[" + cnt.numero + "]: " + cnt.descrizione);

				List<Consuntivo> lc = operazioni.FindListaConsuntivi(cnt.numero);
				if(lc != null)
					{
					Consuntivo nouseCn = new Consuntivo();
					Table tCCn = new Table();
					int ncolCn = nouseCn.Count();
					for (int i = 0; i < ncolCn; i++)
						{
						TableColumn tc = new TableColumn();
						tc.Width = new GridLength(70, GridUnitType.Pixel);
						tCn.Columns.Add(tc);
						}
					var rgCn = new TableRowGroup();

					TableRow rowintCn = new TableRow();
					rowintCn.Background = Brushes.Transparent;
					rowintCn.FontSize = headerSize;
					rowintCn.FontWeight = FontWeights.Bold;
					foreach (string str in nouseCn.Titoli())
						{
						TableCell tc = new TableCell(new Paragraph(new Run(str)));
						tc.BorderBrush = Brushes.Black;
						tc.BorderThickness = new Thickness(0, 1, 0, 1);
						rowintC.Cells.Add(tc);
						}
					rgCn.Rows.Add(rowintCn);

					TableRow rowtypCn = new TableRow();
					rowtypCn.Background = Brushes.Transparent;
					rowtypCn.FontSize = headerSize;
					rowtypCn.FontWeight = FontWeights.Bold;

					rgCn.Rows.Add(rowtypCn);
					foreach (Consuntivo cons in lc)
						{
						TableRow row = new TableRow();
						row.Background = Brushes.Transparent;
						row.FontSize = textSize;
						row.FontWeight = FontWeights.Normal;
						foreach (string str in cons.Valori())
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
					}
				}
#endif
			#endregion


			return doc;
			}
		Section CreateSectionWithPageBreak(bool pageBreak = false)
			{
			Section section = new Section();
			section.BreakPageBefore = pageBreak;
			return section;
			}
		void PrintTesto(ref Section sez, string testo)
			{
			Paragraph pr = new Paragraph(new Run(testo));
			sez.Blocks.Add(pr);
			}
		void PrintTitoliTabella(ref Table table, ref TableRowGroup rowGroup, Riga empty)
			{
			int ncol = empty.Count();							// Colonne della tabella
			foreach (TipoColonna str in empty.Tipi())
				{
				TableColumn tc = new TableColumn();
				tc.Width = new GridLength(str.larghezzaColonna, GridUnitType.Pixel);
				table.Columns.Add(tc);
				}
			TableRow rowint = new TableRow();					// Riga di intestazione
			rowint.Background = Brushes.Transparent;
			rowint.FontSize = headerSize;
			rowint.FontWeight = FontWeights.Bold;
			foreach (string str in empty.Titoli())
				{
				TableCell tc = new TableCell(new Paragraph(new Run(str)));
				tc.BorderBrush = Brushes.Black;
				tc.BorderThickness = new Thickness(0, 1, 0, 1);
				rowint.Cells.Add(tc);
				}
			rowGroup.Rows.Add(rowint);
			}
		void PrintRigaTabella(ref Table table, ref TableRowGroup rowGroup, Riga dati)
			{
			TableRow row = new TableRow();
			row.Background = Brushes.Transparent;
			row.FontSize = textSize;
			row.FontWeight = FontWeights.Normal;
			foreach (string str in dati.Valori())
				{
				TableCell tc = new TableCell(new Paragraph(new Run(str)));
				tc.BorderBrush = Brushes.Black;
				tc.BorderThickness = new Thickness(0, 0, 0, 0.5);
				row.Cells.Add(tc);
				}
			rowGroup.Rows.Add(row);
			}
		#endregion
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
			dgOperazioni.CommitEdit();
			// dgOperazioni.Items.Refresh();
			dgOperazioni.InvalidateVisual();
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
			txtErrori.Text = ViewErrors();
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
		private void CorreggiOpCons_Click(object sender, RoutedEventArgs e)
			{
			int n = operazioni.CorreggiOperazioniSenzaConsuntivo();
			if(n > 0)
				MessageBox.Show("Corrette "+n.ToString()+" operazioni non impostate come consuntivo");
			dgOperazioni.CommitEdit();
			dgOperazioni.CancelEdit();
			dgOperazioni.Items.Refresh();
			dgConsuntivi.Items.Refresh();
			}
		private void About_Click(object sender, RoutedEventArgs e)
			{
			StringBuilder strb = new StringBuilder();
			strb.Append(Properties.Resources.Programma);
			strb.Append("\nby " + Properties.Resources.Autore);
			strb.Append("\nVersione " + Properties.Resources.Versione + " (" + Properties.Resources.Data+").");
			strb.Append("\nWebsite: " + Properties.Resources.SitoWeb);
			strb.Append("\nBlog (and program feedback): " + Properties.Resources.Blog);
			MessageBox.Show(strb.ToString());
			}
		private void button_test_Click(object sender, RoutedEventArgs e)
			{
			
			Encryption enc = new Encryption();
			//enc.Salt = "12345678";
			enc.Salt = "ABCxc@";
			string passwd = "ant@ni20";

			enc.ClearErrors();

			StringBuilder strb = new StringBuilder();
			strb.Append("Salt=<<<"+enc.Salt+">>>\n");
			foreach(Operazione op in operazioni.operazioni)
				{
				string crittografato = enc.Encrypt(op.ToString(), passwd);
				string decriptato = enc.Decrypt(crittografato, passwd);
				bool ok = String.Equals(op.ToString(), decriptato);
				strb.Append(decriptato + ":" + (ok ? "\t->OK" : "\t *** ERRORE ***") + '\n');
				}
			MessageBox.Show(strb.ToString());
			MessageBox.Show("ERRORS:\n" + enc.Errors());
								
				
			}
		private void Preferenze_Click(object sender, RoutedEventArgs e)
			{
			PreferenzeWindow pfw = new PreferenzeWindow(ref operazioni);
			bool? ret = pfw.ShowDialog();
			}
		}
    }
