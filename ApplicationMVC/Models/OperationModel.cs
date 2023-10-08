using System;
using MySql.Data.MySqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace ApplicationMVC.Models;

public class OperationModel
{
	private string _connection_string;

	public OperationModel(string connection_string)
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
            DataSet ds = new DataSet();
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
		return ExecuteQuery(new MySqlCommand("SELECT * FROM AllOperation"));
	}

    public DataTable GetAllAgents(DateTime date, string codetype, int i_nr)
    {
        MySqlCommand cmd = new("SELECT name, nr FROM AllFieldAgentOperation WHERE date=@date AND codetype=@codetype AND i_nr=@i_nr");
        cmd.Parameters.AddWithValue("date", date);
        cmd.Parameters.AddWithValue("codetype", codetype);
        cmd.Parameters.AddWithValue("i_nr", i_nr);
        return ExecuteQuery(cmd);
    }

    public DataTable GetOne(DateTime date, string codetype, int i_nr)
    {
        MySqlCommand cmd = new MySqlCommand("SELECT * FROM AllOperation WHERE date=@date AND codetype=@codetype AND i_nr=@i_nr");
        cmd.Parameters.AddWithValue("date", date);
        cmd.Parameters.AddWithValue("codetype", codetype);
        cmd.Parameters.AddWithValue("i_nr", i_nr);
        return ExecuteQuery(cmd);
    }

    public bool AddAgent(DateTime date, string codetype, int i_nr, string name, int nr)
    {
        MySqlCommand cmd = new("INSERT INTO FieldAgentOperation(date, codetype, i_nr, name, nr) VALUES (@date, @codetype, @i_nr, @name, @nr)");
        cmd.Parameters.AddWithValue("date", date);
        cmd.Parameters.AddWithValue("codetype", codetype);
        cmd.Parameters.AddWithValue("i_nr", i_nr);
        cmd.Parameters.AddWithValue("name", name);
        cmd.Parameters.AddWithValue("nr", nr);
        return 1 == ExecuteNonQuery(cmd);
    }

    public bool RemoveAgent(DateTime date, string codetype, int i_nr, string name, int nr)
    {
        MySqlCommand cmd = new("DELETE FROM FieldAgentOperation WHERE date=@date AND codetype=@codetype AND i_nr=@i_nr AND name=@name AND nr=@nr");
        cmd.Parameters.AddWithValue("date", date);
        cmd.Parameters.AddWithValue("codetype", codetype);
        cmd.Parameters.AddWithValue("i_nr", i_nr);
        cmd.Parameters.AddWithValue("name", name);
        cmd.Parameters.AddWithValue("nr", nr);
        return 1 == ExecuteNonQuery(cmd);
    }

    public bool Complete(DateTime date, string codetype, int i_nr, bool success)
    {
        MySqlCommand cmd = new("complete_operation");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("date", date);
        cmd.Parameters.AddWithValue("codetype", codetype);
        cmd.Parameters.AddWithValue("i_nr", i_nr);
        cmd.Parameters.AddWithValue("success", success);
        return 1 == ExecuteNonQuery(cmd);
    }

    public bool Create(DateTime date, string codetype, int i_nr, DateTime end_date, string region_name, string region_terrain)
    {
        MySqlCommand cmd = new("INSERT INTO Operation(date, codetype, i_nr, end_date, region_name, region_terrain) VALUES (@date, @codetype, @i_nr, @end_date, @region_name, @region_terrain)");
        cmd.Parameters.AddWithValue("date", date);
        cmd.Parameters.AddWithValue("codetype", codetype);
        cmd.Parameters.AddWithValue("i_nr", i_nr);
        cmd.Parameters.AddWithValue("end_date", end_date);
        cmd.Parameters.AddWithValue("region_name", region_name);
        cmd.Parameters.AddWithValue("region_terrain", region_terrain);
        return 1 == ExecuteNonQuery(cmd);
    }
}

