using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Assignment_08_ADODOTNET
{
    internal class Program
    {
        static string connectionString = "server=DESKTOP-R0LG4NK\\SQLEXPRESS;database=Day8Db;trusted_connection=true";
        static void Main(string[] args)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                bool exit = false;
                do
                {
                    Console.WriteLine("\n===== Menu =====");
                    Console.WriteLine("1. Insert Records");
                    Console.WriteLine("2. Update Record");
                    Console.WriteLine("3. Delete Record");
                    Console.WriteLine("4. Search Record");
                    Console.WriteLine("5. Display All Records");
                    Console.WriteLine("6. Display Top 5 Records");
                    Console.WriteLine("7. Exit");
                    Console.Write("Enter your choice (1-7): ");
                    if (int.TryParse(Console.ReadLine(), out int choice))
                    {
                        switch (choice)
                        {
                            case 1:
                                InsertRecords(connection);
                                break;
                            case 2:
                                Console.Write("Enter PId to update: ");
                                string updatePId = Console.ReadLine();
                                Console.Write("Enter new price: ");
                                if (decimal.TryParse(Console.ReadLine(), out decimal newPrice))
                                {
                                    UpdateRecord(connection, updatePId, newPrice);
                                }
                                else
                                {
                                    Console.WriteLine("Invalid price input.");
                                }
                                break;
                            case 3:
                                Console.Write("Enter PId to delete: ");
                                string deletePId = Console.ReadLine();
                                DeleteRecord(connection, deletePId);
                                break;
                            case 4:
                                Console.Write("Enter product name to search: ");
                                string searchProductName = Console.ReadLine();
                                SearchRecord(connection, searchProductName);
                                break;
                            case 5:
                                DisplayAllRecords(connection);
                                break;
                            case 6:
                                DisplayTop5Records(connection);
                                break;
                            case 7:
                                Console.WriteLine("*** Thank you ***");
                                exit = true;
                                break;
                            default:
                                Console.WriteLine("Invalid choice. Please enter a number between 1 and 7.");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a number between 1 and 7.");
                        Console.ReadLine(); // Consume the invalid input
                    }
                } while (!exit);
            }
        }

        static void InsertRecords(SqlConnection connection)
        {
            string insertQuery = "INSERT INTO Products VALUES (@PId, @PName, @PPrice, @MnfDate, @ExpDate)";
            Console.Write("Enter the number of records to insert: ");
            if (int.TryParse(Console.ReadLine(), out int numberOfRecords) && numberOfRecords > 0)
            {
                for (int i = 1; i <= numberOfRecords; i++)
                {
                    Console.WriteLine($"\nEnter details for Record {i}:");
                    Console.WriteLine("Enter PId: ");
                    string pId = Console.ReadLine();
                    Console.Write("Enter PName: ");
                    string pName = Console.ReadLine();
                    Console.Write("Enter PPrice: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal pPrice))
                    {
                        Console.Write("Enter MnfDate (YYYY-MM-DD): ");
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime mnfDate))
                        {
                            Console.Write("Enter ExpDate (YYYY-MM-DD): ");
                            if (DateTime.TryParse(Console.ReadLine(), out DateTime expDate))
                            {
                                using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                                {
                                    insertCommand.Parameters.AddWithValue("@PId", pId);
                                    insertCommand.Parameters.AddWithValue("@PName", pName);
                                    insertCommand.Parameters.AddWithValue("@PPrice", pPrice);
                                    insertCommand.Parameters.AddWithValue("@MnfDate", mnfDate);
                                    insertCommand.Parameters.AddWithValue("@ExpDate", expDate);
                                    insertCommand.ExecuteNonQuery();
                                }
                                Console.WriteLine($"Record {i} inserted successfully.");
                            }
                            else
                            {
                                Console.WriteLine("Invalid ExpDate format.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid MnfDate format.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid PPrice input.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid input for the number of records.");
            }
        }

        static void UpdateRecord(SqlConnection connection, string pId, decimal newPrice)
        {
            string updateQuery = "UPDATE Products SET PPrice = @NewPrice WHERE PId = @PId";
            using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
            {
                updateCommand.Parameters.AddWithValue("@NewPrice", newPrice);
                updateCommand.Parameters.AddWithValue("@PId", pId);
                int rowsAffected = updateCommand.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine($"Record with PId {pId} updated successfully.");
                }
                else
                {
                    Console.WriteLine($"Record with PId {pId} not found.");
                }
            }
        }

        static void DeleteRecord(SqlConnection connection, string pId)
        {
            string deleteQuery = "DELETE FROM Products WHERE PId = @PId";
            using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection))
            {
                deleteCommand.Parameters.AddWithValue("@PId", pId);
                int rowsAffected = deleteCommand.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine($"Record with PId {pId} deleted successfully.");
                }
                else
                {
                    Console.WriteLine($"Record with PId {pId} not found.");
                }
            }
        }

        static void SearchRecord(SqlConnection connection, string productName)
        {
            string searchQuery = "SELECT * FROM Products WHERE PName = @ProductName";
            using (SqlCommand searchCommand = new SqlCommand(searchQuery, connection))
            {
                searchCommand.Parameters.AddWithValue("@ProductName", productName);
                using (SqlDataReader reader = searchCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        Console.WriteLine($"\nProduct found. Details for Product Name: {productName}\n");
                        Console.WriteLine("{0,-10} {1,-15} {2,-10} {3,-12} {4,-12}", "PId", "PName", "PPrice", "MnfDate", "ExpDate");
                        while (reader.Read())
                        {
                            Console.WriteLine("{0,-10} {1,-15} {2,-10} {3,-12} {4,-12}", reader["PId"], reader["PName"], reader["PPrice"], reader["MnfDate"], reader["ExpDate"]);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Product with name '{productName}' not found.");
                    }
                }
            }
        }


        static void DisplayAllRecords(SqlConnection connection)
        {
            string selectQuery = "SELECT * FROM Products";
            using (SqlCommand command = new SqlCommand(selectQuery, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Console.WriteLine("\nAll Records:\n");
                    Console.WriteLine("{0,-10} {1,-15} {2,-10} {3,-12} {4,-12}", "PId", "PName", "PPrice", "MnfDate", "ExpDate");
                    while (reader.Read())
                    {
                        Console.WriteLine("{0,-10} {1,-15} {2,-10} {3,-12} {4,-12}", reader["PId"], reader["PName"], reader["PPrice"], reader["MnfDate"], reader["ExpDate"]);
                    }
                }
            }
        }

        //static string GeneratePId()
        //{
        //    // Generate a new PId with the format "P" followed by 5 digits
        //    return $"P{new Random().Next(10000, 99999)}";
        //}

        static void DisplayTop5Records(SqlConnection connection)
        {
            string selectQuery = "SELECT TOP 5 * FROM Products ORDER BY PName DESC";
            using (SqlCommand command = new SqlCommand(selectQuery, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Console.WriteLine("Top 5 Records in Descending Order of PName:\n");
                    Console.WriteLine("{0,-10} {1,-15} {2,-10} {3,-12} {4,-12}", "PId", "PName", "PPrice", "MnfDate", "ExpDate");

                    while (reader.Read())
                    {
                        Console.WriteLine("{0,-10} {1,-15} {2,-10} {3,-12} {4,-12}", reader["PId"], reader["PName"], reader["PPrice"], reader["MnfDate"], reader["ExpDate"]);
                    }
                }
            }
        }
    }
}
