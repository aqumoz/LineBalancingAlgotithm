   M S B A M L   `   `   `  ����Q  MLineBalancingAlgorithm, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:LineBalancingAlgorithmLineBalancingAlgorithm  5   	   '    !LineBalancingAlgorithm.MainWindow   5   	   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   6local$clr-namespace:LineBalancingAlgorithm     5   	     � Title$  
MainWindow��5   	   $	��450��6   $
��1600��6)   .��5	      �  � ColumnDefinitions 5
   
   �� 5      �� 5       � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   &   System.Windows.Controls.DataGrid  -   5        $��Grid1��5      $
��Auto��6    $��2��6.      AlternatingRowBackground
 �����$   HorizontalScrollBarVisibility$
 Auto=�6e      AutoGenerateColumns$ False��6�      CanUserAddRows$
 True��6�      CanUserDeleteRows$
 True��6�      CanUserReorderColumns$
 True��6�   	   CanUserSortColumns$
	 True��6�   
   CanUserResizeColumns$

 True��6     CanUserResizeRows$
 True��6(     Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$	 No.��6%   1  +System.Windows.Controls.DataGridBoundColumn   Binding 63   ��  �� Path$	 Num�  5      $1 +Work Element Description (for own refrence)��6%    6Z   �� $ Description�  5      $ 	Tek [min]��6%    68   �� $	 Tek�  5      $ Must Be Preceded By��6%    6B   �� $ 
PrecededBy�  5      $	 RPW��6%    62   �� $	 RPW�  5      $ Column��6%    65   �� $ Column�  5      $ Branch��6%    65   �� $ Branch�5      �� 5    
   -   5        $��BtnCalc��5    ,   $
��Auto��6   $��2��6   $��	Calculate��6;   $��Bottom=�6O   �� 5"   
   $��Precedence diagram��6   $��1��6-   $��0��6=   $
��Bold"�6J   $��14(�6\   $��Center=�6j   $	��Top=�6�   $��30��6�   �� 5#   
   -   5         ��Name$ Diagram��5#      $��1��6!   $��1��61   $
��Auto��6>   $
��Auto��6L   �� 5$   
   -   5        $��BtnCopy��5$   <   $
��Auto��6   $��1��6   $��2��6/   $
��Copy��6K   $��Bottom=�6Z   �� 5%   
   -   5         ��Name$ gridSplitter��5%      $��1��6-   $
��Left=�6=   $	��434��6X   $	��Top=�6e   $��5��6}   $��3��6�   5&      5'      