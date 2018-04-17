using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataPointViewModelServerWebsite;

namespace SQLDataBase
{
    public class SQLDataBaseAccess
    {
        private string connectionstring;

        public SQLDataBaseAccess(string connection)
        {
            connectionstring = connection;
        }

        //Raw SQL Table functionality
        public List<DataPointTimeModel> GetTimeDataPoints()
        {
            string sqlQuery = "SELECT * FROM TimeDataTable";

            List<DataPointTimeModel> datapoints = new List<DataPointTimeModel>();

            using (SqlConnection con = new SqlConnection(connectionstring))
            using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DataPointTimeModel temp = new DataPointTimeModel()
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

        public List<DataPointTimeModel> GetTimeDataPointsByGroupID(int ID)
        {
            List<DataPointTimeModel> datapoints = new List<DataPointTimeModel>();
            string sqlQuery = "SELECT * FROM TimeDataTable where DataPointGroupID=@ID";
            using (SqlConnection con = new SqlConnection(connectionstring))
            using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
            {
                con.Open();
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DataPointTimeModel temp = new DataPointTimeModel()
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

        public List<DataPointTimeModel> GetTimeDataPointsByGroupIDAndDate(int ID, string startdate, string enddate)
        {
            string sqlQuery = "SELECT * FROM TimeDataTable WHERE DataPointGroupID=@ID AND DataPointDateTime >= @StartDate AND DataPointDateTime <= @EndDate";

            List<DataPointTimeModel> datapoints = new List<DataPointTimeModel>();

            using (SqlConnection con = new SqlConnection(connectionstring))
            using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
            {
                cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = Convert.ToDateTime(startdate);
                cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = Convert.ToDateTime(enddate);
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID;
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DataPointTimeModel temp = new DataPointTimeModel()
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

        public List<DataPointTimeModel> GetTimeDataPointsByDate(string startdate, string enddate)
        {
            string sqlQuery = "SELECT * FROM TimeDataTable WHERE DataPointDateTime >= @StartDate AND DataPointDateTime <= @EndDate";

            List<DataPointTimeModel> datapoints = new List<DataPointTimeModel>();

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
                        DataPointTimeModel temp = new DataPointTimeModel()
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

        public int AddTimeDataPoint(DataPointTimeModel ADD)
        {
            string sqlQuery = "INSERT into TimeDataTable (DataPointDateTime, DataPointGroupID, DataPointX, DataPointY, DataPointZ)  OUTPUT INSERTED.DataPointUniqueID VALUES (@DataPointDateTime, @DataPointGroupID, @DataPointX, @DataPointY, @DataPointZ)";

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

        public int DeleteTimeDataPointByID(int ID)
        {
            string sqlQuery = "DELETE from TimeDataTable Where DataPointUniqueID=@DataPointUniqueID";
            using (SqlConnection con = new SqlConnection(connectionstring))
            using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
            {
                con.Open();
                cmd.Parameters.Add("@DataPointUniqueID", SqlDbType.Int).Value = ID;
                return cmd.ExecuteNonQuery();
            }
        }

        public int EditTimeDataPoint(DataPointTimeModel edit)
        {
            string sqlQuery = "UPDATE TimeDataTable SET DataPointDateTime=@DataPointDateTime, DataPointGroupID=@DataPointGroupID, DataPointX=@DataPointX, DataPointY=@DataPointY, DataPointZ=@DataPointZ  Where DataPointUniqueID=@DataPointUniqueID";

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
        public List<DataPointFFTModel> GetFFTDataPoints()
        {
            string sqlQuery = "SELECT * FROM FFTDataTable";

            List<DataPointFFTModel> datapoints = new List<DataPointFFTModel>();

            using (SqlConnection con = new SqlConnection(connectionstring))
            using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DataPointFFTModel temp = new DataPointFFTModel()
                        {
                            DataPointDateTime = Convert.ToDateTime(reader["DataPointDateTime"]),
                            DataPointUniqueID = Convert.ToInt32(reader["DataPointUniqueID"]),
                            DataPointGroupID = Convert.ToInt32(reader["DataPointGroupID"]),
                            DataPointX = Convert.ToDouble(reader["DataPointX"]),
                            DataPointY = Convert.ToDouble(reader["DataPointY"]),
                            DataPointZ = Convert.ToDouble(reader["DataPointZ"]),
                            DataPointFreq = Convert.ToDouble(reader["DataPointFreq"]),
                        };
                        datapoints.Add(temp);
                    }
                }
            }
            return datapoints;
        }

        public List<DataPointFFTModel> GetFFTDataPointByGroupID(int ID)
        {
            List<DataPointFFTModel> datapoints = new List<DataPointFFTModel>();
            string sqlQuery = "SELECT * FROM FFTDataTable WHERE DataPointGroupID=@ID";
            using (SqlConnection con = new SqlConnection(connectionstring))
            using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
            {
                con.Open();
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DataPointFFTModel temp = new DataPointFFTModel()
                        {
                            DataPointDateTime = Convert.ToDateTime(reader["DataPointDateTime"]),
                            DataPointUniqueID = Convert.ToInt32(reader["DataPointUniqueID"]),
                            DataPointGroupID = Convert.ToInt32(reader["DataPointGroupID"]),
                            DataPointX = Convert.ToDouble(reader["DataPointX"]),
                            DataPointY = Convert.ToDouble(reader["DataPointY"]),
                            DataPointZ = Convert.ToDouble(reader["DataPointZ"]),
                            DataPointFreq = Convert.ToDouble(reader["DataPointFreq"]),
                        };
                        datapoints.Add(temp);
                    }
                }
            }
            return datapoints;
        }

        public List<DataPointFFTModel> GetFFTDataPointsByGroupIDAndDate(int ID, string startdate, string enddate)
        {
            string sqlQuery = "SELECT * FROM FFTDataTable WHERE DataPointGroupID=@ID AND DataPointDateTime >= @StartDate AND DataPointDateTime <= @EndDate";

            List<DataPointFFTModel> datapoints = new List<DataPointFFTModel>();

            using (SqlConnection con = new SqlConnection(connectionstring))
            using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
            {
                cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = Convert.ToDateTime(startdate);
                cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = Convert.ToDateTime(enddate);
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID;
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DataPointFFTModel temp = new DataPointFFTModel()
                        {
                            DataPointDateTime = Convert.ToDateTime(reader["DataPointDateTime"]),
                            DataPointGroupID = Convert.ToInt32(reader["DataPointGroupID"]),
                            DataPointUniqueID = Convert.ToInt32(reader["DataPointUniqueID"]),
                            DataPointX = Convert.ToDouble(reader["DataPointX"]),
                            DataPointY = Convert.ToDouble(reader["DataPointY"]),
                            DataPointZ = Convert.ToDouble(reader["DataPointZ"]),
                            DataPointFreq = Convert.ToDouble(reader["DataPointFreq"]),
                        };
                        datapoints.Add(temp);
                    }
                }
            }
            return datapoints;
        }

        public List<DataPointFFTModel> GetFFTDataPointsByDate(string startdate, string enddate)
        {
            string sqlQuery = "SELECT * FROM FFTDataTable WHERE DataPointDateTime >= @StartDate AND DataPointDateTime <= @EndDate";

            List<DataPointFFTModel> datapoints = new List<DataPointFFTModel>();

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
                        DataPointFFTModel temp = new DataPointFFTModel()
                        {
                            DataPointDateTime = Convert.ToDateTime(reader["DataPointDateTime"]),
                            DataPointGroupID = Convert.ToInt32(reader["DataPointGroupID"]),
                            DataPointUniqueID = Convert.ToInt32(reader["DataPointUniqueID"]),
                            DataPointX = Convert.ToDouble(reader["DataPointX"]),
                            DataPointY = Convert.ToDouble(reader["DataPointY"]),
                            DataPointZ = Convert.ToDouble(reader["DataPointZ"]),
                            DataPointFreq = Convert.ToDouble(reader["DataPointFreq"]),
                        };
                        datapoints.Add(temp);
                    }
                }
            }
            return datapoints;
        }

        public int AddFFTDataPoint(DataPointFFTModel ADD)
        {
            string sqlQuery = "INSERT into FFTDataTable (DataPointDateTime, DataPointGroupID, DataPointX, DataPointY, DataPointZ, DataPointFreq)  OUTPUT INSERTED.DataPointUniqueID VALUES (@DataPointDateTime, @DataPointGroupID, @DataPointX, @DataPointY, @DataPointZ, @DataPointFreq)";

            using (SqlConnection con = new SqlConnection(connectionstring))
            using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
            {
                con.Open();
                cmd.Parameters.Add("@DataPointDateTime", SqlDbType.DateTime).Value = ADD.DataPointDateTime;
                cmd.Parameters.Add("@DataPointGroupID", SqlDbType.Int).Value = ADD.DataPointGroupID;
                cmd.Parameters.Add("@DataPointX", SqlDbType.Float).Value = ADD.DataPointX;
                cmd.Parameters.Add("@DataPointY", SqlDbType.Float).Value = ADD.DataPointY;
                cmd.Parameters.Add("@DataPointZ", SqlDbType.Float).Value = ADD.DataPointZ;
                cmd.Parameters.Add("@DataPointFreq", SqlDbType.Float).Value = ADD.DataPointFreq;

                return (int)cmd.ExecuteScalar();
            }
        }

        public int DeleteFFTDataPointByID(int ID)
        {
            string sqlQuery = "DELETE from FFTDataTable Where DataPointGroupID=@DataPointUniqueID";
            using (SqlConnection con = new SqlConnection(connectionstring))
            using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
            {
                con.Open();
                cmd.Parameters.Add("@DataPointUniqueID", SqlDbType.Int).Value = ID;
                return cmd.ExecuteNonQuery();
            }
        }

        public int EditFFTDataPoint(DataPointFFTModel edit)
        {
            string sqlQuery = "UPDATE FFTDataTable SET DataPointDateTime=@DataPointDateTime, DataPointGroupID=@DataPointGroupID, DataPointX=@DataPointX, DataPointY=@DataPointY, DataPointZ=@DataPointZ, DataPointFreq=@DataPointFreq   Where DataPointUniqueID=@DataPointUniqueID";

            //No need to use SqlDataReader here since we are just using the Sql Query to persist to database
            using (SqlConnection con = new SqlConnection(connectionstring))
            using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
            {
                con.Open();
                cmd.Parameters.Add("@DataPointGroupID", SqlDbType.Int).Value = edit.DataPointGroupID;
                cmd.Parameters.Add("@DataPointDateTime", SqlDbType.DateTime).Value = edit.DataPointDateTime;
                cmd.Parameters.Add("@DataPointX", SqlDbType.Float).Value = edit.DataPointX;
                cmd.Parameters.Add("@DataPointY", SqlDbType.Float).Value = edit.DataPointY;
                cmd.Parameters.Add("@DataPointZ", SqlDbType.Float).Value = edit.DataPointZ;
                cmd.Parameters.Add("@DataPointFreq", SqlDbType.Float).Value = edit.DataPointFreq;
                return cmd.ExecuteNonQuery();
            }
        }

        //RMS SQL Table functionality
        public List<DataPointRMSModel> GetRMSDataPoints()
        {
            string sqlQuery = "SELECT * FROM RMSDataTable";

            List<DataPointRMSModel> datapoints = new List<DataPointRMSModel>();

            using (SqlConnection con = new SqlConnection(connectionstring))
            using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DataPointRMSModel temp = new DataPointRMSModel()
                        {
                            DataPointDateTime = Convert.ToDateTime(reader["DataPointDateTime"]),
                            DataPointUniqueID = Convert.ToInt32(reader["DataPointUniqueID"]),
                            DataPointGroupID = Convert.ToInt32(reader["DataPointGroupID"]),
                            DataPointXRMS = Convert.ToDouble(reader["DataPointXRMS"]),
                            DataPointYRMS = Convert.ToDouble(reader["DataPointYRMS"]),
                            DataPointZRMS = Convert.ToDouble(reader["DataPointZRMS"]),
                        };
                        datapoints.Add(temp);
                    }
                }
            }
            return datapoints;
        }

        public List<DataPointRMSModel> GetRMSDataPointsByDate(string startdate, string enddate)
        {
            string sqlQuery = "SELECT * FROM RMSDataTable WHERE DataPointDateTime >= @StartDate AND DataPointDateTime <= @EndDate";

            List<DataPointRMSModel> datapoints = new List<DataPointRMSModel>();

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
                        DataPointRMSModel temp = new DataPointRMSModel()
                        {
                            DataPointDateTime = Convert.ToDateTime(reader["DataPointDateTime"]),
                            DataPointUniqueID = Convert.ToInt32(reader["DataPointUniqueID"]),
                            DataPointGroupID = Convert.ToInt32(reader["DataPointGroupID"]),
                            DataPointXRMS = Convert.ToDouble(reader["DataPointXRMS"]),
                            DataPointYRMS = Convert.ToDouble(reader["DataPointYRMS"]),
                            DataPointZRMS = Convert.ToDouble(reader["DataPointZRMS"]),
                        };
                        datapoints.Add(temp);
                    }
                }
            }
            return datapoints;
        }

        public List<DataPointRMSModel> GetRMSDataPointsByGroupIDAndDate(int ID, string startdate, string enddate)
        {
            string sqlQuery = "SELECT * FROM RMSDataTable WHERE DataPointGroupID=@ID AND DataPointDateTime >= @StartDate AND DataPointDateTime <= @EndDate";

            List<DataPointRMSModel> datapoints = new List<DataPointRMSModel>();

            using (SqlConnection con = new SqlConnection(connectionstring))
            using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
            {
                cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = Convert.ToDateTime(startdate);
                cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = Convert.ToDateTime(enddate);
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID;
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DataPointRMSModel temp = new DataPointRMSModel()
                        {
                            DataPointDateTime = Convert.ToDateTime(reader["DataPointDateTime"]),
                            DataPointUniqueID = Convert.ToInt32(reader["DataPointUniqueID"]),
                            DataPointGroupID = Convert.ToInt32(reader["DataPointGroupID"]),
                            DataPointXRMS = Convert.ToDouble(reader["DataPointXRMS"]),
                            DataPointYRMS = Convert.ToDouble(reader["DataPointYRMS"]),
                            DataPointZRMS = Convert.ToDouble(reader["DataPointZRMS"]),
                        };
                        datapoints.Add(temp);
                    }
                }
            }
            return datapoints;
        }

        public int AddRMSDataPoint(DataPointRMSModel ADD)
        {
            string sqlQuery = "INSERT into RMSDataTable (DataPointDateTime, DataPointGroupID, DataPointXRMS, DataPointYRMS, DataPointZRMS)  OUTPUT INSERTED.DataPointUniqueID VALUES (@DataPointDateTime, @DataPointGroupID, @DataPointXRMS, @DataPointYRMS, @DataPointZRMS)";

            using (SqlConnection con = new SqlConnection(connectionstring))
            using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
            {
                con.Open();
                cmd.Parameters.Add("@DataPointDateTime", SqlDbType.DateTime).Value = ADD.DataPointDateTime;
                cmd.Parameters.Add("@DataPointGroupID", SqlDbType.Int).Value = ADD.DataPointGroupID;
                cmd.Parameters.Add("@DataPointXRMS", SqlDbType.Float).Value = ADD.DataPointXRMS;
                cmd.Parameters.Add("@DataPointYRMS", SqlDbType.Float).Value = ADD.DataPointYRMS;
                cmd.Parameters.Add("@DataPointZRMS", SqlDbType.Float).Value = ADD.DataPointZRMS;

                return (int)cmd.ExecuteScalar();
            }
        }

        public List<DataPointRMSModel> GetRMSDataPointByGroupID(int ID)
        {
            List<DataPointRMSModel> datapoints = new List<DataPointRMSModel>();
            string sqlQuery = "SELECT * from RMSDataTable where DataPointGroupID=@ID";
            using (SqlConnection con = new SqlConnection(connectionstring))
            using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
            {
                con.Open();
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DataPointRMSModel temp = new DataPointRMSModel()
                        {
                            DataPointGroupID = Convert.ToInt32(reader["DataPointGroupID"]),
                            DataPointUniqueID = Convert.ToInt32(reader["DataPointUniqueID"]),
                            DataPointDateTime = Convert.ToDateTime(reader["DataPointDateTime"]),
                            DataPointXRMS = Convert.ToDouble(reader["DataPointXRMS"]),
                            DataPointYRMS = Convert.ToDouble(reader["DataPointYRMS"]),
                            DataPointZRMS = Convert.ToDouble(reader["DataPointZRMS"]),
                        };
                        datapoints.Add(temp);
                    }
                }
            }
            return datapoints;
        }

        public int DeleteRMSDataPointByID(int ID)
        {
            string sqlQuery = "DELETE from RMSDataTable Where DataPointUniqueID=@DataPointUniqueID";
            using (SqlConnection con = new SqlConnection(connectionstring))
            using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
            {
                con.Open();
                cmd.Parameters.Add("@DataPointUniqueID", SqlDbType.Int).Value = ID;
                return cmd.ExecuteNonQuery();
            }
        }

        public int EditRMSDataPoint(DataPointRMSModel edit)
        {
            string sqlQuery = "UPDATE RMSDataTable SET DataPointDateTime=@DataPointDateTime, DataPointGroupID=@DataPointGroupID, DataPointXRMS=@DataPointXRMS, DataPointYRMS=@DataPointYRMS, DataPointZRMS=@DataPointZRMS  Where DataPointUniqueID=@DataPointUniqueID";

            //No need to use SqlDataReader here since we are just using the Sql Query to persist to database
            using (SqlConnection con = new SqlConnection(connectionstring))
            using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
            {
                con.Open();
                cmd.Parameters.Add("@DataPointGroupID", SqlDbType.Int).Value = edit.DataPointGroupID;
                cmd.Parameters.Add("@DataPointDateTime", SqlDbType.DateTime).Value = edit.DataPointDateTime;
                cmd.Parameters.Add("@DataPointXRMS", SqlDbType.Float).Value = edit.DataPointXRMS;
                cmd.Parameters.Add("@DataPointYRMS", SqlDbType.Float).Value = edit.DataPointYRMS;
                cmd.Parameters.Add("@DataPointZRMS", SqlDbType.Float).Value = edit.DataPointZRMS;
                return cmd.ExecuteNonQuery();
            }
        }


    }
}
