using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using toDoApp_Server.Data;
using toDoApp_Server.Models;

namespace toDoApp_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoAppController : ControllerBase
    {
        [HttpGet]
        [Route("GetNotes")]
        
        public JsonResult GetNotes()
        {
            try
            {
                var connection = new DataConnection();
                var procedure = "sp_ListTasks";
                var tb = new DataTable();
                using(var conn = new SqlConnection(connection.GetString()))
                {
                    using(var sqlCmd = new SqlCommand(procedure, conn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        conn.Open();
                        var dReader = sqlCmd.ExecuteReader();
                        tb.Load(dReader);
                        dReader.Close();
                        conn.Close();
                    }

                    return new JsonResult(tb);
                }                
            }catch(Exception e)
            {
                Console.WriteLine(e);
                return new JsonResult(null);
            }            
        }

        [HttpPost]
        [Route("Create")]
        public JsonResult CreateNote([FromForm] toDoAppModel note)
        {
            try
            {
                var connection = new DataConnection();
                var procedure = "sp_CreateNote";
                var tb = new DataTable();
                using(var conn = new SqlConnection(connection.GetString()))
                {
                    using (var sqlCmd = new SqlCommand(procedure, conn))
                    {
                        sqlCmd.Parameters.AddWithValue("task", note.task);
                        sqlCmd.Parameters.AddWithValue("expDate", note.expDate);
                        sqlCmd.Parameters.AddWithValue("notes", note.notes);
                        sqlCmd.CommandType= CommandType.StoredProcedure;
                        conn.Open();
                        sqlCmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                return new JsonResult("Successfully inserted");
            }catch(Exception e)
            {
                Console.WriteLine(e);
                return new JsonResult(e);
            }
        }

        [HttpGet]
        [Route("GetOne")]
        public JsonResult GetATask(string id)
        {
            try
            {
                var connection = new DataConnection();
                var procedure = "sp_GetANote";
                var tb = new DataTable();
                using (var conn = new SqlConnection(connection.GetString()))
                {
                    using (var sqlCmd = new SqlCommand(procedure, conn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("id", id);
                        conn.Open();
                        var dReader = sqlCmd.ExecuteReader();
                        tb.Load(dReader);
                        dReader.Close();
                        conn.Close();
                    }
                }
                return new JsonResult(tb);
            }
            catch (Exception e) { return new JsonResult(e); }
        }

        [HttpPut]
        [Route("updateNote")]
        public JsonResult UpdateNote([FromForm] string[] notes)
        {
            try
            {
                var connection = new DataConnection();
                var procedure = "sp_UpdateNote";
                using (var conn = new SqlConnection(connection.GetString()))
                {
                    using(var sqlCmd = new SqlCommand(procedure, conn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("id", notes[0]);
                        sqlCmd.Parameters.AddWithValue("task", notes[1]);
                        sqlCmd.Parameters.AddWithValue("expDate", notes[2]);
                        sqlCmd.Parameters.AddWithValue("notes", notes[3]);
                        conn.Open();
                        sqlCmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                return new JsonResult("Updated");
            }catch(Exception e) { return new JsonResult(e);}
        }

        [HttpDelete]
        [Route("deleteNote")]
        public JsonResult DeleteNote(string id)
        {
            try
            {
                var connection = new DataConnection();
                var procedure = "sp_DeleteNote";
                using(var conn = new SqlConnection(connection.GetString()))
                {
                    using(var sqlCmd = new SqlCommand(procedure, conn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("id", id);
                        conn.Open();
                        sqlCmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                return new JsonResult("Deleted");
            }catch(Exception e) { return new JsonResult(e); }
        }
    }
    
}
