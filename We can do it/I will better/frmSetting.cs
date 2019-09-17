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
namespace I_will_better
{
    public partial class frmSetting : Form
    {
        public frmSetting()
        {
            InitializeComponent();
        }
        public frmSetting(int a, int b, int k, int c, int d, int w)
        {
            InitializeComponent();
            killtask = k;
            A = a;
            B = b;
            C = c;
            D = d;
            W = w;
        }
        int killtask = 1;
        int A, B, C, D,W;
        private void frmSetting_Load(object sender, EventArgs e)
        {
            if (killtask == 1)
                checkBox1.CheckState = CheckState.Checked;
            else
                checkBox1.CheckState = CheckState.Unchecked;

        }
      
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                killtask = 1;
            else killtask = 0;
            saveSetting();
        }

        private void saveSetting()
        {

            String filepath = "config.ini";// đường dẫn của file muốn tạo
            FileStream fs = new FileStream(filepath, FileMode.Create);//Tạo file mới tên là test.txt            
            StreamWriter sWriter = new StreamWriter(fs, Encoding.UTF8);//fs là 1 FileStream
            sWriter.WriteLine(A);
            if (W == 1)
                sWriter.WriteLine(B);
            else
                sWriter.WriteLine(0);
            sWriter.WriteLine(killtask);
            if (W == 1)
            {
                sWriter.WriteLine(C);
                sWriter.WriteLine(D);
            }
            else
            {
                sWriter.WriteLine(0);
                sWriter.WriteLine(0);
            }
            // Ghi và đóng file
            sWriter.Flush();
            fs.Close();

        }
    }
}
