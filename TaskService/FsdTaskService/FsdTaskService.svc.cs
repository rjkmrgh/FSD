using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace FsdTaskService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class FsdTaskService : IFsdTaskService
    {
        private string m_connectionString = ConfigurationManager.ConnectionStrings["FSD"].ConnectionString;
        private const string c_EndTask = "dbo.EndTask";
        private const string c_UpdateTask = "dbo.UpdateTaskDetails";
        private const string c_GetTask = "dbo.GetTaskDetails";
        private const string c_AddTask = "dbo.AddTaskDetails";


        public void AddTask(TaskDetails task)
        {
            using (SqlConnection sqlconn = new SqlConnection(m_connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(c_AddTask, sqlconn))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@taskname", task.TaskName);
                    if (task.ParentTaskNames.Count > 0)
                        sqlCommand.Parameters.AddWithValue("@parenttaskname", task.ParentTaskNames.FirstOrDefault());
                    else
                        sqlCommand.Parameters.AddWithValue("@parenttaskname", DBNull.Value);
                    sqlCommand.Parameters.AddWithValue("@startdate", task.StartDate);
                    sqlCommand.Parameters.AddWithValue("@Enddate", task.EndDate);
                    sqlCommand.Parameters.AddWithValue("@priority", task.Priority);
                    sqlconn.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        public void UpdateTask(TaskDetails task)
        {
            using (SqlConnection sqlconn = new SqlConnection(m_connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(c_UpdateTask, sqlconn))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@taskid", task.TaskID);
                    sqlCommand.Parameters.AddWithValue("@taskname", task.TaskName);
                    if (task.ParentTaskNames.Count > 0)
                        sqlCommand.Parameters.AddWithValue("@parenttaskname", task.ParentTaskNames.FirstOrDefault());
                    else
                        sqlCommand.Parameters.AddWithValue("@parenttaskname", DBNull.Value);
                    sqlCommand.Parameters.AddWithValue("@startdate", task.StartDate);
                    sqlCommand.Parameters.AddWithValue("@Enddate", task.EndDate);
                    sqlCommand.Parameters.AddWithValue("@priority", task.Priority);
                    sqlconn.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        public List<TaskDetails> ViewTask()
        {
            List<TaskDetails> objTaskDetailslist = new List<TaskDetails>();
            using (SqlConnection sqlconn = new SqlConnection(m_connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(c_GetTask, sqlconn))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlconn.Open();
                    using (SqlDataReader dr = sqlCommand.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                TaskDetails objTask = new TaskDetails
                                {
                                    TaskID = Convert.ToInt32(dr["TaskID"]),
                                    TaskName = Convert.ToString(dr["TaskName"]),
                                    StartDate = Convert.ToDateTime(dr["StartDate"]),
                                    EndDate = Convert.ToDateTime(dr["EndDate"]),
                                    Priority = Convert.ToInt32(dr["Priority"]),
                                    IsEnded = Convert.ToBoolean(dr["IsEnded"]),
                                    ParentTaskNames = new List<string> { Convert.ToString(dr["ParentTaskName"]) }
                                };
                                objTaskDetailslist.Add(objTask);
                            }
                        }
                    }
                }
            }
            return objTaskDetailslist;
        }

        public void EndTask(int taskID)
        {
            using (SqlConnection sqlconn = new SqlConnection(m_connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(c_EndTask, sqlconn))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@taskid", taskID);
                    sqlconn.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }
    }
}
