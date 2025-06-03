// Piotr Bacior - 15 722 WSEI Kraków

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
            cbWydzial.ItemsSource = db.Wydzialy.ToList();
        }

        private void WczytajDane()
        {
            // Używamy pełnych encji zamiast anonimowych typów
            dgPracownicy.ItemsSource = db.Pracownicy.ToList();
        }

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
                    cbWydzial.SelectedItem = p.Wydzialy.FirstOrDefault();
                }
            }
        }

        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {
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

        private void BtnOdswiez_Click(object sender, RoutedEventArgs e)
        {
            WczytajDane();
            CzyscPola();
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
            cbWydzial.SelectedIndex = -1;
        }
    }
}
