   M S B A M L   `   `   `  ����E  AEscapeRoom, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:EscapeRoom.ViewModelsEscapeRoom.ViewModels  5      -    'EscapeRoom.Views.ReservationHistoryView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      2vm#clr-namespace:EscapeRoom.ViewModels     5      .��5	      � ���#252525  � RowDefinitions  5
   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��TWOJE REZERWACJE��6   $��24(�5      $
��Bold"�5      $��20,20,20,10q�5      
�������$��Center=�5      �� 5   
   $��1��6   $��20q�5      
������ $��1q�5      
���������5      �� *UserReservations+��5      �� 5      .R�5      �� 
�������$��1q�5       �� Padding$ 15q�5       $��5q�5!       �� CornerRadius$ 5��5"      $	��500��5#      ._�5$      �   5%      �� 5&   "   $
��Auto �60   �� 5'   "   $
��Auto �60   �� 5(   "   $
��Auto �60   �� 5)   "   $
��Auto �60    � ColumnDefinitions 5+      �� 5,   "   $��* �63   �� 5-   "   $
��Auto �63   .+�51      �� $��0��6(   $��0��65   $
��Bold"�53   &   $��18(�54   &   ���	#FF8BC34A��52   &   �� *NazwaPokoju+55   &   �� 58      $��0��6(   $��1��65   
�������$��Right=�5;   &   ��59   &   �� *StatusDisplay+5;   &   �� 5>      $��1��6)   $��0��66    �� Orientation$ 
Horizontal=�5?   '   $��0,10,0,0q�5@   '   .+�5A   "   �� $��Data: ��6,   
�������5B   *   �� 5C   "   
���������6,   �� *DataRozpoczecia+ �� StringFormat$ {0:dd.MM.yyyy}��5D   *   5E      �� 5H      $��2��6)   $��0��66   $ 
Horizontal=�5I   '   $��0,5,0,0q�5J   '   .+�5K   "   �� $��Liczba graczy: ��6,   
�������5L   *   �� 5M   "   
���������6,   �� *
LiczbaOsob+5N   *   5O      �� 5R      $��3��6)   $��0��66   $ 
Horizontal=�5S   '   $��0,5,0,0q�5T   '   .+�5U   "   �� $��Poziom trudnosci: ��6,   
�������5V   *   �� 5W   "   
���������6,   �� *
Trudnosc+5X   *   �� 5Y   "   $��/5��6,   
�������5Z   *   5[      �� 5^      $��3��6(   $��1��65   
�������$��Right=�5a   &   ��5_   &   �� *Cena+$ {0:C}��5a   &   5b      5c      5d      ��5f      �� 5g       �� 
TargetType" ��6   .�5h      ��  �� Property � �� �� Value$ Stretch=�6C   �� 5i       � ��
 ���� 63   �� 5j       � ��$	 0,5q�6/   5k      5m      5n      5o      