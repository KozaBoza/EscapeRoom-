   M S B A M L   `   `   `  ����  
EscapeRoom*    $EscapeRoom.Views.ReservationFormView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      $	��600��5      $	��452��6   ���#252525$��Segoe UI Black)�5      
�������.��5      � $��20q�6   .+�5	   
   �� $��0,98,0,0q�6   .+�5
      �� $��36(�6   $
��Bold"�6&   $��0,0,0,10q�68     �� TextAlignment$  Center=�6J   $��48��6a   �� 6n    � Language$ pl-pl�6r   $�
REZERWACJE��6�   �� 6�   �� 6�   $ pl-pl�6�   6�   �� 5      $��POKOJ:��6   �� 5      $
��Bold"�6=   ��6   �� *RoomViewModel.Nazwa+6=   �� 5      $��DATA:��6   (  "System.Windows.Controls.DatePicker  5      $��0,0,0,10q�6t      SelectedDate 6   �� *DataRozpoczecia+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�6t   �� 5      $��LICZBA OSOB:��6   �� 5      $��0,0,0,10q�62   ��6   �� *
LiczbaOsob+62   �� 5      $��STATUS:��6   �� 5      $��0,0,0,10q�64   ��6   �� *
StatusText+64   �� 5      $��0,0,0,10q�6A   ��6   �� *RoomAvailabilityMessage+6A   �� 5       �� Orientation$ 
Horizontal=�6   .+�5      �� $��	POTWIERDZ��6   $��0,0,5,0q�5      $��97��6*   $��34��65   ���	#FF16A570���	#FFF4EAEA��5      �� *ConfirmReservationCommand+{�5      �� *	IsValid+��5   A   Z� 6o   �� 5      $��ANULUJ��6   $	��101��5   F   ���	#FFFEF3F3���	#FFF05541��6   �� *CancelReservationCommand+��6R   Z� 6�   5      5      5       � 	Resources 5         BoolToVisibilityConverter��%   &	        �� 5   
   6'   5       5!      