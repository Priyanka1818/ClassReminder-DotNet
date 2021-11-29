using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace Login.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly model.ConnectionStrings con;
        private IConfiguration _configuration;
        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// List users
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] model.User vm)
        {
            var connstring = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            return await Task.Run(() =>
            {
                using (var c = new MySqlConnection(connstring))
                {

                    var sql = @"SELECT * FROM user 
                                WHERE (@id = 0 OR id = @id) 
                                AND (@name IS NULL OR UPPER(name) = UPPER(@name))";
                    var query = c.Query<model.User>(sql, vm, commandTimeout: 30);
                    return Ok(query);
                }
            });
        }

        /// <summary>
        /// Create user
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] model.User vm)
        {
            return await Task.Run(() =>
            {
                using (var c = new MySqlConnection(con.MySQL))
                {
                    var sql = @"INSERT INTO user (name) VALUES (@name)";
                    c.Execute(sql, vm, commandTimeout: 30);
                    return Ok();
                }
            });
        }
    }
}
