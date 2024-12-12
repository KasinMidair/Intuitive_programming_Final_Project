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
        public string connStr = @"Data Source=KasinMidair-Dss\SQLEXPRESS_BEGIN;Initial Catalog=SLIDING_PUZZLE_DB;Integrated Security=True;";
        public SqlDataAdapter dataAdapter;
        public DataSet ds = new DataSet();
        public  DataTable dt = new DataTable();
    }
}
