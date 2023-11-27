using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using System.Windows.Forms;


namespace KeyBord
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool autoKeyPressEnabled = false;
        private Thread autoKeyPressThread;
        private int interval=10000;//10초

        public MainWindow()
        {
            InitializeComponent();
        }
        //private void VirtualKey_Click(object sender, RoutedEventArgs e)
        //{
        //    // 클릭된 가상 키에 대한 처리를 여기에 추가합니다.
        //    if (sender is Button button)
        //    {
        //        InputTextBox.Text += button.Content.ToString();
        //    }
        //}
        private void AutoKeyPressCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // 체크박스가 체크되면 자동 키 누름 스레드를 시작합니다.
            if (int.TryParse(IntervalTextBox.Text, out interval)&&!autoKeyPressEnabled)
            {
                autoKeyPressEnabled = true;
                autoKeyPressThread = new Thread(AutoKeyPressThreadMethod);
                autoKeyPressThread.IsBackground = true;
            autoKeyPressThread.Start();
            }
            else
            {
                System.Windows.MessageBox.Show("Invalid interval. Please enter a valid number.");
                AutoKeyPressCheckBox.IsChecked = false;
            }
        }

        private void AutoKeyPressCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if(autoKeyPressEnabled)
            {
            autoKeyPressEnabled = false;

            // 체크박스가 해제되면 스레드를 중지합니다.
            if (autoKeyPressThread != null && autoKeyPressThread.IsAlive)
                autoKeyPressThread.Join();
            }

        }

        private void AutoKeyPressThreadMethod()
        {
            while (autoKeyPressEnabled)
            {
                // Pause 키를 누름
                SendKeys.SendWait("{BREAK}");
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                {
                    //UI관련 코드
                    int.TryParse(IntervalTextBox.Text, out interval);
                }));
                // 1초 대기 (1000), 60초=60000
                Thread.Sleep(interval);
            }
        }
        void App_Exit(object sender, ExitEventArgs e)
        {
            autoKeyPressEnabled = false;
            autoKeyPressThread.Abort();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                this.Close();
                Environment.Exit(0);//강제종료
        }

    }
}
