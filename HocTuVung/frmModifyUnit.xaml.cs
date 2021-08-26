using System;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace HocTuVung
{
    using ViewModels;
    using System.Collections.ObjectModel;
    using HocTuVung.Models;

    /// <summary>
    /// Interaction logic for frmModifyUnit.xaml
    /// </summary>
    public partial class frmModifyUnit : Window
    {

        public ObservableCollection<Unit> ListUnits = new ObservableCollection<Unit>();

        public frmModifyUnit()
        {
            InitializeComponent();
            this.listUnit.ItemsSource = ListUnits;
            this.RefreshList();
        }

        private void listUnit_Loaded(object sender, RoutedEventArgs e)
        {
            this.UpdateColumnSize(listUnit);
        }

        private void listUnit_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.UpdateColumnSize(listUnit);
        }

        private void UpdateColumnSize(ListView listView)
        {
            int autoFillColumnIndex = (listView.View as GridView).Columns.Count - 1;
            if (listView.ActualWidth == Double.NaN)
                listView.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            double remainingSpace = listView.ActualWidth;
            for (int i = 0; i < (listView.View as GridView).Columns.Count; i++)
                if (i != autoFillColumnIndex)
                    remainingSpace -= (listView.View as GridView).Columns[i].ActualWidth;
            remainingSpace = Math.Max(100, remainingSpace - 0);
            (listView.View as GridView).Columns[autoFillColumnIndex].Width = remainingSpace >= 0 ? remainingSpace : 0;
        }

        private void RefreshList()
        {
            ListUnits.Clear();
            var units = new UnitViewModel().GetList();
            for (int i = 0; i < units.Count; ++i)
            {
                units[i].STT = i + 1;
                ListUnits.Add(units[i]);
            }
        }

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            dialogAdd dlg = new dialogAdd();
            dlg.Owner = this;
            if (dlg.ShowDialog() == true)
            {
                var viewModel = new UnitViewModel();
                var result = viewModel.Create(dlg.Data);
                if (!(result is null))
                {
                    MessageBox.Show(result.ToString(), "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    this.RefreshList();
                }
            }
        }

        private void btnSua_Click(object sender, RoutedEventArgs e)
        {
            if (listUnit.SelectedItems.Count == 1)
            {
                Unit unit = listUnit.SelectedItem as Unit;
                dialogEdit dlg = new dialogEdit(unit.Name);
                dlg.Owner = this;
                if (dlg.ShowDialog() == true)
                {
                    var viewModel = new UnitViewModel();
                    var result = viewModel.Update(unit.Id, dlg.Data);
                    if (result is null)
                    {
                        this.RefreshList();
                    }
                    else
                    {
                        MessageBox.Show(result.ToString(), "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (listUnit.SelectedItems.Count == 1)
            {
                Unit unit = listUnit.SelectedItem as Unit;
                if (MessageBox.Show("Bạn có chắn chắn muốn xóa, các dữ liệu liên quan đều sẽ bị xóa theo!", "Cảnh báo", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    var viewModel = new UnitViewModel();
                    var result = viewModel.Delete(unit.Id);
                    if (result is null)
                    {
                        this.RefreshList();
                    }
                    else
                    {
                        MessageBox.Show(result.ToString(), "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
