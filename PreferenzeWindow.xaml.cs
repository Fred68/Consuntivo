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
// using System.Windows.Shapes;
using System.IO;					// Path

namespace WPF02
	{
	/// <summary>
	/// Logica di interazione per PreferenzeWindow.xaml
	/// </summary>
	public partial class PreferenzeWindow : Window
		{
		Operazioni operazioni;

		public PreferenzeWindow(ref Operazioni operazioni)
			{
			InitializeComponent();
			this.operazioni = operazioni;
			SetDialogFromOperazioni();
			}
		private void button_Preferenze_Click(object sender, RoutedEventArgs e)
			{
			SetPreferencesFromDialog();
			}
		private void button_Annulla_Click(object sender, RoutedEventArgs e)
			{
			this.DialogResult = false;
			Close();
			}
		private void button_OK_Click(object sender, RoutedEventArgs e)
			{
			SetOperazioniFromDialog();
			this.DialogResult = true;
			Close();
#warning METTERE CONTROLLO, SE password, salt nulli e crittografia abilitata: Segnalare e annullare il comando	
			}
		private void SetDialogFromOperazioni()
			{
			tbNome1.Text = operazioni.Filename;
			tbNome1.IsReadOnly = true;
			tbNome1.Background = Brushes.LightGray;
			chkAttivo1.IsChecked = true;
			chkAttivo1.IsEnabled = false;
			tbNome2.Text = operazioni.NomeSecondario;
			chkAttivo2.IsChecked = operazioni.AttivoSecondario;
			chkCripto1.IsChecked = operazioni.Cripto1;
			chkCripto2.IsChecked = operazioni.Cripto2;
			tbPwd1.Text = operazioni.Passphrase1;
			tbPwd2.Text = operazioni.Passphrase2;
			tbSalt.Text = operazioni.Salt;
			chkMempwd1.IsChecked = operazioni.StorePassphrase1;
			chkMempwd2.IsChecked = operazioni.StorePassphrase2;
			}
		private void SetOperazioniFromDialog()
			{
			if (chkAttivo2.IsChecked.HasValue)	operazioni.AttivoSecondario = ((bool)chkAttivo2.IsChecked) ? true : false;
			operazioni.NomeSecondario = tbNome2.Text;
			if (chkCripto1.IsChecked.HasValue)	operazioni.Cripto1= ((bool)chkCripto1.IsChecked) ? true : false;
			if (chkCripto2.IsChecked.HasValue)	operazioni.Cripto2 = ((bool)chkCripto2.IsChecked) ? true : false;
			if (chkMempwd1.IsChecked.HasValue) operazioni.StorePassphrase1 = ((bool)chkMempwd1.IsChecked) ? true : false;
			if (chkMempwd2.IsChecked.HasValue) operazioni.StorePassphrase2 = ((bool)chkMempwd2.IsChecked) ? true : false;
			operazioni.Passphrase1 = tbPwd1.Text;
			operazioni.Passphrase2 = tbPwd2.Text;
			operazioni.Salt = tbSalt.Text;
			CheckDialogContent();
			}
		private void SetPreferencesFromDialog()
			{
			if (chkAttivo2.IsChecked.HasValue)	Properties.Settings.Default.SecondSaveActive = ((bool)chkAttivo2.IsChecked) ? true : false;
			Properties.Settings.Default.SecondSaveFilename = Path.GetFileName(tbNome2.Text);
			Properties.Settings.Default.SecondSavePath = Path.GetDirectoryName(tbNome2.Text);
			if (chkCripto1.IsChecked.HasValue)	Properties.Settings.Default.FirstSaveEncryptActive = ((bool)chkCripto1.IsChecked) ? true : false;
			if (chkCripto2.IsChecked.HasValue)	Properties.Settings.Default.SecondSaveEncryptActive = ((bool)chkCripto2.IsChecked) ? true : false;
			if (chkMempwd1.IsChecked.HasValue)	Properties.Settings.Default.FirstSaveStorePassphrase = ((bool)chkMempwd1.IsChecked) ? true : false;
			if (chkMempwd2.IsChecked.HasValue)	Properties.Settings.Default.SecondSaveStorePassphrase = ((bool)chkMempwd2.IsChecked) ? true : false;
			if (Properties.Settings.Default.FirstSaveStorePassphrase)	Properties.Settings.Default.FirstSavePassphrase = tbPwd1.Text;
			if (Properties.Settings.Default.SecondSaveStorePassphrase)	Properties.Settings.Default.SecondSavePassphrase = tbPwd2.Text;
			Properties.Settings.Default.Salt = tbSalt.Text;
			if (CheckDialogContent())
				Properties.Settings.Default.Save();
			else
				MessageBox.Show("Valori mancanti o errati: le preferenze non sono state memorizzate.");
			}
		private bool CheckDialogContent()
			{
			bool ok = true;
			if (chkAttivo2.IsChecked.HasValue)      // Save secondario attivo e nome secondario vuoto
				if ((bool)(chkAttivo2.IsChecked) && (tbNome2.Text.Length < 1))
					{
					ok = false;
					MessageBox.Show("Manca il nome del file del salvataggio secondario.");
					}
			if (chkCripto1.IsChecked.HasValue)      // Criptatura attiva ma password nulla
				if (((bool)chkCripto1.IsChecked) && (tbPwd1.Text.Length < 1))
					{
					ok = false;
					MessageBox.Show("La password del salvataggio principale è nulla.");
					}
			if (chkCripto2.IsChecked.HasValue)
				if (((bool)chkCripto2.IsChecked) && (tbPwd2.Text.Length < 1))
					{
					ok = false;
					MessageBox.Show("La password del salvataggio secondario è nulla.");
					}
			if (tbSalt.Text.Length < 1)             // Salt nullo
				{
				ok = false;
				MessageBox.Show("Il 'salt' per generare le password è nullo.");
				}
			return ok;
			}
		private void tbNome2_GotFocus(object sender, RoutedEventArgs e)
			{
			Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
			dlg.DefaultExt = ".txt";
			dlg.Filter = "Text documents (.txt)|*.txt";
			Nullable<bool> rt = dlg.ShowDialog();
			if (rt == true)
				{
				bool save = true;
				if(File.Exists(dlg.FileName))
					{
					if (MessageBox.Show("Il file esiste già ! Sovrascrivere ?", "File esistente", MessageBoxButton.YesNo) == MessageBoxResult.No)
						save = false;
					}
				if (save)
					{
					try
						{
						FileStream fs = File.Create(dlg.FileName);
						if(!fs.CanWrite)
							{
							MessageBox.Show("Impossibile scrivere sul file specificato");
							save = false;
							}
						fs.Close();
						}
					catch(Exception ex)
						{
						MessageBox.Show(ex.Message);
						save = false;
						}
					}
				if(save)
					tbNome2.Text = dlg.FileName;
				}
			Keyboard.ClearFocus();
			}
		}
	}
