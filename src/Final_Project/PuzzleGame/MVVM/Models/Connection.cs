﻿using System;
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
        public string connStr = "data source=Lenovo_Legion_5;initial catalog=SLIDING_PUZZLE_DB;user id=SlidingPuzzle;password=123456789";
        public SqlDataAdapter dataAdapter;
        public DataSet ds = new DataSet();
        public  DataTable dt = new DataTable();
    }
}