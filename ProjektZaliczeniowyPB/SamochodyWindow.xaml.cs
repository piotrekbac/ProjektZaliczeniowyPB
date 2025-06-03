// Piotr Bacior - 15 722 WSEI Kraków
// SamochodyWindow.xaml.cs - logika zarządzania samochodami

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ProjektZaliczeniowyPB
{
    /// <summary>
    /// Okno WPF do zarządzania samochodami.
    /// Pozwala na przeglądanie, dodawanie, edycję, usuwanie i odświeżanie danych samochodów.
    /// </summary>
    public partial class SamochodyWindow : Window
    {
        // Kontekst bazy danych Entity Framework do obsługi operacji na bazie
        private ProjektZaliczeniowyBazaSamochodowEntities db = new ProjektZaliczeniowyBazaSamochodowEntities();

        // Referencja do aktualnie wybranego samochodu (dla edycji/usuwania)
        private Samochody wybranySamochod;

        /// <summary>
        /// Konstruktor okna — inicjalizuje komponenty i ładuje dane samochodów do tabeli.
        /// </summary>
        public SamochodyWindow()
        {
            InitializeComponent();
            WczytajDane();
        }

        /// <summary>
        /// Wczytuje dane samochodów z bazy i ustawia jako źródło danych dla DataGrid.
        /// Pokazuje tylko najważniejsze kolumny (bez relacji do Zakupów).
        /// </summary>
        private void WczytajDane()
        {
            // Wyświetlamy tylko kolumny z tabeli Samochody (bez relacji do Zakupów, które nie są tutaj potrzebne)
            dgSamochody.ItemsSource = db.Samochody
                .Select(s => new
                {
                    s.SamochodID,
                    s.Model,
                    s.RokProdukcji,
                    s.NumerSeryjny,
                    s.WersjaWyposazenia
                }).ToList();
        }

        /// <summary>
        /// Obsługuje zaznaczenie samochodu w tabeli (DataGrid).
        /// Ładuje dane wybranego samochodu do formularza edycji.
        /// </summary>
        private void dgSamochody_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgSamochody.SelectedItem != null)
            {
                dynamic s = dgSamochody.SelectedItem;
                txtModel.Text = s.Model;
                txtRokProdukcji.Text = s.RokProdukcji.ToString();
                txtNumerSeryjny.Text = s.NumerSeryjny;
                txtWersja.Text = s.WersjaWyposazenia;
            }
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku "Dodaj".
        /// Tworzy nowy samochód na podstawie danych z formularza i zapisuje go w bazie danych.
        /// Waliduje poprawność danych (w tym długość numeru seryjnego i poprawność roku).
        /// Po sukcesie odświeża tabelę i czyści formularz.
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
        /// Obsługuje kliknięcie przycisku "Edytuj".
        /// Aktualizuje dane wybranego samochodu i zapisuje zmiany w bazie.
        /// Waliduje poprawność danych (w tym długość numeru seryjnego i poprawność roku).
        /// Po sukcesie odświeża tabelę i czyści formularz.
        /// </summary>
        private void BtnEdytuj_Click(object sender, RoutedEventArgs e)
        {
            if (dgSamochody.SelectedItem != null && int.TryParse(txtRokProdukcji.Text, out int rok))
            {
                dynamic s = dgSamochody.SelectedItem;

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
        /// Obsługuje kliknięcie przycisku "Usuń".
        /// Usuwa wybrany samochód z bazy danych.
        /// Po sukcesie odświeża tabelę i czyści formularz.
        /// </summary>
        private void BtnUsun_Click(object sender, RoutedEventArgs e)
        {
            if (dgSamochody.SelectedItem != null)
            {
                dynamic s = dgSamochody.SelectedItem;

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
        /// Obsługuje kliknięcie przycisku "Odśwież".
        /// Powtórnie wczytuje dane z bazy.
        /// </summary>
        private void BtnOdswiez_Click(object sender, RoutedEventArgs e)
        {
            WczytajDane();
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
        }
    }
}