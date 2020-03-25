using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Microsoft.Extensions.Configuration;
using UserInfromationAPI.Models;

namespace UserInfromationAPI.DAL
{
    public static class DbConnectionString
    {
       public static string ConnectionString =  new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()["ConnectionString"];
    }
    public class UserData : IUserData
    {
        private readonly SqlConnection _con = new SqlConnection(DbConnectionString.ConnectionString);
        private SqlCommand _cmd;

        public bool UserExist(int userId)
        {
            _con.Open();
            try
            {
                _cmd = new SqlCommand("select UserID from UserInfo where UserID=@UserID", _con);
                _cmd.Parameters.AddWithValue("@UserID", userId);
                var reader = _cmd.ExecuteReader();
                return reader.HasRows;
            }
            finally
            {
                _con.Close();
            }
        }
        public string AddUser(User user)
        {           
            try
            {
                _con.Open();
                _cmd = new SqlCommand("insert into UserInfo values (@FirstName, @LastName, @Email, @Department, @PhoneNumber, @Age, @Role)", _con);
                _cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                _cmd.Parameters.AddWithValue("@LastName", user.LastName);
                _cmd.Parameters.AddWithValue("@Email", user.Email);
                _cmd.Parameters.AddWithValue("@Department", user.Department);
                _cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                _cmd.Parameters.AddWithValue("@Age", user.Age);
                _cmd.Parameters.AddWithValue("@Role", user.Role);
                _cmd.ExecuteNonQuery();
                _con.Close();
                _con.Open();
                _cmd = new SqlCommand("select UserID from UserInfo where Email=@Email", _con);
                _cmd.Parameters.AddWithValue("@Email", user.Email);
                _cmd.ExecuteNonQuery();
                var reader = _cmd.ExecuteReader();
                var id = 0;
                if (reader.Read())
                {
                    id = (int)reader["UserID"];
                }
                _con.Close();

                _con.Open();
                _cmd = new SqlCommand("insert into Address values (@UserID, @HouseNumber, @City, @State, @Country, @Pincode)", _con);
                _cmd.Parameters.AddWithValue("@UserID", id);
                _cmd.Parameters.AddWithValue("@HouseNumber", user.Address.HouseNumber);
                _cmd.Parameters.AddWithValue("@City", user.Address.City);
                _cmd.Parameters.AddWithValue("@State", user.Address.State);
                _cmd.Parameters.AddWithValue("@Country", user.Address.Country);
                _cmd.Parameters.AddWithValue("@Pincode", user.Address.Pincode);
                _cmd.ExecuteNonQuery();
                _con.Close();
                return $"User with UserID : {id} has been created.";
            }
            finally
            {
                _con.Close();
            }
        }

