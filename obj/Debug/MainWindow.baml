   M S B A M L   `   `   `  ����E  AEscapeRoom, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:EscapeRoom.ViewModelsEscapeRoom.ViewModels  5   	   3clr-namespace:EscapeRoom.ViewsEscapeRoom.Views  5   	   !    EscapeRoom.Views.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2vm#clr-namespace:EscapeRoom.ViewModels     5   	   0viewsclr-namespace:EscapeRoom.Views     5   	   -   5          � Title$  Escape Room - Panel��5	   	   $
��1000��5
   	   $	��400��6    � WindowStartupLocation$ CenterScreen=�5   	   ���#252525
�������$��Segoe UI Black)�5   	    � DataContext 5      )   #EscapeRoom.ViewModels.MainViewModel  5   
    � 	Resources 5         BoolToVisibilityConverter��%   &	        �� 5   
   6'   5      .��5      ]� .+�5   
   o� $	��Top=�6   ���#333333
�������.@�5      m� $��_Plik��6   .@�5      m� -   5        $��	_Wyjście��5      6-   5      m� 5      $��_Widok��6   .@�5      m� -   5        $��_Pełny ekran��5      61   5      m� 5      $��_Pomoc��6   .@�5      m� -   5        $��_O programie��5      61   5       5!      �� 5#   
   -   5         ��Name$ MainContentControl��5#      $	��Top=�65   $��Stretch=�6J   $��Stretch=�6h   5$      � 5&   
   $��Bottom=�6   ���#252525$	��155��6<   $	��848��6I    � ColumnDefinitions 5'      �� 5(      $
��Auto �6#   $��46.907��60   �� 5)      �� 5*      $
��Auto �6#   .+�5-      �� $
��Bold"�69   $	��Top=�6K   $��
0,73,742,0q�6c   $��1��6w   $��22��6�    J� RenderTransformOrigin$ 	1.189,0.9&�5.      ��5-      �� *CurrentUserName+y�5.   :   �� *IsLogoutVisible+ �� 	Converter# [  �� 50       �� Orientation$ 
Horizontal=�6   $��1��62   $��
93,0,99,51q�6B   $ 0.499,1.214&�6V   .+�51      �� -   5        $��POKOJE��51      $	��288��6E   $��58��6Q   $��Segoe UI Black)�6t   ���	#FFF8568D���#252525��6]   Z� 6�   �� 52      -   5        $��
REZERWACJE��52      $	��256��6O   ���	#FF602DC8$��Segoe UI Black)�6�   ���#252525$��58��6�   ��6[   Z� 6�   53      �� 54      $ 
Horizontal=�6   $��1��62   $ 0.482,9.153&�6B   $��
93,84,0,10q�6f   $
��Left=�6z   $	��544��6�   .+�55      �� -   5        $��RECENZJE��55      $	��288��6I   ���	#FFF05541���#252525$��Segoe UI Black)�6�   $��56��6�   ��6l   Z� 6�   �� 56      -	   5        $��PŁATNOŚCI��56      $��0q�6H   $	��255��6S   $��Segoe UI Black)�6v   ���#252525���	#FF16A570$��56��6�   ��6_   Z� 6�   57      �� 59      -
   5        $��	LOGOWANIE��59      $��637,23,9,10q�6D   $��1��6Y   $ -2.387,2.77&�6i   ���	#FFFBBE53���#252525y�5:      �� *IsLogoutVisible+# [  ��6r   Z� 6�   5<      5=      5>      