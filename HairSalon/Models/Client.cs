using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace HairSalon.Models
{
  public class Client
  {
    private string _name;
    private string _phoneNumber;
    private int _id;
    private int _stylistId;

    public Client (string name, string phoneNumber, int stylistId, int id = 0)
    {
      _name = name;
      _phoneNumber = phoneNumber;
      _stylistId = stylistId;
      _id = id;
    }

    public string GetName()
    {
      return _name;
    }

    public void SetName(string newName)
    {
      _name = newName;
    }

    public string GetPhoneNumber()
    {
      return _phoneNumber;
    }

    public int GetId()
    {
      return _id;
    }

    public int GetStylistId()
    {
      return _stylistId;
    }

    public static List<Client> GetAll()
    {
      List<Client> allClients = new List<Client> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM client;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int ClientId = rdr.GetInt32(0);
        string ClientName = rdr.GetString(1);
        string ClientPhone = rdr.GetString(1);
        int Clientstylistid = rdr.GetInt32(3);
        Client newClient = new Client(ClientName, ClientPhone, Clientstylistid, ClientId);
        allClients.Add(newClient);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allClients;
    }

    public static void ClearAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM client;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
       conn.Dispose();
      }
    }

    public static Client Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM client WHERE id = (@searchId);";
      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int ClientId = 0;
      string ClientName = "";
      string ClientPhone = "";
      int ClientStylistId = 0;
      while(rdr.Read())
      {
        ClientId = rdr.GetInt32(0);
        ClientName = rdr.GetString(1);
        ClientPhone = rdr.GetString(2);
        ClientStylistId = rdr.GetInt32(3);
      }
      Client newClient = new Client(ClientName, ClientPhone, ClientStylistId, ClientId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newClient;
    }

    public override bool Equals(System.Object otherClient)
    {
      if (!(otherClient is Client))
      {
        return false;
      }
      else
      {
         Client newClient = (Client) otherClient;
         bool idEquality = this.GetId() == newClient.GetId();
         bool nameEquality = this.GetName() == newClient.GetName();
         bool phoneEquality = this.GetPhoneNumber() == newClient.GetPhoneNumber();
         bool stylistEquality = this.GetStylistId() == newClient.GetStylistId();
         return (idEquality && nameEquality && phoneEquality && stylistEquality);
       }
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO client (name, phonenumber, stylist_id) VALUES (@name, @phone, @stylist_id);";
      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@name";
      name.Value = this._name;
      cmd.Parameters.Add(name);
      MySqlParameter phone = new MySqlParameter();
      phone.ParameterName = "@phone";
      phone.Value = this._phoneNumber;
      cmd.Parameters.Add(phone);
      MySqlParameter stylistid = new MySqlParameter();
      stylistid.ParameterName = "@stylist_id";
      stylistid.Value = this._stylistId;
      cmd.Parameters.Add(stylistid);
      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void Edit(string newName)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE Client SET name = @newName WHERE id = @searchId;";
      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);
      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@newName";
      name.Value = newName;
      cmd.Parameters.Add(name);
      cmd.ExecuteNonQuery();
      _name = newName;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void Delete(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM client WHERE id = @thisId;";
      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "thisId";
      thisId.Value = id;
      cmd.Parameters.Add(thisId);
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

  }
}
