using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace RHKJ
{
    public partial class Login : Form
    {

        public MySqlConnection myConn;
        public static MySqlDataReader reader;
        public Login()
        {
            InitializeComponent();
        }

        //public MySqlConnection conn;
        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void loginBtn_Click(object sender, EventArgs e)
        {

            Connection_Sql();
            bool flag = Login_Check(myConn);
            if(flag)
            {

                //Form1 mainForm = new Form1();
                //mainForm.ShowDialog();
                this.DialogResult = DialogResult.OK;
                this.Close();
                
            }
            else
            {
                Console.WriteLine("输入错误!");
            }
        }

        public void Connection_Sql()
        {
            String connectStr = "server=127.0.0.1;port=3306;user=root;password=123456;database=RHKJ_DataBase;";
            myConn = new MySqlConnection(connectStr);
            try
            {
                myConn.Open();
                Console.WriteLine("数据库已经连接!");
                
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);

            }
            
        }

        private bool Login_Check(MySqlConnection myConn)
        {
            string accountText = this.accountText.Text;
            string passwordText = this.passwordText.Text;
            //string sql = "select account , password from userinfo where account=accountText and password=passwordText";
            string sql = string.Format("select name, account, password from userinfo where account = '{0}' and password = '{1}'", accountText, passwordText);
            //Console.WriteLine(sql);
            MySqlCommand cmd = new MySqlCommand(sql, myConn);
            reader = cmd.ExecuteReader();
            if(reader.Read())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
