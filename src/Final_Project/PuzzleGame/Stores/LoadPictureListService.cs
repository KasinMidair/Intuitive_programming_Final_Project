using PuzzleGame.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PuzzleGame.Stores
{
    public class LoadPictureListService
    {
        Connection connection = new Connection();

        public void LoadPictureList(List<Picture> PicList, string name)
        {
            connection.dataAdapter = new SqlDataAdapter($"Select * from PICTURE where PLAYERNAME is NULL or PLAYERNAME = '{name}'", connection.connStr);

            connection.dataAdapter.Fill(connection.ds, "PICTURE");
            connection.dt = connection.ds.Tables["PICTURE"];

            foreach (DataRow dr in connection.dt.Rows)
            {
                PicList.Add(new Picture { Name = Convert.ToString(dr["PICNAME"]), Url = Convert.ToString(dr["PICPATH"]) });
            }
        }

        public void DeletePicture(string picName)
        {
            connection.dataAdapter = new SqlDataAdapter($"Delete from PICTURE where PICNAME = '{picName}'", connection.connStr);
            Console.WriteLine("hello");
        }
    }
}
