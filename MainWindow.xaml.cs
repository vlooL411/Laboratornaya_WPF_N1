using Laboratornaya_WPF_N1.Pages;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Laboratornaya_WPF_N1
{
    public partial class MainWindow : Window
    {
        Autorization autorization = new Autorization();
        Registration registration = new Registration();
        ShowLids showLids = new ShowLids();
        AddLid addLid = new AddLid();
        EditLid editLid = new EditLid();
        public MainWindow()
        {
            InitializeComponent();

            Navigate(autorization);
            autorization.BtnMoveRegistration.Click += (o, e) => Navigate(registration);
            registration.BtnMoveAutorization.Click += (o, e) => Navigate(autorization);
            autorization.ActionAutorization += () => { showLids.FillLids(autorization?.User); Navigate(showLids); };
            registration.ActionRegistration += () => Navigate(autorization);

            showLids.BtnAddLid.Click += (o, e) => { Navigate(addLid); addLid.SetUser(autorization?.User); };
            showLids.ActEditLid += () => { Navigate(editLid); editLid.SetUser(showLids.Lids?.SelectedItem as Lid); };

            addLid.ActAddLid += () => { showLids.Update(); Navigate(showLids); };
            addLid.BtnEndWithoutSaveLids.Click += (o, e) => Navigate(showLids);

            editLid.ActSaveLid += () => { showLids.Update(); Navigate(showLids); };
            editLid.BtnEndWithoutSaveLids.Click += (o, e) => Navigate(showLids);
        }
        public void Navigate(Page Move)
        {
            MainShow.Navigate(Move);
            Title = Move.Title;
        }
        void MainShow_Navigated(object o, NavigationEventArgs e)
        {
            var page = e.Content;
            if (MainShow.CanGoBack && (page is ShowLids || page is Autorization))
                MainShow.RemoveBackEntry();
        }
        void Back_Click(object o, RoutedEventArgs e) { if (MainShow.CanGoBack) MainShow.GoBack(); }
        void Exit_Click(object o, RoutedEventArgs e) => Navigate(autorization);

    }
}