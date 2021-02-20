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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Squirrel;


namespace SiegeSkinSwitcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SolidColorBrush Off = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        SolidColorBrush On = new SolidColorBrush(Color.FromRgb(240, 222, 45));

        //settings
        bool status = false;
        String UbiFolder = @"";
        string docPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SkinSwitcher");
        public MainWindow()
        {
            InitializeComponent();
            Directory.CreateDirectory(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SkinSwitcher"));
            Directory.CreateDirectory(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"SkinSwitcher\normal" ));
            Directory.CreateDirectory(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"SkinSwitcher\esl"));
            SettingsLoad();
        }
        private void Bu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Bu.Toggled1 == true)
            {
                status = true;
            }
            else
            {
                status = false;
            }

        }

        public void SettingsSave()
        {
            string[] lines = { UbiFolder, status + ""};
            using (StreamWriter outputFile = new StreamWriter(System.IO.Path.Combine(docPath, "Skinswitcher.txt")))
            {
                foreach (string line in lines)
                    outputFile.WriteLine(line);
            }
        }
        public void SettingsLoad()
        {
            if (File.Exists(System.IO.Path.Combine(docPath, "Skinswitcher.txt")))
            {
                string[] lines = System.IO.File.ReadAllLines(System.IO.Path.Combine(docPath, "Skinswitcher.txt"));
                UbiFolder = lines[0];
                status = Boolean.Parse(lines[1]);
                Bu.setStatus(status);
                
            }
            else
            { 
                SettingsCreate();
            }
        }
        public void SettingsCreate()
        {
            string[] lines = { @"Ubipath", "false"};
            
            using (StreamWriter outputFile = new StreamWriter(System.IO.Path.Combine(docPath, "Skinswitcher.txt")))
            {
                foreach (string line in lines)
                    outputFile.WriteLine(line);
            }
            SettingsLoad();
        }

       
        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {

            DirectoryInfo dir1 = new DirectoryInfo(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"SkinSwitcher\esl"));
            DirectoryInfo dir2 = new DirectoryInfo(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"SkinSwitcher\normal"));
            DirectoryInfo ubidir = new DirectoryInfo(UbiFolder);
            SettingsSave();

            if (status)
            {
                UbiClear();
                CopyFiles(dir1, ubidir);

            }
            else {
                UbiClear();
                CopyFiles(dir2, ubidir);

            }

        }

        private void UbiClear()
        {
            DirectoryInfo directory = new DirectoryInfo(UbiFolder);

            foreach (FileInfo file in directory.GetFiles())
            {
                file.Delete();
            }

            foreach (DirectoryInfo dir in directory.GetDirectories())
            {
                dir.Delete(true);
            }
        }
        public static void CopyFiles(DirectoryInfo source, DirectoryInfo target)
        {
            foreach (DirectoryInfo dir in source.GetDirectories())
                CopyFiles(dir, target.CreateSubdirectory(dir.Name));
            foreach (FileInfo file in source.GetFiles())
                file.CopyTo(System.IO.Path.Combine(target.FullName, file.Name));
        }


        private void ubiFolder_Click(object sender, RoutedEventArgs e)
        {
            SettingsLoad();
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = @"C:\Program Files (x86)\Ubisoft\Ubisoft Game Launcher\savegames" ;
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                UbiFolder = dialog.FileName;

            }
            SettingsSave();
        }

        private void normalConfig_Click(object sender, RoutedEventArgs e)
        {
            DirectoryInfo dir1 = new DirectoryInfo(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"SkinSwitcher\esl"));
            DirectoryInfo dir2 = new DirectoryInfo(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"SkinSwitcher\normal"));
            DirectoryInfo ubidir = new DirectoryInfo(UbiFolder);
            CopyFiles(ubidir, dir2);
        }

        private void ELSConfig_Click(object sender, RoutedEventArgs e)
        {

            DirectoryInfo dir1 = new DirectoryInfo(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"SkinSwitcher\esl"));
            DirectoryInfo dir2 = new DirectoryInfo(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"SkinSwitcher\normal"));
            DirectoryInfo ubidir = new DirectoryInfo(UbiFolder);
            CopyFiles(ubidir, dir1);
        }
    }
}
