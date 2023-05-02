using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using System.Data;
using System.Threading;
using System.ComponentModel;
using System.IO.Packaging;

namespace WPF_App
{
    public class MainWindowsVM : INotifyPropertyChanged
    {
        public ObservableCollection<Music> musicList { get; set; }
        public ObservableCollection<ObservableCollection<Music>> playlists { get; set; }
        public ObservableCollection<Music> SelectedPlaylist { get; set; }
        public int SelectedPlaylistIndex { get; set; }
        //public Music SelectedMusic { get; set; }
        public Music SelectedMusic
        {
            get { return _music; }
            set { _music = value; Notify(nameof(SelectedMusic)); }
        }
        private Music _music;
        private MySqlDB dbContext { get; set; }
        public ICommand Add { get; private set; }
        public ICommand Remove { get; private set; }
        public ICommand Lyrics { get; private set; }
        public ICommand Update { get; private set; }
        public ICommand Create { get; private set; }
        public ICommand AddToPlaylist { get; private set; }
        public ICommand Show { get; private set; }
        
        public MainWindowsVM()
        {
            dbContext = new MySqlDB();
            playlists = new ObservableCollection<ObservableCollection<Music>>();
            musicList = new ObservableCollection<Music>();

            long n = dbContext.NumberOfTables();

            for (int i = 0; i < n; i++)
            {
                playlists.Add(dbContext.GetPlaylistTable(i));
            }

            musicList = playlists.FirstOrDefault();
            
            BeginCommand();
        }

        public void BeginCommand()
        {
            Add = new RelayCommand((object _) =>
            {
                Music music = new Music();
                SignUpVM signUpVM = new SignUpVM();

                SignUp signUp = new SignUp();
                signUp.DataContext = signUpVM;
                bool? verify = signUp.ShowDialog();

                if (verify.HasValue && verify.Value)
                {
                    music = signUpVM.SelectedMusic;

                    musicList.Add(music);

                    dbContext.InsertIntoTable(music, 0);
                }
            });
            Remove = new RelayCommand((object _) =>
            {
                dbContext.DeleteFromMainTable(SelectedMusic);
                musicList.Remove(SelectedMusic);
            });
            Lyrics = new RelayCommand((object _) =>
            {
                Process.Start(SelectedMusic.Lyrics);
            });
            Update = new RelayCommand((object _) =>
            {
                Update update = new Update();
                update.DataContext = SelectedMusic;

                bool? verify = update.ShowDialog();

                if (verify.HasValue && verify.Value)
                {
                    Notify("SelectedMusic");

                    dbContext.UpdateMainTable(SelectedMusic);
                }
            });
            Create = new RelayCommand((object _) =>
            {
                ObservableCollection<Music> newPlaylist = new ObservableCollection<Music>
                {
                    SelectedMusic
                };
                playlists.Add(newPlaylist);

                dbContext.CreateTable(SelectedMusic, playlists.Count() - 1);
            });
            AddToPlaylist = new RelayCommand((object _) =>
            {
                Notify("SelectedPlaylistIndex");
                playlists[SelectedPlaylistIndex].Add(SelectedMusic);

                dbContext.InsertIntoTable(SelectedMusic, SelectedPlaylistIndex);
            });
            Show = new RelayCommand((object _) => {
                Notify("SelectedPlaylistIndex");
                if (SelectedPlaylistIndex < playlists.Count())
                {
                    SelectedPlaylist = playlists[SelectedPlaylistIndex];
                }
                Notify("SelectedPlaylist");
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.  
        // The CallerMemberName attribute that is applied to the optional propertyName  
        // parameter causes the property name of the caller to be substituted as an argument.  
        private void Notify(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
