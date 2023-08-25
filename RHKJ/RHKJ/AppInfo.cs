using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHKJ
{
    public delegate void FontEventHandler();
    public class AppInfo
    {
        private string greetingText = "欢迎使用应用程序!";
        private string appVersion = "1.0";
        private string name = "RHKJ";

        private Size windowSize = new Size(100, 100);
        private Font windowFont = new Font("宋体", 9, FontStyle.Regular);
        private Color toolBarColor = SystemColors.Control;

        public event FontEventHandler FontChange;

        public void SomeOp()
        {
            if(this.FontChange != null)
            {
                this.FontChange.Invoke();
            }
        }
        [CategoryAttribute("信息"), DefaultValueAttribute(true), ReadOnlyAttribute(true)]
        public string GreetingText
        {
            get { return greetingText; }
            set { greetingText = value; }
        }
        [CategoryAttribute("信息"),DescriptionAttribute("版本号"),DefaultValueAttribute("1.0"),ReadOnlyAttribute(true)]
        public string AppVersion
        {
            get { return appVersion; }
            set { appVersion = value; }
        }
        [CategoryAttribute("信息"), DescriptionAttribute("公司"), DefaultValueAttribute("RHKJ"), ReadOnlyAttribute(true)]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        [CategoryAttribute("设置")]
        public Size WindowSize
        {
            get { return windowSize; }
            set { windowSize = value; }
        }
        [CategoryAttribute("设置")]
        public Font WindowFont
        {
            get { return windowFont; }
            set { windowFont = value; }
        }
        [CategoryAttribute("设置")]
        public Color ToolbarColor
        {
            get { return toolBarColor; }
            set { toolBarColor = value; }
        }
    }
}
