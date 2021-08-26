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
    /// Interaction logic for dialogEdit.xaml
    /// </summary>
    public partial class dialogEdit : Window
    {
        public string Data { get; set; } = "";

        public dialogEdit(string text)
        {
            InitializeComponent();
            this.tbInput.Text = text;
            this.Data = text;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            var tmp = tbInput.Text.Trim();
            if (tmp.Length == 0)
            {
                MessageBox.Show("Tên không được để trống", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (Data != tmp)
                {
                    Data = tmp;
                    this.DialogResult = true;
                }
                else
                {
                    this.DialogResult = false;
                }
            }
        }
    }
}
