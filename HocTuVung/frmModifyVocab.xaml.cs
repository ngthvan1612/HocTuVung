using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace HocTuVung
{
    using ViewModels;
    using Models;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Interaction logic for frmModifyVocab.xaml
    /// </summary>
    public partial class frmModifyVocab : Window
    {
        public ObservableCollection<Unit> ListUnit = new ObservableCollection<Unit>();
        public ObservableCollection<Vocab> ListVocab = new ObservableCollection<Vocab>();

        public frmModifyVocab()
        {
            InitializeComponent();
            this.listUnit.ItemsSource = ListUnit;
            this.listVocab.ItemsSource = ListVocab;
            this.RefreshListUnit();
        }

        private void listUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.RefreshListVocab();
        }

        private void listVocab_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.UpdateColumnSize(this.listVocab);
        }

        private void listVocab_Loaded(object sender, RoutedEventArgs e)
        {
            this.UpdateColumnSize(this.listVocab);
        }

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            if (listUnit.SelectedItem != null)
            {
                VocabViewModel viewModel = new VocabViewModel();
                Unit unit = listUnit.SelectedItem as Unit;
                dialogAddVocab dlg = new dialogAddVocab();
                dlg.Owner = this;
                if (dlg.ShowDialog() == true)
                {
                    var result = viewModel.Create(unit.Id, dlg.English, dlg.Vietnam);
                    if (result is null)
                    {
                        this.RefreshListVocab();
                    }
                    else
                    {
                        MessageBox.Show(result.ToString(), "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void btnSua_Click(object sender, RoutedEventArgs e)
        {
            if (listUnit.SelectedItem != null && listVocab.SelectedItem != null)
            {
                VocabViewModel viewModel = new VocabViewModel();
                Unit unit = listUnit.SelectedItem as Unit;
                Vocab vocab = listVocab.SelectedItem as Vocab;
                dialogEditVocab dlg = new dialogEditVocab(vocab.English, vocab.Vietnam);
                dlg.Owner = this;
                if (dlg.ShowDialog() == true)
                {
                    var result = viewModel.Edit(vocab.Id, dlg.English, dlg.Vietnam);
                    if (result is null)
                    {
                        this.RefreshListVocab();
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
            if (listUnit.SelectedItem != null && listVocab.SelectedItem != null)
            {
                Vocab vocab = listVocab.SelectedItem as Vocab;
                if (MessageBox.Show("Bạn có chắn chắn muốn xóa?", "Cảnh báo", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    var viewModel = new VocabViewModel();
                    var result = viewModel.Delete(vocab.Id);
                    if (result is null)
                    {
                        this.RefreshListVocab();
                    }
                    else
                    {
                        MessageBox.Show(result.ToString(), "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
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

        private void RefreshListVocab()
        {
            if (listUnit.SelectedItem != null)
            {
                VocabViewModel viewModel = new VocabViewModel();
                Unit unit = listUnit.SelectedItem as Unit;
                ListVocab.Clear();
                var vocabs = viewModel.GetList(unit.Id);
                for (int i = 0; i < vocabs.Count; ++i)
                {
                    vocabs[i].STT = i + 1;
                    ListVocab.Add(vocabs[i]);
                }
            }
        }
    }
}
