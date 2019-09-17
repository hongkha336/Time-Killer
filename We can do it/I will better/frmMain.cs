using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Management;
using System.Dynamic;
using Microsoft.Win32;
using System.IO;
namespace I_will_better
{
    public partial class frmMain : Form
    {

        /*
           System.Diagnostics.Process.Start("shutdown", "-s -f -t 0");// shutdown
             System.Diagnostics.Process.Start("shutdown", "-r -f -t 0");//restart
         * System.Diagnostics.Process.Start("shutdown", "-r -f ");
         */
        public frmMain()
        {
            InitializeComponent();
            NameProcessBrowser = new List<string>();
            NameProcessGame = new List<string>();
        }

        int DefaultLang = 1;
        int isKillTask = 1;
        int isStrong = 0;
        Process[] processList;
        List<string> Namelist;
        List<string> NameProgramelist;
        List<string> NameProcessBrowser;
        List<string> NameProcessGame;

        int KillGame = 0;
        int killBrowser = 0;

        int height = 200;
        int height2 = 597;


        private void frmMain_Load(object sender, EventArgs e)
        {
           // vô hiệu hóa nút thoát
            thoatToolStripMenuItem.Visible = false;
            GetInstalledApps();
            this.Height = height;
            // tắt pn
            pn.Enabled = false;
            // Phím tắt
            this.KeyPreview = true;
            // LoadSetting
            loadSetting();
            // Kiểm tra ngôn ngữ
            checkLangue();
            // load Dữ liệu chặn
            loadBrowser();
            loadGame();
            //Khởi động tiến trình
            timerKillTask.Enabled = true;
            timer1.Enabled = true;

            //
            if (KillGame == 1)
                chkGame.CheckState = CheckState.Checked;
            if (killBrowser == 1)
                chkBrowser.CheckState = CheckState.Checked;
            if (isStrong == 1)
                chkStrong.CheckState = CheckState.Checked;
          

        }

        bool isWordIn(string word, string WORD)
        {
            WORD = WORD.ToLower();
            word = word.ToLower();
            for (int i = 0; i <= WORD.Length - word.Length; i++)
                if (WORD.Substring(i, word.Length) == word)
                    return true;
            return false;
        }

        public void GetInstalledApps()
        {
            NameProgramelist = new List<string>();
            string uninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(uninstallKey))
            {
                foreach (string skName in rk.GetSubKeyNames())
                {
                    using (RegistryKey sk = rk.OpenSubKey(skName))
                    {
                        try
                        {
                            string temp = sk.GetValue("DisplayName").ToString();
                            // loại một số phần mềm hệ thống như Microsoft;
                            // if(temp.Substring(0,9).ToLower() !="microsoft")
                            if (!isWordIn("microsoft", temp) && !isWordIn("windows", temp) && !isWordIn("office", temp)
                                && !isWordIn("intel", temp) && !isWordIn("sql", temp) && !isWordIn("update", temp) && !isWordIn("visual", temp))
                                NameProgramelist.Add(temp);
                        }
                        catch (Exception ex)
                        { }
                    }
                }

                   listBox2.DataSource = NameProgramelist;
                //label1.Text = listBox1.Items.Count.ToString();

            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Namelist = new List<string>();
            processList = Process.GetProcesses();
            foreach (Process process in processList)
            {
                Namelist.Add(process.ProcessName);
            }

            listBox1.DataSource = Namelist;

            if (killBrowser != 0)
            {
                killOpenBrowser();
            }
            if(KillGame !=0)
            {
                killOpenGame();
            }
            listBox1.DataSource = Namelist;
        }

        private void killProcess(string name)
        {
            try
            {
                if (!name.Equals(""))
                {
                    foreach (var process in Process.GetProcessesByName(name))
                    {
                        process.Kill();

                    }
                }
            }
            catch
            {}
        }
        private void killOpenBrowser()
        {
            for (int j = 0; j < NameProcessBrowser.Count; j++)
                 for(int i=0; i<Namelist.Count; i++)
                {
                    if (isWordIn(NameProcessBrowser[j], Namelist[i]))
                    {
                        killProcess(Namelist[i]);
                        if(isStrong == 1)
                        {
                            System.Diagnostics.Process.Start("shutdown", "-r -f -t 0");
                        }
                        break;
                    }
                }
        }

