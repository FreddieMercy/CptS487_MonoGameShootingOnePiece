using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.ComponentModel;

namespace KmapInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow(string _path)
        {
            WindowState = WindowState.Minimized;
            path = _path;
            InitializeComponent();
            #region Load Json
            try
            {
                dict = Extensions.FromJsonToDictionary(File.ReadAllText(path));
            }
            catch (Exception)
            {
                dict = new Dictionary<string, string>();
            }
            #endregion
            #region set up grid
            List<string> items = item.ToList<string>();
            List<string> tracker = new List<string>();
            foreach (string i in dict.Keys)
            {
                if (!tracker.Contains(i))
                {
                    tracker.Add(i);
                    tbs.Add(setUpRow(i));
                    items.Remove(i);
                }
            }

            foreach (string i in items)
            {
                tbs.Add(setUpRow(i));
                dict[i] = "";
            }

            RowCount = tbs.Count;

            foreach (KeyValuePair<TextBlock, TextBox> k in tbs)
            {
                KeyBindings.Children.Add(k.Key);
                KeyBindings.Children.Add(k.Value);
            }
            #endregion
            #region Set Up QWERTY 
            //Add the initial buttons in "BeckerBoxUI.xaml"

            SettingsForKeyboard();

            //sort all the keys above to their locations as specified
            SortKeys(); //QWERTY

            SetItemssourceOfKeyboardKeys(); //QWERTY

            SizeChanged += setUpAllKeysStyles_Normal;   //QWERTY
            setUpAllKeysStyles_Normal();
            #endregion
        }

        private void keyBinding(object sender, TextChangedEventArgs e)
        {
            TextBox t = sender as TextBox;

            if (dict.ContainsKey(t.Tag.ToString()))
            {
                dict[t.Tag.ToString()] = t.Text;
            }
        }

        private void Submit(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(path, Extensions.FromDictionaryToJson(dict));
            this.OnPropertyChanged("Submit");
            this.Close();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Keys.Clear();
        }

        private void _Clear_Click(object sender, RoutedEventArgs e)
        {
            foreach (TextBox tb in getAllControlsByType.FindVisualChildren<TextBox>(KmapUI))
            {
                tb.Text = "";
            }
        }

        private void _Shift_Click(object sender, RoutedEventArgs e)
        {
            tmpBtnCollection.Clear();
            SetItemssourceOfKeyboardKeys();
            Dispatcher.BeginInvoke((Action)(() =>
            {
                CollectionAlltheButtonsInTheView();

            }), System.Windows.Threading.DispatcherPriority.Input);
        }

        private void SelectTextBox(object sender, EventArgs e)
        {
            buffer = sender as TextBox;
        }
        private void UnSelectTextBox(object sender, EventArgs e)
        {
            buffer = null;
        }
        private void Button_Click(object sender = null, EventArgs e = null)
        {
            if (buffer != null)
            {
                buffer.Text = (sender as Button).Content.ToString();
            }
        }

        private void Space_Button_Click(object sender, RoutedEventArgs e)
        {
            if (buffer != null)
            {
                buffer.Text = " ";
            }
        }

        private void KmapUI_Loaded(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }
    }
}