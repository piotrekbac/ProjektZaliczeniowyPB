// Piotr Bacior - 15 722 WSEI Kraków

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ProjektZaliczeniowyPB
{
    /// <summary>
    /// Okno WPF do zarządzania pracownikami.
    /// Pozwala na przeglądanie, dodawanie, edycję, usuwanie i odświeżanie danych pracowników.
    /// </summary>
    public partial class PracownicyWindow : Window
    {
        // Kontekst bazy danych Entity Framework do obsługi operacji na bazie
        private ProjektZaliczeniowyBazaSamochodowEntities db = new ProjektZaliczeniowyBazaSamochodowEntities();

        // Referencja do aktualnie wybranego pracownika (dla edycji/usuwania)
        private Pracownicy wybranyPracownik;

        /// <summary>
        /// Konstruktor okna — inicjalizuje komponenty, ładuje dane pracowników oraz listę wydziałów do ComboBoxa.
        /// </summary>
        public PracownicyWindow()
        {
            InitializeComponent();
            WczytajDane();

            //Ładowanie listy wydziałów do ComboBoxa
            cbWydzial.ItemsSource = db.Wydzialy.ToList();
        }

        /// <summary>
        /// Wczytuje wszystkich pracowników z bazy i ustawia jako źródło danych dla DataGrid.
        /// </summary>
        private void WczytajDane()
        {
            // Używamy pełnych encji zamiast anonimowych typów
            dgPracownicy.ItemsSource = db.Pracownicy.ToList();
        }

        /// <summary>
        /// Obsługuje zaznaczenie pracownika w tabeli (DataGrid).
        /// Ładuje dane wybranego pracownika do formularza edycji.
        /// </summary>
        private void dgPracownicy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgPracownicy.SelectedItem is Pracownicy p)
            {
                wybranyPracownik = db.Pracownicy.Find(p.PracownikID);

                if (wybranyPracownik != null)
                {
                    txtImie.Text = p.Imie;
                    txtNazwisko.Text = p.Nazwisko;
                    txtEmail.Text = p.Email;
                    txtUlica.Text = p.Ulica;
                    txtNumerBudynku.Text = p.NumerBudynku;
                    txtKodPocztowy.Text = p.KodPocztowy;
                    txtMiejscowosc.Text = p.Miejscowosc;
                    // Przypisanie wydziału do ComboBoxa (pierwszy wydział z listy powiązanych)
                    cbWydzial.SelectedItem = p.Wydzialy.FirstOrDefault();
                }
            }
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku "Dodaj".
        /// Tworzy nowego pracownika na podstawie danych z formularza i zapisuje go w bazie danych.
        /// Po sukcesie odświeża tabelę i czyści formularz.
        /// </summary>
        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {
            // Walidacja: wymagane wybranie wydziału
            if (!(cbWydzial.SelectedItem is Wydzialy wydzial))
            {
                MessageBox.Show("Wybierz wydział.");
                return;
            }

            var nowy = new Pracownicy
            {
                Imie = txtImie.Text,
                Nazwisko = txtNazwisko.Text,
                Email = txtEmail.Text,
                Ulica = txtUlica.Text,
                NumerBudynku = txtNumerBudynku.Text,
                KodPocztowy = txtKodPocztowy.Text,
                Miejscowosc = txtMiejscowosc.Text
            };

            // Przypisanie pracownika do wybranego wydziału
            nowy.Wydzialy.Add(wydzial);

            try
            {
                db.Pracownicy.Add(nowy);
                db.SaveChanges();
                WczytajDane();
                CzyscPola();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd dodawania:\n" + ex.Message);
            }
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku "Edytuj".
        /// Aktualizuje dane wybranego pracownika na podstawie formularza i zapisuje zmiany w bazie.
        /// Po sukcesie odświeża tabelę i czyści formularz.
        /// </summary>
        private void BtnEdytuj_Click(object sender, RoutedEventArgs e)
        {
            if (wybranyPracownik == null) return;

            wybranyPracownik.Imie = txtImie.Text;
            wybranyPracownik.Nazwisko = txtNazwisko.Text;
            wybranyPracownik.Email = txtEmail.Text;
            wybranyPracownik.Ulica = txtUlica.Text;
            wybranyPracownik.NumerBudynku = txtNumerBudynku.Text;
            wybranyPracownik.KodPocztowy = txtKodPocztowy.Text;
            wybranyPracownik.Miejscowosc = txtMiejscowosc.Text;

            // Aktualizacja wydziału (jeśli wybrano inny)
            if (cbWydzial.SelectedItem is Wydzialy wydzial)
            {
                wybranyPracownik.Wydzialy.Clear();
                wybranyPracownik.Wydzialy.Add(wydzial);
            }

            try
            {
                db.SaveChanges();
                WczytajDane();
                CzyscPola();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd edycji:\n" + ex.Message);
            }
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku "Usuń".
        /// Usuwa wybranego pracownika z bazy danych.
        /// Po sukcesie odświeża tabelę i czyści formularz.
        /// Obsługuje wyjątek, jeśli pracownik jest powiązany z innymi encjami (np. zakupami).
        /// </summary>
        private void BtnUsun_Click(object sender, RoutedEventArgs e)
        {
            if (wybranyPracownik == null) return;

            try
            {
                db.Pracownicy.Remove(wybranyPracownik);
                db.SaveChanges();
                WczytajDane();
                CzyscPola();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nie można usunąć pracownika przypisanego do zakupu.\n" + ex.Message,
                    "Błąd relacji", MessageBoxButton.OK, MessageBoxImage.Error);
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
        /// Czyści wszystkie pola formularza oraz resetuje wybór wydziału.
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
            cbWydzial.SelectedIndex = -1;
        }
    }
}