        private void killOpenGame()
        {
            for (int j = 0; j < NameProcessGame.Count; j++)
                for (int i = 0; i < Namelist.Count; i++)
                {
                    if (isWordIn(NameProcessGame[j], Namelist[i]))
                    {
                        killProcess(Namelist[i]);
                        if (isStrong == 1)
                        {
                            System.Diagnostics.Process.Start("shutdown", "-r -f -t 0");
                        }
                        break;
                    }
                }
        }

        private void killTaskManager()
        {
            killProcess("taskmgr");
        }

        private void loadBrowser()
        {
            try
            {
                using (StreamReader sr = new StreamReader("Databrowser.ini"))
                {
                    string line;

                    // doc va hien thi cac dong trong file cho toi
                    // khi tien toi cuoi file. 
                    while ((line = sr.ReadLine()) != null)
                    {
                        NameProcessBrowser.Add(line);
                    }
                }
            }
            catch (Exception e)
            {}
        }

        private void loadGame()
        {
            try
            {
                using (StreamReader sr = new StreamReader("Datagame.ini"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.ToLower();
                        NameProcessGame.Add(line);
                    }
                }
            }
            catch (Exception e)
            { }
        }

        string sloganEng = "If you want to stop here, please think about why chose me! \n Exit?";
        string sloganVi = "Nếu bạn muốn dừng ở đây, hãy nghĩ lại lý do tại sao bạn đã chọn tôi! \n Thoát?";

        private void thoatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult tl;
            if (DefaultLang != 1)
                tl = MessageBox.Show(sloganEng, "We Can Do It", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            else
                tl = MessageBox.Show(sloganVi, "We Can Do It", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(tl == DialogResult.Yes)
             Application.Exit();
        }

        private void thongtinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("copyright by \n Đặng Nguyễn Hồng Kha");
           
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (frmMain.ModifierKeys == Keys.Alt || frmMain.ModifierKeys == Keys.F4)
            {
                e.Cancel = true;
                
            }   


        }

        private void timerKillTask_Tick(object sender, EventArgs e)
        {
            if(isKillTask == 1)
            killTaskManager();
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.K)
            {
                this.Close();
            }
            if (e.Control && e.KeyCode == Keys.L)
            {
                frmSetting f = new frmSetting(DefaultLang, isStrong, isKillTask, KillGame, killBrowser, willSave);
                f.ShowDialog();
                loadSetting();
            }
            if(e.Control && e.Alt && e.KeyCode == Keys.A)
            {
                if (panel1.Enabled == true)
                {
                    panel1.Enabled = false;
                    hideToolStripMenuItem.Visible = false;
                }
                else
                {
                    panel1.Enabled = true;
                    hideToolStripMenuItem.Visible = true;
                }
            }
        }

