   M S B A M L   `   `   `  ����G  Cpresentacion, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:capa_presentacioncapa_presentacion  5   	       capa_presentacion.Login   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:capa_presentacion     5   	     � Title$  LOGIN��5   	   $��Center=�6   $��397.025��64   $��710.706��6E    � UseLayoutRounding .  � WindowStartupLocation$ CenterScreen=�5	   &    � 
ResizeMode$ NoResize=�6K    � Icon$ 	icono.png��6a    � 	Resources 5
       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $ Diccionario.xaml?�6%   %    5      ��5      �� 5   
    �� ImageSource$ fondologin.jpg��6   	 j� Stretch$	 UniformToFill=�62   
 J� Effect
 5      3  -System.Windows.Media.Effects.DropShadowEffect  5   
   .��5      � $��0,0,0,0q�6   ��5   
   �� 5       �� Color$ Black��6    �� Opacity$	 0.5Q�6,   .+�5   
   �� $��Usuario��6    	  label#��[  6"   $
��Left=�6A   $��
149,62,0,0q�6\   $	��Top=�6p   $��18(�6�   �� 5   
   $��Contraseña��6   #��[  6%   $
��Left=�6D   $��149,145,0,0q�6_   $	��Top=�6t   $��Microsoft Sans Serif)�6�   $��18(�6�   �� 5   
   -   5         ��Name$ 	tbUsuario��5        textBox#��[ 6%   $
��Left=�6F   $��30��6a   $��
149,92,0,0q�6m    �� TextWrapping$
 Wrap=�6�   $	��Top=�6�   $	��410��6�   $��18(�6�   G� 5   
   -   5         G�Name$ tbContra��5        passBox#��[ 6(   $
��Left=�6I   $��30��6d   $��149,175,0,0q�6p   $	��Top=�6�   $	��410��6�   $��18(�6�   �� 5   
   -   5         ��Name$ btnLogin��5      $��ACCEDER��6#    
 button#��[ 6L   $��140,239,128,73.2q�6l   $	��410��6�   $��40��6�   $��18(�6�   �� 5    
   -   5         ��Name$ 	statusBar��5       $
��Left=�6'   $��29��6B   $��	0,332,0,0q�6N   $	��Top=�6a   $	��707��6y    J� RenderTransformOrigin$ 0.498,-0.145&�6�   ��5!      �� 5"      $	 Red��6"   $ 0Q�6.   .@�5$      �� -   5         ��Name$ lblInfo��5$      $
��Left=�6%   $��23��6@   $	��Top=�6L   $	��697��6e   
�������$��10(�6�   $�� ��6�   ��6q   Z� 6�   5%      5&      5'      