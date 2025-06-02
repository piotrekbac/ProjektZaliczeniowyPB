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

//Piotr Bacior - 15 722 WSEI Kraków

namespace ProjektZaliczeniowyPB
{
    /// <summary>
    /// Logika interakcji dla okna PolisyWindow.xaml
    /// </summary>
    public partial class PolisyWindow : Window
    {
        // Obiekt kontekstu bazy danych
        private ProjektZaliczeniowyBazaSamochodowEntities db = new ProjektZaliczeniowyBazaSamochodowEntities();

        public PolisyWindow()
        {
            InitializeComponent();
            WczytajDane();
            WczytajComboBoxy();
        }

        /// <summary>
        /// Ładuje dane polis do DataGrid
        /// </summary>
        private void WczytajDane()
        {
            dgPolisy.ItemsSource = db.Polisy.ToList();
        }

        /// <summary>
        /// Wczytuje dane zakupów do ComboBoxa
        /// </summary>
        private void WczytajComboBoxy()
        {
            cbZakup.ItemsSource = db.Zakupy.ToList();
        }

        /// <summary>
        /// Dodaje nową polisę do bazy
        /// </summary>
        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {
            if (cbZakup.SelectedItem is Zakupy zakup &&
                dpRozpoczecie.SelectedDate is DateTime start &&
                dpZakonczenie.SelectedDate is DateTime end &&
                start <= end)
            {
                var polisa = new Polisy
                {
                    ZakupID = zakup.ZakupID,
                    DataRozpoczecia = start,
                    DataZakonczenia = end
                };

                db.Polisy.Add(polisa);
                db.SaveChanges();
                WczytajDane();
            }
            else
            {
                MessageBox.Show("Upewnij się, że daty są poprawne i wszystkie pola zostały wybrane.", "Błąd");
            }
        }

        /// <summary>
        /// Usuwa zaznaczoną polisę
        /// </summary>
        private void BtnUsun_Click(object sender, RoutedEventArgs e)
        {
            if (dgPolisy.SelectedItem is Polisy p)
            {
                var usun = db.Polisy.Find(p.PolisaID);
                db.Polisy.Remove(usun);
                db.SaveChanges();
                WczytajDane();
            }
        }

        /// <summary>
        /// Odświeża widok polis
        /// </summary>
        private void BtnOdswiez_Click(object sender, RoutedEventArgs e)
        {
            WczytajDane();
        }
    }
}
