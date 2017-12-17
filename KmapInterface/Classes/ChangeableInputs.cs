using System;
using System.Windows;
using System.ComponentModel;
namespace KmapInterface
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private class ChangableInputs : Inputs
        {
            //public event EventHandler ChangableInputsHandler;

            public string PrimaryContent { get; private set; }
            public string SecondContent { get; private set; }

            public ChangableInputs(string prime, string second) : base(prime)
            {
                if (string.IsNullOrEmpty(second) || string.IsNullOrEmpty(prime))
                {
                    throw new NullReferenceException("ChangableInputs(string.IsNullOrEmpty(second) || string.IsNullOrEmpty(prime))");
                }

                PrimaryContent = prime;
                SecondContent = second;
            }

            public void BindWithSpecKey(object sender, EventArgs e)
            {
                if (string.IsNullOrEmpty(Content))
                {
                    throw new NullReferenceException("string.IsNullOrEmpty(Content)");
                }

                if (Content == PrimaryContent)
                {
                    Content = SecondContent;
                }

                else if (Content == SecondContent)
                {
                    Content = PrimaryContent;
                }

                else
                {
                    throw new ArgumentException("Content != Prime && Content != Second");
                }


            }
        }
    }
}