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
using System.Data.OracleClient;
using System.Data;

namespace GUI_DB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public OracleConnection conn = Singleton.getConnection();
       // OracleCommand cmd = new OracleCommand();
        OracleConnection oracleConnection1 = SingletonConn.getConnection();
        OracleDataAdapter dataAdapter1;
        DataTable dt;
        public MainWindow()
        {
            InitializeComponent();
            oracleConnection1.Open();
            dataAdapter1 = new OracleDataAdapter();
            dataAdapter1.SelectCommand = new OracleCommand();
            dt = new DataTable();

        }


       
        private void Client(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Client c = new Client();
            c.ShowDialog();
            c.Close();
            this.Show();

        }

        private void Appointment(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Appointment a = new Appointment();
            a.ShowDialog();
            a.Close();
            this.Show();
        }

        private void AllWorkers(object sender, RoutedEventArgs e)
        {
            //in this window add the option to see the tables-Doctors+Nurse
            this.Hide();
            MedicalWorkers m = new MedicalWorkers();
            m.ShowDialog();
            m.Close();
            this.Show();

        }

        private void WorkIn(object sender, RoutedEventArgs e)
        {
            this.Hide();
            WorkIn w = new WorkIn();
            w.ShowDialog();
            w.Close();
            this.Show();
        }
    }
}
