using System;
using System.Collections.Generic;
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

namespace GPRSDataShow
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Left = 0;
            this.Top = 0;
            this.Width = SystemParameters.WorkArea.Width;
            this.Height = SystemParameters.WorkArea.Height;
            this.Loaded += MainWindow_Loaded;
            this.Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("是否关闭此程序?", "消息提示", MessageBoxButton.OKCancel) != MessageBoxResult.OK) e.Cancel = true;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.frame.Source = new Uri(@"\Views\YGMainView.xaml" , UriKind.Relative);
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            throw new Exception("最新错误消息");
            MenuItem menuItem = (MenuItem)e.OriginalSource;
            switch(menuItem.Name)
            {
                case "MenuClose":
                    if(MessageBox.Show("是否关闭此程序?","消息提示",MessageBoxButton.OKCancel) == MessageBoxResult.OK) Application.Current.Shutdown();
                    break;
            }
        }

        //private void txtMin_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    this.txtMin.Foreground = new SolidColorBrush { Color = Color.FromRgb(255, 127, 39) };
        //}

        //private void txtMin_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    this.txtMin.Foreground = new SolidColorBrush { Color = Colors.White };
        //}

        private void txtMin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.ActualHeight > SystemParameters.WorkArea.Height || this.ActualWidth > SystemParameters.WorkArea.Width)
            {
                this.WindowState = System.Windows.WindowState.Normal;
            }
        }
    }
}
