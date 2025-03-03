using System;
using System.Linq;
using System.Windows;
using System.Collections.Generic;
using System.DirectoryServices;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        private int idCounter = 1;
        private const int N = 25;
        private Person[] people = new Person[N];
        private int count = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddPerson_Click(object sender, RoutedEventArgs e)
        {
            if (count >= N)
            {
                MessageBox.Show("Storage is full.");
                return;
            }

            string name = NameInput.Text;
            int age;
            if (!int.TryParse(AgeInput.Text, out age))
            {
                MessageBox.Show("Invalid age input.");
                return;
            }

            people[count] = new Person { Id = idCounter++, Name = name, Age = age };
            count++;
            DisplayPeople();
        }

        private void DisplayPeople()
        {
            PeopleList.Items.Clear();
            for (int i = 0; i < count; i++)
            {
                PeopleList.Items.Add($"{people[i].Id}: {people[i].Name}, {people[i].Age}");
            }
        }

        private void SortByAge_Click(object sender, RoutedEventArgs e)
        {
            Array.Sort(people, 0, count, Comparer<Person>.Create((a, b) => a.Age.CompareTo(b.Age)));
            DisplayPeople();
        }

        private void SortByName_Click(object sender, RoutedEventArgs e)
        {
            Array.Sort(people, 0, count, Comparer<Person>.Create((a, b) => a.Name.CompareTo(b.Name)));
            DisplayPeople();
        }

        private void SearchByAge_Click(object sender, RoutedEventArgs e)
        {
            int searchAge;
            if (!int.TryParse(SearchAgeInput.Text, out searchAge))
            {
                MessageBox.Show("Invalid age input.");
                return;
            }
            var results = people.Take(count).Where(p => p.Age == searchAge).ToList();
            DisplaySearchResults(results);
        }

        private void SearchByName_Click(object sender, RoutedEventArgs e)
        {
            string searchName = SearchNameInput.Text;
            var results = people.Take(count).Where(p => p.Name.Equals(searchName, StringComparison.OrdinalIgnoreCase)).ToList();
            DisplaySearchResults(results);
        }

        private void DisplaySearchResults(List<Person> results)
        {
            SearchResults.Items.Clear();
            foreach (var person in results)
            {
                SearchResults.Items.Add($"{person.Id}: {person.Name}, {person.Age}");
            }
        }

        private void RemoveByName_Click(object sender, RoutedEventArgs e)
        {
            string nameToRemove = RemoveNameInput.Text;
            int index = Array.IndexOf(people, people.Take(count).FirstOrDefault(p => p.Name.Equals(nameToRemove, StringComparison.OrdinalIgnoreCase)));
            if (index != -1)
            {
                for (int i = index; i < count - 1; i++)
                {
                    people[i] = people[i + 1];
                }
                count--;
                DisplayPeople();
            }
            else
            {
                MessageBox.Show("Name not found.");
            }
        }

        private void RemoveByAge_Click(object sender, RoutedEventArgs e)
        {
            int ageToRemove;
            if (!int.TryParse(RemoveAgeInput.Text, out ageToRemove))
            {
                MessageBox.Show("Invalid age input.");
                return;
            }

            int index = Array.IndexOf(people, people.Take(count).FirstOrDefault(p => p.Age == ageToRemove));
            if (index != -1)
            {
                for (int i = index; i < count - 1; i++)
                {
                    people[i] = people[i + 1];
                }
                count--;
                DisplayPeople();
            }
            else
            {
                MessageBox.Show("Age not found.");
            }
        }
    }

    public struct Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}