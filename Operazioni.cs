
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;       // Per Observable collection
using System.Collections.Specialized;
using System.Windows.Controls;              // Per DataGrid
using System.IO;                            // Per operazioni su stream
using System.Linq;							// Per Collection<>.OrderBy


namespace WPF02
	{
	public delegate void StatusChangedHandler();
	public class Operazioni
		{
		static string intestazioneFile = "@INTESTAZIONE";
		enum readStat { Indefinito, Operazioni, Conti, OpStandard, Intestazione };
		enum checkType { Conti_Unique, OpStandard_Unique, Op_ContiValid, Op_OpStdValid, OpStd_ContiValid, Op_Consuntivo_old };

		ObservableCollection<Operazione> opVisibili						// Operazioni visibili
			{ get; set; }			
		ObservableCollection<Operazione> opTotali						// Tutte le operazioni
			{ get; set; }
		ObservableCollection<Conto> cntTotali							// Tutti i conti
			{ get; set; }
		ObservableCollection<OpStandard> opStdTotali					// Tutte le operazioni standard
			{ get; set; }

		Encryption encriptor;

		Dictionary<int, List<Consuntivo>> dicConsuntivi					// Tutti i consuntivi
			{ get; set; }
		List<Consuntivo> consTotali										// Solo i consuntivi del conto scelto
			{ get; set; }
		List<Consuntivo> empty;

		bool comboContiUpdated = false;

		StatusChangedHandler statHandler = null; 
		bool bStatusOk = true;

		string filename;												// Per operazioni su file
		bool readwriteCripted;
		string readwritePassword;

		List<string> lstLog;
		List<string> lstErr;

		public Filtro filtro;

		#region PROPRIETA
		public bool LogAbilitato { get; set; }
		public ObservableCollection<Operazione> operazioni
			{
			get { return opVisibili; }
			set { opVisibili = value; }
			}
		public ObservableCollection<Conto> conti
			{
			get { return cntTotali; }
			set { cntTotali = value; }
			}
		public ObservableCollection<OpStandard> opStandard
			{
			get { return opStdTotali; }
			set { opStdTotali = value; }
			}
		public bool status
			{
			get { return bStatusOk; }
			set
				{
				bStatusOk = value;
				if (statHandler != null)
					statHandler();
				}
			}
		public StatusChangedHandler azioneStatus
			{
			get { return statHandler; }
			set { statHandler = value; }
			}
		public string Filename
			{
			get { return filename; }
			set { filename = value; }
			}
		public bool contiUpdated
			{
			get { return comboContiUpdated; }
			set { comboContiUpdated = value; }
			}
		public string NomeSecondario {get; set;}
		public bool AttivoSecondario { get; set; }
		public bool Cripto1 { get; set; }
		public bool Cripto2 { get; set; }
		public string Passphrase1 { get; set; }
		public string Passphrase2 { get; set; }
		public string Salt { get; set; }
		public bool StorePassphrase1 { get; set; }
		public bool StorePassphrase2 { get; set; }
		#endregion

		void opChanged(object sender, NotifyCollectionChangedEventArgs e)
			{
			if(opVisibiliChangedEventEnabled)
				{
				StringBuilder strb = new StringBuilder();
				if (e.NewItems != null)
					{
					if (LogAbilitato)
						strb.Append("Aggiunti " + e.NewItems.Count + " elementi");
					foreach (Operazione op in e.NewItems)
						{
						opTotali.Add(op);
						}
					}
				if (e.OldItems != null)
					{
					if (LogAbilitato)
						strb.Append("Rimossi " + e.OldItems.Count + " elementi");
					foreach (Operazione op in e.OldItems)
						{
						opTotali.Remove(op);
						}
					}
				if (LogAbilitato)
					lstLog.Add(strb.ToString());
				}
			}
		void cntChanged(object sender, NotifyCollectionChangedEventArgs e)
			{
			comboContiUpdated = false;
			}
		void opsChanged(object sender, NotifyCollectionChangedEventArgs e)
			{
			#warning DA SCRIVERE...
			}
		bool opVisibiliChangedEventEnabled;

		public Operazioni()
			{
			LeggiConfigurazione();

			opTotali = new ObservableCollection<Operazione>();
			opVisibili = new ObservableCollection<Operazione>();
			opVisibiliChangedEventEnabled = true;
			opVisibili.CollectionChanged += opChanged;
			cntTotali = new ObservableCollection<Conto>();
			cntTotali.CollectionChanged += cntChanged;
			opStdTotali = new ObservableCollection<OpStandard>();
			opStdTotali.CollectionChanged += opsChanged;

			empty = new List<Consuntivo>();

			dicConsuntivi = new Dictionary<int, List<Consuntivo>>();
			dicConsuntivi.Add(0, empty);
			consTotali = dicConsuntivi[0];

			filename = "";
			readwriteCripted = false;
			readwritePassword = "";
			lstLog = new List<string>();
			lstErr = new List<string>();
			LogAbilitato = false;

			filtro = new Filtro();
			encriptor = new Encryption();
			
			}
		/// <summary>
		/// Legge i parametri di configurazione dalle impostazioni utente del programma
		/// </summary>
		public void LeggiConfigurazione()
			{
			NomeSecondario = Properties.Settings.Default.SecondSavePath + Path.DirectorySeparatorChar + Properties.Settings.Default.SecondSaveFilename;
			AttivoSecondario = Properties.Settings.Default.SecondSaveActive;
			Cripto1 = Properties.Settings.Default.FirstSaveEncryptActive;
			Cripto2 = Properties.Settings.Default.SecondSaveEncryptActive;
			Passphrase1 = Properties.Settings.Default.FirstSavePassphrase;
			Passphrase2 = Properties.Settings.Default.SecondSavePassphrase;
			Salt = Properties.Settings.Default.Salt;
			StorePassphrase1 = Properties.Settings.Default.FirstSaveStorePassphrase;
			StorePassphrase2 = Properties.Settings.Default.SecondSaveStorePassphrase;
			}
		/// <summary>
		/// Scrive i parametri di configurazione nelle impostazioni utente del programma
		/// </summary>
		public void ScriviConfigurazione()
			{
			Properties.Settings.Default.SecondSavePath = Path.GetDirectoryName(NomeSecondario);
			Properties.Settings.Default.SecondSaveFilename = Path.GetFileName(NomeSecondario);
			Properties.Settings.Default.SecondSaveActive = AttivoSecondario;
			Properties.Settings.Default.FirstSaveEncryptActive = Cripto1;
			Properties.Settings.Default.SecondSaveEncryptActive = Cripto2;
			Properties.Settings.Default.FirstSavePassphrase = Passphrase1;
			Properties.Settings.Default.SecondSavePassphrase = Passphrase2;
			Properties.Settings.Default.Salt = Salt;
			Properties.Settings.Default.FirstSaveStorePassphrase = StorePassphrase1;
			Properties.Settings.Default.SecondSaveStorePassphrase = StorePassphrase2;
			Properties.Settings.Default.Save();
			}
		/// <summary>
		/// Aggiorna le griglie dati con le collezioni contenute
		/// </summary>
		/// <param name="dgOperazioni"></param>
		/// <param name="dgConti"></param>
		/// <param name="dgOpStandard"></param>
		/// <param name="dgConsuntivi"></param>
		public void setDatiGrid(DataGrid dgOperazioni, DataGrid dgConti, DataGrid dgOpStandard, DataGrid dgConsuntivi)
			{
			dgOperazioni.AutoGenerateColumns = true;
			dgOperazioni.ItemsSource = this.opVisibili;
			dgConti.AutoGenerateColumns = true;
			dgConti.ItemsSource = this.cntTotali;
			dgOpStandard.AutoGenerateColumns = true;
			dgOpStandard.ItemsSource = this.opStdTotali;
			dgConsuntivi.AutoGenerateColumns = true;
			dgConsuntivi.ItemsSource = this.consTotali;
			dgConsuntivi.ItemsSource = this.consTotali;
			dgConsuntivi.IsReadOnly = true;
			}
		/// <summary>
		/// Aggiorna la griglie dei consuntivi con quello con il numero selezionato
		/// </summary>
		/// <param name="dgConsuntivi"></param>
		/// <param name="nc"></param>
		public void setConsuntivoGrid(DataGrid dgConsuntivi, int nc)
			{
			if (dicConsuntivi.ContainsKey(nc))
				this.consTotali = dicConsuntivi[nc];
			else
				this.consTotali = empty;
			dgConsuntivi.AutoGenerateColumns = true;
			dgConsuntivi.ItemsSource = this.consTotali;
			dgConsuntivi.IsReadOnly = true;
			dgConsuntivi.Items.Refresh();
			}
		/// <summary>
		/// Restituisce una stringa con la lista dei messaggi
		/// </summary>
		/// <returns></returns>
		public string MsgList()
			{
			StringBuilder strb = new StringBuilder();
			strb.Append("Azioni eseguite:\n");
			foreach (string str in lstLog)
				{
				strb.Append(str + '\n');
				}
			return strb.ToString();
			}
		/// <summary>
		/// Enumeratore dei messaggi
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> Messaggi()
			{
			foreach (string str in lstLog)
				yield return str;
			yield break;
			}
		/// <summary>
		/// Enumeratore degli errori
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> Errori()
			{
			foreach (string str in lstErr)
				yield return str;
			yield break;
			}
		/// <summary>
		/// Numero degli errori
		/// </summary>
		/// <returns></returns>
		public int ErroriCount()
			{
			return lstErr.Count;
			}
		/// <summary>
		/// Enumeratore delle liste dei consuntivi
		/// </summary>
		/// <returns></returns>
		public IEnumerable<List<Consuntivo>> ListeConsuntivi()
			{
			foreach (List<Consuntivo> lc in dicConsuntivi.Values)
				yield return lc;
			yield break;
			}
		/// <summary>
		/// Enumeratore delle operazioni standard
		/// </summary>
		/// <returns></returns>
		public IEnumerable<OpStandard> OpStandard()
			{
			foreach (OpStandard ops in opStandard)
				yield return ops;
			yield break;
			}
		/// <summary>
		/// Enumeratore dei conti
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Conto> Conti()
			{
			foreach (Conto cnt in cntTotali)
				yield return cnt;
			yield break;
			}
		/// <summary>
		/// Calcella la lista dei log
		/// </summary>
		public void CancellaLog()
			{
			lstLog.Clear();
			}
		/// <summary>
		/// Restituisce una stringa con le operazioni
		/// </summary>
		/// <returns></returns>
		public string ListaOperazioni()
			{
			StringBuilder strb = new StringBuilder();
			int n = opTotali.Count;
			if (n > 0)
				{
				strb.Append("OPERAZIONI[" + n.ToString() + "]\n");
				}
			else
				strb.Append("NESSUNA OPERAZIONE\n");
			foreach (Operazione op in opTotali)
				{
				strb.Append(op.ToString() + Riga.Separatore.line);
				}
			return strb.ToString();
			}
		/// <summary>
		/// Restituisce una stringa con i conti
		/// </summary>
		/// <returns></returns>
		public string ListaConti()
			{
			StringBuilder strb = new StringBuilder();
			int n = cntTotali.Count;
			if (n > 0)
				{
				strb.Append("CONTI[" + n.ToString() + "]\n");
				}
			else
				strb.Append("NESSUN CONTO\n");
			foreach (Conto cn in cntTotali)
				{
				strb.Append(cn.ToString() + Riga.Separatore.line);
				}
			return strb.ToString();
			}
		/// <summary>
		/// Restituisce una stringa con le operazioni standard
		/// </summary>
		/// <returns></returns>
		public string ListaOpStandard()
			{
			StringBuilder strb = new StringBuilder();
			int n = opStdTotali.Count;
			if (n > 0)
				{
				strb.Append("OPERAZIONI STANDARD[" + n.ToString() + "]\n");
				}
			else
				strb.Append("NESSUNA OPERAZIONE STANDARD\n");
			foreach (OpStandard ops in opStdTotali)
				{
				strb.Append(ops.ToString() + Riga.Separatore.line);
				}
			return strb.ToString();
			}
		/// <summary>
		/// Restituisce una stringa con i consuntivi attualmente elaborati
		/// </summary>
		/// <returns></returns>
		public string ListaConsuntivi()
			{
			StringBuilder strb = new StringBuilder();
			int n = consTotali.Count;
			if (n > 0)
				{
				strb.Append("CONSUNTIVI [" + n.ToString() + "]\n");
				}
			else
				strb.Append("NESSUN CONSUNTIVO\n");
			foreach (Consuntivo cns in consTotali)
				{
				strb.Append(cns.ToString() + Riga.Separatore.line);
				}
			return strb.ToString();
			}
		/// <summary>
		/// Inserisce una operazione alla riga indicata
		/// </summary>
		/// <param name="indx"></param>
		/// <returns></returns>
		public bool OpInserisciAt(int indx)
			{
			bool ok = false;
			Operazione op = new Operazione();
			if ((indx >= 0) && (indx < opVisibili.Count))
				{
				opVisibili.Insert(indx, op);
				ok = true;
				}
			return ok;
			}
		/// <summary>
		/// Inserisce una operazione prima dell'operazione indicata
		/// </summary>
		/// <param name="opi"></param>
		/// <returns></returns>
		public bool OpInserisciAt(Operazione opi)
			{
			bool ok = false;
			Operazione op = new Operazione();
			int indx = opVisibili.IndexOf(opi);
			if(indx!=-1)
				{
				opVisibili.Insert(indx, op);
				ok = true;
				}
			return ok;
			}
		/// <summary>
		/// Salva il file
		/// </summary>
		/// <param name="fullfilename">Nome completo di path</param>
		/// <param name="primario">Se false, usa il nome secondario</param>
		/// <returns></returns>
		public bool Save(string fullfilename, bool primario = true)
			{
			bool ret = true;
			readwriteCripted = false;
			if(primario)
				{
				if(Cripto1)
					{
					readwriteCripted = true;
					readwritePassword = Passphrase1;
					encriptor.ClearErrors();
					encriptor.Salt = Salt;
					}
				}
			else
				{
				fullfilename = this.NomeSecondario;
				if(Cripto2)
					{
					readwriteCripted = true;
					readwritePassword = Passphrase2;
					encriptor.ClearErrors();
					encriptor.Salt = Salt;
					}
				}
			try
				{
				using (StreamWriter sw = new StreamWriter(fullfilename, false, Encoding.UTF8))
					{
					WriteLine(sw, this.Intestazione());
					WriteLine(sw, new Operazione().Intestazione());
					foreach (Operazione op in opTotali)
						{
						WriteLine(sw, op.ToString());
						}
					WriteLine(sw, new Conto().Intestazione());
					foreach (Conto op in cntTotali)
						{
						WriteLine(sw, op.ToString());
						}
					WriteLine(sw, new OpStandard().Intestazione());
					foreach (OpStandard op in opStdTotali)
						{
						WriteLine(sw, op.ToString());
						}
					this.filename = fullfilename;
					}
				}
			catch (Exception ex)
				{
				if(LogAbilitato)
					lstLog.Add("Errore salvataggio file:\n"+ex.ToString());
				ret = false;
				}
			return ret;
			}
		/// <summary>
		/// Azzera i dati in memoria
		/// </summary>
		public void New()
			{
			opTotali.Clear();
			opVisibili.Clear();
			cntTotali.Clear();
			opStdTotali.Clear();
			filename = "";
			status = true;
			}
		/// <summary>
		/// Apre il file indicato
		/// </summary>
		/// <param name="fullfilename">Nome completo di path</param>
		/// <returns></returns>
		public bool Open(string fullfilename)
			{
			bool ret = true;
			readwriteCripted = false;
			readwritePassword = "";
			readStat rstat = readStat.Indefinito;
			status = true;
			try
				{
				using (StreamReader sr = new StreamReader(fullfilename, Encoding.UTF8))
					{
					if (ReadFirstLineAndSetCripted(sr, ref rstat, ref readwritePassword))
						{
						while (sr.Peek() >= 0)
							{
							string rline = ReadLine(sr);
							try
								{
								if (rline == new Operazione().Intestazione())
									rstat = readStat.Operazioni;
								else if (rline == new Conto().Intestazione())
									rstat = readStat.Conti;
								else if (rline == new OpStandard().Intestazione())
									rstat = readStat.OpStandard;
								//else if (rline == Operazioni.intestazioneFile)			// ELIMINARE
								//	rstat = readStat.Intestazione;						// ELIMINARE
								else
									{
									switch (rstat)
										{
										case readStat.Operazioni:
												{
												Operazione op = new Operazione();
												if (op.FromString(rline))
													{
													opVisibili.Add(op);
													}
												else
													{
													throw new Exception("Errore analisi linea: " + rline, new Exception("Errore analisi linea"));
													}
												}
											break;
										case readStat.Conti:
												{
												Conto op = new Conto();
												if (op.FromString(rline))
													{
													cntTotali.Add(op);
													}
												else
													{
													throw new Exception("Errore analisi linea: " + rline, new Exception("Errore analisi linea"));
													}
												}
											break;
										case readStat.OpStandard:
												{
												OpStandard op = new OpStandard();
												if (op.FromString(rline))
													{
													opStdTotali.Add(op);
													}
												else
													{
													throw new Exception("Errore analisi linea: " + rline, new Exception("Errore analisi linea"));
													}
												}
											break;
										case readStat.Intestazione:
											break;
										case readStat.Indefinito:
											throw new Exception("Errore sintattico nel file" + rline, new Exception("Errore sintattico nel file"));
										}
									}
								ret = true;
								}
							catch (Exception ex)
								{
								if (LogAbilitato)
									lstLog.Add("Errore in importazione dati:\n" + ex.ToString());
								ret = false;
								status = false;
								}
							}
						}
					}
				}
			catch (Exception ex)
				{
				if (LogAbilitato)
					lstLog.Add("Errore in lettura file:\n" + ex.ToString());
				ret = false;
				status = false;
				}
			if(ret)
				this.filename = fullfilename;
			return ret;
			}
		/// <summary>
		/// Restituisce la stringa di intestazione del file
		/// </summary>
		/// <returns></returns>
		public string Intestazione()
			{
			return Operazioni.intestazioneFile;
			}
		/// <summary>
		/// Legge una linea da file
		/// </summary>
		/// <param name="sr">Streamreader</param>
		/// <returns></returns>
		private string ReadLine(StreamReader sr)
			{
			string str = "";
			str = sr.ReadLine();
			if (readwriteCripted == true)
				str = encriptor.Decrypt(str, readwritePassword);
			return str;
			}
		/// <summary>
		/// Scrive la linea su file
		/// </summary>
		/// <param name="sw">Streamwriter</param>
		/// <param name="txt">linea da scrivere (senza newline finale)</param>
		private void WriteLine(StreamWriter sw, string txt)
			{
			if(readwriteCripted)
				{
				txt = encriptor.Encrypt(txt, readwritePassword);
				}
			sw.WriteLine(txt);
			}
		/// <summary>
		/// Legge la prima riga del file e determina se criptato e se con password principale o secondaria
		/// </summary>
		/// <param name="sr"></param>
		/// <param name="rstat"></param>
		/// <param name="password"></param>
		private bool ReadFirstLineAndSetCripted(StreamReader sr, ref readStat rstat, ref string password)
			{
			bool ok = false;
			password = "";
			readwriteCripted = false;
			string str;
			rstat = readStat.Indefinito;
			try
				{
				str = sr.ReadLine();									// Legge la prima riga
				if(str.Length > 0)
					{
					if(str.StartsWith(Operazioni.intestazioneFile))     // La verifica in chiaro
						{
						password = "";
						readwriteCripted = false;
						ok = true;
						throw new Exception("Ok");
						}
					else												// Prova con le password
						{
						string dec;
						encriptor.ClearErrors();
						encriptor.Salt = Salt;
						dec = encriptor.Decrypt(str, Passphrase1);
						if(dec.StartsWith(Operazioni.intestazioneFile))
							{
							password = Passphrase1;
							readwriteCripted = true;
							ok = true;
							throw new Exception("Ok");
							}
						encriptor.ClearErrors();
						dec = encriptor.Decrypt(str, Passphrase2);
						if (dec.StartsWith(Operazioni.intestazioneFile))
							{
							password = Passphrase2;
							readwriteCripted = true;
							ok = true;
							throw new Exception("Ok");
							}
						}
					}
				}
			catch(Exception ex)
				{
				if (ex.Message != "Ok")
					ok = false;
				else
					rstat = readStat.Intestazione;
				}
			return ok;
			}
		#region RICERCA e FILTRO
		/// <summary>
		/// Cerca OpStandard, null se non trovata
		/// </summary>
		/// <param name="numero"></param>
		/// <returns></returns>
		public OpStandard FindOpStandard(int numero)	
			{
			OpStandard found = null;
			foreach(OpStandard ops in opStdTotali)
				{
				if(ops.numero == numero)
					{
					found = ops;
					break;
					}
				}
			return found;
			}
		/// <summary>
		/// Cerca Conto , null se non trovato
		/// </summary>
		/// <param name="numero"></param>
		/// <returns></returns>
		public Conto FindConto(int numero)				
			{
			Conto found = null;
			foreach (Conto ops in cntTotali)
				{
				if (ops.numero == numero)
					{
					found = ops;
					break;
					}
				}
			return found;
			}
		/// <summary>
		/// Cerca la lista dei consuntivi per un numero di conto
		/// </summary>
		/// <param name="numero_conto"></param>
		/// <returns></returns>
		public List<Consuntivo> FindListaConsuntivi(int numero_conto)		
			{
			List<Consuntivo> found = null;
			if (dicConsuntivi.ContainsKey(numero_conto))
				found = dicConsuntivi[numero_conto];
			return found;
			}
		/// <summary>
		/// Applica il filtro alle operazioni visualizzate
		/// </summary>
		public void ApplicaFiltro()
			{
			opVisibiliChangedEventEnabled = false;
			opVisibili.Clear();
			foreach(Operazione op in opTotali)
				{
				if (filtro.Check(op))
					opVisibili.Add(op);
				}
			opVisibiliChangedEventEnabled = true;
			filtro.IsAttivo = true;
			}
		/// <summary>
		/// Rimuove il filtro dalle operazioni visualizzate
		/// </summary>
		public void CancellaFiltro()
			{
			opVisibiliChangedEventEnabled = false;
			opVisibili.Clear();
			foreach (Operazione op in opTotali)
				opVisibili.Add(op);
			opVisibiliChangedEventEnabled = true;
			filtro.IsAttivo = false;
			}
		#endregion
		#region CHECK e CALCOLO
		/// <summary>
		/// Verifica ed imposta lo stato
		/// </summary>
		/// <returns></returns>
		public bool Check()                         
			{
			bool ok = true;
			lstErr.Clear();
			List<bool> oki = new List<bool>();
			oki.Add(Check(checkType.Conti_Unique));
			oki.Add(Check(checkType.OpStandard_Unique));
			oki.Add(Check(checkType.OpStd_ContiValid));
			oki.Add(Check(checkType.Op_ContiValid));
			oki.Add(Check(checkType.Op_OpStdValid));
			oki.Add(Check(checkType.Op_Consuntivo_old));
			foreach (bool x in oki)
				ok = ok && x;
			this.status = ok;   // Usa la proprietà, per attivare l'handler
			return ok;
			}
		/// <summary>
		/// Esegue il controllo specifico del tipo scelto
		/// </summary>
		/// <param name="typ"></param>
		/// <returns></returns>
		bool Check(checkType typ)					
			{
			bool ok = false;
			switch(typ)
				{
				case checkType.Conti_Unique:
					ok = Riga.CheckUnique(this.conti);      // verifica unicità dei numeri dei conti 
					if (!ok) lstErr.Add("Conti duplicati.");
					break;
				case checkType.OpStandard_Unique:			// Verifica unicità dei numeri dei conti 
					ok = Riga.CheckUnique(this.opStandard);
					if (!ok) lstErr.Add("Operazioni standard duplicate.");
					break;
				case checkType.OpStd_ContiValid:			// Verifica che i conti delle opStandard esistano
					ok = true;
					foreach(OpStandard os in this.opStandard)
						{
						foreach(int nc in os.Conti())
							{
							bool found = false;
							foreach(Conto c in this.cntTotali)
								{
								if (c.numero == Math.Abs(nc))
									found = true;
								}
							if(!found)
								{
								ok = false;
								lstErr.Add("Operazione standard <" + os.numero + "> con numero di conto non valido.");
								}
							}
						}
					break;
				case checkType.Op_ContiValid:				// Verifica che i conti delle Operazioni esistano
					ok = true;
					foreach (Operazione op in this.opTotali)
						{
						foreach (int nc in op.Conti())
							{
							bool found = false;
							foreach (Conto c in this.cntTotali)
								{
								if (c.numero == Math.Abs(nc))
									found = true;
								}
							if (!found)
								{
								ok = false;
								lstErr.Add("Operazione <" + op.descrizione + "> del <" + op.data + "> con numero di conto non valido.");
								}
							}
						}
					break;
				case checkType.Op_OpStdValid:				// Verifica che le opStandard delle Operazioni esistano
					ok = true;
					foreach(Operazione op in this.opTotali)
						{
						bool found = false;
						if(op.tipostd == 0)
							{
							found = true;
							}
						else
							{
							foreach (OpStandard os in this.opStandard)
								{
								if (os.numero == op.tipostd)
									found = true;
								}
							}
						if (!found)
							{
							ok = false;
							lstErr.Add("Operazione <" + op.descrizione + "> del <" + op.data + "> con numero di operazione standard non valido.");
							}
						}
					break;
				case checkType.Op_Consuntivo_old:
					DateTime now = DateTime.Now;
					ok = true;
					foreach (Operazione op in this.opTotali)
						{
						DateTime dt = Riga.String2DateTime(op.data);
						if((now > dt)&&(!op.consuntivo))
							{
							ok = false;
							lstErr.Add("Operazione <" + op.descrizione + "> del <" + op.data + "> non impostata come consuntivo.");
							}
						}	
					break;
				default:
					throw new Exception("Caso CheckType non gestito.");
				}
			return ok;
			}
		/// <summary>
		/// Inserisce i conti delle OpStandard nelle Operazioni
		/// </summary>
		public void EspandeOpStandard()				
			{
			if (Check())
				{
				foreach (Operazione op in opTotali)
					{
					if (op.tipostd != 0)
						{
						OpStandard ops = FindOpStandard(op.tipostd);
						if (ops != null)
							{
							op.conti = ops.conti;
							}
						else
							{
							lstErr.Add("L'operazione " + op.descrizione + " del " + op.data + " contiene l'operazione standard " + op.tipostd.ToString() + " inesistente.");
							this.status = false;
							}
						}
					}
				}
			}
		/// <summary>
		/// Ordina le operazioni
		/// </summary>
		/// <returns></returns>
		public List<Operazione> OrdinaOperazioni()
			{
			List<Operazione> tmp = this.opTotali.ToList<Operazione>();
			List<Operazione> tmpOrd = new List<Operazione>(tmp.OrderBy(x => x));
			return tmpOrd;
			}
		/// <summary>
		/// Genera le liste dei consuntivi
		/// </summary>
		/// <returns></returns>
		public bool GeneraConsuntivi()
			{
			bool ok = Check();
			try
				{
				if (ok)
					{
					// Crea il dizionario dei conti
					dicConsuntivi.Clear();
					try
						{
						foreach (Conto conto in conti)
							{
							int nc = conto.numero;
							if (nc > 0)
								dicConsuntivi.Add(nc, new List<Consuntivo>());
							}
						}
					catch
						{
						ok = false;
						lstErr.Add("Errore nella creazione della lista dei consuntivi");
						this.status = false;
						throw new Exception("Fallita generazione consuntivi");
						}
					// Ordina le operazioni
					List<Operazione> opOrdinata = OrdinaOperazioni();
					// Percorre le operazioni e le ricopia nei rispettivi consuntivi
					try
						{
						foreach (Operazione op in opOrdinata)       // Percorre le operazioni
							{
							foreach (int nc in op.Conti())          // Percorre i conti dell'operazione
								{
								int nca = Math.Abs(nc);             // Estrae il conto
								int ncs = Math.Sign(nc);
								// Crea nuova voce di consuntivo, copiando i dati dell'operazione
								Consuntivo cns = new Consuntivo(Riga.String2DateTime(op.data), op.descrizione, (op.importo) * ncs, op.tipo, 0, op.consuntivo, op.verificato,false);
								dicConsuntivi[nca].Add(cns);            // E la aggiunge alla lista del conto rispettivo
								}
							}
						}
					catch
						{
						ok = false;
						lstErr.Add("Errore nella generazione dei consuntivi");
						this.status = false;
						throw new Exception("Fallita generazione consuntivi");
						}
					// Calcola i totali dei consuntivi, percorrendo, per ogni conto, la lista dei consuntivi
					// foreach(List<Consuntivo> lcons in dicConsuntivi.Values)
					foreach(int nc_key in dicConsuntivi.Keys)
						{
						List<Consuntivo> lcons = dicConsuntivi[nc_key];
						decimal totale = 0;
						decimal x;

						Consuntivo.AlertConto alert = Consuntivo.AlertConto.Disattivo;
						Conto tmp = FindConto(nc_key);
						if (tmp != null)
							alert = tmp.alert;

						foreach (Consuntivo cons in lcons)
							{
							x = cons.importo;
							switch(cons.tipo)
								{
								case Riga.Tipo.A:       // Azzera e reimposta al valore
									totale = x;
									break;
								case Riga.Tipo.P:       // Somma o sottrae
									totale += x;
									break;
								case Riga.Tipo.N:		// Non fa nulla
									break;
								}
							cons.totale = totale;

							switch(alert)
								{
								case Consuntivo.AlertConto.Negativo:
									if (cons.totale < 0)
										cons.err = true;
									break;
								case Consuntivo.AlertConto.Positivo:
									if (cons.totale > 0)
										cons.err = true;
									break;
								default:
									break;
								}
							}
						}
					}
				}
			catch(Exception e)
				{
				ok = false;
				lstErr.Add(e.Message);
				this.status = false;
				}
			return ok;
			}	
		/// <summary>
		/// Imposta le operazioni antecedenti alla data di sistema come consuntivi
		/// </summary>
		/// <returns></returns>
		public int CorreggiOperazioniSenzaConsuntivo()
			{
			int n = 0;
			DateTime now = DateTime.Now;
			foreach(Operazione op in this.opTotali)
				{
				DateTime dt = Riga.String2DateTime(op.data);
				if ((now > dt) && (!op.consuntivo))
					{
					op.consuntivo = true;
					n++;
					}
				}
			return n;
			}
		#endregion
		}
	}
