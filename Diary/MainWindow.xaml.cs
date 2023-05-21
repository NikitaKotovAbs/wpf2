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
        string fileName;

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
                listBox.Items.Add(note.NameNote);
            }
        }

        void ReFillerListBox(ListBox listBox, List<Note> notes, DateTime dateTime)
        {
            listBox.Items.Clear();
            foreach (Note note in notes)
            {
                if (note.DateNote.Date == dateTime.Date)
                    listBox.Items.Add(note.NameNote);
            }
        }

        void deleteNote(List<Note> notes, string name)
        {
            foreach (Note item in notes)
            {
                if (item.NameNote == name)
                {
                    notes.Remove(item);
                    break;
                }    
            }
            if (dpDataObject.Text != "")
            {
                
                ReFillerListBox(lbNameObjects, notes, DateTime.Parse(dpDataObject.Text));
                JSON.JsonWriter(path, fileName, notes);
            }
        }

        void updateNote(List<Note> notes, string name)
        {
            foreach (Note item in notes)
            {
                if (item.NameNote == name)
                {
                    if (dpDataObject.Text == "")
                    {
                        MessageBox.Show("Выбирете дату для заметки!");
                    }
                    else
                    {
                        if (tbNameObject.Text.Trim() == "" && tbDescriptionObject.Text.Trim() == "")
                            MessageBox.Show("Все данные об оьъекте должны быть заполнены!");
                        else
                        {
                            Note note = notes.Find(x => x.NameNote == name);
                            note.NameNote = tbNameObject.Text;
                            note.DescriptionNote = tbDescriptionObject.Text;
                            note.DateNote = DateTime.Parse(dpDataObject.Text);

                            ReFillerListBox(lbNameObjects, notes, note.DateNote);
                            JSON.JsonWriter(path, fileName, notes);
                        }
                    }
                    break;
                }
            }
        }

        bool IsExistNoteWithName(List<Note> notes, string name)
        {
            foreach (Note item in notes)
                if (item.NameNote == name)
                    return true;
            return false;
        }

        Note getFilterNote(List<Note> notes, string name)
        {
            if (notes.Count != 0)
                foreach (Note note in notes)
                    if (note.NameNote == name)
                        return note;
            return null;
        }


        public MainWindow()
        {
            InitializeComponent();

            fileName = "File.json";
            path = Directory.GetCurrentDirectory() + "/";

            JsonFileManager(path, fileName);

            notes = JSON.JsonReader(notes, path, fileName);

            dpDataObject.Text = DateTime.Now.ToString();
            ReFillerListBox(lbNameObjects, notes, DateTime.Now);

        }

        private void btDeleteObject_Click(object sender, RoutedEventArgs e)
        {
            if(lbNameObjects.SelectedIndex > -1)
            {
                deleteNote(notes, lbNameObjects.SelectedItem.ToString());
            }
            else
            {
                MessageBox.Show("Следует выбрать запись!");
            }
        }

        private void tbCreateObject_Click(object sender, RoutedEventArgs e)
        {
            if (dpDataObject.Text == "")
            {
                MessageBox.Show("Выбирете дату для заметки!");
            }
            else
            {
                if (tbNameObject.Text.Trim() == "" && tbDescriptionObject.Text.Trim() == "")
                    MessageBox.Show("Все данные об оьъекте должны быть заполнены!");
                else
                {
                    if (IsExistNoteWithName(notes, tbNameObject.Text.Trim()))
                    {
                        MessageBox.Show("Объект уже существует в системе!");
                    }
                    else
                    {
                        Note note = new Note();
                        note.NameNote = tbNameObject.Text.Trim();
                        note.DescriptionNote = tbDescriptionObject.Text.Trim();
                        note.DateNote = DateTime.Parse(dpDataObject.Text).Date;
                        notes.Add(note);

                        ReFillerListBox(lbNameObjects, notes, note.DateNote);
                        JSON.JsonWriter(path, fileName, notes);

                        tbNameObject.Clear();
                        tbDescriptionObject.Clear();
                        MessageBox.Show("Объект добавлен!");
                    }
                }
            }

        }

        private void btSaveObject_Click(object sender, RoutedEventArgs e)
        {
            if (lbNameObjects.SelectedIndex > -1)
            {
                updateNote(notes, lbNameObjects.SelectedItem.ToString());
            }
            else
            {
                MessageBox.Show("Следует выбрать запись!");
            }
        }

        private void lbNameObjects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbNameObjects.SelectedIndex != -1)
            {
                string s = lbNameObjects.SelectedItem.ToString();
                Note note = getFilterNote(notes, s);
                tbNameObject.Text = note.NameNote;
                tbDescriptionObject.Text = note.DescriptionNote;
                dpDataObject.Text = note.DateNote.ToString();
            }
        }

        private void dpDataObject_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if  (notes.Count != 0)
            {
                ReFillerListBox(lbNameObjects, notes, DateTime.Parse(dpDataObject.Text));
            }
        }
    }
}
