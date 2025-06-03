using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

// Piotr Bacior - 15 722 WSEI Kraków

namespace ProjektZaliczeniowyPB
{
    /// <summary>
    /// Logika interakcji dla okna zarządzania klientami (KlienciWindow.xaml)
    /// </summary>
    public partial class KlienciWindow : Window
    {
        private ProjektZaliczeniowyBazaSamochodowEntities db = new ProjektZaliczeniowyBazaSamochodowEntities();
        private Klienci wybranyKlient;

        /// <summary>
        /// Konstruktor okna - inicjalizuje komponenty i ładuje dane.
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
        /// Czyści wszystkie pola formularza.
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
        /// Obsługuje zaznaczenie klienta w tabeli — ładuje dane do pól tekstowych.
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
        /// Obsługuje kliknięcie przycisku "Dodaj" — dodaje nowego klienta do bazy.
        /// </summary>
        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {
            if (!WalidujDane()) return;

            try
            {
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

                db.Klienci.Add(klient);
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
        /// Obsługuje kliknięcie przycisku "Edytuj" — edytuje zaznaczonego klienta.
        /// </summary>
        private void BtnEdytuj_Click(object sender, RoutedEventArgs e)
        {
            if (wybranyKlient == null)
            {
                MessageBox.Show("Wybierz klienta z listy.", "Brak wyboru", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!WalidujDane()) return;

            try
            {
                var klient = db.Klienci.Find(wybranyKlient.KlientID);
                klient.Imie = txtImie.Text;
                klient.Nazwisko = txtNazwisko.Text;
                klient.Email = txtEmail.Text;
                klient.NumerTelefonu = txtTelefon.Text;
                klient.Ulica = txtUlica.Text;
                klient.NumerBudynku = txtNumerBudynku.Text;
                klient.KodPocztowy = txtKodPocztowy.Text;
                klient.Miejscowosc = txtMiejscowosc.Text;

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
        /// Obsługuje kliknięcie przycisku "Usuń" — usuwa zaznaczonego klienta z bazy.
        /// </summary>
        private void BtnUsun_Click(object sender, RoutedEventArgs e)
        {
            if (wybranyKlient == null)
            {
                MessageBox.Show("Wybierz klienta do usunięcia.", "Brak wyboru", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var klient = db.Klienci.Find(wybranyKlient.KlientID);
                db.Klienci.Remove(klient);
                db.SaveChanges();
                WczytajDane();
                CzyscPola();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas usuwania:\n" + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku "Odśwież" — ponownie wczytuje dane z bazy.
        /// </summary>
        private void BtnOdswiez_Click(object sender, RoutedEventArgs e)
        {
            WczytajDane();
            CzyscPola();
        }

        /// <summary>
        /// Waliduje wymagane pola formularza klienta.
        /// </summary>
        /// <returns>True jeśli dane są poprawne, w przeciwnym razie false.</returns>
        private bool WalidujDane()
        {
            if (string.IsNullOrWhiteSpace(txtImie.Text) ||
                string.IsNullOrWhiteSpace(txtNazwisko.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtTelefon.Text))
            {
                MessageBox.Show("Imię, nazwisko, e-mail i telefon są wymagane.", "Brak danych", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (txtTelefon.Text.Length != 9 || !txtTelefon.Text.All(char.IsDigit))
            {
                MessageBox.Show("Telefon musi mieć 9 cyfr.", "Błąd formatu", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
    }
}
