using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Net.Http.Headers;
using System.ComponentModel;
using Newtonsoft.Json;

namespace WPF_App
{
    public class SignUpVM : INotifyPropertyChanged
    {
        public Music musicSearch { get; set; }
        public Music musicResult { get; set; }
        public ObservableCollection<Music> musicResultList { get; set; }
        public ICommand Search { get; private set; }
        public int index { get; set; }
        public Music SelectedMusic { get; set; }
        public SignUpVM()
        {
            musicSearch = new Music();
            musicResult = new Music();
            musicResultList = new ObservableCollection<Music>();
            musicSearch.Title = "Search Song";
            musicSearch.Artist = "Artist";
            musicResult.Title = "Result Here";
            BeginCommand();
        }

        public void BeginCommand()
        {
            Search = new RelayCommand(async (object _) =>
            {
                musicResultList.Clear();

                string title = musicSearch.Title;

                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://genius-song-lyrics1.p.rapidapi.com/search/?q={title}&per_page=10&page=1"),
                    Headers =
                    {
                        { "X-RapidAPI-Key", "insert here your key" },
                        { "X-RapidAPI-Host", "genius-song-lyrics1.p.rapidapi.com" },
                    },
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    dynamic bodyJson = JsonConvert.DeserializeObject(body);
                    for(int i = 0; i < bodyJson.hits.Count; i++)
                    {
                        if (bodyJson.hits[i].type == "song")
                        {
                            Music result = new Music();
                            result.Title = bodyJson.hits[i].result.full_title;
                            result.Artist = bodyJson.hits[i].result.artist_names;
                            result.Lyrics = bodyJson.hits[i].result.url;
                            musicResultList.Add(result);
                        }
                    }
                    
                }
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
