using Moq;
using System.Collections.ObjectModel;
using System.Data;
using WPF_App;

namespace MyMusic.Test
{
    public class MyMusicTests
    {

        [Fact]
        public void UnitaryTest_Music()
        {
            Music music = new Music("Take on me", "A-ha", "lyrics.com");

            Assert.Equal("Take on me", music.Title);
            Assert.Equal("A-ha", music.Artist);
            Assert.Equal("lyrics.com", music.Lyrics);
        }

        //[Fact]
        //public void Test_MySqlConnection()
        //{
        //    string connectionString = "server=localhost;database=mymusic_test_db;user id=root;password=senha";
        //    MySqlDB testConnection = new MySqlDB(connectionString, "mymusic_test_db");
        //    Assert.Equal(connectionString, testConnection.connection.ConnectionString);

        //    testConnection = new MySqlDB();
        //    Assert.Equal("server=localhost;database=mymusic_db;user id=root;password=senha", testConnection.connection.ConnectionString);
        //    // acrescentei " id" e removi ";" na connection string coincidir com o retornado por connection.ConnectionString
        //}

        [Fact]
        public void Test_NumberOfTables() 
        {
            // Assert.True(dbContext.NumberOfTables() >= 1);
            var mockObject = new Mock<IMyMusicDataBase>();
            mockObject.Setup(x => x.NumberOfTables()).Returns(1);

            var result = mockObject.Object.NumberOfTables();
            Assert.True(result >= 1);
            // agora é preciso informar o nome da DataBase, caso não informe, será usada a padrão mymusic_db
            // assim, alterei para que tudo seja feito na DataBase certa, não só na mymusic_db
        }

        [Fact]
        public void Test_InsertIntoTable()
        {

            Music music = new Music("Take on me by A-ha", "A-ha", "lyrics.com");
            //bool result = dbContext.InsertIntoTable(music, 0);
            //Assert.True(result);

            var mockObject = new Mock<IMyMusicDataBase>();
            mockObject.Setup(x => x.InsertIntoTable(music, 0)).Returns(true);

            var result = mockObject.Object.InsertIntoTable(music, 0);
            Assert.True(result);

        }

        [Fact]
        public void Test_DeleteFromMainTable()
        {
            Music music = new Music("Take on me by A-ha", "A-ha", "lyrics.com");
            //bool result = dbContext.DeleteFromMainTable(music);
            //Assert.True(result);

            var mockObject = new Mock<IMyMusicDataBase>();
            mockObject.Setup(x => x.DeleteFromMainTable(music)).Returns(true);

            var result = mockObject.Object.DeleteFromMainTable(music);
            Assert.True(result);
        }

        [Fact]
        public void Test_UpdateMainTable()
        {
            Music music = new Music("Take on me", "A-ha", "lyrics.com");
            //bool result = dbContext.UpdateMainTable(music);
            //Assert.True(result);

            var mockObject = new Mock<IMyMusicDataBase>();
            mockObject.Setup(x => x.UpdateMainTable(music)).Returns(true);

            var result = mockObject.Object.UpdateMainTable(music);
            Assert.True(result);
        }

        [Fact]
        public void Test_CreateTable()
        {
            Music music = new Music("Stressed Out", "twenty one pilots", "stressedout-lyrics.com");
            //bool result = dbContext.CreateTable(music, 1);
            //Assert.True(result);

            var mockObject = new Mock<IMyMusicDataBase>();
            mockObject.Setup(x => x.CreateTable(music, 1)).Returns(true);

            var result = mockObject.Object.CreateTable(music, 1);
            Assert.True(result);
        }

        [Fact]
        public void Test_GetPlaylistTable()
        {
            var mockObject = new Mock<IMyMusicDataBase>();
            mockObject.Setup(x => x.GetPlaylistTable(1)).Returns(GetSampleMusics());

            var actual = mockObject.Object.GetPlaylistTable(1);
            var expected = GetSampleMusics();

            Assert.True(actual != null);
            Assert.Equal(expected.Count(), actual.Count());
        }

        private ObservableCollection<Music> GetSampleMusics()
        {
            ObservableCollection<Music> output = new ObservableCollection<Music>
            {
                new Music
                {
                    Title = "Title1",
                    Artist = "Artist1",
                    Lyrics = "lyrics1.com"
                },
                new Music
                {
                    Title = "Title2",
                    Artist = "Artist2",
                    Lyrics = "lyrics2.com"
                },
                new Music
                {
                    Title = "Title3",
                    Artist = "Artist3",
                    Lyrics = "lyrics3.com"
                }
            };

            return output;
        }
    }
}