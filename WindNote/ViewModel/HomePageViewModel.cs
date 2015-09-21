using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WindNote.DataAccessLayer;
using WindNote.DataAccessLayer.Context;
using WindNote.DataAccessLayer.Repositories;
using WindNote.Events;
using WindNote.Events.Commands;
using WindNote.Gui.CustomControls;
using WindNote.Model;
using WindNote.Mvvm;
using WindNote.MvvmBase;

namespace WindNote.ViewModel
{

    public class HomePageViewModel : NotifyPropertyChangedViewModel
    {
        #region ViewModel Data Bindings
        private IRepositoryFactory RepositoryFactory;
        private IEntityFactory EntityFactory;

        private BindableObservableCollection<Note> _notes;
        public BindableObservableCollection<Note> Notes
        {
            get { return _notes; }
            set
            {
                _notes = value;
                this.RaisePropertyChangedEvent("Notes");
            }
        }

        private Note _comboBoxSelectedNote = null;
        public Note ComboBoxSelectedNote
        {
            get { return _comboBoxSelectedNote; }
            set
            {
                if (value != null)
                {
                    this.UnsubscribeComboBoxSelectedNotePropertyChanged();
                    this.UpdateActivitiesInDatabase();
                    _comboBoxSelectedNote = value;
                    this.SubscribeComboBoxSelectedNotePropertyChanged();
                    this.GetActivitiesSelectedNote();
                    this.RaisePropertyChangedEvent("ComboBoxSelectedNote");
                    this.RaisePropertyChangedEvent("ComboBoxSelectedNoteText");
                }
            }
        }
        public String ComboBoxSelectedNoteText
        {
            get { return ComboBoxSelectedNote != null ? ComboBoxSelectedNote.Title : null; }
            set { ComboBoxSelectedNote.Title = value; }
        }

        private BindableObservableCollection<Activity> _activities;
        public BindableObservableCollection<Activity> Activities
        {
            get { return _activities; }
            set
            {
                _activities = value;
                this.RaisePropertyChangedEvent("Activities");
            }
        }

        private Activity _listViewSelectedActivity = null;
        public Activity ListViewSelectedActivity
        {
            get { return _listViewSelectedActivity; }
            set
            {
                if (value != null)
                {
                    _listViewSelectedActivity = value;
                    this.RaisePropertyChangedEvent("ListViewSelectedActivity");
                }
            }
        }

        #endregion

        #region ViewModel Initialization

        public HomePageViewModel(IEventAggregator eventAggregator,
                                 IApplicationDispatcher dispatcher,
                                 IRepositoryFactory repositoryFactory,
                                 IEntityFactory entityFactory) :  base(eventAggregator, dispatcher)
        {
            this.RepositoryFactory = repositoryFactory;
            this.EntityFactory = entityFactory;
            this.SubscribeToEvents();
            this.GetNotesAsync();
        }

        public void SubscribeToEvents()
        {
            this.EventAggregator.GetEvent<ApplicationExitEvent>().Subscribe(HandleApplicationExitEvent);
            this.EventAggregator.GetEvent<UpdateDatabaseEvent>().Subscribe(HandleUpdateDatabaseEvent);
        }

        public void GetNotesAsync()
        {
            Task task = new Task(() =>
            {
                GetNotes();
            });
            this.ExecuteTaskGetNotesAsync(task);
        }

        public async void ExecuteTaskGetNotesAsync(Task task)
        {
            task.Start();
            await task;
        }

        public void GetNotes()
        {
            NotesRepository notesRepository = this.RepositoryFactory.CreateNotesRepository();
            List<Note> notes = notesRepository.GetAll();
            notes = notes.OrderBy(note => note.PositionOnList).ToList();
            this.Notes = new BindableObservableCollection<Note>(notes);
            this.Notes.CollectionChanged += Notes_CollectionChanged;

            if (this.Notes.Count == 0)
            {
                this.AppendNoteUIThread();
            }
            else
            {
                this.SetLastUsedNote();
            }
        }

        public void AppendNoteUIThread()
        {
            this.Dispatcher.Invoke(this.AppendNoteAndSelect);
        }

        public void SetLastUsedNote()
        {
            int noteId = Properties.Settings.Default.LastUsedNoteId;
            this.ComboBoxSelectedNote = this.Notes.FirstOrDefault(note => note.Id == noteId) ?? this.Notes.First();
        }

        public void GetActivitiesSelectedNote()
        {
            ActivitiesRepository activitiesRepository = this.RepositoryFactory.CreateActivitiesRepository();
            List<Activity> activities = activitiesRepository.FindActivitiesOfNote(this.ComboBoxSelectedNote.Id);
            activities = activities.OrderBy(activity => activity.PositionOnList).ToList();
            this.Activities = new BindableObservableCollection<Activity>(activities);
            this.Activities.CollectionChanged += Activities_CollectionChanged;
        }

        #endregion

        #region ViewModel Command Bindings

        public ICommand PrependActivityAndSelectCommand
        {
            get { return new DelegateCommand(PrependActivityAndSelect); }
        }

        public ICommand AppendActivityAndSelectCommand
        {
            get { return new DelegateCommand(AppendActivityAndSelect); }
        }

