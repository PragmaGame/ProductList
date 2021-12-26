using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using UnityEditor;

namespace Core
{
    public class Model : MonoBehaviour
    {
        static string _connectionPath = "URI=file:" + Application.streamingAssetsPath + "/Product.db";

        private IDbConnection _dbconn;

        private void Awake()
        {
            _dbconn = new SqliteConnection(_connectionPath);
        }

        public IEnumerable<ProductData> Loading()
        {
            var data = new List<ProductData>();
            
            _dbconn.Open();
            IDbCommand dbcmd = _dbconn.CreateCommand();
            dbcmd.CommandText = "SELECT * " + "FROM products";
            IDataReader reader = dbcmd.ExecuteReader();
            
            while (reader.Read())
            {
                var name = reader.GetString(0);
                var type = reader.GetString(1);
                var price = reader.GetInt32(2);

                data.Add(new ProductData(name,type, price));
            }
            
            reader.Close();
            dbcmd.Dispose();
            _dbconn.Close();

            return data;
        }

        public void Save(IEnumerable<ProductData> data)
        {
            _dbconn.Open();
            
            IDbCommand dbcmd = _dbconn.CreateCommand();
            dbcmd.CommandText = "DELETE " + "FROM products";
            dbcmd.ExecuteNonQuery();

            foreach (var item in data)
            {
                dbcmd.CommandText = $"INSERT INTO products VALUES (\"{item.name}\",\"{item.type}\", {item.price})";
                dbcmd.ExecuteNonQuery();
            }
            
            dbcmd.Dispose();
            _dbconn.Close();
        }
    }
}

public class ProductData
{
    public string name;
    public string type;
    public int price;

    public ProductData(string name, string type, int price)
    {
        this.name = name;
        this.type = type;
        this.price = price;
    }
}