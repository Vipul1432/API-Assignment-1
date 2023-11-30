using API_CRUD.Context;
using API_CRUD.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KeyValueServiceController : ControllerBase
    {
        private readonly KeyValueServiceContext _context;

        public KeyValueServiceController(KeyValueServiceContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a key-value pair by the provided key.
        /// </summary>
        /// <param name="key">The key to search for.</param>
        /// <returns>Returns the key-value pair if found; otherwise, returns a 404 (Not Found) response.</returns>
        [HttpGet("{key}")]
        public async Task<IActionResult> Get(string key)
        {
            try
            {
                // Using FirstOrDefaultAsync instead of FindAsync
                var keyValue = await _context.KeyValues.FirstOrDefaultAsync(x => x.Key == key);

                if (keyValue == null)
                {
                    return NotFound($"Key '{key}' not found.");
                }

                return Ok(keyValue);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Adds or updates a key-value pair in the database.
        /// </summary>
        /// <param name="keyValue">The key-value pair to be added or updated.</param>
        /// <returns>Returns Ok if the key-value pair is added successfully; otherwise, returns a 409 (Conflict) response if the key already exists.</returns>
        [HttpPost]
        public async Task<IActionResult> AddOrUpdate([FromBody] KeyValueRequest keyValue)
        {
            try
            {
                var existingKeyValue = await _context.KeyValues.FirstOrDefaultAsync(x => x.Key == keyValue.Key);

                if (existingKeyValue != null)
                {
                    return Conflict($"Key '{keyValue.Key}' already exists.");
                }

                _context.KeyValues.Add(keyValue);
                await _context.SaveChangesAsync();

                return Ok($"Key '{keyValue.Key}' added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the value of an existing key in the database.
        /// </summary>
        /// <param name="key">The key to update.</param>
        /// <param name="value">The new value for the key.</param>
        /// <returns>Returns Ok if the key is updated successfully; otherwise, returns a 404 (Not Found) response if the key is not found.</returns>
        [HttpPatch("{key}/{value}")]
        public async Task<IActionResult> Update(string key, string value)
        {
            try
            {
                var keyValue = await _context.KeyValues.FirstOrDefaultAsync(x => x.Key == key);

                if (keyValue == null)
                {
                    return NotFound($"Key '{key}' not found.");
                }

                keyValue.Value = value;
                await _context.SaveChangesAsync();

                return Ok($"Value of key '{key}' updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a key-value pair by the provided key.
        /// </summary>
        /// <param name="key">The key to delete.</param>
        /// <returns>Returns Ok if the key-value pair is deleted successfully; otherwise, returns a 404 (Not Found) response if the key is not found.</returns>
        [HttpDelete("{key}")]
        public async Task<IActionResult> Delete(string key)
        {
            try
            {
                var keyValue = await _context.KeyValues.FirstOrDefaultAsync(x => x.Key == key);

                if (keyValue == null)
                {
                    return NotFound($"Key '{key}' not found.");
                }

                _context.KeyValues.Remove(keyValue);
                await _context.SaveChangesAsync();

                return Ok($"Key '{key}' deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
