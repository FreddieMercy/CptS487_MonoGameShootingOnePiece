using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Threading;

namespace KmapInterface
{
    //Find the Control Elements based on the X and Y
    public static class GridExtensions
    {
        public static UIElement GetElements<TControl>(this List<ControlsXYandWidthHeight> list, double row, double column)
            where TControl : UIElement
        {
            if (list != null)
            {
                var elements = from ControlsXYandWidthHeight element in list
                               where element.Self() is TControl &&
                                     element.X() <= row &&
                                     element.X() + element.Width() >= row &&
                                     element.Y() + element.Height() >= column &&
                                     element.Y() <= column &&
                                     element.Self().IsEnabled
                               select element as ControlsXYandWidthHeight;

                if (elements != null)
                {
                    Collection<ControlsXYandWidthHeight> tmps = new Collection<ControlsXYandWidthHeight>(elements.ToList());

                    if (tmps.Count == 0)
                    {
                        return null;
                    }

                    //Used in overlap caused by "Enlarging"
                    //if more than one had been selected, return what with the highest Zindex
                    ControlsXYandWidthHeight tmp = tmps.Aggregate((btn1, btn2) => btn1.ZIndex() > btn2.ZIndex() ? btn1 : btn2);

                    return tmp.Self();
                }

                return null;
            }

            return null;
        }
    }

    //get All Control Elements (such as, in XAML) By Type
    public static class getAllControlsByType
    {
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }

    public interface ControlsXYandWidthHeight
    {
        UIElement Self();
        double X();
        double Y();
        double Width();
        double Height();
        int ZIndex();
    }
    
    public class IsControlXYandWidthHeight : ControlsXYandWidthHeight
    {
        private UIElement _self;
        public UIElement Self()
        {
            return _self;
        }

        public double X()
        {
            if (Height() <= 0 && Width() <= 0)
            {
                return 0;
            }
            return Self().PointToScreen(new Point(0d, 0d)).X;
        }

        public double Y()
        {
            if (Height() <= 0 && Width() <= 0)
            {
                return 0;
            }

            return Self().PointToScreen(new Point(0d, 0d)).Y;
        }

        public virtual double Width()
        {
            return (Self() as Control).ActualWidth;
        }

        public virtual double Height()
        {
            return (Self() as Control).ActualHeight;
        }

        public int ZIndex()
        {
            return Panel.GetZIndex(Self());
        }

        public IsControlXYandWidthHeight(UIElement self)
        {
            /*
            if(self == null | x < 0 | y < 0 | width <= 0 | height <= 0 | 
                Double.IsNaN(x) | Double.IsNaN(y) | Double.IsNaN(width) | Double.IsNaN(height) | 
                Double.IsPositiveInfinity(x) | Double.IsPositiveInfinity(y) | Double.IsPositiveInfinity(width) | Double.IsPositiveInfinity(height) |
                Double.IsNegativeInfinity(x) | Double.IsNegativeInfinity(y) | Double.IsNegativeInfinity(width) | Double.IsNegativeInfinity(height))
            {
                throw new ArgumentException("Arguments are not valid!!!!");
            }
            */  //<----- Don't need all those, since if the Controls are not visible, don't need to worry about it

            if (self == null)
            {
                throw new ArgumentException("Self is null!!!!");
            }

            _self = self;
        }
    }
}
