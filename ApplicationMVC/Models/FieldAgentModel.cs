using System;
using MySql.Data.MySqlClient;
using System.Data;

namespace ApplicationMVC.Models;

public class FieldAgentModel : BaseModel
{
    public FieldAgentModel(string connection_string) : base(connection_string) { }

	public DataTable GetAll()
	{
		return ExecuteQuery(new MySqlCommand("SELECT * FROM AllFieldAgent"));
	}

    public DataTable GetOne(string name, int nr)
    {
        MySqlCommand cmd = new("SELECT name, nr, uname as full_name, salary, speciality, competence, num_operations as operations, num_succesful_operations as successful_operations FROM AllFieldAgent WHERE name=@name AND nr=@nr");
        cmd.Parameters.AddWithValue("name", name);
        cmd.Parameters.AddWithValue("nr", nr);
        return ExecuteQuery(cmd);
    }

    public DataTable GetOperations(string name, int nr)
    {
        MySqlCommand cmd = new("SELECT date, codetype, i_nr, completed FROM AllFieldAgentOperation WHERE name=@name AND nr=@nr");
        cmd.Parameters.AddWithValue("name", name);
        cmd.Parameters.AddWithValue("nr", nr);
        return ExecuteQuery(cmd);
    }
}

