using System.Windows;

// Piotr Bacior - 15 722 WSEI Kraków

namespace ProjektZaliczeniowyPB
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnKlienci_Click(object sender, RoutedEventArgs e)
        {
            var okno = new KlienciWindow();
            okno.ShowDialog();
        }

        private void BtnSamochody_Click(object sender, RoutedEventArgs e)
        {
            var okno = new SamochodyWindow();
            okno.ShowDialog();
        }

        private void BtnPracownicy_Click(object sender, RoutedEventArgs e)
        {
            var okno = new PracownicyWindow();
            okno.ShowDialog();
        }

        private void BtnPolisy_Click(object sender, RoutedEventArgs e)
        {
            var okno = new PolisyWindow();
            okno.ShowDialog();
        }

        private void BtnZakupy_Click(object sender, RoutedEventArgs e)
        {
            var okno = new ZakupyWindow();
            okno.ShowDialog();
        }
    }
}
