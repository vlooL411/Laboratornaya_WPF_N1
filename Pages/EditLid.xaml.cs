using System;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Laboratornaya_WPF_N1.Pages
{
    public partial class EditLid : Page
    {
        public Action ActSaveLid;
        public EditLid() => InitializeComponent();
        Lid Lid;
        LaboratornayN1Entities BD = new LaboratornayN1Entities();
        public void SetUser(Lid lid)
        {
            Lid = lid;
            DataContext = Lid;
            if (Lid.DurationCall == null)
                Duration.IsReadOnly = false;
            if (Lid.DateCallLid == null)
                DateCallLid.IsReadOnly = false;
            FillUsers();
            FindLidNumber();
        }
        void FillUsers()
        {
            BindingUser.Items.Clear();
            BindingUser.Items.Add(Lid.User);
            BindingUser.SelectedIndex = 0;
            foreach (var u in BD.Users.Where(uId => uId.ID != Lid.UserID))
                BindingUser.Items.Add(u);
        }
        void FindLidNumber()
        {
            LidCalls.Items.Clear();
            foreach (var l in BD.Lids.Where(li => li.NumberPhoneClient == Phone.Text))
                LidCalls.Items.Add(l);
        }
        void BtnSaveLids_Click(object o, RoutedEventArgs e) => SaveLid();
        void SaveLid()
        {
            var user = BindingUser?.SelectedItem as User;
            if (user == null || Lid == null ||
                Duration.Text == "" || DateCallLid.Text == "") return;
            try
            {
                Lid.DateCallLid = DateTime.Parse(DateCallLid.Text);
                Lid.DurationCall = int.Parse(Duration.Text);

                Lid.Rate.WorkWithObjections = float.Parse(WorkWithObjections.Text);
                Lid.Rate.MasteringTheSkillsOfSales = float.Parse(MasteringTheSkillsOfSales.Text);
                Lid.Rate.KnowledgeOfСompanysProducts = float.Parse(KnowledgeOfСompanysProducts.Text);
            }
            catch { return; }

            Lid.NumberPhoneClient = long.Parse(Phone.Text).ToString();
            Lid.DateCreateLid = DateTime.Parse(DateCreateLid.Text);
            Lid.Comment = Comment.Text == "" ? null : Comment.Text;
            Lid.Status = Convert.ToByte(Status.IsChecked.Value);
            Lid.UserID = user.ID;
            Lid.User = user;

            BD.Lids.AddOrUpdate(Lid);
            try
            {
                BD.SaveChanges();
                ActSaveLid?.Invoke();
            }
            catch { MessageBox.Show("Error save data", "Error server BD"); }
        }
        void FindLidOnNumber_Click(object o, RoutedEventArgs e) => FindLidNumber();
        void Reset_Click(object sender, RoutedEventArgs e) => DataContext = Lid;
    }
    class NotTrue : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool) return !(bool)value;
            if (value is Visibility) return (Visibility)value == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}