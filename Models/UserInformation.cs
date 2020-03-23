using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace UserInfromationAPI.Models
{
   
    public class UserInformation : IUserInformation
    {
        SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=UserDB;Integrated Security=True;" +
            "Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        SqlCommand cmd;

        public bool findUserByID(int UserID)
        {
            con.Open();
            try
            {
                cmd = new SqlCommand("select UserID from UserInfo where UserID=@UserID", con);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                    return true;
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }
        }
        public string AddUser(User user)
        {
            //Address insert            
            try
            {
                con.Open();
                cmd = new SqlCommand("insert into UserInfo values (@FirstName, @LastName, @Email, @Department, @PhoneNumber, @Age, @Role)", con);
                cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                cmd.Parameters.AddWithValue("@LastName", user.LastName);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Department", user.Department);
                cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                cmd.Parameters.AddWithValue("@Age", user.Age);
                cmd.Parameters.AddWithValue("@Role", user.Role);
                cmd.ExecuteNonQuery();
                con.Close();


                con.Open();
                cmd = new SqlCommand("select UserID from UserInfo where Email=@Email", con);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.ExecuteNonQuery();
                SqlDataReader reader = cmd.ExecuteReader();
                int id = 0;
                if (reader.Read())
                {
                    id = (int)reader["UserID"];
                }
                con.Close();

                con.Open();
                //Console.WriteLine($" HouseNumber: {user.Address.HouseNumber } City: {user.Address.City }");
                cmd = new SqlCommand("insert into Address values (@UserID, @HouseNumber, @City, @State, @Country, @Pincode)", con);
                cmd.Parameters.AddWithValue("@UserID", id);
                cmd.Parameters.AddWithValue("@HouseNumber", user.Address.HouseNumber);
                cmd.Parameters.AddWithValue("@City", user.Address.City);
                cmd.Parameters.AddWithValue("@State", user.Address.State);
                cmd.Parameters.AddWithValue("@Country", user.Address.Country);
                cmd.Parameters.AddWithValue("@Pincode", user.Address.Pincode);
                cmd.ExecuteNonQuery();
                con.Close();
                return $"User with UserID : {id} has been created.";
            }
            catch 
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public List<User> GetAllUser(int? ID)
        {
            try
            {
                con.Open();
                if (ID == null)
                {
                    cmd = new SqlCommand("select * from UserInfo", con);
                }
                else
                {
                    cmd = new SqlCommand("select * from UserInfo where UserID=@UserID", con);
                    cmd.Parameters.AddWithValue("@UserID", ID);
                }

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    List<User> result = new List<User>();
                    while (reader.Read())
                    {
                        User item = new User()
                        {
                            UserID = (int)reader["UserID"],
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Department = reader["Department"].ToString(),
                            PhoneNumber = reader["PhoneNumber"].ToString(),
                            Email = reader["Email"].ToString(),
                            Age = (int)reader["Age"],
                            Role = reader["Role"].ToString(),
                            // Address = reader["Address"].ToString()
                            //Address = address                    
                        };
                        result.Add(item);
                    }
                    con.Close();
                    con.Open();
                    if (ID == null)
                    {
                        cmd = new SqlCommand("select * from Address", con);
                    }
                    else
                    {
                        cmd = new SqlCommand("select * from Address where UserID=@UserID", con);
                        cmd.Parameters.AddWithValue("@UserID", ID);
                    }
                    reader = cmd.ExecuteReader();

                    List<Address> addressList = new List<Address>();
                    while (reader.Read())
                    {
                        Address address = new Address()
                        {
                            HouseNumber = reader["HouseNumber"].ToString(),
                            City = reader["City"].ToString(),
                            State = reader["State"].ToString(),
                            Country = reader["Country"].ToString(),
                            Pincode = (int)reader["Pincode"]
                        };
                        addressList.Add(address);
                    }
                    con.Close();

                    for (var i = 0; i < addressList.Count; i++)
                    {
                        result[i].Address = addressList[i];
                    }
                    return result;
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }
        }
        public string DeleteUser(int ID)
        {
            try
            {
                con.Open();
                cmd = new SqlCommand("delete from Address where UserID=@UserID ", con);
                cmd.Parameters.AddWithValue("@UserID", ID);
                int UserExist = cmd.ExecuteNonQuery();
                if (UserExist == 0)
                {
                    con.Close();
                    return $"User with ID: {ID} not found";
                }
                con.Close();
                con.Open();
                cmd = new SqlCommand("delete from UserInfo where UserID=@UserID ", con);
                cmd.Parameters.AddWithValue("@UserID", ID);
                UserExist = cmd.ExecuteNonQuery();
                if (UserExist == 0)
                {
                    con.Close();
                    return $"User with ID: {ID} not found";
                }
                con.Close();
                return $"User having ID: {ID} has been removed.";
            }
            catch
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }
        public string UpdateUser(int ID, User user)
        {
            try
            {
                if (user == null)
                {
                    return "Not data available to update.";
                }

                if (findUserByID(ID))
                {
                    StringBuilder updateQuery = new StringBuilder("update UserInfo SET ");
                    if (String.IsNullOrEmpty(user.FirstName) == false)
                    {
                        updateQuery.Append(" FirstName = @FirstName,");
                    }
                    if (String.IsNullOrEmpty(user.LastName) == false)
                    {
                        updateQuery.Append(" LastName = @LastName,");
                    }
                    if (String.IsNullOrEmpty(user.Email) == false)
                    {
                        updateQuery.Append(" Email = @Email,");
                    }
                    if (String.IsNullOrEmpty(user.Department) == false)
                    {
                        updateQuery.Append(" Department = @Department,");
                    }
                    if (String.IsNullOrEmpty(user.PhoneNumber) == false)
                    {
                        updateQuery.Append(" PhoneNumber = @PhoneNumber,");
                    }
                    if (user.Age != 0)
                    {
                        updateQuery.Append(" Age = @Age,");
                    }
                    if (String.IsNullOrEmpty(user.Role) == false)
                    {
                        updateQuery.Append(" Role = @Role,");
                    }
                    string updateUserQuery = updateQuery.ToString();
                    con.Open();
                    cmd = new SqlCommand(updateUserQuery.Substring(0, updateUserQuery.Length - 1) + " where UserID=@UserID ", con);

                    if (String.IsNullOrEmpty(user.FirstName) == false)
                    {
                        cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                    }
                    if (String.IsNullOrEmpty(user.LastName) == false)
                    {
                        cmd.Parameters.AddWithValue("@LastName", user.LastName);
                    }
                    if (String.IsNullOrEmpty(user.Email) == false)
                    {
                        cmd.Parameters.AddWithValue("@Email", user.Email);
                    }
                    if (String.IsNullOrEmpty(user.Department) == false)
                    {
                        cmd.Parameters.AddWithValue("@Department", user.Department);
                    }
                    if (String.IsNullOrEmpty(user.PhoneNumber) == false)
                    {
                        cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                    }
                    if ((user.Age) != 0)
                    {
                        cmd.Parameters.AddWithValue("@Age", user.Age);
                    }
                    if (String.IsNullOrEmpty(user.Role) == false)
                    {
                        cmd.Parameters.AddWithValue("@Role", user.Role);
                    }
                    cmd.Parameters.AddWithValue("@UserID", ID);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    if (user.Address != null)
                    {

                        StringBuilder updateAddress = new StringBuilder("update Address SET ");
                        if (String.IsNullOrEmpty(user.Address.HouseNumber) == false)
                        {
                            updateAddress.Append(" HouseNumber = @HouseNumber,");
                        }
                        if (String.IsNullOrEmpty(user.Address.City) == false)
                        {
                            updateAddress.Append(" City = @City,");
                        }
                        if (String.IsNullOrEmpty(user.Address.State) == false)
                        {
                            updateAddress.Append(" State = @State,");
                        }
                        if (String.IsNullOrEmpty(user.Address.Country) == false)
                        {
                            updateAddress.Append(" Country = @Country,");
                        }
                        if (user.Address.Pincode != 0)
                        {
                            updateAddress.Append(" Pincode = @Pincode,");
                        }


                        string updateAddressQuery = updateAddress.ToString();
                        con.Open();
                        cmd = new SqlCommand(updateAddressQuery.Substring(0, updateAddressQuery.Length - 1) + " where UserID=@UserID ", con);

                        if (String.IsNullOrEmpty(user.Address.HouseNumber) == false)
                        {
                            cmd.Parameters.AddWithValue("@HouseNumber", user.Address.HouseNumber);
                        }
                        if (String.IsNullOrEmpty(user.Address.City) == false)
                        {
                            cmd.Parameters.AddWithValue("@City", user.Address.City);
                        }
                        if (String.IsNullOrEmpty(user.Address.State) == false)
                        {
                            cmd.Parameters.AddWithValue("@State", user.Address.State);
                        }
                        if (String.IsNullOrEmpty(user.Address.Country) == false)
                        {
                            cmd.Parameters.AddWithValue("@Country", user.Address.Country);
                        }
                        if (user.Address.Pincode != 0)
                        {
                            cmd.Parameters.AddWithValue("@Pincode", user.Address.Pincode);
                        }

                        cmd.Parameters.AddWithValue("@UserID", ID);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    return "User data has been updated.";
                }
                else
                {
                    return $"User with ID: {ID} does not exist.";
                }                
            }
            catch 
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }
    }
}
