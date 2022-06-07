using DesktopContactsApp.Classes;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DesktopContactsApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Contact> Contacts { get; set; } = new List<Contact>();
        public MainWindow()
        {
            InitializeComponent();
            ReadDatabase();
        }

        private void NewContact_OnClick(object sender, RoutedEventArgs e)
        {
            var newContactWindow = new NewContactWindow();
            newContactWindow.ShowDialog();
            ReadDatabase();
        }

        private void ReadDatabase()
        {
            using (var connection = new SQLiteConnection(App.dbPath))
            {
                connection.CreateTable<Contact>();
                Contacts = (connection.Table<Contact>().ToList()).OrderBy(c => c.Name).ToList();
            }

            if (Contacts != null)
            {
                ContactsListView.ItemsSource = Contacts;
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchTextBox = sender as TextBox;

            var filteredList = Contacts.Where(c => c.Name.ToLower().Contains(searchTextBox.Text.ToLower())).ToList();

            ContactsListView.ItemsSource = filteredList;
        }

        private void ContactsListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Contact selectedContact = (Contact) ContactsListView.SelectedItem;

            if (selectedContact != null)
            {
                var newContactWindow = new ContactDetailsWindow(selectedContact);
                newContactWindow.ShowDialog();

                ReadDatabase();
            }
        }
    }
}
