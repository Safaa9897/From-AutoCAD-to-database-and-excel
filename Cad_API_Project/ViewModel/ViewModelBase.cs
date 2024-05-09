using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Cad_API_Project.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace Cad_API_Project.ViewModel
{
    public class ViewModelBase :  INotifyPropertyChanged
    {
        public static ViewModelBase Instance { get; } = new ViewModelBase();



        #region NotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        #endregion



        public static int BlockInstanceCount { get;  set; }



        public static ObservableCollection<BlockItem> AllBlocks { get; set; }



        public BlockItem BlockReftoBlockItem(BlockReference blockRef)
        {
            if (blockRef == null)
            {
                // Handle the case where blockRef is null
                return default;
            }

            BlockItem item = new BlockItem
            {
                IsChecked = true,
                Name = blockRef.Name,
                Description = GetAttribute(blockRef),
                BlockCount = 0  // Set BlockCount to 0 initially
            };

            return item;
        }


        private string GetAttribute(BlockReference blk)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            string attributes = "";
            foreach (ObjectId attRefId in blk.AttributeCollection)
            {
                using (Transaction trans = doc.TransactionManager.StartTransaction())
                {
                    DBObject obj = trans.GetObject(attRefId, OpenMode.ForRead);
                    AttributeReference attRef = obj as AttributeReference;
                    if (attRef != null)
                    {
                        //attributes += attRef.TextString + ",";
                        //attributes += attRef.Tag + "=" + attRef.TextString + "|" + "Rotation=" + attRef.Rotation + ",";
                        attributes += attRef.Tag + "=" + attRef.TextString + ",";
                    }
                }
                attributes = attributes.Substring(0, attributes.Length - 1);
            }
            return attributes;
        }
    }
}

