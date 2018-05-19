using Dapper;
using NUnit.Framework;
using System.Data;
using System.Linq;
using WebAPI.Models;

namespace Tests.Models
{
    public class PersonRepositoryTest
    {
        PersonRepository repo = new PersonRepository();

        void PopulateDb(IDbConnection db)
        {
            db.Execute(System.IO.File.ReadAllText("../../../../WebAPI/sql/data.sql"));
        }

        [SetUp]
        public void Setup()
        {
        }

        [TearDown]
        public void TearDown()
        {
            using (var db = repo.Connect())
            {
                db.Execute("TRUNCATE tblpersons");
            }
        }
        
        [Test]
        public void ReturnsPersons()
        {
            using (var db = repo.Connect())
            {
                Assert.AreEqual(repo.GetPersons().Count, 0);
                PopulateDb(db);
                Assert.AreEqual(repo.GetPersons().Count, 3);
            }
        }

        [Test]
        public void ReturnsNullWhenPersonNotFound()
        {
            using (var db = repo.Connect())
            {
                Assert.IsNull(repo.Get(5));
            }
        }

        [Test]
        public void ReturnsPersonById()
        {
            using (var db = repo.Connect())
            {
                PopulateDb(db);
                var person = repo.Get(3);
                Assert.IsNotNull(person);
                Assert.AreEqual(3, person.id);
                Assert.AreEqual("John", person.first_name);
                Assert.AreEqual("Petrucci", person.last_name);
            }
        }

        [Test]
        public void UpdatesPerson()
        {
            using (var db = repo.Connect())
            {
                PopulateDb(db);
                var expected = new Person {id = 3, first_name = "Allan", last_name = "Key", phone = "+123321123"};
                repo.Update(expected);
                var actual = db.Query<Person>("SELECT * FROM tblpersons WHERE id=@id", new {id = 3}).FirstOrDefault();
                Assert.AreEqual(expected.first_name, actual.first_name);
                Assert.AreEqual(expected.last_name, actual.last_name);
                Assert.AreEqual(expected.phone, actual.phone);
            }
        }

        [Test]
        public void CreatesPerson()
        {
            using (var db = repo.Connect())
            {
                PopulateDb(db);
                var fn = "Allan";
                var ln = "Key";
                var ph = "+123321123";
                var person = new Person {first_name = fn, last_name = ln, phone = ph};
                var createdPerson = repo.Create(person);
                Assert.AreEqual(4, createdPerson.id);
                var dbPerson = db.Query<Person>("SELECT * FROM tblpersons WHERE id=@id", new {id = 4}).FirstOrDefault();
                Assert.IsNotNull(dbPerson);
                Assert.AreEqual(createdPerson.first_name, dbPerson.first_name);
                Assert.AreEqual(createdPerson.last_name, dbPerson.last_name);
                Assert.AreEqual(createdPerson.phone, dbPerson.phone);
            }
        }

        [Test]
        public void DeletesPerson()
        {
            using (var db = repo.Connect())
            {
                PopulateDb(db);
                repo.Delete(3);
                var person = db.Query<Person>("SELECT * FROM tblpersons WHERE id=@id", new {id = 3}).FirstOrDefault();
                Assert.IsNull(person);
            }
        }
    }
}