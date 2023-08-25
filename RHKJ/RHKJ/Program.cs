using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RHKJ
{
    static class Program
    {
        private static log4net.ILog loginfo = log4net.LogManager.GetLogger(typeof(Program));
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            log4net.Config.XmlConfigurator.Configure();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form2());
            /// <summary>
            /// 登录成功后,让登录界面消失显示主界面,在loginForm里如果账号正确则会让其DialogResult=OK；
            /// </summary>
            Login loginForm = new Login();
            loginForm.ShowDialog();
            if(loginForm.DialogResult == DialogResult.OK)
            {
                Application.Run(new mainForm());
            }
            else
            {
                return;
            }

        }
    }
}
