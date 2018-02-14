using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//SQL access functions 
namespace MyAcceleAppSQL
{
    public class SQLDataAccess
    {
        private string connectionstring;

        public SQLDataAccess(string connection)
        {
            connectionstring = connection;
        }

        public List<DataPointModel> GetDataPoints()
        {
            string sqlQuery = "SELECT * FROM DataTable";

            List<DataPointModel> datapoints = new List<DataPointModel>();

            using (SqlConnection con = new SqlConnection(connectionstring))
            using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DataPointModel temp = new DataPointModel()
                        {
                            DataPointDateTime = Convert.ToDateTime(reader["DataPointDateTime"]),
                            DataPointID = Convert.ToInt32(reader["DataPointID"]),
                            DataPointX = Convert.ToDouble(reader["DataPointX"]),
                            DataPointY = Convert.ToDouble(reader["DataPointY"]),
                            DataPointZ = Convert.ToDouble(reader["DataPointZ"]),
                        };
                        datapoints.Add(temp);
                    }
                }
            }
            return datapoints;
        }

        public List<string> GetDataPointsDates()
        {
            string sqlQuery = "SELECT DataPointDateTime FROM DataTable";

            List<string> datapoints = new List<string>();

            using (SqlConnection con = new SqlConnection(connectionstring))
            using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string temp = Convert.ToDateTime(reader["DataPointDateTime"]).ToString("MM/dd/yyyy hh:mm:ss.fff tt");
                        datapoints.Add(temp);
                    }
                }
            }
            return datapoints;
        }

        public List<DataPointModel> GetDataPointsByDate(string startdate, string enddate)
        {
            string sqlQuery = "SELECT * FROM DataTable WHERE DataPointDateTime >= @StartDate AND DataPointDateTime <= @EndDate";

            List<DataPointModel> datapoints = new List<DataPointModel>();

            using (SqlConnection con = new SqlConnection(connectionstring))
            using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
            {
                cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = Convert.ToDateTime(startdate);
                cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = Convert.ToDateTime(enddate);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DataPointModel temp = new DataPointModel()
                        {
                            DataPointDateTime = Convert.ToDateTime(reader["DataPointDateTime"]),
                            DataPointID = Convert.ToInt32(reader["DataPointID"]),
                            DataPointX = Convert.ToDouble(reader["DataPointX"]),
                            DataPointY = Convert.ToDouble(reader["DataPointY"]),
                            DataPointZ = Convert.ToDouble(reader["DataPointZ"]),
                        };
                        datapoints.Add(temp);
                    }
                }
            }
            return datapoints;
        }

        public int AddDataPoint(DataPointModel ADD)
        {
            string sqlQuery = "INSERT into DataTable (DataPointDateTime, DataPointX, DataPointY, DataPointZ)  OUTPUT INSERTED.DataPointID VALUES (@DataPointDateTime, @DataPointX, @DataPointY, @DataPointZ)";

            using (SqlConnection con = new SqlConnection(connectionstring))
            using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
            {
                con.Open();
                cmd.Parameters.Add("@DataPointDateTime", SqlDbType.DateTime).Value = ADD.DataPointDateTime;
                cmd.Parameters.Add("@DataPointX", SqlDbType.Float).Value = ADD.DataPointX;
                cmd.Parameters.Add("@DataPointY", SqlDbType.Float).Value = ADD.DataPointY;
                cmd.Parameters.Add("@DataPointZ", SqlDbType.Float).Value = ADD.DataPointZ;
                
                
                return (int)cmd.ExecuteScalar();
            }
        }
        public DataPointModel GetDataPointByID(int ID)
        {
            DataPointModel point = new DataPointModel();
            string sqlQuery = "SELECT * from DataTable where DataPointID=@ID";
            using (SqlConnection con = new SqlConnection(connectionstring))
            using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
            {
                con.Open();
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        point.DataPointID = Convert.ToInt32(reader["DataPointID"]);
                        point.DataPointDateTime = Convert.ToDateTime(reader["DataPointDateTime"]);
                        point.DataPointX = Convert.ToDouble(reader["DataPointX"]);
                        point.DataPointY = Convert.ToDouble(reader["DataPointY"]);
                        point.DataPointZ = Convert.ToDouble(reader["DataPointZ"]);
                    }
                }
            }
            return point;
        }

        public int DeleteDataPointByID(int ID)
        {
            string sqlQuery = "DELETE from DataTable Where DataPointID=@DataPointID";
            using (SqlConnection con = new SqlConnection(connectionstring))
            using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
            {
                con.Open();
                cmd.Parameters.Add("@DataPointID", SqlDbType.Int).Value = ID;
                return cmd.ExecuteNonQuery();
            }
        }

        public int EditDataPoint(DataPointModel edit)
        {
            string sqlQuery = "UPDATE DataTable SET DataPointDateTime=@DataPointDateTime, DataPointX=@DataPointX, DataPointY=@DataPointY, DataPointZ=@DataPointZ  Where DataPointID=@DataPointID";

            //No need to use SqlDataReader here since we are just using the Sql Query to persist to database
            using (SqlConnection con = new SqlConnection(connectionstring))
            using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
            {
                con.Open();
                cmd.Parameters.Add("@DataPointID", SqlDbType.Int).Value = edit.DataPointID;
                cmd.Parameters.Add("@DataPointDateTime", SqlDbType.DateTime).Value = edit.DataPointDateTime;
                cmd.Parameters.Add("@DataPointX", SqlDbType.Float).Value = edit.DataPointX;
                cmd.Parameters.Add("@DataPointY", SqlDbType.Float).Value = edit.DataPointY;
                cmd.Parameters.Add("@DataPointZ", SqlDbType.Float).Value = edit.DataPointZ;
                return cmd.ExecuteNonQuery();
            }
        }
    }
}
