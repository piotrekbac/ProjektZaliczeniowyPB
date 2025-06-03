using System;
using System.Linq;
using System.Windows;

// Piotr Bacior - 15 722 WSEI Kraków

namespace ProjektZaliczeniowyPB
{
    /// <summary>
    /// Logika interakcji dla okna zarządzania samochodami (SamochodyWindow.xaml)
    /// </summary>
    public partial class SamochodyWindow : Window
    {
        private ProjektZaliczeniowyBazaSamochodowEntities db = new ProjektZaliczeniowyBazaSamochodowEntities();
        private Samochody wybranySamochod;

        /// <summary>
        /// Konstruktor okna. Inicjalizuje komponenty i ładuje dane z bazy.
        /// </summary>
        public SamochodyWindow()
        {
            InitializeComponent();
            WczytajDane();
        }

        /// <summary>
        /// Wczytuje listę samochodów z bazy danych.
        /// </summary>
        private void WczytajDane()
        {
            dgSamochody.ItemsSource = db.Samochody.ToList();
        }

        /// <summary>
        /// Czyści wszystkie pola formularza.
        /// </summary>
        private void CzyscPola()
        {
            txtModel.Text = "";
            txtRokProdukcji.Text = "";
            txtNumerSeryjny.Text = "";
            txtWersja.Text = "";
            wybranySamochod = null;
        }

        /// <summary>
        /// Obsługuje zaznaczenie rekordu z tabeli i wczytuje dane do formularza.
        /// </summary>
        private void dgSamochody_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (dgSamochody.SelectedItem is Samochody s)
            {
                wybranySamochod = s;
                txtModel.Text = s.Model;
                txtRokProdukcji.Text = s.RokProdukcji.ToString();
                txtNumerSeryjny.Text = s.NumerSeryjny;
                txtWersja.Text = s.WersjaWyposazenia;
            }
        }

        /// <summary>
        /// Dodaje nowy samochód do bazy danych.
        /// </summary>
        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {
            if (!WalidujDane(out int rok)) return;

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
                MessageBox.Show("Błąd podczas dodawania samochodu:\n" + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Edytuje zaznaczonego samochód.
        /// </summary>
        private void BtnEdytuj_Click(object sender, RoutedEventArgs e)
        {
            if (wybranySamochod == null)
            {
                MessageBox.Show("Wybierz samochód do edycji.", "Brak wyboru", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!WalidujDane(out int rok)) return;

            try
            {
                var samochod = db.Samochody.Find(wybranySamochod.SamochodID);
                samochod.Model = txtModel.Text;
                samochod.RokProdukcji = rok;
                samochod.NumerSeryjny = txtNumerSeryjny.Text;
                samochod.WersjaWyposazenia = txtWersja.Text;

                db.SaveChanges();
                WczytajDane();
                CzyscPola();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas edycji samochodu:\n" + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Usuwa zaznaczonego samochód z bazy.
        /// </summary>
        private void BtnUsun_Click(object sender, RoutedEventArgs e)
        {
            if (wybranySamochod == null)
            {
                MessageBox.Show("Wybierz samochód do usunięcia.", "Brak wyboru", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var doUsuniecia = db.Samochody.Find(wybranySamochod.SamochodID);
                db.Samochody.Remove(doUsuniecia);
                db.SaveChanges();
                WczytajDane();
                CzyscPola();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas usuwania samochodu:\n" + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Odświeża dane w tabeli.
        /// </summary>
        private void BtnOdswiez_Click(object sender, RoutedEventArgs e)
        {
            WczytajDane();
            CzyscPola();
        }

        /// <summary>
        /// Waliduje dane w formularzu, sprawdza poprawność numeru seryjnego i roku.
        /// </summary>
        private bool WalidujDane(out int rok)
        {
            rok = 0;

            if (string.IsNullOrWhiteSpace(txtModel.Text) ||
                string.IsNullOrWhiteSpace(txtRokProdukcji.Text) ||
                string.IsNullOrWhiteSpace(txtNumerSeryjny.Text) ||
                string.IsNullOrWhiteSpace(txtWersja.Text))
            {
                MessageBox.Show("Uzupełnij wszystkie pola!", "Brak danych", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!int.TryParse(txtRokProdukcji.Text, out rok) || rok < 1950 || rok > DateTime.Now.Year + 1)
            {
                MessageBox.Show("Podaj poprawny rok produkcji (1950–" + (DateTime.Now.Year + 1) + ").", "Błąd danych", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (txtNumerSeryjny.Text.Length < 15 || txtNumerSeryjny.Text.Length > 50)
            {
                MessageBox.Show("Numer seryjny musi mieć od 15 do 50 znaków.", "Błąd formatu", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
    }
}
