   M S B A M L   `   `   `  ���� E  AConsuntivo, Version=1.0.0.2, Culture=neutral, PublicKeyToken=nullclr-namespace:WPF02WPF02      WPF02.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     %localclr-namespace:WPF02     -     � Title$  
Consuntivo��$	��580��$
��1000��$	��100��.��� .+�o� -    o�Name$
 menu��$��1��$
��Left=�$��
10,0,10,10q�$	��Top=�.@�m� $
��File��.@�m� -   $	��New��m� -   $
��Open��m� -   $
��Save��m� -   $��
Save as...��m� -   $��Print...��m� -   $
��Quit��m� $
��Edit��.@�m� -	   $��	View data��m� -
   $��View log��m� -   $��
Enable log��m� -   $��Disable log��m� -   $��	Clear log��m� -   $��Preferenze...��m� $��Filtra��.@�m� -   $��Imposta filtro��m� $��Calcola��.@�m� -   $��	Controlla��m� -   $��Vedi errori��m� -   $*��$Correggi operazioni senza consuntivo��m� -   $��Genera consuntivi��m� $
��Help��.@�m� -   $��About...���� -    ��Name$ 
tabControl��$��10,30,10,10q�.@��� $��
Operazioni��.��� ���	#FFE5E5E5$��0,4,0,-4q�.+�&   System.Windows.Controls.DataGrid  -     Name$ dgOperazioni��$��10,52,10,10q�   AutoGenerateColumns . $��2q�$ �� IsSynchronizedWithCurrentItem$ FalseY��� -    ��Name$ pausa��$��Pausa��$
��Left=�$��24��$��	10,26,0,0q�$	��Top=�$��48���� -   $ vediProprieta��$��Vedi proprietà��$
��Left=�$��24��$��	63,26,0,0q�$	��Top=�$��89�� �� 	IsDefault . �� -   $ listaOperazioni��$��Lista Operazioni��$
��Left=�$��24��$��
157,26,0,0q�$	��Top=�$	��114�� . �� -   $ Filtra��$��Applica filtro��$
��Left=�$��24��$��
276,26,0,0q�$	��Top=�$��76�� . �� -   $ VediMsg��$��Vedi msg��$
��Left=�$��24��$��
356,26,0,0q�$	��Top=�$��56�� . �� -   $ editSelected��$��Modifica��$
��Left=�$��24��$��
416,26,0,0q�$	��Top=�$��62���� -   $ 	Inserisci��$��	Inserisci��$
��Left=�$��24��$��
484,26,0,0q�$	��Top=�$��76���� -   $ button��$
��Test��$
��Left=�$��
565,26,0,0q�$	��Top=�$��75��$��24���� $��Conti��.��� ���	#FFE5E5E5.+�  -   $ dgConti��$��2q�$��10,52,10,10q��� $��Operazioni standard��.��� ���	#FFE5E5E5.+�  -    $ dgOpStandard��$��2q�$��10,52,10,10q��� $��
Consuntivo��.��� ���	#FFE5E5E5.+�  -!   $ dgConsuntivi��$��2q�$��10,52,10,10q��� -"    ��Name$ lstConti��$
��Left=�$��22��$��	10,14,0,0q�$	��Top=�$	��301���� -#   $ btGeneraCons��$��
Visualizza��$��Right=�$��
0,13,10,10q�$	��Top=�$	��109��$��28���� -$   	 ��Name$	 label��$��Consuntivo del conto:��$
��Left=�$��28��$��
336,13,0,0q�$	��Top=�$	��129��$
��Bold"�$��Italic$��� -%   $	 lblContoCons��$��-��$
��Left=�$��28��$��
482,13,0,0q�$	��Top=�$	��351��$
��Bold"�$��14.667(��� $��Errori��.��� ���	#FFE5E5E5.+��� -&   
 ��Name$
 	txtErrori��$��2q�$��10,10,10,10q� �� TextWrapping$
 Wrap=��� -'   $	 	lblStatus��$��-��$��	0,10,10,0q�$	��Top=�$��46��$��Right=�$
��Bold"��� -(    ��Name$ imageOk��$��Right=�$��26��$��	0,10,80,0q�$	��Top=�$��26��$��ok.bmp���� -)   $ imageErr��$��Right=�$��26��$��	0,10,80,0q�$	��Top=�$��26��$��err.bmp��