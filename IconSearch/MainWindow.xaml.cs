using System;
using System.Collections;
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

namespace IconSearch
{
    public partial class MainWindow : Window
    {
        public string FilePath { get; set; } = @"E:\活动图标";
        public string Extension { get; set; } = @".jpg";
        public List<string> IconNameList { get; set; }

        public MainWindow()
        {
           InitializeComponent();
            if (Directory.Exists(FilePath))
            {
                IconNameList = GetIconName(FilePath);
            }
            else
            {
                IconId.IsReadOnly = true;
                MessageBox.Show($"\"{FilePath}\"未找到，请确认路径！");
            }
           
        }

        private void IconId_TextChanged(object sender, TextChangedEventArgs e)
        {

            //当查找到图片时
            if (((IList)IconNameList).Contains(IconId.Text))
            {
                IconPath.Text = GetIconPath(Convert.ToInt32(IconId.Text));
                string soursePath = FilePath + "\\" + IconId.Text + Extension;
                BitmapImage image = new BitmapImage(new Uri(soursePath, UriKind.Absolute));
                IconImage.Source = image;
            }

            var textBox = sender as TextBox;

            TextChange[] change = new TextChange[e.Changes.Count];
            e.Changes.CopyTo(change, 0);

            int offset = change[0].Offset;
            if (change[0].AddedLength <= 0) return;
            int num = 0;
            if (textBox == null || int.TryParse(textBox.Text, out num)) return;
            textBox.Text = textBox.Text.Remove(offset, change[0].AddedLength);
            textBox.Select(offset, 0);
        }

        //验证输入为数字
        private void IconId_KeyDown(object sender, KeyEventArgs e)
        {
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)))
            {
                e.Handled = true;
            }
        }

        public string GetIconPath(int iconId)
        {
            int itemicon = (int) ((iconId - 1000) / 16);
            return $"set:itemicon{itemicon} image:{iconId}";
        }

        public List<string> GetIconName(string filePath)
        {
            var files = Directory.GetFiles(FilePath, Extension);
            return files.Select(System.IO.Path.GetFileNameWithoutExtension).ToList();
        }
    }
}