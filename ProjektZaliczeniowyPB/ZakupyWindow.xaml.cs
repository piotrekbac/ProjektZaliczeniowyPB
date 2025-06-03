// Piotr Bacior - 15 722 WSEI Kraków

using System;
using System.Linq;
using System.Windows;

namespace ProjektZaliczeniowyPB
{
    /// <summary>
    /// Okno WPF do zarządzania zakupami samochodów.
    /// Pozwala na przeglądanie, dodawanie, edycję, usuwanie i odświeżanie danych zakupów.
    /// </summary>
    public partial class ZakupyWindow : Window
    {
        // Kontekst bazy danych Entity Framework do obsługi operacji na bazie
        private ProjektZaliczeniowyBazaSamochodowEntities db = new ProjektZaliczeniowyBazaSamochodowEntities();

        // Referencja do aktualnie wybranego zakupu (dla edycji/usuwania)
        private Zakupy wybranyZakup;

        /// <summary>
        /// Konstruktor okna — inicjalizuje komponenty i ładuje dane zakupów oraz listy do ComboBoxów.
        /// </summary>
        public ZakupyWindow()
        {
            InitializeComponent();
            WczytajDane();
            WczytajComboBoxy();
        }

        /// <summary>
        /// Wczytuje wszystkie zakupy z bazy i ustawia jako źródło danych dla DataGrid.
        /// </summary>
        private void WczytajDane()
        {
            dgZakupy.ItemsSource = db.Zakupy.ToList();
        }

        /// <summary>
        /// Wczytuje listy klientów, pracowników i samochodów z bazy do odpowiednich ComboBoxów.
        /// </summary>
        private void WczytajComboBoxy()
        {
            cbKlient.ItemsSource = db.Klienci.ToList();
            cbPracownik.ItemsSource = db.Pracownicy.ToList();
            cbSamochod.ItemsSource = db.Samochody.ToList();
        }

        /// <summary>
        /// Czyści wszystkie pola formularza oraz resetuje wybór zakupu.
        /// </summary>
        private void CzyscPola()
        {
            cbKlient.SelectedItem = null;
            cbPracownik.SelectedItem = null;
            cbSamochod.SelectedItem = null;
            dpDataZakupu.SelectedDate = null;
            wybranyZakup = null;
        }

        /// <summary>
        /// Obsługuje zaznaczenie zakupu w tabeli (DataGrid).
        /// Ładuje dane wybranego zakupu do formularza edycji.
        /// </summary>
        private void dgZakupy_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (dgZakupy.SelectedItem is Zakupy z)
            {
                wybranyZakup = z;
                cbKlient.SelectedItem = db.Klienci.Find(z.KlientID);
                cbPracownik.SelectedItem = db.Pracownicy.Find(z.PracownikID);
                cbSamochod.SelectedItem = db.Samochody.Find(z.SamochodID);
                dpDataZakupu.SelectedDate = z.DataZakupu;
            }
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku "Dodaj".
        /// Tworzy nowy zakup na podstawie danych z formularza i zapisuje go w bazie danych.
        /// Po sukcesie odświeża tabelę i czyści formularz.
        /// </summary>
        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {
            // Walidacja danych — nie pozwala dodać niekompletnych lub błędnych danych
            if (!WalidujDane(out Klienci klient, out Pracownicy pracownik, out Samochody samochod, out DateTime data)) return;

            try
            {
                var zakup = new Zakupy
                {
                    KlientID = klient.KlientID,
                    PracownikID = pracownik.PracownikID,
                    SamochodID = samochod.SamochodID,
                    DataZakupu = data
                };

                db.Zakupy.Add(zakup);
                db.SaveChanges();
                WczytajDane();
                CzyscPola();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas dodawania zakupu:\n" + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku "Edytuj".
        /// Aktualizuje dane wybranego zakupu na podstawie formularza i zapisuje zmiany w bazie.
        /// Po sukcesie odświeża tabelę i czyści formularz.
        /// </summary>
        private void BtnEdytuj_Click(object sender, RoutedEventArgs e)
        {
            if (wybranyZakup == null)
            {
                MessageBox.Show("Wybierz rekord z listy do edycji.", "Brak wyboru", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!WalidujDane(out Klienci klient, out Pracownicy pracownik, out Samochody samochod, out DateTime data)) return;

            try
            {
                var zakup = db.Zakupy.Find(wybranyZakup.ZakupID);
                zakup.KlientID = klient.KlientID;
                zakup.PracownikID = pracownik.PracownikID;
                zakup.SamochodID = samochod.SamochodID;
                zakup.DataZakupu = data;

                db.SaveChanges();
                WczytajDane();
                CzyscPola();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas edycji zakupu:\n" + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku "Usuń".
        /// Usuwa wybrany zakup z bazy danych.
        /// Po sukcesie odświeża tabelę i czyści formularz.
        /// </summary>
        private void BtnUsun_Click(object sender, RoutedEventArgs e)
        {
            if (wybranyZakup == null)
            {
                MessageBox.Show("Wybierz zakup do usunięcia.", "Brak wyboru", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var zakup = db.Zakupy.Find(wybranyZakup.ZakupID);
                db.Zakupy.Remove(zakup);
                db.SaveChanges();
                WczytajDane();
                CzyscPola();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas usuwania zakupu:\n" + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
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
        /// Waliduje dane formularza zakupu.
        /// Sprawdza czy wybrano klienta, pracownika, samochód oraz datę zakupu.
        /// </summary>
        /// <param name="klient">Zwracany wybrany klient.</param>
        /// <param name="pracownik">Zwracany wybrany pracownik.</param>
        /// <param name="samochod">Zwracany wybrany samochód.</param>
        /// <param name="data">Zwracana data zakupu.</param>
        /// <returns>True jeśli dane są poprawne, w przeciwnym razie false.</returns>
        private bool WalidujDane(out Klienci klient, out Pracownicy pracownik, out Samochody samochod, out DateTime data)
        {
            klient = cbKlient.SelectedItem as Klienci;
            pracownik = cbPracownik.SelectedItem as Pracownicy;
            samochod = cbSamochod.SelectedItem as Samochody;
            data = dpDataZakupu.SelectedDate ?? DateTime.MinValue;

            if (klient == null || pracownik == null || samochod == null || data == DateTime.MinValue)
            {
                MessageBox.Show("Uzupełnij wszystkie pola formularza.", "Błąd danych", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
    }
}