using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using MetoLibrary.Models;
using MetoLibrary.Utilities.Logger;

namespace MetoLibrary.DAL.Repositorys
{
    public class ImageRepository
    {
        private readonly string _connectionString;

        public ImageRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> SaveImageAsync(string imagePath)
        {
            Logger.Log($"Attempting to save image: {imagePath}");
            byte[] imageData = File.ReadAllBytes(imagePath);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    Logger.Log("Database connection opened.");

                    string query = "INSERT INTO Images (ImagePath, ImageData, CreatedAt) VALUES (@ImagePath, @ImageData, @CreatedAt)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ImagePath", imagePath);
                        command.Parameters.AddWithValue("@ImageData", imageData);
                        command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

                        Logger.Log("Executing query.");
                        int result = await command.ExecuteNonQueryAsync();
                        if (result > 0)
                        {
                            Logger.Log($"Image saved to database: {imagePath}");
                            return true;
                        }
                        else
                        {
                            Logger.Log($"Failed to save image to database: {imagePath}");
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log($"Exception: {ex.Message}");
                    Logger.Log(ex);
                    return false;
                }
            }
        }

        public async Task<List<ImageModel>> GetImagesAsync()
        {
            List<ImageModel> images = new List<ImageModel>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT Id, ImagePath, ImageData, CreatedAt FROM Images";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            images.Add(new ImageModel
                            {
                                Id = reader.GetInt32(0),
                                ImagePath = reader.GetString(1),
                                ImageData = (byte[])reader["ImageData"],
                                CreatedAt = reader.GetDateTime(3)
                            });
                        }
                    }
                }
            }
            return images;
        }
    }
}
