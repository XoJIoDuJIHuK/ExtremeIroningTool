   M S B A M L   `   `   `  ����M  IExtremeIroningTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:ExtremeIroningToolExtremeIroningTool  5   	   M+clr-namespace:ExtremeIroningTool.MVVM.ViewsExtremeIroningTool.MVVM.Views  5   	   #    ExtremeIroningTool.MainWindow   5   	   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:ExtremeIroningTool     5   	   =views+clr-namespace:ExtremeIroningTool.MVVM.Views     5   	     � Topmost$
  True��5	   	   $	��864��5
   	   $
��1536��5   	    � WindowStyle$
 None=�5   	    � WindowStartupLocation$ Manual=�5   	    � WindowState$ 	Maximized=�5   	    � 	Resources 5         TakeAllScreen �� 
TargetType �� Property �� Value��%�   &	        �� 5   
   " >�6&   .�5      ��  � ��$ 0��6)   �� 5       � ��$ 3��6-   �� 5       � ��$ 0��6,   �� 5       � ��$ 2��60   5      5      .��5      �  � ColumnDefinitions 5   
   �� 5      $��
0.7265625* �6   �� 5      $��
0.2734375* �6   	 � RowDefinitions	 5   
   �� 5      $)��#0.09259259259259259259259259259259* �6   �� 5       $)��#0.88888888888888888888888888888889* �6   �� 5!      $)��#0.01851851851851851851851851851852* �6   .+�5$   
   0   *ExtremeIroningTool.MVVM.Views.ViewMainMenu  -   5        
  Name$
 MainMenu��5$      $y�Visible=�5%      #��[  5&      5   /ExtremeIroningTool.MVVM.Views.ViewCountrySelect  5(   
   -   5          Name$ CountrySelect��5(   "   $y�	Collapsed=�5)   "   #��[  5*      4   .ExtremeIroningTool.MVVM.Views.ArmyConfigurator  5,   
   -   5          Name$ ArmyConfigurator��5,   !   $y�	Collapsed=�5-   !   #��[  5.      *   $ExtremeIroningTool.MVVM.Views.Battle  50   
   -   5          Name$ Battle��50      $y�	Collapsed=�51      #��[  52      /   )ExtremeIroningTool.MVVM.Views.AudioPlayer  54   
   -   5          Name$ Player��54      $��1��56      $��1��57      {�55      �� *PlayerEnabled+57      �� 59   
   $��50��5:      $��50��5;      $
��Left=�5<      $��Bottom=�5=      
���  ��$��2��5?      $��3��5@      ��59      �� *ExitApplicationCommand+5@      5B      5C      