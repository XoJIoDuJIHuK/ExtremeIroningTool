   M S B A M L   `   `   `  ����M  IExtremeIroningTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM+clr-namespace:ExtremeIroningTool.MVVM.ViewsExtremeIroningTool.MVVM.Views  5      8    2ExtremeIroningTool.MVVM.Views.GeneralSelectionCard   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      =local+clr-namespace:ExtremeIroningTool.MVVM.Views     5      
  � Tag  5
      �� 5   
    �� RelativeSource 6   ��  �� Mode$ FindAncestor=� �� AncestorType>   8ExtremeIroningTool.MVVM.Views.BottomPartArmyConfigurator"  .��5      �  � RowDefinitions 5   
   �� 5      $��0.0667* �6   �� 5      $��0.2667* �6   �� 5      $��0.0667* �6   �� 5      $��0.2333* �6   �� 5      $��0.0667* �6   �� 5      $��0.2333* �6   �� 5      $��0.0667* �6    � ColumnDefinitions 5   
   �� 5      $��0.24096* �6   �� 5      $��	0.024096* �6   �� 5      $��	0.518073* �6   �� 5      $��0.19277* �6   �� 5      $��	0.024096* �6   .+�5   
   �� -   5         ��Name$ ArmyGroupName��5      $��2��5       $��1��5!      $��2��5"      $��Right=�5$      $��22(�5%      $��Regular"�5&      ��5#      �� *Name+5&      �� 5(   
   -   5         ��Name$ SelectButton��5(         NoHover#��[  5)      $��0��5*      $��5��5+      $��0��5,      $��7��5-      
������   50      �� 51       6   �� $ FindAncestor=�" ��53      �� 55   
   -   5        $ DeleteButton��56      #��[  55      $��3��57      $��3��58      $��3��59      $��Delete��5:        5<      �� 5=       6   �� $ FindAncestor=�" ��5?      �� 5A   
   -   5        $
 Icon��5A      $��1��5B      $��5��5C      $��0��5D      
������   5G      ��  �� " ��.��5H      �� ��6   �� *Commander.PathToIcon+5I      �� 5K   
   $��3��6   $��3��5L      $��2��5M      $��10q�5N      
������ ��5P      �� *Armies+��5Q      �� 5R      .R�5S      ��  �� Orientation$ 
Horizontal=�6!   $��Right=�5T   !   5U      ��5W      �� 5X      	 �� 
TargetType"	 ��6   .�5Y      �� 
 �� Property
 � {� �� Value$ False��62   �� 5Z      
 � �� 5[      �� 5\       �� 
TargetType" ��6.   .R�5]   "   �� 5^      5`      5a      ��5c      �� 5d      .R�5e      �� $��40��6:   $��40��6E   $��5q�6Q   ��6   �� *
PathToIcon+6Q   5f      5h      5i      5j      