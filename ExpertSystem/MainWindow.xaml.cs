using ExpertSystem.Question;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
using UniversalHelpers.ConsoleManager;

namespace ExpertSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            ConsoleManager.Show();
            InitializeComponent();

            SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder()
            {
                DataSource = @"..\..\data\awd-data.sqlite"
            };

            using (SQLiteConnection conn = new SQLiteConnection(builder.ConnectionString).OpenAndReturn())
            {
                SQLiteCommand command = new SQLiteCommand("SELECT * FROM DATA WHERE ID<15", conn);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine(reader.GetValue(0));
                }

                conn.Close();
            }
        }
    }
}
