   M S B A M L   `   `   `  ����  Asteroids_Rebirth"    Asteroids_Rebirth.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     6F   -   5          � Icon$  logo.ico��5       � Title$ Asteroids-Rebirth��5   	   $	��480��6#   $	��640��60   ���	#FF008F88 � 
ResizeMode$ NoResize=�6T    � 	Resources 5         BlackAndWhite �� Property �� Value��%�   &	        �� 5	   
   .�5
      ��  � ��$ Arial)�63   �� 5       � ��
 �   �63   �� 5       � ��
 �����63   5      5      .��5      � -   5         �Name$
 grid��5      $��0,0,0,0q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   6,   �� 5      $��* �6   6)   .+�5   
   � -   5        $ Record��5      $
��Left=�6   $	��452��6:   $��2��6G   $	��Top=�6X   $	��634��6p   .+�5      �� -   5        $��No��5      $
��Left=�6"   $��216,298,0,0q�6=   #��[  6R   
���2͚�$	��Top=�6�   $��70��6�   $��22��6�   ��6�   Z� �� 5      -   5         ��Name$ 	YesButton��5      $	��Yes��6(   $
��Left=�66   $��394,298,0,0q�6Q   #��[  6f   
���2͚�$	��Top=�6�   $��70��6�   $��22��6�   {�.  	 J� Opacity$		 0.3Q�6  ��6�   Z� 6  �� 5      -   5        
 ��Name$
 nameOfPlayer��5      $
��Left=�6-   $��3q�6H   #��[  6\   $��32��6�   $��273,196,0,0q�6�    �� TextWrapping$
 Wrap=�6�   $	��Top=�6�   $	��170��6�   
���2͚�$��14(�6%  ��6�   Z� .��63  ��5      �� 5       �� EndPoint$
 0,20&�6*    � MappingMode$ Absolute=�6:    �� 
StartPoint$	 0,0&�6Q   .��5      �  � Color$ 	#FFABADB3��6'    � Offset$
 0.05Q�69   � 5      $ 	#FFE2E3EA��6'   $
 0.07Q�69   � 5      $ 	#FF85A430��6'   $ 1Q�69   5      .��5    '   5"      �� 5#      -   5         ��Name$ 	ScoreText��5#      $��Your score:��6'   $
��Left=�6=   $��
240,70,0,0q�6X   $	��Top=�6l   $��98��6�   $	��322��6�   $��30(�6�   
���2͚�6�   �� 5$      $��
Your name:��6   $
��Left=�6)   $��137,196,0,0q�6D   $	��Top=�6Y   $��32��6q   $	��101��6}   
���2͚�$��16(�6�   �� 5%      $%��Do you want to add your record?��6   $
��Left=�6>   $��240,252,0,0q�6Y   $	��Top=�6n   $	��185��6�   
���2͚�6�   5&      (� 5'   
   -   5         (�Name$ gameViewBox��5'      $��1��6'    J� OpacityMask �	#FF9C8080.��5)      �� -	   5         ��Name$ canvas��5)      $��75��6%   $��54��60   6<   5*      �� 5+   
   -
   5        $ Information��5+      $��Score: 0  Lives:3��6%   $
��Left=�6A   $��	10,10,0,0q�6\   
���2͚�$	��Top=�6�   $��54��6�   $	��302��6�   $��20(�6�   $��2��6�   ��6�   Z� 6�   � 5-   
   -   5        $
 menu��5-      $
��Left=�6   $	��452��68   $��2��6E   $	��Top=�6V   $	��634��6n   .+�5.      �� -   5        $��
Start game��5.      $
��Left=�6*   $��495,152,0,0q�6E   $	��Top=�6Z   $��75��6r   #��[  6}   6�   �� 5/      -   5        $��Controls��5/      $
��Left=�6(   $��495,210,0,0q�6C   $	��Top=�6X   $��75��6p   #��[  6{   6�   �� 50      -   5        $
��Exit��50      $
��Left=�6$   $	��Top=�6?   $��75��6W   $��495,238,0,0q�6b   #��[  6w   6�   �� 51      -   5        $��Records��51      $
��Left=�6'   $��495,181,0,0q�6B   $	��Top=�6W   $��75��6o   #��[  6z   6�   52      53      54      