using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace proj_forum
{/// <summary>
 /// Logika interakcji dla klasy MainWindow.xaml
 /// </summary>
    public partial class MainWindow : Window
    {
        string path;

        public MainWindow()
        {
            InitializeComponent();
            //zamykamy pewne funkcjonalności, gdyż musielibyśmy inaczej oprogramować obiekt DataGrid, a tego nie potrafię
            datagrid_projekt_lista.CanUserAddRows = false;
            datagrid_projekt_lista.CanUserDeleteRows = false;
            datagrid_projekt_lista.IsReadOnly = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //path = (new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath;
                path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location));
                var baza = new Base(path);
                baza.AddBaseIfNotExist();
                if (baza.GetBaseVersion() != 1)
                {
                    // tutaj dałbym konwerter na starej bazy na nową, ale narazie nie mamy takiego konwertera więc program tego nie zrobi
                    MessageBox.Show("Nieprawidłowa wersja bazy. Konwersja niemożliwa w tej wersji programu.Nastąpi zamknięcie programu", "Projekty-info");
                    this.Close();
                }
                DataTable tablicaprojektow = baza.PobierzDaneTabeli("projekt_lista");
                datagrid_projekt_lista.ItemsSource = tablicaprojektow.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ". Program zostanie zamknięty.", "Projekty-info");
                this.Close();
            }

        }
    }
}
