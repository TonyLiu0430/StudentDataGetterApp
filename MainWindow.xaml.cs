using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StudentDataGetterApp {
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window {
        private DataGetter dataGetter = new DataGetter();
        public MainWindow() {
            InitializeComponent();
            int minumumYear = 99;
            LowerYearComboBox.ItemsSource = Enumerable.Range(minumumYear, DateTime.Now.Year - (minumumYear + 1911) + 1).ToList();
            UpperYearComboBox.ItemsSource = Enumerable.Range(minumumYear, DateTime.Now.Year - (minumumYear + 1911) + 1).ToList();
        }
        /*
        private void CookieFileSelecter_Click(object sender, RoutedEventArgs e) {
            var dialog = new Microsoft.Win32.OpenFileDialog {
                DefaultExt = "",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Google\\Chrome\\User Data\\Default\\Network",
                FileName = "Cookies",
            };
            bool? result = dialog.ShowDialog();
            if (result == true) {
                dataGetter.CookieFile = new FileInfo(dialog.FileName);
                //CookieFileLabel.Content = "現在開啟:" + dataGetter.CookieFile.FullName;
            }
        }

        private void ChromeStateFileSelecter_Click(object sender, RoutedEventArgs e) {
            var dialog = new Microsoft.Win32.OpenFileDialog {
                DefaultExt = "",
                InitialDirectory = "C:\\Users\\Username\\AppData\\Local\\Google\\Chrome\\User Data",
                FileName = "Local State",
            };
            bool? result = dialog.ShowDialog();
            if (result == true) {
                dataGetter.StateFileOrigin = new FileInfo(dialog.FileName);
                //ChromeStateFileLabel.Content = "現在開啟:" + dataGetter.CookieFile.FullName;
            }
        }
        */
        private void LowerYearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            int lowerYear = (int)LowerYearComboBox.SelectedItem;
            UpperYearComboBox.ItemsSource = Enumerable.Range(lowerYear, DateTime.Now.Year - (lowerYear + 1911) + 1).ToList();
        }

        private void 日間部_Checked(object sender, RoutedEventArgs e) {

        }

        private void 進修部_Checked(object sender, RoutedEventArgs e) {
            MessageBox.Show("暫不支援進修部", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
            進修部.IsChecked = false;
        }

        private void 學士班_Checked(object sender, RoutedEventArgs e) {

        }

        private void 碩士班_Checked(object sender, RoutedEventArgs e) {
            MessageBox.Show("暫不支援碩士班", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
            碩士班.IsChecked = false;
        }

        private void 博士班_Checked(object sender, RoutedEventArgs e) {
            MessageBox.Show("暫不支援博士班", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
            博士班.IsChecked = false;
        }

        private async void StartGetter_ClickAsync(object sender, RoutedEventArgs e) {
            if(LowerYearComboBox.SelectedItem == null || UpperYearComboBox.SelectedItem == null) {
                MessageBox.Show("年份不可為空", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if((int)LowerYearComboBox.SelectedItem > (int)UpperYearComboBox.SelectedItem ) {
                MessageBox.Show("年份範圍錯誤", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if(日間部.IsChecked == false && 進修部.IsChecked == false) {
                MessageBox.Show("至少選擇一個部別", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if(學士班.IsChecked == false && 碩士班.IsChecked == false && 博士班.IsChecked == false) {
                MessageBox.Show("至少選擇一個班別", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if(CookieInput.Text == "") {
                MessageBox.Show("請輸入Cookie", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            StartGetter.IsEnabled = false;
            dataGetter.QueryYearLower = (int)LowerYearComboBox.SelectedItem;
            dataGetter.QueryYearUpper = (int)UpperYearComboBox.SelectedItem;
            dataGetter.Cookie = CookieInput.Text;
            FetchingProgressBar.IsIndeterminate = true;
            await dataGetter.StartFetchingAsync(() => { });
            FetchingProgressBar.IsIndeterminate = false;
            dataGetter.SaveData();
            StartGetter.IsEnabled = true;
            MessageBox.Show("資料已儲存", "成功", MessageBoxButton.OK, MessageBoxImage.Information);

            if (!Directory.Exists(".\\record")) {
                Directory.CreateDirectory(".\\record");
            }
            File.Create(".\\record\\cookie.txt").Close();
            File.WriteAllText(".\\record\\cookie.txt", CookieInput.Text);
        }

        private void CookieInput_Init(object sender, EventArgs e) {
            if(Directory.Exists(".\\record")) {
                if (File.Exists(".\\record\\cookie.txt")) {
                    CookieInput.Text = File.ReadAllText(".\\record\\cookie.txt");
                }
            }
        }

        private void OpenResultDir_Click(object sender, RoutedEventArgs e) {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo {
                FileName = "explorer.exe",
                Arguments = ".\\result",
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal
            });
        }
    }
}
