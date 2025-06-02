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
using System.Windows.Shapes;

//Piotr Bacior - 15 722 WSEI Kraków

namespace ProjektZaliczeniowyPB
{
    /// <summary>
    /// Logika interakcji dla okna zarządzania klientami (KlienciWindow.xaml)
    /// </summary>
    public partial class KlienciWindow : Window
    {
        //Tworzymy obiekt bazy danych, dzięki któremu mamy dostęp do tabel oraz operacji na bazie
        private ProjektZaliczeniowyBazaSamochodowEntities db = new ProjektZaliczeniowyBazaSamochodowEntities();

        //Zmienna do przechowywania aktualnie wybranego klienta
        private Klienci wybranyKlient;

        //Konstruktor okna - wywołuje się przy otwarciu tego okna
        public KlienciWindow()
        {
            InitializeComponent(); //Inicjalizuje wszystkie elementy graficzne zdefiniowane w pliku XAML
            WczytajDane();         //Ładuje dane klientów do tabeli przy starcie okna
        }

        //Metoda do pobierania i wyświetlania wszystkich klientów w DataGrid
        private void WczytajDane()
        {
            //Przypisuje listę klientów z bazy jako źródło danych do DataGrid (dgKlienci)
            dgKlienci.ItemsSource = db.Klienci.ToList();
        }

        private void dgKlienci_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // TODO: obsługa zaznaczenia klienta
        }


        //Obsługa kliknięcia przycisku "Dodaj"
        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {
            //Sprawdzamy czy pola Imię i Nazwisko nie są puste (wymagane dane)
            if (!string.IsNullOrWhiteSpace(txtImie.Text) && !string.IsNullOrWhiteSpace(txtNazwisko.Text))
            {
                //Tworzymy nowy obiekt klienta na podstawie danych z pól tekstowych
                var nowy = new Klienci
                {
                    Imie = txtImie.Text,
                    Nazwisko = txtNazwisko.Text,
                    Email = txtEmail.Text,
                    NumerTelefonu = txtTelefon.Text
                };

                //Dodajemy nowego klienta do bazy i zapisujemy zmiany
                db.Klienci.Add(nowy);
                db.SaveChanges();

                //Odświeżamy listę klientów i czyścimy pola tekstowe po dodaniu
                WczytajDane();
                CzyscPola();
            }
        }

        //Obsługa kliknięcia przycisku "Edytuj"
        private void BtnEdytuj_Click(object sender, RoutedEventArgs e)
        {
            //Sprawdzamy czy jakiś klient został wybrany w tabeli
            if (dgKlienci.SelectedItem is Klienci k)
            {
                //Pobieramy klienta z bazy po ID i aktualizujemy jego dane na podstawie pól tekstowych
                wybranyKlient = db.Klienci.Find(k.KlientID);
                wybranyKlient.Imie = txtImie.Text;
                wybranyKlient.Nazwisko = txtNazwisko.Text;
                wybranyKlient.Email = txtEmail.Text;
                wybranyKlient.NumerTelefonu = txtTelefon.Text;

                //Zapisujemy zmiany w bazie
                db.SaveChanges();

                //Odświeżamy widok oraz czyścimy pola
                WczytajDane();
                CzyscPola();
            }
        }

        //Obsługa kliknięcia przycisku "Usuń"
        private void BtnUsun_Click(object sender, RoutedEventArgs e)
        {
            //Sprawdzamy czy jest wybrany klient do usunięcia
            if (dgKlienci.SelectedItem is Klienci k)
            {
                //Szukamy klienta po ID, usuwamy go z bazy i zapisujemy zmiany
                var doUsuniecia = db.Klienci.Find(k.KlientID);
                db.Klienci.Remove(doUsuniecia);
                db.SaveChanges();

                //Odświeżamy listę oraz czyścimy pola
                WczytajDane();
                CzyscPola();
            }
        }

        //Obsługa kliknięcia przycisku "Odśwież"
        private void BtnOdswiez_Click(object sender, RoutedEventArgs e)
        {
            //Ponownie pobieramy dane z bazy i wyświetlamy w DataGrid
            WczytajDane();
        }

        //Metoda pomocnicza do czyszczenia pól tekstowych w oknie po wykonaniu operacji
        private void CzyscPola()
        {
            txtImie.Text = "";
            txtNazwisko.Text = "";
            txtEmail.Text = "";
            txtTelefon.Text = "";
        }
    }
}
