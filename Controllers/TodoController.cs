using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace To_Do.Controllers
{
    public class TodoController : Controller
    {
        private readonly DAL.ToDoItem _dalTodoItem;

        public TodoController(DAL.ToDoItem dalTodoItem)
        {
            _dalTodoItem = dalTodoItem;
        }
        public IActionResult Index()
        {
            return View(_dalTodoItem.Get());
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var items = _dalTodoItem.Get();
                return Ok(items);
            }
            catch (Exception e)
            {
                return BadRequest("Something went wrong");
            }
        }

        [HttpPost("AddItem")]
        public IActionResult AddItem(Models.ToDoItem obj)
        {
            bool result = _dalTodoItem.Add(obj);
            var item = _dalTodoItem.GetLatest();
            return result ? Ok($"{{\"Object\": {{\"Id\": \"{item.Id}\", \"Title\": \"{item.Title}\", \"DueDate\": \"{item.DueDate}\"}}, \"Message\": \"Added successfully!\"}}") : BadRequest("Failed adding a new task.");
        }

        [HttpPut("EditItem")]
        public IActionResult EditItem(Models.ToDoItem obj)
        {
            bool result = _dalTodoItem.Edit(obj);
            return result ? Ok("Changes saved!") : BadRequest("Failed to save changes.");
        }

        [HttpDelete("DeleteItem/{id}")]
        public IActionResult DeleteItem(int id)
        {
            bool result = _dalTodoItem.Delete(id);
            return result ? Ok("Task removed succesfully!") : BadRequest("Couldn't delete task.");
        }
    }
}
