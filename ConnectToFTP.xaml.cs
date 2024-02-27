using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ftpExample
{
    /// <summary>
    /// Логика взаимодействия для ConnectToFTP.xaml
    /// </summary>
    public partial class ConnectToFTP : Window
    {
        public ConnectToFTP()
        {
            InitializeComponent();
        }

        private void btnConnection_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public string IP
        {
            get { return txtIP.Text; }
        }

        public string Login
        {
            get { return txtLogin.Text; }
        }

        public string Password
        {
            get { return txtPass.Text; }
        }
    }
}