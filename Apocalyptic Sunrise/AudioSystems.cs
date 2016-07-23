using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Apocalyptic_Sunrise
{
    public class AudioControls
    {
        public int songID;
        public float audioVolume;
        public bool repeatSong;
        public bool isAudioPlaying;

        public static void SetVariables(float audioVolume, bool repeatSong)
        {
            MediaPlayer.Volume = audioVolume;
            MediaPlayer.IsRepeating = repeatSong;
        }
    }

    public class AudioSystems
    {
        static List<AudioControls> audios = new List<AudioControls>();
        public static Song[] songLoaded = new Song[4];

        //public AudioSystems(int songID = -1)
        //{
        //    audios;
        //}


        public static void LoadContent(ContentManager content)
        {
            songLoaded[0] = content.Load<Song>("Audio/Epic-Music-Mix-Science-Fiction_by_Dragon's_Refuge_intro-edit");
            songLoaded[1] = content.Load<Song>("Audio/Technicolor_by_Hydra");
            songLoaded[2] = content.Load<Song>("Audio/Winter_Jazz_Up_by_Paper_Coelacanth");
            songLoaded[3] = content.Load<Song>("Audio/Nimbus_by_92");
        }

        public void Update()
        {
            //if (audios != null)
            //{
            //    while
            //        if (!audio.isAudioPlaying)
            //        {
            //            MediaPlayer.Stop();
            //        }
            //    }
            //}

        }
        
        public static void StartPlayingAudio(int songID, float audioVolume, bool repeatSong)
        {
            //AudioSystems aS = new AudioSystems(songID);
            //aS.LoadContent
            AudioControls.SetVariables(audioVolume, repeatSong);

            MediaPlayer.Play(songLoaded[songID]);
        }
    }
}
