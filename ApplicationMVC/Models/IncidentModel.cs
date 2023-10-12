using System;
using MySql.Data.MySqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace ApplicationMVC.Models;

public class IncidentModel : BaseModel
{
    public IncidentModel(string connection_string) : base(connection_string) { }

	public DataTable GetAll()
	{
		return ExecuteQuery(new MySqlCommand("SELECT * FROM Incident"));
	}

    public DataTable Search(string query)
    {
        MySqlCommand cmd = new("SELECT * FROM Incident WHERE name LIKE @query");
        cmd.Parameters.AddWithValue("query", "%" + query + "%");
        return ExecuteQuery(cmd);
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