        public List<User> GetAllUser(int? id)
        {
            try
            {
                _con.Open();
                if (id == null)
                    _cmd = new SqlCommand("select * from UserInfo", _con);
                else
                {
                    _cmd = new SqlCommand("select * from UserInfo where UserID=@UserID", _con);
                    _cmd.Parameters.AddWithValue("@UserID", id);
                }

                var reader = _cmd.ExecuteReader();
                if (!reader.HasRows) return null;
                var result = new List<User>();
                while (reader.Read())
                {
                    var item = new User()
                    {
                        UserId = (int)reader["UserID"],
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Department = reader["Department"].ToString(),
                        PhoneNumber = reader["PhoneNumber"].ToString(),
                        Email = reader["Email"].ToString(),
                        Age = (int)reader["Age"],
                        Role = reader["Role"].ToString()         
                    };
                    result.Add(item);
                }
                _con.Close();
                _con.Open();
                if (id == null)
                {
                    _cmd = new SqlCommand("select * from Address", _con);
                }
                else
                {
                    _cmd = new SqlCommand("select * from Address where UserID=@UserID", _con);
                    _cmd.Parameters.AddWithValue("@UserID", id);
                }
                reader = _cmd.ExecuteReader();

                var addressList = new List<Address>();
                while (reader.Read())
                {
                    var address = new Address()
                    {
                        HouseNumber = reader["HouseNumber"].ToString(),
                        City = reader["City"].ToString(),
                        State = reader["State"].ToString(),
                        Country = reader["Country"].ToString(),
                        Pincode = (int)reader["Pincode"]
                    };
                    addressList.Add(address);
                }
                _con.Close();

                for (var i = 0; i < addressList.Count; i++)
                {
                    result[i].Address = addressList[i];
                }
                return result;
            }
            finally
            {
                _con.Close();
            }
        }
        public string DeleteUser(int id)
        {
            try
            {
                _con.Open();
                _cmd = new SqlCommand("delete from Address where UserID=@UserID ", _con);
                _cmd.Parameters.AddWithValue("@UserID", id);
                var userExist = _cmd.ExecuteNonQuery();
                if (userExist == 0)
                {
                    _con.Close();
                    return $"User with ID: {id} not found";
                }
                _con.Close();
                _con.Open();
                _cmd = new SqlCommand("delete from UserInfo where UserID=@UserID ", _con);
                _cmd.Parameters.AddWithValue("@UserID", id);
                userExist = _cmd.ExecuteNonQuery();
                if (userExist == 0)
                {
                    _con.Close();
                    return $"User with ID: {id} not found";
                }
                _con.Close();
                return $"User having ID: {id} has been removed.";
            }
            finally
            {
                _con.Close();
            }
        }
        public string UpdateUser(int id, User user)
        {
            try
            {
                if (user == null)
                {
                    return "Not data available to update.";
                }
                if (UserExist(id))
                {
                    var updateQuery = new StringBuilder("update UserInfo SET ");
                    if (string.IsNullOrEmpty(user.FirstName) == false)
                    {
                        updateQuery.Append(" FirstName = @FirstName,");
                    }
                    if (string.IsNullOrEmpty(user.LastName) == false)
                    {
                        updateQuery.Append(" LastName = @LastName,");
                    }
                    if (string.IsNullOrEmpty(user.Email) == false)
                    {
                        updateQuery.Append(" Email = @Email,");
                    }
                    if (string.IsNullOrEmpty(user.Department) == false)
                    {
                        updateQuery.Append(" Department = @Department,");
                    }
                    if (string.IsNullOrEmpty(user.PhoneNumber) == false)
                    {
                        updateQuery.Append(" PhoneNumber = @PhoneNumber,");
                    }
                    if (user.Age != 0)
                    {
                        updateQuery.Append(" Age = @Age,");
                    }
                    if (string.IsNullOrEmpty(user.Role) == false)
                    {
                        updateQuery.Append(" Role = @Role,");
                    }
                    var updateUserQuery = updateQuery.ToString();
                    _con.Open();
                    _cmd = new SqlCommand(updateUserQuery.Substring(0, updateUserQuery.Length - 1) + " where UserID=@UserID ", _con);

                    if (string.IsNullOrEmpty(user.FirstName) == false)
                    {
                        _cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                    }
                    if (string.IsNullOrEmpty(user.LastName) == false)
                    {
                        _cmd.Parameters.AddWithValue("@LastName", user.LastName);
                    }
                    if (string.IsNullOrEmpty(user.Email) == false)
                    {
                        _cmd.Parameters.AddWithValue("@Email", user.Email);
                    }
                    if (string.IsNullOrEmpty(user.Department) == false)
                    {
                        _cmd.Parameters.AddWithValue("@Department", user.Department);
                    }
                    if (string.IsNullOrEmpty(user.PhoneNumber) == false)
                    {
                        _cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                    }
                    if ((user.Age) != 0)
                    {
                        _cmd.Parameters.AddWithValue("@Age", user.Age);
                    }
                    if (string.IsNullOrEmpty(user.Role) == false)
                    {
                        _cmd.Parameters.AddWithValue("@Role", user.Role);
                    }
                    _cmd.Parameters.AddWithValue("@UserID", id);
                    _cmd.ExecuteNonQuery();
                    _con.Close();
                    if (user.Address == null) return "User data has been updated.";
                    var updateAddress = new StringBuilder("update Address SET ");
                    if (string.IsNullOrEmpty(user.Address.HouseNumber) == false)
                    {
                        updateAddress.Append(" HouseNumber = @HouseNumber,");
                    }
                    if (string.IsNullOrEmpty(user.Address.City) == false)
                    {
                        updateAddress.Append(" City = @City,");
                    }
                    if (string.IsNullOrEmpty(user.Address.State) == false)
                    {
                        updateAddress.Append(" State = @State,");
                    }
                    if (string.IsNullOrEmpty(user.Address.Country) == false)
                    {
                        updateAddress.Append(" Country = @Country,");
                    }
                    if (user.Address.Pincode != 0)
                    {
                        updateAddress.Append(" Pincode = @Pincode,");
                    }


                    var updateAddressQuery = updateAddress.ToString();
                    _con.Open();
                    _cmd = new SqlCommand(updateAddressQuery.Substring(0, updateAddressQuery.Length - 1) + " where UserID=@UserID ", _con);

                    if (string.IsNullOrEmpty(user.Address.HouseNumber) == false)
                    {
                        _cmd.Parameters.AddWithValue("@HouseNumber", user.Address.HouseNumber);
                    }
                    if (string.IsNullOrEmpty(user.Address.City) == false)
                    {
                        _cmd.Parameters.AddWithValue("@City", user.Address.City);
                    }
                    if (string.IsNullOrEmpty(user.Address.State) == false)
                    {
                        _cmd.Parameters.AddWithValue("@State", user.Address.State);
                    }
                    if (string.IsNullOrEmpty(user.Address.Country) == false)
                    {
                        _cmd.Parameters.AddWithValue("@Country", user.Address.Country);
                    }
                    if (user.Address.Pincode != 0)
                    {
                        _cmd.Parameters.AddWithValue("@Pincode", user.Address.Pincode);
                    }
                    _cmd.Parameters.AddWithValue("@UserID", id);
                    _cmd.ExecuteNonQuery();
                    _con.Close();
                    return "User data has been updated.";
                }
                else
                    return $"User with ID: {id} does not exist.";
            }
            finally
            {
                _con.Close();
            }
        }
    }
}
