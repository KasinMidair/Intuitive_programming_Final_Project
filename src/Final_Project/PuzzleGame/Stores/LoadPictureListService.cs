using PuzzleGame.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using System.Data.Common;
using System.Collections.ObjectModel;
using System.Windows;

namespace PuzzleGame.Stores
{
    public class LoadPictureListService
    {
        private static volatile LoadPictureListService _instance;
        public static LoadPictureListService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LoadPictureListService();
                }

                return _instance;
            }
        }

        Connection connection = new Connection();

        // Reload the Piclist and get data from database
        public void LoadPictureList(ObservableCollection<Picture> PicList, string id)
        {

            connection.dataAdapter = new SqlDataAdapter($"Select PICNAME, PICPATH, PLAYERID, isDEFAULT from PICTURE where isDEFAULT = 1 or PLAYERID = '{id}'", connection.connStr);

            //check if PICTURE table is existed before putting data in, if it exists then delete it
            if (connection.ds.Tables.Contains("PICTURE"))
            {
                connection.ds.Tables.Remove(connection.ds.Tables["PICTURE"]);
            }
            connection.dataAdapter.Fill(connection.ds, "PICTURE");
            connection.dt = connection.ds.Tables["PICTURE"];

            PicList.Clear();
            foreach (DataRow dr in connection.dt.Rows)
            {
                PicList.Add(new Picture
                {
                    Name = Convert.ToString(dr["PICNAME"]),
                    Url = Convert.ToString(dr["PICPATH"]),
                    PlayerID = Convert.ToString(dr["PLAYERID"]),
                    isDefault = Convert.ToBoolean(dr["isDEFAULT"])
                });
            }
        }

        // delete a picture from database
        public bool DeletePicture( Picture pic, string PlayerID)
        {
            if (pic.isDefault == true)
            {
                _ = CusDialogService.Instance.ShowDialog("Can not delete DEFAULT PICTURE!");
                return false;
            }
            else
            {
                connection.dataAdapter.DeleteCommand = new SqlCommand($"Delete from PICTURE where PICNAME = '{pic.Name}' and PLAYERID = '{PlayerID}'", connection.Conn);

                DataRow[] dataRow = connection.dt.Select($"PICNAME = '{pic.Name}'");
                foreach (DataRow dr in dataRow)
                    dr.Delete();

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(connection.dataAdapter);
                connection.dataAdapter.Update(connection.ds, "PICTURE");

                return true;
            }
        }

        // add a picture from database
        public void AddPicture(Picture newPicture, string PlayerID)
        {
            connection.dataAdapter = new($"Select * from PICTURE", connection.connStr); 

            connection.dataAdapter.Fill(connection.ds, "PICTURE");
            connection.dt = connection.ds.Tables["PICTURE"];

            //check if picture existed
            var existingRow = connection.dt.AsEnumerable().FirstOrDefault(row =>
                    row.Field<string>("PICNAME") == newPicture.Name &&
                    row.Field<string>("PLAYERID") == PlayerID
                    );
            if (existingRow != null) return;

            DataRow dataRow = connection.dt.NewRow();
            dataRow["PICNAME"] = newPicture.Name;
            dataRow["PICPATH"] = newPicture.Url;
            dataRow["PLAYERID"] = PlayerID;
            dataRow["isDEFAULT"] = 0;

            connection.dt.Rows.Add(dataRow);

            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(connection.dataAdapter);
            connection.dataAdapter.Update(connection.ds, "PICTURE");
            //Load the picture list after delete picture
        }

    }
}