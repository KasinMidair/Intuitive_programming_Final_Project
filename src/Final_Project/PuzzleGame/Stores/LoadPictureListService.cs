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
        Connection connection = new Connection();

        public void LoadPictureList(ObservableCollection<Picture> PicList, string name)
        {
            connection.dataAdapter = new SqlDataAdapter($"Select PICNAME, PICPATH, PLAYERNAME, isDEFAULT from PICTURE where isDEFAULT = 1 or PLAYERNAME = '{name}'", connection.connStr);

            connection.dataAdapter.Fill(connection.ds, "PICTURE");
            connection.dt = connection.ds.Tables["PICTURE"];

            foreach (DataRow dr in connection.dt.Rows)
            {
                PicList.Add(new Picture 
                { 
                    Name = Convert.ToString(dr["PICNAME"]), 
                    Url = Convert.ToString(dr["PICPATH"]),
                    PlayerName = Convert.ToString(dr["PLAYERNAME"]),
                    isDefault = Convert.ToBoolean(dr["isDEFAULT"])
                });
            }
        }

        public bool DeletePicture(Picture pic)
        {
            if (pic.isDefault == true)
            {
                MessageBox.Show("Khong the xoa tranh mac dinh!");
                return false;
            }
            else
            {
                connection.dataAdapter.DeleteCommand = new SqlCommand($"Delete from PICTURE where PICNAME = '{pic.Name}'", connection.Conn);

                DataRow[] dataRow = connection.dt.Select($"PICNAME = '{pic.Name}'");
                foreach (DataRow dr in dataRow)
                    dr.Delete();

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(connection.dataAdapter);
                connection.dataAdapter.Update(connection.ds, "PICTURE");

                return true;
            }
        }

        public void AddPicture(Picture newPicture, string PlayerName)
        {
            connection.dataAdapter = new ($"Select * from PICTURE", connection.connStr);

            connection.dataAdapter.Fill(connection.ds, "PICTURE");
            connection.dt = connection.ds.Tables["PICTURE"];


            DataRow dataRow = connection.dt.NewRow();
            dataRow["PICNAME"] = newPicture.Name;
            dataRow["PICPATH"] = newPicture.Url;
            dataRow["PLAYERNAME"] = PlayerName;
            dataRow["isDEFAULT"] = 0;

            connection.dt.Rows.Add(dataRow);

            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(connection.dataAdapter);
            connection.dataAdapter.Update(connection.ds, "PICTURE");
        }
    }
}