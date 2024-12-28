using PuzzleGame.MVVM.Models;
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

namespace PuzzleGame.Stores
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

        // load data tu table GAMEROUND vao gameRounds
        public void LoadGameRoundsWithPieces(ObservableCollection<GameRound> gameRounds, string inputPieces)
        {
            connection.dataAdapter = new SqlDataAdapter(
                $"SELECT GAMEID, PLAYERNAME, PLAYERID, PIECES, PLAYTIME, PLAYDATE FROM GAMEROUND WHERE PIECES = '{inputPieces}' ORDER BY PLAYTIME",
                connection.connStr);

            //check if GAMEROUND table is existed before putting data in, if it exists then delete it
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
