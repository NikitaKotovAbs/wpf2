using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Diary
{
    public partial class MainWindow : Window
    {
        string path;
        string file;

        List<Note> notes;


        void JsonFileManager(string path, string fileName)
        {
            if (!File.Exists(path + fileName))
            {
                File.Create(path + fileName);
            }
        }


        void ReFillerListBox(ListBox listBox, List<Note> notes)
        {
            listBox.Items.Clear();
            foreach (Note note in notes)
            {
                listBox.Items.Add(note.NN);
            }
        }

        void ReFillerListBox(ListBox listBox, List<Note> notes, DateTime dateTime)
        {
            listBox.Items.Clear();
            foreach (Note note in notes)
            {
                if (note.Date.Date == dateTime.Date)
                    listBox.Items.Add(note.NN);
            }
        }

        void deleteNote(List<Note> notes, string name)
        {
            foreach (Note item in notes)
            {
                if (item.NN == name)
                {
                    notes.Remove(item);
                    break;
                }    
            }
            if (Data.Text != "")
            {
                ReFillerListBox(ListboxXML, notes, DateTime.Parse(Data.Text));
                JSON.JsonWriter(path, file, notes);
            }
        }

        void updateNote(List<Note> notes, string name)
        {
            foreach (Note item in notes)
            {
                if (item.NN == name)
                {
                    if (Data.Text == "")
                    {
                        MessageBox.Show("Выбирете дату для заметки!");
                    }
                    else
                    {
                        if (Txtbox.Text.Trim() == "" && Txtbox2.Text.Trim() == "")
                            MessageBox.Show("Все данные об оьъекте должны быть заполнены!");
                        else
                        {
                            Note note = notes.Find(x => x.NN == name);
                            note.NN = Txtbox.Text;
                            note.Description = Txtbox2.Text;
                            note.Date = DateTime.Parse(Data.Text);

                            ReFillerListBox(ListboxXML, notes, note.Date);
                            JSON.JsonWriter(path, file, notes);
                        }
                    }
                    break;
                }
            }
        }

        bool IsExistNoteWithName(List<Note> notes, string name)
        {
            foreach (Note item in notes)
                if (item.NN == name)
                    return true;
            return false;
        }

        Note getFilterNote(List<Note> notes, string name)
        {
            if (notes.Count != 0)
                foreach (Note note in notes)
                    if (note.NN == name)
                        return note;
            return null;
        }


        public MainWindow()
        {
            InitializeComponent();

            file = "File.json";
            path = Directory.GetCurrentDirectory() + "/";

            JsonFileManager(path, file);

            notes = JSON.JsonReader(notes, path, file);

            Data.Text = DateTime.Now.ToString();
            ReFillerListBox(ListboxXML, notes, DateTime.Now);

        }

        private void Dell_Click(object sender, RoutedEventArgs e)
        {
            if(ListboxXML.SelectedIndex > -1)
            {
                deleteNote(notes, ListboxXML.SelectedItem.ToString());
            }
            else
            {
                MessageBox.Show("Следует выбрать запись!");
            }
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (Data.Text == "")
            {
                MessageBox.Show("Выбирете дату для заметки!");
            }
            else
            {
                if (Txtbox.Text.Trim() == "" && Txtbox2.Text.Trim() == "")
                    MessageBox.Show("Все данные об оьъекте должны быть заполнены!");
                else
                {
                    if (IsExistNoteWithName(notes, Txtbox.Text.Trim()))
                    {
                        MessageBox.Show("Объект уже существует в системе!");
                    }
                    else
                    {
                        Note note = new Note();
                        note.NN = Txtbox.Text.Trim();
                        note.Description = Txtbox2.Text.Trim();
                        note.Date = DateTime.Parse(Data.Text).Date;
                        notes.Add(note);

                        ReFillerListBox(ListboxXML, notes, note.Date);
                        JSON.JsonWriter(path, file, notes);

                        Txtbox.Clear();
                        Txtbox2.Clear();
                        MessageBox.Show("Объект добавлен!");
                    }
                }
            }

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (ListboxXML.SelectedIndex > -1)
            {
                updateNote(notes, ListboxXML.SelectedItem.ToString());
            }
            else
            {
                MessageBox.Show("Следует выбрать запись!");
            }
        }

        private void ListboxXML_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListboxXML.SelectedIndex != -1)
            {
                string s = ListboxXML.SelectedItem.ToString();
                Note note = getFilterNote(notes, s);
                Txtbox.Text = note.NN;
               Txtbox2.Text = note.Description;
                Data.Text = note.Date.ToString();
            }
        }

        private void Data_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if  (notes.Count != 0)
            {
                ReFillerListBox(ListboxXML, notes, DateTime.Parse(Data.Text));
            }
        }
    }
}
