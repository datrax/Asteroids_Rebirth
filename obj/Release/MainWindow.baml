   M S B A M L   `   `   `  ����   Asteroids_Rebirth"    Asteroids_Rebirth.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -     � Icon$  logo.ico�� � Title$ Asteroids-Rebirth��$	��480��$	��640�����	#FF008F88 � 
ResizeMode$ NoResize=� � 	Resources    BlackAndWhite �� Property �� Value��%[   &	        �� .���  � ��$ Arial)���  � ��
 �   ���  � ��
 �����.��� -    �Name$
 grid��$��0,0,0,0q� � RowDefinitions �� $
��Auto ��� $��* �.+�� -   $ Record��$
��Left=�$	��452��$��2��$	��Top=�$	��634��.+��� -   $��No��$
��Left=�$��216,298,0,0q�#��[  
���2͚�$	��Top=�$��70��$��22����Z� �� -    ��Name$ 	YesButton��$	��Yes��$
��Left=�$��394,298,0,0q�#��[  
���2͚�$	��Top=�$��70��$��22��{�.  	 J� Opacity$		 0.3Q���Z� �� -   
 ��Name$
 nameOfPlayer��$
��Left=�$��3q�#��[  $��32��$��273,196,0,0q� �� TextWrapping$
 Wrap=�$	��Top=�$	��170��
���2͚�$��14(���Z� .������  �� EndPoint$
 0,20&� � MappingMode$ Absolute=� �� 
StartPoint$	 0,0&�.���  � Color$ 	#FFABADB3�� � Offset$
 0.05Q�� $ 	#FFE2E3EA��$
 0.07Q�� $ 	#FF85A430��$ 1Q�.���� -    ��Name$ 	ScoreText��$��Your score:��$
��Left=�$��
240,70,0,0q�$	��Top=�$��98��$	��322��$��30(�
���2͚��� $��
Your name:��$
��Left=�$��137,196,0,0q�$	��Top=�$��32��$	��101��
���2͚�$��16(��� $%��Do you want to add your record?��$
��Left=�$��240,252,0,0q�$	��Top=�$	��185��
���2͚�(� -    (�Name$ gameViewBox��$��1�� J� OpacityMask �	#FF9C8080.���� -	    ��Name$ canvas��$��75��$��54���� -
   $ Information��$��Score: 0  Lives:3��$
��Left=�$��	10,10,0,0q�
���2͚�$	��Top=�$��54��$	��302��$��20(�$��2����Z� � -   $
 menu��$
��Left=�$	��452��$��2��$	��Top=�$	��634��.+��� -   $��
Start game��$
��Left=�$��495,152,0,0q�$	��Top=�$��75��#��[  �� -   $��Controls��$
��Left=�$��495,210,0,0q�$	��Top=�$��75��#��[  �� -   $
��Exit��$
��Left=�$	��Top=�$��75��$��495,238,0,0q�#��[  �� -   $��Records��$
��Left=�$��495,181,0,0q�$	��Top=�$��75��#��[  