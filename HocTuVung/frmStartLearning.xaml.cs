using HocTuVung.Models;
using HocTuVung.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for frmStartLearning.xaml
    /// </summary>
    public partial class frmStartLearning : Window
    {
        public ObservableCollection<Unit> ListUnit = new ObservableCollection<Unit>();

        public int Repeat { get; set; }
        public int SelectedUnitId { get; set; }

        public frmStartLearning()
        {
            InitializeComponent();
            this.listUnit.ItemsSource = ListUnit;
            this.RefreshListUnit();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            int repeat;
            if (listUnit.SelectedItem is null)
            {
                MessageBox.Show("Bạn phải chọn một unit", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (!int.TryParse(tbRepeat.Text, out repeat))
                {
                    MessageBox.Show("Số lần lặp lại phải là số!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (repeat <= 0)
                    {
                        MessageBox.Show("Số lần lặp lại phải lớn hơn 0", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        this.Repeat = repeat;
                        this.SelectedUnitId = (listUnit.SelectedItem as Unit).Id;
                        this.DialogResult = true;
                    }
                }
            }
        }

        private void RefreshListUnit()
        {
            ListUnit.Clear();
            var viewModel = new UnitViewModel();
            var units = viewModel.GetList();
            for (int i = 0; i < units.Count; ++i)
            {
                ListUnit.Add(units[i]);
            }
            this.listUnit.SelectedIndex = 0;
        }
    }
}
