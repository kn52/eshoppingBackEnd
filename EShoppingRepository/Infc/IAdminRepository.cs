﻿namespace EShoppingModel.Infc
{
    using EShoppingModel.Model;
    using EShoppingModel.Dto;
    public interface IAdminRepository
    {
        User AdminLogin(LoginDto loginDto);
    }
}
