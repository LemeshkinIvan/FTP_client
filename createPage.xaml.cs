using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ftpExample
{
    /// <summary>
    /// Логика взаимодействия для createPage.xaml
    /// </summary>
    public partial class createPage : Window
    {
        public string FileName
        {
            get { return txtName.Text; }
        }
        public createPage()
        {
            InitializeComponent();
            this.Title = "Дайте имя";
        }

        private void btnSetNameFile_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}