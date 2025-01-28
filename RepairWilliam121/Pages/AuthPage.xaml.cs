﻿using System;
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

namespace RepairWilliam121.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
            this.Loaded += AuthPage_Loaded;
        }

        private void AuthPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Очистка полей
            log.Clear();
            pass.Clear();
            txtHintLogin.Visibility = Visibility.Visible;
            txtHintPass.Visibility = Visibility.Visible;

        }

        private void log_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtHintLogin.Visibility = Visibility.Visible;
            if (log.Text.Length > 0)
            {
                txtHintLogin.Visibility = Visibility.Hidden;
            }
        }

        private void txtHintLogin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            log.Focus();
        }

        private void pass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            txtHintPass.Visibility = Visibility.Visible;
            if (pass.Password.Length > 0)
            {
                txtHintPass.Visibility = Visibility.Hidden;
            }
        }

        private void txtHintPass_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pass.Focus();
        }

        private void ButtonGoIn_Click(object sender, RoutedEventArgs e)
        {
            string username = log.Text;
            string password = pass.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var db = new RepairDBEntities())
            {
                var user = db.Users.AsNoTracking().FirstOrDefault(u => u.Username == username);

                if (user == null)
                {
                    MessageBox.Show("Пользователь с такими данными не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (user.PasswordHash != password)
                {
                    MessageBox.Show("Неверный пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MessageBox.Show("Авторизация успешна!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                switch (user.Role)
                {
                    case "Client":
                        NavigationService.Navigate(new UserPage(user.UserID));
                        break;
                    case "Admin":
                        NavigationService.Navigate(new AdminPage());
                        break;
                    case "Technician":
                        NavigationService.Navigate(new ManagerPage());
                        break;
                }
            }
        }
    }
}
