using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Runtime.Serialization;
using System.Windows.Navigation;
using Microsoft.WindowsAzure.MobileServices;
using Windows.Foundation.Collections;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Threading;
using Newtonsoft.Json;
using Windows.Phone.Speech.Synthesis;

using System.Xml.Linq;
using Windows.ApplicationModel;


namespace Poliglot {
    public partial class GamePlayPage : PhoneApplicationPage {

        private IMobileServiceTable<LanguageSample> languageTable = App.MobileService.GetTable<LanguageSample>();
        IEnumerable<LanguageSample> languages;
        List<string> firstgroup_languages;
        List<string> secondgroup_languages;
        List<string> thirdgroup_languages;

        List<string> firstgroup_samples;
        List<string> secondgroup_samples;
        List<string> thirdgroup_samples;

        List<int> first_selectedLanguagesList;
        List<int> second_selectedLanguagesList;
        List<int> third_selectedLanguagesList;

        string correct_language = "";
        private int languageInOrderCount = 1; //starting from 1
        System.Windows.Threading.DispatcherTimer dt= new System.Windows.Threading.DispatcherTimer();

        int timeCounter = 5;

        public GamePlayPage(){
            InitializeComponent();
        }

        //in case user leaves the page unexpectedly
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e){
            base.OnNavigatedFrom(e);
            timeCounter = 0;
            dt.Stop();
            dt = null;
            firstgroup_languages = null; 
            secondgroup_languages = null; 
            thirdgroup_languages = null;
            firstgroup_samples = null;
            secondgroup_samples = null;
            thirdgroup_samples = null;
            first_selectedLanguagesList = null;
            second_selectedLanguagesList = null;
            third_selectedLanguagesList = null;
            correct_language = "";
            languageInOrderCount = 1; //starting from 1
            tbQuestionNumber.Text="1";
            tbRound.Text="1";
            tbScore.Text = "0";
            tbLevel.Text = "1";
            tb4Language.Text = "";
            tb1Language.Text = ""; 
            tb2Language.Text = ""; 
            tb3Language.Text = ""; 
            tbTimeToAnswer.Text="";
            tbTitle.Text="Playing Page";
            tbLanguageSample.Text = "";
        }

        void dt_Tick(object sender, EventArgs e){
            //new question from LIST
            if (timeCounter == 0){
                dt.Stop();
                int questionNumber = int.Parse(tbQuestionNumber.Text);
                int round = int.Parse(tbRound.Text);
                if (questionNumber == 2 && round == 3){
                    tbQuestionNumber.Text = "3";
                    //MessageBox.Show("Before prepare" + questionNumber + " ja " + round);
                    prepareScreenToShowResultOfGame();
                    return;
                } else {
                    if (questionNumber == 3){
                        questionNumber = 1;
                        tbQuestionNumber.Text = questionNumber.ToString();
                        round++;
                        tbRound.Text = round.ToString();
                    } else {
                        questionNumber++;
                        tbQuestionNumber.Text = questionNumber.ToString();
                    }
                }

                //take the next question automatically 
                int firstCount = firstgroup_languages.Count();
                int secondCount = secondgroup_languages.Count();
                int thirdCount = thirdgroup_languages.Count();
                Random r = new Random();
                languageInOrderCount++;

                //take next question
                if (languageInOrderCount <= 2)  { // 1 2
                    int ind1 = first_selectedLanguagesList.ElementAt(languageInOrderCount);
                    int correct_answer_position = r.Next(1, 5);
                    if (correct_answer_position == 1)  {
                        correct_language = firstgroup_languages.ElementAt(ind1);
                        tb1Language.Text = correct_language;
                        List<int> fakeAlternativesList = new List<int>();
                        while (fakeAlternativesList.Count < 3) {
                            int luku = r.Next(1, firstCount);
                            if (!fakeAlternativesList.Contains(luku) && ind1 != luku){
                                fakeAlternativesList.Add(luku);
                            }
                        }
                        tb2Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                        tb3Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                        tb4Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                    } else if (correct_answer_position == 2) {
                        correct_language = firstgroup_languages.ElementAt(ind1);
                        tb2Language.Text = correct_language;
                        List<int> fakeAlternativesList = new List<int>();
                        while (fakeAlternativesList.Count < 3) {
                            int luku = r.Next(1, firstCount);
                            if (!fakeAlternativesList.Contains(luku) && ind1 != luku){
                                fakeAlternativesList.Add(luku);
                            }
                        }
                        tb1Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                        tb3Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                        tb4Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                    }  else if (correct_answer_position == 3){
                        correct_language = firstgroup_languages.ElementAt(ind1);
                        tb3Language.Text = correct_language;
                        List<int> fakeAlternativesList = new List<int>();
                        while (fakeAlternativesList.Count < 3){
                            int luku = r.Next(1, firstCount);
                            if (!fakeAlternativesList.Contains(luku) && ind1 != luku){
                                fakeAlternativesList.Add(luku);
                            }
                        }
                        tb1Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                        tb2Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                        tb4Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                    } else if (correct_answer_position == 4) {
                        correct_language = firstgroup_languages.ElementAt(ind1);
                        tb4Language.Text = correct_language;
                        List<int> fakeAlternativesList = new List<int>();
                        while (fakeAlternativesList.Count < 3){
                            int luku = r.Next(1, firstCount);
                            if (!fakeAlternativesList.Contains(luku) && ind1 != luku) {
                                fakeAlternativesList.Add(luku);
                            }
                        }
                        tb1Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                        tb2Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                        tb3Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                    }
                    tbLanguageSample.Text = firstgroup_samples.ElementAt(ind1);
                } else if (languageInOrderCount <= 5) {//3 4 5
                    int ind1 = second_selectedLanguagesList.ElementAt(languageInOrderCount - 3);
                    int correct_answer_position = r.Next(1, 5);
                    if (correct_answer_position == 1){
                        correct_language = secondgroup_languages.ElementAt(ind1);
                        tb1Language.Text = correct_language;
                        List<int> fakeAlternativesList = new List<int>();
                        while (fakeAlternativesList.Count < 3){
                            int luku = r.Next(1, secondCount);
                            if (!fakeAlternativesList.Contains(luku) && ind1 != luku){
                                fakeAlternativesList.Add(luku);
                            }
                        }
                        tb2Language.Text = secondgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                        tb3Language.Text = secondgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                        tb4Language.Text = secondgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                    } else if (correct_answer_position == 2)  {
                        correct_language = secondgroup_languages.ElementAt(ind1);
                        tb2Language.Text = correct_language;
                        List<int> fakeAlternativesList = new List<int>();
                        while (fakeAlternativesList.Count < 3){
                            int luku = r.Next(1, secondCount);
                            if (!fakeAlternativesList.Contains(luku) && ind1 != luku){
                                fakeAlternativesList.Add(luku);
                            }
                        }
                        tb1Language.Text = secondgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                        tb3Language.Text = secondgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                        tb4Language.Text = secondgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                    } else if (correct_answer_position == 3) {
                        correct_language = secondgroup_languages.ElementAt(ind1);
                        tb3Language.Text = correct_language;
                        List<int> fakeAlternativesList = new List<int>();
                        while (fakeAlternativesList.Count < 3) {
                            int luku = r.Next(1, secondCount);
                            if (!fakeAlternativesList.Contains(luku) && ind1 != luku){
                                fakeAlternativesList.Add(luku);
                            }
                        }
                        tb1Language.Text = secondgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                        tb2Language.Text = secondgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                        tb4Language.Text = secondgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                    } else if (correct_answer_position == 4) {
                        correct_language = secondgroup_languages.ElementAt(ind1);
                        tb4Language.Text = correct_language;
                        List<int> fakeAlternativesList = new List<int>();
                        while (fakeAlternativesList.Count < 3){
                            int luku = r.Next(1, secondCount);
                            if (!fakeAlternativesList.Contains(luku) && ind1 != luku){
                                fakeAlternativesList.Add(luku);
                            }
                        }
                        tb1Language.Text = secondgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                        tb2Language.Text = secondgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                        tb3Language.Text = secondgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                    }
                    tbLanguageSample.Text = secondgroup_samples.ElementAt(ind1);
                } else if (languageInOrderCount <= 8) { //6 7 8
                    int ind1 = third_selectedLanguagesList.ElementAt(languageInOrderCount - 6);
                    int correct_answer_position = r.Next(1, 5);
                    if (correct_answer_position == 1){
                        correct_language = thirdgroup_languages.ElementAt(ind1);
                        tb1Language.Text = correct_language;
                        List<int> fakeAlternativesList = new List<int>();
                        while (fakeAlternativesList.Count < 3){
                            int luku = r.Next(1, thirdCount);
                            if (!fakeAlternativesList.Contains(luku) && ind1 != luku){
                                fakeAlternativesList.Add(luku);
                            }
                        }
                        tb2Language.Text = thirdgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                        tb3Language.Text = thirdgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                        tb4Language.Text = thirdgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                    } else if (correct_answer_position == 2){
                        correct_language = thirdgroup_languages.ElementAt(ind1);
                        tb2Language.Text = correct_language;
                        List<int> fakeAlternativesList = new List<int>();
                        while (fakeAlternativesList.Count < 3){
                            int luku = r.Next(1, thirdCount);
                            if (!fakeAlternativesList.Contains(luku) && ind1 != luku){
                                fakeAlternativesList.Add(luku);
                            }
                        }
                        tb1Language.Text = thirdgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                        tb3Language.Text = thirdgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                        tb4Language.Text = thirdgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                    } else if (correct_answer_position == 3){
                        correct_language = thirdgroup_languages.ElementAt(ind1);
                        tb3Language.Text = correct_language;
                        List<int> fakeAlternativesList = new List<int>();
                        while (fakeAlternativesList.Count < 3){
                            int luku = r.Next(1, thirdCount);
                            if (!fakeAlternativesList.Contains(luku) && ind1 != luku) {
                                fakeAlternativesList.Add(luku);
                            }
                        }
                        tb1Language.Text = thirdgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                        tb2Language.Text = thirdgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                        tb4Language.Text = thirdgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                    } else if (correct_answer_position == 4) {
                        correct_language = thirdgroup_languages.ElementAt(ind1);
                        tb4Language.Text = correct_language;
                        List<int> fakeAlternativesList = new List<int>();
                        while (fakeAlternativesList.Count < 3)  {
                            int luku = r.Next(1, thirdCount);
                            if (!fakeAlternativesList.Contains(luku) && ind1 != luku){
                                fakeAlternativesList.Add(luku);
                            }
                        }
                        tb1Language.Text = thirdgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                        tb2Language.Text = thirdgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                        tb3Language.Text = thirdgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                    }
                    tbLanguageSample.Text = thirdgroup_samples.ElementAt(ind1);
                }
                //increment question & possibly round counter
                tbTimeToAnswer.Text = timeCounter.ToString();
                timeCounter = 5;
                dt.Start();
            } else {
                tbTimeToAnswer.Text = timeCounter.ToString();
                timeCounter--;
            }
        }

