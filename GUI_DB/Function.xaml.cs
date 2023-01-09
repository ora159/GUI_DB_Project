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
    /// Interaction logic for Function.xaml
    /// </summary>
    public partial class Function : Window
    {
        string query;
        DataTable dt;
        OracleDataAdapter dataAdapter1;
        OracleConnection oracleConnection1 = SingletonConn.getConnection();
        OracleCommand InsertCommand;
        OracleCommand callProcCommand;
        OracleCommand callFuncComm;
        public Function()
        {
            InitializeComponent();
            if (oracleConnection1.State == System.Data.ConnectionState.Closed)
                oracleConnection1.Open();
            dataAdapter1 = new OracleDataAdapter();
            dataAdapter1.SelectCommand = new OracleCommand();
            //UpdateCommand=new OracleCommand();
            InsertCommand = new OracleCommand();
            callProcCommand = new OracleCommand();
            callFuncComm=new OracleCommand();
            dt = new DataTable();
            InitFields();
        }
        private void InitFields()
        {
            getIdDoctor();
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
        private DataRow getMyRowById(string id)
        {
            try
            {
                query = "select* from medicalworker";
                foreach (DataRow item in this.myGridConn(query).Rows)
                    if (item["IDMW"].ToString() == id)
                        return item;

                throw new Exception("The code Not Found");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            /* callProcCommand.Connection = oracleConnection1;
             callProcCommand.CommandType = CommandType.StoredProcedure;
             callProcCommand.CommandText = "LessThenAvg_numOfWorkYear";  
             callProcCommand.Parameters.Add("dId", OracleType.Number);
             callProcCommand.Parameters["dId"].Direction = ParameterDirection.Input;
             callProcCommand.Parameters["dId"].Value = int.Parse (this.idDoctor.Text.ToString());
             callProcCommand.Parameters.Add("dSalary ", OracleType.Number);
             callProcCommand.Parameters["dSalary"].Direction = ParameterDirection.Input;
             //callProcCommand.Parameters["dSalary"].Value =int.Parse( this.salaryD.Text.ToString());
             callProcCommand.ExecuteNonQuery();*/
            try
            {

                callFuncComm.Connection = oracleConnection1;
                callFuncComm.CommandType = CommandType.StoredProcedure;
                callFuncComm.CommandText = "MoreThenAvg_numOfWorkYear";
                callFuncComm.Parameters.Add("b", OracleType.Number);
                callFuncComm.Parameters["b"].Direction = ParameterDirection.ReturnValue;
                callFuncComm.Parameters.Add("idDoctor", OracleType.Number);
                callFuncComm.Parameters["idDoctor"].Direction = ParameterDirection.Input;
               callFuncComm.Parameters["idDoctor"].Value = this.idDoctor.Text.ToString();
               callFuncComm.ExecuteNonQuery();

            this.ans.Text = callFuncComm.Parameters["b"].Value.ToString();
                if (ans.Text == "1")
                    MessageBox.Show(" Yes");
                else MessageBox.Show("No");

                callFuncComm.ExecuteNonQuery();
           }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void getIdDoctor()
        {
            string query = "select idMW from doctor";
            idDoctor.ItemsSource = convertTableToList(myGridConn(query));
        }
        
        private void idDoctor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          

            try
            {
                if (idDoctor.SelectedItem == null)
                    return;
                String Myid = idDoctor.SelectedValue.ToString();
                System.Data.DataRow doctor = getMyRowById(Myid);
                this.salaryD.Text = doctor["SALARY"].ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
