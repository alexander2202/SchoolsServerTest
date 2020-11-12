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
    public class Migration
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

        public class StudentMigration
        {
            public int SchoolID { get; set; }
            public int ClassID { get; set; }
            public int StudentID { get; set; }
            public string Date { get; set; }
        }

        //зачислить ученика 
        public string AdmissionStudent(StudentMigration NewMigration)
        {
            string Answer = "OK";
            NpgsqlConnection conn = new NpgsqlConnection(connString);
            conn.Open();
            NpgsqlCommand command = conn.CreateCommand();
            try
            {
                try
                {
                    //добавить запись о зачислении
                    command.CommandText = "INSERT INTO public.\"HISTORY\"(\"SCHOOL_ID\", \"CLASS_ID\", \"STUDENT_ID\", " +
                        "\"ADMISSION_DATE\", \"DISMISSIAL_DATE\") VALUES " +
                        "(" + NewMigration.SchoolID.ToString() + ", " + NewMigration.ClassID.ToString() + ", " +
                        NewMigration.StudentID.ToString() + ", " + "'" + NewMigration.Date + "'" + ", " +
                        "null" + ")";
                    command.ExecuteNonQuery();

                    //добавить связь в таблицу связей
                    command.CommandText = "INSERT INTO public.\"CLASSES_STUDENTS\"(" +
                        "\"CLASSES_ID\", \"STUDENTS_ID\") " +
                        "VALUES(" + NewMigration.ClassID.ToString() + ", " +
                        NewMigration.StudentID.ToString() + ")";
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

        //отчислить ученика 
        public string DismissialStudent(StudentMigration NewMigration)
        {
            string Answer = "OK";
            DateTime ADMISSION_DATE = DateTime.MinValue;
            NpgsqlConnection conn = new NpgsqlConnection(connString);
            conn.Open();
            NpgsqlCommand command = conn.CreateCommand();
            try
            {
                try
                {
                    //узнаем дату зачисления, чтобы новая была больше её или равна
                    command.CommandText = "SELECT \"HISTORY\".\"ADMISSION_DATE\" " +
                        "FROM public.\"HISTORY\" " +
                        "WHERE \"SCHOOL_ID\" = " + NewMigration.SchoolID.ToString() + " " +
                        "AND \"CLASS_ID\" = " + NewMigration.ClassID.ToString() + " " +
                        "AND \"STUDENT_ID\" = " + NewMigration.StudentID.ToString() + ";";
                    NpgsqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ADMISSION_DATE = DateTime.Parse(reader.GetValue(0).ToString());
                    }

                    if (ADMISSION_DATE > DateTime.Parse(NewMigration.Date))
                    {
                        throw new System.Exception("Дата отчисления не должна быть меньше даты зачисления!");
                    }

                    //обновить дату отчисления
                    command.CommandText = "UPDATE public.\"HISTORY\" " +
                        "SET \"DISMISSIAL_DATE\" = " + NewMigration.Date + " " +
                        "WHERE \"SCHOOL_ID\" = " + NewMigration.SchoolID.ToString() + " " +
                        "AND \"CLASS_ID\" = " + NewMigration.ClassID.ToString() + " " +
                        "AND \"STUDENT_ID\" = " + NewMigration.StudentID.ToString() + ")";
                    command.ExecuteNonQuery();

                    //удалить связь из таблицы связей
                    command.CommandText = "DELETE FROM public.\"CLASSES_STUDENTS\" " +
                        "WHERE \"CLASSES_ID\" = " + NewMigration.ClassID.ToString() + " " +
                        "AND \"STUDENTS_ID\" = " + NewMigration.StudentID.ToString();
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

        //Информация об учениках за период
        public string GetStudentsByPeriod(StudentMigration NewMigration)
        {
            string Answer = string.Empty;
            NpgsqlConnection conn = new NpgsqlConnection(connString);
            conn.Open();

            NpgsqlCommand command =
                new NpgsqlCommand("SELECT \"STUDENTS\".* " +
                    "FROM public.\"HISTORY\" " +
                    "LEFT JOIN \"STUDENTS\" ON \"STUDENTS\".\"ID\" = \"HISTORY\".\"STUDENT_ID\" " +
                    "WHERE \"ADMISSION_DATE\" <= " + "'" + NewMigration.Date.ToString() + "'" + " " +
                    "AND (\"DISMISSIAL_DATE\" >= " + "'" + NewMigration.Date.ToString() + "'" + " OR \"DISMISSIAL_DATE\" IS NULL) " +
                    "AND \"HISTORY\".\"CLASS_ID\" = " + NewMigration.ClassID.ToString() + " " +
                    "AND \"HISTORY\".\"SCHOOL_ID\" = " + NewMigration.SchoolID.ToString(), conn);

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
    }
}
