   M S B A M L   `   `   `  ����E  AEscapeRoom, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:EscapeRoom.ViewsEscapeRoom.Views  5      =#clr-namespace:EscapeRoom.ViewModelsEscapeRoom.ViewModels  5      #    EscapeRoom.Views.RegisterView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:EscapeRoom.Views     5      :
viewmodels#clr-namespace:EscapeRoom.ViewModels     5      $	��600��5	      $	��452��6   ���#252525$��Segoe UI Black)�5      
�������  � 	Resources  5         BoolToVisibilityConverter��%   &	        �� 5   
   6'   5      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5       $
��Auto �6   �� 5!      $��* �6    � ColumnDefinitions 5#   
   �� 5$      $
��Auto �6   �� 5%      $��* �6   .+�5(   
   �� $��0��6   $��2��6!   $��REJESTRACJA��65   $��Center=�5)      $��30(�61   $
��Bold"�6?   $��	0,24,0,10q�6Q   ���	#FF252525���	#FF5FEE5B$	��408��6�    �� TextAlignment$ Center=�6�   �� 5+   
   $��1��6   $��0��6!   $��TELEFON:��61   $��10q�6A   $��Center=�6M   �� 5,   
   $��1��6   $��1��6   $	��268��5-      $
��Left=�6   $��
10,10,0,10q�69   ��5,   /   �� *	Telefon+ �� UpdateSourceTrigger$ PropertyChanged=�5-   9   �� 5/   
   $��2��6   $��0��6!   $��E-MAIL:��61   $��10q�6@   $��Center=�6L   �� 50   
   $��2��6   $��1��6   $	��268��51      $
��Left=�6   $��
10,10,0,10q�69   ��50   /   �� *Email+$ PropertyChanged=�51   9   �� 53   
   $��3��6   $��0��6!   $��HASŁO:��61   $��10q�6?   $��Center=�6K   G� 54   
   -   5         G�Name$ PasswordInput��54      $��3��55      $��1��56      $	��268��57      $
��Left=�58      $��
10,10,0,10q�59      5:      �� 5<   
   $��4��6   $��0��6!   $��POTWIERDŹ HASŁO:��61   $��10q�6I   $��Center=�6U   G� 5=   
   -   5        $ ConfirmPasswordInput��5=      $��4��64   $��1��6A   $	��268��6Q   $
��Left=�6]   $��
10,10,0,10q�6x   5>      �� 5@   
   $��5��6   $��0��6!   $��IMIE:��61   $��10q�6>   $��Center=�6J   �� 5A   
   $��5��6   $��1��6   $	��268��5B      $
��Left=�6   $��
10,10,0,10q�69   ��5A   /   �� *	FirstName+$ PropertyChanged=�5B   9   �� 5D   
   $��6��6   $��0��6!   $��	NAZWISKO:��61   $��10q�6B   $��Center=�6N   �� 5E   
   $��6��6   $��1��6   $	��268��5F      $
��Left=�6   $��
10,10,0,10q�69   ��5E   /   �� *
LastName+$ PropertyChanged=�5F   9   �� 5H   
   $��7��6   $��2��6!   
���  ��$��Center=�5I      $��0,10,0,0q�6)   ��5H   5   �� *ErrorMessage+5I   )   �� 5M   
   $��8��6   $��ZAREJESTRUJ��6   $��141,0,325,74q�6X   $��2��6n   ���	#FF7498E8���	#FF252525$��2��6�   ��64   �� *RegisterCommand+��6   Z� 6�   �� 5O   
   $��8��6   $��ANULUJ��6   $��50��6T   $��144,0,181,0q�6`   $	��Top=�6v   $��1��6�   $��2��6�   ���	#FF252525���	#FFDDC77F��6/   �� *GoToLoginCommand+��6�   Z� 5R      5S      