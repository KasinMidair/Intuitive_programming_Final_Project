using PuzzleGame.Core;
using PuzzleGame.Core.Helper;
using PuzzleGame.MVVM.Models;
using PuzzleGame.Stores;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace PuzzleGame.MVVM.ViewModels
{
    class GalleryViewModel : ObservableObject
    {
    
        private ObservableObject _currentPage;
        public ObservableObject CurrentPage
        {
            get => _currentPage;
            set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    EventAggregator.GetEvent<PubSubEvent<ObservableObject>>().Publish(CurrentPage);
                }
            }
        }

        //Connection...
        Connection connection = new Connection();

        //Pictures...
        public ObservableCollection<Picture> pictureList = new ObservableCollection<Picture>();
        public ObservableCollection<Picture> PictureList
        {
            get => pictureList;
            set
            {
                pictureList = value;
                OnPropertyChanged();
            }
        }

        Picture _selectedPicture;
        public Picture SelectedPicture
        {
            get { return _selectedPicture; }
            set
            {
                _selectedPicture = value;
                OnPropertyChanged();
            }
        }   


        //Command...
        public RelayCommand<object> AddPicturePageOpenCommand { get; set; }
        public RelayCommand<Object> DeletePictureCommand { get; set; }

        public GalleryViewModel()
        {
            _wndBgr = defaultColornum2;
            EventAggregator.GetEvent<PubSubEvent<string>>().Subscribe((o) => LoadPicTureList());

            LoadPictureListService.Instance.LoadPictureList(PictureList, GameModel.Instance.Player.Id);
            SelectedPicture = PictureList.ElementAt(0);
            AddPicturePageOpenCommand = new RelayCommand<object>((o) => 
            {
                CusDialogService.Instance.ShowAddPicture(new AddPictureViewModel(this.PictureList)); 
            });

            DeletePictureCommand = new RelayCommand<object>((o) => { DeleteSelectedPicture(GameModel.Instance.Player.Id); });
        }


        //set the selectionPicture is PictureList[0] if you add/delete pictures
        private void LoadPicTureList()
        {
            LoadPictureListService.Instance.LoadPictureList(PictureList, GameModel.Instance.Player.Id);

            SelectedPicture = PictureList.ElementAt(0);

        }

         void DeleteSelectedPicture(string playerID)
        {
            string url = SelectedPicture.Url;
            string n = SelectedPicture.Name;

            if (LoadPictureListService.Instance.DeletePicture(SelectedPicture, playerID) == true)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() => {
                    EventAggregator.GetEvent<PubSubEvent<string>>().Publish("");
                });

                // delete picture file

                if (File.Exists(url))
                {
                    try
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        File.Delete(url);

                        Console.WriteLine("File đã được xóa thành công.");
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine($"Lỗi khi xóa tệp: {ex.Message}");
                    }

                }

            }
        }

    }
}
