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
            _sfxClock.Controller.Seek(TimeSpan.Zero, TimeSeekOrigin.BeginTime);
            _sfxClock.Controller.Begin();
        }
        public void ChangeBackgroundMusic()
        {
            if (_backgroundClock != null)
            {
                _backgroundClock.Controller.Stop();
                _backgroundMusic.Clock = null;
            }

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
    }
}
