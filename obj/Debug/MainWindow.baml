   M S B A M L   `   `   `  ����E  AEscapeRoom, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:EscapeRoom.ViewModelsEscapeRoom.ViewModels  5   	   3clr-namespace:EscapeRoom.ViewsEscapeRoom.Views  5   	   !    EscapeRoom.Views.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2vm#clr-namespace:EscapeRoom.ViewModels     5   	   0viewsclr-namespace:EscapeRoom.Views     5   	   -   5          � Title$  Escape Room - Panel��5	   	   $
��1000��5
   	   $	��400��6    � WindowStartupLocation$ CenterScreen=�5   	   ���#252525
�������$��Segoe UI Black)�5   	    � DataContext 5      )   #EscapeRoom.ViewModels.MainViewModel  5   
    � 	Resources 5         BoolToVisibilityConverter��%   &	        �� 5   
   6'   5      .��5      ]� .+�5   
   o� $	��Top=�6   ���#333333
�������.@�5      m� $��_Plik��6   .@�5      m� -   5        $��	_Wyjście��5      6-   5      m� 5      $��_Widok��6   .@�5      m� -   5        $��_Pełny ekran��5      61   5       m� 5!      $��_Pomoc��6   .@�5"      m� -   5        $��_O programie��5"      61   5#      5$      �� 5&   
   -   5         ��Name$ MainContentControl��5&      $	��Top=�5'      $��Stretch=�5(      $��Stretch=�5)      5*      � 5,   
   $��Bottom=�6   ���#252525$	��155��6<   $	��848��6I    � ColumnDefinitions 5-      �� 5.      $
��Auto �6#   $��46.907��60   �� 5/      �� 50      $
��Auto �6#   .+�53      �� $
��Bold"�54      $	��Top=�55      $��
0,73,742,0q�56      $��1��57      $��22��58       J� RenderTransformOrigin$ 	1.189,0.9&�59      ��53      �� *CurrentUserName+y�5:      �� *
IsLoggedIn+ �� 	Converter# [  �� 5<       �� Orientation$ 
Horizontal=�6   $��1��5=      $��
93,0,99,51q�5>      $ 0.499,1.214&�5?      .+�5@      �� -   5        $��POKOJE��5@      $	��288��5B      $��58��5C      $��Segoe UI Black)�5E      ���	#FFF8568D���#252525��5D      Z� 5G      �� 5H      -   5        $��
REZERWACJE��5H      $	��256��5J      ���	#FF602DC8$��Segoe UI Black)�5M      ���#252525$��58��5O      ��5K      Z� y�5P      �� *
IsLoggedIn+# [  5Q      �� 5S      $ 
Horizontal=�6   $��1��5T      $ 0.482,9.153&�5U      $��
93,84,0,10q�5V      $
��Left=�5W      $	��544��5X      .+�5Y      �� -   5        $��KONTAKT��5Y      $	��288��5[      ���	#FFF05541���#252525$��Segoe UI Black)�5_      $��56��5`      ��5]      Z� 5`      �� 5a      -	   5        	 ��Name$	 UserAccountSettingsButton��5a      $��USTAWIENIA KONTA��5b      $	��255��5d      $��Segoe UI Black)�5f      ���#252525���	#FF16A570$��56��5i      ��5e      Z� y�5j      �� *
IsLoggedIn+# [  5k      �� 5m      -
   5        $��	LOGOWANIE��5m      $��637,23,9,71q�5o      $��1��5p      $ -2.387,2.77&�5q      ���	#FFFBBE53���#252525y�5r      �� *IsLogoutVisible+# [  ��5s      Z� 5u      �� 5w      -   5        $��WYLOGUJ��5w      $��637,84,9,10q�5y      $��1��5z      $ -2.387,2.77&�5{      ���#FF6B73���#252525y�5|      �� *
IsLoggedIn+# [  ��5}      Z� 5      �� 5�      -   5        $��ADMIN��5�      $��637,23,9,76q�5�      $��1��5�      $ -2.387,2.77&�5�      ���#FF6B73���#252525y�5�      �� *	IsAdmin+# [  ��5�      Z� 5�      5�      5�      5�      