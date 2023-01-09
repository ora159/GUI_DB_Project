using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Windows;

namespace GUI_DB
{
    class SingletonConn
    {

        OracleConnectionStringBuilder myCStringB = new OracleConnectionStringBuilder();


        private static OracleConnection myConn = null;

        public static OracleConnection getConnection()
        {
            //try
            //{
                OracleConnectionStringBuilder myCStringB = new OracleConnectionStringBuilder();

                myCStringB.UserID = "SYSTEM";//put in username
                myCStringB.Password = "0R@Bitan";//put in password
                myCStringB.DataSource = "XE";//use this for connecting to Machon lev

                
                if (myConn == null)

                    myConn = new OracleConnection(myCStringB.ConnectionString);
                return myConn;
            //}
            //catch(Exception ex)
           // {
                //MessageBox.Show(ex.Message);
           // }

        }
    


    }
}





