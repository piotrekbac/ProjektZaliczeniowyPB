// Piotr Bacior - 15 722 WSEI Kraków

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ProjektZaliczeniowyPB
{
    /// <summary>
    /// Okno WPF do zarządzania klientami.
    /// Pozwala na przeglądanie, dodawanie, edycję, usuwanie i odświeżanie danych klientów.
    /// </summary>
    public partial class KlienciWindow : Window
    {
        // Kontekst bazy danych Entity Framework do obsługi operacji na bazie
        private ProjektZaliczeniowyBazaSamochodowEntities db = new ProjektZaliczeniowyBazaSamochodowEntities();

        // Referencja do aktualnie wybranego klienta (dla edycji/usuwania)
        private Klienci wybranyKlient;

        /// <summary>
        /// Konstruktor okna — inicjalizuje komponenty i ładuje dane klientów do tabeli.
        /// </summary>
        public KlienciWindow()
        {
            InitializeComponent();
            WczytajDane();
        }

        /// <summary>
        /// Wczytuje dane klientów z bazy i ustawia jako źródło danych dla DataGrid.
        /// </summary>
        private void WczytajDane()
        {
            dgKlienci.ItemsSource = db.Klienci.ToList();
        }

        /// <summary>
        /// Czyści wszystkie pola formularza oraz resetuje wybór klienta.
        /// </summary>
        private void CzyscPola()
        {
            txtImie.Text = "";
            txtNazwisko.Text = "";
            txtEmail.Text = "";
            txtTelefon.Text = "";
            txtUlica.Text = "";
            txtNumerBudynku.Text = "";
            txtKodPocztowy.Text = "";
            txtMiejscowosc.Text = "";
            wybranyKlient = null;
        }

        /// <summary>
        /// Obsługuje zaznaczenie klienta w tabeli (DataGrid).
        /// Ładuje dane wybranego klienta do formularza edycji.
        /// </summary>
        private void dgKlienci_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgKlienci.SelectedItem is Klienci k)
            {
                wybranyKlient = k;
                txtImie.Text = k.Imie;
                txtNazwisko.Text = k.Nazwisko;
                txtEmail.Text = k.Email;
                txtTelefon.Text = k.NumerTelefonu;
                txtUlica.Text = k.Ulica;
                txtNumerBudynku.Text = k.NumerBudynku;
                txtKodPocztowy.Text = k.KodPocztowy;
                txtMiejscowosc.Text = k.Miejscowosc;
            }
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku "Dodaj".
        /// Tworzy nowego klienta na podstawie danych z formularza i zapisuje go w bazie danych.
        /// Po sukcesie odświeża tabelę i czyści formularz.
        /// </summary>
        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {
            // Walidacja danych — nie pozwala dodać niekompletnych lub błędnych danych
            if (!WalidujDane()) return;

            try
            {
                // Tworzenie nowego obiektu Klienci na podstawie danych z formularza
                var klient = new Klienci
                {
                    Imie = txtImie.Text,
                    Nazwisko = txtNazwisko.Text,
                    Email = txtEmail.Text,
                    NumerTelefonu = txtTelefon.Text,
                    Ulica = txtUlica.Text,
                    NumerBudynku = txtNumerBudynku.Text,
                    KodPocztowy = txtKodPocztowy.Text,
                    Miejscowosc = txtMiejscowosc.Text
                };

                // Dodanie klienta do bazy i zapisanie zmian
                db.Klienci.Add(klient);
                db.SaveChanges();
                WczytajDane();
                CzyscPola();
            }
            catch (Exception ex)
            {
                // Obsługa błędów podczas dodawania nowego klienta
                MessageBox.Show("Błąd podczas dodawania:\n" + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku "Edytuj".
        /// Aktualizuje dane wybranego klienta na podstawie formularza i zapisuje zmiany w bazie.
        /// Po sukcesie odświeża tabelę i czyści formularz.
        /// </summary>
        private void BtnEdytuj_Click(object sender, RoutedEventArgs e)
        {
            // Sprawdzenie czy klient został wybrany
            if (wybranyKlient == null)
            {
                MessageBox.Show("Wybierz klienta z listy.", "Brak wyboru", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Walidacja danych
            if (!WalidujDane()) return;

            try
            {
                // Pobranie klienta z bazy po ID i aktualizacja jego danych
                var klient = db.Klienci.Find(wybranyKlient.KlientID);
                klient.Imie = txtImie.Text;
                klient.Nazwisko = txtNazwisko.Text;
                klient.Email = txtEmail.Text;
                klient.NumerTelefonu = txtTelefon.Text;
                klient.Ulica = txtUlica.Text;
                klient.NumerBudynku = txtNumerBudynku.Text;
                klient.KodPocztowy = txtKodPocztowy.Text;
                klient.Miejscowosc = txtMiejscowosc.Text;

                // Zapisanie zmian w bazie
                db.SaveChanges();
                WczytajDane();
                CzyscPola();
            }
            catch (Exception ex)
            {
                // Obsługa błędów podczas edycji klienta
                MessageBox.Show("Błąd podczas edycji:\n" + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku "Usuń".
        /// Usuwa wybranego klienta z bazy danych.
        /// Po sukcesie odświeża tabelę i czyści formularz.
        /// </summary>
        private void BtnUsun_Click(object sender, RoutedEventArgs e)
        {
            // Sprawdzenie czy klient został wybrany
            if (wybranyKlient == null)
            {
                MessageBox.Show("Wybierz klienta do usunięcia.", "Brak wyboru", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Wyszukanie klienta w bazie po ID i usunięcie go
                var klient = db.Klienci.Find(wybranyKlient.KlientID);
                db.Klienci.Remove(klient);
                db.SaveChanges();
                WczytajDane();
                CzyscPola();
            }
            catch (Exception ex)
            {
                // Obsługa błędów podczas usuwania klienta
                MessageBox.Show("Błąd podczas usuwania:\n" + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku "Odśwież".
        /// Powtórnie wczytuje dane z bazy i czyści formularz.
        /// </summary>
        private void BtnOdswiez_Click(object sender, RoutedEventArgs e)
        {
            WczytajDane();
            CzyscPola();
        }

        /// <summary>
        /// Waliduje wymagane pola formularza klienta.
        /// Sprawdza, czy wymagane pola nie są puste oraz czy telefon ma 9 cyfr i zawiera tylko cyfry.
        /// </summary>
        /// <returns>True jeśli dane są poprawne, w przeciwnym razie false.</returns>
        private bool WalidujDane()
        {
            // Sprawdzenie czy wymagane pola (imię, nazwisko, email, telefon) są wypełnione
            if (string.IsNullOrWhiteSpace(txtImie.Text) ||
                string.IsNullOrWhiteSpace(txtNazwisko.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtTelefon.Text))
            {
                MessageBox.Show("Imię, nazwisko, e-mail i telefon są wymagane.", "Brak danych", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // Sprawdzenie formatu numeru telefonu (dokładnie 9 cyfr)
            if (txtTelefon.Text.Length != 9 || !txtTelefon.Text.All(char.IsDigit))
            {
                MessageBox.Show("Telefon musi mieć 9 cyfr.", "Błąd formatu", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
    }
}