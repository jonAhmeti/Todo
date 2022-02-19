using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace To_Do.DAL
{
    public class ToDoItem
    {
        private readonly string _connectionString;

        public ToDoItem(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("todo");
        }

        public bool Add(Models.ToDoItem obj)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                using SqlCommand command = new SqlCommand("sp_todoItemAdd", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@Title", obj.Title);
                command.Parameters.AddWithValue("@DueDate", obj.DueDate);

                connection.Open();
                return command.ExecuteNonQuery() != -1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        
        public Models.ToDoItem Get(int id)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                using SqlCommand command = new SqlCommand("sp_todoItemGetById", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                return Mapper(command.ExecuteReader()).FirstOrDefault();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public IEnumerable<Models.ToDoItem> Get()
        {
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                using SqlCommand command = new SqlCommand("sp_todoItemGetAll", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                connection.Open();
                return Mapper(command.ExecuteReader());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public Models.ToDoItem GetLatest()
        {
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                using SqlCommand command = new SqlCommand("sp_todoItemGetLatest", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                connection.Open();
                return Mapper(command.ExecuteReader()).FirstOrDefault();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public bool Edit(Models.ToDoItem obj)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                using SqlCommand command = new SqlCommand("sp_todoItemEdit", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Id", obj.Id);
                command.Parameters.AddWithValue("@Title", obj.Title);
                command.Parameters.AddWithValue("@DueDate", obj.DueDate);

                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                using SqlCommand command = new SqlCommand("sp_todoItemDelete", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        public IEnumerable<Models.ToDoItem> Mapper(SqlDataReader reader)
        {
            try
            {
                var list = new List<Models.ToDoItem>();
                while (reader.Read())
                {
                    list.Add(new Models.ToDoItem
                    {
                        Id = reader.GetInt32("Id"),
                        Title = reader.GetString("Title"),
                        DueDate = reader.GetDateTime("DueDate")
                    });
                }
                return list;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
