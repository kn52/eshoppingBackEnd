﻿namespace EShoppingModel.Infc
{
    using EShoppingModel.Model;
    using EShoppingModel.Dto;
    public interface IAdminRepository
    {
        User AdminLogin(LoginDto loginDto);
        string AddBook(BookDto bookDto, string token);
        string UpdateBook(BookDto bookDto, string token);
        string DeleteBook(int bookId, string token);
        string GenerateJSONWebToken(int userId);
    }
}
