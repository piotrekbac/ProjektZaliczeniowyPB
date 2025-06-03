// Piotr Bacior - 15 722 WSEI Kraków

using System;
using System.Linq;
using System.Windows;

namespace ProjektZaliczeniowyPB
{
    /// <summary>
    /// Okno WPF do zarządzania polisami.
    /// Pozwala na przeglądanie, dodawanie, edycję, usuwanie i odświeżanie danych polis.
    /// </summary>
    public partial class PolisyWindow : Window
    {
        // Kontekst bazy danych Entity Framework do obsługi operacji na bazie
        private ProjektZaliczeniowyBazaSamochodowEntities db = new ProjektZaliczeniowyBazaSamochodowEntities();

        // Referencja do aktualnie wybranej polisy (dla edycji/usuwania)
        private Polisy wybranaPolisa;

        /// <summary>
        /// Konstruktor okna — inicjalizuje komponenty, ładuje dane polis i wypełnia ComboBox z zakupami.
        /// </summary>
        public PolisyWindow()
        {
            InitializeComponent();
            WczytajDane();
            WczytajComboBoxy();
        }

        /// <summary>
        /// Wczytuje wszystkie polisy z bazy i ustawia jako źródło danych dla DataGrid.
        /// </summary>
        private void WczytajDane()
        {
            dgPolisy.ItemsSource = db.Polisy.ToList();
        }

        /// <summary>
        /// Wczytuje listę zakupów z bazy i ustawia jako źródło danych dla ComboBoxa.
        /// </summary>
        private void WczytajComboBoxy()
        {
            cbZakup.ItemsSource = db.Zakupy.ToList();
        }

        /// <summary>
        /// Czyści formularz – resetuje pola oraz wybór polisy.
        /// </summary>
        private void CzyscPola()
        {
            cbZakup.SelectedItem = null;
            dpRozpoczecie.SelectedDate = null;
            dpZakonczenie.SelectedDate = null;
            wybranaPolisa = null;
        }

        /// <summary>
        /// Obsługuje zaznaczenie polisy w tabeli (DataGrid).
        /// Ładuje dane wybranej polisy do formularza edycji.
        /// </summary>
        private void dgPolisy_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (dgPolisy.SelectedItem is Polisy p)
            {
                wybranaPolisa = p;
                cbZakup.SelectedItem = db.Zakupy.Find(p.ZakupID);
                dpRozpoczecie.SelectedDate = p.DataRozpoczecia;
                dpZakonczenie.SelectedDate = p.DataZakonczenia;
            }
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku "Dodaj".
        /// Tworzy nową polisę na podstawie danych z formularza i zapisuje ją w bazie danych.
        /// Po sukcesie odświeża tabelę i czyści formularz.
        /// </summary>
        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {
            // Walidacja danych – nie pozwala dodać niekompletnych lub błędnych danych
            if (!WalidujDane(out Zakupy zakup, out DateTime start, out DateTime end)) return;

            try
            {
                // Tworzenie nowego obiektu Polisy na podstawie danych z formularza
                var nowa = new Polisy
                {
                    ZakupID = zakup.ZakupID,
                    DataRozpoczecia = start,
                    DataZakonczenia = end
                };

                // Dodanie polisy do bazy i zapisanie zmian
                db.Polisy.Add(nowa);
                db.SaveChanges();
                WczytajDane();
                CzyscPola();
            }
            catch (Exception ex)
            {
                // Obsługa błędów podczas dodawania nowej polisy
                MessageBox.Show("Błąd podczas dodawania polisy:\n" + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku "Edytuj".
        /// Aktualizuje dane wybranej polisy na podstawie formularza i zapisuje zmiany w bazie.
        /// Po sukcesie odświeża tabelę i czyści formularz.
        /// </summary>
        private void BtnEdytuj_Click(object sender, RoutedEventArgs e)
        {
            // Sprawdzenie czy polisa została wybrana
            if (wybranaPolisa == null)
            {
                MessageBox.Show("Wybierz polisę z listy.", "Brak wyboru", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Walidacja danych
            if (!WalidujDane(out Zakupy zakup, out DateTime start, out DateTime end)) return;

            try
            {
                // Pobranie polisy z bazy po ID i aktualizacja jej danych
                var polisa = db.Polisy.Find(wybranaPolisa.PolisaID);
                polisa.ZakupID = zakup.ZakupID;
                polisa.DataRozpoczecia = start;
                polisa.DataZakonczenia = end;

                // Zapisanie zmian w bazie
                db.SaveChanges();
                WczytajDane();
                CzyscPola();
            }
            catch (Exception ex)
            {
                // Obsługa błędów podczas edycji polisy
                MessageBox.Show("Błąd podczas edycji polisy:\n" + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku "Usuń".
        /// Usuwa wybraną polisę z bazy danych.
        /// Po sukcesie odświeża tabelę i czyści formularz.
        /// </summary>
        private void BtnUsun_Click(object sender, RoutedEventArgs e)
        {
            // Sprawdzenie czy polisa została wybrana
            if (wybranaPolisa == null)
            {
                MessageBox.Show("Wybierz polisę do usunięcia.", "Brak wyboru", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Wyszukanie polisy w bazie po ID i usunięcie jej
                var usun = db.Polisy.Find(wybranaPolisa.PolisaID);
                db.Polisy.Remove(usun);
                db.SaveChanges();
                WczytajDane();
                CzyscPola();
            }
            catch (Exception ex)
            {
                // Obsługa błędów podczas usuwania polisy
                MessageBox.Show("Błąd podczas usuwania polisy:\n" + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
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
        /// Waliduje dane formularza polisy.
        /// Sprawdza czy wybrano zakup i obie daty oraz czy daty są zgodne (rozpoczęcie <= zakończenie).
        /// </summary>
        /// <param name="zakup">Zwracany wybrany zakup.</param>
        /// <param name="start">Zwracana data rozpoczęcia.</param>
        /// <param name="end">Zwracana data zakończenia.</param>
        /// <returns>True jeśli dane są poprawne, w przeciwnym razie false.</returns>
        private bool WalidujDane(out Zakupy zakup, out DateTime start, out DateTime end)
        {
            zakup = cbZakup.SelectedItem as Zakupy;
            start = dpRozpoczecie.SelectedDate ?? DateTime.MinValue;
            end = dpZakonczenie.SelectedDate ?? DateTime.MinValue;

            // Sprawdzenie czy wszystkie wymagane pola są uzupełnione
            if (zakup == null || start == DateTime.MinValue || end == DateTime.MinValue)
            {
                MessageBox.Show("Uzupełnij wszystkie dane: zakup i obie daty.", "Błąd danych", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // Sprawdzenie poprawności zakresu dat
            if (start > end)
            {
                MessageBox.Show("Data zakończenia nie może być przed datą rozpoczęcia.", "Błąd daty", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
    }
}