using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HocTuVung
{
    /// <summary>
    /// Interaction logic for dialogAddVocab.xaml
    /// </summary>
    public partial class dialogAddVocab : Window
    {
        public string English { get; set; } = "";
        public string Vietnam { get; set; } = "";

        public dialogAddVocab()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.English = tbEnglish.Text.Trim();
            this.Vietnam = tbVietnam.Text.Trim();
            if (this.English.Length == 0 || this.Vietnam.Length == 0)
            {
                MessageBox.Show("Tên không được để trống", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                this.DialogResult = true;
            }
        }
    }
}
