   M S B A M L   `   `   `  ����P  LPSAMWPFControlLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\Bclr-namespace:PSAMWPFControlLibrary;assembly=PSAMWPFControlLibraryPSAMWPFControlLibrary  5   	   J FDPA_Musicsheets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:DPA_MusicsheetsDPA_Musicsheets 5   	       DPA_Musicsheets.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ]PSAMWPFControlBclr-namespace:PSAMWPFControlLibrary;assembly=PSAMWPFControlLibrary     5   	   /localclr-namespace:DPA_Musicsheets     5   	   -   5          � Title$  
MainWindow��5	   	   $	��500��6   $
��1380��6)   .��5
      � -   5         �Name$ MainGrid��5
       � RowDefinitions 5   
   �� 5      $
��368* �6   �� 5      $
��105* �6    � ColumnDefinitions 5   
   �� 5      $
��408* �6   �� 5      $
��109* �6   .+�5   
   ]� .+�5      o� $	��Top=�6   .@�5      m� $��_File��6   .@�5      m� -   5        $��_Open��5      6.   m� 5      -   5        $��_Save��5      6.   m� 5      -   5        $��
_SaveToPdf��5      63   5      m� 5      $��_Edit��6   .@�5      m� -   5        $��_Restore Previous��5      6:   5      m� 5      $��_Insert��6   .@�5      m� -   5        $��_Insert Clef��5      65   m� 5      -   5        $��_Insert Tempo��5      66   �� 5       m� 5!      -	   5        $��_Insert Timing 3/4��5!      6;   m� 5"      -
   5        $��_Insert Timing 4/4��5"      6;   m� 5#      -   5        $��_Insert Timing 6/8��5#      6;   5$      5%      �� 5&      5'      �� 5(   
   -   5         ��Name$ txt_MidiFilePath��5(      $
��Left=�6,   $��23��6G   $��	90,27,0,0q�6S    �� TextWrapping$
 Wrap=�6f   $��../../../smb1-Theme.mid��6z   $	��Top=�6�   $	��233��6�   �� 5)   
   -   5         ��Name$ btnPlay��5)      $
��Play��6"   $
��Left=�61   $��
353,27,0,0q�6L   $	��Top=�6`   $��75��6x   6�   �� 5*   
   -   5        $ btnOpen��5*      $
��Open��6"   $
��Left=�61   $��	10,27,0,0q�6L   $	��Top=�6_   $��75��6w   6�   �� 5+   
   -   5        $ btn_Stop��5+      $
��Stop��6#   $
��Left=�62   $��
433,27,0,0q�6M   $	��Top=�6a   $��75��6y   6�   �� 5,   
   -   5        $ btn_ShowContent��5,      $��Show content��6*   $
��Left=�6A   $��
353,53,0,0q�6\   $	��Top=�6p   $	��155��6�   6�   �� 5E   
   -   5        $��	Displayer��5E      $
 Wrap=�6#    �� AcceptsReturn . $��10,81,575,31q�6L   $��2��6�   �� 5F   
   -   5        $ btnSave��5F      $��Save to lilypond��6"   $
��Left=�6=   $��
568,27,0,0q�6X   $	��Top=�6l   $	��120��6�   6�   �� 5G   
   -   5        $ txtFilename��5G      $
��Left=�6'   $��23��6B   $��
568,50,0,0q�6N   $
 Wrap=�6b   $��Lilyfile��6v   $	��Top=�6�   $	��216��6�   �� 5H   
   -   5        $ btnUndo��5H      $
��Undo��6"   $
��Left=�61   $��
433,78,0,0q�6L   $	��Top=�6`   $��75��6x   $��1��6�   �� 5I   
   -   5        $��
tabControl��5I      $
��Left=�6'   $	��357��6B   $��
568,81,0,0q�6O   $	��Top=�6c   $	��474��6{   $��2��6�   5J      5K      