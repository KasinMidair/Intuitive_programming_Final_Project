using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PuzzleGame.Core;
using PuzzleGame.MVVM.Models;
using PuzzleGame.Core.Helper;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Caliburn.Micro;

namespace PuzzleGame.MVVM.ViewModels
{
    public class LevelSelectionViewModel : ObservableObject
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

        public RelayCommand<object> OpenGalleryCommand { get; set; } //command for Gallery button
        public RelayCommand<object> SelectPictureCommand { get; set; }


        Connection connection = new Connection();
        public List<Picture> PictureList { get; set; }
        public int hour, minute, second;

        Picture _selectedPicture;
        public Picture SelectedPicture
        {
            get { return _selectedPicture; }
            set
            { 
                _selectedPicture = value;
                OnPropertyChanged("SelectedPicture");
            }
        }

        public LevelSelectionViewModel()
        {
            _wndBgr = defaultColornum1;
            PictureList = new List<Picture>();
            LoadPicList();

            OpenGalleryCommand = new RelayCommand<object>((o) =>
            {
                CurrentPage = new GalleryViewModel();
            });

            SelectPictureCommand = new RelayCommand<object>((o) =>
            {
            });
        }

        void LoadPicList()
        {
            connection.dataAdapter = new SqlDataAdapter("Select * from PICTURE", connection.connStr);

            connection.dataAdapter.Fill(connection.ds, "PICTURE");
            connection.dt = connection.ds.Tables["PICTURE"];

            foreach (DataRow dr in connection.dt.Rows)
            {
                PictureList.Add(new Picture { Name = Convert.ToString(dr["PICNAME"]), Url = Convert.ToString(dr["PICPATH"]) });
            }
        }

    }
}
