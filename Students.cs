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
    public class Students
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

        public class Student
        {
            public string FIO { get; set; }
            public string Birthdate { get; set; }
            public string Address { get; set; }
        }

        //возвращает ученика по ID
        public string GetStudentByID(string StudentID)
        {
            string Answer = string.Empty;
            NpgsqlConnection conn = new NpgsqlConnection(connString);
            conn.Open();

            NpgsqlCommand command =
                new NpgsqlCommand("SELECT * FROM public.\"STUDENTS\" WHERE \"ID\" = " + StudentID, conn);
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
                        Record += "\"FIO\": ";
                    if (j == 2)
                        Record += "\"BIRTHDATE\": ";
                    if (j == 3)
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

        //добавляет ученика в БД
        public string AddNewStudent(Student NewStudent)
        {
            string Answer = "OK";
            NpgsqlConnection conn = new NpgsqlConnection(connString);
            conn.Open();
            NpgsqlCommand command = conn.CreateCommand();
            try
            {
                try
                {
                    command.CommandText = "INSERT INTO public.\"STUDENTS\"(\"FIO\", \"BIRTHDATE\", \"ADDRESS\") VALUES " + "(" +
                        "'" + NewStudent.FIO + "'" + ", " + "DATE '" + NewStudent.Birthdate + "'" + ", " + "'" + NewStudent.Address + "'" + ")";
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

        //возвращает cписок учеников в школе на данный момент
        public string GetStudentsBySchoolID(string SchoolID)
        {
            string Answer = string.Empty;
            NpgsqlConnection conn = new NpgsqlConnection(connString);
            conn.Open();

            NpgsqlCommand command =
                new NpgsqlCommand("select \"STUDENTS\".\"FIO\", \"STUDENTS\".\"BIRTHDATE\", " + 
                  "\"STUDENTS\".\"ADDRESS\", \"CLASSES\".\"NUMBER\", \"CLASSES\".\"LITER\" " +
                  "from \"STUDENTS\" " +
                  "left join \"CLASSES_STUDENTS\" ON \"CLASSES_STUDENTS\".\"STUDENTS_ID\" = \"STUDENTS\".\"ID\" " +
                  "left join \"SCHOOL_CLASSES\" ON \"SCHOOL_CLASSES\".\"CLASS_ID\" = \"CLASSES_STUDENTS\".\"CLASSES_ID\" " +
                  "left join \"CLASSES\" ON \"CLASSES\".\"ID\" = \"CLASSES_STUDENTS\".\"CLASSES_ID\" " +
                  "where \"SCHOOL_CLASSES\".\"SCHOOL_ID\" = " + SchoolID, conn);

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
                        Record += "\"FIO\": ";
                    if (j == 1)
                        Record += "\"BIRTHDATE\": ";
                    if (j == 2)
                        Record += "\"ADDRESS\": ";
                    if (j == 3)
                        Record += "\"NUMBER\": ";
                    if (j == 4)
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
