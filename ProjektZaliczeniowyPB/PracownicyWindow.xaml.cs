using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjektZaliczeniowyPB
{
    /// <summary>
    /// Logika interakcji dla okna PracownicyWindow.xaml
    /// </summary>
    public partial class PracownicyWindow : Window
    {
        // Kontekst bazy danych
        private ProjektZaliczeniowyBazaSamochodowEntities db = new ProjektZaliczeniowyBazaSamochodowEntities();

        // Wybrany pracownik
        private Pracownicy wybranyPracownik;

        public PracownicyWindow()
        {
            InitializeComponent();
            WczytajDane();
        }

        /// <summary>
        /// Wczytuje pracowników z bazy i przypisuje do DataGrid
        /// </summary>
        private void WczytajDane()
        {
            dgPracownicy.ItemsSource = db.Pracownicy.ToList();
        }

        /// <summary>
        /// Dodaje nowego pracownika
        /// </summary>
        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtImie.Text) && !string.IsNullOrWhiteSpace(txtNazwisko.Text))
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
        }

        /// <summary>
        /// Edytuje dane wybranego pracownika
        /// </summary>
        private void BtnEdytuj_Click(object sender, RoutedEventArgs e)
        {
            if (dgPracownicy.SelectedItem is Pracownicy p)
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
        }

        /// <summary>
        /// Usuwa wybranego pracownika z bazy
        /// </summary>
        private void BtnUsun_Click(object sender, RoutedEventArgs e)
        {
            if (dgPracownicy.SelectedItem is Pracownicy p)
            {
                var doUsuniecia = db.Pracownicy.Find(p.PracownikID);
                db.Pracownicy.Remove(doUsuniecia);
                db.SaveChanges();
                WczytajDane();
                CzyscPola();
            }
        }

        /// <summary>
        /// Odświeża dane w tabeli
        /// </summary>
        private void BtnOdswiez_Click(object sender, RoutedEventArgs e)
        {
            WczytajDane();
        }

        /// <summary>
        /// Czyści pola formularza
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
        }
    }
}
