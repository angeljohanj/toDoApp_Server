using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.Metadata;
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
                var oNotes = new List<toDoAppModel>();
                var connection = new DataConnection();
                var procedure = "sp_ListTasks";
                using(var conn = new SqlConnection(connection.GetString()))
                {
                    using(var sqlCmd = new SqlCommand(procedure, conn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        conn.Open();
                        var dReader = sqlCmd.ExecuteReader();
                        while (dReader.Read())
                        {
                            oNotes.Add(
                                new toDoAppModel
                                {
                                    id = Convert.ToInt32(dReader["id"]),
                                    task = dReader["task"].ToString(),
                                    expDate = dReader["expDate"].ToString(),
                                    notes = dReader["notes"].ToString()
                                }); 
                        }
                        dReader.Close();
                        conn.Close();
                    }

                    return new JsonResult(oNotes);
                }                
            }catch(Exception e)
            {
                Console.WriteLine(e);
                return new JsonResult(null);
            }            
        }

        [HttpPost]
        [Route("Create")]
        public JsonResult CreateNote(toDoAppModel note)
        {
            try
            {
                var connection = new DataConnection();
                var procedure = "sp_CreateNote";
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

            }catch(Exception e){

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
                var note = new toDoAppModel();
                var connection = new DataConnection();
                var procedure = "sp_GetANote";
                using (var conn = new SqlConnection(connection.GetString()))
                {
                    using (var sqlCmd = new SqlCommand(procedure, conn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("id", id);
                        conn.Open();
                        var dReader = sqlCmd.ExecuteReader();
                        if (dReader.Read())
                        {
                            note.id = Convert.ToInt32(dReader["id"]);
                            note.task = dReader["task"].ToString();
                            note.expDate = dReader["expDate"].ToString();
                            note.notes = dReader["notes"].ToString();
                        }
                        else
                        {
                            note = null;
                            Console.WriteLine("something went wrong");
                        }
                        dReader.Close();
                        conn.Close();
                    }
                }
                return new JsonResult(note);
            }
            catch (Exception e) { return new JsonResult(e); }
        }

        [HttpPut]
        [Route("updateNote")]
        public JsonResult UpdateNote(toDoAppModel notes)
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
                        sqlCmd.Parameters.AddWithValue("id", notes.id);
                        sqlCmd.Parameters.AddWithValue("task", notes.task);
                        sqlCmd.Parameters.AddWithValue("expDate", notes.expDate);
                        sqlCmd.Parameters.AddWithValue("notes", notes.notes);
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

        [HttpPost]
        [Route("UserAdd")]
        public JsonResult UserAdd(UserModel nUser)
        {
            try
            {
                var connection = new DataConnection();
                var procedure= "sp_UserAdd";
                using(var conn = new SqlConnection(connection.GetString()))
                {
                    using(var sqlCmd = new SqlCommand(procedure, conn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("username", nUser.username);
                        sqlCmd.Parameters.AddWithValue("password", nUser.password);
                        conn.Open();
                        sqlCmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                return new JsonResult("User added successfully!");
            }catch(Exception ex)
            {
                return new JsonResult(ex);
            }
         }

        [HttpPost]
        [Route("Login")]

        public JsonResult Login(UserModel user)
        {
            try
            {
                var connection = new DataConnection();
                var procedure = "sp_ValidateLogin";
                using(var conn = new SqlConnection(connection.GetString()))
                {
                    using(var sqlCmd = new SqlCommand(procedure, conn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("username", user.username);
                        sqlCmd.Parameters.AddWithValue("password", user.password);
                        conn.Open();
                        using(var dReader = sqlCmd.ExecuteReader())
                        {
                            if (dReader.Read())
                            {
                                user.id = Convert.ToInt32(dReader["id"]);
                                user.username = dReader["username"].ToString();                                
                            }
                            dReader.Close();
                        }
                        conn.Close();
                        
                    }
                }
                return new JsonResult(user);
            }catch(Exception ex)
            {
                return new JsonResult(ex);
            }
        }

    }
    
}
