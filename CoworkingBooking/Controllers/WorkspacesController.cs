using Microsoft.AspNetCore.Mvc;
using CoworkingBooking.Data;
using CoworkingBooking.Models;
using Microsoft.EntityFrameworkCore;
using CoworkingBooking.Models.DTO;



namespace CoworkingBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkspacesController : ControllerBase
    {
        private readonly AppDbContext context;
        public WorkspacesController(AppDbContext context)
        {
            this.context = context;
        }
        [HttpGet] // Get all workspaces
        public async Task<ActionResult<IEnumerable<Workspace>>> GetWorkspaces()
        {
            return await context.Workspaces.ToListAsync();
        }
        [HttpGet("{id}")] // Get workspace by id
        public async Task<ActionResult<Workspace>> GetWorkspace(int id)
        {
            var workspace = await context.Workspaces.FindAsync(id);

            if (workspace == null)
                return NotFound();

            return workspace;
        }
        [HttpPost] // Add(Create) new workspace
        public async Task<ActionResult<Workspace>> CreateWorkspace(Workspace workspace)
        {
            context.Workspaces.Add(workspace);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetWorkspace), new { id = workspace.Id }, workspace);
        }
        [HttpPut("{id}")] // Update workspace
        public async Task<IActionResult> UpdateWorkspace(int id, WorkspaceUpdateDto workspaceDto)
        {
            var workspace = await context.Workspaces.FindAsync(id);
            if (workspace == null)
                return NotFound();
            if (workspaceDto.Name != null)
                workspace.Name = workspaceDto.Name;
            if (workspaceDto.Location != null)
                workspace.Location = workspaceDto.Location;
            if (workspaceDto.IsAvailable != null)
                workspace.IsAvailable = workspaceDto.IsAvailable.Value;
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id}")] // Delete workspace
        public async Task<IActionResult> DeleteWorkspace(int id)
        {
            var workspace = await context.Workspaces.FindAsync(id);
            if (workspace == null)
                return NotFound();
            context.Workspaces.Remove(workspace);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
