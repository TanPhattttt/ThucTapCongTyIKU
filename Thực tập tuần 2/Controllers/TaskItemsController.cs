using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Thực_tập_tuần_2.Data;
using Thực_tập_tuần_2.Models;
using Thực_tập_tuần_2.Repositories;

namespace Thực_tập_tuần_2.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskItemsController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;

        public TaskItemsController(ITaskRepository context)
        {
            _taskRepository = context;
        }

        private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // GET: api/TaskItems
        [Authorize(Roles = "User,Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
        {
            var userId = GetUserId();
            var tasks = await _taskRepository.GetTasksByUserIdAsync(userId);
            return Ok(tasks);
        }

        // GET: api/TaskItems/5
        [Authorize(Roles = "User,Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetTaskItem(Guid id)
        {
            var task = await _taskRepository.GetTaskByIdAsync(id); // chỉ lọc theo id
            if (task == null) return NotFound();
            return Ok(task);
        }

        // PUT: api/TaskItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "User,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskItem(Guid id, TaskItem taskItem)
        {
            var userId = GetUserId();
            var task = await _taskRepository.GetTaskByIdAsync(id);
            if (task == null) return NotFound();

            task.Title = taskItem.Title;
            task.Description = taskItem.Description;
            task.IsCompleted = taskItem.IsCompleted;

            var result = await _taskRepository.UpdateTaskAsync(task);
            return result ? NoContent() : StatusCode(500);
        }

        // POST: api/TaskItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public async Task<ActionResult<TaskItem>> PostTaskItem(TaskItem taskItem)
        {
            var result = await _taskRepository.CreateTaskAsync(taskItem);
            return CreatedAtAction(nameof(GetTaskItem), new { id = result.Id }, result);
        }

        // DELETE: api/TaskItems/5
        [Authorize(Roles = "User,Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskItem(Guid id)
        {
            var userId = GetUserId();
            var result = await _taskRepository.DeleteTaskAsync(id, userId);

            if (!result)
                return NotFound();

            return Ok();
        }

    }
}
