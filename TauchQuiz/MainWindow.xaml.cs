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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TauchQuiz
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region VARS
        private Random r = new Random();
        private const int ShowStatusAfterLines = 15; 
        private int Counter_Question = 0;                                          
        private int Counter_Status = ShowStatusAfterLines;
        private int Counter_Correct = 0;
        private string UserAnswer;
        private bool Radios_Enabled = true;
        private bool Nightmode_Enabled = false;

        private List<int> RandomTable = new List<int>();
        private List<T_Fragen> data;
        #endregion VARS
        public MainWindow()
        {
            InitializeComponent();
            DiveDataEntities enteties = new DiveDataEntities();
            data = enteties.T_Fragen.ToList();
            RefreshWithOrderedQuestion(Counter_Question);
        }


        #region EVENTS  
        // EVENTS:  Checkboxes  
        private void ZufallsModus_Checked(object sender, RoutedEventArgs e)
        {
            this.CreateList();
            BTN_Next_Click_ToNextQuestion(sender, e);
        }
        private void Nachtmodus_Checked(object sender, RoutedEventArgs e)
        {
            CheckAndChangeColorMode();
        }

        // EVENTS: Buttons     
        private void BTN_Next_Click_ToNextQuestion(object sender, RoutedEventArgs e)
        {
            // Check if random-mode is checked
            if (CB_ZufallsModus.IsChecked == false)
            {
                Counter_Question++;
                RefreshWithOrderedQuestion(Counter_Question);
            }
            else
            {
                Counter_Question = this.GetRandomInt();
                RefreshWithRandomQuestion();
            }
        }
        private void Btn_Cheat_Click(object sender, RoutedEventArgs e)
        {
            Cheat();
        }

        // EVENTS: FAKE_RadioButtons  
        private void BtnA_Click(object sender, RoutedEventArgs e)
        {
            //this.UserAnswer = "a";
            if (this.Radios_Enabled == true)
                this.RB_a.IsChecked = true;
            this.DisableRadioBTNs();
            //    this.RB_a_Checked(sender, e);
        }
        private void BtnB_Click(object sender, RoutedEventArgs e)
        {
            //this.UserAnswer = "b";
            if (this.Radios_Enabled == true)
                this.RB_b.IsChecked = true;
            this.DisableRadioBTNs();
            //     this.RB_b_Checked(sender, e);
        }
        private void BtnC_Click(object sender, RoutedEventArgs e)
        {
            //this.UserAnswer = "c";
            if (this.Radios_Enabled == true)
                this.RB_c.IsChecked = true;
            this.DisableRadioBTNs();
            //     this.RB_c_Checked(sender, e);
        }
        private void BtnD_Click(object sender, RoutedEventArgs e)
        {
            //this.UserAnswer = "d";
            if (this.Radios_Enabled == true)
                this.RB_d.IsChecked = true;
            this.DisableRadioBTNs();
            //    this.RB_d_Checked(sender, e);
        }

        // EVENTS: RadioButtons    
        private void RB_a_Checked(object sender, RoutedEventArgs e)
        {
            this.UserAnswer = "a";
            ShowCorrectAnswer();
        }
        private void RB_b_Checked(object sender, RoutedEventArgs e)
        {
            this.UserAnswer = "b";
            ShowCorrectAnswer();
        }         
        private void RB_c_Checked(object sender, RoutedEventArgs e)
        {
            this.UserAnswer = "c";
            ShowCorrectAnswer();
        }
        private void RB_d_Checked(object sender, RoutedEventArgs e)
        {
            this.UserAnswer = "d";
            ShowCorrectAnswer();
        }

        private void CB_Nightmode_Click(object sender, RoutedEventArgs e)
        {
            CheckAndChangeColorMode();
        }
        #endregion EVENTS

        #region METHS
        private void RefreshWithOrderedQuestion(int QuestionCounter)    // New Question from (ordered)-array
        {
            if (this.Counter_Question == data.Count()-1)
            {
                this.Counter_Question = 0;
                QuestionCounter = 0;
            }
            ClearWindow();
            this.Lb_FragenNr.Content = QuestionCounter + 1 + "  von  " + (this.data.Count() );
            this.TB_Fragentext.Text = this.removeStringTags(data[QuestionCounter].Frage);
            this.Antwort_b.Text = this.removeStringTags(data[QuestionCounter].AntwortB); 
            this.Antwort_a.Text = this.removeStringTags(data[QuestionCounter].AntwortA); 
            this.Antwort_c.Text = this.removeStringTags(data[QuestionCounter].AntwortC); 
            this.Antwort_d.Text = this.removeStringTags(data[QuestionCounter].AntwortD);
            this.showStats();
        }                          
        private void RefreshWithRandomQuestion()                        // New Question from (random)-list
        {
            if (this.Counter_Question == data.Count() - 1)
                this.Counter_Question = 0;

            this.ClearWindow();
            this.Lb_FragenNr.Content = this.data[Counter_Question].P_Fragen_ID; 
            this.TB_Fragentext.Text = this.removeStringTags(data[Counter_Question].Frage);
            this.Antwort_a.Text = this.removeStringTags(data[Counter_Question].AntwortA);
            this.Antwort_b.Text = this.removeStringTags(data[Counter_Question].AntwortB);
            this.Antwort_c.Text = this.removeStringTags(data[Counter_Question].AntwortC);
            this.Antwort_d.Text = this.removeStringTags(data[Counter_Question].AntwortD);
            this.showStats();
        }
        private bool CheckAnswer()                                      // Check User-Answer, if correct increment @Counter-Correct
        {
            if (this.UserAnswer == data[Counter_Question].Loesung)
            {
                Counter_Correct++;
                return true;
            }
            else
                return false;
        }
        private void DisableRadioBTNs()                                 // DISables ALL Radiobuttuns
        {
            this.Radios_Enabled = false;
            this.RB_a.IsEnabled = false;
            this.RB_b.IsEnabled = false;
            this.RB_c.IsEnabled = false;
            this.RB_d.IsEnabled = false;
        }
        private void EnableRadioBTNs()                                  // ENables ALL Radiobuttuns
        {
            this.Radios_Enabled = true;
            this.RB_a.IsEnabled = true;
            this.RB_b.IsEnabled = true;             
            this.RB_c.IsEnabled = true;
            this.RB_d.IsEnabled = true;
        }
        private void ShowCorrectAnswer()                                // ALL Answers = red, THEN correct A. = green
        {
            this.DisableRadioBTNs();
            this.Antwort_a.Foreground = Brushes.Red;
            this.Antwort_b.Foreground = Brushes.Red;
            this.Antwort_c.Foreground = Brushes.Red;
            this.Antwort_d.Foreground = Brushes.Red;
            switch (data[Counter_Question].Loesung)
            {
                case "a":
                    this.Antwort_a.Foreground = Brushes.Green;
                    break;
                case "b":
                    this.Antwort_b.Foreground = Brushes.Green;
                    break;
                case "c":
                    this.Antwort_c.Foreground = Brushes.Green;
                    break;
                case "d":
                    this.Antwort_d.Foreground = Brushes.Green;
                    break;
                default:
                    break;
            }
            this.CheckAnswer();
            this.Counter_Status--;
            this.L_Richtige.Foreground = Brushes.Green;
            this.L_Richtige.Content = this.Counter_Correct;
        }
        private void ClearWindow()                                      // clear RadioButtons + All Anwers = Black (again)
        {
            // Remove RadioButton
            this.RB_a.IsChecked = false;
            this.RB_b.IsChecked = false;
            this.RB_c.IsChecked = false;
            this.RB_d.IsChecked = false;
            EnableRadioBTNs();
            
            // Remove all colors from Answers
            // with disabled nightmode
            if (Nightmode_Enabled == true)
            {
                Color C_NiceFont = Color.FromRgb(55, 155, 155);
                SolidColorBrush SCB_Font = new SolidColorBrush(C_NiceFont);
                this.Antwort_a.Foreground = SCB_Font;
                this.Antwort_b.Foreground = SCB_Font;
                this.Antwort_c.Foreground = SCB_Font;
                this.Antwort_d.Foreground = SCB_Font;
            }
            else
            {
                this.Antwort_a.Foreground = Brushes.Black;
                this.Antwort_b.Foreground = Brushes.Black;
                this.Antwort_c.Foreground = Brushes.Black;
                this.Antwort_d.Foreground = Brushes.Black;
            }
        }
        private string removeStringTags(string str)                     // Removes all /n and /r Tags from Question-DB
        {
            str = str.Replace("\\n", "");
            str = str.Replace("\\r", "");
            return str;
        }
        private void showStats()                                        // Reports overview to user: X of Y questions done, Z% of them correct
        {
            if (this.Counter_Status == 0)
            {
                MessageBox.Show("Sie haben bisher " + this.Counter_Correct + " von "
                    //+ (this.Counter_Question) + " Fragen [" + Calc_percent() + "%] RICHTIG beantwortet" 
                    + (this.data.Count()) + " Fragen [" + Calc_percent() + "%] RICHTIG beantwortet"
                    + "\nNaja,immerhin sind Sie gesund...", "Statusbericht");
                this.Counter_Status = ShowStatusAfterLines;
            }
        }
        private double Calc_percent()                                   // calculates percent of correct answers 
        {
            //double q = Counter_Question;
            double c = Counter_Correct;
            double q = this.data.Count();
            double res = 100 /  (q / c);
            res = Math.Round(res, 2);
            return res;
        }
        private int GetRandomInt()                                      // Get random-Index for Randommode
        {
            if (RandomTable.Count == 0)              // if list is empty,
                this.CreateList();                   // create a new one

            int maxRnd = data.Count();
            int temp_index = r.Next(maxRnd);
            if (RandomTable.Contains(temp_index))   
            {
                RandomTable.Remove(temp_index);
                return temp_index;
            }
            else
            {
                this.GetRandomInt();
                return 0;
            }           
        }
        private void CreateList()                                       // creates list with all Indexes of Question-DB 
        {
            RandomTable.Clear();
            foreach (var item in data)
            {
                RandomTable.Add(item.P_Fragen_ID);
            }
        }
        private void CheckAndChangeColorMode()                          // changes the colormode of window if Checkbox = checked 
        {
            Color C_NiceDarkBlue = Color.FromRgb(19, 9, 51);
            SolidColorBrush SCB_Background = new SolidColorBrush(C_NiceDarkBlue);
            Color C_NiceFont = Color.FromRgb(55, 155, 155);
            SolidColorBrush SCB_Font = new SolidColorBrush(C_NiceFont);

            if (this.CB_Nachtmodus.IsChecked == true)   
            {   // ENable Nightmode

                this.TB_Frage.Foreground            = SCB_Font;
                this.Lb_FragenNr.Foreground         = SCB_Font;
                this.CB_ZufallsModus.Foreground     = SCB_Font;
                this.CB_Nachtmodus.Foreground       = SCB_Font;
                this.TB_Fragentext.Foreground       = SCB_Font;
                this.RB_a.Foreground                = SCB_Font;
                this.RB_b.Foreground                = SCB_Font;
                this.RB_c.Foreground                = SCB_Font;
                this.RB_d.Foreground                = SCB_Font;
                this.Antwort_a.Foreground           = SCB_Font;
                this.Antwort_b.Foreground           = SCB_Font;
                this.Antwort_c.Foreground           = SCB_Font;
                this.Antwort_d.Foreground           = SCB_Font;
                this.BTN_Next.Background            = Brushes.DarkBlue;
                this.BTN_Next.Foreground            = SCB_Font;
                this.Btn_Cheat.Background           = Brushes.DarkBlue;
                this.Btn_Cheat.Foreground           = SCB_Font;
                this.Grid.Background                = SCB_Background;
                
                this.Nightmode_Enabled = true;
            }
            else
            {   //DISable Nightmode

                this.TB_Frage.Foreground            = Brushes.Black;
                this.Lb_FragenNr.Foreground         = Brushes.Black;
                this.CB_ZufallsModus.Foreground     = Brushes.Black;
                this.CB_Nachtmodus.Foreground       = Brushes.Black;
                this.TB_Fragentext.Foreground       = Brushes.Black;                                                   
                this.RB_a.Foreground                = Brushes.Black;
                this.RB_b.Foreground                = Brushes.Black;
                this.RB_c.Foreground                = Brushes.Black;
                this.RB_d.Foreground                = Brushes.Black;                                                 
                this.Antwort_a.Foreground           = Brushes.Black;
                this.Antwort_b.Foreground           = Brushes.Black;
                this.Antwort_c.Foreground           = Brushes.Black;
                this.Antwort_d.Foreground           = Brushes.Black;
                this.BTN_Next.Background            = Brushes.DarkGray;
                this.BTN_Next.Foreground            = Brushes.Black;
                this.Btn_Cheat.Background           = Brushes.DarkGray;
                this.Btn_Cheat.Foreground           = Brushes.Black;
                this.Grid.Background                = Brushes.White;

                this.Nightmode_Enabled = false;
            }
        }
        private void Cheat()                                            // debug-helper ;-)
        {
            int i = 0;
            string cheat = "";
            foreach (var item in this.data)
            {
                ++i;
                cheat += " " + i + item.Loesung + ",";
                if ((i == 22) | (i == 41) | (i == 66))
                {
                    cheat += "\n";
                }
            }
            this.L_Loesung.Content = cheat;
            this.L_Loesung.Foreground = Brushes.DarkOrange;
        }
        #endregion METHS
    }

}
