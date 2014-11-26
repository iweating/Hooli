﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MySql.Data.MySqlClient;

namespace Hooli.MySql
{
    public class DBConnect
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;
        private string port;

        public DBConnect()
        {
            Initialize();
        }

        private void Initialize()
        {
            server = "sql4.freemysqlhosting.net";
            port = "3306";
            database = "sql458381";
            uid = "sql458381";
            password = "tX5*nQ8*";
            string connectionString = "Server=" + server + ";" + "Port=" + port + ";" + "Database=" + database + ";" + "Uid=" + uid + ";" + "Password=" + password + ";";
            connection = new MySqlConnection(connectionString);
        }

        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch(MySqlException ex)
            {
                return false;
            }
        }

        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch(MySqlException ex)
            {
                return false;
            }
        }

        public void Insert(string query)
        {
            //open connection

            if(this.OpenConnection()==true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);
                
                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        public void Update(string query)
        {
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand();

                cmd.CommandText = query;
                cmd.Connection = connection;

                cmd.ExecuteNonQuery();

                this.CloseConnection();
            }
        }

        public void Delete(string query)
        {
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        public DataTable GetData(string query)
        {
            DataTable dt = new DataTable();
            try
            {
                this.OpenConnection();
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);
                dt.Load(cmd.ExecuteReader());

                return dt;
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("NO DB CXN FOUND");
                return null;
            }
            finally
            {
                //close connection
                this.CloseConnection();
            }
        }

        public MySqlConnection GetConnection()
        {
            return this.connection;
        }

        /*public List<string>[] Select(string query)
        {
            List<string>[] list=new List<string>[99];
            for(int i=0;i<list.Length;i++)
            {
                list[i] = new List<string>();
            }

        }

        public int Count()
        {

        }*/

        public void Backup()
        {

        }

        public void Restore()
        {

        }
    }
}