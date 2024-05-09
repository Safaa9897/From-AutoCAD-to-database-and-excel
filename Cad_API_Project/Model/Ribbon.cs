using Autodesk.AutoCAD.Runtime;
using Cad_API_Project.Command;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Autodesk.Windows;




namespace Cad_API_Project.Model
{
    public class Ribbon
    {
        public const string RibbonTitle = "Block Exporter";
        public const string RibbonId = "10 10";
        #region create ribbon

        public static void CreateRibbon()
        {


            RibbonControl ribbon = ComponentManager.Ribbon;
            if (ribbon != null)
            {
                RibbonTab rtab = ribbon.FindTab(RibbonId);
                if (rtab != null)
                {
                    ribbon.Tabs.Remove(rtab);
                }

                rtab = new RibbonTab();
                rtab.Title = RibbonTitle;
                rtab.Id = RibbonId;
                ribbon.Tabs.Add(rtab);
                Ribbon.AddContentToTab(rtab);


            }
        }

        #endregion


        #region create ribbon Tab

        private static void AddContentToTab(RibbonTab rtab)
        {
            rtab.Panels.Add(AddPanelOne());
        }

        #endregion


        #region create ribbon panel and button

        private static RibbonPanel AddPanelOne()
        {
            RibbonPanelSource rps = new RibbonPanelSource();
            rps.Title = "Blocks Count";
            RibbonPanel rp = new RibbonPanel();
            rp.Source = rps;
            RibbonButton rci = new RibbonButton();
            rci.Name = "ITI Addin";
            rps.DialogLauncher = rci;


            //create button1
            var addinAssembly = typeof(Ribbon).Assembly;
            RibbonButton btnPythonShell = new RibbonButton
            {
                Orientation = Orientation.Vertical,
                AllowInStatusBar = true,
                Size = RibbonItemSize.Large,
                Name = "Block Exporter",
                ShowText = true,
                Text = "Block Exporter",
                Description = "Displays all the blocks in the file and exports them to Database or Excel sheet",
                CommandHandler = new RelayCommand(new OpenMainWindow().Execute)
            };

            rps.Items.Add(btnPythonShell);

            return rp;
        }

        #endregion


        public static System.Windows.Media.ImageSource GetEmbeddedPng(System.Reflection.Assembly app, string imageName)
        {
            var file = app.GetManifestResourceStream(imageName);
            BitmapDecoder source = PngBitmapDecoder.Create(file, BitmapCreateOptions.None, BitmapCacheOption.None);
            return source.Frames[0];
        }
    }
}
