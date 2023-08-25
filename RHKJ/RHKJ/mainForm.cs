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
    public partial class mainForm : Form
    {
        //变量
        public static List<Goods> tempGoods;
        
        int xPos;
        int yPos;
        bool MoveFlag;

        public SettingForm setForm;
        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(mainForm));


        Bitmap m_MyImage = new Bitmap("D:\\download\\haitun.jpg");

        //把listView中的数据保存到tempGoods
        public void storeData()
        {
            for(int i=0; i< listView1.Items.Count; i++)
            {
                //for(int j=0; j<listView1.Items[i].SubItems.Count; j++)
                //{
                //    tempGoods[i].Name = listView1.Items[i].SubItems[j].Text;
                //}
                tempGoods[i].Name = listView1.Items[i].SubItems[0].Text;
                tempGoods[i].Price = int.Parse(listView1.Items[i].SubItems[1].Text);
                tempGoods[i].Desc = listView1.Items[i].SubItems[2].Text;
            }
        }
        public mainForm()
        {
            InitializeComponent();

            this.MouseWheel += new MouseEventHandler(pictureBox6_MouseWheel);
            pictureBox6.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //设置日期时间空间既显示日期也显示时间
            //如果将 Format 属性设置为 Custom 值，则需要通过设置 CustomFormat 属性值来自定义显示日期时间的格式
            dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            dateTimePicker1.Format = DateTimePickerFormat.Custom;

            timer1.Interval = 1000;
            timer1.Start();

            MySqlDataReader dataReader = Login.reader;
            //MessageBox.Show(dataReader.Read().ToString());
            this.label15.Text = dataReader.GetString(0);

            //测试ListView展示数据
            showFunc();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            dateTimePicker1.ResetText();
        }

        private void button2_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                this.contextMenuStrip_Hardware.Show(button2, new Point(e.X, e.Y));
            }
        }

        private void showFunc()
        {
            List<Goods> goods = new List<Goods>();
            goods = select();
            listView1.Items.Clear();
            foreach(var g in goods)
            {
                ListViewItem lv = new ListViewItem();
                lv.Text = g.Name;
                lv.SubItems.Add(g.Price.ToString());
                lv.SubItems.Add(g.Desc);

                listView1.Items.Add(lv);
            }

            tempGoods = goods;
        }

        public List<Goods> select()
        {
            String connectStr = "server=127.0.0.1;port=3306;user=root;password=123456;database=RHKJ_DataBase;";
            MySqlConnection myConn = new MySqlConnection();
            myConn.ConnectionString = connectStr;
            myConn.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = myConn;
            cmd.CommandText = "select goods_name,goods_price,goods_desc from goodsinfo";
            MySqlDataReader reader = cmd.ExecuteReader();

            List<Goods> list = new List<Goods>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Goods u = new Goods();
                    //u.Name = reader.GetString(0);
                    //u.Account = reader["account"].ToString();
                    u.Name = reader["goods_name"].ToString();
                    u.Price = (int)reader["goods_price"];
                    u.Desc = reader["goods_desc"].ToString();

                    list.Add(u);
                }
            }

            return list;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            setForm = new SettingForm();
            setForm.Show();
            setForm.sendImageUrlEvent += new SettingForm.sendImageUrl(Pic_Operator);
        }
        private void pictureBox6_MouseWheel(object sender, MouseEventArgs e)
        {
            var t = pictureBox6.Size;
            t.Width += e.Delta;
            t.Height += e.Delta;
            pictureBox6.Size = t;
            pictureBox6.Location = new Point((this.Width - pictureBox6.Width) / 2, (this.Height - pictureBox6.Height) / 2);
        }
        private void pictureBox6_MouseDown(object sender, MouseEventArgs e)
        {
            MoveFlag = true;
            xPos = e.X;
            yPos = e.Y;
        }

        private void pictureBox6_MouseUp(object sender, MouseEventArgs e)
        {
            MoveFlag = false;
        }

        private void pictureBox6_MouseMove(object sender, MouseEventArgs e)
        {
            if(MoveFlag)
            {
                this.pictureBox6.Left += Convert.ToInt16(e.X - xPos);
                this.pictureBox6.Top += Convert.ToInt16(e.Y - yPos);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string url = "D:/download/logo.png";
            Pic_Operator(url);

            string info = label15.Text + "点击了" + button1.Text;
            LogRecord.WriteOperLog(info);
            //pictureBox6.Image = PictureScale();

            log.Info(info);
        }

        public  void Pic_Operator(string url)
        {
            this.pictureBox6.Image = Image.FromFile(url);
        }

        Bitmap PictureScale(Image SourceImage, int nImageWidth, int nImageHeight)
        {
            if((SourceImage.Width > pictureBox6.Width) || (SourceImage.Height > pictureBox6.Height) )
            {
                if(SourceImage.Height*pictureBox6.Width < SourceImage.Width*pictureBox6.Height)
                {
                    nImageHeight = (pictureBox6.Width * SourceImage.Height) / SourceImage.Width;
                    nImageWidth = pictureBox6.Width;
                }
                else
                {
                    nImageHeight = pictureBox6.Height;
                    nImageWidth = (SourceImage.Width*pictureBox6.Height) / SourceImage.Height;
                }
            }
            else
            {
                nImageHeight = SourceImage.Height;
                nImageWidth = SourceImage.Width;
            }

            Bitmap b = new Bitmap(nImageWidth, nImageHeight);
            Graphics g = Graphics.FromImage(b);

            Rectangle srcRect = new Rectangle(0, 0, SourceImage.Width, SourceImage.Height);
            Rectangle desRect = new Rectangle(0, 0, nImageWidth, nImageHeight);

            g.DrawImage(SourceImage, desRect, srcRect, GraphicsUnit.Pixel);
            g.Dispose();
            return b;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string info = label15.Text + "点击了" + button2.Text + " ";

            LogRecord.WriteOperLog(info);

            log.Info(info);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string info = label15.Text + "点击了" + button3.Text + " ";

            LogRecord.WriteOperLog(info);

            log.Info(info);
        }
    }

    public class Goods
    {
        private string name;
        private int price;
        private string desc;

        public string Name
        {
            get;
            set;
        }
        public int Price
        {
            get;
            set;
        }
        public string Desc
        {
            get;
            set;
        }
    }
}
