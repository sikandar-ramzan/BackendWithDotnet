using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Task2.Controllers
{
    [Route("api/todo")]
    [ApiController]
    [Authorize(Roles = "Admin, User")]

    public class TodoController : ControllerBase
    {
        
        private readonly DataContext _dataContext;

        public TodoController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        

        private string GetUserIdFromClaims()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity?.FindFirst("UserId");

            return userId?.Value;
        }

        private string GetUserRoleFromClaims()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var roleClaim = identity?.FindFirst(ClaimTypes.Role);

            return roleClaim?.Value;
        }

        //GET All Todos
        [HttpGet] 
        public async Task<ActionResult<List<Todo>>> GetAllTodos()
        {
            var userRole = GetUserRoleFromClaims();
            List<Todo> todos;

            if (userRole == "Admin")
            {
                todos = await _dataContext.Todos.ToListAsync();
            }
            else if (userRole == "User")
            {
                string userId = GetUserIdFromClaims();
                todos = await _dataContext.Todos.Where(todo => todo.AuhtorId == userId).ToListAsync();
            }
            else
            {
                return Forbid(); // Return 403 Forbidden if user has unknown role
            }

            return Ok(todos);
            
        }

        //GET Id specific Todo
        [HttpGet("{id}")] 
        public async Task<ActionResult<Todo>> GetTodo(int id)
        {
            var userRole = GetUserRoleFromClaims();

            var todo = await _dataContext.Todos.FindAsync(id);
            if (todo == null)
                return NotFound($"No todo found corresponding to id: {id}");

            if (userRole == "User" && todo.AuhtorId != GetUserIdFromClaims())
                return Forbid("You are unauthorized to do this"); // User can only access their own todos

            return Ok(todo);
            
        }

        //POST Todo
        [HttpPost] 
        public async Task<ActionResult<List<Todo>>> CreateTodo(Todo todo)
        {
            var todoFromDb = await _dataContext.Todos.FindAsync(todo.Id);
            if (todoFromDb != null)
                return BadRequest($"Todo with the same Id: {todo.Id} already exists!");

            string userId = GetUserIdFromClaims();

            Todo newTodo = new Todo()
            {
                Id = todo.Id,
                Title = todo.Title,
                IsCompleted = todo.IsCompleted,
                AuhtorId = userId,
            };

            _dataContext.Todos.Add(newTodo);
            await _dataContext.SaveChangesAsync();
            return Ok(await _dataContext.Todos.ToListAsync());
        }

        //UPDATE Todo
        [HttpPut("{id}")] 
        public async Task<ActionResult<List<Todo>>> UpdateTodo(int id, TodoDto updatedTodo)
        {

            var userRole = GetUserRoleFromClaims();

            var todoFromDb = await _dataContext.Todos.FindAsync(id);
            if (todoFromDb == null)
                return BadRequest($"No todo found corresponding to id: {id}");

            if (userRole == "User" && todoFromDb.AuhtorId != GetUserIdFromClaims())
                return Forbid("You are unauthorized to do this"); // User can only update their own todos

            if (todoFromDb.Id != updatedTodo.Id)
                return BadRequest($"Cannot override todo id.\nOriginal id: {id}\nPayload id: {updatedTodo.Id}");

            todoFromDb.Title = updatedTodo.Title;
            todoFromDb.IsCompleted = updatedTodo.IsCompleted;

            await _dataContext.SaveChangesAsync();

            return Ok(await _dataContext.Todos.ToListAsync());
        }

        //DELETE Todo
        [HttpDelete("{id}")] 
        public async Task<ActionResult<List<Todo>>> DeleteTodo(int id)
        {
            var userRole = GetUserRoleFromClaims();

            var todoFromDb = await _dataContext.Todos.FindAsync(id);
            if (todoFromDb == null)
                return BadRequest($"No todo found corresponding to id: {id}");

            if (userRole == "User" && todoFromDb.AuhtorId != GetUserIdFromClaims())
                return Forbid("You are unauthorized to do this"); // User can only delete their own todos


            _dataContext.Todos.Remove(todoFromDb);

            await _dataContext.SaveChangesAsync();

            return Ok(await _dataContext.Todos.ToListAsync());
        }
    }
}
