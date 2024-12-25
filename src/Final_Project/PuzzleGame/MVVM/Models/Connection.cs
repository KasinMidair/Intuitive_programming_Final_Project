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
        public string connStr = "data source=DESKTOP-Q75RUSO;initial catalog=SLIDING_PUZZLE_DB;user id=nam;password=nam";
        static string _connStr = "data source=DESKTOP-Q75RUSO;initial catalog=SLIDING_PUZZLE_DB;user id=nam;password=nam";
        public SqlConnection Conn = new SqlConnection(_connStr);
        public SqlDataAdapter dataAdapter;
        public DataSet ds = new DataSet();
        public  DataTable dt = new DataTable();
    }
}
