using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_App
{
    public interface IMyMusicDataBase
    {
        long NumberOfTables();
        ObservableCollection<Music> GetPlaylistTable(int i);
        bool InsertIntoTable(Music music, int i);
        bool DeleteFromMainTable(Music music);
        bool UpdateMainTable(Music music);
        bool CreateTable(Music music, int i);

    }
}
