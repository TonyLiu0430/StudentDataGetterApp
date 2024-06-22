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
        private readonly DataGetter dataGetter = new();
        public MainWindow() {
            InitializeComponent();
            int minumumYear = 99;
            LowerYearComboBox.ItemsSource = Enumerable.Range(minumumYear, DateTime.Now.Year - (minumumYear + 1911) + 1).ToList();
            UpperYearComboBox.ItemsSource = Enumerable.Range(minumumYear, DateTime.Now.Year - (minumumYear + 1911) + 1).ToList();
        }

        private void LowerYearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            int lowerYear = (int)LowerYearComboBox.SelectedItem;
            UpperYearComboBox.ItemsSource = Enumerable.Range(lowerYear, DateTime.Now.Year - (lowerYear + 1911) + 1).ToList();
        }

        private void 不支援部_Checked(object sender, RoutedEventArgs e) {
            CheckBox checkBox = (CheckBox)sender;
            MessageBox.Show($"暫不支援{checkBox.Content}", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
            checkBox.IsChecked = false;
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
            try {
                await dataGetter.StartFetchingAsync();
            } catch(UnAuthorizedException) {
                return;
            } finally {
                FetchingProgressBar.IsIndeterminate = false;
                StartGetter.IsEnabled = true;
            }
            dataGetter.SaveData();
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
            if(!Directory.Exists(".\\result")) {
                Directory.CreateDirectory(".\\result");
            }
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo {
                FileName = "explorer.exe",
                Arguments = ".\\result",
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal
            });
        }

        private async void GetCookieButton_ClickAsync(object sender, RoutedEventArgs e) {
            MessageBox.Show("暫不支援功能", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
            var cookieFile = new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data\Default\Network\Cookies");
            var chromeStateFile = new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data\Local State");
            if (!cookieFile.Exists) {
                MessageBox.Show("找不到Cookie檔案", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!chromeStateFile.Exists) {
                MessageBox.Show("找不到Chrome Local State檔案", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string key = CookieGetter.GetKey(chromeStateFile);
            string cookie = await CookieGetter.GetCookieAsync(cookieFile, key);
            CookieInput.Text = cookie;
        }
    }
}
