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
    /// Interaction logic for Appointment.xaml
    /// </summary>
    public partial class Appointment : Window
    {
        string query;
        DataTable dt;
        OracleDataAdapter dataAdapter1;
        OracleConnection oracleConnection1 = SingletonConn.getConnection();
        OracleCommand InsertCommand;
        public Appointment()
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
            getAllCodes();
            getAllIdClients();
            this.appoCodes.SelectedItem = null;
            this.appoCode.Text = "";
            this.appoPrice.Text = "";
            this.appoHouer.SelectedItem = null;
            this.appoHouer2.SelectedItem = null;
            this.appoIdClients.SelectedItem = null;
            this.allAppoRB.IsChecked = false;
            this.IDnurseRB.IsChecked = false;
            this.IDdoctorRB.IsChecked = false;
            this.idMWLable.Visibility = Visibility.Hidden;
            this.appDateRB.IsChecked = false;
            this.DateAppLable.Visibility = Visibility.Hidden;
            this.selectDateApp.Visibility = Visibility.Hidden;
            this.appoIdMW.Visibility = Visibility.Hidden;
            this.OK_AppDate.Visibility = Visibility.Hidden;
            this.appoIdMW.SelectedItem = null;
            //this.appoDate2.SelectedDate = DateTime.Today;
            this.appoHouer.ItemsSource = initHouer();
            this.appoHouer2.ItemsSource = initHouer();
            this.appoCode2.Text = "";
            this.appoIdClients2.Text = "";
            this.appoIdMW2.Text = "";
            this.appoPrice2.Text = "";
            
        }
        //check if the data is valid
        private bool validate()
        {
            Boolean b = false;
            if (this.appoCode.Text == "")
                b = true;
            if(this.appoIdClients.SelectedItem.ToString()==null)
                b = true;
            if (this.appoIdMW.SelectedItem.ToString()==null)
                b = true;
            if(this.appoHouer.SelectedItem.ToString()==null)
                b = true;
            if(this.appoDate.SelectedDate.ToString()==null)
                b = true;
            if(this.appoPrice.Text=="")
                b = true;
            if(b)
                MessageBox.Show("missing details");
            if(this.appoCode.Text.ToString().Length>10 || this.appoPrice.Text.ToString().Length>10)
                throw new Exception("Check Your's Fields Again");
        
            return b;
        }
        private bool validate2()
        {
            Boolean b = false;
            if (this.appoCode2.Text == "")
                b = true;
            if (this.appoIdClients2.Text == "")
                b = true;
            if (this.appoIdMW2.Text == "")
                b = true;
            if (this.appoHouer2.SelectedItem.ToString() == null)
                b = true;
            if (this.appoDate2.SelectedDate.ToString() == null)
                b = true;
            if (this.appoPrice2.Text == "")
                b = true;
            if (b)
                MessageBox.Show("missing details");
            if (this.appoCode2.Text.ToString().Length > 10 || this.appoPrice2.Text.ToString().Length > 4)
                throw new Exception("Check Your's Fields Again");
            return b;
        }
        public List<String> initHouer()
        {
            try
            {
                List<String> l = new List<string>();

                for (int i = 7; i < 18; ++i)
                    l.Add(String.Format(i.ToString()));
                 return l;
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /*private void initHouer(){
        for (int i =7; i < 18; ++i){
              ComboBoxItem newItem = new ComboBoxItem();
              newItem.Content = i ;appoHouer.Items.Add(newItem);  
            }}*/

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
        //get functions
        private void getAllAppo()
        {
            try
            {
                query = "select * from appointment";
                dataGridAppo.ItemsSource = myGridConn(query).DefaultView;
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR: " + e.Message);
            }
        }
        private void getAllCodes()
        {

            try
            {
                query = "select codeapp from appointment";
                appoCodes.ItemsSource = convertTableToList(myGridConn(query));
                
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR: " + e.Message);
            }
        }
        private void getAllIdClients()
        {
            try
            {
                query = "select idClient  from Client";
                appoIdClients.ItemsSource = convertTableToList(myGridConn(query));
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR: " + e.Message);
            }
           
        }
        private void getAllIdNurse()
        {
            try
            {
                query = "select idmw from medicalworker where idmw in (select idmw from nurse)";
                appoIdMW.ItemsSource = convertTableToList(myGridConn(query));
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR: " + e.Message);
            }
        }
        private void getAllIdDoctors()
        {
            try
            {
                query = "select idmw from medicalworker where idmw in (select idmw from doctor)";
                appoIdMW.ItemsSource = convertTableToList(myGridConn(query));
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR: " + e.Message);
            }
        }
        private DataRow getMyAppoCode(String code)
        {
            try
            {
                query = "select* from appointment";
                foreach (DataRow item in this.myGridConn(query).Rows)
                    if (item["CODEAPP"].ToString() == code)
                        return item;

                throw new Exception("The code Not Found");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void getClientByDate()
        {
            try
            {
                query = "select  c.fname,c.lname,a.codeapp,a.hourapp,a.price " +
                        "from appointment a,client c" +
                        " where a.idclient = c.idclient and " +
                        "dateapp = To_DATE('" + this.selectDateApp.Text.ToString() + "', 'dd/mm/yyyy')";

                dataGridAppo.ItemsSource = myGridConn(query).DefaultView;
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR: " + e.Message);
            }
        }
        private void ShowAllAppo(object sender, RoutedEventArgs e)
        {
            this.appDateRB.IsChecked = false;
            this.DateAppLable.Visibility = Visibility.Hidden;
            this.selectDateApp.Visibility = Visibility.Hidden;
            this.OK_AppDate.Visibility = Visibility.Hidden;
            getAllAppo();
        }

        private void appoIdClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (appoIdClients.SelectedItem == null)
                    return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }
        private void appoHouer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (appoHouer.SelectedItem == null)
                    return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }
        private void appoIdMW_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (appoIdMW.SelectedItem == null)
                    return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }
        private void IDnurseRB_Checked(object sender, RoutedEventArgs e)
        {
            this.IDdoctorRB.IsChecked = false;
            this.idMWLable.Visibility = Visibility.Visible;
            this.appoIdMW.Visibility = Visibility.Visible;
            getAllIdNurse();
        }
        private void IDdoctorRB_Checked(object sender, RoutedEventArgs e)
        {
            this.IDnurseRB.IsChecked = false;
            this.idMWLable.Visibility = Visibility.Visible;
            this.appoIdMW.Visibility = Visibility.Visible;
            getAllIdDoctors();
        }
        //ADD 
        private void AddNewMW(object sender, RoutedEventArgs e)
        {
            try
            {
                if (validate() == false)
                {
                   
                    int code = int.Parse((appoCode.Text.ToString()));
                    int idClient = int.Parse((appoIdClients.SelectedValue.ToString()));
                    int idmw = int.Parse((appoIdMW.SelectedValue.ToString()));
                    //string sub3=sub2.Remove(2,3);
                    int huors = int.Parse((appoHouer.SelectedValue.ToString()));
                    int price = int.Parse(appoPrice.Text.ToString());

                    InsertCommand.Connection = oracleConnection1;
                    InsertCommand.CommandText = "insert into Appointment(Codeapp,Dateapp,Price,Idclient,Idmw,Hourapp)" + //+ values(38, To_DATE('08/01/2017', 'dd/mm/yyyy'), 310, 421257005, 7066909, 19);
                                                "values(" + code + "," + " to_date('" + this.appoDate.Text.ToString() + "','dd/mm/yyyy')," +
                                                price + "," + idClient + "," + idmw + "," + huors + ")";

                    InsertCommand.ExecuteNonQuery();
                    MessageBox.Show("Add APPOINTMENT SUCCESFULY");
                    this.appoDate.SelectedDate = DateTime.Today;
                    InitFields();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR ADD: " + ex.Message);
            }
        }
        //UPDATE OR DELETE
        private void appoCodes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (appoCodes.SelectedItem == null)
                    return;
                String MyAppoCode = appoCodes.SelectedValue.ToString();
                System.Data.DataRow appo = getMyAppoCode(MyAppoCode);
                this.appoCode2.Text = appo["CODEAPP"].ToString();
                this.appoIdClients2.Text = appo["IDCLIENT"].ToString();
                this.appoIdMW2.Text = appo["IDMW"].ToString();
                this.appoHouer2.Text = appo["HOURAPP"].ToString();
                this.appoPrice2.Text = appo["PRICE"].ToString();
                this.appoDate2.Text = appo["DATEAPP"].ToString();
                
             
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void DeleteAppo(object sender, RoutedEventArgs e)
        {
            
            this.appoHouer2.IsEnabled = false;
            this.appoDate2.IsEnabled = false;
            this.appoPrice2.IsReadOnly = true;
            try
            {
                if (appoCodes.SelectedValue == null)
                    return;
                
                MessageBox.Show("Are you want to Delete?");
                
                string code = (appoCodes.SelectedValue.ToString());
                DeleteAppoCode(code);
                this.appoDate2.SelectedDate = DateTime.Today;
                this.appoHouer2.IsEnabled = true;
                this.appoDate2.IsEnabled = true;
                this.appoPrice2.IsReadOnly = false;
                InitFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void DeleteAppoCode(string code)
        {
            OracleCommand cmd = new OracleCommand();

            try
            {
                int mycode = int.Parse(code);
                cmd = new OracleCommand();
                if (validate2() == false)//check the data
                {
                    cmd.Connection = oracleConnection1;
                    cmd.CommandText = "delete from appointment where codeapp=" + mycode;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Delete succesfuly");

                }
            }
            catch (Exception ex)
            {

                throw new Exception("ERROR UPDATE" + ex.Message);
            }
        }
        private void UpdateAppo(object sender, RoutedEventArgs e)
        {
            try
            {
                if (appoCodes.SelectedValue == null)
                    return;
                string code = (appoCodes.SelectedValue.ToString());
                UpdateAppoCode(code);
                this.appoDate2.SelectedDate = DateTime.Today;
                InitFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void UpdateAppoCode(string code)
        {

            OracleCommand cmd = new OracleCommand();

            try
            {
                int mycode = int.Parse(code);
                cmd = new OracleCommand();
                if (validate2() == false)//check the data
                {
                    cmd.Connection = oracleConnection1;
                    //To_DATE('01/06/1980','dd/mm/yyyy')
                    cmd.CommandText = "update appointment set hourapp ='" + this.appoHouer2.Text +
                                                                     "', dateapp=To_DATE('" + this.appoDate2.Text + "','dd/mm/yyyy')," +
                                                                     "price=" + this.appoPrice2.Text+ 
                                                                     "where codeapp=" + mycode;
                    
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Update succesfuly");

                }
            }
            catch (Exception ex)
            {

                throw new Exception("ERROR UPDATE" + ex.Message);
            }
        }

        private void ShowDateApp(object sender, RoutedEventArgs e)
        {
            this.DateAppLable.Visibility = Visibility.Visible;
            this.selectDateApp.Visibility = Visibility.Visible;
            this.OK_AppDate.Visibility = Visibility.Visible;
            this.allAppoRB.IsChecked = false;
        }

        private void OK_AppDate_Click(object sender, RoutedEventArgs e)
        {
            getClientByDate();
        }
    }
    }