        public ICommand InsertActivityAndSelectCommand
        {
            get { return new DelegateCommand(InsertActivityAndSelect); }
        }

        public ICommand RemoveActivityAndSelectCommand
        {
            get { return new DelegateCommand(RemoveActivityAndSelect); }
        }

        public ICommand AppendNoteAndSelectCommand
        {
            get { return new DelegateCommand(AppendNoteAndSelect); }
        }

        public ICommand RemoveNoteAndSelectCommand
        {
            get { return new DelegateCommand(RemoveNoteAndSelect); }
        }

        public ICommand SelectPreviousActivityCommand
        {
            get { return new DelegateCommand(SelectPreviousActivity); }
        }

        public ICommand SelectNextActivityCommand
        {
            get { return new DelegateCommand(SelectNextActivity); }
        }

        public ICommand MoveActivityToNoteCommand
        {
            get { return new DelegateCommand(MoveActivityToNote); }
        }

        public ICommand CopyActivityToNoteCommand
        {
            get { return new DelegateCommand(CopyActivityToNote); }
        }

        public ICommand MoveSelectedActivityUpCommand
        {
            get { return new DelegateCommand(MoveSelectedActivityUp); }
        }

        public ICommand MoveSelectedActivityDownCommand
        {
            get { return new DelegateCommand(MoveSelectedActivityDown); }
        }

        private void PrependActivityAndSelect(object parameter)
        {
            Activity activity = this.InsertNewActivity(0);
            this.ListViewSelectedActivity = activity;
        }

        private void AppendActivityAndSelect(object parameter)
        {
            Activity activity = this.InsertNewActivity(this.Activities.Count);
            this.ListViewSelectedActivity = activity;
        }

        private void InsertActivityAndSelect(object parameter)
        {
            Activity activity = (Activity) parameter;
            Activity newActivity = this.InsertNewActivity(activity.PositionOnList + 1);
            this.ListViewSelectedActivity = newActivity;
        }

        private Activity InsertNewActivity(int position)
        {
            Activity newActivity = EntityFactory.CreateActivity(this.ComboBoxSelectedNote.Id);
            return this.InsertActivity(newActivity, position);
        }

        private Activity InsertActivity(Activity activity, int position)
        {
            ActivitiesRepository activitiesRepository = this.RepositoryFactory.CreateActivitiesRepository();
            this.Activities.Insert(position, activity);
            activitiesRepository.Insert(activity);
            return activity;
        }

        private void RemoveActivityAndSelect(object parameter)
        {
            Activity activity = (Activity) parameter;
            int removedActivityPosition = activity.PositionOnList;
            this.RemoveActivity(activity);
            this.SelectActivityAfterRemoved(removedActivityPosition);
        }

        private void RemoveActivity(Activity activity)
        {
            ActivitiesRepository activitiesRepository = this.RepositoryFactory.CreateActivitiesRepository();
            this.Activities.Remove(activity);
            activitiesRepository.Remove(activity);
        }

        private void SelectActivityAfterRemoved(int removedActivityPosition)
        {
            if (this.Activities.Count > 0)
            {
                this.ListViewSelectedActivity = removedActivityPosition < this.Activities.Count ?
                                                this.Activities[removedActivityPosition] : this.Activities.Last();
            }
        }

        private void AppendNoteAndSelect(object parameter)
        {
            this.AppendNoteAndSelect();
        }

        private void AppendNoteAndSelect()
        {
            Note note = this.AppendNote();
            this.ComboBoxSelectedNote = note;
        }

        private Note AppendNote()
        {
            NotesRepository notesRepository = this.RepositoryFactory.CreateNotesRepository();
            Note note = EntityFactory.CreateNote();
            this.Notes.Add(note);
            notesRepository.Insert(note);
            return note;
        }

        private void RemoveNoteAndSelect(object parameter)
        {
            Note note = this.ComboBoxSelectedNote;
            int removedNoteIndex = this.Notes.IndexOf(note);
            this.RemoveNote(note);
            this.Activities = null;
            this.SelectNoteAfterRemoved(removedNoteIndex);
        }

        private void RemoveNote(Note note)
        {
            NotesRepository notesRepository = this.RepositoryFactory.CreateNotesRepository();
            this.Notes.Remove(note);
            notesRepository.Remove(note);
        }

        private void SelectNoteAfterRemoved(int removedNoteIndex)
        {
            if (this.Notes.Count == 0)
            {
                this.AppendNoteAndSelect();
            }
            else
            {
                this.ComboBoxSelectedNote = removedNoteIndex == 0 ?
                                            this.Notes.First() : this.Notes[removedNoteIndex - 1];
            }
        }

        private void SelectPreviousActivity(object paramter)
        {
            int index = this.ListViewSelectedActivity.PositionOnList - 1;
            this.ListViewSelectedActivity = index >= 0 ?
                                            this.Activities[index] : this.Activities.Last();
        }

        private void SelectNextActivity(object paramter)
        {
            int index = this.ListViewSelectedActivity.PositionOnList + 1;
            this.ListViewSelectedActivity = index < this.Activities.Count() ?
                                            this.Activities[index] : this.Activities.First();
        }

