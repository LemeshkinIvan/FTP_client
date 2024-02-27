using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Shapes;

namespace ftpExample
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string PREV_ADDRES = "ftp://";
        const string DEFAULT_PATH = @"C:\";

        string FTP_curr_path;

        string[] copy_buffer;

        string IP;
        string login;
        string password;

        string[] nextFolder = new string[2];

        //file manager logic

        public MainWindow()
        {
            InitializeComponent();
            this.Title = "Tiny FTP Manager(x64)";

            setDisk();
            getFilesAndDir(DEFAULT_PATH);            
            // menu paste disable
        }

        private void setDisk()
        {
            var diskList = DriveInfo.GetDrives();

            for (int i = 0; i < diskList.Length; i++)
            {
                comboDisk.Items.Add(diskList[i]);
            }

            comboDisk.Text = DEFAULT_PATH;
        }

        private void getFilesAndDir(string path)
        {
            try
            {
                Dictionary<string, string> files = new Dictionary<string, string>();

                foreach (var f in FileManager.ShowWhatHaveThisDirectory(path))
                {
                    var path_file = f.Key;
                    int i = path_file.LastIndexOf('\\') + 1;
                    string p = path_file.Remove(0, i);
                    files.Add(p, f.Value);

                    files.Remove(path_file);
                }

                lblCurrPath.Content = path;
                lbx_local.ItemsSource = files;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_connect_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnConn_Click(object sender, RoutedEventArgs e)
        {

        }

        //context Menu logic
        private void createFile(object sender, RoutedEventArgs e)
        {
            createPage c_p = new createPage();

            if (c_p.ShowDialog() == true)
            {
                string name = c_p.FileName;

                if (name != null)
                {
                    try
                    {
                        FileManager.createFile(lblCurrPath.Content.ToString() + "\\" + name);
                        getFilesAndDir(lblCurrPath.Content.ToString());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        private void createFolder(object sender, RoutedEventArgs e)
        {
            createPage c_p = new createPage();

            if (c_p.ShowDialog() == true)
            {
                string name = c_p.FileName;

                if (name != null)
                {
                    try
                    {
                        FileManager.createFolder(lblCurrPath.Content.ToString() + "\\" + name);
                        getFilesAndDir(lblCurrPath.Content.ToString());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        private void deleteFile(object sender, RoutedEventArgs e)
        {
            var item = lbx_local.SelectedItem;

            if (item == null)
            {
                return;
            }

            string[] temp = item.ToString().Split(',');
            string path = lblCurrPath.Content.ToString() + temp[0].Trim('[');

            try
            {
                FileManager.deleteFile(path);
                getFilesAndDir(lblCurrPath.Content.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void copyFile(object sender, RoutedEventArgs e)
        {
            var items = lbx_local.SelectedItems;

            if (items == null)
            {
                return;
            }

            copy_buffer = new string[items.Count];
            int u = 0;

            foreach (var item in items)
            {
                int index = item.ToString().LastIndexOf(',');
                string f = item.ToString().Substring(0, index);

                f = f.Trim('[');

                copy_buffer[u] = f;
                u++;
            }
            
            return;
        }

        private void pasteFile(object sender, RoutedEventArgs e)
        {
             bool isEmpty = true;

            if (copy_buffer == null)
            {
                MessageBox.Show("Буфер пуст");
                return;
            }

           foreach(var t in copy_buffer)
           {
                if(t != null)
                {   
                    isEmpty = false;
                    break;
                }
           }

            if (isEmpty)
            {

            }
            else
            {
                MessageBox.Show("Буфер пуст");
            }
        }

        private void renameFile(object sender, RoutedEventArgs e)
        {
            var item = lbx_local.SelectedItem;
            // можно и лучше
            string[] temp = item.ToString().Split(',');

            string old_name = temp[0].Trim('[');

            createPage c_p = new createPage();

            if (old_name == null)
            {
                return;
            }

            if (c_p.ShowDialog() == true)
            {
                string new_name = lblCurrPath.Content.ToString() + c_p.FileName;

                if (new_name != null)
                {
                    FileManager.renameFile(lblCurrPath.Content.ToString(), old_name.ToString(), new_name);
                }

                getFilesAndDir(lblCurrPath.Content.ToString());
            }
        }

        private void openFile(object sender, RoutedEventArgs e)
        {
            var d = lbx_local.SelectedItem;

            if (d == null)
            {
                return;
            }

            if (lbx_local.SelectedIndex == 0)
            {
                var q = lblCurrPath.Content.ToString();

                for (int i = 0; i < 2; i++)
                {
                    int w = q.LastIndexOf('\\');
                    q = q.Remove(w, q.Length - w);
                }

                getFilesAndDir(q + '\\');
                return;
            }
            // да, это глобалка
            nextFolder = d.ToString().Split(',');
            nextFolder[0] = nextFolder[0].Remove(0, 1);

            if (nextFolder[0].Length != 3)
            {
                nextFolder[0] += '\\';
            }

            getFilesAndDir(lblCurrPath.Content.ToString() + nextFolder[0]);
        }

        // ftp Logic
        private void btnFTP(object sender, RoutedEventArgs e)
        {
            ConnectToFTP ftp_w = new ConnectToFTP();

            if (ftp_w.ShowDialog() == true)
            {
                if ((ftp_w.IP != null) && (ftp_w.Login != null))
                {
                    IP = ftp_w.IP;
                    login = ftp_w.Login;
                    password = ftp_w.Password;

                    try
                    {
                        ClientFtp.Init(PREV_ADDRES + IP, login, password);

                        Regex regex = new Regex(@"^([d-])([rwxt-]{3}){3}\s+\d{1,}\s+.*?(\d{1,})\s+(\w+\s+\d{1,2}\s+(?:\d{4})?)(\d{1,2}:\d{2})?\s+(.+?)\s?$",
                            RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

                        List<FileDirectoryInfo> list = ClientFtp.ListDirectoryDetails()
                                                             .Select(s =>
                                                             {
                                                                 Match match = regex.Match(s);
                                                                 if (match.Length > 5)
                                                                 {
                                                                     string type = match.Groups[1].Value == "d" ? "DIR.png" : "FILE.png";

                                                                     string size = "";
                                                                     if (type == "FILE.png")
                                                                         size = (Int32.Parse(match.Groups[3].Value.Trim()) / 1024).ToString() + " кБ";

                                                                     return new FileDirectoryInfo(size, type, match.Groups[6].Value, match.Groups[4].Value, txt_adres.Text);
                                                                 }
                                                                 else return new FileDirectoryInfo();
                                                             }).ToList();

                        list.Add(new FileDirectoryInfo("", "DEFAULT.png", "...", "", txt_adres.Text));
                        list.Reverse();

                        lbx_Second.DataContext = list;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString() + ": \n" + ex.Message);
                    }
                }
                else
                {
                    return;
                }
            }
        }
        string ftpCutName()
        {
            string new_line = "";
            int count = 0;

            for (int y = 0; y < FTP_curr_path.Length; y++)
            {
                if (FTP_curr_path[y] == '"')
                {
                    count++;
                }

                if (count == 1)
                {
                    new_line += FTP_curr_path[y];
                }

                if (count == 2)
                {
                    break;
                }
            }
            new_line = new_line.Trim('"');

            return new_line;
        }
        private void ftp_openFolder(object sender, RoutedEventArgs e)
        {
            FileDirectoryInfo item = (FileDirectoryInfo)lbx_Second.SelectedItem;

            if (item == null)
            {
                return;
            }

            if (item.Name == "...")
            {
                FTP_curr_path = ClientFtp.PrintWorkingDirectory();
                int pos = ftpCutName().LastIndexOf('/');
                FTP_curr_path = FTP_curr_path.Substring(0, pos);
              
                ClientFtp.ChangeWorkingDirectory(FTP_curr_path);
                return;
            }

            FTP_curr_path = ClientFtp.PrintWorkingDirectory();         
            string path = ftpCutName() + '/' + item.Name;
            MessageBox.Show(path);

            try
            {
                ClientFtp.ChangeWorkingDirectory(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void ftp_copyFile(object sender, RoutedEventArgs e)
        {
            }

        void ftp_createFile(object sender, RoutedEventArgs e)
        {

        }

        void ftp_pasteFile(object sender, RoutedEventArgs e)
        {

        }

        void ftp_deleteFile(object sender, RoutedEventArgs e)
        {

        }

        private void btnSearch2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void lbx_local_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void comboDisk_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var t = comboDisk.SelectedValue;
            getFilesAndDir(t.ToString());
            lblCurrPath.Content = t.ToString();

            // размеры дисков выводим
            string[] sizes = new string[2];
            var j = DriveInfo.GetDrives();

            foreach (var el in j)
            {
                if (el.Name == t.ToString())
                {
                    lblDiskSize.Content = el.TotalFreeSpace / 2048 + " мб из " + el.TotalSize / 2048 + " мб свободно";
                }
            }
        }

        private void getSettingsPage(object sender, RoutedEventArgs e)
        {
            SettingsPage s_p = new SettingsPage();

            if (s_p.ShowDialog() == true)
            {
                return;
            }
        }
    }
}