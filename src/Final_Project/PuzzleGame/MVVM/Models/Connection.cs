using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleGame.MVVM.Models
{
    //connection
    public partial class Connection
    {
        public static  string connStr = "data source=Lenovo_Legion_5;initial catalog=SLIDING_PUZZLE_DB;user id=SlidingPuzzle;password=123456789";
        public SqlConnection conn = new SqlConnection(connStr);
        public SqlDataAdapter dataAdapter;
        public DataSet ds = new DataSet();
        public  DataTable dt = new DataTable();
    }

    //tables
    public class img
    {
        public string imgname { get; set; }
        public string imgpath { get; set; }
    }

    //get data from database then fill to table and convert to list
    public class GalleryConnection : Connection
    {
        public List<img> imglist = new List<img>();

        void LoadPicList()
        {
            conn.Open();

            dataAdapter = new SqlDataAdapter("Select * from PICTURE", connStr);

            dataAdapter.Fill(ds, "PICTURE");
            dt = ds.Tables["PICTURE"];

            conn.Close();
            foreach (DataRow dr in dt.Rows)
            {
                imglist.Add(new img { imgname = Convert.ToString(dr["imgname"]), imgpath = Convert.ToString(dr["imgpath"]) });
            }

        }
    }
}
