using System;
using System.Collections.Generic;
using System.Data;

namespace Vulnerable.Sample.WebApp.People 
{
    public interface IPeopleService 
    {
        IEnumerable<People> FindPeopleByName(string name);
    }

    public class PeopleService : IPeopleService 
    {
        private readonly IDbConnection _connection;

        public PeopleService(IDbConnection connection) => 
            _connection = connection;

        public IEnumerable<People> FindPeopleByName(string name) 
        {
            var list = new List<People>();

            using(_connection) 
            using(var command = _connection.CreateCommand())
            {
                command.CommandText = $"SELECT ID, NAME FROM PEOPLE WHERE NAME LIKE '%{name}%' ORDER BY NAME ASC";
                command.CommandTimeout = 5000;

                using(var reader = command.ExecuteReader())
                while(reader.Read())
                {
                    var (idPeople, namePeople) = (Convert.ToInt32(reader["Id"]), reader["Name"].ToString());
                    list.Add(new People(idPeople, namePeople));
                }
            }

            return list;
        }
    }
}
