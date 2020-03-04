using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Laboratornaya_WPF_N1.Pages
{
    public partial class ShowLids : Page
    {
        public Action ActEditLid;
        public ShowLids() => InitializeComponent();
        LaboratornayN1Entities BD = new LaboratornayN1Entities();
        public void FillLids(User user)
        {
            CheckStatus.IsChecked = null;
            Users.Items.Clear();
            Users.Items.Add(new ComboBoxItem { Content = "Все" });
            Users.Items.Add(user);
            Users.SelectedIndex = 1;
            foreach (var el in BD.Users)
                if (el.ID != user.ID)
                    Users.Items.Add(el);
        }
        public void FillLids(User user, bool? Status)
        {
            Lids.ItemsSource = null;
            List<Lid> lids;
            if (user != null)
                lids = BD.Lids.Where(l => l.UserID == user.ID).ToList();
            else lids = BD.Lids.ToList();
            if (Status != null)
                lids = lids.Where(u => Convert.ToBoolean(u.Status) == Status).ToList();
            Lids.ItemsSource = lids;
        }
        public void Update()
        {
            var user = Users?.SelectedItem;
            if (user != null)
                FillLids(user as User, CheckStatus?.IsChecked);
        }
        void ComboBox_Selected(object o, RoutedEventArgs e) => Update();
        void BtnEditLid_Click(object o, RoutedEventArgs e) { if (Lids?.SelectedItem != null) ActEditLid?.Invoke(); }
    }
    class RequireSkillConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var rate = value as Rate;
            return $"{Math.Round(rate.WorkWithObjections.Value, 2)} {Math.Round(rate.MasteringTheSkillsOfSales.Value, 2)} {Math.Round(rate.KnowledgeOfСompanysProducts.Value, 2)}";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}