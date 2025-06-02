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

// Piotr Bacior - 15 722 WSEI Kraków

namespace ProjektZaliczeniowyPB
{
    /// <summary>
    /// Logika interakcji dla okna SamochodyWindow.xaml
    /// </summary>
    public partial class SamochodyWindow : Window
    {
        // Obiekt kontekstu bazy danych
        private ProjektZaliczeniowyBazaSamochodowEntities db = new ProjektZaliczeniowyBazaSamochodowEntities();

        // Wybrany samochód do edycji
        private Samochody wybranySamochod;

        public SamochodyWindow()
        {
            InitializeComponent();
            WczytajDane();
        }

        /// <summary>
        /// Wczytuje dane samochodów do DataGrid
        /// </summary>
        private void WczytajDane()
        {
            dgSamochody.ItemsSource = db.Samochody.ToList();
        }

        /// <summary>
        /// Dodaje nowy samochód na podstawie danych z pól
        /// </summary>
        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtModel.Text) && int.TryParse(txtRokProdukcji.Text, out int rok))
            {
                var nowy = new Samochody
                {
                    Model = txtModel.Text,
                    RokProdukcji = rok,
                    NumerSeryjny = txtNumerSeryjny.Text,
                    WersjaWyposazenia = txtWersja.Text
                };

                db.Samochody.Add(nowy);
                db.SaveChanges();
                WczytajDane();
                CzyscPola();
            }
        }

        /// <summary>
        /// Edytuje dane wybranego samochodu
        /// </summary>
        private void BtnEdytuj_Click(object sender, RoutedEventArgs e)
        {
            if (dgSamochody.SelectedItem is Samochody s && int.TryParse(txtRokProdukcji.Text, out int rok))
            {
                wybranySamochod = db.Samochody.Find(s.SamochodID);
                wybranySamochod.Model = txtModel.Text;
                wybranySamochod.RokProdukcji = rok;
                wybranySamochod.NumerSeryjny = txtNumerSeryjny.Text;
                wybranySamochod.WersjaWyposazenia = txtWersja.Text;

                db.SaveChanges();
                WczytajDane();
                CzyscPola();
            }
        }

        /// <summary>
        /// Usuwa wybrany samochód
        /// </summary>
        private void BtnUsun_Click(object sender, RoutedEventArgs e)
        {
            if (dgSamochody.SelectedItem is Samochody s)
            {
                var usun = db.Samochody.Find(s.SamochodID);
                db.Samochody.Remove(usun);
                db.SaveChanges();
                WczytajDane();
                CzyscPola();
            }
        }

        /// <summary>
        /// Odświeża widok danych
        /// </summary>
        private void BtnOdswiez_Click(object sender, RoutedEventArgs e)
        {
            WczytajDane();
        }

        /// <summary>
        /// Czyści pola tekstowe
        /// </summary>
        private void CzyscPola()
        {
            txtModel.Text = "";
            txtRokProdukcji.Text = "";
            txtNumerSeryjny.Text = "";
            txtWersja.Text = "";
        }
    }
}