        private void MoveActivityToNote(object parameter)
        {
            this.CopyActivityToNote(parameter);
            object[] parameters = (object[])parameter;
            Activity activity = (Activity)parameters[0];
            this.RemoveActivity(activity);
        }

        private void CopyActivityToNote(object parameter)
        {
            object[] parameters = (object[])parameter;
            Activity activity = (Activity)parameters[0];
            Note note = (Note)parameters[1];
            Activity copy = this.EntityFactory.CreateActivity(note.Id);
            copy.Name = activity.Name;
            this.InsertCopyActivityAndSelect(copy, activity);
        }

        private void InsertCopyActivityAndSelect(Activity copy, Activity activity)
        {
            if (this.ComboBoxSelectedNote.Id == copy.IdNote)
            {
                this.InsertActivity(copy, activity.PositionOnList + 1);
                this.ListViewSelectedActivity = copy;
            }
            else
            {
                ActivitiesRepository activitiesRepository = this.RepositoryFactory.CreateActivitiesRepository();
                activitiesRepository.IncreasePositionsOnListAndInsert(copy);
                this.ListViewSelectedActivity = activity;
            }
        }

        private void MoveSelectedActivityDown(object parameter)
        {
            int index = this.ListViewSelectedActivity.PositionOnList;
            int newIndex = index + 1;
            if (newIndex < this.Activities.Count)
            {
                this.Activities.Move(index, newIndex);
            }
        }

        private void MoveSelectedActivityUp(object parameter)
        {
            int index = this.ListViewSelectedActivity.PositionOnList;
            int newIndex = index - 1;
            if (newIndex >= 0)
            {
                this.Activities.Move(index, newIndex);
            }
        }

        #endregion

        #region ViewModel Event Handling

        public void Activities_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (this.ItemsInsertedToCollection(e))
            {
                foreach (Activity insertedActivity in e.NewItems)
                {
                    this.UpdateActivitiesPositions();
                }
            }

            if (this.ItemsRemovedFromCollection(e))
            {
                foreach (Activity removedActivity in e.OldItems)
                {
                    this.UpdateActivitiesPositions();
                }
            }
        }

        private bool ItemsInsertedToCollection(NotifyCollectionChangedEventArgs e)
        {
            return e.NewItems != null;
        }

        private bool ItemsRemovedFromCollection(NotifyCollectionChangedEventArgs e)
        {
            return e.OldItems != null;
        }

        private void UpdateActivitiesPositions()
        {
            for (int i = 0; i < this.Activities.Count; ++i)
            {
                Activity activity = this.Activities[i];
                activity.PositionOnList = i;
            }
        }

        public void Notes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (this.ItemsInsertedToCollection(e))
            {
                foreach (Note insertedNote in e.NewItems)
                {
                    this.UpdateNotesPositions();
                }
            }

            if (this.ItemsRemovedFromCollection(e))
            {
                foreach (Note removedNote in e.OldItems)
                {
                    this.UpdateNotesPositions();
                }
            }
        }

        private void UpdateNotesPositions()
        {
            for (int i = 0; i < this.Notes.Count; ++i)
            {
                Note note = this.Notes[i];
                note.PositionOnList = i;
            }
        }

        private void UnsubscribeComboBoxSelectedNotePropertyChanged()
        {
            if (this.ComboBoxSelectedNote != null)
            {
                this.ComboBoxSelectedNote.PropertyChanged -= HandleSelectedComboBoxNoteChanged;
            }
        }

        private void SubscribeComboBoxSelectedNotePropertyChanged()
        {
            if (this.ComboBoxSelectedNote != null)
            {
                this.ComboBoxSelectedNote.PropertyChanged += HandleSelectedComboBoxNoteChanged;
            }
        }

        private void HandleSelectedComboBoxNoteChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Title")
            {
                this.RaisePropertyChangedEvent("ComboBoxSelectedNoteText");
            }
        }

        #endregion

        #region ViewModel EventAggregator Handlers

        private void HandleApplicationExitEvent(object message)
        {
            Properties.Settings.Default.LastUsedNoteId = this.ComboBoxSelectedNote.Id;
            Properties.Settings.Default.Save();
            this.UpdateDatabase();
        }

        private void HandleUpdateDatabaseEvent(object message)
        {
            this.UpdateDatabase();
        }

        private void UpdateDatabase()
        {
            this.UpdateActivitiesInDatabase();
            this.UpdateNotesInDatabase();
        }

        private void UpdateActivitiesInDatabase()
        {
            if(this.Activities != null)
            {
                ActivitiesRepository activitiesRepository = this.RepositoryFactory.CreateActivitiesRepository();
                List<Activity> activities = this.Activities.ToList<Activity>();
                activitiesRepository.Update(activities);
            }
        }

        private void UpdateNotesInDatabase()
        {
            if (this.Notes != null)
            {
                NotesRepository notesRepository = this.RepositoryFactory.CreateNotesRepository();
                List<Note> notes = this.Notes.ToList<Note>();
                notesRepository.Update(notes);
            }
        }

        #endregion

    }

}
