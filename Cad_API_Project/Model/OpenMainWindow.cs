using Cad_API_Project.Command;
using Cad_API_Project.DataContext;
using Cad_API_Project.View;

namespace Cad_API_Project.Model
{
    public class OpenMainWindow : ICadCommand
    {
        public override void Execute()
        {
            //OpenWindow();
            AutoCadCommands _autoCadCommands = new AutoCadCommands();
            _autoCadCommands.Filter();

        }
        public void OpenWindow()
        {
            // Create an instance of the MainWindowView
            MainWindowView _mainWindow = new MainWindowView();

            // Show the window
            _mainWindow.Show();

            //AutoCadCommands _autoCadCommands = new AutoCadCommands();
            //_autoCadCommands.Filter();
        }

    }
}
