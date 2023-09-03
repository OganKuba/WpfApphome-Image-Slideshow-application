using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Forms;
using IOPath = System.IO.Path;
using WPF = System.Windows;
using System.Globalization;
using System.Reflection;

namespace WpfApphome
{
   
    public partial class MainWindow : Window
    {
        private List<ISlideshowEffect> _effects = new List<ISlideshowEffect>();
        public MainWindow()
            {
                InitializeComponent();
                //LoadPlugins();
                foreach (var drive in DriveInfo.GetDrives())
                {
                    if (drive.DriveType == DriveType.Fixed)
                    {
                        var item = new TreeViewItem
                        {
                            Header = drive.Name,
                            Tag = drive.Name
                        };

                        item.Items.Add(null);
                        item.Expanded += Folder_Expanded;
                        folderExplorer.Items.Add(item);
                    }
                }
            }
        private void StartSlideshow(ISlideshowEffect effect)
        {
            var imagePaths = imageList.Items.Cast<string>().ToList();
            if (imagePaths.Count == 0) return;

            var slideshowWindow = new SlideshowWindow(effect, imagePaths);
            this.IsEnabled = false;
            slideshowWindow.Closed += (sender, e) => this.IsEnabled = true;
            slideshowWindow.Show();
        }
        private void StartHorizontalSlideshow_Click(object sender, RoutedEventArgs e)
        {
            StartSlideshow(new HorizontalEffect());
        }

        private void StartVerticalSlideshow_Click(object sender, RoutedEventArgs e)
        {
            StartSlideshow(new VerticalEffect());
        }

        private void StartOpacitySlideshow_Click(object sender, RoutedEventArgs e)
        {
            StartSlideshow(new OpacityEffect());
        }
        private void StartSlideshow_Click(object sender, RoutedEventArgs e)
        {
            string effect = (slideshowEffectComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (effect == null)
            {
                

                return;
            }

            switch (effect.ToLower())
            {
                case "horizontal effect":
                    StartSlideshow(new HorizontalEffect());
                    break;

                case "vertical effect":
                    StartSlideshow(new VerticalEffect());
                    break;

                case "opacity effect":
                    StartSlideshow(new OpacityEffect());
                    break;

                default:
                    // Handle invalid selection or add default case
                    break;
            }
        }

       


        private void OpenFolder_Click(object sender, RoutedEventArgs e)
            {
                using (var dialog = new FolderBrowserDialog())
                {
                    var result = dialog.ShowDialog();

                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        string path = dialog.SelectedPath;
                        LoadImagesFromFolder(path);
                    }
                    else
                    {
                        fileInfoText.Text = "No file selected";

                    }
                }

            }
            private void LoadImagesFromFolder(string path)
            {

                imageList.Items.Clear();


                var imageFiles = Directory.EnumerateFiles(path, "*.jpg").Concat(Directory.EnumerateFiles(path, "*.png"));

                foreach (var file in imageFiles)
                {
                    imageList.Items.Add(file);
                }
            }

            private void Exit_Click(object sender, RoutedEventArgs e)
            {
                WPF.Application.Current.Shutdown();

            }

            private void About_Click(object sender, RoutedEventArgs e)
            {
                WPF.MessageBox.Show("This is simple slideshpow application");
            }
            private void Folder_Expanded(object sender, RoutedEventArgs e)
            {
                var item = (TreeViewItem)sender;



                if (item.Items.Count == 1 && item.Items[0] == null)
                {
                    item.Items.Clear();
                    try
                    {
                        var path = (string)item.Tag;
                        var dirs = Directory.GetDirectories(path);
                        foreach (var dir in dirs)
                        {
                            var subitem = new TreeViewItem
                            {
                                Header = System.IO.Path.GetFileName(dir),
                                Tag = dir
                            };


                            subitem.Items.Add(null);
                            subitem.Expanded += Folder_Expanded;
                            item.Items.Add(subitem);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            private void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
            {
                imageList.Items.Clear();
                var treeViewItem = folderExplorer.SelectedItem as TreeViewItem;
                if (treeViewItem != null)
                {
                    var path = (string)treeViewItem.Tag;
                    try
                    {
                        var imageFiles = Directory.EnumerateFiles(path, "*.jpg");
                        foreach (var file in imageFiles)
                        {
                            imageList.Items.Add(file);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            private void OnImageListSelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                if (imageList.SelectedItem != null)
                {
                    var path = imageList.SelectedItem.ToString();

                    var fileInfo = new FileInfo(path);
                    var sizeKB = fileInfo.Length / 1024;

                    var image = new BitmapImage(new Uri(path));
                    var width = image.PixelWidth;
                    var height = image.PixelHeight;

                    fileInfoText.Text = $"Name: {fileInfo.Name}\nWidth: {width}\nHeight: {height}\nSize: {sizeKB} KB";

                }
                else
                {
                    fileInfoText.Text = "No file selected";

                }
            }
        private void LoadPlugins()
        {
            string pluginDirectory = IOPath.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");

            foreach (string dllFile in Directory.GetFiles(pluginDirectory, "*.dll"))
            {
                var assembly = Assembly.LoadFrom(dllFile);

                foreach (Type type in assembly.GetTypes())
                {
                    if (typeof(ISlideshowEffect).IsAssignableFrom(type) && !type.IsInterface)
                    {
                        var effect = Activator.CreateInstance(type) as ISlideshowEffect;

                        if (effect != null)
                        {
                            _effects.Add(effect);
                        }
                    }
                }
            }
        }




    }
}