        private void loadSetting()
        {
            int i = 0;
            try
            {
                using (StreamReader sr = new StreamReader("config.ini"))
                {
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                     if(i==0)
                     {
                         DefaultLang = Convert.ToInt16(line);
                         i++;
                     }
                     else
                     {
                         if(i==1)
                         {
                             isStrong = Convert.ToInt16(line);
                             i++;
                         }
                         else
                         {
                             if(i == 2)
                             {
                                 isKillTask = Convert.ToInt16(line);
                                 i++;
                             }
                             else
                             {
                                if(willSave == 1)
                                {
                                    if(i == 3)
                                    {
                                        KillGame = Convert.ToInt16(line);
                                        i++;
                                    }
                                    else
                                    {
                                        if(i==1)
                                        {
                                            killBrowser = Convert.ToInt16(line);
                                            i++;
                                        }
                                    }
                                }

                             }
                         }
                         
                     }
                    }
                }
            }
            catch (Exception e)
            { }
        }

        private void chkBrowser_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBrowser.Checked)
                killBrowser = 1;
            else killBrowser = 0;
            if (willSave == 1)
            saveSetting();
        }

        private void goitiengviet()
        {
            chucnagToolStripMenuItem.Text = "Chức năng";
            thoatToolStripMenuItem.Text = "Thoát";
         
            thongtinToolStripMenuItem.Text = "Thông tin";
            ngonnguToolStripMenuItem.Text = "Ngôn ngữ";
            tienganhToolStripMenuItem.Text = "Tiếng Anh";
            tiengvietToolStripMenuItem.Text = "Tiếng Việt";

            chkBrowser.Text = "Vô hiệu hóa trình duyệt";
            chkGame.Text = "Vô hiệu hóa game online";
            chkTime.Text = "Hẹn giờ tắt máy";
            chkStrong.Text = "Chế độ trừng phạt";
            checkBox1.Text = "Ghi nhớ lựa chọn";
            btnLess.Text = "Thu nhỏ";
            morongToolStripMenuItem.Text = "Mở rộng";
            lbDS1.Text = "Danh sách tiến trình";
            lbDS2.Text = "Danh sách phần mềm";
        }

        private void goitienganh()
        {
            lbDS1.Text = "Processing list";
            morongToolStripMenuItem.Text = "Show more";
            btnLess.Text = "Show less";
            chucnagToolStripMenuItem.Text = "Option";
            thoatToolStripMenuItem.Text = "Exit";
          
            thongtinToolStripMenuItem.Text = "About";
            ngonnguToolStripMenuItem.Text = "Languages";
            tienganhToolStripMenuItem.Text = "English";
            tiengvietToolStripMenuItem.Text = "Vietnamese";

            chkBrowser.Text = "Disable all browers";
            chkGame.Text = "Disable game online";
            chkTime.Text = "Put a shutdown timer";
            chkStrong.Text = "Punishment mode";
            checkBox1.Text = "Save setting";
            lbDS2.Text = "Progarams list";
        }

       private void checkLangue()
        {
            if (DefaultLang == 1)
                goitiengviet();
            else goitienganh();
        }
    
     

        private void saveSetting()
       {
  
           String filepath = "config.ini";// đường dẫn của file muốn tạo
           FileStream fs = new FileStream(filepath, FileMode.Create);//Tạo file mới tên là test.txt            
           StreamWriter sWriter = new StreamWriter(fs, Encoding.UTF8);//fs là 1 FileStream
           sWriter.WriteLine(DefaultLang);
            if(willSave ==1)
                 sWriter.WriteLine(isStrong);
            else
                sWriter.WriteLine(0);
           sWriter.WriteLine(isKillTask);
           if (willSave == 1)
           {
               sWriter.WriteLine(killBrowser);
               sWriter.WriteLine(KillGame);
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

        private void chkGame_CheckedChanged(object sender, EventArgs e)
        {
            if(chkGame.Checked)
            {
                KillGame = 1;
            }
            else
            {
                KillGame = 0;
            }
            if (willSave == 1)
            saveSetting();
        }

        private void chkStrong_CheckedChanged(object sender, EventArgs e)
        {
            if (chkStrong.Checked)
                isStrong = 1;
            else isStrong = 0;
            if(willSave == 1)
            saveSetting();
        }

        private void chkTime_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTime.Checked)
                pn.Enabled = true;
            else
                pn.Enabled = false;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            int time = Convert.ToInt16(numericUpDown1.Value) * 3600 + Convert.ToInt16(numericUpDown2.Value) * 60 + Convert.ToInt16(numericUpDown3.Value);
            System.Diagnostics.Process.Start("shutdown", "-s -f -t " + time.ToString());
            if (DefaultLang == 1)
            {
                MessageBox.Show("Đã thiết lập");
            }
            else
                MessageBox.Show("Timer Set");
        }

        private void tiengvietToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DefaultLang = 1;
            saveSetting();
            checkLangue();
            
        }

        private void tienganhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DefaultLang = 0;
            saveSetting();
            checkLangue();
        }

        int willSave = 0;
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                willSave = 1;
            else
                willSave = 0;

        }

        private void morongToolStripMenuItem_Click(object sender, EventArgs e)
        {
            morongToolStripMenuItem.Visible = false;
            this.Height = height2;
        }

        private void btnLess_Click(object sender, EventArgs e)
        {
            morongToolStripMenuItem.Visible = true;
            this.Height = height;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void hideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            hideToolStripMenuItem.Visible = false;
        }

 

      


      
    }

}
