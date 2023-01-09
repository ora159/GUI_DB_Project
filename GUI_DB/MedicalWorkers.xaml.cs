using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GUI_DB
{
    /// <summary>
    /// Interaction logic for MedicalWorkers.xaml
    /// </summary>
    public partial class MedicalWorkers : Window
    {
        string query;
        DataTable dt;
        OracleDataAdapter dataAdapter1;
        OracleConnection oracleConnection1 = SingletonConn.getConnection();
        OracleCommand InsertCommand;
        public MedicalWorkers()
        {
            try
            {
                InitializeComponent();
                if (oracleConnection1.State == System.Data.ConnectionState.Closed)
                    oracleConnection1.Open();
                dataAdapter1 = new OracleDataAdapter();
                dataAdapter1.SelectCommand = new OracleCommand();
                //UpdateCommand=new OracleCommand();
                InsertCommand = new OracleCommand();
                dt = new DataTable();

                InitFields();
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR: " + e.Message);
            }
        }
        private void InitFields()
        {
            getDep1_2();
            this.allMWRB.IsChecked = false;
            this.allNurseRB.IsChecked = false;
            this.DoctorRB.IsChecked = false;
            this.AvgDRB.IsChecked = false;
            this.AVGNRB.IsChecked = false;
            this.SUMDRB.IsChecked = false;
            this.SUMNRB.IsChecked = false;
        }
        public DataTable myGridConn(string str)
        {

            dt.Clear();
            dt = new DataTable();
            //prepare query 
            string SQLquery = str;
            //prepare data adapter for sql query 
            dataAdapter1.SelectCommand.Connection = oracleConnection1;
            dataAdapter1.SelectCommand.CommandText = SQLquery;
            dataAdapter1.Fill(dt);

            return dt;

        }
        private List<string> convertTableToList(DataTable dt)
        {
            List<string> list = new List<string>();
            foreach (DataRow item in dt.Rows)
                list.Add(item.ItemArray[0].ToString());
            return list;
        }
        private void getAllMW()
        {
            try
            {
                query = "select * from medicalworker";
                dataGridMW.ItemsSource = myGridConn(query).DefaultView;
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR: " + e.Message);
            }
        }
        private void getAllNurses()
        {
            try
            {
                query = "select * from medicalworker where idmw in (select idmw from nurse)";
                dataGridMW.ItemsSource = myGridConn(query).DefaultView;
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR: " + e.Message);
            }
        }
        private void getAllDoctors()
        {
            try
            {
                query = "select * from medicalworker where idmw in (select idmw from doctor)";
                dataGridMW.ItemsSource = myGridConn(query).DefaultView;
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR: " + e.Message);
            }
        }
        private void getAVGD()
        {
            try
            {
                query = "select Round( AVG (salary)) " +
                         "from medicalworker m,doctor d " +
                            "where m.idmw = d.idmw";
                dataGridMW2.ItemsSource = myGridConn(query).DefaultView;
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR: " + e.Message);
            }
        }
        private void getAVGN()
        {
            try
            {
                query = "select Round( AVG (salary)) " +
                         "from medicalworker m,nurse n " +
                            "where m.idmw = n.idmw";
                dataGridMW2.ItemsSource = myGridConn(query).DefaultView;
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR: " + e.Message);
            }
        }
        private void getSUMD()
        {
            try
            {
                query = "select SUM (salary) " +
                         "from medicalworker m, doctor d " +
                            "where m.idmw = d.idmw";
                dataGridMW3.ItemsSource = myGridConn(query).DefaultView;
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR: " + e.Message);
            }
        }
        private void getSUMN()
        {
            try
            {
                query = "select SUM (salary) " +
                         "from medicalworker m,nurse n " +
                            "where m.idmw = n.idmw";
                dataGridMW3.ItemsSource = myGridConn(query).DefaultView;
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR: " + e.Message);
            }
        }
        private void getDep1_2()
        {
            try
            {
                query = "select namedepartment from department";
                Dep1List.ItemsSource = convertTableToList(myGridConn(query));
                Dep2List.ItemsSource = convertTableToList(myGridConn(query));
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR: " + e.Message);
            }
        }
        private void getAndNurses(string D1, string D2)
        {
            try
            {
                query = "select m.fname,m.lname,n.idmw "+
                         "from medicalworker m,nurse n, department d,department d2, work_in w, work_in w2 "+
                         "where w.idmw = m.idmw and "+
                         "m.idmw = n.idmw and "+
                         "w.codedepartment = d.codedepartment and "+
                          "w.namedepartment like '"+D1+"' and "+
                          "w2.idmw = m.idmw and "+
                          "w2.idmw = n.idmw and "+
                          "w2.codedepartment = d2.codedepartment and "+
                           "w2.namedepartment like '"+D2+"'";


                dataGridN.ItemsSource = myGridConn(query).DefaultView;
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR: " + e.Message);
            }
        }
        private void getOrNurses(string D1,string D2)
        {
            try
            {
                query="select m.fname,m.lname,d.namedepartment "+
                "from medicalworker m,nurse n,department d, work_in w  "+
                "where w.idmw = m.idmw and "+
                "m.idmw = n.idmw and "+
                "w.codedepartment = d.codedepartment and "+
                "w.namedepartment like '"+D1+"' "+
                "union "+
                "select m.fname,m.lname,d.namedepartment "+
                "from medicalworker m,nurse n,department d, work_in w "+
                "where w.idmw = m.idmw and "+
                "m.idmw = n.idmw and "+
                "w.codedepartment = d.codedepartment and "+
                "w.namedepartment like '"+D2+"'";
                
                dataGridN.ItemsSource = myGridConn(query).DefaultView;
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR: " + e.Message);
            }
        }
        private void ShowAllMW(object sender, RoutedEventArgs e)
        {
            this.allNurseRB.IsChecked = false;
            this.DoctorRB.IsChecked = false;
            getAllMW();
        }
        private void ShowNurses(object sender, RoutedEventArgs e)
        {
            this.allMWRB.IsChecked = false;
            this.DoctorRB.IsChecked = false;
            getAllNurses();
        }

        private void ShowDoctors(object sender, RoutedEventArgs e)
        {
            this.allNurseRB.IsChecked = false;
            this.allMWRB.IsChecked = false;
            getAllDoctors();
            dataGridMW.Items.Refresh();
        }

        private void ShowAVGD(object sender, RoutedEventArgs e)
        {
            this.AVGNRB.IsChecked = false;
            this.SUMDRB.IsChecked = false;
            this.SUMNRB.IsChecked = false;
            getAVGD();
            dataGridMW2.Items.Refresh();
        }

        private void ShowAVGN(object sender, RoutedEventArgs e)
        {
            this.AvgDRB.IsChecked = false;
            this.SUMNRB.IsChecked = false;
            this.SUMDRB.IsChecked = false;
            getAVGN();
            dataGridMW2.Items.Refresh();
        }

        private void ShowSUMDD(object sender, RoutedEventArgs e)
        {
            this.SUMNRB.IsChecked = false;
            this.AVGNRB.IsChecked = false;
            this.AvgDRB.IsChecked = false;
            getSUMD();
            dataGridMW3.Items.Refresh();
        }

        private void ShowSUMDN(object sender, RoutedEventArgs e)
        {
            this.SUMDRB.IsChecked = false;
            this.AVGNRB.IsChecked = false;
            this.AvgDRB.IsChecked = false;
            getSUMN();
            dataGridMW3.Items.Refresh();
        }

        private void AndButton_Click(object sender, RoutedEventArgs e)
        {
            string nameD1 = this.Dep1List.SelectedValue.ToString();
            string nameD2 = this.Dep2List.SelectedValue.ToString();
            getAndNurses(nameD1, nameD2);
        }

        private void OrButton_Click(object sender, RoutedEventArgs e)
        {
            string nameD1 = this.Dep1List.SelectedValue.ToString();
            string nameD2= this.Dep2List.SelectedValue.ToString();
            getOrNurses(nameD1,nameD2);
        }

        private void Dep1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Dep2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void UpdateSallary_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
          Function c = new Function();
            c.ShowDialog();
            c.Close();
            this.Show();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
