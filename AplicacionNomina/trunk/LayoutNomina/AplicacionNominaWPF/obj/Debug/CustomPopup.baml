   M S B A M L   `   `   `  ????  AplicacionNominaWPF%    AplicacionNominaWPF.CustomPopup   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          ?Name$  CustomPopupWindow??5   	    ? Title$ CustomPopup??5   	    ? WindowStartupLocation$ CenterScreen=?6   $	??200??6B   $	??500??6O    ? 
ResizeMode$ NoResize=?5   
    ? Icon$! Images/logoBanescoColor.png??6    .??5      ? ??5	   
   ?? 5
       ?? ImageSource$" Images/logoBanescoGrande.png??6    ?? 	Transform 5      V? 5      .~?5      T?  T? X$	 350Q?6-    T? Y$	 -20Q?65   6?   ?? 5      	 ?? ScaleX$		 0.7Q?6)   
 ?? ScaleY$	
 1.4Q?66   6E   5      5       ? ColumnDefinitions 5   
   ?? 5      $
??130* ?6   ?? 5      $
??265* ?6   ?? 5      $
??197* ?6    ? RowDefinitions 5   
   ?? 5      $	??53* ?6   ?? 5      $	??67* ?6   ?? 5      $	??52* ?6   .+?5   
   ?? $??30,0,0,0q?6   $??1??6&   $??0??67   $??1??6G   ?? 5       -   5         ??Name$ imgIcono??5       $??Images/iconError.png??6&   $??75??6D   $??66??6O   6]   5!      ?? 5#   
   -   5         ??Name$ 
lblMensaje??5#   I   $??15(?6   $??	0,17,11,0q?6"   $
??Wrap=?65   $??2??6]   $??1??6q   $??1??6?   )'Archivo nómina o proveedores exportado5%      ?? 5'   
   -   5         ??Name$ 
btnAceptar??5'   $   
???????$??
66,0,11,10q?6S   $??2??6g   $??2??6w   $
??Auto??6?   $??30??6?   $??Bottom=?6?   .??5(      ?? 	Aceptar6!   5)      5+      5,      