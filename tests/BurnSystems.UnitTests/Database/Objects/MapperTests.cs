﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Data.SqlClient;
using System.Data.Common;
using BurnSystems.AdoNet.Queries;
using BurnSystems.Database.Objects;

namespace BurnSystems.UnitTests.Database.Objects
{
    [TestFixture]
    public class MapperTests
    {
        public DbConnection GetDatabaseConnection()
        {
            var sqlConnection = new SqlConnection("Server=localhost\\SQLEXPRESS;Database=mb_test;Trusted_Connection=True;");
            sqlConnection.Open();

            var deleteQuery = new DeleteQuery("persons");
            sqlConnection.ExecuteNonQuery(deleteQuery);

            return sqlConnection;
        }

        [Test]
        public void TestDatabaseConnection()
        {
            using (var sqlConnection = this.GetDatabaseConnection())
            {
            }
        }

        [Test]
        public void TestInsertPerson()
        {
            using (var sqlConnection = this.GetDatabaseConnection())
            {
                var mapper = new Mapper<Person>("persons", sqlConnection);

                Person person;
                Person person2;
                Person person3;
                InsertPersons(mapper, out person, out person2, out person3);

                Assert.That(person.Id, Is.Not.EqualTo(0));
                Assert.That(person2.Id, Is.Not.EqualTo(0));
                Assert.That(person3.Id, Is.Not.EqualTo(0));
                Assert.That(person.Id, Is.Not.EqualTo(person2.Id));
                Assert.That(person2.Id, Is.Not.EqualTo(person3.Id));
                Assert.That(person.Id, Is.Not.EqualTo(person3.Id));
            }
        }

        [Test]
        public void TestSelectPerson()
        {
            using (var sqlConnection = this.GetDatabaseConnection())
            {
                var mapper = new Mapper<Person>("persons", sqlConnection);
                InsertPersons(mapper);

                var persons = mapper.GetAll();

                Assert.That(persons.Length, Is.EqualTo(3));
                Assert.That(persons[0].Id, Is.Not.EqualTo(0));
                Assert.That(persons[0].Id, Is.Not.EqualTo(persons[1].Id));
                Assert.That(persons[1].Id, Is.Not.EqualTo(persons[2].Id));
                Assert.That(persons[0].Id, Is.Not.EqualTo(persons[2].Id));

                var karl = persons.Where(x => x.Prename == "Karl").FirstOrDefault();
                Assert.That(karl, Is.Not.Null);

                Assert.That(karl.Name, Is.EqualTo("Heinz"));
                Assert.That(karl.Age_Temp, Is.EqualTo(12));
                Assert.That(karl.Weight, Is.EqualTo(12.34));
                Assert.That(karl.Sex, Is.EqualTo(Sex.Male));
            }
        }

        [Test]
        public void TestSelectPersonByWhere()
        {
            using (var sqlConnection = this.GetDatabaseConnection())
            {
                var mapper = new Mapper<Person>("persons", sqlConnection);
                InsertPersons(mapper);

                var where = new Dictionary<string, object>();
                where["Prename"] = "Karl";
                var persons = mapper.Get(where);

                Assert.That(persons.Length, Is.EqualTo(1));

                var karl = persons.Where(x => x.Prename == "Karl").FirstOrDefault();
                Assert.That(karl, Is.Not.Null);

                Assert.That(karl.Name, Is.EqualTo("Heinz"));
                Assert.That(karl.Age_Temp, Is.EqualTo(12));
                Assert.That(karl.Weight, Is.EqualTo(12.34));
                Assert.That(karl.Sex, Is.EqualTo(Sex.Male));
            }
        }

