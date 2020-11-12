using System;
using Npgsql;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Net;

namespace SchoolsServer
{
    public class Schools
    {
        public const string connString = "Host=127.0.0.1;Username=postgres;Password=root;Database=SCHOOLS_DB";
        readonly NpgsqlConnection conn = new NpgsqlConnection(connString);
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }

        public int SchoolID { get; set; }

        //возвращает список школ
        public string GetSchools()
        {
            string Answer = string.Empty;
            NpgsqlConnection conn = new NpgsqlConnection(connString);
            conn.Open();

            NpgsqlCommand command =
                new NpgsqlCommand("SELECT * FROM public.\"SHOOLS\" ", conn);
            NpgsqlDataReader reader = command.ExecuteReader();
            List<string> ListRecords = new List<string>();
            string Record = string.Empty;

            while (reader.Read())
            {

                int fieldsCount = reader.FieldCount;
                Record = "{";
                for (int j = 0; j < fieldsCount; j++)
                {
                    if (j == 0)
                        Record += "\"ID\": ";
                    if (j == 1)
                        Record += "\"NAME\": ";
                    if (j == 2)
                        Record += "\"ADDRESS\": ";

                    Record += "\"" + reader.GetValue(j).ToString().Trim() + "\",";
                }

                Record += "}";
                ListRecords.Add(Record);

            }
            conn.Close();
            Answer = "[" + String.Join(",", ListRecords.ToArray()) + "]";
            return Answer;
        }

        //возвращает школу по ID
        public string GetSchoolByID(string SchoolID)
        {
            string Answer = string.Empty;
            NpgsqlConnection conn = new NpgsqlConnection(connString);
            conn.Open();

            NpgsqlCommand command =
                new NpgsqlCommand("SELECT * FROM public.\"SHOOLS\" WHERE \"ID\" = " + SchoolID, conn);
            NpgsqlDataReader reader = command.ExecuteReader();
            List<string> ListRecords = new List<string>();
            string Record = string.Empty;

            while (reader.Read())
            {

                int fieldsCount = reader.FieldCount;
                Record = "{";
                for (int j = 0; j < fieldsCount; j++)
                {
                    if (j == 0)
                        Record += "\"ID\": ";
                    if (j == 1)
                        Record += "\"NAME\": ";
                    if (j == 2)
                        Record += "\"ADDRESS\": ";

                    Record += "\"" + reader.GetValue(j).ToString().Trim() + "\",";
                }

                Record += "}";
                ListRecords.Add(Record);

            }
            conn.Close();
            Answer = "[" + String.Join(",", ListRecords.ToArray()) + "]";
            return Answer;
        }

        private class ErrorJson
        {
            public int Code { get; set; }
            public string MessageCode { get; set; }
        }

        // Отправка страницы с ошибкой
        public string SendError(int Code)
        {
            // Получаем строку вида "200 OK"
            // HttpStatusCode хранит в себе все статус-коды HTTP/1.1
            //string CodeStr = Code.ToString() + " " + ((HttpStatusCode)Code).ToString();

            ErrorJson account = new ErrorJson
            {
                Code = Code,
                MessageCode = ((HttpStatusCode)Code).ToString()
            };

            return JsonConvert.SerializeObject(account, Formatting.Indented);
        }

        public class School
        {
            public string Name { get; set; }
            public string Address { get; set; }
        }

        //добавляет школу в БД
        public string AddNewSchool(School NewSchool)
        {
            string Answer = "OK";
            NpgsqlConnection conn = new NpgsqlConnection(connString);
            conn.Open();
            NpgsqlCommand command = conn.CreateCommand();
            try
            {
                try
                {
                    command.CommandText = "INSERT INTO public.\"SHOOLS\"(\"NAME\", \"ADDRESS\") VALUES " + "(" +
                        "'" + NewSchool.Name + "'" + ", " + "'" + NewSchool.Address + "'" + ")";
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Answer = e.Message;
                }
            }
            finally
            {
                conn.Close();
            }
            return Answer;
        }

    }
}
