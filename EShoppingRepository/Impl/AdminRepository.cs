﻿namespace EShoppingModel.Impl
{
    using EShoppingModel.Model;
    using EShoppingModel.Dto;
    using EShoppingModel.Infc;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using EShoppingModel.Util;
    public class AdminRepository : IAdminRepository
    {
        public AdminRepository(IConfiguration configuration)
        {
            this.Configuration = configuration;
            DBString = this.Configuration["ConnectionString:DBConnection"];
        }
        public IConfiguration Configuration { get; set; }
        public User AdminLogin(LoginDto loginDto)
        {
            using (SqlConnection conn = new SqlConnection(this.DBString))
            {
                using (SqlCommand cmd = new SqlCommand("spLogin", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.AddWithValue("@email", loginDto.email);
                    cmd.Parameters.AddWithValue("@user_role", 0);

                    try
                    {
                        conn.Open();
                        SqlDataReader rdr = cmd.ExecuteReader();
                        if (rdr.HasRows)
                        {
                            User admin =  new User();
                            while (rdr.Read())
                            {
                                var epass = SaltGenerator.EncodePassword(loginDto.password, rdr["key_new"].ToString());
                                var dpass = SaltGenerator.Base64Decode(rdr["password"].ToString());
                                if (epass.Equals(dpass))
                                {
                                    admin.id = Convert.ToInt32(rdr["id"]);
                                    admin.fullName = rdr["full_name"].ToString();
                                    admin.email = rdr["email"].ToString();
                                    admin.password = rdr["password"].ToString();
                                    admin.phoneNo = rdr["phone_no"].ToString();
                                    admin.emailVerified = (bool)rdr["email_verified"];
                                    admin.userRole = Convert.ToInt32(rdr["user_role"]);
                                }
                                return admin;
                            }
                            return null;
                        }
                    }
                    catch
                    {
                        return null;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            return null;
        }
        public string AddBook(BookDto bookDto)
        {
            using (SqlConnection conn = new SqlConnection(this.DBString))
            {
                using (SqlCommand cmd = new SqlCommand("spAddBook", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.AddWithValue("@auther_name", bookDto.authorName);
                    cmd.Parameters.AddWithValue("@book_detail", bookDto.bookDetail);
                    cmd.Parameters.AddWithValue("@book_image_src", bookDto.bookImageSrc);
                    cmd.Parameters.AddWithValue("@book_name", bookDto.bookName);
                    cmd.Parameters.AddWithValue("@book_price", bookDto.bookPrice);
                    cmd.Parameters.AddWithValue("@isbn_number", bookDto.isbnNumber);
                    cmd.Parameters.AddWithValue("@no_of_copies", bookDto.noOfCopies);
                    cmd.Parameters.AddWithValue("@publishing_year", bookDto.publishingYear);
                    cmd.Parameters.Add("@key", SqlDbType.Int).Direction = ParameterDirection.Output;

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        string key = cmd.Parameters["@key"].Value.ToString();
                        if (key != "")
                        {
                            return key;
                        }
                    }
                    catch
                    {
                        return null;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            return "Book Not Added";
        }
        public string UpdateBook(BookDto bookDto)
        {
            using (SqlConnection conn = new SqlConnection(this.DBString))
            {
                using (SqlCommand cmd = new SqlCommand("spUpdateBook", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.AddWithValue("@auther_name", bookDto.authorName);
                    cmd.Parameters.AddWithValue("@book_detail", bookDto.bookDetail);
                    cmd.Parameters.AddWithValue("@book_image_src", bookDto.bookImageSrc);
                    cmd.Parameters.AddWithValue("@book_name", bookDto.bookName);
                    cmd.Parameters.AddWithValue("@book_price", bookDto.bookPrice);
                    cmd.Parameters.AddWithValue("@isbn_number", bookDto.isbnNumber);
                    cmd.Parameters.AddWithValue("@no_of_copies", bookDto.noOfCopies);
                    cmd.Parameters.AddWithValue("@publishing_year", bookDto.publishingYear);
                    
                    try
                    {
                        conn.Open();
                        int count = cmd.ExecuteNonQuery();
                        if (count > 0)
                        {
                            return "Book Updated Successfully";
                        }
                    }
                    catch                    
                    {
                        return null;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            return "Book Not Found";
        }
        public string DeleteBook(int bookId)
        {
            using (SqlConnection conn = new SqlConnection(this.DBString))
            {
                using (SqlCommand cmd = new SqlCommand("spDeleteBook", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.AddWithValue("@isbn_number", bookId);

                    try
                    {
                        conn.Open();
                        int count = cmd.ExecuteNonQuery();
                        if (count > 0)
                        {
                            return "Book Deleted Successfully";
                        }
                    }
                    catch
                    {
                        return null;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            return "Book Not Found";
        }
        public string GenerateJSONWebToken(int userId, string userRole)
        {
            return TokenGenerator.GenerateJSONWebToken(userId,userRole, Configuration);
        }
        
        private readonly string DBString = null;
    }
}
