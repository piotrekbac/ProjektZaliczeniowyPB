using System;
using System.Linq;
using System.Windows;

namespace ProjektZaliczeniowyPB
{
    public partial class PracownicyWindow : Window
    {
        private ProjektZaliczeniowyBazaSamochodowEntities db = new ProjektZaliczeniowyBazaSamochodowEntities();
        private Pracownicy wybranyPracownik;

        public PracownicyWindow()
        {
            InitializeComponent();
            WczytajDane();
        }

        private void WczytajDane()
        {
            dgPracownicy.ItemsSource = db.Pracownicy.ToList();
        }

        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtImie.Text) ||
                string.IsNullOrWhiteSpace(txtNazwisko.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtUlica.Text) ||
                string.IsNullOrWhiteSpace(txtNumerBudynku.Text) ||
                string.IsNullOrWhiteSpace(txtKodPocztowy.Text) ||
                string.IsNullOrWhiteSpace(txtMiejscowosc.Text))
            {
                MessageBox.Show("Uzupełnij wszystkie pola!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

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
                    Miejscowosc = txtMiejscowosc.Text
                };

                db.Pracownicy.Add(nowy);
                db.SaveChanges();
                WczytajDane();
                CzyscPola();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas dodawania:\n" + (ex.InnerException?.InnerException?.Message ?? ex.Message),
                                "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnEdytuj_Click(object sender, RoutedEventArgs e)
        {
            if (dgPracownicy.SelectedItem is Pracownicy p)
            {
                try
                {
                    wybranyPracownik = db.Pracownicy.Find(p.PracownikID);
                    wybranyPracownik.Imie = txtImie.Text;
                    wybranyPracownik.Nazwisko = txtNazwisko.Text;
                    wybranyPracownik.Email = txtEmail.Text;
                    wybranyPracownik.Ulica = txtUlica.Text;
                    wybranyPracownik.NumerBudynku = txtNumerBudynku.Text;
                    wybranyPracownik.KodPocztowy = txtKodPocztowy.Text;
                    wybranyPracownik.Miejscowosc = txtMiejscowosc.Text;

                    db.SaveChanges();
                    WczytajDane();
                    CzyscPola();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd podczas edycji:\n" + (ex.InnerException?.InnerException?.Message ?? ex.Message),
                                    "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnUsun_Click(object sender, RoutedEventArgs e)
        {
            if (dgPracownicy.SelectedItem is Pracownicy p)
            {
                try
                {
                    var usun = db.Pracownicy.Find(p.PracownikID);
                    db.Pracownicy.Remove(usun);
                    db.SaveChanges();
                    WczytajDane();
                    CzyscPola();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd podczas usuwania:\n" + (ex.InnerException?.InnerException?.Message ?? ex.Message),
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
            txtUlica.Text = "";
            txtNumerBudynku.Text = "";
            txtKodPocztowy.Text = "";
            txtMiejscowosc.Text = "";
        }
    }
}
