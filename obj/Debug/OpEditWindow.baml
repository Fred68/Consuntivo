   M S B A M L   `   `   `  ����E  AConsuntivo, Version=1.0.0.2, Culture=neutral, PublicKeyToken=nullclr-namespace:WPF02WPF02  5   	       WPF02.OpEditWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   %localclr-namespace:WPF02     5   	     � Title$  OpEditWindow��5   	   $��296.032��6   $��678.497��6/    � WindowStartupLocation$ CenterScreen=�6?   .��5	      � $��0,0,0,0q�6   .+�5
      �� -   5         ��Name$ btCancel��5
      $��Cancel��6   $
��Left=�6.   $��496,181,0,0q�6I   $	��Top=�6^   $��75��6v   $��35��6�   �� 5      -   5        $
 btOk��5      $��Ok��6   $
��Left=�6&   $��576,181,0,0q�6A   $	��Top=�6V   $��75��6n   $��35��6�   �� 5      -   5         ��Name$ tbNota��5      $
��Left=�6   $��23��67   $��508,47,-14,0q�6C    �� TextWrapping$
 Wrap=�6Y   $��TextBox��6m   $	��Top=�6|   $	��144��6�   (  "System.Windows.Controls.DatePicker  5      -   5          Name$ dpData��5      $
��Left=�6   $��	10,46,0,0q�6:   $	��Top=�6M   �� 5      -   5        $ tbDescrizione��5      $
��Left=�6#   $��23��6>   $��
159,46,0,0q�6J   $
 Wrap=�6^   $��TextBox��6r   $	��Top=�6�   $	��344��6�   �� 5      -   5        $ 	tbImporto��5      $
��Left=�6   $��23��6:   $��
10,123,0,0q�6F   $
 Wrap=�6Z   $��TextBox��6n   $	��Top=�6}   $	��144��6�    J� RenderTransformOrigin$ 2.125,0.304&�6�   �� 5      -   5         ��Name$ chkConsuntivo��5      $��
Consuntivo��6$   $
��Left=�69   $��
508,92,0,0q�6T   $	��Top=�6h   $ -0.017,-0.608&�6�   �� 5      -   5        $ chkVerificato��5      $��
Verificato��6$   $
��Left=�69   $��508,123,0,0q�6T   $	��Top=�6i   �� 5      -	   5         ��Name$ cbTipo��5      $
��Left=�6   $��159,124,0,0q�68   $	��Top=�6M   $��77��6e   �� 5      -
   5        $ tbConti��5      $
��Left=�6   $��23��68   $��225,196,0,0q�6D   $
 Wrap=�6Y   $��TextBox��6m   $	��Top=�6|   $	��210��6�   6�   �� 5      -   5        	 ��Name$	 label��5   
   $��Data operazione��6   $
��Left=�63   $��	10,15,0,0q�6N   $	��Top=�6a   $	��144��6y   �� 5      -   5        $	 label1��5   
   $��Descrizione operazione��6   $
��Left=�6;   $��
159,15,0,0q�6V   $	��Top=�6j   $	��344��6�   �� 5      -   5        $	 label2��5   
   $
��Note��6   $
��Left=�6)   $��
508,15,0,0q�6D   $	��Top=�6X   $	��120��6p   �� 5      -   5        $	 label3��5   
   $��Importo��6   $
��Left=�6,   $��	10,92,0,0q�6G   $	��Top=�6Z   $	��144��6r   �� 5      -   5        $	 label3_Copy��5   
   $
��Tipo��6   $
��Left=�6.   $��
159,92,0,0q�6I   $	��Top=�6]   $��77��6u   �� 5      -   5        $	 label3_Copy1��5   
   $��Operazione standard��6    $
��Left=�6>   $��
10,165,0,0q�6Y   $	��Top=�6m   $	��210��6�   �� 5      -   5        $	 label3_Copy2��5   
   $��Conti (con segno)��6    $
��Left=�6<   $��225,165,0,0q�6W   $	��Top=�6l   $	��210��6�   $��26��6�   �� 5      -   5        $ cbOperazioniStandard��5      $
��Left=�6+   $��
10,196,0,0q�6F   $	��Top=�6Z   $	��210��6r   $��23��6~   5      5      