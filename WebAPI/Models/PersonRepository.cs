using System;
using System.Collections.Generic;
//using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using MySql.Data.MySqlClient;
using Dapper;
 
namespace WebAPI.Models
{
    public class PersonRepository
    {
        //string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        string connectionString = "server=127.0.0.1;uid=root;pwd=root;database=persons";
        public List<Person> GetPersons()
        {
            List<Person> persons = new List<Person>();
            using(IDbConnection db = new MySqlConnection(connectionString))
            {
                persons = db.Query<Person>("SELECT * FROM tblPersons").ToList();
            }
            return persons;
        }
 
        public Person Get(int id)
        {
            Person person = null;
            using (IDbConnection db = new MySqlConnection(connectionString))
            {
                person = db.Query<Person>("SELECT * FROM tblPersons WHERE id = @id", new { id }).FirstOrDefault();
            }
            return person;
        }
 
        public Person Create(Person person)
        {
            using (IDbConnection db = new MySqlConnection(connectionString))
            {
                var sqlQuery = "INSERT INTO tblPersons VALUES(0, @first_name, @last_name, @phone); SELECT LAST_INSERT_ID()";
                person.id = db.Query<int>(sqlQuery, person).FirstOrDefault();
            }
            return person;
        }
 
        public void Update(Person person)
        {
            using (IDbConnection db = new MySqlConnection(connectionString))
            {
                var sqlQuery = "UPDATE tblPersons SET first_name = @first_name, last_name = @last_name, phone = @phone WHERE id = @id";
                db.Execute(sqlQuery, person);
            }
        }
 
        public void Delete(int id)
        {
             using (IDbConnection db = new MySqlConnection(connectionString))
             {
                 var sqlQuery = "DELETE FROM tblPersons WHERE id = @id";
                 db.Execute(sqlQuery, new { id });
             }
        }
    }
}