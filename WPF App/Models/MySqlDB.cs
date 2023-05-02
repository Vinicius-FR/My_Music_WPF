using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Runtime.Remoting.Contexts;
using System.Configuration;
using MySql.Data.MySqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Collections.ObjectModel;

namespace WPF_App
{
    public class MySqlDB : IMyMusicDataBase
    {
        public MySqlConnection connection { get; private set; }
        private MySqlCommand command { get; set; }
        private string dbName { get; set; }
        public MySqlDB()
        {
            dbName = "mymusic_db";
            connection = new MySqlConnection($"server=localhost;database={dbName};user id=root;password=senha");
            command = connection.CreateCommand();
        }

        public MySqlDB(string connectionString, string DbName)
        {
            dbName = DbName;
            connection = new MySqlConnection(connectionString);
            command = connection.CreateCommand();
        }

        public long NumberOfTables()
        {
            connection.Open();
            command.CommandText = $"SELECT COUNT(*) as total_tables FROM information_schema.tables WHERE table_schema = '{dbName}';";
            DataTable dt = new DataTable();
            dt.Load(command.ExecuteReader());
            connection.Close();

            DataRow dr = dt.Rows[0];

            return dr.Field<long>("total_tables");
        }

        public ObservableCollection<Music> GetPlaylistTable(int i)
        {
            connection.Open();
            command.CommandText = $"SELECT Title, Artist, Lyrics FROM Playlist{i};";
            DataTable newDt = new DataTable();
            newDt.Load(command.ExecuteReader());
            connection.Close();

            ObservableCollection<Music> musicList = new ObservableCollection<Music>();

            foreach (DataRow row in newDt.Rows)
            {
                Music song = new Music();
                song.Title = row.Field<string>("Title");
                song.Artist = row.Field<string>("Artist");
                song.Lyrics = row.Field<string>("Lyrics");

                musicList.Add(song);

            }

            return musicList;
        }

        public bool InsertIntoTable(Music music, int i)
        {
            try
            {
                connection.Open();
                command.CommandText = $"INSERT INTO Playlist{i} (Title, Artist, Lyrics) VALUES (@valor1, @valor2, @valor3)";

                command.Parameters.AddWithValue("@valor1", music.Title);
                command.Parameters.AddWithValue("@valor2", music.Artist);
                command.Parameters.AddWithValue("@valor3", music.Lyrics);
                command.ExecuteNonQuery();
                command.Parameters.Clear();
                connection.Close();
                
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\nCouldn't insert data to database!!");
            }
            
        }

        public bool DeleteFromMainTable(Music music)
        {
            try
            {
                connection.Open();
                command.CommandText = "DELETE FROM Playlist0 WHERE (Title = @valor);";

                command.Parameters.AddWithValue("@valor", music.Title);
                command.ExecuteNonQuery();
                command.Parameters.Clear();
                connection.Close();

                return true;
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message + "\nCouldn't delete data from database!!");
            }
            
        }

        public bool UpdateMainTable(Music music)
        {
            try
            {
                connection.Open();
                command.CommandText = "UPDATE Playlist0 SET Title = @valor1, Artist = @valor2 WHERE (Lyrics = @valor3);";

                command.Parameters.AddWithValue("@valor1", music.Title);
                command.Parameters.AddWithValue("@valor2", music.Artist);
                command.Parameters.AddWithValue("@valor3", music.Lyrics);
                command.ExecuteNonQuery();
                command.Parameters.Clear();
                connection.Close();

                return true;
            }
            catch( Exception ex)
            {
                throw new Exception(ex.Message + "\nCouldn't update data in database!!");
            }
            
        }

        public bool CreateTable(Music music, int i)
        {
            try
            {
                connection.Open();
                command.CommandText = $"CREATE TABLE IF NOT EXISTS Playlist{i} (Title VARCHAR(255), Artist VARCHAR(255), Lyrics VARCHAR(255)); " +
                                      $"INSERT INTO Playlist{i} (Title, Artist, Lyrics) VALUES (@valor1, @valor2, @valor3)";

                command.Parameters.AddWithValue("@valor1", music.Title);
                command.Parameters.AddWithValue("@valor2", music.Artist);
                command.Parameters.AddWithValue("@valor3", music.Lyrics);
                command.ExecuteNonQuery();
                command.Parameters.Clear();
                connection.Close();

                return true;
            }
            catch( Exception ex)
            {
                throw new Exception(ex.Message + "\nCouldn't create table in database!!");
            }
            
        }
    }

}
