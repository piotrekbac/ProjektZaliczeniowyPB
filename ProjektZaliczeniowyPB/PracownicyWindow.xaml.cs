using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ProjektZaliczeniowyPB
{
    /// <summary>
    /// Logika interakcji dla okna zarządzania pracownikami (PracownicyWindow.xaml)
    /// </summary>
    public partial class PracownicyWindow : Window
    {
        private ProjektZaliczeniowyBazaSamochodowEntities db = new ProjektZaliczeniowyBazaSamochodowEntities();
        private Pracownicy wybranyPracownik;

        public PracownicyWindow()
        {
            InitializeComponent();
            WczytajDane();
            WczytajComboBoxy();
        }

        /// <summary>
        /// Wczytuje dane pracowników do DataGrid.
        /// </summary>
        private void WczytajDane()
        {
            dgPracownicy.ItemsSource = db.Pracownicy.ToList();
        }

        /// <summary>
        /// Ładuje listę wydziałów do ComboBoxa.
        /// </summary>
        private void WczytajComboBoxy()
        {
            cbWydzial.ItemsSource = db.Wydzialy.ToList();
        }

        /// <summary>
        /// Czyści pola formularza.
        /// </summary>
        private void CzyscPola()
        {
            txtImie.Text = "";
            txtNazwisko.Text = "";
            txtEmail.Text = "";
            txtUlica.Text = "";
            txtNumerBudynku.Text = "";
            txtKodPocztowy.Text = "";
            txtMiejscowosc.Text = "";
            cbWydzial.SelectedItem = null;
            wybranyPracownik = null;
        }

        /// <summary>
        /// Obsługuje wybór wiersza z DataGrid i wypełnia formularz.
        /// </summary>
        private void dgPracownicy_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (dgPracownicy.SelectedItem is Pracownicy p)
            {
                wybranyPracownik = p;
                txtImie.Text = p.Imie;
                txtNazwisko.Text = p.Nazwisko;
                txtEmail.Text = p.Email;
                txtUlica.Text = p.Ulica;
                txtNumerBudynku.Text = p.NumerBudynku;
                txtKodPocztowy.Text = p.KodPocztowy;
                txtMiejscowosc.Text = p.Miejscowosc;

                // Wyświetlamy pierwszy przypisany wydział (jeśli istnieje)
                cbWydzial.SelectedItem = p.Wydzialy.FirstOrDefault();
            }
        }

        /// <summary>
        /// Dodaje nowego pracownika do bazy danych.
        /// </summary>
        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {
            if (!WalidujDane(out Wydzialy wydzial)) return;

            try
            {
                var nowy = new Pracownicy
                {
                    Imie = txtImie.Text,
                    Nazwisko = txtNazwisko.Text,
                    Email = txtEmail.Text,
                    Ulica = txtUlica.Text,
                    NumerBudynku = txtNumerBudynku.Text,
                    KodPocztowy = txtKodPocztowy.Text,
                    Miejscowosc = txtMiejscowosc.Text,
                    Wydzialy = new List<Wydzialy> { wydzial } // dodanie relacji wiele-do-wielu
                };

                db.Pracownicy.Add(nowy);
                db.SaveChanges();
                WczytajDane();
                CzyscPola();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas dodawania:\n" + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Edytuje dane zaznaczonego pracownika.
        /// </summary>
        private void BtnEdytuj_Click(object sender, RoutedEventArgs e)
        {
            if (wybranyPracownik == null)
            {
                MessageBox.Show("Wybierz pracownika do edycji.", "Brak wyboru", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!WalidujDane(out Wydzialy wydzial)) return;

            try
            {
                var pracownik = db.Pracownicy.Find(wybranyPracownik.PracownikID);
                pracownik.Imie = txtImie.Text;
                pracownik.Nazwisko = txtNazwisko.Text;
                pracownik.Email = txtEmail.Text;
                pracownik.Ulica = txtUlica.Text;
                pracownik.NumerBudynku = txtNumerBudynku.Text;
                pracownik.KodPocztowy = txtKodPocztowy.Text;
                pracownik.Miejscowosc = txtMiejscowosc.Text;

                // Nadpisz wydziały (dla uproszczenia przyjmujemy jeden wybór)
                pracownik.Wydzialy.Clear();
                pracownik.Wydzialy.Add(wydzial);

                db.SaveChanges();
                WczytajDane();
                CzyscPola();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas edycji:\n" + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Usuwa zaznaczonego pracownika z bazy, jeśli nie ma powiązanych zakupów.
        /// </summary>
        private void BtnUsun_Click(object sender, RoutedEventArgs e)
        {
            if (wybranyPracownik == null)
            {
                MessageBox.Show("Wybierz pracownika do usunięcia.", "Brak wyboru", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var pracownik = db.Pracownicy.Find(wybranyPracownik.PracownikID);

                if (pracownik.Zakupy.Any())
                {
                    MessageBox.Show("Nie można usunąć pracownika z przypisanymi zakupami.",
                                    "Błąd relacyjny", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                db.Pracownicy.Remove(pracownik);
                db.SaveChanges();
                WczytajDane();
                CzyscPola();
            }commit 
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas usuwania:\n" + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Odświeża widok danych.
        /// </summary>
        private void BtnOdswiez_Click(object sender, RoutedEventArgs e)
        {
            WczytajDane();
            CzyscPola();
        }

        /// <summary>
        /// Waliduje pola formularza i zwraca wybrany wydział.
        /// </summary>
        private bool WalidujDane(out Wydzialy wydzial)
        {
            wydzial = cbWydzial.SelectedItem as Wydzialy;

            if (string.IsNullOrWhiteSpace(txtImie.Text) ||
                string.IsNullOrWhiteSpace(txtNazwisko.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtUlica.Text) ||
                string.IsNullOrWhiteSpace(txtNumerBudynku.Text) ||
                string.IsNullOrWhiteSpace(txtKodPocztowy.Text) ||
                string.IsNullOrWhiteSpace(txtMiejscowosc.Text) ||
                wydzial == null)
            {
                MessageBox.Show("Uzupełnij wszystkie pola i wybierz wydział.",
                                "Brak danych", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
    }
}
