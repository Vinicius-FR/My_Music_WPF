using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_App
{
    public class Music : INotifyPropertyChanged
    {
        [Key]
        public int Id { get; set; }
        private string title;
        private string artist;
        private string lyrics;

        public event PropertyChangedEventHandler PropertyChanged;

        public Music() { }

        public Music(string title, string artist, string lyrics)
        {
            this.title = title;
            this.artist = artist;
            this.lyrics = lyrics;
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Artist
        {
            get { return artist; }
            set { artist = value; }
        }

        public string Lyrics
        {
            get { return lyrics; }
            set { lyrics = value; Notify(nameof(Lyrics)); }
        }

        private void Notify(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
