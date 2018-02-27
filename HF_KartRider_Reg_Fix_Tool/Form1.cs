using System;
using System.Windows.Forms;

namespace HF_KartRider_Reg_Fix_Tool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
                
        private void Button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog kr = new OpenFileDialog
            {
                //选择文件标题
                Title = "请选择韩服跑跑主文件",
                //筛选
                Filter = "KartRider.exe|KartRider.exe"
            };
            if (kr.ShowDialog() == DialogResult.OK)
                textBox1.Text = kr.FileName;
        }

        char[] temp = new char[50];
        char[] temp1 = new char[50];

        //写入注册表信息
        private void Button2_Click(object sender, EventArgs e)
        {
            string fileName = textBox1.Text;

            //检测是否选择安装路径
            if (fileName == "")
                MessageBox.Show("您还没有选择安装位置，请重试！", "系统提示", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                //修复按钮不可用
                button2.Enabled = false;
                button2.Text = "正在修复...";

                if (Core.GetOS3264().Equals("32"))
                    Core.WriteReg32(fileName);
                else if (Core.GetOS3264().Equals("64"))
                    Core.WriteReg64(fileName);
                else
                    MessageBox.Show("系统错误，点击确认按钮退出程序", "系统错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    
                if (Core.flag == true)
                {
                    MessageBox.Show("修复成功！");
                    button2.Visible = false;
                    button3.Visible = true;
                    button3.Enabled = true;
                }
                else
                {
                    MessageBox.Show("修复失败，请重试！");
                    button2.Enabled = true;
                    button2.Text = "开始修复";
                }
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
