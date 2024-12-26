using PuzzleGame.MVVM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.ComponentModel;
using System.Windows;
using System.Reflection;



namespace PuzzleGame.Stores
{
    public enum AudioType
    {
        BACKGROUND_MSC,
        SFX_MSC,
        ALL
    }
    public class MusicSystemService
    {

        private static volatile MusicSystemService _instance;
        public static MusicSystemService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MusicSystemService();
                }

                return _instance;
            }
        }

        List<string> bgAudioSources;
        List<string> endGameAudioSources;
        string sfxAdioSource;
        readonly MediaPlayer _sfx;
        MediaClock _sfxClock;
        int curbgAudio;
        readonly MediaPlayer _backgroundMusic;
        MediaClock _backgroundClock;
        readonly MediaPlayer _endGameMusic;
        MediaClock _endGameClock;

        public MusicSystemService()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) return;

            //BG audio sys
            bgAudioSources = new List<string>();
            curbgAudio = 2;
            List<string> bgAudioPaths = new List<string>()
            {
                "Assets/Audio/DefaultBGMusic.mp3",
                "Assets/Audio/BGMusicNum2.mp3",
                "Assets/Audio/BGMusicNum3.mp3"
            };
            _backgroundMusic = new MediaPlayer();
            foreach (var item in bgAudioPaths)
            {
                bgAudioSources.Add(ExtractEmbeddedResource(item));
            }


            //SFX audio sys
            endGameAudioSources = new List<string>();
            List<string> endGameAudioPaths= new List<string>()
            {
                "Assets/Audio/Lose.wav",
                "Assets/Audio/Win.wav"
            };
            _endGameMusic = new MediaPlayer();
            foreach (var item in endGameAudioPaths)
            {
                endGameAudioSources.Add(ExtractEmbeddedResource(item));
            }


            //SFX button audio sys
            _sfx = new MediaPlayer();
            sfxAdioSource = ExtractEmbeddedResource("Assets/Audio/btn_click.mp3");
            MediaTimeline mediaTimelineSFX = new MediaTimeline(new Uri(sfxAdioSource, UriKind.RelativeOrAbsolute));
            _sfxClock = mediaTimelineSFX.CreateClock();
            _sfx.Clock = _sfxClock;

        }


        public void PlayBTN_ClickSound()
        {
            _sfxClock.Controller.Stop();
            _sfxClock.Controller.Begin();
        }

        public void EndGame_Sound(int endGamestatus)
        {
            if (_endGameClock != null)
            {
                _endGameClock.Controller.Stop();
                _endGameMusic.Clock = null;
            }
            MediaTimeline mediaTimeline = new MediaTimeline(new Uri(endGameAudioSources[endGamestatus], UriKind.RelativeOrAbsolute));
            _endGameClock = mediaTimeline.CreateClock();
            _endGameMusic.Clock = _endGameClock;
            _endGameClock.Controller.Begin();
        }

        public void ChangeBackgroundMusic(int bgIndex)
        {
            if (_backgroundClock != null)
            {
                _backgroundClock.Controller.Stop();
                _backgroundMusic.Clock = null;
            }
            curbgAudio = bgIndex;
            MediaTimeline mediaTimeline = new MediaTimeline(new Uri(bgAudioSources[curbgAudio], UriKind.RelativeOrAbsolute));
            mediaTimeline.RepeatBehavior = RepeatBehavior.Forever;
            _backgroundClock = mediaTimeline.CreateClock();
            _backgroundMusic.Clock = _backgroundClock;
            _backgroundClock.Controller.Begin();
        }

        public void Dispose()
        {
            _backgroundClock.Controller.Stop();
            _backgroundMusic.Clock = null;  
            _backgroundMusic.Close();
            _sfx.Clock= null;
            _sfx.Close();
        }

        /// <summary>
        /// Extract the embedded resource to temporary file 
        /// </summary>
        /// <param name="resourcePath"></param>
        /// <returns></returns>
        private string ExtractEmbeddedResource(string resourcePath)
        {
            //Create a temporary path

            string tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetFileName(resourcePath));

            if (!File.Exists(tempFilePath))
            {
                // open embedded audio resource
                Uri resourceUri = new Uri($"pack://application:,,,/{resourcePath}");
                using (Stream resourceStream = Application.GetResourceStream(resourceUri).Stream)
                using (FileStream fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
                {
                    resourceStream.CopyTo(fileStream);
                }
            }

            return tempFilePath;
        }

        public void SetVolume(AudioType audioType, double bgVolume, double sfxVolume)
        {
            switch (audioType)
            {
                case AudioType.BACKGROUND_MSC:
                    _backgroundMusic.Volume = bgVolume;
                    break;
                case AudioType.SFX_MSC:
                    _sfx.Volume = _endGameMusic.Volume = sfxVolume;
                  
                    break;
                default:
                    _backgroundMusic.Volume = bgVolume;
                    _sfx.Volume = _endGameMusic.Volume= sfxVolume;
                    break;
            }
        }

    }
}
