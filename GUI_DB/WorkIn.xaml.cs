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
    /// Interaction logic for WorkIn.xaml
    /// </summary>
    public partial class WorkIn : Window
    {
        string query;
        DataTable dt;
        OracleDataAdapter dataAdapter1;
        OracleConnection oracleConnection1 = SingletonConn.getConnection();
        OracleCommand InsertCommand;
        public WorkIn()
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
            //nurseIdList.ItemsSource = getAllNurseId();
            getAllNurseId();
            //depNameList=ItemsSource=getAllDepName();
            getAllDepName();
            
            this.codeDepLable.Visibility = Visibility.Hidden;
            this.codeDep.Visibility = Visibility.Hidden;
            this.codeDepLable2.Visibility = Visibility.Hidden;
            this.codeDep2.Visibility = Visibility.Hidden;
            this.depNameLable.Visibility = Visibility.Hidden;
            this.depNameList2.Visibility = Visibility.Hidden;
            this.codeDep.Text = "";
            this.nurseIdList.SelectedItem = null;
            this.depNameList.SelectedItem = null;
            this.nurseIdList2.SelectedItem = null;
            this.depNameList2.SelectedItem = null;
            this.selectIdN.SelectedItem = null;
            this.allWorkInRB.IsChecked = false;
            this.NurseRB.IsChecked = false;
            this.DepRB.IsChecked = false;
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
        public DataRow getNurseById(string id)
        {
            try
            {
                query = "select* from medicalworker";
                foreach (DataRow item in this.myGridConn(query).Rows)
                    if (item["IDMW"].ToString() == id)
                        return item;

                throw new Exception("The ID Not Found");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataRow getDepCod(string name)
        {
            try
            {
                query = "select* from department";
                foreach (DataRow item in this.myGridConn(query).Rows)
                    if (item["namedepartment"].ToString() == name)
                        return item;

                throw new Exception("The ID Not Found");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void getCodeDep(string name,int num)
        {
            try
            {
                System.Data.DataRow dep = getDepCod(name);
                if(num==1)
                this.codeDep.Text = dep["codedepartment"].ToString();
                else
                    this.codeDep2.Text = dep["codedepartment"].ToString();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        private void getAllWorkIn()
        {
            try
            {
                query = "select * from work_in";
                dataGridWorkIn.ItemsSource = myGridConn(query).DefaultView;
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR: " + e.Message);
            }

        }
        private void getAllNurse()
        {
            try
            {
                query = "select * from Nurse";
                dataGridWorkIn.ItemsSource = myGridConn(query).DefaultView;
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR: " + e.Message);
            }
        }
        private void getAllDep()
        {
            try
            {
                query = "select * from Department";
                dataGridWorkIn.ItemsSource = myGridConn(query).DefaultView;
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR: " + e.Message);
            }
        }
        private void getAllNurseId()
        {
            try
            {
                query = "select idmw from medicalworker m " +
                    "where m.idmw in (select idmw from work_in)";
                nurseIdList.ItemsSource = convertTableToList(myGridConn(query));
                selectIdN.ItemsSource = convertTableToList(myGridConn(query));
                nurseIdList2.ItemsSource= convertTableToList(myGridConn(query));
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR: " + e.Message);
            }
        }
        private void getAllDepName()
        {
            try
            {
                query = "select namedepartment from department";
                depNameList.ItemsSource = convertTableToList(myGridConn(query));
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR: " + e.Message);
            }
        }
        private void getDep_Nurse(int id,int option)
        {
            try
            {
                query = "select namedepartment from work_in where idmw="+id;
                if (option==1)
                    dataGridWorkIn.ItemsSource = myGridConn(query).DefaultView;
                else if(option==2)
                   depNameList2.ItemsSource = convertTableToList(myGridConn(query));
                
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR from getDep" + e.Message);
            }
        }
        private List<string> convertTableToList(DataTable dt)
        {
            List<string> list = new List<string>();
            foreach (DataRow item in dt.Rows)
                list.Add(item.ItemArray[0].ToString());
            return list;
        }
        private bool validate()
        {
            Boolean b = false;
            if (this.nurseIdList.SelectedItem.ToString() == null)
                b = true;
            if(this.depNameList.SelectedItem.ToString()==null)
                b = true;
            if (this.codeDep.Text == "")
                b = true;
            if (b)
                MessageBox.Show("missing details");
            return b;
        }
        private bool validate2()
        {
            Boolean b = false;
            if (this.nurseIdList2.SelectedItem.ToString() == null)
                b = true;
            if (this.depNameList2.SelectedItem.ToString() == null)
                b = true;
            if(this.codeDep2.Text=="")
                b = true;
            if (b)
                MessageBox.Show("missing details");
            return b;
        }
        private void ShowAllWorkIn(object sender, RoutedEventArgs e)
        {
            this.NurseRB.IsChecked = false;
            this.DepRB.IsChecked = false;
            getAllWorkIn();

        }
        private void ShowNurse(object sender, RoutedEventArgs e)
        {
            this.allWorkInRB.IsChecked = false;
            this.DepRB.IsChecked = false;
            getAllNurse();     
        }
        private void ShowDep(object sender, RoutedEventArgs e)
        {
            this.allWorkInRB.IsChecked = false;
            this.NurseRB.IsChecked = false;
            getAllDep();
        }

        private void nurseIdList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (nurseIdList.SelectedItem == null)
                    return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }

        }
        private void depNameList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (nurseIdList.SelectedItem == null)
                return;
            this.codeDepLable.Visibility = Visibility.Visible;
            this.codeDep.Visibility = Visibility.Visible;
            string name = this.depNameList.SelectedValue.ToString();
            getCodeDep(name,1);
        }

        private void AddNewWorkIn(object sender, RoutedEventArgs e)
        {
            try
            {
                if (validate() == false)
                {
                    int id = int.Parse((nurseIdList.SelectedValue.ToString()));
                    int code = int.Parse((codeDep.Text.ToString()));
                    InsertCommand.Connection = oracleConnection1;
                    InsertCommand.CommandText = "insert into work_in (idMW,Codedepartment,Namedepartment)" +
                                                   "values(" + id + "," + code + "," + "'" + this.depNameList.SelectedItem.ToString() + "')";
                    
                }
                InsertCommand.ExecuteNonQuery();
                MessageBox.Show("Add department to nurse succesfuly");
                InitFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }


        }

        private void selectIdN_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {// select id nurse and we show the all department he work in
            if (selectIdN.SelectedItem == null)
                return;
            int id =int.Parse(selectIdN.SelectedItem.ToString());
            getDep_Nurse(id,1);

        }

        private void nurseIdList2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (nurseIdList2.SelectedItem == null)
                    return;
                int id = int.Parse(nurseIdList2.SelectedItem.ToString());
                this.depNameLable.Visibility = Visibility.Visible;
                this.depNameList2.Visibility = Visibility.Visible;

                getDep_Nurse(id,2);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void depNameList2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (nurseIdList2.SelectedItem == null)
                return;
            this.codeDepLable2.Visibility = Visibility.Visible;
            this.codeDep2.Visibility = Visibility.Visible;
            string name = this.depNameList2.SelectedValue.ToString();
            getCodeDep(name,2);
        }

        
        private void DeleteById(int id)
        {
            OracleCommand cmd = new OracleCommand();

            try
            {

                cmd = new OracleCommand();
                cmd.Connection = oracleConnection1;
                cmd.CommandText = "delete from work_in where idmw=" + "'" + this.nurseIdList2.SelectedItem.ToString() + "' "
                    +"and namedepartment="+"'"+this.depNameList2.SelectedItem.ToString()+"'";
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
               
            }
            catch (Exception e)
            {
                throw new Exception("ERORE Delete:" + e.Message);

            }
        }

        private void DeleteWorkIn(object sender, RoutedEventArgs e)
        {

            try
            {
                if (nurseIdList2.SelectedValue == null)
                    return;

                int id = int.Parse(nurseIdList2.SelectedItem.ToString());
                DeleteById(id);
                MessageBox.Show("Successfully Deleted");

                InitFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
