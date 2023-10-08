using System;
using MySql.Data.MySqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace ApplicationMVC.Models;

public class FieldAgentModel
{
    private readonly string _connection_string;

    public FieldAgentModel(string connection_string)
    {
        _connection_string = connection_string;
    }

	private DataTable RunQuery(MySqlCommand cmd)
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

	public DataTable GetAll()
	{
		return RunQuery(new MySqlCommand("SELECT * FROM AllFieldAgent"));
	}

    public DataTable GetOne(string name, int nr)
    {
        MySqlCommand cmd = new("SELECT name, nr, uname as full_name, salary, speciality, competence, num_operations as operations, num_succesful_operations as successful_operations FROM AllFieldAgent WHERE name=@name AND nr=@nr");
        cmd.Parameters.AddWithValue("name", name);
        cmd.Parameters.AddWithValue("nr", nr);
        return RunQuery(cmd);
    }

    public DataTable GetOperations(string name, int nr)
    {
        MySqlCommand cmd = new("SELECT date, codetype, i_nr, completed FROM AllFieldAgentOperation WHERE name=@name AND nr=@nr");
        cmd.Parameters.AddWithValue("name", name);
        cmd.Parameters.AddWithValue("nr", nr);
        return RunQuery(cmd);
    }
}

