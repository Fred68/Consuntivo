
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
	class Operazioni
		{
		enum readStat { Indefinito, Operazioni, Conti, OpStandard };
		enum checkType { Conti_Unique, OpStandard_Unique, Op_ContiValid, Op_OpStdValid, OpStd_ContiValid };

		ObservableCollection<Operazione> opVisibili						// Operazioni visibili
			{ get; set; }			
		ObservableCollection<Operazione> opTotali						// Tutte le operazioni
			{ get; set; }
		ObservableCollection<Conto> cntTotali							// Tutti i conti
			{ get; set; }
		ObservableCollection<OpStandard> opStdTotali					// Tutte le operazioni standard
			{ get; set; }

		//Dictionary<int, ObservableCollection<Consuntivo>> dicConsuntivi	// Tutti i consuntivi
		//	{ get; set; }
		//ObservableCollection<Consuntivo> consTotali                     // Solo i consuntivi del conto scelto
		//	{ get; set; }
		//ObservableCollection<Consuntivo> empty;

		Dictionary<int, List<Consuntivo>> dicConsuntivi	// Tutti i consuntivi
			{ get; set; }
		List<Consuntivo> consTotali                     // Solo i consuntivi del conto scelto
			{ get; set; }
		List<Consuntivo> empty;


		bool comboContiUpdated = false;

		StatusChangedHandler statHandler = null; 
		bool bStatusOk = true;

		string filename;

		List<string> lstLog;
		List<string> lstErr;

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
		public List<Consuntivo> consuntivi
			{
			get { return consTotali; }
			set { consTotali = value; }
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
		#endregion

		void opChanged(object sender, NotifyCollectionChangedEventArgs e)
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
				lstLog.Add(strb.ToString()/*+"\t"+sender.ToString()*/);
			}
		void cntChanged(object sender, NotifyCollectionChangedEventArgs e)
			{
			comboContiUpdated = false;
			}
		void opsChanged(object sender, NotifyCollectionChangedEventArgs e)
			{
			#warning DA SCRIVERE...
			}

		public Operazioni()
			{
			opTotali = new ObservableCollection<Operazione>();
			opVisibili = new ObservableCollection<Operazione>();
			opVisibili.CollectionChanged += opChanged;
			cntTotali = new ObservableCollection<Conto>();
			cntTotali.CollectionChanged += cntChanged;
			opStdTotali = new ObservableCollection<OpStandard>();
			opStdTotali.CollectionChanged += opsChanged;

			empty = new List<Consuntivo>();

			dicConsuntivi = new Dictionary<int, List<Consuntivo>>();
			dicConsuntivi.Add(0, empty);
			consTotali = dicConsuntivi[0];
			// consTotali = new ObservableCollection<Consuntivo>();

			filename = "";
			lstLog = new List<string>();
			lstErr = new List<string>();
			LogAbilitato = false;
			}

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
		public IEnumerable<string> Messaggi()
			{
			foreach (string str in lstLog)
				yield return str;
			yield break;
			}
		public IEnumerable<string> Errori()
			{
			foreach (string str in lstErr)
				yield return str;
			yield break;
			}
		public int ErroriCount()
			{
			return lstErr.Count;
			}

		public void CancellaLog()
			{
			lstLog.Clear();
			}
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
		public bool Save(string fullfilename)
			{
			bool ret = true;
			try
				{
				using (StreamWriter sw = new StreamWriter(fullfilename, false, Encoding.UTF8))
					{
					sw.WriteLine(new Operazione().Intestazione());
					foreach (Operazione op in opTotali)
						{
						sw.WriteLine(op.ToString());
						}
					sw.WriteLine(new Conto().Intestazione());
					foreach (Conto op in cntTotali)
						{
						sw.WriteLine(op.ToString());
						}
					sw.WriteLine(new OpStandard().Intestazione());
					foreach (OpStandard op in opStdTotali)
						{
						sw.WriteLine(op.ToString());
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
		public void New()
			{
			opTotali.Clear();
			opVisibili.Clear();
			cntTotali.Clear();
			opStdTotali.Clear();
			filename = "";
			status = true;
			}
		public bool Open(string fullfilename)
			{
			bool ret = true;
			readStat rstat = readStat.Indefinito;
			status = true;
			try
				{
				using (StreamReader sr = new StreamReader(fullfilename, Encoding.UTF8))
					{
					while(sr.Peek() >= 0)
						{
						string rline = sr.ReadLine();
						try
							{
							if (rline == new Operazione().Intestazione())
								rstat = readStat.Operazioni;
							else if (rline == new Conto().Intestazione())
								rstat = readStat.Conti;
							else if (rline == new OpStandard().Intestazione())
								rstat = readStat.OpStandard;
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

		#region RICERCA
		public OpStandard FindOpStandard(int numero)	// Cerca OpStandard, null se non trovata
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
		public Conto FindConto(int numero)				// Cerca Conto , null se non trovato
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
		#endregion

		#region CHECK e CALCOLO
		public bool Check()                         // Verifica ed imposta lo stato
			{
			bool ok = true;
			lstErr.Clear();
			List<bool> oki = new List<bool>();
			oki.Add(Check(checkType.Conti_Unique));
			oki.Add(Check(checkType.OpStandard_Unique));
			oki.Add(Check(checkType.OpStd_ContiValid));
			oki.Add(Check(checkType.Op_ContiValid));
			oki.Add(Check(checkType.Op_OpStdValid));
			foreach (bool x in oki)
				ok = ok && x;
			this.status = ok;   // Usa la proprietà, per attivare l'handler
			return ok;
			}
		bool Check(checkType typ)					// Esegue il controllo specifico del tipo scelto
			{
			bool ok = false;
			switch(typ)
				{
				case checkType.Conti_Unique:
					ok = Riga.CheckUnique(this.conti);      // verifica unicità dei numeri dei conti 
					if (!ok) lstErr.Add("Conti duplicati");
					break;
				case checkType.OpStandard_Unique:			// Verifica unicità dei numeri dei conti 
					ok = Riga.CheckUnique(this.opStandard);
					if (!ok) lstErr.Add("Operazioni standard duplicate");
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
								lstErr.Add("Operazione standard <" + os.numero + "> con numero di conto non valido");
								}
							}
						}
					break;
				case checkType.Op_ContiValid:				// Verifica che i conti delle Operazioni esistano
					ok = true;
					foreach (Operazione op in this.operazioni)
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
								lstErr.Add("Operazione <" + op.descrizione + "> del <" + op.data + "> con numero di conto non valido");
								}
							}
						}
					break;
				case checkType.Op_OpStdValid:				// Verifica che le opStandard delle Operazioni esistano
					ok = true;
					foreach(Operazione op in this.operazioni)
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
							lstErr.Add("Operazione <" + op.descrizione + "> del <" + op.data + "> con numero di operazione standard non valido");
							}
						}
					break;
				}
			return ok;
			}
		public void EspandeOpStandard()				// Inserisce i conti delle OpStandard nelle Operazioni
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
		public List<Operazione> OrdinaOperazioni()
			{
			List<Operazione> tmp = this.opTotali.ToList<Operazione>();
			List<Operazione> tmpOrd = new List<Operazione>(tmp.OrderBy(x => x));
			return tmpOrd;
			}
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
						//for(int indx=0; indx < opOrdinata.Count; indx++)
							{
							//Operazione op = operazioni[indx];
							foreach (int nc in op.Conti())          // Percorre i conti dell'operazione
								{
								int nca = Math.Abs(nc);             // Estrae il conto
								int ncs = Math.Sign(nc);
								// Crea nuova voce di consuntivo, copiando i dati dell'operazione
								Consuntivo cns = new Consuntivo(Riga.String2DateTime(op.data), op.descrizione, (op.importo) * ncs, op.tipo, 0);
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
					// Calcola i totali dei consuntivi
					#warning DA SCRIVERE
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
		#endregion
		}
	}
