   M S B A M L   `   `   `  ����\  XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35`Dclr-namespace:System.Windows.Controls;assembly=PresentationFrameworkSystem.Windows.Controls  5      E AEscapeRoom, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:EscapeRoom.HelpersEscapeRoom.Helpers 5      )   #EscapeRoom.Views.AdminDashboardView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml      5      YcontrolsDclr-namespace:System.Windows.Controls;assembly=PresentationFramework      5      2local clr-namespace:EscapeRoom.Helpers      5      $	��600��5      $	��400��6   
����������#252525$��Segoe UI Black)�5        � 	Resources  5	         BoolToVisibilityConverter��%   &	        �� 5
   
   60   5      .��5      � $��20,20,32,20q�6   .+�5   
   �� $��	82,0,49,0q�6   .+�5      �� $��PANEL ADMINISTRATORA��6   $��30(�64   $
��Bold"�6B   $��0,0,0,10q�6T    �� TextAlignment$ Center=�6f   ���	#FFFBBE53$	��398��6�   ���	#FF252525$��41��6�   �� 5       �� Orientation$ 
Horizontal=�6   $��0,0,0,10q�62   .+�5      �� $��  LICZBA POKOI: ��6   $
��Bold"�64   $	��117��6F   �� 5      $��5,0,20,0q�68   ��6   �� *
TotalRooms+68   5      �� 5      $ 
Horizontal=�6   $��2,0,0,10q�62   $	��359��6D   ���	#FF1D1D1E.+�5      �� $��REZERWACJE: ��6   $��5,8,6,8q�60   $��84��6A   �� 5      $��OCZEKUJĄCE: ��6   $��8,8,6,8q�60   $��88��6A   �� 5      $	��2,0q�6A   ��6   �� *PendingReservations+6A   �� 5      $��ZATWIERDZONE: ��6   $��0,8,20,8q�62   $	��111��6D   �� 5      $	��2,0q�6C   ��6   �� *ConfirmedReservations+6C   5      �� 5      $��ZATWIERDŹ REZERWACJE��6   $��0,5,0,10q�5       $��Center=�6'   ���	#FF16A570���#252525$	��250��6�   $��30��6�   ��5   4   �� *#!ApprovePendingReservationsCommand+y�5      �� *HasPendingReservations+ �� 	Converter# [  ��5    p   Z� 6�   �� 5#      $ 
Horizontal=�6   $��0,0,0,10q�62   $	��463��6D   .+�5$      �� $��    UŻYTKOWNICY: ��6   $
��Bold"�65   $	��116��6G   �� 5%      $	��0,0q�68   ��6   �� *
TotalUsers+68   5&      �� 5(      $ 
Horizontal=�6   $��0,0,0,10q�62   .+�5)      �� $��    PRZYCHÓD: ��6   $
��Bold"�62   $	��105��6D   �� 5*      $	��0,0q�6J   ��6   �� *TotalRevenue+ �� StringFormat$ C��6J   5+      �� 5-      $�� OSTATNIE REZERWACJE:��6   $
��Bold"�65   $��5q�6G   $	��302��6R   $��14��6^   �� 5.      $��43��6A   $	��292��6M   ���	#FF1D1D1E��6   �� *RecentReservations+��6p   Z� ��5/      �� 50      .R�51      �� $ 
Horizontal=�6%   .+�52      �� $��0,0,5,0q�6^   
���������6(   �� *RezerwacjaId+$ ID: {0}��6o   �� 53      $��0,0,5,0q�6\   
���������6(   �� *Status+$ Status: {0}��6m   �� 54      
���������6(   �� *
LiczbaOsob+$ 
Osób: {0}��6^   55      56      58      �� 5:      $�� OSTATNIE WIADOMOŚCI:��6   $
��Bold"�65   $��5,10,5,5q�6G   $	��302��6Y   $��14��6e   �� 5;      $��68��6=   $	��292��6I   ���	#FF1D1D1E��6   �� *RecentMessages+��6l   Z� ��5<      �� 5=      .R�5>      �� .+�5?      �� 
�������$
��Bold"�6j   ��6(   �� *Email+$ Od: {0}��6j   �� 5@      
�������$��CharacterEllipsis=�6q   ��6(   �� *Tresc+$ Wiadomość: {0}��6q   5A      5B      5D      �� 5F      $��0,10,0,0q�67   $	��356��6I   ��6   �� *StatusMessage+6I   � 5G       � IsIndeterminate . $��24��5I      $��0,10,0,0q�6&   $	��316��68   y�5H      �� *IsBusy+# [  5I   8   5J      5K      5L      