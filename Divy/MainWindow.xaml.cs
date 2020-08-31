using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Printing;
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
using Divy.Common;

namespace Divy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            new Tracing(); // Init Tracing
            InitializeComponent();
            try
            {
                throw new Exception("Boom Boom Boom");
            }
            catch(Exception ex)
            {
                Tracing.Fatal("Atomic Failure", ex);
            }
        }
    }
}
