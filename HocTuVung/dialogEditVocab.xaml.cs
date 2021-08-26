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
    /// Interaction logic for dialogEditVocab.xaml
    /// </summary>
    public partial class dialogEditVocab : Window
    {
        public string English { get; set; }
        public string Vietnam { get; set; }

        public dialogEditVocab(string eng, string vie)
        {
            InitializeComponent();
            this.English = eng;
            this.Vietnam = vie;
            this.tbEnglish.Text = eng;
            this.tbVietnam.Text = vie;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            var eng = tbEnglish.Text.Trim();
            var vie = tbVietnam.Text.Trim();
            if (eng.Length == 0 || vie.Length == 0)
            {
                MessageBox.Show("Tên không được để trống", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (eng != this.English || vie != this.Vietnam)
                {
                    this.English = eng;
                    this.Vietnam = vie;
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
