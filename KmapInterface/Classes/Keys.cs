using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows;
using System.ComponentModel;

namespace KmapInterface
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public class Keys
        {
            private Inputs input = null;
            public int RowIndex { get; private set; }
            public int ColIndex { get; private set; }

            //used to track all keys location
            static Dictionary<Keys, Tuple<int, int>> _tracker = new Dictionary<Keys, Tuple<int, int>>();
            public string Content
            {
                get
                {
                    return input.Content;
                }
            }

            public Keys(string prime, string second, int row, int col, int RowDefinition, Button Shift, ObservableCollection<Keys> collection = null)
            {
                //inputs construction
                if (string.IsNullOrEmpty(prime))
                {
                    throw new NullReferenceException("Keys(string.IsNullOrEmpty(prime))");
                }

                if (!string.IsNullOrEmpty(second))
                {
                    //Changeable
                    input = new ChangableInputs(prime, second);
                }
                else
                {
                    //Just "Inputs"
                    input = new Inputs(prime);
                }

                //RowIndex

                if (row < 0 | row >= RowDefinition)//((MainWindow)System.Windows.Application.Current.MainWindow)._keyBoard.RowDefinitions.Count)
                {
                    throw new OutOfMemoryException("row <= 0 | row >= ((MainWindow)System.Windows.Application.Current.MainWindow)._keyBoard.RowDefinitions.Count");
                }

                RowIndex = row;

                //ColIndex

                if (col < 0)
                {
                    throw new OutOfMemoryException("row <= 0 | row >= ((MainWindow)System.Windows.Application.Current.MainWindow)._keyBoard.ColumnDefinitions.Count");
                }

                ColIndex = col;

                //static tracker
                Tuple<int, int> tmp = new Tuple<int, int>(RowIndex, ColIndex);
                trackContains(tmp);

                _tracker.Add(this, tmp);

                //Shift/Cap

                if (input is ChangableInputs)
                {
                    //((MainWindow)System.Windows.Application.Current.MainWindow)._Shift.Click += (input as ChangableInputs).BindWithSpecKey;
                    Shift.Click += (input as ChangableInputs).BindWithSpecKey;
                }

            }

            private void trackContains(Tuple<int, int> tmp)
            {
                //same location
                if (_tracker.Values.Contains(tmp))
                {
                    string s = "";

                    foreach (Keys k in _tracker.Keys)
                    {
                        if (k.RowIndex == RowIndex && k.ColIndex == ColIndex)
                        {
                            if (k.input is ChangableInputs)
                            {
                                s = (k.input as ChangableInputs).PrimaryContent + "/" + (k.input as ChangableInputs).SecondContent;
                            }
                            else
                            {
                                s = k.input.Content;
                            }
                            break;
                        }
                    }

                    if (input is ChangableInputs)
                    {
                        string s2 = (input as ChangableInputs).PrimaryContent + "/" + (input as ChangableInputs).SecondContent;
                        throw new Exception("The Key '" + s2 + "' and '" + s + "' have the same location: < " + RowIndex + ", " + ColIndex + ">");
                    }
                    else
                    {
                        throw new Exception("The Key '" + input.Content + "' and '" + s + "' have the same location: < " + RowIndex + ", " + ColIndex + ">");
                    }
                }

                //same key
                foreach (Keys k in _tracker.Keys)
                {
                    string s = "";
                    string s2 = "";

                    if (k.input is ChangableInputs)
                    {
                        s = (k.input as ChangableInputs).PrimaryContent + "/" + (k.input as ChangableInputs).SecondContent;
                    }
                    else
                    {
                        s = k.input.Content;
                    }

                    if (input is ChangableInputs)
                    {
                        s2 = (input as ChangableInputs).PrimaryContent + "/" + (input as ChangableInputs).SecondContent;
                    }
                    else
                    {
                        s2 = input.Content;
                    }

                    if (s == s2)
                    {
                        throw new Exception("Multiple key '" + s + "' had been added!!");
                    }
                }
            }

            public static List<Keys> Items
            {
                get
                {
                    return _tracker.Keys.ToList<Keys>();
                }
            }

            public static void Clear()
            {
                _tracker.Clear();
            }
        }
    }
}