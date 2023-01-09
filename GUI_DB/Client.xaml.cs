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
    /// Interaction logic for Client.xaml
    /// </summary>
    public partial class Client : Window
    {
        string query;
        DataTable dt;
        OracleDataAdapter dataAdapter1;
        OracleConnection oracleConnection1 = SingletonConn.getConnection();
        OracleCommand InsertCommand;
  

        public Client()
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
                //Initialize Tools
                this.dataGridClient.Items.Refresh();
                //this.clientIdList.Visibility = Visibility.Hidden;
                this.clientIdList.SelectedItem = null;
                getAllClientsName();

                //InitFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        void GridBind(string str)
        {
            dt.Clear();
            dt = new DataTable();
            //prepare query 
            string SQLquery = str;
            //prepare data adapter for sql query 
            dataAdapter1.SelectCommand.Connection = oracleConnection1;
            dataAdapter1.SelectCommand.CommandText = SQLquery;//מרוזת של השליפה
            dataAdapter1.Fill(dt);
            //return dt
            //attach source in data grid for displaying results - Data GRid 
            //View is names dgvSelect
            dataGridClient.ItemsSource = dt.DefaultView;
        }
        private void ComboboxClients(string str)
        {
            dt.Clear();
            dt = new DataTable();
            //prepare query 
            string SQLquery = str;
            //prepare data adapter for sql query 
            dataAdapter1.SelectCommand.Connection = oracleConnection1;
            dataAdapter1.SelectCommand.CommandText = SQLquery;
            dataAdapter1.Fill(dt);
            //attach source in data grid for displaying results - Data GRid 
            //View is names dgvSelect
            clientIdList.ItemsSource = convertTableToList(dt);
        }
        private DataTable ReturnDt(string str)
        {
            dt.Clear();
            dt = new DataTable();
            //prepare query 
            string SQLquery = str;
            //prepare data adapter for sql query 
            dataAdapter1.SelectCommand.Connection = oracleConnection1;
            dataAdapter1.SelectCommand.CommandText = SQLquery;//מרוזת של השליפה
            dataAdapter1.Fill(dt);
            return dt;
        }
        private void getAllClilents()
        {
            try
            {
                query = "select * from Client";
                GridBind(query);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void getAllClientId()
        {
            try
            {
                query = "select idClient  from Client";
                ComboboxClients(query);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void getAllClientsName()
        {
            try
            {
                query = "select fname  from Client";
                ComboboxClients(query);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public DataRow getClientsByName(string name)
        {
            try
            {
                query = "select* from Client";
                foreach (DataRow item in this.ReturnDt(query).Rows)
                    if (item["FNAME"].ToString() == name)
                        return item;

                throw new Exception("The name Not Found");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void ShowAllClient(object sender, RoutedEventArgs e)
        {
            if (allClientRB.IsChecked == true)
                getAllClilents();
            else
                allClientRB.IsChecked = false; 
           
            

        }
        private List<string> convertTableToList(DataTable dt)
        {
            List<string> list = new List<string>();
            foreach (DataRow item in dt.Rows)
                list.Add(item.ItemArray[0].ToString());
            return list;
        }
        //validatethe data
        private bool validate()
        {
            Boolean b = false;
            if (clientId1.ToString() == "")
                b = true;
            if (clientFname.Text.ToString() == "")
                b = true;
            if (clientLname.Text.ToString() == "")
                b = true;
            if (clientAddress.Text.ToString() == "")
                b = true;
            if (clientGeder.Text.ToString() == "")
                b = true;
            if (clientPhoneNum.Text.ToString() == "")
                b = true;
            if (clientBirthDate.SelectedDate == null)
                b = true;
            if (b)
                MessageBox.Show("missing details");
            if (clientFname.Text.ToString().Length > 20 || clientLname.Text.ToString().Length > 20 || clientAddress.Text.ToString().Length > 20 || clientPhoneNum.Text.ToString().Length > 10 || ( (clientGeder.Text.ToString() != "femal") && (clientGeder.Text.ToString() != "male") )  )
                throw new Exception("Check Your's Fields Again");
            return b;
        }
        private bool validate2()
        {
            
               Boolean b = false;
            if (Id1Client.ToString() == "")
                b = true;
            if (FnameClient.Text.ToString() == "")
                b = true;
            if (LnameClient.Text.ToString() == "")
                b = true;
            if (AddressClient.Text.ToString() == "")
                b = true;
            if (GederClient.Text.ToString() == "")
                b = true;
            if (PhoneNumClient.Text.ToString() == "")
                b = true;
            if (birthdayClient.Text.ToString() == "")
                b = true;
            
            if (clientFname.Text.ToString().Length >20)
                b = true;
            if (clientLname.Text.ToString().Length > 20)
                b = true;
            if( clientAddress.Text.ToString().Length >20)
                b = true;
            if (clientPhoneNum.Text.ToString().Length > 10)
                b = true;
            /*if ((clientGeder.Text != "femal") && (clientGeder.Text != "male"))
            {
                MessageBox.Show(clientGeder.Text + " " + clientGeder.Text.ToString());
                b = true;
            }*/
                 
            if (b)
                MessageBox.Show("Check Your's Fields Again");
            //throw new Exception("Check Your's Fields Again");
            return b;
        }
        //function for the "NEW" option
        private void AddNewClient(object sender, RoutedEventArgs e)
        {
            if (validate() == false)
            {
                try
                {
                    InsertCommand.Connection = oracleConnection1;
                    InsertCommand.CommandText = "insert into Client(Idclient,Fname,Lname,Address,Birthdate,Gender,Phonenumber)" + //values("+ 322122197, 'Shir', 'Biton', 'Cerem Bivne', To_DATE('09/01/1996', 'dd/mm/yyyy'), 'male', '0544643153')+
                                                   "values(" +
                                                              this.clientId1.Text + ","
                                                              + "'" + this.clientFname.Text.ToString() + "',"
                                                             + "'" + this.clientLname.Text.ToString() + "',"
                                                           + "'" + this.clientAddress.Text.ToString() + "', to_date('" + this.clientBirthDate.Text.ToString() + "','dd/mm/yyyy'),"
                                                           + "'" + this.clientGeder.Text.ToString() + "',"
                                                            + "'" + this.clientPhoneNum.Text.ToString() + "'"
                                                              + " )";
                    Done_messege("Client", "new");
                }

                catch (Exception ex)
                {
                    MessageBox.Show("ERROR ADD CLIENT " ,ex.Message);
                }
            }
        }
        private void Done_messege(string tableName, string mode)
        {
            try
            {
                InsertCommand.ExecuteNonQuery();
                MessageBox.Show(mode + " " + tableName + " succesfuly");
                getAllClilents();//to update the DateaGrid
               // getAllClientId();
                this.clientBirthDate.SelectedDate = DateTime.Today;
                InitFields();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        private void InitFields()
        {
            this.clientId1.Text = "";
            this.clientFname.Text = "";
            this.clientLname.Text = "";
            this.clientAddress.Text = "";
            this.clientBirthDate.SelectedDate = null ;
            this.clientGeder.Text = "";
            this.clientPhoneNum.Text = "";
            this.clientIdList.SelectedItem = null;
            this.allClientRB.IsChecked = false;
            this.getAllClientsName();
            this.Id1Client.Text = "";
            this.FnameClient.Text = "";
            this.LnameClient.Text = "";
            this.AddressClient.Text = "";
            this.GederClient.Text = "";
            this.PhoneNumClient.Text = "";
            this.birthdayClient.Text = "";
            this.clientIdList.SelectedItem = null;

        }
        
        //function for the "OPTION" option
        private void DeleteClient(object sender, RoutedEventArgs e)
        {
            try
            {
                if (clientIdList.SelectedValue == null)
                    return;

                string name = (clientIdList.SelectedValue.ToString());
                DeleteClientId(name);
                MessageBox.Show("Successfully Deleted");
                
                getAllClilents();
                getAllClientId();
                InitFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void DeleteClientId(string nameClient)
        {
            OracleCommand cmd = new OracleCommand();

            try
            {
                
                cmd = new OracleCommand();
                cmd.Connection = oracleConnection1 ;
                cmd.CommandText = "delete from client where fname=" + "'"+this.FnameClient.Text.ToString()+"'";
                cmd.CommandType = CommandType.Text;
                //oracleConnection1.Open();
                cmd.ExecuteNonQuery();
                //oracleConnection1.Close();
            }
            catch (Exception e)
            {
                throw new Exception("Error Delete "+e.Message);
              
            }
        }
        private void UpdateClient(object sender, RoutedEventArgs e)
        {

            try
            {
                if (clientIdList.SelectedValue == null)
                    return;
                string name =(clientIdList.SelectedValue.ToString());
                UpdateClientId((name));

                getAllClilents();
                getAllClientsName();

                InitFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void UpdateClientId(string name)
        {
            OracleCommand cmd = new OracleCommand();

            try
            {
                cmd = new OracleCommand();
                if (validate2() == false)//check the data
                {
                    cmd.Connection = oracleConnection1;
                    cmd.CommandText = "update Client set fname ='" + this.FnameClient.Text.ToString() +
                                                                     "', lname='" + this.LnameClient.Text.ToString() +
                                                                     "', address='" + this.AddressClient.Text.ToString() +
                                                                     "', phonenumber='" + this.PhoneNumClient.Text.ToString() +
                                                                     "' where idclient=" + this.Id1Client.Text;

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
        private void ClientIdList(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (clientIdList.SelectedItem == null)
                    return;
                String MyclientName = clientIdList.SelectedItem.ToString();
                System.Data.DataRow client = getClientsByName(MyclientName);
                this.Id1Client.Text = client["idClient"].ToString();
                this.FnameClient.Text = client["fname"].ToString();
                this.LnameClient.Text = client["lname"].ToString();
                this.AddressClient.Text = client["address"].ToString();
                this.GederClient.Text = client["gender"].ToString();
                this.birthdayClient.Text = client["birthdate"].ToString();
                this.PhoneNumClient.Text = client["phonenumber"].ToString();
           
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
    }   
}
