﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Author;
using BookStoreApp.API.Static;
using Microsoft.AspNetCore.Authorization;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthorsController : ControllerBase
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthorsController> _logger;

        public AuthorsController(BookStoreDbContext context, IMapper mapper, ILogger<AuthorsController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorReadOnlyDto>>> GetAuthors()
        {
            _logger.LogInformation("Get all authors");
            try
            {
                var authors = await _context.Authors.ToListAsync();
                var authorDto = _mapper.Map<IEnumerable<AuthorReadOnlyDto>>(authors);
                return Ok(authorDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get authors: {ex}");
                return StatusCode(500, ex.Message);
            }
            
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        
        public async Task<ActionResult<AuthorDetailsDto>> GetAuthor(int id)
        {
            _logger.LogInformation("Get all author");
            try
            {
                if (_context.Authors == null)
                {
                    return NotFound();
                }

                var author = await _context.Authors
                    .Include(a => a.Books)
                    .ProjectTo<AuthorDetailsDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(q => q.Id == id);

                if (author == null)
                {
                    return NotFound();
                }

               // var authorDto = _mapper.Map<AuthorReadOnlyDto>(author);
                return Ok(author);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get authors: {ex}");
                return StatusCode(500, Messages.ErrorMessage);
            }
            
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutAuthor(int id, AuthorUpdateDto authorDto)
        {
            if (id != authorDto.Id)
            {
                return BadRequest();
            }

            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            _mapper.Map(authorDto, author);
            _context.Entry(authorDto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await AuthorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AuthorCreateDto>> PostAuthor(AuthorCreateDto authorDto)
        {
          if (_context.Authors == null)
          {
              return Problem("Entity set 'BookStoreDbContext.Authors'  is null.");
          }

          var author = _mapper.Map<Author>(authorDto);
          await _context.Authors.AddAsync(author);
          await _context.SaveChangesAsync();

          return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            try
            {
                if (_context.Authors == null)
                {
                    return NotFound();
                }

                var author = await _context.Authors.FindAsync(id);
                if (author == null)
                {
                    _logger.LogWarning($"Update ID invalid in {nameof(DeleteAuthor)} - ID: {id}");
                    return NotFound();
                }

                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();

                _logger.LogWarning($"{nameof(Author)} record not found in {nameof(DeleteAuthor)} - ID: {id}");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, $"Error performing DELETE in {nameof(DeleteAuthor)}");
                return StatusCode(500, Messages.ErrorMessage);
            }
           
        }

        private async Task<bool> AuthorExists(int id)
        {
            return await _context.Authors?.AnyAsync(e => e.Id == id);
        }
    }
}
