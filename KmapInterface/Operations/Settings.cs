using System.ComponentModel;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Collections;
using System;
using System.Linq;
using System.Windows.Data;

namespace KmapInterface
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region XAML attributes Field

        public event PropertyChangedEventHandler PropertyChanged;
        //private int height = 24;
        private int width = 100;
        public string _margin_right;

        public int _margin = 0;
        public int margin
        {
            get { return _margin; }
            set
            {
                _margin = value;
                //Notify the binding that the value has changed.
                this.OnPropertyChanged("margin");
            }
        }

        public string margin_right
        {
            get { return _margin_right; }
            set
            {
                _margin_right = value;
                this.OnPropertyChanged("margin_right");
            }
        }
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public int _r = 0;
        public int RowCount
        {
            get { return _r; }
            set
            {
                _r = value;
                //Notify the binding that the value has changed.
                this.OnPropertyChanged("RowCount");
            }
        }
        #endregion
        private string[] item = { "Up", "Down", "Left", "Right", "Shoot" };
        private Dictionary<string, string> dict;
        private string path = "";
        private List<KeyValuePair<TextBlock, TextBox>> tbs = new List<KeyValuePair<TextBlock, TextBox>>();// new List<TextBlock>();// Dictionary<string,TextBlock>();

        private Hashtable table = new Hashtable(); //save all the sorted keys based on their row and col indexes.
        private Keys k; //it saves all the key initiated, but before sorting
        private TextBox buffer = null;
        private List<ItemsControl> keyboard_Rows = new List<ItemsControl>(); //add each row of keyboard which contains the keys, in sequence
        
        private List<ControlsXYandWidthHeight> tmpBtnCollection = new List<ControlsXYandWidthHeight>();

        private KeyValuePair<TextBlock, TextBox> setUpRow(string i)
        {
            TextBox tmp = new TextBox() { Text = ((dict.ContainsKey(i)) ? dict[i] : ""), Tag = i, Width = width, /*Margin = new Thickness(0, height * tbs.Count, 0, 0), */HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Stretch };
            tmp.SetValue(Grid.ColumnProperty, 1);
            tmp.SetValue(Grid.RowProperty, tbs.Count);
            tmp.TextChanged += keyBinding;
            tmp.GotFocus += SelectTextBox;
            tmp.LostFocus += UnSelectTextBox;
            TextBlock tb = new TextBlock() { Text = i, Width = width,/* Margin = new Thickness(0, height * tbs.Count, 0, 0), */HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Stretch };
            tb.SetValue(Grid.RowProperty, tbs.Count);
            tb.SetValue(Grid.ColumnProperty, 0);

            return new KeyValuePair<TextBlock, TextBox>(key: tb, value: tmp);
        }

        private void SettingsForKeyboard()
        {
            margin = 10; //set the margin / height in xaml, for the purpose of keeping it consist
            otherMarginInit(); //Only reason why I put all those int method is for saving space. Don't need to care if "margin" exists. The definition is in "Methods.cs"
            AddRows();
            AddKeys();

        }

        private void otherMarginInit()
        {
            //Don't need to care if "margin" exists
            margin_right = "0,0," + margin + ",0";
            this.DataContext = this; //do not modify it if "margin" exists
            //_______________
        }

        private void AddRows()
        {
            //****** add each row of keyboard which contains the keys in sequence
            keyboard_Rows.Add(_Keyboard_FirstRow);
            keyboard_Rows.Add(_Keyboard_SecondRow);
            keyboard_Rows.Add(_Keyboard_ThirdRow);
            keyboard_Rows.Add(_Keyboard_ForthRow);

            //****** end adding keys into the rows
        }

        private void AddKeys()
        {
            //------ init all keys

            //*** First row
            //k = new Keys("`", "~", 0, 1, _keyBoard.RowDefinitions.Count, _Shift);
            k = new Keys("1", "!", 0, 2, _keyBoard.RowDefinitions.Count, _Shift);
            k = new Keys("2", "@", 0, 3, _keyBoard.RowDefinitions.Count, _Shift);

            k = new Keys("3", "#", 0, 4, _keyBoard.RowDefinitions.Count, _Shift);
            k = new Keys("4", "$", 0, 5, _keyBoard.RowDefinitions.Count, _Shift);
            k = new Keys("5", "%", 0, 6, _keyBoard.RowDefinitions.Count, _Shift);

            k = new Keys("6", "^", 0, 7, _keyBoard.RowDefinitions.Count, _Shift);
            k = new Keys("7", "&", 0, 8, _keyBoard.RowDefinitions.Count, _Shift);
            k = new Keys("8", "*", 0, 9, _keyBoard.RowDefinitions.Count, _Shift);

            k = new Keys("9", "(", 0, 10, _keyBoard.RowDefinitions.Count, _Shift);
            k = new Keys("0", ")", 0, 11, _keyBoard.RowDefinitions.Count, _Shift);
            //k = new Keys("-", "_", 0, 12, _keyBoard.RowDefinitions.Count, _Shift);

            //k = new Keys("=", "+", 0, 13, _keyBoard.RowDefinitions.Count, _Shift);

            //*** Second row
            k = new Keys("Q", "Q", 1, 13, _keyBoard.RowDefinitions.Count, _Shift); //reverse the order for better layout
            k = new Keys("W", "W", 1, 12, _keyBoard.RowDefinitions.Count, _Shift);
            k = new Keys("E", "E", 1, 11, _keyBoard.RowDefinitions.Count, _Shift);

            k = new Keys("R", "R", 1, 10, _keyBoard.RowDefinitions.Count, _Shift);
            k = new Keys("T", "T", 1, 9, _keyBoard.RowDefinitions.Count, _Shift);
            k = new Keys("Y", "Y", 1, 8, _keyBoard.RowDefinitions.Count, _Shift);

            k = new Keys("U", "U", 1, 7, _keyBoard.RowDefinitions.Count, _Shift);
            k = new Keys("I", "I", 1, 6, _keyBoard.RowDefinitions.Count, _Shift);
            k = new Keys("O", "O", 1, 5, _keyBoard.RowDefinitions.Count, _Shift);

            k = new Keys("P", "P", 1, 4, _keyBoard.RowDefinitions.Count, _Shift);
            //k = new Keys("]", "}", 1, 3, _keyBoard.RowDefinitions.Count, _Shift);
            //k = new Keys("[", "{", 1, 2, _keyBoard.RowDefinitions.Count, _Shift);
            //k = new Keys("\\", "|", 1, 1, _keyBoard.RowDefinitions.Count, _Shift);

            //*** Third row
            k = new Keys("A", "A", 2, 1, _keyBoard.RowDefinitions.Count, _Shift);
            k = new Keys("S", "S", 2, 2, _keyBoard.RowDefinitions.Count, _Shift);
            k = new Keys("D", "D", 2, 3, _keyBoard.RowDefinitions.Count, _Shift);

            k = new Keys("F", "F", 2, 4, _keyBoard.RowDefinitions.Count, _Shift);
            k = new Keys("G", "G", 2, 5, _keyBoard.RowDefinitions.Count, _Shift);
            k = new Keys("H", "H", 2, 6, _keyBoard.RowDefinitions.Count, _Shift);

            k = new Keys("J", "J", 2, 7, _keyBoard.RowDefinitions.Count, _Shift);
            k = new Keys("K", "K", 2, 8, _keyBoard.RowDefinitions.Count, _Shift);
            k = new Keys("L", "L", 2, 9, _keyBoard.RowDefinitions.Count, _Shift);

            //k = new Keys(";", ":", 2, 10, _keyBoard.RowDefinitions.Count, _Shift);
            //k = new Keys("'", "\"", 2, 11, _keyBoard.RowDefinitions.Count, _Shift);

            //*** Forth row
            k = new Keys("Z", "Z", 3, 1, _keyBoard.RowDefinitions.Count, _Shift);
            k = new Keys("X", "X", 3, 2, _keyBoard.RowDefinitions.Count, _Shift);
            k = new Keys("C", "C", 3, 3, _keyBoard.RowDefinitions.Count, _Shift);

            k = new Keys("V", "V", 3, 4, _keyBoard.RowDefinitions.Count, _Shift);
            k = new Keys("B", "B", 3, 5, _keyBoard.RowDefinitions.Count, _Shift);
            k = new Keys("N", "N", 3, 6, _keyBoard.RowDefinitions.Count, _Shift);

            k = new Keys("M", "M", 3, 7, _keyBoard.RowDefinitions.Count, _Shift);
            //k = new Keys(",", "<", 3, 8, _keyBoard.RowDefinitions.Count, _Shift);
            //k = new Keys(".", ">", 3, 9, _keyBoard.RowDefinitions.Count, _Shift);

            //k = new Keys("/", "?", 3, 10, _keyBoard.RowDefinitions.Count, _Shift);

            //------ end init keys
        }

 
    }
}