        [Test]
        public void TestSelectPersonById()
        {
            using (var sqlConnection = this.GetDatabaseConnection())
            {
                var mapper = new Mapper<Person>("persons", sqlConnection);
                InsertPersons(mapper);

                var persons = mapper.GetAll();

                var karl = persons.Where(x => x.Prename == "Karl").FirstOrDefault();
                Assert.That(karl, Is.Not.Null);

                var karl2 = mapper.Get(karl.Id);

                Assert.That(karl2.Name, Is.EqualTo(karl.Name));
                Assert.That(karl2.Age_Temp, Is.EqualTo(karl.Age_Temp));
                Assert.That(karl2.Weight, Is.EqualTo(karl.Weight));
                Assert.That(karl2.Sex, Is.EqualTo(karl.Sex));
            }
        }

        [Test]
        public void TestDeletePersonByInstance()
        {
            using (var sqlConnection = this.GetDatabaseConnection())
            {
                var mapper = new Mapper<Person>("persons", sqlConnection);
                InsertPersons(mapper);

                var persons = mapper.GetAll();

                var karl = persons.Where(x => x.Prename == "Karl").FirstOrDefault();
                Assert.That(karl, Is.Not.Null);

                mapper.Delete(karl);

                persons = mapper.GetAll();
                karl = persons.Where(x => x.Prename == "Karl").FirstOrDefault();
                Assert.That(karl, Is.Null);
            }
        }

        [Test]
        public void TestDeletePersonById()
        {
            using (var sqlConnection = this.GetDatabaseConnection())
            {
                var mapper = new Mapper<Person>("persons", sqlConnection);
                InsertPersons(mapper);

                var persons = mapper.GetAll();

                var karl = persons.Where(x => x.Prename == "Karl").FirstOrDefault();
                Assert.That(karl, Is.Not.Null);

                mapper.Delete(karl.Id);

                persons = mapper.GetAll();
                karl = persons.Where(x => x.Prename == "Karl").FirstOrDefault();
                Assert.That(karl, Is.Null);
            }
        }

        [Test]
        public void TestUpdatePerson()
        {
            using (var sqlConnection = this.GetDatabaseConnection())
            {
                var mapper = new Mapper<Person>("persons", sqlConnection);
                InsertPersons(mapper);

                var persons = mapper.GetAll();

                var karl = persons.Where(x => x.Prename == "Karl").FirstOrDefault();
                Assert.That(karl, Is.Not.Null);
                Assert.That(karl.Name, Is.EqualTo("Heinz"));

                karl.Name = "Mommenschatz";
                mapper.Update(karl);

                persons = mapper.GetAll();
                karl = persons.Where(x => x.Prename == "Karl").FirstOrDefault();
                Assert.That(karl, Is.Not.Null);
                Assert.That(karl.Name, Is.EqualTo("Mommenschatz"));
            }
        }

        /// <summary>
        /// Inserts three persons into database
        /// </summary>
        /// <param name="mapper">Mapper to be used</param>
        private static void InsertPersons(Mapper<Person> mapper)
        {
            Person p1;
            Person p2;
            Person p3;
            InsertPersons(mapper, out p1, out p2, out p3);
        }

        /// <summary>
        /// Inserts three persons into database
        /// </summary>
        /// <param name="mapper">Mapper to be used</param>
        /// <param name="person">First person that has been added</param>
        /// <param name="person2">Second person that has been added</param>
        /// <param name="person3">Third person that has been added</param>
        private static void InsertPersons(Mapper<Person> mapper, out Person person, out Person person2, out Person person3)
        {
            person = new Person();
            person.Prename = "Karl";
            person.Name = "Heinz";
            person.Age_Temp = 12;
            person.Weight = 12.34;
            person.Sex = Sex.Male;
            person.Obsolete = "ABC";

            mapper.Add(person);

            person2 = new Person();
            person2.Prename = "Otto";
            person2.Name = "Meier";
            person2.Age_Temp = 34;
            person2.Weight = 56.78;
            person2.Sex = Sex.Male;
            person2.Obsolete = "DEF";

            mapper.Add(person2);

            person3 = new Person();
            person3.Prename = "Gertrud";
            person3.Name = "Müller";
            person3.Age_Temp = 56;
            person3.Weight = 90.12;
            person3.Sex = Sex.Female;
            person3.Obsolete = "ABC";

            mapper.Add(person3);
        }
    }
}