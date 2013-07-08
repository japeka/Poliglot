using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Windows;
using Microsoft.WindowsAzure.MobileServices;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Poliglot.Resources;
using Newtonsoft.Json;
using System.Xml.Linq;
using Windows.ApplicationModel;


/*  GAME LOGIC
 *  Kolme kierrosta, jossa kussakin kysytään 3 kysymystä
 *  Vaikeustason mukaan kysymyksiä alkaa tulla 
 *  Taso        kysymykset tulevat seuraavilta vaik.tasoilta
 *  Level 1     1 - 3   
 *  Level 2     2 - 4
 *  Level 3     3 - 5
 *   
 *  Vastaamiseen on vain 5 sekuntia aikaa, jonka aikana
 *  on valittava vastausvaihtoehto. Sekuntikello alkaa
 *  käydä siitä, kun kysymys kielestä ja vaihtoehtojen
 *  ilmestyessä. Mitä nopeammin kysymykseen vastataan
 *  sen enemmän pisteitä tulee. Kysymyksestä saatava
 *  
 *              kysymys     vaik.taso   round(aika)     total_kysymyksestä
 *  1 level =   1             1         *       3       = 3
 *  2 level =   1             2         *       4       = 8
 *  3 level =   1             3         *       4       = 12
 *  
 *  tarkoitus ottaa mukaan kielistä xml-pohjainen esitys siltä varalta, kun
 *  azuren käyttöoikeudet päättyvät, pelin pelaamista voi jatkaa käyttäen
 *  lokaalia xml-tietovarastoa.
 *  todo LAST
 */

namespace Poliglot {

    //LanguageSample
    public class LanguageSample
    {
        public int Id { get; set; }
        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; }
        [JsonProperty(PropertyName = "level")]
        public int Level { get; set; }
        [JsonProperty(PropertyName = "sample")]
        public String Sample { get; set; }
    }


    public partial class MainPage : PhoneApplicationPage {

        private MobileServiceCollection<LanguageSample, LanguageSample> items;
        private IMobileServiceTable<LanguageSample> languageSampleTable = App.MobileService.GetTable<LanguageSample>();

        // Constructor
        public MainPage()  {
            checkOnlineStatus();
            InitializeComponent();
            BuildLocalizedApplicationBar();
            // Sample code to localize the ApplicationBar
        }
        
        private void btnPlayPoliglotClickEvent(object sender, RoutedEventArgs e){
           // NavigationService.Navigate(new Uri("/SecondPage.xaml?msg=" + textBox1.Text, UriKind.Relative));
           NavigationService.Navigate(new Uri("/GamePlayPage.xaml?level="+getDifficultyLevel(), UriKind.Relative));
        }

        private void btnHelpClickEvent(object sender, RoutedEventArgs e){
           NavigationService.Navigate(new Uri("/InstructionsPage.xaml", UriKind.Relative));
        }

        private int getDifficultyLevel() {
            if (rbFirst.IsChecked==true) {
                return 1;
            } else if (rbSecond.IsChecked == true) {
                return 2;
            } else {
                return 3;
            }
        }

       private async void checkOnlineStatus() {
            try {
                items = await languageSampleTable
                    .Where(LanguageSample => LanguageSample.Level == 1)
                    .ToCollectionAsync();
                btnPlay.IsEnabled = true;
                rbFirst.IsEnabled = true;
                rbSecond.IsEnabled = true;
                rbThird.IsEnabled = true;
                return;
            } catch (MobileServiceInvalidOperationException e) {
                try {
                    string langXMLPath = Path.Combine("", "Assets/languages.xml");
                    XDocument loadedData = XDocument.Load(langXMLPath);
                    btnPlay.IsEnabled = true;
                    rbFirst.IsEnabled = true;
                    rbSecond.IsEnabled = true;
                    rbThird.IsEnabled = true;
                } catch (Exception exa) {
                    btnPlay.IsEnabled = false;
                    rbFirst.IsEnabled = false;
                    rbSecond.IsEnabled = false;
                    rbThird.IsEnabled = false;
                    MessageBox.Show("It is not possible to play the game due to the failure of downloading the guestions from xml-file!\n\nIt is advisable to be in touch with the developer of the game. Questions can be addressed directly to janne.kalliokulju@gmail.com", "Alert", MessageBoxButton.OK);
                }
                return;
            } catch (Exception exa) { 
                try {
                    string langXMLPath = Path.Combine("", "Assets/languages.xml");
                    XDocument loadedData = XDocument.Load(langXMLPath);
                    btnPlay.IsEnabled = true;
                    rbFirst.IsEnabled = true;
                    rbSecond.IsEnabled = true;
                    rbThird.IsEnabled = true;
                } catch (Exception ex) {
                    btnPlay.IsEnabled = false;
                    rbFirst.IsEnabled = false;
                    rbSecond.IsEnabled = false;
                    rbThird.IsEnabled = false;
                    MessageBox.Show("It is not possible to play the game due to the failure of downloading the guestions from xml-file!", "Alert", MessageBoxButton.OK);
                }

                return;
            }
        }

        // Sample code for building a localized ApplicationBar
        private  void BuildLocalizedApplicationBar() {
            // Set the page's ApplicationBar to a new instance of ApplicationBl3l300d##Y#Wdar.
            ApplicationBar = new ApplicationBar();
            ApplicationBar.Mode = ApplicationBarMode.Default;
            ApplicationBar.Opacity = 1.0;
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;
            ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem("Privacy Policy");
            ApplicationBar.MenuItems.Add(appBarMenuItem);
            appBarMenuItem.Click += new EventHandler(menuItem1_Click);
        }

        private void menuItem1_Click(object sender, EventArgs e) {
            openPPSite();
        }

        private async void openPPSite() {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("http://javacoder.eu/poliglot/privacypolicyPoliglot.html"));
        }

    }
}