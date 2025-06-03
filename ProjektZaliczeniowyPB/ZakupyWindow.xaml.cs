using System;
using System.Linq;
using System.Windows;

// Piotr Bacior - 15 722 WSEI Kraków

namespace ProjektZaliczeniowyPB
{
    /// <summary>
    /// Logika interakcji dla okna ZakupyWindow.xaml
    /// </summary>
    public partial class ZakupyWindow : Window
    {
        private ProjektZaliczeniowyBazaSamochodowEntities db = new ProjektZaliczeniowyBazaSamochodowEntities();
        private Zakupy wybranyZakup;

        /// <summary>
        /// Konstruktor - inicjalizacja komponentów i danych.
        /// </summary>
        public ZakupyWindow()
        {
            InitializeComponent();
            WczytajDane();
            WczytajComboBoxy();
        }

        /// <summary>
        /// Wczytuje listę zakupów z bazy.
        /// </summary>
        private void WczytajDane()
        {
            dgZakupy.ItemsSource = db.Zakupy.ToList();
        }

        /// <summary>
        /// Ładuje dane do ComboBoxów.
        /// </summary>
        private void WczytajComboBoxy()
        {
            cbKlient.ItemsSource = db.Klienci.ToList();
            cbPracownik.ItemsSource = db.Pracownicy.ToList();
            cbSamochod.ItemsSource = db.Samochody.ToList();
        }

        /// <summary>
        /// Czyści wszystkie pola formularza.
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
        /// Obsługuje zaznaczenie wiersza w DataGrid.
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
        /// Dodaje nowy zakup.
        /// </summary>
        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {
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
        /// Edytuje wybrany zakup.
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
        /// Usuwa wybrany zakup.
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
        /// Odświeża dane i czyści formularz.
        /// </summary>
        private void BtnOdswiez_Click(object sender, RoutedEventArgs e)
        {
            WczytajDane();
            CzyscPola();
        }

        /// <summary>
        /// Waliduje formularz – sprawdza poprawność danych.
        /// </summary>
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
