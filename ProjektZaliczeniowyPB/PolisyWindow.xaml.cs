using System;
using System.Linq;
using System.Windows;

// Piotr Bacior - 15 722 WSEI Kraków

namespace ProjektZaliczeniowyPB
{
    /// <summary>
    /// Logika interakcji dla okna zarządzania polisami (PolisyWindow.xaml)
    /// </summary>
    public partial class PolisyWindow : Window
    {
        private ProjektZaliczeniowyBazaSamochodowEntities db = new ProjektZaliczeniowyBazaSamochodowEntities();
        private Polisy wybranaPolisa;

        /// <summary>
        /// Konstruktor - inicjalizacja komponentów i załadowanie danych.
        /// </summary>
        public PolisyWindow()
        {
            InitializeComponent();
            WczytajDane();
            WczytajComboBoxy();
        }

        /// <summary>
        /// Wczytuje listę polis z bazy danych.
        /// </summary>
        private void WczytajDane()
        {
            dgPolisy.ItemsSource = db.Polisy.ToList();
        }

        /// <summary>
        /// Ładuje dane zakupów do ComboBoxa.
        /// </summary>
        private void WczytajComboBoxy()
        {
            cbZakup.ItemsSource = db.Zakupy.ToList();
        }

        /// <summary>
        /// Czyści formularz.
        /// </summary>
        private void CzyscPola()
        {
            cbZakup.SelectedItem = null;
            dpRozpoczecie.SelectedDate = null;
            dpZakonczenie.SelectedDate = null;
            wybranaPolisa = null;
        }

        /// <summary>
        /// Wczytuje dane zaznaczonej polisy do formularza.
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
        /// Dodaje nową polisę.
        /// </summary>
        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {
            if (!WalidujDane(out Zakupy zakup, out DateTime start, out DateTime end)) return;

            try
            {
                var nowa = new Polisy
                {
                    ZakupID = zakup.ZakupID,
                    DataRozpoczecia = start,
                    DataZakonczenia = end
                };

                db.Polisy.Add(nowa);
                db.SaveChanges();
                WczytajDane();
                CzyscPola();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas dodawania polisy:\n" + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Edytuje wybraną polisę.
        /// </summary>
        private void BtnEdytuj_Click(object sender, RoutedEventArgs e)
        {
            if (wybranaPolisa == null)
            {
                MessageBox.Show("Wybierz polisę z listy.", "Brak wyboru", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!WalidujDane(out Zakupy zakup, out DateTime start, out DateTime end)) return;

            try
            {
                var polisa = db.Polisy.Find(wybranaPolisa.PolisaID);
                polisa.ZakupID = zakup.ZakupID;
                polisa.DataRozpoczecia = start;
                polisa.DataZakonczenia = end;

                db.SaveChanges();
                WczytajDane();
                CzyscPola();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas edycji polisy:\n" + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Usuwa zaznaczoną polisę.
        /// </summary>
        private void BtnUsun_Click(object sender, RoutedEventArgs e)
        {
            if (wybranaPolisa == null)
            {
                MessageBox.Show("Wybierz polisę do usunięcia.", "Brak wyboru", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var usun = db.Polisy.Find(wybranaPolisa.PolisaID);
                db.Polisy.Remove(usun);
                db.SaveChanges();
                WczytajDane();
                CzyscPola();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas usuwania polisy:\n" + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Odświeża dane w tabeli i czyści formularz.
        /// </summary>
        private void BtnOdswiez_Click(object sender, RoutedEventArgs e)
        {
            WczytajDane();
            CzyscPola();
        }

        /// <summary>
        /// Waliduje dane formularza – sprawdza pola ComboBox i daty.
        /// </summary>
        private bool WalidujDane(out Zakupy zakup, out DateTime start, out DateTime end)
        {
            zakup = cbZakup.SelectedItem as Zakupy;
            start = dpRozpoczecie.SelectedDate ?? DateTime.MinValue;
            end = dpZakonczenie.SelectedDate ?? DateTime.MinValue;

            if (zakup == null || start == DateTime.MinValue || end == DateTime.MinValue)
            {
                MessageBox.Show("Uzupełnij wszystkie dane: zakup i obie daty.", "Błąd danych", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (start > end)
            {
                MessageBox.Show("Data zakończenia nie może być przed datą rozpoczęcia.", "Błąd daty", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
    }
}
