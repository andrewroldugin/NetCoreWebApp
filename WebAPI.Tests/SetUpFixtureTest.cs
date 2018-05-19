using Dapper;
using NUnit.Framework;
using WebAPI.Models;

namespace Tests
{
    [SetUpFixture]
    public class SetUpFixtureTest
    {
        PersonRepository repo = new PersonRepository();
        [OneTimeSetUp]
        public void Setup()
        {
            repo.connectionString = "server=127.0.0.1;uid=root;pwd=root";
            using (var db = repo.Connect())
            {
                db.Execute("CREATE DATABASE persons_test; USE persons_test;");
                db.Execute(System.IO.File.ReadAllText("../../../../WebAPI/sql/tables.sql"));
            }
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            using (var db = repo.Connect())
            {
                db.Execute("DROP DATABASE persons_test;");
            }
        }
    }
}