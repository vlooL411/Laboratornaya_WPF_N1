using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Laboratornaya_WPF_N1.Pages
{
    public partial class Autorization : Page
    {
        public event Action ActionAutorization;
        User _User;
        public User User { get => _User; set { if ((_User = value) != null) ActionAutorization?.Invoke(); } }
        public Autorization() => InitializeComponent();
        void Autorization_Click(object o, RoutedEventArgs e)
        {
            if (Username.Text == "" || Password.Password == "") return;
            var password = Convert.ToBase64String(new SHA256CryptoServiceProvider()
                .ComputeHash(Encoding.ASCII.GetBytes(Password.Password)));
            try
            {
                User = new LaboratornayN1Entities().Users.
                    Where(u => u.Username == Username.Text && u.Password == password).First();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
    }

}