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

        //Raw SQL Table functionality
        public List<DataPointModel> GetRawDataPoints()
        {
            string sqlQuery = "SELECT * FROM RawDataTable";

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
                            DataPointUniqueID = Convert.ToInt32(reader["DataPointUniqueID"]),
                            DataPointGroupID = Convert.ToInt32(reader["DataPointGroupID"]),
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

        public List<DataPointModel> GetRawDataPointsByDate(string startdate, string enddate)
        {
            string sqlQuery = "SELECT * FROM RawDataTable WHERE DataPointDateTime >= @StartDate AND DataPointDateTime <= @EndDate";

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
                            DataPointUniqueID = Convert.ToInt32(reader["DataPointUniqueID"]),
                            DataPointGroupID = Convert.ToInt32(reader["DataPointGroupID"]),
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

        public int AddRawDataPoint(DataPointModel ADD)
        {
            string sqlQuery = "INSERT into RawDataTable (DataPointDateTime, DataPointGroupID, DataPointX, DataPointY, DataPointZ)  OUTPUT INSERTED.DataPointID VALUES (@DataPointDateTime, @DataPointGroupID, @DataPointX, @DataPointY, @DataPointZ)";

            using (SqlConnection con = new SqlConnection(connectionstring))
            using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
            {
                con.Open();
                cmd.Parameters.Add("@DataPointDateTime", SqlDbType.DateTime).Value = ADD.DataPointDateTime;
                cmd.Parameters.Add("@DataPointGroupID", SqlDbType.Int).Value = ADD.DataPointGroupID;
                cmd.Parameters.Add("@DataPointX", SqlDbType.Float).Value = ADD.DataPointX;
                cmd.Parameters.Add("@DataPointY", SqlDbType.Float).Value = ADD.DataPointY;
                cmd.Parameters.Add("@DataPointZ", SqlDbType.Float).Value = ADD.DataPointZ;
                
                
                return (int)cmd.ExecuteScalar();
            }
        }

        public DataPointModel GetRawDataPointByID(int ID)
        {
            DataPointModel point = new DataPointModel();
            string sqlQuery = "SELECT * from RawDataTable where DataPointUniqueID=@ID";
            using (SqlConnection con = new SqlConnection(connectionstring))
            using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
            {
                con.Open();
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        point.DataPointUniqueID = Convert.ToInt32(reader["DataPointUniqueID"]);
                        point.DataPointGroupID = Convert.ToInt32(reader["DataPointGroupID"]);
                        point.DataPointDateTime = Convert.ToDateTime(reader["DataPointDateTime"]);
                        point.DataPointX = Convert.ToDouble(reader["DataPointX"]);
                        point.DataPointY = Convert.ToDouble(reader["DataPointY"]);
                        point.DataPointZ = Convert.ToDouble(reader["DataPointZ"]);
                    }
                }
            }
            return point;
        }

        public int DeleteRawDataPointByID(int ID)
        {
            string sqlQuery = "DELETE from RawDataTable Where DataPointUniqueID=@DataPointUniqueID";
            using (SqlConnection con = new SqlConnection(connectionstring))
            using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
            {
                con.Open();
                cmd.Parameters.Add("@DataPointUniqueID", SqlDbType.Int).Value = ID;
                return cmd.ExecuteNonQuery();
            }
        }

        public int EditRawDataPoint(DataPointModel edit)
        {
            string sqlQuery = "UPDATE RawDataTable SET DataPointDateTime=@DataPointDateTime, DataPointGroupID=@DataPointGroupID, DataPointX=@DataPointX, DataPointY=@DataPointY, DataPointZ=@DataPointZ  Where DataPointID=@DataPointID";

            //No need to use SqlDataReader here since we are just using the Sql Query to persist to database
            using (SqlConnection con = new SqlConnection(connectionstring))
            using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
            {
                con.Open();
                cmd.Parameters.Add("@DataPointUniqueID", SqlDbType.Int).Value = edit.DataPointUniqueID;
                cmd.Parameters.Add("@DataPointGroupID", SqlDbType.Int).Value = edit.DataPointGroupID;
                cmd.Parameters.Add("@DataPointDateTime", SqlDbType.DateTime).Value = edit.DataPointDateTime;
                cmd.Parameters.Add("@DataPointX", SqlDbType.Float).Value = edit.DataPointX;
                cmd.Parameters.Add("@DataPointY", SqlDbType.Float).Value = edit.DataPointY;
                cmd.Parameters.Add("@DataPointZ", SqlDbType.Float).Value = edit.DataPointZ;
                return cmd.ExecuteNonQuery();
            }
        }

        //FFT SQL Table functionality
        public List<DataPointModel> GetFFTDataPoints()
        {
            string sqlQuery = "SELECT * FROM FFTDataTable";

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
                            DataPointUniqueID = Convert.ToInt32(reader["DataPointUniqueID"]),
                            DataPointGroupID = Convert.ToInt32(reader["DataPointGroupID"]),
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

        public List<DataPointModel> GetFFTDataPointsByDate(string startdate, string enddate)
        {
            string sqlQuery = "SELECT * FROM FFTDataTable WHERE DataPointDateTime >= @StartDate AND DataPointDateTime <= @EndDate";

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
                            DataPointUniqueID = Convert.ToInt32(reader["DataPointUniqueID"]),
                            DataPointGroupID = Convert.ToInt32(reader["DataPointGroupID"]),
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

        public int AddFFTDataPoint(DataPointModel ADD)
        {
            string sqlQuery = "INSERT into FFTDataTable (DataPointDateTime, DataPointGroupID, DataPointX, DataPointY, DataPointZ)  OUTPUT INSERTED.DataPointID VALUES (@DataPointDateTime, @DataPointGroupID, @DataPointX, @DataPointY, @DataPointZ)";

            using (SqlConnection con = new SqlConnection(connectionstring))
            using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
            {
                con.Open();
                cmd.Parameters.Add("@DataPointDateTime", SqlDbType.DateTime).Value = ADD.DataPointDateTime;
                cmd.Parameters.Add("@DataPointGroupID", SqlDbType.Int).Value = ADD.DataPointGroupID;
                cmd.Parameters.Add("@DataPointX", SqlDbType.Float).Value = ADD.DataPointX;
                cmd.Parameters.Add("@DataPointY", SqlDbType.Float).Value = ADD.DataPointY;
                cmd.Parameters.Add("@DataPointZ", SqlDbType.Float).Value = ADD.DataPointZ;


                return (int)cmd.ExecuteScalar();
            }
        }

        public DataPointModel GetFFTDataPointByID(int ID)
        {
            DataPointModel point = new DataPointModel();
            string sqlQuery = "SELECT * from FFTDataTable where DataPointUniqueID=@ID";
            using (SqlConnection con = new SqlConnection(connectionstring))
            using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
            {
                con.Open();
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        point.DataPointUniqueID = Convert.ToInt32(reader["DataPointUniqueID"]);
                        point.DataPointGroupID = Convert.ToInt32(reader["DataPointGroupID"]);
                        point.DataPointDateTime = Convert.ToDateTime(reader["DataPointDateTime"]);
                        point.DataPointX = Convert.ToDouble(reader["DataPointX"]);
                        point.DataPointY = Convert.ToDouble(reader["DataPointY"]);
                        point.DataPointZ = Convert.ToDouble(reader["DataPointZ"]);
                    }
                }
            }
            return point;
        }

        public int DeleteFFTDataPointByID(int ID)
        {
            string sqlQuery = "DELETE from FFTDataTable Where DataPointUniqueID=@DataPointUniqueID";
            using (SqlConnection con = new SqlConnection(connectionstring))
            using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
            {
                con.Open();
                cmd.Parameters.Add("@DataPointUniqueID", SqlDbType.Int).Value = ID;
                return cmd.ExecuteNonQuery();
            }
        }

        public int EditFFTDataPoint(DataPointModel edit)
        {
            string sqlQuery = "UPDATE FFTDataTable SET DataPointDateTime=@DataPointDateTime, DataPointGroupID=@DataPointGroupID, DataPointX=@DataPointX, DataPointY=@DataPointY, DataPointZ=@DataPointZ  Where DataPointID=@DataPointID";

            //No need to use SqlDataReader here since we are just using the Sql Query to persist to database
            using (SqlConnection con = new SqlConnection(connectionstring))
            using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
            {
                con.Open();
                cmd.Parameters.Add("@DataPointUniqueID", SqlDbType.Int).Value = edit.DataPointUniqueID;
                cmd.Parameters.Add("@DataPointGroupID", SqlDbType.Int).Value = edit.DataPointGroupID;
                cmd.Parameters.Add("@DataPointDateTime", SqlDbType.DateTime).Value = edit.DataPointDateTime;
                cmd.Parameters.Add("@DataPointX", SqlDbType.Float).Value = edit.DataPointX;
                cmd.Parameters.Add("@DataPointY", SqlDbType.Float).Value = edit.DataPointY;
                cmd.Parameters.Add("@DataPointZ", SqlDbType.Float).Value = edit.DataPointZ;
                return cmd.ExecuteNonQuery();
            }
        }
    }
}
