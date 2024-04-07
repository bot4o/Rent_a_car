using Rent_a_car.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using System;

namespace Rent_a_car.Repository
{
    public class Data : IData
    {
        private readonly IConfiguration configuration;
        private readonly string dbcon = "";
        private readonly IWebHostEnvironment webhost;

        public Data(IConfiguration configuration, IWebHostEnvironment webhost)
        {
            this.configuration = configuration;
            dbcon = this.configuration.GetConnectionString("DefaultConnection");
            this.webhost = webhost;
        }
        public List<Driver> GetAllDrivers()
        {
            List<Driver> drivers = new List<Driver>();
            Driver dr;
            SqlConnection con = GetSqlConnection();
            try 
            {
               con.Open();
               string qry = "Select * from Drivers;";
               SqlDataReader reader = GetData(qry, con);
               while(reader.Read())
               {
                   dr = new Driver();
                   dr.Id = int.Parse(reader["ID"].ToString());
                   dr.Username = reader["Username"].ToString();
                   dr.Password = reader["Password"].ToString();
                   dr.First_Name = reader["First_Name"].ToString();
                   dr.Last_Name = reader["Last_Name"].ToString();
                   dr.EGN = reader["EGN"].ToString();
                   dr.Phone = reader["Phone"].ToString();
                   dr.Email = reader["Email"].ToString();
                   drivers.Add(dr);
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
            return drivers;
        }
        public bool AddDriver(Driver newdriver)
        {
            bool isSaved = false;
            SqlConnection con = GetSqlConnection();
            try
            {
                con.Open();
                string qry = String.Format("Insert into Drivers(Username, Password, First_Name, Last_Name, EGN, Phone, Email) values(" +
                        "'{0}','{1}','{2}','{3}','{4}','{5}','{6}')", newdriver.Username, newdriver.Password, newdriver.First_Name, newdriver.Last_Name, newdriver.EGN, newdriver.Phone, newdriver.Email);
                isSaved = SaveData(qry, con);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
            return isSaved;
        }
        public List<Car> GetAllCars()
        {
            var cars = new List<Car>();
            Car car;
            SqlConnection con = GetSqlConnection();
            try
            {
                con.Open();
                string qry = "Select * from Cars";
                SqlDataReader reader = GetData(qry, con);
                while (reader.Read())
                {
                    car = new Car();
                    car.Id = int.Parse(reader["ID"].ToString());
                    car.Brand = reader["Brand"].ToString();
                    car.Model = reader["Model"].ToString();
                    car.Year = int.Parse(reader["Year"].ToString());
                    car.Seats = int.Parse(reader["Seats"].ToString());
                    car.Description = reader["Description"].ToString();
                    car.Price = double.Parse(reader["Price"].ToString());
                    cars.Add(car);
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
            return cars;
        }
        private SqlDataReader GetData(string qry, SqlConnection con)
        {
            SqlDataReader reader = null;
            try
            {
                SqlCommand cmd = new SqlCommand(qry, con);
                reader = cmd.ExecuteReader();
            }
            catch (Exception)
            {
                throw;
            }
            return reader;
        }
        public bool AddNewCar(Car newcar)
        {
            bool isSaved = false;
            SqlConnection con = GetSqlConnection();
            try
            {
                con.Open();
                string qry = string.Format("INSERT INTO Cars(Brand, Model, Year, Seats, Description, Price) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')",
                        newcar.Brand, newcar.Model, newcar.Year, newcar.Seats, newcar.Description, newcar.Price);
                isSaved = SaveData(qry, con);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                // Close the connection if it's open
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return isSaved;
        }

        private SqlConnection GetSqlConnection()
        {
            return new SqlConnection(dbcon);
        }

        private bool SaveData(string qry, SqlConnection con)
        {
            bool isSaved = false;
            try
            {
                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.ExecuteNonQuery();
                isSaved = true;
            }
            catch (Exception)
            {
                throw;
            }
            return isSaved;
        }
    }
}

