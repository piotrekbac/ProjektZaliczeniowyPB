using System;
using System.Linq;
using System.Windows;

//Piotr Bacior - 15 722 WSEI Kraków

namespace ProjektZaliczeniowyPB
{
    /// <summary>
    /// Logika interakcji dla okna ZakupyWindow.xaml
    /// </summary>
    public partial class ZakupyWindow : Window
    {
        // Połączenie z bazą danych
        private ProjektZaliczeniowyBazaSamochodowEntities db = new ProjektZaliczeniowyBazaSamochodowEntities();

        public ZakupyWindow()
        {
            InitializeComponent();
            WczytajDane();
            WczytajComboBoxy();
        }

        /// <summary>
        /// Załadowanie wszystkich zakupów do DataGrid
        /// </summary>
        private void WczytajDane()
        {
            dgZakupy.ItemsSource = db.Zakupy.ToList();
        }

        /// <summary>
        /// Wczytuje dane do wszystkich ComboBoxów
        /// </summary>
        private void WczytajComboBoxy()
        {
            cbKlient.ItemsSource = db.Klienci.ToList();
            cbPracownik.ItemsSource = db.Pracownicy.ToList();
            cbSamochod.ItemsSource = db.Samochody.ToList();
        }

        /// <summary>
        /// Dodaje nowy zakup do bazy danych
        /// </summary>
        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {
            if (cbKlient.SelectedItem is Klienci klient &&
                cbPracownik.SelectedItem is Pracownicy pracownik &&
                cbSamochod.SelectedItem is Samochody samochod &&
                dpDataZakupu.SelectedDate is DateTime dataZakupu)
            {
                var zakup = new Zakupy
                {
                    KlientID = klient.KlientID,
                    PracownikID = pracownik.PracownikID,
                    SamochodID = samochod.SamochodID,
                    DataZakupu = dataZakupu
                };

                db.Zakupy.Add(zakup);
                db.SaveChanges();
                WczytajDane();
            }
        }

        /// <summary>
        /// Usuwa wybrany zakup z bazy
        /// </summary>
        private void BtnUsun_Click(object sender, RoutedEventArgs e)
        {
            if (dgZakupy.SelectedItem is Zakupy z)
            {
                var usun = db.Zakupy.Find(z.ZakupID);
                db.Zakupy.Remove(usun);
                db.SaveChanges();
                WczytajDane();
            }
        }

        /// <summary>
        /// Odświeża listę zakupów
        /// </summary>
        private void BtnOdswiez_Click(object sender, RoutedEventArgs e)
        {
            WczytajDane();
        }
    }
}
