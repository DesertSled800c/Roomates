using Microsoft.Data.SqlClient;
using Roommates.Models;
using Roommates.Repositories;
using System.Collections.Generic;

namespace Roomates.Repositories
{
    public class RoommateRepository : BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString) { }

        public List<Roommate> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "Select * FROM Roommate";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Roommate> roommate = new();
                        while (reader.Read())
                        {
                            Roommate chore = new Roommate
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                                MovedInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate")),

                            };
                            roommate.Add(chore);
                        }
                        return roommate;
                    }
                }
            }
        }

        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // first name, their rent portion, and the name of the room they occupy
                    cmd.CommandText = "SELECT FirstName, RentPortion, Room.* " +
                        "FROM Roommate " +
                        "LEFT JOIN Room ON Room.id = Roommate.RoomId " +
                        "WHERE Id = @id ";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Roommate roommate = null;

                        if (reader.Read())
                        {
                            roommate = new Roommate
                            {
                                Id = id,
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                                Room = new Room
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    MaxOccupancy = reader.GetInt32(reader.GetOrdinal("MaxOccupancy")),
                                }
                            };
                        }
                        return roommate;
                    }
                }
            }
        }
    }
}
