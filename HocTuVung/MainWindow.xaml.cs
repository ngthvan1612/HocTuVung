using System.Windows;
using System.ComponentModel;
using System.Windows.Data;
using System;
using System.Windows.Media;

namespace HocTuVung
{
    using HocTuVung.ViewModels;
    using Learning;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private SchedulingAlgorithm learningAlgorithm = new SchedulingAlgorithm();

        private int WRONG = 0;

        private bool _isLearning = false;
        public bool IsLearning
        {
            get { return _isLearning; }
            set
            {
                _isLearning = value;
                if (value)
                {
                    btnBatDau.Background = Brushes.Gray;
                    btnDungLai.Background = Brushes.DodgerBlue;
                }
                else
                {
                    btnBatDau.Background = Brushes.DodgerBlue;
                    btnDungLai.Background = Brushes.Gray;
                }
                this.UpdateLayout();
                btnBatDau.IsEnabled = btnDungLai.IsEnabled = true;
                btnBatDau.IsEnabled = !value;
                btnDungLai.IsEnabled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsLearning"));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            new Models.TuVungContext();
            IsLearning = false;
            this.learningAlgorithm.LearningType = LearningType.VieToEng;
        }

        private void btnListUnit_Click(object sender, RoutedEventArgs e)
        {
            frmModifyUnit frm = new frmModifyUnit();
            frm.Owner = this;
            frm.ShowDialog();
        }

        private void btnBatDau_Click(object sender, RoutedEventArgs e)
        {
            frmStartLearning frm = new frmStartLearning();
            frm.Owner = this;
            if (frm.ShowDialog() == true)
            {
                var viewModel = new VocabViewModel();
                var vocabs = viewModel.GetList(frm.SelectedUnitId);
                if (vocabs.Count == 0)
                {
                    MessageBox.Show("Không có từ vựng nào trong unit này!");
                    return;
                }
                learningAlgorithm.Reset();
                learningAlgorithm.BuildQueue(vocabs, frm.Repeat);
                IsLearning = true;
                tbQuestion.Text = string.Format("Question: {0}\n", learningAlgorithm.GetPercent()) + learningAlgorithm.CurrentQuestion();
                WRONG = 0;
            }
        }

        private void btnDungLai_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn dừng lại", "Cảnh báo", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                learningAlgorithm.Reset();
                IsLearning = false;
                WRONG = 0;
            }
        }

        private void btnListVocab_Click(object sender, RoutedEventArgs e)
        {
            frmModifyVocab frm = new frmModifyVocab();
            frm.Owner = this;
            frm.ShowDialog();
        }

        private void tbAnswer_KeyDown(object sender, KeyEventArgs e)
        {
            if (IsLearning)
            {
                if (e.Key == Key.Enter)
                {
                    if (learningAlgorithm.CheckAnswer(tbAnswer.Text))
                    {
                        learningAlgorithm.PlayAudio();
                        learningAlgorithm.Next();
                        tbAnswer.Text = "";
                        tbQuestion.Text = string.Format("Question: {0}\n", learningAlgorithm.GetPercent()) + learningAlgorithm.CurrentQuestion();
                        WRONG = 0;
                    }
                    else
                    {
                        MessageBox.Show("Sai!");
                        WRONG++;
                        if (WRONG >= 3)
                        {
                            MessageBox.Show("Câu trả lời là:\n" + learningAlgorithm.CurrentAnswer());
                        }
                    }
                }
            }
        }
    }
}
