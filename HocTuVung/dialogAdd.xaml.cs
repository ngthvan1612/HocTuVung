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
    /// Interaction logic for dialogAdd.xaml
    /// </summary>
    public partial class dialogAdd : Window
    {
        public string Data { get; set; } = "";

        public dialogAdd()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Data = tbInput.Text.Trim();
            if (this.Data.Length == 0)
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
