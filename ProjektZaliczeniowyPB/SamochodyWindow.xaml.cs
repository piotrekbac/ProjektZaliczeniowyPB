// Piotr Bacior - 15 722 WSEI Kraków
// SamochodyWindow.xaml.cs - logika zarządzania samochodami

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ProjektZaliczeniowyPB
{
    /// <summary>
    /// Logika interakcji dla okna SamochodyWindow.xaml
    /// </summary>
    public partial class SamochodyWindow : Window
    {
        private ProjektZaliczeniowyBazaSamochodowEntities db = new ProjektZaliczeniowyBazaSamochodowEntities();
        private Samochody wybranySamochod;

        public SamochodyWindow()
        {
            InitializeComponent();
            WczytajDane();
        }

        /// <summary>
        /// Załadowanie danych z bazy do DataGrid
        /// </summary>
        private void WczytajDane()
        {
            dgSamochody.ItemsSource = db.Samochody.ToList();
        }

        /// <summary>
        /// Obsługa zaznaczenia wiersza w tabeli samochodów
        /// </summary>
        private void dgSamochody_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgSamochody.SelectedItem is Samochody s)
            {
                txtModel.Text = s.Model;
                txtRokProdukcji.Text = s.RokProdukcji.ToString();
                txtNumerSeryjny.Text = s.NumerSeryjny;
                txtWersja.Text = s.WersjaWyposazenia;
            }
        }

        /// <summary>
        /// Dodawanie nowego samochodu
        /// </summary>
        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtModel.Text) ||
                string.IsNullOrWhiteSpace(txtRokProdukcji.Text) ||
                string.IsNullOrWhiteSpace(txtNumerSeryjny.Text) ||
                string.IsNullOrWhiteSpace(txtWersja.Text))
            {
                MessageBox.Show("Uzupełnij wszystkie pola!", "Brak danych", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(txtRokProdukcji.Text, out int rok))
            {
                MessageBox.Show("Podaj poprawny rok produkcji (liczba).", "Błąd danych", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (txtNumerSeryjny.Text.Length < 15 || txtNumerSeryjny.Text.Length > 50)
            {
                MessageBox.Show("Numer seryjny musi mieć od 15 do 50 znaków.", "Błąd formatu", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
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
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas dodawania:\n" + (ex.InnerException?.InnerException?.Message ?? ex.Message),
                                "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Edycja istniejącego samochodu
        /// </summary>
        private void BtnEdytuj_Click(object sender, RoutedEventArgs e)
        {
            if (dgSamochody.SelectedItem is Samochody s && int.TryParse(txtRokProdukcji.Text, out int rok))
            {
                if (txtNumerSeryjny.Text.Length < 15 || txtNumerSeryjny.Text.Length > 50)
                {
                    MessageBox.Show("Numer seryjny musi mieć od 15 do 50 znaków.", "Błąd formatu", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                try
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
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd podczas edycji samochodu:\n" + (ex.InnerException?.InnerException?.Message ?? ex.Message),
                                    "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Usuwanie zaznaczonego samochodu
        /// </summary>
        private void BtnUsun_Click(object sender, RoutedEventArgs e)
        {
            if (dgSamochody.SelectedItem is Samochody s)
            {
                try
                {
                    var usun = db.Samochody.Find(s.SamochodID);
                    db.Samochody.Remove(usun);
                    db.SaveChanges();
                    WczytajDane();
                    CzyscPola();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd podczas usuwania samochodu:\n" + (ex.InnerException?.InnerException?.Message ?? ex.Message),
                                    "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Odświeżenie danych w tabeli
        /// </summary>
        private void BtnOdswiez_Click(object sender, RoutedEventArgs e)
        {
            WczytajDane();
        }

        /// <summary>
        /// Czyszczenie pól formularza
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
