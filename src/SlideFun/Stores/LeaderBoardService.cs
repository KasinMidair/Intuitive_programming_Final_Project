﻿using SlideFun.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Windows;

namespace SlideFun.Stores
{
    public class LeaderBoardService
    {
        private static volatile LeaderBoardService _instance;
        public static LeaderBoardService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LeaderBoardService();
                }

                return _instance;
            }
        }
        public ObservableCollection<GameRound> GameRoundsList = new ObservableCollection<GameRound>();
        Connection connection = new Connection();
        // Find all records in GAMEROUNDTABLE matching inputPieces and playerID or just inputPieces when playerId = null
        public void LoadGameRounds(ObservableCollection<GameRound> gameRounds, string inputPieces, string playerId = null)
        {
            string sql = $"SELECT GAMEID, PLAYERNAME, PLAYERID, PIECES, PLAYTIME, PLAYDATE, RANK() OVER (ORDER BY PLAYTIME ASC) AS RANKING FROM GAMEROUND WHERE PIECES = '{inputPieces}'";

            if (!string.IsNullOrEmpty(playerId))
            {
                sql += $" AND PLAYERID = '{playerId}'";
            }

            sql += " ORDER BY RANKING";

            connection.dataAdapter = new SqlDataAdapter(sql, connection.connStr);

            // Check if GAMEROUND table exists before putting data in, if it exists then delete it
            if (connection.ds.Tables.Contains("GAMEROUND"))
            {
                connection.ds.Tables.Remove(connection.ds.Tables["GAMEROUND"]);
            }

            connection.dataAdapter.Fill(connection.ds, "GAMEROUND");
            connection.dt = connection.ds.Tables["GAMEROUND"];

            // Clear the gameRounds list before adding new data
            GameRoundsList.Clear();

            foreach (DataRow dr in connection.dt.Rows)
            {
                gameRounds.Add(new GameRound
                {
                    Ranking  = Convert.ToString(dr["RANKING"]),
                    PlayerName = Convert.ToString(dr["PLAYERNAME"]),
                    PlayerID = Convert.ToString(dr["PLAYERID"]),
                    Pieces = Convert.ToString(dr["PIECES"]),
                    Time = Convert.ToString(dr["PLAYTIME"]),
                    Date = Convert.ToString(dr["PLAYDATE"])
                });
            }
        }
        // them gameRound vao table
        public void AddGameRound(GameRound gameRound)
        {
            string query = $"Insert into GAMEROUND values" +
                $"('{(GetRowCount()+1).ToString("D6")}', '{gameRound.PlayerName}', '{gameRound.PlayerID}', '{gameRound.Pieces}', '{gameRound.Time}', '{gameRound.Date}')";

            SqlCommand command = new SqlCommand(query, connection.Conn);

            connection.Conn.Open();

            command.ExecuteNonQuery();

            connection.Conn.Close();
        }

        // dem so dong trong table GAMEROUND
        int GetRowCount()
        {
            int rowCount = 0;

            SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM GAMEROUND", connection.Conn);

            connection.Conn.Open();
            rowCount = (int)command.ExecuteScalar();
            connection.Conn.Close();

            return rowCount;
        }
        
    }
}
