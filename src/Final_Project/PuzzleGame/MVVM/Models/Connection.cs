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
        public string connStr = "data source=KasinMidair-Dss\\SQLEXPRESS_BEGIN;initial catalog=SLIDING_PUZZLE_DB;trusted_connection=true";
        static string _connStr = "data source=KasinMidair-Dss\\SQLEXPRESS_BEGIN;initial catalog=SLIDING_PUZZLE_DB;trusted_connection=true";
        public SqlConnection Conn = new SqlConnection(_connStr);
        public SqlDataAdapter dataAdapter;
        public DataSet ds = new DataSet();
        public  DataTable dt = new DataTable();
    }
}
