   M S B A M L   `   `   `  ����M  IExtremeIroningTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:ExtremeIroningToolExtremeIroningTool  5   	   M+clr-namespace:ExtremeIroningTool.MVVM.ViewsExtremeIroningTool.MVVM.Views  5   	   #    ExtremeIroningTool.MainWindow   5   	   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:ExtremeIroningTool     5   	   =views+clr-namespace:ExtremeIroningTool.MVVM.Views     5   	   -   5          � Topmost$
  True��5	   	    � WindowStyle$
 None=�5
   	    � WindowStartupLocation$ Manual=�5   	    � WindowState$ 	Maximized=�5   	    � 	Resources 5         TakeAllScreen �� 
TargetType �� Property �� Value��%�   &	        �� 5   
   " >�6&   .�5      ��  � ��$ 0��6)   �� 5       � ��$ 3��6-   �� 5       � ��$ 0��6,   �� 5       � ��$ 2��60   5      5      ��5      !� 5   
    !� Visual 5      r� 5      -   5        	 r�Name$	 MWBackground��5      
 r� LoadedBehavior$
 Manual=�5       r� IsMuted 5      �� *CurrentMutedState+5      5      .��5"      �  � ColumnDefinitions 5$   
   �� 5%      $��
0.7265625* �6   �� 5&      $��
0.2734375* �6    � RowDefinitions 5(   
   �� 5)      $)��#0.09259259259259259259259259259259* �6   �� 5*      $)��#0.88888888888888888888888888888889* �6   �� 5+      $)��#0.01851851851851851851851851851852* �6   .+�5.   
   0   *ExtremeIroningTool.MVVM.Views.ViewMainMenu  -   5          Name$ MainMenu��5.      $y�Visible=�5/      #��[  50      5   /ExtremeIroningTool.MVVM.Views.ViewCountrySelect  52   
   -   5          Name$ CountrySelect��52   "   $y�	Collapsed=�53   "   #��[  54      4   .ExtremeIroningTool.MVVM.Views.ArmyConfigurator  56   
   -   5          Name$ ArmyConfigurator��56   !   $y�	Collapsed=�57   !   #��[  58      1   +ExtremeIroningTool.MVVM.Views.ViewModifiers  5:   
   -   5          Name$ 	Modifiers��5:      $y�	Collapsed=�5;      #��[  5<      *   $ExtremeIroningTool.MVVM.Views.Battle  5>   
   -   5          Name$ Battle��5>      $y�	Collapsed=�5?      #��[  5@      3   -ExtremeIroningTool.MVVM.Views.BattalionEditor  5B   
   -   5          Name$ BattalionEditor��5B       $y�	Collapsed=�5C       #��[  5D       /   )ExtremeIroningTool.MVVM.Views.AudioPlayer  5F   
   -	   5          Name$ Player��5F      $��1��5G      $��1��5H      �� 5J   
   $y�Hidden=�5K      $��50��5L      $��50��5M      $
��Left=�5N      $��Bottom=�5O      
���  ��$��2��5Q      $��3��5R      ��5J      �� *ExitApplicationCommand+5R      5T      5U      