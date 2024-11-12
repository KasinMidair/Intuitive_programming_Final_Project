using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleGame.MVVM.Models
{
    public partial class Connection
    {
        public SqlConnection connection = new SqlConnection(@"data source=Lenovo_Legion_5;initial catalog=SlidingPuzzle_DB;user id=SlidingPuzzle;password=<<123456789>>");
        public SqlDataAdapter adapter;
        public DataSet dataSet;
        public DataTable dataTable;
        string sqlStr;

        public class Image()
        {
            public string imageName { get; set; }
            public string imagePath { get; set; }
        }
        
        public List<Image> LoadGALLERY()
        {
            connection.Open();
            sqlStr = "Select * GALLERY";

            adapter = new SqlDataAdapter(sqlStr, connection);
            dataSet = new DataSet();
            adapter.Fill(dataSet, "GALLERY");
            dataTable = dataSet.Tables["GALLERY"];

            List<Image> imageList = new List<Image>();
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Image image = new Image();
                image.imageName = dataTable.Rows[i]["ImageName"].ToString();
                image.imagePath = dataTable.Rows[i]["Address"].ToString();
                imageList.Add(image);
            }

            return imageList;
        }
    


        public void CloseConnection()
        {
            connection.Close();
        }


    }
}
