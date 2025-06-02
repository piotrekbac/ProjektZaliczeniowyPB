using System;
using System.Linq;
using System.Windows;

namespace ProjektZaliczeniowyPB
{
    public partial class ZakupyWindow : Window
    {
        private ProjektZaliczeniowyBazaSamochodowEntities db = new ProjektZaliczeniowyBazaSamochodowEntities();

        public ZakupyWindow()
        {
            InitializeComponent();
            WczytajDane();
            WczytajComboBoxy();
        }

        private void WczytajDane()
        {
            dgZakupy.ItemsSource = db.Zakupy.ToList();
        }

        private void WczytajComboBoxy()
        {
            cbKlient.ItemsSource = db.Klienci.ToList();
            cbPracownik.ItemsSource = db.Pracownicy.ToList();
            cbSamochod.ItemsSource = db.Samochody.ToList();
        }

        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {
            var klient = cbKlient.SelectedItem as Klienci;
            var pracownik = cbPracownik.SelectedItem as Pracownicy;
            var samochod = cbSamochod.SelectedItem as Samochody;
            var dataZakupu = dpDataZakupu.SelectedDate;

            if (klient == null || pracownik == null || samochod == null || dataZakupu == null)
            {
                MessageBox.Show("Wybierz klienta, pracownika, samochód i datę zakupu.", "Brak danych", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var zakup = new Zakupy
                {
                    KlientID = klient.KlientID,
                    PracownikID = pracownik.PracownikID,
                    SamochodID = samochod.SamochodID,
                    DataZakupu = dataZakupu.Value
                };

                db.Zakupy.Add(zakup);
                db.SaveChanges();
                WczytajDane();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas dodawania zakupu:\n" + (ex.InnerException?.InnerException?.Message ?? ex.Message),
                                "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnUsun_Click(object sender, RoutedEventArgs e)
        {
            if (dgZakupy.SelectedItem is Zakupy z)
            {
                try
                {
                    var usun = db.Zakupy.Find(z.ZakupID);
                    db.Zakupy.Remove(usun);
                    db.SaveChanges();
                    WczytajDane();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd podczas usuwania zakupu:\n" + (ex.InnerException?.InnerException?.Message ?? ex.Message),
                                    "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnOdswiez_Click(object sender, RoutedEventArgs e)
        {
            WczytajDane();
        }
    }
}
