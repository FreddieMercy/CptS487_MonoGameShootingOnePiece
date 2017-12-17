using System;
using System.Windows;
using System.ComponentModel;
namespace KmapInterface
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private class Inputs
        {
            public string Content { get; protected set; } //The reason why I am using "string" instead of "char" is, seeking the possibility that user can input words (i.e: place, time, and personal pronoun）
                                                          //which should be more convenient for the user comparing to input single "char" each time 

            public Inputs(string content)
            {
                if (string.IsNullOrEmpty(content))
                {
                    throw new NullReferenceException("Inputs(string.IsNullOrEmpty(content))");
                }

                Content = content;
            }
        }
    }
}
