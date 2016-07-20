using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Apocalyptic_Sunrise
{
    class AudioSystems
    {


        public void Update()
        {

        }
    }

    public class LoadMusic
    {
        public enum listOfSongs
        {
            MainMenu,
            InGame1,
            InGame2
        }

        public Song MainMenuSong;
        public Song InGameSong1;
        public Song InGameSong2;

        public int songID;

        public float audioVolune;
        public bool isAudioPlaying;

        public LoadMusic(int songID, float audioVolume)
        {

        }

        public void Update()
        {

        }

        public void LoadContent(ContentManager content)
        {
            MainMenuSong = content.Load<Song>("scoreFont");
            InGameSong1 = content.Load<Song>("scoreFont");
            InGameSong2 = content.Load<Song>("scoreFont");
        }

        public static LoadMusic StartPlayingAudio(int songID, float audioVolume)
        {
            LoadMusic lm = new LoadMusic(songID, audioVolume);

            return lm;
        }
    }
}
