using System;
using MySql.Data.MySqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace ApplicationMVC.Models;

public class IncidentModel
{
    private readonly string _connection_string;

    public IncidentModel(string connection_string)
    {
        _connection_string = connection_string;
    }

    private int ExecuteNonQuery(MySqlCommand cmd)
    {
        try
        {
            MySqlConnection db = new(_connection_string);
            db.Open();
            cmd.Connection = db;
            int rows_affected = cmd.ExecuteNonQuery();
            db.Close();
            return rows_affected;
        } catch (MySqlException ex)
        {
            Console.WriteLine(ex.ToString());
            throw new(ex.Message);
        }
    }

    private DataTable ExecuteQuery(MySqlCommand cmd)
	{
        try
        {
            MySqlConnection db = new(_connection_string);
            db.Open();
            cmd.Connection = db;
            MySqlDataAdapter adapter = new(cmd);
            DataSet ds = new();
            adapter.Fill(ds, "table");
            DataTable table = ds.Tables["table"];
            db.Close();
            return table;
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex.ToString());
            throw ex;
        }
    }

	public DataTable GetAll()
	{
		return ExecuteQuery(new MySqlCommand("SELECT * FROM Incident"));
	}

    public DataTable GetOne(int nr)
    {
        MySqlCommand cmd = new("SELECT * FROM Incident WHERE nr=@nr");
        cmd.Parameters.AddWithValue("nr", nr);
        return ExecuteQuery(cmd);
    }

    public DataTable GetOperations(int nr)
    {
        MySqlCommand cmd = new("SELECT date, codetype FROM AllOperation WHERE i_nr=@nr");
        cmd.Parameters.AddWithValue("nr", nr);
        return ExecuteQuery(cmd);
    }
}

