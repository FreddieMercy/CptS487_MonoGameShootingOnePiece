using System;
using System.ComponentModel;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace KmapInterface
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private void setUpAllKeysStyles_Normal(object sender = null, EventArgs e = null)
        {
            for (int i = 0; i < keyboard_Rows.Count; i++)
            {
                keyboard_Rows[i].ItemTemplate = setUpTheKeysStyle(_keyBoard.Children.IndexOf((keyboard_Rows[i].Parent as Grid)));
            }

            Dispatcher.BeginInvoke((Action)(() =>
            {
                CollectionAlltheButtonsInTheView();
            }));
        }

        private void CollectionAlltheButtonsInTheView()
        {
            tmpBtnCollection.Clear();

            foreach (Button btn in getAllControlsByType.FindVisualChildren<Button>(KmapUI))
            {
                btn.Focusable = false; //Use this for "eye-Enlarging"
                tmpBtnCollection.Add(new IsControlXYandWidthHeight(btn));
            }
        }
        private void SortKeys()
        {
            table.Clear();
            //init the item_collection based on rowIndex
            for (int i = 0; i < _keyBoard.RowDefinitions.Count; i++)
            {
                table[i] = new SortedSet<Tuple<int, Keys>>();
            }

            //sort each items_collection based on the columnIndex
            foreach (Keys x in Keys.Items)
            {
                (table[Keys.Items[Keys.Items.IndexOf(x)].RowIndex] as SortedSet<Tuple<int, Keys>>).Add(new Tuple<int, Keys>(Keys.Items[Keys.Items.IndexOf(x)].ColIndex, x));
            }
        }

        private void SetItemssourceOfKeyboardKeys()
        {
            for (int i = 0; i < keyboard_Rows.Count; i++)
            {
                List<Keys> sortedKeys = new List<Keys>();

                foreach (Tuple<int, Keys> x in (table[i] as SortedSet<Tuple<int, Keys>>))
                {
                    sortedKeys.Add(x.Item2);
                }

                keyboard_Rows[i].ItemsSource = sortedKeys;
            }
        }

        private DataTemplate setUpTheKeysStyle(int i)
        {
            //ListBox, sort of
            DataTemplate template = new DataTemplate();

            //Button
            FrameworkElementFactory btn = new FrameworkElementFactory(typeof(Button));

            btn.SetBinding(Button.ContentProperty, new Binding("Content"));

            btn.SetValue(Button.HeightProperty, _keyBoard.RowDefinitions[i].ActualHeight);
            btn.SetValue(Button.WidthProperty, Math.Max(0, ((this.ActualWidth - margin*4) /(getKeyRowCount(i)+1) -margin)));

            btn.SetValue(Button.MarginProperty, new Thickness(0, 0, margin, 0));
            btn.SetValue(Button.PaddingProperty, new Thickness(0));
            //btn.SetValue(Button.FocusableProperty, false);
            btn.AddHandler(Button.ClickEvent, new RoutedEventHandler(Button_Click));

            template.VisualTree = btn;

            return template;
        }

        private int getKeyRowCount(int i)
        {
            switch (i/2)
            {
                case 0:
                    return _Keyboard_FirstRow.Items.Count-1;

                case 1:
                    return _Keyboard_SecondRow.Items.Count-1;

                case 2:
                    return _Keyboard_ThirdRow.Items.Count+2;

                case 3:
                    
                    return _Keyboard_ForthRow.Items.Count+2;
                default:
                    throw new Exception("You added new rows? define it at 'getKeyRowCount(int i)'");
            }
        }

        private static object GetValueFromStyle(object styleKey, DependencyProperty property)
        {
            Style style = Application.Current.TryFindResource(styleKey) as Style;
            while (style != null)
            {
                var setter =
                    style.Setters
                        .OfType<Setter>()
                        .FirstOrDefault(s => s.Property == property);

                if (setter != null)
                {
                    return setter.Value;
                }

                style = style.BasedOn;
            }
            return null;
        }
    }
}
