using PuzzleGame.MVVM.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

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
        string sfxAdioSource;
        readonly MediaPlayer _sfx;
        MediaClock _sfxClock;
        int curbgAudio;
        readonly MediaPlayer _backgroundMusic;
        MediaClock _backgroundClock;

        public MusicSystemService()
        {
            bgAudioSources = new List<string>();
            curbgAudio = 2;
            List<string> bgAudioPaths = new List<string>()
            {
                "Assets/Audio/DefaultBGMusic.mp3",
                "Assets/Audio/BGMusicNum2.mp3",
                "Assets/Audio/BGMusicNum3.mp3"
            };
            sfxAdioSource = ExtractEmbeddedResource("Assets/Audio/btn_click.mp3");
            _backgroundMusic = new MediaPlayer();
            _sfx = new MediaPlayer();
            foreach (var item in bgAudioPaths)
            {
                bgAudioSources.Add(ExtractEmbeddedResource(item)); 
            }

            MediaTimeline mediaTimelineSFX = new MediaTimeline(new Uri(sfxAdioSource, UriKind.RelativeOrAbsolute));
            _sfxClock = mediaTimelineSFX.CreateClock();
            _sfx.Clock = _sfxClock;
        }
        public void PlayBTN_ClickSound()
        {
            _sfxClock.Controller.Stop();    
            _sfxClock.Controller.Begin();
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
        }

        /// <summary>
        /// Extract the embedded resource to temporary file 
        /// </summary>
        /// <param name="resourcePath"></param>
        /// <returns></returns>
        private string ExtractEmbeddedResource(string resourcePath)
        {
            //Create a temporary path

            string tempFilePath = Path.Combine("pack://application:,,,/Assets/Imgs", Path.GetFileName(resourcePath));

            if (!File.Exists(tempFilePath))
            {
                // open embedded audio resource
                Uri resourceUri = new Uri($"{resourcePath}",UriKind.Relative);
                using (Stream resourceStream = Application.GetResourceStream(resourceUri).Stream)
                using (FileStream fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
                {
                    resourceStream.CopyTo(fileStream);
                }
            }

            return tempFilePath;
        }

        /// <summary>
        /// set the application audio volume
        /// </summary>
        /// <param name="audioType"></param>
        /// <param name="bgVolume"></param>
        /// <param name="sfxVolume"></param>
        public void SetVolume(AudioType audioType,double bgVolume,double sfxVolume)
        {
            switch(audioType)
            {
                case AudioType.BACKGROUND_MSC:
                    _backgroundMusic.Volume = bgVolume;
                     break;  
                case AudioType.SFX_MSC:
                    _sfx.Volume = sfxVolume;
                    break;
                default:
                    _backgroundMusic.Volume = bgVolume;
                    _sfx.Volume = sfxVolume;
                    break;  
            } 
        }

    }
}
