using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace CampTrack_MVP1
{

    public class Database
    {
        private string Host { get; }
        private string Source { get; }
        private string Port { get; }
        private string User { get; }
        private string Password { get; }
        public string StrProvider { get; }

        /// <summary>
        /// Default contructor for Database class.
        /// </summary>
        public Database()
        {

            Host = "192.168.172.40";
            Source = "camptrack";
            Port = "3306";
            User = "maya";
            Password = "Hallo.013";
            StrProvider = SetStrProvider(Host, Source, Port, User, Password);

        }

        /// <summary>
        /// Custom constructor for Database class.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="source"></param>
        /// <param name="port"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        public Database(string host, string source, string port, string user, string password)
        {

            Host = host;
            Source = source;
            Port = port;
            User = user;
            Password = password;
            StrProvider = SetStrProvider(Host, Source, Port, User, Password);

        }

        //method to build the string in constructors.
        private string SetStrProvider(string host, string source, string port, string user, string password)
        {

            string StrProvider = "server=" + host + ";port=" + port + ";database=" + source + ";uid=" + user + ";password=" + password;
            return StrProvider;

        }

        public string CheckName(string cardNumber)
        {
            MySqlConnection cnn = new MySqlConnection(StrProvider);
            string query = "SELECT lastname FROM client WHERE cardnumber='" + cardNumber + "'";
            MySqlCommand commandDB = new MySqlCommand(query, cnn);
            MySqlDataReader reader;

            string lastName = null;


            try
            {
                cnn.Open();

                reader = commandDB.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        lastName = reader.GetString(0);
                    }
                }
                else
                {
                    Console.WriteLine("No name found.");
                }

                cnn.Close();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return lastName;
        }

        private string loadValue(string query, string value)
        {

            MySqlConnection cnn = new MySqlConnection(StrProvider);
            MySqlCommand cmd = cnn.CreateCommand();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("?value", value);
            MySqlDataReader reader;

            try
            {
                cnn.Open();

                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {

                    while (reader.Read())
                    {

                        return reader.GetString(0);

                    }
                }
                else
                {
                    Console.WriteLine("No value found.");
                }

                cnn.Close();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;

        }
        private bool loadBool(string query, string value)
        {

            MySqlConnection cnn = new MySqlConnection(StrProvider);
            MySqlCommand cmd = cnn.CreateCommand();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("?value", value);
            MySqlDataReader reader;

            try
            {
                cnn.Open();

                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {

                    while (reader.Read())
                    {

                        return reader.GetBoolean(0);

                    }
                }
                else
                {
                    Console.WriteLine("No value found.");
                }

                cnn.Close();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;

        }

        public string LoadClientID(string cardNumber)
        {
            string clientID = loadValue("SELECT clientid FROM client WHERE cardnumber=?value", cardNumber);
            return clientID;
        }

        public string LoadLicensePlate(string clientID)
        {
            string licensePlate = loadValue("SELECT licenseplate FROM object WHERE clientid=?value", clientID);
            return licensePlate;
        }

        public bool LoadParked(string clientID)
        {
            bool parked = loadBool("SELECT parked FROM object WHERE clientid=?value", clientID);
            return parked;
        }

        public string LoadObjectID(string licenseplate)
        {
            string objectID = loadValue("SELECT objectid FROM object WHERE licenseplate=?value", licenseplate);
            return objectID;
        }

        public int LoadObjectCount(string clientID)
        {
            int counter;
            int nofound = 0;
            string count = loadValue("SELECT COUNT(*) as Number FROM object WHERE clientid=?value", clientID);
            bool tryparse = Int32.TryParse(count, out counter);
            if (tryparse == true)
            {
                return counter;
            }else
            {
                Debug.WriteLine("No value found in objects");
                return nofound;
            }
           
        }

        public void UpdateParking(bool parking, string objectid)
        {
            MySqlConnection cnn = new MySqlConnection(StrProvider);
            MySqlCommand cmd = cnn.CreateCommand();
            cmd.CommandText = "UPDATE object SET parked=?parked WHERE objectid=?objectid";

            cmd.Parameters.AddWithValue("?parked", parking);
            cmd.Parameters.AddWithValue("?objectid", objectid);

            try
            {
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }

}
