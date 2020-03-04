using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Laboratornaya_WPF_N1.Pages
{
    public partial class Registration : Page
    {
        LaboratornayN1Entities BD = new LaboratornayN1Entities();
        public event Action ActionRegistration;
        public Registration() => InitializeComponent();
        void BtnRegistration_Click(object o, RoutedEventArgs e)
        {
            if (Username.Text == "" || Password.Password == "" || TryPassword.Password == "" ||
                Password.Password != TryPassword.Password)
                return;
            if (FIO.Text == "")
            {
                FIO.BorderBrush = Brushes.Red;
                return;
            }
            else FIO.BorderBrush = Brushes.Gray;
            if (BD.Users.Where(u => u.Username == Username.Text).Count() != 0)
            {
                Username.BorderBrush = Brushes.Red;
                return;
            }
            else Username.BorderBrush = Brushes.Gray;

            var password = Convert.ToBase64String(new SHA256CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(Password.Password)));
            BD.Users.Add(new User
            {
                Username = Username.Text,
                Password = password,
                FIO = FIO.Text,
                Rate = new Rate
                {
                    KnowledgeOfСompanysProducts = 0.5f,
                    MasteringTheSkillsOfSales = 0.5f,
                    WorkWithObjections = 0.5f
                }
            });
            BD.SaveChanges();
            ActionRegistration?.Invoke();
            Password.Password = (TryPassword.Password = "");
        }
    }
}