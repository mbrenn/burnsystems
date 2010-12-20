﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using BurnSystems.Database.Objects;

namespace BurnSystems.UnitTests.Database.Objects
{
    /// <summary>
    /// These tests check if the Mapper class is correctly working. 
    /// These tests don't need database access
    /// </summary>
    [TestFixture]
    public class MapperTests
    {
        [Test]
        public void TestNoDatabaseClassAttribute()
        {
            Assert.Throws<TypeInitializationException>(() => new Converter<NoDatabaseClassAttribute>());
        }

        [Test]
        public void TestMultipleDatabaseKeyAttributes()
        {
            Assert.Throws<TypeInitializationException>(() => new Converter<MultipleDatabaseKeyAttributes>());
        }

        [Test]
        public void TestNoDatabaseKeyAttributes()
        {
            Assert.Throws<TypeInitializationException>(() => new Converter<NoDatabaseKeyAttributes>());
        }

        [Test]
        public void TestInvalidDatabaseKeyType()
        {
            Assert.Throws<TypeInitializationException>(() => new Converter<InvalidDatabaseKeyType>());
        }

        [Test]
        public void TestValidClassMapperCreation()
        {
            var mapper = new Converter<Person>();
        }

        [Test]
        public void TestConversionToInstance()
        {
            var data = new Dictionary<string, object>();
            data["Id"] = 12;
            data["Prename"] = "Karl";
            data["Name"] = "Heinz";
            data["Age"] = 54;
            data["Weight"] = 76.54;
            data["Sex"] = "Male";

            var mapper = new Converter<Person>();
            var person = mapper.ConvertToInstance(data);

            Assert.That(person.Id, Is.EqualTo(12));
            Assert.That(person.Prename, Is.EqualTo("Karl"));
            Assert.That(person.Name, Is.EqualTo("Heinz"));
            Assert.That(person.Age_Temp, Is.EqualTo(54));
            Assert.That(person.Weight, Is.EqualTo(76.54));
            Assert.That(person.Sex, Is.EqualTo(Sex.Male));
            Assert.That(person.Obsolete, Is.Null);
        }

        [Test]
        public void TestConversionToDatabaseObject()
        {
            var person = new Person();
            person.Id = 12;
            person.Prename = "Karl";
            person.Name = "Heinz";
            person.Age_Temp = 54;
            person.Weight = 76.54;
            person.Sex = Sex.Male;
            person.Obsolete = "ABC";

            var mapper = new Converter<Person>();
            var data = mapper.ConvertToDatabaseObject(person);

            Assert.That(data["Id"], Is.EqualTo(12));
            Assert.That(data["Prename"], Is.EqualTo("Karl"));
            Assert.That(data["Name"], Is.EqualTo("Heinz"));
            Assert.That(data["Age"], Is.EqualTo(54));
            Assert.That(data["Weight"], Is.EqualTo(76.54));
            Assert.That(data["Sex"], Is.EqualTo("Male"));
            Assert.That(data.ContainsKey("Obsolete"), Is.False);
        }

        public class NoDatabaseClassAttribute
        {
            public string Name
            {
                get;
                set;
            }
        }

        [DatabaseClass]
        public class MultipleDatabaseKeyAttributes
        {
            [DatabaseKey("Id1")]
            public int Id
            {
                get;
                set;
            }

            [DatabaseKey("Id2")]
            public int Id2
            {
                get;
                set;
            }
        }

        [DatabaseClass]
        public class NoDatabaseKeyAttributes
        {
            public int Id
            {
                get;
                set;
            }

            public int Id2
            {
                get;
                set;
            }
        }

        [DatabaseClass]
        public class InvalidDatabaseKeyType
        {
            [DatabaseKey("Id")]
            public string Id
            {
                get;
                set;
            }

            /// <summary>
            /// 
            /// </summary>
            [DatabaseProperty("Prename")]
            public string Prename
            {
                get;
                set;
            }
        }

        public enum Sex
        {
            Male,
            Female
        }

        [DatabaseClass]
        public class Person
        {
            [DatabaseKey("Id")]
            public int Id
            {
                get;
                set;
            }

            /// <summary>
            /// 
            /// </summary>
            [DatabaseProperty("Prename")]
            public string Prename
            {
                get;
                set;
            }

            [DatabaseProperty("Name")]
            public string Name
            {
                get;
                set;
            }

            [DatabaseProperty("Age")]
            public int Age_Temp
            {
                get;
                set;
            }

            [DatabaseProperty("Weight")]
            public double Weight
            {
                get;
                set;
            }

            [DatabaseProperty("Sex")]
            public Sex Sex
            {
                get;
                set;
            }

            public string Obsolete
            {
                get;
                set;
            }
        }
    }
}
