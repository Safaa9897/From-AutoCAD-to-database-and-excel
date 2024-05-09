using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System.Collections.Generic;
using Cad_API_Project.ViewModel;
using Cad_API_Project.Model;
using System.Windows;
using Cad_API_Project.View;

namespace Cad_API_Project.DataContext
{
    public class AutoCadCommands : ViewModelBase
    {

        #region Filter Method

        [CommandMethod("BlockExpoterAPI")]
        public void Filter()
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor editor = doc.Editor;

            using (Transaction tr = doc.TransactionManager.StartTransaction())
            {
                try
                {
                    TypedValue[] typedValue = new TypedValue[1];
                    typedValue[0] = new TypedValue((int)DxfCode.Start, "INSERT");
                    SelectionFilter filter = new SelectionFilter(typedValue);
                    PromptSelectionResult psr = editor.SelectAll(filter);

                    if (psr.Status == PromptStatus.OK)
                    {
                        BlockReference blockRef;
                        SelectionSet selectionSet = psr.Value;

                        if (selectionSet != null)
                        {
                            var allblocksList = new List<BlockItem>();
                            var allblocksDict = new Dictionary<string, int>();

                            foreach (SelectedObject selectedObject in selectionSet)
                            {
                                blockRef = tr.GetObject(selectedObject.ObjectId, OpenMode.ForRead) as BlockReference;

                                if (!(blockRef.Name.Contains("*")))
                                {
                                    BlockItem item = new BlockItem();
                                    item = BlockReftoBlockItem(blockRef);

                                    if (allblocksDict.ContainsKey(item.Name))
                                    {
                                        allblocksDict[item.Name]++;
                                    }
                                    else
                                    {
                                        allblocksDict.Add(item.Name, 1);
                                        allblocksList.Add(item);
                                    }
                                }
                            }
                            foreach (var item in allblocksList)
                            {
                                item.BlockCount = allblocksDict[item.Name];
                            }

                            ViewModelBase.AllBlocks = new System.Collections.ObjectModel.ObservableCollection<BlockItem>(allblocksList);
                            // Call CreateRibbon method to create the ribbon
                            Ribbon.CreateRibbon();
                            //Opens the View
                            OpenWindow();

                        }
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        public void OpenWindow()
        {
            MainWindowView mainWindow = new MainWindowView();
            Autodesk.AutoCAD.ApplicationServices.Application.ShowModalWindow(mainWindow);
        }

        #endregion

    }
}

