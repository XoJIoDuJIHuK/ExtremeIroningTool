   M S B A M L   `   `   `  ����M  IExtremeIroningTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM+clr-namespace:ExtremeIroningTool.MVVM.ViewsExtremeIroningTool.MVVM.Views  5      3    -ExtremeIroningTool.MVVM.Views.BattalionEditor   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      =local+clr-namespace:ExtremeIroningTool.MVVM.Views     5      .��5	      �    
LightGreen#��[  6     � ColumnDefinitions  5
   
   �� 5      $)��#0.01041666666666666666666666666667* �6   �� 5      $)��#0.97916666666666666666666666666667* �6   �� 5      $)��#0.01041666666666666666666666666667* �6    � RowDefinitions 5   
   �� 5      $)��#0.01851851851851851851851851851852* �6   �� 5      $)��#0.05555555555555555555555555555556* �6   �� 5      $)��#0.01851851851851851851851851851852* �6   �� 5      $)��#0.88888888888888888888888888888889* �6   �� 5      $)��#0.01851851851851851851851851851852* �6   .+�5   
   � -   5         �Name$ NavigationButtons��5      $��1��5      $��1��5        5      �� 5      $)��#0.24202127659574468085106382978723* �6#   �� 5      $)��#0.51595744680851063829787234042553* �6#   �� 5      $)��#0.24202127659574468085106382978723* �6#   .+�5       �� $��0��5!        DefaultButton#��[ 5"      $
��Back��5#      ��5       �� *BackCommand+5#      �� 5%      $��Battalion Editor��6    
 Header#��[ 5&      $��1��5'      5)      � 5+   
   $��3��6   $��1��5,      .+�5.      &   System.Windows.Controls.DataGrid  -   5           AutoGenerateColumns$ False��5/      ��5.      �� *AllBattalions+   Columns 51      0  *System.Windows.Controls.DataGridTextColumn  52      ,  &System.Windows.Controls.DataGridColumn   Header$
 Name��6)      
IsReadOnly$
 True��6P   1  +System.Windows.Controls.DataGridBoundColumn   Binding 67   �� *name+6P     53      $ Health��6)    69   �� *health+  54      $ Organization��6)    6?   �� *organization+  55      $ 
SoftAttack��6)    6=   �� *
softAttack+  56      $ 
HardAttack��6)    6=   �� *
hardAttack+  57      $ Defense��6)    6:   �� *	defence+  58      $ Breakthrough��6)    6?   �� *breakthrough+  59      $ Armor��6)    68   �� *armor+  5:      $ Piercing��6)    6;   �� *
piercing+  5;      $ 
FrontWidth��6)    6=   �� *
frontWidth+  5<      $ 
PathToIcon��6)    6=   �� *
pathToIcon+  5=      $ VehicleRatio��6)    6?   �� *vehicleRatio+5?      5A      5C      5D      