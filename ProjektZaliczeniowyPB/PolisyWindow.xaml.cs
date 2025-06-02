using System;
using System.Linq;
using System.Windows;

namespace ProjektZaliczeniowyPB
{
    public partial class PolisyWindow : Window
    {
        private ProjektZaliczeniowyBazaSamochodowEntities db = new ProjektZaliczeniowyBazaSamochodowEntities();

        public PolisyWindow()
        {
            InitializeComponent();
            WczytajDane();
            WczytajComboBoxy();
        }

        private void WczytajDane()
        {
            dgPolisy.ItemsSource = db.Polisy.ToList();
        }

        private void WczytajComboBoxy()
        {
            cbZakup.ItemsSource = db.Zakupy.ToList();
        }

        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {
            var selectedZakup = cbZakup.SelectedItem as Zakupy;

            if (selectedZakup == null || dpRozpoczecie.SelectedDate == null || dpZakonczenie.SelectedDate == null)
            {
                MessageBox.Show("Wybierz zakup i daty.", "Brak danych", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var start = dpRozpoczecie.SelectedDate.Value;
            var end = dpZakonczenie.SelectedDate.Value;

            if (start > end)
            {
                MessageBox.Show("Data zakończenia nie może być wcześniejsza niż rozpoczęcia.", "Błąd daty", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var polisa = new Polisy
                {
                    ZakupID = selectedZakup.ZakupID,
                    DataRozpoczecia = start,
                    DataZakonczenia = end
                };

                db.Polisy.Add(polisa);
                db.SaveChanges();
                WczytajDane();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas dodawania polisy:\n" + (ex.InnerException?.InnerException?.Message ?? ex.Message),
                                "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnUsun_Click(object sender, RoutedEventArgs e)
        {
            if (dgPolisy.SelectedItem is Polisy p)
            {
                try
                {
                    var usun = db.Polisy.Find(p.PolisaID);
                    db.Polisy.Remove(usun);
                    db.SaveChanges();
                    WczytajDane();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd podczas usuwania polisy:\n" + (ex.InnerException?.InnerException?.Message ?? ex.Message),
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
