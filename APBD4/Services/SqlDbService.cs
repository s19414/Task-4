using APBD4.Models;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System.Data.Common;
using System.Net.Security;
using System.Text.Json;

namespace APBD4.Services;
public class SqlDbService
{
    private string connectionString;
    public SqlDbService(IConfiguration _configuration) {
        connectionString = _configuration.GetConnectionString("AnimalsDB");
    }
//CONTROLLER FUNCTIONS
    public string AnimalListToJSON(List<Animal> animals)
    {
        if(animals == null)
        {
            return "";
        }
        else
        {
            return JsonSerializer.Serialize(animals);
        }
        
    }
    

    //read animal from JSON and add to database if valid
    public bool AddAnimal(Animal animal)
    {
        //rudimentary validation.. basically just for show
        if(animal.Area == "" || animal.Name == "" || animal.Category == "")
        {
            return false;
        }
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandText = @"INSERT INTO Animal(Name, Description, Category, Area) VALUES(@name, @desc, @category ,@area);";
            com.Parameters.AddWithValue("@name", animal.Name);
            com.Parameters.AddWithValue("@desc", animal.Description);
            com.Parameters.AddWithValue("@category", animal.Category);
            com.Parameters.AddWithValue("@area", animal.Area);

            con.Open();
            com.ExecuteNonQuery();
            return true;
        }
    }

    public bool UpdateAnimal(int idOfUpdatedAnimal, Animal updatedAnimal)
    {
        List<Animal> animals = GetAnimalsOrderedBy("name");
        //check if updated animal actually exists in DB
        foreach(Animal animal in animals)
        {
            if (animal.IdAnimal== idOfUpdatedAnimal)
            {
                //do work
                using(SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = @"UPDATE Animal 
                                    SET Name=@name, Description=@desc, Category=@category, Area=@area
                                    WHERE IdAnimal=" + idOfUpdatedAnimal.ToString() + ";";
                    com.Parameters.AddWithValue("@name", updatedAnimal.Name);
                    com.Parameters.AddWithValue("@desc", updatedAnimal.Description);
                    com.Parameters.AddWithValue("@category", updatedAnimal.Category);
                    com.Parameters.AddWithValue("@area", updatedAnimal.Area);
                    con.Open();
                    com.ExecuteNonQuery();
                }
                return true;
            }
        }
        return false;
    }

    public bool DeleteAnimal(int idOfDeletedAnimal) {
        List<Animal> animals = GetAnimalsOrderedBy("name");
        foreach(Animal animal in animals)
        {
            if (animal.IdAnimal == idOfDeletedAnimal)
            {
                using(SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = @"DELETE FROM Animal WHERE IdAnimal = " + idOfDeletedAnimal.ToString() + ";";
                    con.Open();
                    com.ExecuteNonQuery();
                    return true;
                }
                
            }
        }
        return false;
    }
    //HELPER FUNCTIONS
    public List<Animal> GetAnimalsOrderedBy(string? orderBy)
    {
        if (orderBy == null)
        {
            orderBy = "name";
        }
        List<Animal> animals = new List<Animal>();
        using (SqlConnection con = new SqlConnection(connectionString))
        {

            SqlCommand com = new SqlCommand();
            com.Connection = con;
            //can't pass orderBy as argument for some reason??
            switch (orderBy)
            {
                case "name":
                    com.CommandText = "SELECT * FROM Animal ORDER BY Name";
                    break;
                case "description":
                    com.CommandText = "SELECT * FROM Animal ORDER BY Description";
                    break;
                case "category":
                    com.CommandText = "SELECT * FROM Animal ORDER BY Category";
                    break;
                case "area":
                    com.CommandText = "SELECT * FROM Animal ORDER BY Area";
                    break;
                //if orderBy doesn't match any of the valid options
                default:
                    return null;
            }

            con.Open();
            SqlDataReader dataReader = com.ExecuteReader();
            while (dataReader.Read())
            {
                animals.Add(new Animal((int)dataReader["IdAnimal"], dataReader["Name"].ToString(), dataReader["Description"].ToString(),
                    dataReader["Category"].ToString(), dataReader["Area"].ToString()));
            }

            return animals;
        }
    }
}
