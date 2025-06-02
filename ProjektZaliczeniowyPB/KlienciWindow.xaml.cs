using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ProjektZaliczeniowyPB
{
    /// <summary>
    /// Logika interakcji dla okna zarządzania klientami (KlienciWindow.xaml)
    /// </summary>
    public partial class KlienciWindow : Window
    {
        private ProjektZaliczeniowyBazaSamochodowEntities db = new ProjektZaliczeniowyBazaSamochodowEntities();
        private Klienci wybranyKlient;

        public KlienciWindow()
        {
            InitializeComponent();
            WczytajDane();
        }

        private void WczytajDane()
        {
            dgKlienci.ItemsSource = db.Klienci.ToList();
        }

        private void dgKlienci_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgKlienci.SelectedItem is Klienci k)
            {
                txtImie.Text = k.Imie;
                txtNazwisko.Text = k.Nazwisko;
                txtEmail.Text = k.Email;
                txtTelefon.Text = k.NumerTelefonu;
                txtNumerBudynku.Text = k.NumerBudynku;
                txtKodPocztowy.Text = k.KodPocztowy;
                txtMiejscowosc.Text = k.Miejscowosc;
            }
        }

        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtImie.Text) || string.IsNullOrWhiteSpace(txtNazwisko.Text)
                || string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtTelefon.Text))
            {
                MessageBox.Show("Imię, nazwisko, email i numer telefonu są wymagane.", "Brak danych", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (txtTelefon.Text.Length != 9 || !txtTelefon.Text.All(char.IsDigit))
            {
                MessageBox.Show("Numer telefonu musi zawierać dokładnie 9 cyfr.", "Błąd formatu", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var nowy = new Klienci
                {
                    Imie = txtImie.Text,
                    Nazwisko = txtNazwisko.Text,
                    Email = txtEmail.Text,
                    NumerTelefonu = txtTelefon.Text,
                    NumerBudynku = txtNumerBudynku.Text,
                    KodPocztowy = txtKodPocztowy.Text,
                    Miejscowosc = txtMiejscowosc.Text
                };

                db.Klienci.Add(nowy);
                db.SaveChanges();
                WczytajDane();
                CzyscPola();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas dodawania klienta:\n" + (ex.InnerException?.InnerException?.Message ?? ex.Message),
                                "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnEdytuj_Click(object sender, RoutedEventArgs e)
        {
            if (dgKlienci.SelectedItem is Klienci k)
            {
                if (string.IsNullOrWhiteSpace(txtImie.Text) || string.IsNullOrWhiteSpace(txtNazwisko.Text)
                    || string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtTelefon.Text))
                {
                    MessageBox.Show("Imię, nazwisko, email i numer telefonu są wymagane.", "Brak danych", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (txtTelefon.Text.Length != 9 || !txtTelefon.Text.All(char.IsDigit))
                {
                    MessageBox.Show("Numer telefonu musi zawierać dokładnie 9 cyfr.", "Błąd formatu", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                try
                {
                    wybranyKlient = db.Klienci.Find(k.KlientID);
                    wybranyKlient.Imie = txtImie.Text;
                    wybranyKlient.Nazwisko = txtNazwisko.Text;
                    wybranyKlient.Email = txtEmail.Text;
                    wybranyKlient.NumerTelefonu = txtTelefon.Text;
                    wybranyKlient.NumerBudynku = txtNumerBudynku.Text;
                    wybranyKlient.KodPocztowy = txtKodPocztowy.Text;
                    wybranyKlient.Miejscowosc = txtMiejscowosc.Text;

                    db.SaveChanges();
                    WczytajDane();
                    CzyscPola();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd podczas edycji klienta:\n" + (ex.InnerException?.InnerException?.Message ?? ex.Message),
                                    "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnUsun_Click(object sender, RoutedEventArgs e)
        {
            if (dgKlienci.SelectedItem is Klienci k)
            {
                try
                {
                    var doUsuniecia = db.Klienci.Find(k.KlientID);
                    db.Klienci.Remove(doUsuniecia);
                    db.SaveChanges();
                    WczytajDane();
                    CzyscPola();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd podczas usuwania klienta:\n" + (ex.InnerException?.InnerException?.Message ?? ex.Message),
                                    "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnOdswiez_Click(object sender, RoutedEventArgs e)
        {
            WczytajDane();
        }

        private void CzyscPola()
        {
            txtImie.Text = "";
            txtNazwisko.Text = "";
            txtEmail.Text = "";
            txtTelefon.Text = "";
            txtNumerBudynku.Text = "";
            txtKodPocztowy.Text = "";
            txtMiejscowosc.Text = "";
        }
    }
}
