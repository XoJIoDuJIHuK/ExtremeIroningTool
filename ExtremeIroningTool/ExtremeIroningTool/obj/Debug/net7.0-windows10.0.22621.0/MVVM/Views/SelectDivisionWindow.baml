   M S B A M L   `   `   `  ����M  IExtremeIroningTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM+clr-namespace:ExtremeIroningTool.MVVM.ViewsExtremeIroningTool.MVVM.Views  5   	   8    2ExtremeIroningTool.MVVM.Views.SelectDivisionWindow   5   	   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   =local+clr-namespace:ExtremeIroningTool.MVVM.Views     5   	     � Title$  SelectDivisionWindow��5   	   $	��800��6&   $	��500��63    � WindowStyle$
 None=�5	   	    � WindowStartupLocation$ CenterScreen=�5
   	    � 	Resources 5         DivisionCard �� 
TargetType  	DarkGreen � ColumnDefinitions
 � Tag �� RelativeSource �� AncestorType	 � RowDefinitions  
PaleYellow  EditIcon
 �� Source  StringToImageConverter �� 	Converter  
DeleteIcon��%�  &	        7  7  7  7  7  7  7  �� 5   
   " ��5      .R�5      � $��0 0 0 10q�6   $	��480��5      $	��100��5      8��   5      �� 5      $��	0.208333* �6'   �� 5      $��	0.104166* �6'   �� 5      $��0.6875* �6'   .+�5      �� $��0��6   ��6(   �� *
PathToIcon+�� 5      $��2��6   $��30(�5      
�������$��Center=�5      $��Center=�5       ��5      �� *Name+5       �� 5"      -   5        
������ $��0��5#      $��0��5$      $��3��5%       5'      �� *DataContext+ �� " ��� 5)      $��1��6   	 5*      �� 5+      $��* �6(   �� 5,      $��* �6(   .+�5/      �� -   5        $��0��5/      8��  52      ��  �� " ��.��53      �� ��6    �� 8
  8  54      �� 55      -   5        $��1��55      8��  58      ��  �� " ��.��59      �� ��6    �� 8
  8  5:      5;      5=      5>      5?      .��5A      �   
LightGreen#��[ 6   	 5C   
   �� 5D      $��0.0125* �6   �� 5E      $��0.125* �6   �� 5F      $��0.0125* �6   �� 5G      $��0.775* �6   �� 5H      $��0.0125* �6   �� 5I      $��0.05* �6   �� 5J      $��0.0125* �6    5L   
   �� 5M      $��0.02* �6   �� 5N      $��0.96* �6   �� 5O      $��0.02* �6   .+�5R   
   �� $��1��5S      $��1��5T      ��5R      �� *CreateNewDivision+.��5V      � $	��480��6   $	��100��5W      #��[ 5X       5Z      �� 5[      $��	0.208333* �6'   �� 5\      $��	0.791666* �6'   .+�5_      � $��0��6   #��[ 5`      .+�5a      �� ��6   ��   AddDivisionIcon#
 [ 5b      �� 5d      $��1��6   
�������$
��Bold"�5f      $��Create new division��5g      $��30(�5h      $��Center=�5i      $��Center=�5j      5l      5n      �� 5p   
   $��1��6   $��3��5q      .��5r      �� -   5        #��[ 5t      $��0q�5u      ��5r      �� *	Divisions+��5w      �� 5x      .R�5y      ��  �� Orientation$ Vertical=�6%   5z      ��5}      �� 5~      .R�5      �� #��[  6&   ��5�      �� *ActualWidth+ �� " ��  SummatorConverter# [  �� ConverterParameter$ 20��5�      5�      5�      �� 5�   
    	 DefaultButton#��[	 5�      #��[ 5�      
���   �$��Close��5�      $��1��5�      $��5��5�      ��5�      �� *HideSelectDivisionWindow+5�      5�      5�      