        private async void getLanguageSamples(string level) {
            try {
                if (level != null) {
                    int d_level = int.Parse(level);

                    firstgroup_languages = new List<string>(); 
                    secondgroup_languages = new List<string>(); 
                    thirdgroup_languages = new List<string>(); 
                    firstgroup_samples = new List<string>(); 
                    secondgroup_samples = new List<string>(); 
                    thirdgroup_samples = new List<string>();

                    /* LEVEL 1:
                     * 1, 2 ja 3
                     * */
                    if(d_level==1){ 

                        languages = await languageTable.Where(LanguageSample => LanguageSample.Level == 1).ToEnumerableAsync();
                        foreach (var language in languages) { //languages 1
                            firstgroup_languages.Add(language.Language);
                            firstgroup_samples.Add(language.Sample);
                        }
                        languages = await languageTable.Where(LanguageSample => LanguageSample.Level == 2).ToEnumerableAsync();
                        foreach (var language in languages){ //languages 2
                            secondgroup_languages.Add(language.Language);
                            secondgroup_samples.Add(language.Sample);
                        }
                        languages = await languageTable.Where(LanguageSample => LanguageSample.Level == 3).ToEnumerableAsync();
                        foreach (var language in languages){ //languages 3
                            thirdgroup_languages.Add(language.Language);
                            thirdgroup_samples.Add(language.Sample);
                        }


                        //choose randomly 3 languages for 1. language
                        int firstCount = firstgroup_languages.Count(); 
                        first_selectedLanguagesList = new List<int>();
                        Random r = new Random();
                        while (first_selectedLanguagesList.Count < 3)  {
                            int luku = r.Next(0, firstCount);
                            if (!first_selectedLanguagesList.Contains(luku)){
                                first_selectedLanguagesList.Add(luku);
                            }
                        }
                        
                        //choose randomly 3 languages for 2. language
                        int secondCount = secondgroup_languages.Count();
                        second_selectedLanguagesList = new List<int>();
                        while (second_selectedLanguagesList.Count < 3) {
                            int luku = r.Next(0, secondCount);
                            if (!second_selectedLanguagesList.Contains(luku)) {
                                second_selectedLanguagesList.Add(luku);
                            }
                        }

                        //choose randomly 3 languages for 3. language
                        int thirdCount = thirdgroup_languages.Count();
                        third_selectedLanguagesList = new List<int>();
                        while (third_selectedLanguagesList.Count < 3) {
                            int luku = r.Next(0, thirdCount);
                            if (!third_selectedLanguagesList.Contains(luku)){
                                third_selectedLanguagesList.Add(luku);
                            }
                        }

                        //Fake alternatives for 1. guestion
                       //place the correct language and fake languagecorrect_answer_positions to their places 
                       int ind = first_selectedLanguagesList.ElementAt(0);
                       int correct_answer_position = r.Next(1, 5);
                       if (correct_answer_position == 1) {
                           correct_language = firstgroup_languages.ElementAt(ind);
                           tb1Language.Text = correct_language;
                           // ind (2) firstCount koko määrä
                           List<int> fakeAlternativesList = new List<int>();
                           while (fakeAlternativesList.Count < 3) {
                               int luku = r.Next(1, firstCount);
                               if (!fakeAlternativesList.Contains(luku) && ind != luku){
                                   fakeAlternativesList.Add(luku);
                               }
                           }
                           tb2Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                           tb3Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                           tb4Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                       } else if (correct_answer_position == 2) {
                           correct_language = firstgroup_languages.ElementAt(ind);
                           tb2Language.Text = correct_language;
                           List<int> fakeAlternativesList = new List<int>();
                           while (fakeAlternativesList.Count < 3){
                               int luku = r.Next(1, firstCount);
                               if (!fakeAlternativesList.Contains(luku) && ind != luku) {
                                   fakeAlternativesList.Add(luku);
                               }
                           }
                           tb1Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                           tb3Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                           tb4Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                       } else if (correct_answer_position == 3) {
                           correct_language = firstgroup_languages.ElementAt(ind);
                           tb3Language.Text = correct_language;
                           List<int> fakeAlternativesList = new List<int>();
                           while (fakeAlternativesList.Count < 3) {
                               int luku = r.Next(1, firstCount);
                               if (!fakeAlternativesList.Contains(luku) && ind != luku) {
                                   fakeAlternativesList.Add(luku);
                               }
                           }
                           tb1Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                           tb2Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                           tb4Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                       } else if (correct_answer_position == 4) {
                           correct_language = firstgroup_languages.ElementAt(ind);
                           tb4Language.Text = correct_language;
                           List<int> fakeAlternativesList = new List<int>();
                           while (fakeAlternativesList.Count < 3 ) {
                               int luku = r.Next(1, firstCount);
                               if (!fakeAlternativesList.Contains(luku) && ind != luku) {
                                   fakeAlternativesList.Add(luku);
                               }
                           }
                           tb1Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                           tb2Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                           tb3Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                       }
                       tbLanguageSample.Text = firstgroup_samples.ElementAt(ind);
                    }

                    /* LEVEL 2:
                     * 2, 3 ja 4
                     * */
                    if(d_level==2){ 
                        languages = await languageTable.Where(LanguageSample => LanguageSample.Level == 2).ToEnumerableAsync();
                        foreach (var language in languages)  {
                            firstgroup_languages.Add(language.Language);
                            firstgroup_samples.Add(language.Sample);
                        }
                        languages = await languageTable.Where(LanguageSample => LanguageSample.Level == 3).ToEnumerableAsync();
                        foreach (var language in languages)  {
                            secondgroup_languages.Add(language.Language);
                            secondgroup_samples.Add(language.Sample);
                        }
                        languages = await languageTable.Where(LanguageSample => LanguageSample.Level == 4).ToEnumerableAsync();
                        foreach (var language in languages) {
                            thirdgroup_languages.Add(language.Language);
                            thirdgroup_samples.Add(language.Sample);
                        }

                        //choose randomly 3 languages for 1. language
                        int firstCount = firstgroup_languages.Count();
                        first_selectedLanguagesList = new List<int>();
                        Random r = new Random();
                        while (first_selectedLanguagesList.Count < 3){
                            int luku = r.Next(0, firstCount);
                            if (!first_selectedLanguagesList.Contains(luku)) {
                                first_selectedLanguagesList.Add(luku);
                            }
                        }

                        //choose randomly 3 languages for 2. language
                        int secondCount = secondgroup_languages.Count();
                        second_selectedLanguagesList = new List<int>();
                        while (second_selectedLanguagesList.Count < 3) {
                            int luku = r.Next(0, secondCount);
                            if (!second_selectedLanguagesList.Contains(luku)){
                                second_selectedLanguagesList.Add(luku);
                            }
                        }

                        //choose randomly 3 languages for 3. language
                        int thirdCount = thirdgroup_languages.Count();
                        third_selectedLanguagesList = new List<int>();
                        while (third_selectedLanguagesList.Count < 3) {
                            int luku = r.Next(0, thirdCount);
                            if (!third_selectedLanguagesList.Contains(luku)) {
                                third_selectedLanguagesList.Add(luku);
                            }
                        }

                        //Fake alternatives for 1. guestion
                        //place the correct language and fake languagecorrect_answer_positions to their places 
                        int ind = first_selectedLanguagesList.ElementAt(0);
                        int correct_answer_position = r.Next(1, 5);
                        if (correct_answer_position == 1){
                            correct_language = firstgroup_languages.ElementAt(ind);
                            tb1Language.Text = correct_language;
                            // ind (2) firstCount koko määrä
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3){
                                int luku = r.Next(1, firstCount);
                                if (!fakeAlternativesList.Contains(luku) && ind != luku){
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb2Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb3Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb4Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        } else if (correct_answer_position == 2) {
                            correct_language = firstgroup_languages.ElementAt(ind);
                            tb2Language.Text = correct_language;
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3) {
                                int luku = r.Next(1, firstCount);
                                if (!fakeAlternativesList.Contains(luku) && ind != luku) {
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb1Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb3Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb4Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        } else if (correct_answer_position == 3) {
                            correct_language = firstgroup_languages.ElementAt(ind);
                            tb3Language.Text = correct_language;
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3){
                                int luku = r.Next(1, firstCount);
                                if (!fakeAlternativesList.Contains(luku) && ind != luku){
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb1Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb2Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb4Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        } else if (correct_answer_position == 4){
                            correct_language = firstgroup_languages.ElementAt(ind);
                            tb4Language.Text = correct_language;
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3) {
                                int luku = r.Next(1, firstCount);
                                if (!fakeAlternativesList.Contains(luku) && ind != luku) {
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb1Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb2Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb3Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        }
                        tbLanguageSample.Text = firstgroup_samples.ElementAt(ind);

                    }


                    /* LEVEL 3:
                     * 3, 4 ja 5
                    **/
                    if (d_level == 3) {
                        languages = await languageTable.Where(LanguageSample => LanguageSample.Level == 3).ToEnumerableAsync();
                        foreach (var language in languages) {
                            firstgroup_languages.Add(language.Language);
                            firstgroup_samples.Add(language.Sample);
                        }
                        languages = await languageTable.Where(LanguageSample => LanguageSample.Level == 4).ToEnumerableAsync();
                        foreach (var language in languages) {
                            secondgroup_languages.Add(language.Language);
                            secondgroup_samples.Add(language.Sample);
                        }
                        languages = await languageTable.Where(LanguageSample => LanguageSample.Level == 5).ToEnumerableAsync();
                        foreach (var language in languages) {
                            thirdgroup_languages.Add(language.Language);
                            thirdgroup_samples.Add(language.Sample);
                        }

                        //choose randomly 3 languages for 1. language
                        int firstCount = firstgroup_languages.Count();
                        first_selectedLanguagesList = new List<int>();
                        Random r = new Random();
                        while (first_selectedLanguagesList.Count < 3) {
                            int luku = r.Next(0, firstCount);
                            if (!first_selectedLanguagesList.Contains(luku)) {
                                first_selectedLanguagesList.Add(luku);
                            }
                        }

                        //choose randomly 3 languages for 2. language
                        int secondCount = secondgroup_languages.Count();
                        second_selectedLanguagesList = new List<int>();
                        while (second_selectedLanguagesList.Count < 3) {
                            int luku = r.Next(0, secondCount);
                            if (!second_selectedLanguagesList.Contains(luku)) {
                                second_selectedLanguagesList.Add(luku);
                            }
                        }

                        //choose randomly 3 languages for 3. language
                        int thirdCount = thirdgroup_languages.Count();
                        third_selectedLanguagesList = new List<int>();
                        while (third_selectedLanguagesList.Count < 3) {
                            int luku = r.Next(0, thirdCount);
                            if (!third_selectedLanguagesList.Contains(luku)) {
                                third_selectedLanguagesList.Add(luku);
                            }
                        }

                        //Fake alternatives for 1. guestion
                        //place the correct language and fake languagecorrect_answer_positions to their places 
                        int ind = first_selectedLanguagesList.ElementAt(0);
                        int correct_answer_position = r.Next(1, 5);
                        if (correct_answer_position == 1){
                            correct_language = firstgroup_languages.ElementAt(ind);
                            tb1Language.Text = correct_language;
                            // ind (2) firstCount koko määrä
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3) {
                                int luku = r.Next(1, firstCount);
                                if (!fakeAlternativesList.Contains(luku) && ind != luku) {
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb2Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb3Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb4Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        } else if (correct_answer_position == 2) {
                            correct_language = firstgroup_languages.ElementAt(ind);
                            tb2Language.Text = correct_language;
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3){
                                int luku = r.Next(1, firstCount);
                                if (!fakeAlternativesList.Contains(luku) && ind != luku) {
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb1Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb3Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb4Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        } else if (correct_answer_position == 3){
                            correct_language = firstgroup_languages.ElementAt(ind);
                            tb3Language.Text = correct_language;
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3){
                                int luku = r.Next(1, firstCount);
                                if (!fakeAlternativesList.Contains(luku) && ind != luku) {
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb1Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb2Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb4Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        }else if (correct_answer_position == 4) {
                            correct_language = firstgroup_languages.ElementAt(ind);
                            tb4Language.Text = correct_language;
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3) {
                                int luku = r.Next(1, firstCount);
                                if (!fakeAlternativesList.Contains(luku) && ind != luku){
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb1Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb2Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb3Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        }
                        tbLanguageSample.Text = firstgroup_samples.ElementAt(ind);
                    } //end of level 3
                    recTimeRunningBack.Visibility = Visibility.Visible;
                    tbTimeRunningLabel.Visibility = Visibility.Visible;
                    tbTimeToAnswer.Visibility = Visibility.Visible;

                    timeCounter = 5;
                    tbTimeToAnswer.Text = timeCounter.ToString();
                    dt.Interval = new TimeSpan(0, 0, 0, 0, 1000);
                    dt.Tick += new EventHandler(dt_Tick);
                    dt.Start();
                }
            } catch (MobileServiceInvalidOperationException e) {
                try {
                    int d_level = int.Parse(tbLevel.Text);
                    string langXMLPath = Path.Combine("", "Assets/languages.xml");
                    XDocument loadedData = XDocument.Load(langXMLPath);
                    var data = from query in loadedData.Descendants("language")
                               select new Language {
                                   Name = (string)query.Element("name"),
                                   Level = (int)query.Element("level"),
                                   Sample = (string)query.Element("sample")
                               };
                    if (d_level == 1)
                    {//languages 1 2 3
                        foreach (var language in data) { 
                            if (language.Level == 1) {
                                firstgroup_languages.Add(language.Name);
                                firstgroup_samples.Add(language.Sample);
                            } else if (language.Level == 2) {
                                secondgroup_languages.Add(language.Name);
                                secondgroup_samples.Add(language.Sample);
                            } else if (language.Level == 3) {
                                thirdgroup_languages.Add(language.Name);
                                thirdgroup_samples.Add(language.Sample);
                           }                            
                        }

                        //choose randomly 3 languages for 1. language
                        int firstCount = firstgroup_languages.Count();
                        first_selectedLanguagesList = new List<int>();
                        Random r = new Random();
                        while (first_selectedLanguagesList.Count < 3){
                            int luku = r.Next(0, firstCount);
                            if (!first_selectedLanguagesList.Contains(luku)){
                                first_selectedLanguagesList.Add(luku);
                            }
                        }

                        //choose randomly 3 languages for 2. language
                        int secondCount = secondgroup_languages.Count();
                        second_selectedLanguagesList = new List<int>();
                        while (second_selectedLanguagesList.Count < 3){
                            int luku = r.Next(0, secondCount);
                            if (!second_selectedLanguagesList.Contains(luku)){
                                second_selectedLanguagesList.Add(luku);
                            }
                        }

                        //choose randomly 3 languages for 3. language
                        int thirdCount = thirdgroup_languages.Count();
                        third_selectedLanguagesList = new List<int>();
                        while (third_selectedLanguagesList.Count < 3) {
                            int luku = r.Next(0, thirdCount);
                            if (!third_selectedLanguagesList.Contains(luku)) {
                                third_selectedLanguagesList.Add(luku);
                            }
                        }

                        //Fake alternatives for 1. guestion
                        //place the correct language and fake languagecorrect_answer_positions to their places 
                        int ind = first_selectedLanguagesList.ElementAt(0);
                        int correct_answer_position = r.Next(1, 5);
                        if (correct_answer_position == 1) {
                            correct_language = firstgroup_languages.ElementAt(ind);
                            tb1Language.Text = correct_language;
                            // ind (2) firstCount koko määrä
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3){
                                int luku = r.Next(1, firstCount);
                                if (!fakeAlternativesList.Contains(luku) && ind != luku){
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb2Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb3Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb4Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        } else if (correct_answer_position == 2) {
                            correct_language = firstgroup_languages.ElementAt(ind);
                            tb2Language.Text = correct_language;
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3){
                                int luku = r.Next(1, firstCount);
                                if (!fakeAlternativesList.Contains(luku) && ind != luku){
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb1Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb3Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb4Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        } else if (correct_answer_position == 3){
                            correct_language = firstgroup_languages.ElementAt(ind);
                            tb3Language.Text = correct_language;
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3){
                                int luku = r.Next(1, firstCount);
                                if (!fakeAlternativesList.Contains(luku) && ind != luku){
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb1Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb2Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb4Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        } else if (correct_answer_position == 4){
                            correct_language = firstgroup_languages.ElementAt(ind);
                            tb4Language.Text = correct_language;
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3){
                                int luku = r.Next(1, firstCount);
                                if (!fakeAlternativesList.Contains(luku) && ind != luku){
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb1Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb2Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb3Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        }
                        tbLanguageSample.Text = firstgroup_samples.ElementAt(ind);


                    } else if (d_level == 2) {
                        //languages 2 3 4
                        foreach (var language in data)
                        { 
                            if (language.Level == 2) {
                                firstgroup_languages.Add(language.Name);
                                firstgroup_samples.Add(language.Sample);
                            } else if (language.Level ==3) {
                                secondgroup_languages.Add(language.Name);
                                secondgroup_samples.Add(language.Sample);
                            } else if (language.Level == 4) {
                                thirdgroup_languages.Add(language.Name);
                                thirdgroup_samples.Add(language.Sample);
                            }
                        }

                        //choose randomly 3 languages for 1. language
                        int firstCount = firstgroup_languages.Count();
                        first_selectedLanguagesList = new List<int>();
                        Random r = new Random();
                        while (first_selectedLanguagesList.Count < 3) {
                            int luku = r.Next(0, firstCount);
                            if (!first_selectedLanguagesList.Contains(luku)) {
                                first_selectedLanguagesList.Add(luku);
                            }
                        }

                        //choose randomly 3 languages for 2. language
                        int secondCount = secondgroup_languages.Count();
                        second_selectedLanguagesList = new List<int>();
                        while (second_selectedLanguagesList.Count < 3){
                            int luku = r.Next(0, secondCount);
                            if (!second_selectedLanguagesList.Contains(luku)) {
                                second_selectedLanguagesList.Add(luku);
                            }
                        }

                        //choose randomly 3 languages for 3. language
                        int thirdCount = thirdgroup_languages.Count();
                        third_selectedLanguagesList = new List<int>();
                        while (third_selectedLanguagesList.Count < 3) {
                            int luku = r.Next(0, thirdCount);
                            if (!third_selectedLanguagesList.Contains(luku)){
                                third_selectedLanguagesList.Add(luku);
                            }
                        }

                        //Fake alternatives for 1. guestion
                        //place the correct language and fake languagecorrect_answer_positions to their places 
                        int ind = first_selectedLanguagesList.ElementAt(0);
                        int correct_answer_position = r.Next(1, 5);
                        if (correct_answer_position == 1) {
                            correct_language = firstgroup_languages.ElementAt(ind);
                            tb1Language.Text = correct_language;
                            // ind (2) firstCount koko määrä
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3) {
                                int luku = r.Next(1, firstCount);
                                if (!fakeAlternativesList.Contains(luku) && ind != luku){
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb2Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb3Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb4Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        } else if (correct_answer_position == 2){
                            correct_language = firstgroup_languages.ElementAt(ind);
                            tb2Language.Text = correct_language;
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3){
                                int luku = r.Next(1, firstCount);
                                if (!fakeAlternativesList.Contains(luku) && ind != luku){
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb1Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb3Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb4Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        } else if (correct_answer_position == 3){
                            correct_language = firstgroup_languages.ElementAt(ind);
                            tb3Language.Text = correct_language;
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3) {
                                int luku = r.Next(1, firstCount);
                                if (!fakeAlternativesList.Contains(luku) && ind != luku){
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb1Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb2Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb4Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        } else if (correct_answer_position == 4) {
                            correct_language = firstgroup_languages.ElementAt(ind);
                            tb4Language.Text = correct_language;
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3) {
                                int luku = r.Next(1, firstCount);
                                if (!fakeAlternativesList.Contains(luku) && ind != luku){
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb1Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb2Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb3Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        }
                        tbLanguageSample.Text = firstgroup_samples.ElementAt(ind);



                    } else if (d_level == 3){
                        //languages 3 4 5
                        foreach (var language in data) { 
                             if (language.Level == 3) {
                                firstgroup_languages.Add(language.Name);
                                firstgroup_samples.Add(language.Sample);
                            } else if (language.Level == 4) {
                                secondgroup_languages.Add(language.Name);
                                secondgroup_samples.Add(language.Sample);
                            } else if (language.Level == 5) {
                                thirdgroup_languages.Add(language.Name);
                                thirdgroup_samples.Add(language.Sample);
                            }
                        }

                        //choose randomly 3 languages for 1. language
                        int firstCount = firstgroup_languages.Count();
                        first_selectedLanguagesList = new List<int>();
                        Random r = new Random();
                        while (first_selectedLanguagesList.Count < 3)  {
                            int luku = r.Next(0, firstCount);
                            if (!first_selectedLanguagesList.Contains(luku)){
                                first_selectedLanguagesList.Add(luku);
                            }
                        }

                        //choose randomly 3 languages for 2. language
                        int secondCount = secondgroup_languages.Count();
                        second_selectedLanguagesList = new List<int>();
                        while (second_selectedLanguagesList.Count < 3) {
                            int luku = r.Next(0, secondCount);
                            if (!second_selectedLanguagesList.Contains(luku)) {
                                second_selectedLanguagesList.Add(luku);
                            }
                        }

                        //choose randomly 3 languages for 3. language
                        int thirdCount = thirdgroup_languages.Count();
                        third_selectedLanguagesList = new List<int>();
                        while (third_selectedLanguagesList.Count < 3) {
                            int luku = r.Next(0, thirdCount);
                            if (!third_selectedLanguagesList.Contains(luku)){
                                third_selectedLanguagesList.Add(luku);
                            }
                        }

                        //Fake alternatives for 1. guestion
                        //place the correct language and fake languagecorrect_answer_positions to their places 
                        int ind = first_selectedLanguagesList.ElementAt(0);
                        int correct_answer_position = r.Next(1, 5);
                        if (correct_answer_position == 1)  {
                            correct_language = firstgroup_languages.ElementAt(ind);
                            tb1Language.Text = correct_language;
                            // ind (2) firstCount koko määrä
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3) {
                                int luku = r.Next(1, firstCount);
                                if (!fakeAlternativesList.Contains(luku) && ind != luku) {
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb2Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb3Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb4Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        } else if (correct_answer_position == 2) {
                            correct_language = firstgroup_languages.ElementAt(ind);
                            tb2Language.Text = correct_language;
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3)  {
                                int luku = r.Next(1, firstCount);
                                if (!fakeAlternativesList.Contains(luku) && ind != luku)  {
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb1Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb3Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb4Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        } else if (correct_answer_position == 3) {
                            correct_language = firstgroup_languages.ElementAt(ind);
                            tb3Language.Text = correct_language;
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3) {
                                int luku = r.Next(1, firstCount);
                                if (!fakeAlternativesList.Contains(luku) && ind != luku) {
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb1Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb2Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb4Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        } else if (correct_answer_position == 4) {
                            correct_language = firstgroup_languages.ElementAt(ind);
                            tb4Language.Text = correct_language;
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3){
                                int luku = r.Next(1, firstCount);
                                if (!fakeAlternativesList.Contains(luku) && ind != luku) {
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb1Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb2Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb3Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        }
                        tbLanguageSample.Text = firstgroup_samples.ElementAt(ind);

                    }
                    recTimeRunningBack.Visibility = Visibility.Visible;
                    tbTimeRunningLabel.Visibility = Visibility.Visible;
                    tbTimeToAnswer.Visibility = Visibility.Visible;

                    timeCounter = 5;
                    tbTimeToAnswer.Text = timeCounter.ToString();
                    dt.Interval = new TimeSpan(0, 0, 0, 0, 1000);
                    dt.Tick += new EventHandler(dt_Tick);
                    dt.Start();
                //    MessageBox.Show("DONE");
                } catch (Exception ex) {
                    //    MessageBox.Show("Problems");
                }
            } catch (Exception ex) {
                //MessageBox.Show("Exception " + ex.ToString());
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e){
            base.OnNavigatedTo(e);
            string level = "";
            if (NavigationContext.QueryString.TryGetValue("level", out level)) {
                tbLevel.Text = level;
                tbQuestionNumber.Text="1";
                tbRound.Text = "1";
                tbScore.Text = "0";
                getLanguageSamples(level);
            } 
        }

        private async void correct() {
            SpeechSynthesizer synth = new SpeechSynthesizer();
            await synth.SpeakTextAsync("Correct");
        }
    
        private async void wrong() {
            SpeechSynthesizer synth = new SpeechSynthesizer();
            await synth.SpeakTextAsync("Wrong");
        }

        /*
                first_selectedLanguagesList         0 1 2
                second_selectedLanguagesList        0 1 2 
                third_selectedLanguagesList         0 1 s
                languageInOrderCount
         */
        private void tbTapItemEvent(object sender, System.Windows.Input.GestureEventArgs e) {
            if (correct_language != null) {
                dt.Stop();

                // take care of displaying the scores and other stuff
                if (((TextBlock)sender).Text.Equals(correct_language)) {
                    int score = int.Parse(tbScore.Text); 
                    int level= int.Parse(tbLevel.Text);
                    int multiplier=int.Parse(tbTimeToAnswer.Text);
                    score += level * multiplier;
                    tbScore.Text = score.ToString();
                    correct();

                    int questionNumber = int.Parse(tbQuestionNumber.Text);
                    int round = int.Parse(tbRound.Text);
                    if (questionNumber == 2 && round == 3) {
                        dt.Stop();
                        tbQuestionNumber.Text = "3";
                        prepareScreenToShowResultOfGame();
                        return;
                    }  else {
                        if (questionNumber == 3) {
                            questionNumber = 1;
                            tbQuestionNumber.Text = questionNumber.ToString();
                            round++;
                            tbRound.Text = round.ToString();
                        } else {
                            questionNumber++;
                            tbQuestionNumber.Text = questionNumber.ToString();
                        }
                    }

                    int firstCount = firstgroup_languages.Count();
                    int secondCount = secondgroup_languages.Count();
                    int thirdCount = thirdgroup_languages.Count();
                    Random r = new Random();

                    //take next question
                    if (languageInOrderCount <= 2)
                    { // 1 2
                        int ind1 = first_selectedLanguagesList.ElementAt(languageInOrderCount);
                        int correct_answer_position = r.Next(1, 5);
                        if (correct_answer_position == 1)
                        {
                            correct_language = firstgroup_languages.ElementAt(ind1);
                            tb1Language.Text = correct_language;
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3)
                            {
                                int luku = r.Next(1, firstCount);
                                if (!fakeAlternativesList.Contains(luku) && ind1 != luku)
                                {
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb2Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb3Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb4Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        }
                        else if (correct_answer_position == 2)
                        {
                            correct_language = firstgroup_languages.ElementAt(ind1);
                            tb2Language.Text = correct_language;
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3)
                            {
                                int luku = r.Next(1, firstCount);
                                if (!fakeAlternativesList.Contains(luku) && ind1 != luku)
                                {
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb1Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb3Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb4Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        }
                        else if (correct_answer_position == 3)
                        {
                            correct_language = firstgroup_languages.ElementAt(ind1);
                            tb3Language.Text = correct_language;
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3)
                            {
                                int luku = r.Next(1, firstCount);
                                if (!fakeAlternativesList.Contains(luku) && ind1 != luku)
                                {
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb1Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb2Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb4Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        }
                        else if (correct_answer_position == 4)
                        {
                            correct_language = firstgroup_languages.ElementAt(ind1);
                            tb4Language.Text = correct_language;
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3)
                            {
                                int luku = r.Next(1, firstCount);
                                if (!fakeAlternativesList.Contains(luku) && ind1 != luku)
                                {
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb1Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb2Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb3Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        }
                        tbLanguageSample.Text = firstgroup_samples.ElementAt(ind1);
                        languageInOrderCount++;
                    }
                    else if (languageInOrderCount <= 5)
                    {//3 4 5
                        int ind1 = second_selectedLanguagesList.ElementAt(languageInOrderCount - 3);
                        int correct_answer_position = r.Next(1, 5);
                        if (correct_answer_position == 1)
                        {
                            correct_language = secondgroup_languages.ElementAt(ind1);
                            tb1Language.Text = correct_language;
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3)
                            {
                                int luku = r.Next(1, secondCount);
                                if (!fakeAlternativesList.Contains(luku) && ind1 != luku)
                                {
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb2Language.Text = secondgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb3Language.Text = secondgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb4Language.Text = secondgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        }
                        else if (correct_answer_position == 2)
                        {
                            correct_language = secondgroup_languages.ElementAt(ind1);
                            tb2Language.Text = correct_language;
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3)
                            {
                                int luku = r.Next(1, secondCount);
                                if (!fakeAlternativesList.Contains(luku) && ind1 != luku)
                                {
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb1Language.Text = secondgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb3Language.Text = secondgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb4Language.Text = secondgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        }
                        else if (correct_answer_position == 3)
                        {
                            correct_language = secondgroup_languages.ElementAt(ind1);
                            tb3Language.Text = correct_language;
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3)
                            {
                                int luku = r.Next(1, secondCount);
                                if (!fakeAlternativesList.Contains(luku) && ind1 != luku)
                                {
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb1Language.Text = secondgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb2Language.Text = secondgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb4Language.Text = secondgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        }
                        else if (correct_answer_position == 4)
                        {
                            correct_language = secondgroup_languages.ElementAt(ind1);
                            tb4Language.Text = correct_language;
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3)
                            {
                                int luku = r.Next(1, secondCount);
                                if (!fakeAlternativesList.Contains(luku) && ind1 != luku)
                                {
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb1Language.Text = secondgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb2Language.Text = secondgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb3Language.Text = secondgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        }
                        tbLanguageSample.Text = secondgroup_samples.ElementAt(ind1);
                        languageInOrderCount++;

                    }
                    else if (languageInOrderCount <= 8)
                    { //6 7 8
                        int ind1 = third_selectedLanguagesList.ElementAt(languageInOrderCount - 6);
                        int correct_answer_position = r.Next(1, 5);
                        if (correct_answer_position == 1)
                        {
                            correct_language = thirdgroup_languages.ElementAt(ind1);
                            tb1Language.Text = correct_language;
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3)
                            {
                                int luku = r.Next(1, thirdCount);
                                if (!fakeAlternativesList.Contains(luku) && ind1 != luku)
                                {
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb2Language.Text = thirdgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb3Language.Text = thirdgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb4Language.Text = thirdgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        }
                        else if (correct_answer_position == 2)
                        {
                            correct_language = thirdgroup_languages.ElementAt(ind1);
                            tb2Language.Text = correct_language;
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3)
                            {
                                int luku = r.Next(1, thirdCount);
                                if (!fakeAlternativesList.Contains(luku) && ind1 != luku)
                                {
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb1Language.Text = thirdgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb3Language.Text = thirdgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb4Language.Text = thirdgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        }
                        else if (correct_answer_position == 3)
                        {
                            correct_language = thirdgroup_languages.ElementAt(ind1);
                            tb3Language.Text = correct_language;
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3)
                            {
                                int luku = r.Next(1, thirdCount);
                                if (!fakeAlternativesList.Contains(luku) && ind1 != luku)
                                {
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb1Language.Text = thirdgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb2Language.Text = thirdgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb4Language.Text = thirdgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        }
                        else if (correct_answer_position == 4)
                        {
                            correct_language = thirdgroup_languages.ElementAt(ind1);
                            tb4Language.Text = correct_language;
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3)
                            {
                                int luku = r.Next(1, thirdCount);
                                if (!fakeAlternativesList.Contains(luku) && ind1 != luku)
                                {
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb1Language.Text = thirdgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb2Language.Text = thirdgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb3Language.Text = thirdgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        }
                        tbLanguageSample.Text = thirdgroup_samples.ElementAt(ind1);
                        languageInOrderCount++;
                    }





                } else {
                    wrong();
                    int questionNumber = int.Parse(tbQuestionNumber.Text);
                    int round = int.Parse(tbRound.Text);
                    if (questionNumber == 2 && round == 3) {
                        dt.Stop();
                        tbQuestionNumber.Text = "3";
                        prepareScreenToShowResultOfGame();
                    }  else {
                        if (questionNumber == 3) {
                            questionNumber = 1;
                            tbQuestionNumber.Text = questionNumber.ToString();
                            round++;
                            tbRound.Text = round.ToString();
                        } else {
                            questionNumber++;
                            tbQuestionNumber.Text = questionNumber.ToString();
                        }
                    }

                    int firstCount = firstgroup_languages.Count();
                    int secondCount = secondgroup_languages.Count();
                    int thirdCount = thirdgroup_languages.Count();
                    Random r = new Random();

                    //take next question
                    if (languageInOrderCount <= 2)
                    { // 1 2
                        int ind1 = first_selectedLanguagesList.ElementAt(languageInOrderCount);
                        int correct_answer_position = r.Next(1, 5);
                        if (correct_answer_position == 1)
                        {
                            correct_language = firstgroup_languages.ElementAt(ind1);
                            tb1Language.Text = correct_language;
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3)
                            {
                                int luku = r.Next(1, firstCount);
                                if (!fakeAlternativesList.Contains(luku) && ind1 != luku)
                                {
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb2Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb3Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb4Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        }
                        else if (correct_answer_position == 2)
                        {
                            correct_language = firstgroup_languages.ElementAt(ind1);
                            tb2Language.Text = correct_language;
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3)
                            {
                                int luku = r.Next(1, firstCount);
                                if (!fakeAlternativesList.Contains(luku) && ind1 != luku)
                                {
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb1Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb3Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb4Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        }
                        else if (correct_answer_position == 3)
                        {
                            correct_language = firstgroup_languages.ElementAt(ind1);
                            tb3Language.Text = correct_language;
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3)
                            {
                                int luku = r.Next(1, firstCount);
                                if (!fakeAlternativesList.Contains(luku) && ind1 != luku)
                                {
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb1Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb2Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb4Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        }
                        else if (correct_answer_position == 4)
                        {
                            correct_language = firstgroup_languages.ElementAt(ind1);
                            tb4Language.Text = correct_language;
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3)
                            {
                                int luku = r.Next(1, firstCount);
                                if (!fakeAlternativesList.Contains(luku) && ind1 != luku)
                                {
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb1Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb2Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb3Language.Text = firstgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        }
                        tbLanguageSample.Text = firstgroup_samples.ElementAt(ind1);
                        languageInOrderCount++;
                    }
                    else if (languageInOrderCount <= 5)
                    {//3 4 5
                        int ind1 = second_selectedLanguagesList.ElementAt(languageInOrderCount - 3);
                        int correct_answer_position = r.Next(1, 5);
                        if (correct_answer_position == 1)
                        {
                            correct_language = secondgroup_languages.ElementAt(ind1);
                            tb1Language.Text = correct_language;
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3)
                            {
                                int luku = r.Next(1, secondCount);
                                if (!fakeAlternativesList.Contains(luku) && ind1 != luku)
                                {
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb2Language.Text = secondgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb3Language.Text = secondgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb4Language.Text = secondgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        }
                        else if (correct_answer_position == 2)
                        {
                            correct_language = secondgroup_languages.ElementAt(ind1);
                            tb2Language.Text = correct_language;
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3)
                            {
                                int luku = r.Next(1, secondCount);
                                if (!fakeAlternativesList.Contains(luku) && ind1 != luku)
                                {
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb1Language.Text = secondgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb3Language.Text = secondgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb4Language.Text = secondgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        }
                        else if (correct_answer_position == 3)
                        {
                            correct_language = secondgroup_languages.ElementAt(ind1);
                            tb3Language.Text = correct_language;
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3)
                            {
                                int luku = r.Next(1, secondCount);
                                if (!fakeAlternativesList.Contains(luku) && ind1 != luku)
                                {
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb1Language.Text = secondgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb2Language.Text = secondgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb4Language.Text = secondgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        }
                        else if (correct_answer_position == 4)
                        {
                            correct_language = secondgroup_languages.ElementAt(ind1);
                            tb4Language.Text = correct_language;
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3)
                            {
                                int luku = r.Next(1, secondCount);
                                if (!fakeAlternativesList.Contains(luku) && ind1 != luku)
                                {
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb1Language.Text = secondgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb2Language.Text = secondgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb3Language.Text = secondgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        }
                        tbLanguageSample.Text = secondgroup_samples.ElementAt(ind1);
                        languageInOrderCount++;

                    }
                    else if (languageInOrderCount <= 8)
                    { //6 7 8
                        int ind1 = third_selectedLanguagesList.ElementAt(languageInOrderCount - 6);
                        int correct_answer_position = r.Next(1, 5);
                        if (correct_answer_position == 1)
                        {
                            correct_language = thirdgroup_languages.ElementAt(ind1);
                            tb1Language.Text = correct_language;
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3)
                            {
                                int luku = r.Next(1, thirdCount);
                                if (!fakeAlternativesList.Contains(luku) && ind1 != luku)
                                {
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb2Language.Text = thirdgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb3Language.Text = thirdgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb4Language.Text = thirdgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        }
                        else if (correct_answer_position == 2)
                        {
                            correct_language = thirdgroup_languages.ElementAt(ind1);
                            tb2Language.Text = correct_language;
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3)
                            {
                                int luku = r.Next(1, thirdCount);
                                if (!fakeAlternativesList.Contains(luku) && ind1 != luku)
                                {
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb1Language.Text = thirdgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb3Language.Text = thirdgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb4Language.Text = thirdgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        }
                        else if (correct_answer_position == 3)
                        {
                            correct_language = thirdgroup_languages.ElementAt(ind1);
                            tb3Language.Text = correct_language;
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3)
                            {
                                int luku = r.Next(1, thirdCount);
                                if (!fakeAlternativesList.Contains(luku) && ind1 != luku)
                                {
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb1Language.Text = thirdgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb2Language.Text = thirdgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb4Language.Text = thirdgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        }
                        else if (correct_answer_position == 4)
                        {
                            correct_language = thirdgroup_languages.ElementAt(ind1);
                            tb4Language.Text = correct_language;
                            List<int> fakeAlternativesList = new List<int>();
                            while (fakeAlternativesList.Count < 3)
                            {
                                int luku = r.Next(1, thirdCount);
                                if (!fakeAlternativesList.Contains(luku) && ind1 != luku)
                                {
                                    fakeAlternativesList.Add(luku);
                                }
                            }
                            tb1Language.Text = thirdgroup_languages.ElementAt(fakeAlternativesList.ElementAt(0));
                            tb2Language.Text = thirdgroup_languages.ElementAt(fakeAlternativesList.ElementAt(1));
                            tb3Language.Text = thirdgroup_languages.ElementAt(fakeAlternativesList.ElementAt(2));
                        }
                        tbLanguageSample.Text = thirdgroup_samples.ElementAt(ind1);
                        languageInOrderCount++;
                    }






                }


            }
           timeCounter = 5;
           tbTimeToAnswer.Text = timeCounter.ToString();
           dt.Start();
        }



        private async void prepareScreenToShowResultOfGame() { 
            tbLanguageSampleLabel.Visibility = Visibility.Collapsed;
            tbQuestionNumber.Visibility = Visibility.Collapsed;
            tbThreeLabel.Visibility = Visibility.Collapsed;
            tbLanguageSample.Visibility = Visibility.Collapsed;
            tbForwardFlash2.Visibility = Visibility.Collapsed;
            tbRound.Visibility = Visibility.Collapsed;
            tbForwardFlash1.Visibility = Visibility.Collapsed;
            tb1Language.Visibility = Visibility.Collapsed;
            tb2Language.Visibility = Visibility.Collapsed;
            tb3Language.Visibility = Visibility.Collapsed;
            tb4Language.Visibility = Visibility.Collapsed;
            recTimeRunningBack.Visibility = Visibility.Collapsed;
            tbTimeRunningLabel.Visibility = Visibility.Collapsed;
            tbTimeToAnswer.Visibility = Visibility.Collapsed;
            tbSelectLanguage.Visibility = Visibility.Collapsed;
            tbTitle.Text="Result Page";



            if(int.Parse(tbScore.Text) >=0 && int.Parse(tbScore.Text) <= 15) {
                imFacePoliglot.Visibility = Visibility.Visible;
                SpeechSynthesizer synth = new SpeechSynthesizer();
                await synth.SpeakTextAsync("You don't know too many languages");
            }
            if (int.Parse(tbScore.Text) > 15 && int.Parse(tbScore.Text) <= 30)
            {
                imFacePoliglot.Visibility = Visibility.Visible;
                SpeechSynthesizer synth = new SpeechSynthesizer();
                await synth.SpeakTextAsync("You are at a basic level");
            }
            if (int.Parse(tbScore.Text) > 30 && int.Parse(tbScore.Text) <= 45)
            {
                imFacePoliglot.Visibility = Visibility.Visible;
                SpeechSynthesizer synth = new SpeechSynthesizer();
                await synth.SpeakTextAsync("You are at an intermediate level");
            }
            if (int.Parse(tbScore.Text) > 45 && int.Parse(tbScore.Text) <= 60)
            {
                imFacePoliglot.Visibility = Visibility.Visible;
                SpeechSynthesizer synth = new SpeechSynthesizer();
                await synth.SpeakTextAsync("You are at an advanced level");
            }
            if (int.Parse(tbScore.Text) > 60 && int.Parse(tbScore.Text) <= 70)
            {
                imFacePoliglot.Visibility = Visibility.Visible;
                SpeechSynthesizer synth = new SpeechSynthesizer();
                await synth.SpeakTextAsync("You are genious");
            }
            if (int.Parse(tbScore.Text) > 70 )
            {
                imFacePoliglot.Visibility = Visibility.Visible;
                SpeechSynthesizer synth = new SpeechSynthesizer();
                await synth.SpeakTextAsync("You are a real poliglot!");
            }            

        }
 
        // Sample code for building a localized ApplicationBar
        private void BuildLocalizedApplicationBar() { }

    }
}