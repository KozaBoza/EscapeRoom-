   M S B A M L   `   `   `  ����E  AEscapeRoom, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:EscapeRoom.ViewModelsEscapeRoom.ViewModels  5      *    $EscapeRoom.Views.ReservationFormView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      2vm#clr-namespace:EscapeRoom.ViewModels     5      $	��600��6>   $	��452��6J   ���#252525$��Segoe UI Black)�5      
�������.��5      � $��20,-26,20,20q�6     � ColumnDefinitions  5	   
   �� 5
      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $	��Top=�6%   $��	0,98,10,0q�6=   $��2��6P   .+�5      �� $��36(�6   $
��Bold"�6&   $��0,0,0,10q�68    �� TextAlignment$ Center=�6J   $��52��6a   ���	#FF252525���	#FFFFFBFB�� 5       � Language$ pl-pl�6   $�
REZERWACJE��6'   5      �� 5      $��POKOJ:��6   �� 5      $
��Bold"�6=   ��6   �� *RoomViewModel.Nazwa+6=   �� 5      $��DATA:��6   (  "System.Windows.Controls.DatePicker  5      $��0,0,0,10q�6g      SelectedDate 6   �� *DataRozpoczecia+ �� UpdateSourceTrigger$ PropertyChanged=�6g   �� 5      $��LICZBA OSOB:��6   �� 5      $��0,0,0,10q�62   ��6   �� *
LiczbaOsob+62   �� 5      $��STATUS:��6   �� 5      $��0,0,0,10q�6A   ��6   �� *RoomAvailabilityMessage+6A   �� 5       �� Orientation$ 
Horizontal=�6   $��0,10,0,0q�62   $��54��6D   .+�5      �� $��	POTWIERDZ��6   $��0,0,5,0q�5      $��97��6*   $��54��65   ���	#FF16A570���	#FFF4EAEA��5      �� *ConfirmReservationCommand+{�5      �� *	IsValid+��5   A   Z� 6o   �� 5       $��ANULUJ��6   $	��101��5!   F   ���	#FFFEF3F3���	#FFF05541��6   �� *CancelReservationCommand+��6R   Z� 6�   5"      5#      �� 5%   
   $��1��6   $��Right=�5&      $��Bottom=�61   $��10,0,0,0q�5'      .+�5(      �� $��SZCZEGOLY POKOJU��6   $��14(�60   $
��Bold"�6>   $��0,0,0,5q�6P   �� 5)      �� 5*      $�POZIOM: ��6    �� 5+      $
��Bold"�6R   �6   �� *RoomViewModel.PoziomTrudnosci+ �� Mode$ OneWay=�6R   5,      �� 5-      �� 5.      $�MAX: ��6    �� 5/      $
��Bold"�6L   �6   �� *RoomViewModel.MaxGraczy+$ OneWay=�6L   50      �� 51      �� 52      $�CENA: ��6    �� 53      $
��Bold"�6W   �6   �� *RoomViewModel.Cena+ �� StringFormat$ C��$ OneWay=�6W   6l   54      56       � 	Resources 57         BoolToVisibilityConverter��%   &	        �� 58   
   6'   59      5:      