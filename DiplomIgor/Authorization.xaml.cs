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
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace DiplomIgor
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        ApplicationContext db;
        public Authorization()
        {
            InitializeComponent();

            db = new ApplicationContext();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private async void Registration_Button(object sender, RoutedEventArgs e)
        {


            if (Registration_Window.Visibility == Visibility.Visible)
            {
                string login = loginReg.Text.Trim();
                string pass = PassReg.Password.Trim();
                string pass_2 = PassReg_2.Password.Trim();
                string email = Email.Text.Trim().ToLower();

                if (login.Length < 5)
                {
                    loginReg.ToolTip = "ЭТО ПОЛЕ ВВЕДЕНО НЕВЕРНО";
                    loginReg.Background = Brushes.DarkRed;
                }
                else if (pass.Length < 5)
                {
                    PassReg.ToolTip = "ЭТО ПОЛЕ ВВЕДЕНО НЕВЕРНО";
                    PassReg.Background = Brushes.DarkRed;
                }
                else if (pass_2 != pass)
                {
                    PassReg_2.ToolTip = "ПАРОЛИ НЕ СОВПАДАЮТ";
                    PassReg_2.Background = Brushes.DarkRed;
                }
                else if (email.Length < 5 || !email.Contains("@") || !email.Contains("."))
                {
                    Email.ToolTip = "ЭТО ПОЛЕ ВВЕДЕНО НЕВЕРНО";
                    Email.Background = Brushes.DarkRed;
                }
                else
                {
                    loginReg.ToolTip = "";
                    loginReg.Text = "Логин";
                    loginReg.Background = Brushes.Transparent;
                    PassReg.ToolTip = "";
                    PassReg.Password = "Password";
                    PassReg.Background = Brushes.Transparent;
                    PassReg_2.ToolTip = "";
                    PassReg_2.Password = "Password";
                    PassReg_2.Background = Brushes.Transparent;
                    Email.ToolTip = "";
                    Email.Text = "Email";
                    Email.Background = Brushes.Transparent;

                    //MessageBox.Show("Всё гуд!");
                    User user = new User(login, pass, email);
                    
                    db.Users.Add(user);
                    db.SaveChanges();
                }
            } else
            {
                #region AnimationReg
                DoubleAnimation btnAnimationClose = new DoubleAnimation();

                btnAnimationClose.From = 75;
                btnAnimationClose.To = 0;
                btnAnimationClose.Duration = TimeSpan.FromSeconds(0.2);
                Login_Window.BeginAnimation(Button.HeightProperty, btnAnimationClose);

                if (Login_Window.Visibility != Visibility.Collapsed)
                    await Task.Delay(TimeSpan.FromSeconds(0.2));

                DoubleAnimation btnAnimation = new DoubleAnimation();
                btnAnimation.From = 0;
                btnAnimation.To = 175;
                btnAnimation.Duration = TimeSpan.FromSeconds(0.6);
                Registration_Window.BeginAnimation(Button.HeightProperty, btnAnimation);
                #endregion

                Registration_Window.Visibility = Visibility.Visible;
                Login_Window.Visibility = Visibility.Collapsed;
            }
        }

        private async void Login_Button(object sender, RoutedEventArgs e)
        {

            if (Login_Window.Visibility == Visibility.Visible)
            {
                string login = loginLog.Text.Trim();
                string pass = passLog.Password.Trim();

                if (login.Length < 5)
                {
                    loginLog.ToolTip = "ЭТО ПОЛЕ ВВЕДЕНО НЕВЕРНО";
                    loginLog.Background = Brushes.DarkRed;
                }
                else if (pass.Length < 5)
                {
                    passLog.ToolTip = "ЭТО ПОЛЕ ВВЕДЕНО НЕВЕРНО";
                    passLog.Background = Brushes.DarkRed;
                } else
                {
                    loginLog.ToolTip = "";
                    loginLog.Text = "Логин";
                    loginLog.Background = Brushes.Transparent;
                    passLog.ToolTip = "";
                    passLog.Password = "Password";
                    passLog.Background = Brushes.Transparent;

                    User authUser = null;
                    using (ApplicationContext db = new ApplicationContext())
                    {
                        authUser = db.Users.Where(b => b.Login == login && b.Pass == pass).FirstOrDefault();
                    }

                    if (authUser != null)
                    {
                        MainWindow mainWindow= new MainWindow();
                        mainWindow.Show();
                        Close();
                    }
                    else
                        MessageBox.Show("Всё плоха!");
                }
            } else
            {
                #region AnimationLog
                DoubleAnimation btnAnimationClose = new DoubleAnimation();

                btnAnimationClose.From = 175;
                btnAnimationClose.To = 0;
                btnAnimationClose.Duration = TimeSpan.FromSeconds(0.6);
                Registration_Window.BeginAnimation(Button.HeightProperty, btnAnimationClose);

                if (Registration_Window.Visibility != Visibility.Collapsed)
                    await Task.Delay(TimeSpan.FromSeconds(0.6));
                DoubleAnimation btnAnimation = new DoubleAnimation();

                btnAnimation.From = 0;
                btnAnimation.To = 75;
                btnAnimation.Duration = TimeSpan.FromSeconds(0.2);
                Login_Window.BeginAnimation(Button.HeightProperty, btnAnimation);
                #endregion

                Registration_Window.Visibility = Visibility.Collapsed;
                Login_Window.Visibility = Visibility.Visible;
            }
            
        }
    }
}
