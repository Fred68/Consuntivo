
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;       // Per Observable collection
using System.Collections.Specialized;
using System.Windows.Controls;              // Per DataGrid
using System.IO;                            // Per operazioni su stream


namespace WPF02
	{
	public delegate void StatusChangedHandler();
	class Operazioni
		{
		ObservableCollection<Operazione> opVisibili { get; set; }
		ObservableCollection<Operazione> opTotali { get; set; }
		ObservableCollection<Conto> cntTotali { get; set; }
		ObservableCollection<OpStandard> opStdTotali { get; set; }
		ObservableCollection<Consuntivo> consTotali { get; set; }

		bool comboContiUpdated = false;
		public bool contiUpdated
			{
			get { return comboContiUpdated; }
			set { comboContiUpdated = value; }
			}

		StatusChangedHandler statHandler = null; 
		bool bStatusOk = true;

		string filename;
		enum readStat	{Indefinito, Operazioni, Conti, OpStandard};
		enum checkType	{Conti_Unique, OpStandard_Unique, Op_ContiValid, Op_OpStdValid, OpStd_ContiValid };

		public string Filename
			{
			get { return filename; }
			set { filename = value; }
			}

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
		public ObservableCollection<Consuntivo> consuntivi
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
			consTotali = new ObservableCollection<Consuntivo>();

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
			dgConsuntivi.AutoGenerateColumns = true;
			dgConsuntivi.ItemsSource = this.consTotali;
			dgConsuntivi.IsReadOnly = true;
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
		public bool Check()							// Verifica ed imposta lo stato
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
			this.status = ok;	// Usa la proprietà, per attivare l'handler
			return ok;
			}
		#region INTERNAL CHECK
		bool Check(checkType typ)
			{
			bool ok = false;
			switch(typ)
				{
				case checkType.Conti_Unique:
					ok = Riga.CheckUnique(this.conti);      // verifica unicità dei numeri dei conti 
					if (!ok) lstErr.Add("\nConti duplicati");
					break;
				case checkType.OpStandard_Unique:			// Verifica unicità dei numeri dei conti 
					ok = Riga.CheckUnique(this.opStandard);
					if (!ok) lstErr.Add("\nOperazioni standard duplicate");
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
								lstErr.Add("\nOperazione standard <" + os.numero + "> con numero di conto non valido");
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
								lstErr.Add("\nOperazione <" + op.descrizione + "> del <" + op.data + "> con numero di conto non valido");
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
							lstErr.Add("\nOperazione <" + op.descrizione + "> del <" + op.data + "> con numero di operazione standard non valido");
							}
						}
					break;
				}
			return ok;
			}
		#endregion
		}
	}
