// Piotr Bacior - 15 722 WSEI Kraków

using System.Windows;

namespace ProjektZaliczeniowyPB
{
    /// <summary>
    /// Główne okno aplikacji.
    /// Pozwala na wybór jednego z dostępnych modułów (Klienci, Samochody, Pracownicy, Polisy, Zakupy).
    /// Każdy przycisk otwiera osobne okno do zarządzania danym obszarem.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Konstruktor okna głównego — inicjalizuje komponenty i interfejs.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku "Zarządzaj Klientami".
        /// Otwiera okno zarządzania klientami w trybie modalnym (ShowDialog).
        /// </summary>
        private void BtnKlienci_Click(object sender, RoutedEventArgs e)
        {
            var okno = new KlienciWindow();
            okno.ShowDialog();
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku "Zarządzaj Samochodami".
        /// Otwiera okno zarządzania samochodami w trybie modalnym.
        /// </summary>
        private void BtnSamochody_Click(object sender, RoutedEventArgs e)
        {
            var okno = new SamochodyWindow();
            okno.ShowDialog();
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku "Zarządzaj Pracownikami".
        /// Otwiera okno zarządzania pracownikami w trybie modalnym.
        /// </summary>
        private void BtnPracownicy_Click(object sender, RoutedEventArgs e)
        {
            var okno = new PracownicyWindow();
            okno.ShowDialog();
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku "Zarządzaj Polisami".
        /// Otwiera okno zarządzania polisami w trybie modalnym.
        /// </summary>
        private void BtnPolisy_Click(object sender, RoutedEventArgs e)
        {
            var okno = new PolisyWindow();
            okno.ShowDialog();
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku "Zarządzaj Zakupami".
        /// Otwiera okno zarządzania zakupami w trybie modalnym.
        /// </summary>
        private void BtnZakupy_Click(object sender, RoutedEventArgs e)
        {
            var okno = new ZakupyWindow();
            okno.ShowDialog();
        }
    }
}