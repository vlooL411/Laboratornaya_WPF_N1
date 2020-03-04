using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Laboratornaya_WPF_N1.Pages
{
    public partial class AddLid : Page
    {
        public Action ActAddLid;
        public AddLid() => InitializeComponent();
        DispatcherTimer Timer;
        public float? TimerPost { get; set; }
        LaboratornayN1Entities BD = new LaboratornayN1Entities();
        public void SetUser(User user)
        {
            Reset(false);
            if (user == null) return;
            DateCreateLid.Text = DateTime.Now.ToString();

            BindingUser.Items.Clear();
            BindingUser.Items.Add(user);
            BindingUser.SelectedIndex = 0;
            foreach (var el in BD.Users)
                if (el.ID != user.ID)
                    BindingUser.Items.Add(el);
        }
        void BindingUser_SelectionChanged(object o, SelectionChangedEventArgs e)
        {
            var user = BindingUser?.SelectedItem as User;
            int duration = 0;
            try { duration = int.Parse(Duration.Text); } catch { }
            DataContext = new Lid { NumberPhoneClient = Phone.Text, DurationCall = duration, Rate = user?.Rate };
        }
        void BtnBeginLids_Click(object o, RoutedEventArgs e)
        {
            long l;
            if (Phone.Text == null || !long.TryParse(Phone.Text, out l)) return;

            Reset();

            DateCallLid.Text = DateTime.Now.ToString();

            TimerPost = 0;
            Timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 500) };
            Timer.Tick += (o1, e1) => Duration.Text = ((int)(TimerPost += 0.5f)).ToString();
            Timer?.Start();
        }
        void CreateLid()
        {
            Timer?.Stop();
            var user = BindingUser?.SelectedItem as User;
            if (user == null || Duration.Text == "" || Phone.Text == "") return;

            string Number;
            DateTime DateCall, DateCreate;
            float WWO, MTSOS, KOCP;

            try
            {
                if (TimerPost == null)
                    TimerPost = float.Parse(Duration.Text);
                Number = long.Parse(Phone.Text).ToString();
                DateCall = DateTime.Parse(DateCallLid.Text);
                DateCreate = DateTime.Parse(DateCreateLid.Text);

                WWO = float.Parse(WorkWithObjections.Text);
                MTSOS = float.Parse(MasteringTheSkillsOfSales.Text);
                KOCP = float.Parse(KnowledgeOfСompanysProducts.Text);
            }
            catch {
                
                return; }

            var rate = new Rate
            {
                WorkWithObjections = WWO,
                MasteringTheSkillsOfSales = MTSOS,
                KnowledgeOfСompanysProducts = KOCP
            };
            BD.Rates.Add(rate);
            var lid = new Lid
            {
                NumberPhoneClient = Number,
                DurationCall = (int)TimerPost,
                Status = Convert.ToByte(Status.IsChecked.Value),
                DateCallLid = DateCall,
                DateCreateLid = DateCreate,
                Comment = Comment.Text == "" ? null : Comment.Text,
                RateID = rate.ID,
                UserID = user.ID
            };
            BD.Lids.Add(lid);

            try
            {
                BD.SaveChanges();
                ActAddLid?.Invoke();
            }
            catch { MessageBox.Show("Error save data", "Error server BD"); }
        }
        public void Reset(bool reset = true)
        {
            Timer?.Stop();
            LidCalls.Items.Clear();

            TimerPost = null;
            Phone.Text = "";
            Duration.Text = "";
            DateCallLid.Text = "";
            DateCreateLid.IsReadOnly = reset;
            DateCallLid.IsReadOnly = reset;
            Duration.IsReadOnly = reset;
            BindingUser.IsEnabled = !reset;
            Status.IsChecked = reset;
            Status.IsEnabled = !reset;
            BtnEndLids.Visibility = reset ? Visibility.Visible : Visibility.Collapsed;
            BtnBeginLids.Visibility = reset ? Visibility.Collapsed : Visibility.Visible;
        }
        void BtnEndLids_Click(object o, RoutedEventArgs e)
        {
            Timer?.Stop();
            Status.IsEnabled = true;
            BtnEndLids.Visibility = Visibility.Collapsed;
        }
        void BtnEndWithoutSaveLids_Click(object o, RoutedEventArgs e) => Reset(false);
        void BtnAddLids_Click(object o, RoutedEventArgs e) => CreateLid();
        void FindLidOnNumber_Click(object o, RoutedEventArgs e)
        {
            LidCalls.Items.Clear();
            foreach (var l in BD.Lids.Where(li => li.NumberPhoneClient == Phone.Text))
                LidCalls.Items.Add(l);
        }
    }
}