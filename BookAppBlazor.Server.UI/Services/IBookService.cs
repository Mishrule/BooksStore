﻿using BookAppBlazor.Server.UI.Services.Base;

namespace BookAppBlazor.Server.UI.Services
{
  public interface IBookService
  {
  
    Task<Response<List<BookReadOnlyDto>>> Get();
      Task<Response<BookDetailsDto>> Get(int id);
      Task<Response<BookUpdateDto>> GetForUpdate(int id);
      Task<Response<int>> Create(BookCreateDto author);
      Task<Response<int>> Edit(int id, BookUpdateDto author);
      Task<Response<int>> Delete(int id);
    
  }
}
