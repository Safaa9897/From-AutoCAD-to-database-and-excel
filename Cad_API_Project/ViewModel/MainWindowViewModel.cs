using Cad_API_Project.Command;
using OfficeOpenXml;
using System;
using System.IO;
using System.Windows.Input;
using System.Windows;
using System.Linq;
using System.Data;


namespace Cad_API_Project.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {

        #region To Excel

        private ICommand _addBlockToExcelCommand;
        public ICommand AddBlockToExcelCommand
        {
            get
            {
                return _addBlockToExcelCommand ?? (_addBlockToExcelCommand = new RelayCommand(AddBlockToExcel));
            }
        }
        

        private void AddBlockToExcel(object parameter)
        {
            try
            {
                ExportToExcel();
                MessageBox.Show("Data exported to Excel successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ExportToExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Create a SaveFileDialog
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*",
                DefaultExt = "xlsx",
                Title = "Save Excel File",
            };

            // Show the SaveFileDialog and get the selected file path
            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                var excelFile = new FileInfo(saveFileDialog.FileName);
                var excelPackage = new ExcelPackage();
                var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

                // Header row
                worksheet.Cells[1, 1].Value = "Name";
                worksheet.Cells[1, 2].Value = "Description";
                worksheet.Cells[1, 3].Value = "BlockCount";

                // Data rows
                int row = 2;
                foreach (var block in AllBlocks.Where(b => b.IsChecked)) // Only export checked items
                {
                    worksheet.Cells[row, 1].Value = block.Name;
                    worksheet.Cells[row, 2].Value = block.Description;
                    worksheet.Cells[row, 3].Value = block.BlockCount;
                    row++;
                }

                // Save the Excel file
                excelPackage.SaveAs(excelFile);
            }
        }

        #endregion

        private bool isChecked;
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                OnPropertyChanged(nameof(IsChecked));
            }
        }
        public MainWindowViewModel()
        {
           
        }

        

    }
}
