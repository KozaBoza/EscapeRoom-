   M S B A M L   `   `   `  ����E  AEscapeRoom, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:EscapeRoom.ViewsEscapeRoom.Views  5      =#clr-namespace:EscapeRoom.ViewModelsEscapeRoom.ViewModels  5      5clr-namespace:EscapeRoom.ModelsEscapeRoom.Models  5          EscapeRoom.Views.RoomView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      0localclr-namespace:EscapeRoom.Views     5      2vm#clr-namespace:EscapeRoom.ViewModels     5      0dataclr-namespace:EscapeRoom.Models     5      $	��600��6;   $	��400��6G   ���#252525$��Segoe UI Black)�5      
�������.��5
      � .+�5   
   �� 
�������$��1q�6$     �� Padding$  16q�68   $��8q�6E   ._�5      �� .+�5      �� $��DOSTĘPNE POKOJE��6   $��20(�63   $
��Bold"�6A   $��0,0,0,10q�6S   $��Center=�6e   �� 5      $
��Auto=�5      
������ $��0q�5      $	��100��5      ��5      �� *Rooms+��5      �� 5       �� DataType   EscapeRoom.Models.Room"  6'   .R�5      �� 
�������$��1q�6=   $��0,5,0,5q�6Q   $  10q�6b    �� CornerRadius$ 5��6o   ._�5   "   �  � ColumnDefinitions 5   &   �� 5   *   $��* �6;   �� 5   *   $
��Auto �6;   �� 5   *   $
��Auto �6;   .+�5   &   �� $��0��60   $��16(�5   0   $
��Bold"�6>   $��Center=�6P   $��0,0,10,0q�5    0   ��5   @   �� *Nazwa+5    0   �� 5"   &   $��1��6-   $��REZERWUJ��6=   $��80��5&   -   $��30��68   $��0,0,5,0q�6D   ���	#FF252525���	#FF8BC34A��5#   -   ��  �� RelativeSource ��  �� AncestorType" �� �� Path$! DataContext.BookRoomCommand���5$   -   �� {�5%   -   ��  �� " ��$ DataContext.IsLoggedIn���5'   D   Z� 6[   �� 5)   &   $��2��6-   $��OPINIE��6=   $��80��5,   -   $��30��68   ���	#FF252525���	#FFD500F9��5*   -   ��  �� " ��$$ DataContext.ShowReviewsCommand���5+   -   �� ��5-   D   Z� 6[   5.   #   5/      50      52      53      54      55       � 	Resources 56         NoHighlightListBoxItemStyle �� 
TargetType	 �� Property
 �� Value �� 
TargetType J� SnapsToDevicePixels �� Triggers P� Property P� Value �� 
TargetName��%�  &	        �� 57   
   " ��64   .�58      �� 	 �@�FocusVisualStyle
 61   Z� �� 59      	 � ��
 5:      �� 5;      " ��6&   .R�5<      �� $��Border��6!   $  5q�6/    . #��z��5=   !   #��z��5>   !   #��z��5?   !   ._�5@      �� 5A       5B      P� 5C       �@��
IsSelected . . �5D   "   �� $ Border��6)   	 � ��

 ���� 6S   �� 5E   "   $ Border��6)   	 � ��

 ���� 6T   5F      P� 5G       �@J�IsMouseOver . . �5H   "   �� $ Border��6)   	 � ��

 ���� 6S   �� 5I   "   $ Border��6)   	 � ��

 ���� 6T   5J      5L      5N      5O      5P      5Q      