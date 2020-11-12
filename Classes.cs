using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Net;

namespace SchoolsServer
{
    public class Classes
    {
        public const string connString = "Host=127.0.0.1;Username=postgres;Password=root;Database=SCHOOLS_DB";
        readonly NpgsqlConnection conn = new NpgsqlConnection(connString);

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

        public class SchoolClass
        {
            public int Number { get; set; }
            public string Liter { get; set; }
        }

        //возвращает класс по ID
        public string GetClassByID(string ClassID)
        {
            string Answer = string.Empty;
            NpgsqlConnection conn = new NpgsqlConnection(connString);
            conn.Open();

            NpgsqlCommand command =
                new NpgsqlCommand("SELECT * FROM public.\"CLASSES\" WHERE \"ID\" = " + ClassID, conn);
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
                        Record += "\"NUMBER\": ";
                    if (j == 2)
                        Record += "\"LITER\": ";

                    Record += "\"" + reader.GetValue(j).ToString().Trim() + "\",";
                }

                Record += "}";
                ListRecords.Add(Record);

            }
            conn.Close();
            Answer = "[" + String.Join(",", ListRecords.ToArray()) + "]";
            return Answer;
        }

        //добавляет класс в БД
        public string AddNewClass(SchoolClass NewClass)
        {
            string Answer = "OK";
            NpgsqlConnection conn = new NpgsqlConnection(connString);
            conn.Open();
            NpgsqlCommand command = conn.CreateCommand();
            try
            {
                try
                {
                    command.CommandText = "INSERT INTO public.\"CLASSES\"(\"NUMBER\", \"LITER\") VALUES " + "(" +
                        "'" + NewClass.Number + "'" + ", " + "'" + NewClass.Liter + "'" + ")";
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

        //возвращает cписок классов в школе
        public string GetClassessBySchoolID(string SchoolID)
        {
            string Answer = string.Empty;
            NpgsqlConnection conn = new NpgsqlConnection(connString);
            conn.Open();

            NpgsqlCommand command =
                new NpgsqlCommand("SELECT \"CLASSES\".* " +
                "FROM public.\"CLASSES\" " +
                "Left join \"SCHOOL_CLASSES\" on \"SCHOOL_CLASSES\".\"CLASS_ID\" = \"CLASSES\".\"ID\"  " +
                "Where \"SCHOOL_CLASSES\".\"SCHOOL_ID\" = " + SchoolID, conn);
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
                        Record += "\"NUMBER\": ";
                    if (j == 2)
                        Record += "\"LITER\": ";

                    Record += "\"" + reader.GetValue(j).ToString().Trim() + "\",";
                }

                Record += "}";
                ListRecords.Add(Record);

            }
            conn.Close();
            Answer = "[" + String.Join(",", ListRecords.ToArray()) + "]";
            return Answer;
        }

    }
}
