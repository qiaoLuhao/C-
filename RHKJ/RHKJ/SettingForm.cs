using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MySql.Data.MySqlClient;
namespace RHKJ
{
    public partial class SettingForm : Form
   {
        private static log4net.ILog setlogInfo = log4net.LogManager.GetLogger(typeof(SettingForm));
        string connStr = "server=127.0.0.1;port=3306;user=root;password=123456;database=RHKJ_DataBase;";
        MySqlConnection myConn;
        MySqlDataAdapter dataAdapter;
        DataSet ds;

        public delegate void sendImageUrl(string url);
        public event sendImageUrl sendImageUrlEvent;

        AppInfo app = new AppInfo();
        public static Font font = new Font("宋体", 9, FontStyle.Regular);

        public SettingForm()
        {
            InitializeComponent();
            app.FontChange += this.OP_Font;

            timer1.Interval = 100;
            timer1.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string info = Login.reader.GetString(0) + "点击了设置中" + button1.Text + "按钮";
            LogRecord.WriteOperLog(info);
            setlogInfo.Info(info);

            StreamWriter wstream = new StreamWriter(@"D:\project\RHKJ\DataFile\info.csv",true);
            
            for(int i=0; i<mainForm.tempGoods.Count; i++)
            {
                //string s = "";
                //s += mainForm.tempGoods[i].Name + " " + mainForm.tempGoods[i].Price + " " + mainForm.tempGoods[i].Desc;
                //MessageBox.Show(s);
                //wstream.WriteLine(s);
                wstream.Write(mainForm.tempGoods[i].Name + ",");
                wstream.Write(mainForm.tempGoods[i].Price + ",");
                wstream.Write(mainForm.tempGoods[i].Desc + ",");
                wstream.Write(DateTime.Now.ToString() + "\r\n");
            }
            wstream.Flush();
            wstream.Close();
            MessageBox.Show("数据保存完毕");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string info = Login.reader.GetString(0) + "点击了设置中" + button1.Text + "按钮";
            LogRecord.WriteOperLog(info);
            setlogInfo.Info(info);

            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Title = "打开文件";
            //过滤文件
            openFile.Filter = "图片文件|*.jpg;*.png;*.gif;.*bmp";
            if(DialogResult.OK == openFile.ShowDialog())
            {
                //MessageBox.Show(openFile.FileName);
                string url = openFile.FileName;
                sendImageUrlEvent(url);
            }

        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            propertyGrid1.Hide();

            myConn = new MySqlConnection();
            myConn.ConnectionString = connStr;
            myConn.Open();

            string sql = "select * from goodsinfo";
            dataAdapter = new MySqlDataAdapter(sql, myConn);
            ds = new DataSet();
            dataAdapter.Fill(ds, "goodsinfo");
            dataGridView1.DataSource = ds.Tables["goodsinfo"];
            dataGridView1.AutoGenerateColumns = false;

        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if (dataGridView1.SelectedRows.Count > 0)
            {
                //int a = dataGridView1.SelectedRows[0].Index;
                string id = dataGridView1.Rows[0].Cells[0].Value.ToString();
                string goodsName = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                string goodsPrice = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                string goodsDesc = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                string goodsVendor = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                setlogInfo.InfoFormat("删除了id={0}-{1}-{2}-{3}-{4}", id, goodsName, goodsPrice, goodsDesc, goodsVendor);


                dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index); //要把一行全选中才能删除

                MySqlCommandBuilder builder = new MySqlCommandBuilder(dataAdapter);
                dataAdapter.Update(ds, "goodsinfo");
            }
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MySqlCommandBuilder builder = new MySqlCommandBuilder(dataAdapter);
            dataAdapter.Update(ds, "goodsinfo");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            propertyGrid1.Show();            
            propertyGrid1.SelectedObject = app;

            //在点击后,订阅AppInfo的事件,放在这里不行,因为恶意点击的话,即使不改变字体设置,事件处理器也会运行
            //app.FontChange += this.OP_Font;
        }
        
        private void OP_Font()
        {
            MessageBox.Show("beidiaoyongle");
            propertyGrid1.Font = app.WindowFont;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(font != app.WindowFont)
            {
                font = app.WindowFont;
                app.SomeOp();
            }
            else
            {
                ;
            }
        }

    }
}
