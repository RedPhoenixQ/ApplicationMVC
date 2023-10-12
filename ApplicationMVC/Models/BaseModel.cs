using System;
using MySql.Data.MySqlClient;
using System.Data;

namespace ApplicationMVC.Models;

public class BaseModel
{
    private readonly string _connection_string;

    public BaseModel(string connection_string)
	{
        _connection_string = connection_string;
    }

    protected int ExecuteNonQuery(MySqlCommand cmd)
    {
        try
        {
            MySqlConnection db = new(_connection_string);
            db.Open();
            cmd.Connection = db;
            int rows_affected = cmd.ExecuteNonQuery();
            db.Close();
            return rows_affected;
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex.ToString());
            throw new(ex.Message);
        }
    }

    protected DataTable ExecuteQuery(MySqlCommand cmd)
    {
        try
        {
            MySqlConnection db = new(_connection_string);
            db.Open();
            cmd.Connection = db;
            MySqlDataAdapter adapter = new(cmd);
            DataTable table = new();
            adapter.Fill(table);
            db.Close();
            return table;
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex.ToString());
            throw ex;
        }
    }
}

