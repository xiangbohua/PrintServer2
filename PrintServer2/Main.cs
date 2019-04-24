using PrintServer2.Properties;
using PrintServer2.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PrintServer2
{
    public partial class Main : Form
    {
        private NotifyIcon notifyIcon = null;
        private bool _finalExit = false;
        public Main()
        {
            InitializeComponent();
            this.InitialTray();
        }

        /// <summary>
        /// Try to get icon for task bar
        /// first: try to find a icon file named logo.icon in folder Resources. if not found then use the build-in icon
        /// </summary>
        /// <returns></returns>
        private Icon GetTaskBarIcon()
        {
            var filePath = Environment.CurrentDirectory + "\\logo.ico";
            var logo = File.Exists(filePath);
            if (logo)
            {
                return new Icon(filePath);
            }
            return Resources._default;
        }
        


        private void InitialTray()
        {
            //实例化一个NotifyIcon对象  
            notifyIcon = new NotifyIcon();
            //托盘图标气泡显示的内容  
            notifyIcon.BalloonTipText = AppSettingHelper.GetOne("IconHint", "Print server was runing in backgroud");
            //托盘图标显示的内容  
            notifyIcon.Text = AppSettingHelper.GetOne("HideHint", "Click icon to open main window");
            
            notifyIcon.Icon = this.GetTaskBarIcon();
            //true表示在托盘区可见，false表示在托盘区不可见  
            notifyIcon.Visible = true;
            //气泡显示的时间（单位是毫秒）  
            notifyIcon.ShowBalloonTip(2000);
            //notifyIcon.MouseClick += notifyIcon_MouseClick;

            MenuItem reciveNoti = new MenuItem("接收通知");
            //reciveNoti.Click += reciveNoti_Click;

            MenuItem stopNoti = new MenuItem("暂停通知");
            //stopNoti.Click += about_Click;

            MenuItem exit = new MenuItem("退出");
            //exit.Click += new EventHandler(exit_Click);

            ////关联托盘控件  
            //注释的这一行与下一行的区别就是参数不同，setting这个参数是为了实现二级菜单  
            //MenuItem[] childen = new MenuItem[] { setting, help, about, exit };  
            MenuItem[] childen = new MenuItem[] { reciveNoti, stopNoti, exit };
            notifyIcon.ContextMenu = new ContextMenu(childen);

            //窗体关闭时触发  
            this.FormClosing += this.Main_FormClosing;
        }

        /// <summary>  
        /// 窗体关闭的单击事件  
        /// </summary>  
        /// <param name="sender"></param>  
        /// <param name="e"></param>  
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_finalExit)
            {
                e.Cancel = true;
                this.Hide();
            }
        }
    }


}
