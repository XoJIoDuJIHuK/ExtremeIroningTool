   M S B A M L   `   `   `  ����M  IExtremeIroningTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM+clr-namespace:ExtremeIroningTool.MVVM.ViewsExtremeIroningTool.MVVM.Views  5      8    2ExtremeIroningTool.MVVM.Views.GeneralSelectionCard   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      =local+clr-namespace:ExtremeIroningTool.MVVM.Views     5      
  � Tag  5
      �� 5   
    �� RelativeSource 6   ��  �� Mode$ FindAncestor=� �� AncestorType>   8ExtremeIroningTool.MVVM.Views.BottomPartArmyConfigurator"  .��5      �    
LightGreen#��[  6    � RowDefinitions 5   
   �� 5      $��0.0667* �6   �� 5      $��0.2667* �6   �� 5      $��0.0667* �6   �� 5      $��0.2333* �6   �� 5      $��0.0667* �6   �� 5      $��0.2333* �6   �� 5      $��0.0667* �6    � ColumnDefinitions 5   
   �� 5      $��0.24096* �6   �� 5      $��	0.024096* �6   �� 5      $��	0.518073* �6   �� 5      $��0.19277* �6   �� 5      $��	0.024096* �6   .+�5   
   �� -   5         ��Name$ ArmyGroupName��5      $��2��5       $��1��5!      $��2��5"      $��Right=�5$      $��22(�5%      $��Regular"�5&      
���������5#      �� *Name+5'      �� 5)   
   $��3��6   $��3��5*      $��2��5+      $��10q�5,        	DarkGreen#��[ 5.      $��0q�5/      ��5-      �� *Armies+��50      �� 51      .R�52      ��  �� Orientation$ 
Horizontal=�6!   $��Right=�53   !   54      ��56      �� 57       �� 
TargetType" ��6   .�58      �� 	 �� Property	 � {�
 �� Value$
 False��62   �� 59      	 � ��
 5:      �� 5;       �� 
TargetType" ��6.   .R�5<   "   �� 5=      5?      5@      ��5B      �� 5C      .R�5D      �� $��40��6:   $��40��6E   $��5q�6Q   ��6   �� *
PathToIcon+6Q   5E      5G      �� 5I   
   -   5         ��Name$ SelectButton��5I        NoHover#��[ 5J      $��0��5K      $��5��5L      $��0��5M      $��7��5N      
������   5Q      �� 5R       6   �� $ FindAncestor=�" ��5T      �� 5V   
   -   5        $
 Icon��5V      $��1��5W      $��5��5X      $��0��5Y      
������   5\      ��  �� " ��.��5]      �� ��6   �� *Commander.PathToIcon+5^      �� 5`   
   -   5        $ DeleteButton��5a      #��[ 5`      $��3��5b      $��3��5c      $��3��5d      $��Delete��5e        
PaleYellow#��[ 5g        5h      �� 5i       6   �� $ FindAncestor=�" ��5k      5m      5n      