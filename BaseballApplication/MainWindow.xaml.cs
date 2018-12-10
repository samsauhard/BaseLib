using BaseLib;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace BaseballApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        BaseballEntities dbContent;
        int playerID;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            String playerLname;

            playerLname = txt_lname.Text;

            try
            {
                if (txt_lname.Text == "")
                {
                    MessageBox.Show("No data found");
                    dbContent = new BaseballEntities();
                    dbContent.Players.Load();
                    dtGrid.ItemsSource = dbContent.Players.ToList();
                }
                else
                {
                    dbContent = new BaseballEntities();
                    dbContent.Players.Load();
                    var query = from item in dbContent.Players
                                where item.LastName == txt_lname.Text
                                select item;

                    dtGrid.ItemsSource = query.ToList();

                    /*   foreach (var s in query)
                       {
                           txt_modifyAverage.Text = s.BattingAverage.ToString();
                           playerID = s.PlayerID;
                       }
                       // MessageBox.Show(playerID);
                       */
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR!!!!" + "\n" + ex.ToString());
            }
        }

        private void btn_modify_Click(object sender, RoutedEventArgs e)
        {

            decimal battingAverage = decimal.Parse(txt_modifyAverage.Text);

            dbContent = new BaseballEntities();

            dbContent.Players.Load();

            (from item in dbContent.Players
             where item.LastName == txt_lname.Text
             select item).ToList().ForEach(x => x.BattingAverage = battingAverage); ;



            dbContent.SaveChanges();

            //dtGrid.ItemsSource = query.ToList();

            var query = from item in dbContent.Players
                        where item.LastName == txt_lname.Text
                        select item;

            dtGrid.ItemsSource = query.ToList();

        }

        private void btn_addShow_Click(object sender, RoutedEventArgs e)
        {
            txt_playerID.IsEnabled = true;
            txt_fname.IsEnabled = true;
            txt_lname1.IsEnabled = true;
            txt_battingAverage.IsEnabled = true;

        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            int pID = int.Parse(txt_playerID.Text);
            String fname = txt_fname.Text;
            String lname = txt_lname1.Text;
            decimal bav = decimal.Parse(txt_battingAverage.Text);

            Player p = new Player();
            p.PlayerID = pID;
            p.FirstName = fname;
            p.LastName = lname;
            p.BattingAverage = bav;
            dbContent = new BaseballEntities();
            dbContent.Players.Load();
            dbContent.Players.Add(p);
            dbContent.SaveChanges();
            dtGrid.ItemsSource = dbContent.Players.ToList();
        }
    }
}

