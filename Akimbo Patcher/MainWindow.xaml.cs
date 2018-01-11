using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Akimbo_Patcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string Version = "0.1.0";
        public MainWindow()
        {
            InitializeComponent();
            // Add Arm Types
            ArmTypes.Items.Add("Left");
            ArmTypes.Items.Add("Right");
            ArmTypes.SelectedIndex = 0;
            // Set Title
            Title = string.Format("IW Akimbo Patcher {0}", Version);
        }

        private void DropBox_Drop(object sender, DragEventArgs e)
        {
            // Check if user dropped files
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Get Files
                string[] dropped = (string[])e.Data.GetData(DataFormats.FileDrop);
                // SEAnim Files
                List<string> seanimFiles = new List<string>();
                // Loop through dropped files to get SEAnims
                foreach(string drop in dropped)
                    // Check extension
                    if(System.IO.Path.GetExtension(drop) == ".seanim")
                        // Add it if it is a SEAnim
                        seanimFiles.Add(drop);
                // Check if we have any processable anims
                if(seanimFiles.Count > 0)
                {
                    // New folder browser
                    using (var dialog = new CommonOpenFileDialog())
                    {
                        // Set to folder
                        dialog.IsFolderPicker = true;
                        // Check result
                        if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                            // Process
                            AnimPatcher.ProcessAkimboSEAnims(seanimFiles.ToArray(), dialog.FileName, ArmTypes.SelectedItem.ToString());
                    }
                }
                // Return if none.
                else
                {
                    MessageBox.Show("No valid animation files dropped.");
                    return;
                }
            }
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            string about = string.Format("IW Akimbo Patcher {0} by Scobalula\nSELibDotNET by DTZxPorter", Version);
            MessageBox.Show(about, "About", MessageBoxButton.OK);
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            // SEAnim Files
            List<string> seanimFiles = new List<string>();
            // New Dialog
            var fileDialog = new OpenFileDialog();
            // Filters
            fileDialog.Filter = "SEAnim files (*.seanim)|*.seanim";
            // Title
            fileDialog.Title = "Select SEAnim files to patch";
            // Set Multiple Selection
            fileDialog.Multiselect = true;
            // Open
            if(fileDialog.ShowDialog() == true)
            {
                // Loop through selected
                foreach(string file in fileDialog.FileNames)
                {
                    // Check extension
                    if (System.IO.Path.GetExtension(file) == ".seanim")
                        // Add it if it is a SEAnim
                        seanimFiles.Add(file);
                }
            }
            else
            {
                return;
            }
            // Check if we have any processable anims
            if (seanimFiles.Count > 0)
            {
                // New folder browser
                using (var dialog = new CommonOpenFileDialog())
                {
                    // Set to folder
                    dialog.IsFolderPicker = true;
                    // Check result
                    if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                        // Process
                        AnimPatcher.ProcessAkimboSEAnims(seanimFiles.ToArray(), dialog.FileName, ArmTypes.SelectedItem.ToString());
                }
            }
            // Return if none.
            else
            {
                MessageBox.Show("No valid animation files selected.");
                return;
            }
        }
    }
}
