   M S B A M L   `   `   `  ���� E  AConsuntivo, Version=1.0.0.2, Culture=neutral, PublicKeyToken=nullclr-namespace:WPF02WPF02      WPF02.FilterWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     %localclr-namespace:WPF02       � Title$  FilterWindow��$��291.894��$��527.34��.���  � ColumnDefinitions �� $	��27* ��� $
��139* �.+��� -    ��Name$ label��$
��Nota��$
��Left=�$��	10,10,0,0q�$	��Top=�$��85��$��26���� -   $ 
label_Copy��$
��Data��$
��Left=�$��	10,41,0,0q�$	��Top=�$��85��$��26���� -   $ label_Copy1��$��Descrizione��$
��Left=�$��	10,72,0,0q�$	��Top=�$��85��$��26���� -   $ label_Copy2��$��Importo��$
��Left=�$��
10,103,0,0q�$	��Top=�$��85��$��26���� -   $ label_Copy3��$��
Consuntivo��$
��Left=�$��
10,134,0,0q�$	��Top=�$��85��$��26���� -   $ label_Copy4��$��
Verificato��$
��Left=�$��
10,165,0,0q�$	��Top=�$��85��$��26���� -   $ label_Copy5��$
��Tipo��$
��Left=�$��
10,196,0,0q�$	��Top=�$��85��$��26���� -   $ label_Copy6��$	��da:��$
��Left=�$��	10,39,0,0q�$	��Top=�$��26��$��1��$��26��(  "System.Windows.Controls.DatePicker  -	     Name$ dtFrom��$
��Left=�$��	41,40,0,0q�$	��Top=�$��1��$��24��$	��144��  -
   $
 dtTo��$
��Left=�$��
240,41,0,0q�$	��Top=�$��1��$��24��$	��144���� -   $ label_Copy7��$��a:��$
��Left=�$��
209,40,0,0q�$	��Top=�$��26��$��1��$��26���� -   $ label_Copy8��$	��da:��$
��Left=�$��
10,102,0,0q�$	��Top=�$��26��$��1��$��26���� -   $ label_Copy9��$��a:��$
��Left=�$��209,103,0,0q�$	��Top=�$��26��$��1��$��26���� -    ��Name$ tbNota��$��1��$
��Left=�$��23��$��	41,10,0,0q� �� TextWrapping$
 Wrap=�$��TextBox��$	��Top=�$	��343���� -   $ tbDescrizione��$��1��$
��Left=�$��23��$��	41,72,0,0q�$
 Wrap=�$��TextBox��$	��Top=�$	��343���� -   $ tbImportoMin��$��1��$
��Left=�$��23��$��
41,107,0,0q�$
 Wrap=�$��TextBox��$	��Top=�$	��144���� -   $ tbImportoMax��$��1��$
��Left=�$��23��$��240,107,0,0q�$
 Wrap=�$��TextBox��$	��Top=�$	��144���� -    ��Name$ chkConsuntivo��$��1��$
��Left=�$��
41,140,0,0q�$	��Top=�$��71�� d� IsThreeState . �� -   $ chkVerificato��$�� ��$��1��$
��Left=�$��
41,171,0,0q�$	��Top=�$��71�� . �� -    ��Name$ cbTipo��$��1��$
��Left=�$��
41,200,0,0q�$	��Top=�$��52���� -   $ cbCondizione��$��1��$
��Left=�$��209,164,0,0q�$	��Top=�$	��175���� -   	 ��Name$
	 btOK��$��OK��$��1��$
��Left=�$��304,200,0,0q�$	��Top=�$��80���� -   $	 btReset��$��Reset��$��1��$
��Left=�$��219,200,0,0q�$	��Top=�$��80���� -   $	 	btAnnulla��$��Annulla��$��1��$
��Left=�$��134,200,0,0q�$	��Top=�$��80��