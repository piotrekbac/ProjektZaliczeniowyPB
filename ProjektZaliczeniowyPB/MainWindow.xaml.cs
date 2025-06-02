using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

//Piotr Bacior - 15 722 WSEI Kraków

namespace ProjektZaliczeniowyPB
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Definiujemy konstruktor klasy MainWindow, który jest wywoływany podczas tworzenia obiektu tej klasy, czyli przy uruchomieniu głównego okna aplikacji 
        public MainWindow()
        {
            //Inicjalizujemy wszystkie komponenty graficzne, zdefiniowane w pliku XAML, dla tego okna
            InitializeComponent();
        }

        //Teraz przechodzimy do obsługi zdarzeń, czyli metod, które będą wywoływane w odpowiedzi na interakcje użytkownika z interfejsem graficznym
        private void BtnKlienci_Click(object sender, RoutedEventArgs e)
        {
            //Tworzy nowe okno do zarządzania klientami
            KlienciWindow okno = new KlienciWindow();

            //Wyświetlamy okno w trybie modalnym, czyli blokujemy interakcję z głównym oknem, dopóki to okno nie zostanie zamknięte
            okno.ShowDialog();
        }
    }
}
