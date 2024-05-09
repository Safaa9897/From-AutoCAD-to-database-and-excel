using Cad_API_Project.DataContext;
using Cad_API_Project.Model;
using Cad_API_Project.ViewModel;
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

namespace Cad_API_Project.View
{
    /// <summary>
    /// Interaction logic for MainWindowView.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        public MainWindowView()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        #region Close Window

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            // Close the window
            Close();
        }

        #endregion



        #region Filter

        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Data_Grid.Items.Filter = FilterMethod;
        }
        private bool FilterMethod(object obj)
        {
            var _blocks = obj as BlockItem;
            return _blocks.Name.ToLower().Contains(FilterTextBox.Text.ToLower());
        }

        #endregion


        #region Load To database

        private void btnLoadBlocksNoAttr_Click(object sender, RoutedEventArgs e)
        {
            LoadDbViewModel _databaseload = new LoadDbViewModel();
            string _result = _databaseload.LoadBlocksNoAttributes();
            MessageBox.Show(_result);
        }

        private void btnLoadBlocksWithAttr_Click(object sender, RoutedEventArgs e)
        {
            LoadDbViewModel _databaseload = new LoadDbViewModel();
            string _result = _databaseload.LoadBlocksWithAttributes();
            MessageBox.Show(_result);
        }

        private void btnLoadLines_Click(object sender, RoutedEventArgs e)
        {
            LoadDbViewModel _databaseload = new LoadDbViewModel();
            string _result = _databaseload.LoadLines();
            MessageBox.Show(_result);
        }

        private void btnLoadMText_Click(object sender, RoutedEventArgs e)
        {
            LoadDbViewModel _databaseload = new LoadDbViewModel();
            string _result = _databaseload.LoadMTexts();
            MessageBox.Show(_result);
        }

        private void btnLoadPlines_Click(object sender, RoutedEventArgs e)
        {
            LoadDbViewModel _databaseload = new LoadDbViewModel();
            string _result = _databaseload.LoadPolylines();
            MessageBox.Show(_result);
        }

        #endregion

    }
}
