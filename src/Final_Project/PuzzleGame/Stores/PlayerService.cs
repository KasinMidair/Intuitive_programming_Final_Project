using PuzzleGame.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleGame.Stores
{
    public class PlayerService
    {
        Connection connection = new Connection();

        // Tim kiem ten nguoi choi
        public Player CheckPlayerName(string playerName)
        {
            Player player = null;

            //neu ten nguoi choi ton tai thi tai thong tin nguoi choi len player, neu ten nguoi choi khong ton tai thi tra ve null
            string query = "Select PLAYERID, PLAYERNAME, PLAYERPASSWORD from PLAYER where PLAYERNAME = @name";
            SqlCommand command = new SqlCommand(query, connection.Conn);
            command.Parameters.AddWithValue("@name", playerName);

            connection.Conn.Open();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    player = new Player
                    {
                        Id = reader.GetString("PLAYERID"),
                        Name = reader.GetString("PLAYERNAME"),
                        Password = reader.GetString("PLAYERPASSWORD")
                    };
                }
            }

            connection.Conn.Close();

            return player;
        }


        // Them nguoi choi moi vao table
        public void AddPlayer(string name, string password)
        {
            // Kiem tra ten nguoi choi da ton tai hay chua
            if (CheckPlayerName(name) == null)
            {
                string query = $"Insert into PLAYER values ('{GetRowCount().ToString("D6")}', '{name}', '{password}')";

                SqlCommand command = new SqlCommand(query, connection.Conn);

                connection.Conn.Open();

                command.ExecuteNonQuery();

                connection.Conn.Close();
            }
        }


        // dem so dong trong table PLAYER
        int GetRowCount()
        {
            int rowCount = 0;

            SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM PLAYER", connection.Conn);

            connection.Conn.Open();
            rowCount = (int)command.ExecuteScalar();
            connection.Conn.Close();

            return rowCount;
        }
    }
}