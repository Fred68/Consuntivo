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
		Operazioni operazioni;

		public MainWindow()
            {
			this.Language = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag);
			InitializeComponent();
			operazioni = new Operazioni();
			UpdateTitle();
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
				dgOperazioni.Items.Refresh();
				dgConti.Items.Refresh();
				dgOpStandard.Items.Refresh();
				UpdateTitle();
				this.InvalidateVisual();
				// MessageBox.Show(operazioni.Filename + str1);
				}
			else
				MessageBox.Show(str2);
			}
		void UpdateTitle()
			{
			if(operazioni.Filename.Length>0)
				this.Title = "Consuntivo " + operazioni.Filename;
			else
				this.Title = "Consuntivo " + "<nessun file>";
			}
		FlowDocument FillFlowDocument(ref FlowDocument doc, Size pageSize)   // Prova
			{
			doc.ColumnWidth = doc.MaxPageWidth = pageSize.Width;
			doc.MaxPageHeight = pageSize.Height;
			
			// Stampa le Operazioni
			Operazione nouse = new Operazione();
			Table tOp = new Table();
			int ncol = nouse.Count();
			//for (int i = 0; i < ncol; i++)
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
			foreach (TipoColonna str in nouse.Tipi())
				{
				TableCell tc = new TableCell(new Paragraph(new Run(str.tipo)));
				tc.BorderBrush = Brushes.Black;
				tc.BorderThickness = new Thickness(0, 1, 0, 1);
				rowtyp.Cells.Add(tc);
				}
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
			foreach (TipoColonna str in nouseC.Tipi())
				{
				TableCell tc = new TableCell(new Paragraph(new Run(str.tipo)));
				tc.BorderBrush = Brushes.Black;
				tc.BorderThickness = new Thickness(0, 1, 0, 1);
				rowtypC.Cells.Add(tc);
				}
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
					//row.Cells.Add(new TableCell(new Paragraph(new Run(str))));
					}
				rgC.Rows.Add(row);
				}
			tCn.RowGroups.Add(rgC);

			doc.Blocks.Add(tCn);

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
						OpEditWindow oped = new OpEditWindow(ref op);
						oped.ShowDialog();
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


		private void dgOperazioni_Loaded(object sender, RoutedEventArgs e)
            {
			operazioni.setDatiGrid(this.dgOperazioni, this.dgConti, this.dgOpStandard);
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
			//operazioni.operazioni.Add(new Operazione());
			//operazioni.operazioni.Add(new Operazione("x", System.DateTime.Now, "pippo", 100, Operazione.Tipo.P, 0,"10"));
			//operazioni.operazioni.Add(new Operazione("y", new System.DateTime(2015,1,30) , "pluto", -20, Operazione.Tipo.P, 0,"-1,-10,90"));
			//operazioni.operazioni.Add(new Operazione("a", new System.DateTime(2010,7,2), "birthday", 1000, Operazione.Tipo.A, 0,""));
			//operazioni.operazioni.Add(new Operazione("", new System.DateTime(2014, 12, 1), "aaa", 1000, Operazione.Tipo.A, 0, "-9,5"));
			//operazioni.operazioni.Add(new Operazione("", new System.DateTime(2010, 8, 30), "bbb", 1000, Operazione.Tipo.A, 0, "-9,5"));
			//operazioni.conti.Add(new Conto(1, "Conto Fred"));
			//operazioni.conti.Add(new Conto(2, "Conto Veronica"));
			//operazioni.conti.Add(new Conto(10, "Conto Galia"));
			//operazioni.conti.Add(new Conto(20, "Spese casa"));
			//operazioni.opStandard.Add(new OpStandard(1, "Prelievo", "-10 -5 -9"));
			//operazioni.opStandard.Add(new OpStandard(2, "Versamento", "10 8 4"));
			}
		private void VediMsg_Click(object sender, RoutedEventArgs e)
			{
			MessageBox.Show(operazioni.MsgList());
			}
		private void Filtra_Click(object sender, RoutedEventArgs e)
			{

			}
		private void editSelected_Click(object sender, RoutedEventArgs e)
			{
			ModificaElementoSelezionato();
			dgOperazioni.Items.Refresh();
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
				printDlg.PrintDocument(idpSource.DocumentPaginator, "OperazioniComplesso");
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

		}
    }
