﻿using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Summary description for PGDBClientManager
/// </summary>
namespace Practic_3_curs.Models
{
    public class PGDBClientManager : IClientManager
    {
        string Connect_Setting;  //!< Данные для подключения к БД
        public PGDBClientManager(string connect_setting)
        {
            Connect_Setting = connect_setting;
        }

        /// <summary>
        /// Добавление нового клиента
        /// </summary>
        /// <param name="client">Данные нового клиента</param>
        public void Add(Stored_Client client)
        {
            if (client.Name != "" && client.Phone != "")
            {
                NpgsqlConnection DB = new NpgsqlConnection(Connect_Setting);
                DB.Open();
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = DB;
                cmd.CommandText = "INSERT INTO \"Client\" (\"Name\", \"Phone\") "
                                + "VALUES ('" + client.Name + "', '" + client.Phone + "')";
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Получение списка клиентов
        /// </summary>
        /// <returns>Список клиентов</returns>
        public List<Stored_Client> GetAllClients()
        {
            List<Stored_Client> clients = new List<Stored_Client>();
            NpgsqlConnection DB = new NpgsqlConnection(Connect_Setting);
            DB.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = DB;
            cmd.CommandText = "SELECT \"ID\", \"Name\", \"Phone\" FROM \"Client\" ORDER BY \"ID\"";
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Stored_Client addClient = new Stored_Client();
                addClient.ID = reader.GetInt32(0);
                addClient.Name = reader.GetString(1).TrimEnd();
                addClient.Phone = reader.GetString(2).TrimEnd();
                clients.Add(addClient);
            }
            DB.Close();
            return clients;
        }

        /// <summary>
        /// Возвращает ID клиента по его наименованию
        /// </summary>
        /// <param name="Name">Наименование клиента</param>
        /// <returns>ID клиента</returns>
        public int GetClientByName(string Name)
        {
            NpgsqlConnection DB = new NpgsqlConnection(Connect_Setting);
            DB.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = DB;
            cmd.CommandText = "SELECT \"ID\" FROM \"Client\" WHERE \"Name\" = '" + Name + "'";
            NpgsqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                DB.Close();
                return reader.GetInt32(0);
            }
            DB.Close();
            throw new Exception("Клиента с таким наименованием не существует");
        }

        /// <summary>
        /// Удаление информации о клиентах
        /// </summary>
        /// <param name="id">Код удаляемого клиента</param>
        public void Remove(int id)
        {
            NpgsqlConnection DB = new NpgsqlConnection(Connect_Setting);
            DB.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = DB;
            cmd.CommandText = "DELETE FROM \"Client\" WHERE \"ID\" = '" + id + "'";
            cmd.ExecuteNonQuery();
            DB.Close();
        }

        /// <summary>
        /// Обновление данных о клиентах
        /// </summary>
        /// <param name="client">Новые данные</param>
        public void Update(Stored_Client client)
        {
            if (client.Name != "" || client.Phone != "")
            {
                NpgsqlConnection DB = new NpgsqlConnection(Connect_Setting);
                DB.Open();
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = DB;
                cmd.CommandText = "UPDATE \"Client\" SET";
                if (client.Name != "")
                {
                    cmd.CommandText += " \"Name\" = '" + client.Name + "'";
                    if (client.Phone != "")
                        cmd.CommandText += ",";
                }
                if (client.Phone != "")
                    cmd.CommandText += " \"Phone\" = '" + client.Phone + "'"; 
                cmd.CommandText += " WHERE \"ID\" = '" + client.ID + "'";
                cmd.ExecuteNonQuery();
                DB.Close();
            }
        }
    }
}
