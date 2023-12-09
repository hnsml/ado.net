using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.Text;
using System.Xml.Linq;

class SkladTable
{
    private string connectionString = "Data Source=SkladTable.sqlite";

    public void CreateSkladTable()
    {
        Console.OutputEncoding = Encoding.UTF8;
        string createSuppliersTable = @"
            CREATE TABLE IF NOT EXISTS Suppliers (
                SupplierId INTEGER UNIQUE PRIMARY KEY,
                SupplierName NVARCHAR(255) NOT NULL
            );
        ";
        string createTypesTable = @"
            CREATE TABLE IF NOT EXISTS Types (
                TypeId INTEGER UNIQUE PRIMARY KEY,
                TypeName NVARCHAR(255) NOT NULL
            );
        ";
        string createGoodsTable = @"
            CREATE TABLE IF NOT EXISTS Goods (
                ProductId INTEGER PRIMARY KEY AUTOINCREMENT,
                Name NVARCHAR(255) UNIQUE NOT NULL,
                TypeId INTEGER NOT NULL,
                SupplierId INTEGER NOT NULL,
                Quantity INTEGER NOT NULL,
                CostPrice DECIMAL(18,2) NOT NULL,
                SupplyDate DATETIME NOT NULL,
                FOREIGN KEY(TypeId) REFERENCES Types(TypeId),
                FOREIGN KEY(SupplierId) REFERENCES Suppliers(SupplierId)
            );
        ";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            using (SqliteCommand command = new SqliteCommand(createSuppliersTable, connection))
            {
                command.ExecuteNonQuery();
            }

            using (SqliteCommand command = new SqliteCommand(createTypesTable, connection))
            {
                command.ExecuteNonQuery();
            }

            using (SqliteCommand command = new SqliteCommand(createGoodsTable, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        string insertTypes = @"
            INSERT OR IGNORE INTO Types (TypeId, TypeName) VALUES (1, 'Ноутбук'), (2, 'Планшет'), (3, 'Монітор');
        ";

        string insertSuppliers = @"
            INSERT OR IGNORE INTO Suppliers (SupplierId, SupplierName) VALUES (1, 'Sumsung'), (2, 'Dell'), (3, 'Lenovo');
        ";

        string insertGoods = @"
            INSERT OR IGNORE INTO Goods (ProductId, Name, TypeId, SupplierId, Quantity, CostPrice, SupplyDate) VALUES 
            (1,'Ноутбук Lenovo IdeaPad Slim 5 16IAH8', 1, 3, 4, 26999, '2023-11-29'),
            (2,'Ноутбук Samsung Galaxy Book2 Pro', 1, 1, 3, 45999, '2023-10-29'),
            (3,'Ноутбук Dell Latitude 5540', 1, 2, 2, 60849, '2022-11-29'),
            (4,'Планшет Lenovo Tab P11 Plus', 2, 3, 3, 9999, '2023-02-25'),
            (5,'Планшет Samsung Galaxy Tab S7', 2, 1, 1, 20899, '2023-08-29'),
            (6,'Dell Latitude 7212 Rugged Extreme Tablet i5', 2, 2, 1, 17450, '2023-01-29'),
            (7,'Монитор 28 Samsung Odyssey G7 LS28BG702', 3, 1, 2, 18999, '2023-07-29'),
            (8,'Монитор Lenovo 29 L29w-30', 3, 3, 4, 7999, '2023-04-29'),
            (9,'Монитор 34 Dell Alienware AW3423DWF', 3, 2, 2, 37999, '2023-08-29'),
            (10,'Монитор 28 Samsung Odyssey G7 LS28BG702', 3, 1, 2, 18999, '2023-07-29');
        ";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            using (SqliteCommand command = new SqliteCommand(insertTypes, connection))
            {
                command.ExecuteNonQuery();
            }

            using (SqliteCommand command = new SqliteCommand(insertSuppliers, connection))
            {
                command.ExecuteNonQuery();
            }

            using (SqliteCommand command = new SqliteCommand(insertGoods, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }

    public void DisplayAllProducts(SqliteConnection connection)
    {
        string query = "SELECT * FROM Goods";
        using (SqliteCommand command = new SqliteCommand(query, connection))
        using (SqliteDataReader reader = command.ExecuteReader())
        {
            Console.WriteLine("\nВся інформація про товар:");
            while (reader.Read())
            {
                Console.WriteLine($"{reader["ProductId"]},{reader["Name"]}, {reader["TypeId"]}, {reader["SupplierId"]}, " +
                                  $"{reader["Quantity"]}, {reader["CostPrice"]}, {reader["SupplyDate"]}");
            }
        }
    }

    public void DisplayAllProductTypes(SqliteConnection connection)
    {
        string query = "SELECT DISTINCT TypeName FROM Types";
        using (SqliteCommand command = new SqliteCommand(query, connection))
        using (SqliteDataReader reader = command.ExecuteReader())
        {
            Console.WriteLine("\nУсі типи товарів:");
            while (reader.Read())
            {
                Console.WriteLine(reader["TypeName"]);
            }
        }
    }

    public void DisplayAllSuppliers(SqliteConnection connection)
    {
        string query = "SELECT DISTINCT SupplierName FROM Suppliers";
        using (SqliteCommand command = new SqliteCommand(query, connection))
        using (SqliteDataReader reader = command.ExecuteReader())
        {
            Console.WriteLine("\nУсі постачальники:");
            while (reader.Read())
            {
                Console.WriteLine(reader["SupplierName"]);
            }
        }
    }

    public void DisplayProductWithMaxQuantity(SqliteConnection connection)
    {
        string query = "SELECT * FROM Goods ORDER BY Quantity DESC LIMIT 1";
        using (SqliteCommand command = new SqliteCommand(query, connection))
        using (SqliteDataReader reader = command.ExecuteReader())
        {
            Console.WriteLine("\nТовар з максимальною кількістю:");
            while (reader.Read())
            {
                Console.WriteLine($"{reader["Name"]}, {reader["Quantity"]}");
            }
        }
    }

    public void DisplayProductWithMinQuantity(SqliteConnection connection)
    {
        string query = "SELECT * FROM Goods ORDER BY Quantity ASC LIMIT 1";
        using (SqliteCommand command = new SqliteCommand(query, connection))
        using (SqliteDataReader reader = command.ExecuteReader())
        {
            Console.WriteLine("\nТовар з мінімальною кількістю:");
            while (reader.Read())
            {
                Console.WriteLine($"{reader["Name"]}, {reader["Quantity"]}");
            }
        }
    }

    public void DisplayProductWithMinCost(SqliteConnection connection)
    {
        string query = "SELECT * FROM Goods ORDER BY CostPrice ASC LIMIT 1";
        using (SqliteCommand command = new SqliteCommand(query, connection))
        using (SqliteDataReader reader = command.ExecuteReader())
        {
            Console.WriteLine("\nТовар з мінімальною собівартістю:");
            while (reader.Read())
            {
                Console.WriteLine($"{reader["Name"]}, {reader["CostPrice"]}");
            }
        }
    }

    public void DisplayProductWithMaxCost(SqliteConnection connection)
    {
        string query = "SELECT * FROM Goods ORDER BY CostPrice DESC LIMIT 1";
        using (SqliteCommand command = new SqliteCommand(query, connection))
        using (SqliteDataReader reader = command.ExecuteReader())
        {
            Console.WriteLine("\nТовар з максимальною собівартістю:");
            while (reader.Read())
            {
                Console.WriteLine($"{reader["Name"]}, {reader["CostPrice"]}");
            }
        }
    }

    public void DisplayProductsByCategory(SqliteConnection connection, string category)
    {
        string query = $"SELECT * FROM Goods INNER JOIN Types ON Goods.TypeId = Types.TypeId WHERE TypeName = '{category}'";
        using (SqliteCommand command = new SqliteCommand(query, connection))
        using (SqliteDataReader reader = command.ExecuteReader())
        {
            Console.WriteLine($"\nТовари категорії '{category}':");
            while (reader.Read())
            {
                Console.WriteLine($"{reader["Name"]}, {reader["TypeName"]}");
            }
        }
    }

    public void DisplayProductsBySupplier(SqliteConnection connection, string supplier)
    {
        string query = $"SELECT * FROM Goods INNER JOIN Suppliers ON Goods.SupplierId = Suppliers.SupplierId WHERE SupplierName = '{supplier}'";
        using (SqliteCommand command = new SqliteCommand(query, connection))
        using (SqliteDataReader reader = command.ExecuteReader())
        {
            Console.WriteLine($"\nТовари постачальника '{supplier}':");
            while (reader.Read())
            {
                Console.WriteLine($"{reader["Name"]}, {reader["SupplierName"]}");
            }
        }
    }

    public void DisplayProductWithLongestStorage(SqliteConnection connection)
    {
        string query = "SELECT * FROM Goods ORDER BY SupplyDate ASC LIMIT 1";
        using (SqliteCommand command = new SqliteCommand(query, connection))
        using (SqliteDataReader reader = command.ExecuteReader())
        {
            Console.WriteLine("\nТовар, який знаходиться на складі найдовше:");
            while (reader.Read())
            {
                Console.WriteLine($"{reader["Name"]}, {reader["SupplyDate"]}");
            }
        }
    }

    public void DisplayAverageQuantityByProductType(SqliteConnection connection)
    {
        string query = "SELECT TypeName, AVG(Quantity) AS AverageQuantity FROM Goods INNER JOIN Types ON Goods.TypeId = Types.TypeId GROUP BY TypeName";
        using (SqliteCommand command = new SqliteCommand(query, connection))
        using (SqliteDataReader reader = command.ExecuteReader())
        {
            Console.WriteLine("\nСередня кількість товарів за кожним типом:");
            while (reader.Read())
            {
                Console.WriteLine($"{reader["TypeName"]}, {reader["AverageQuantity"]}");
            }
        }
    }

    static void Main()
    {
        SkladTable skladTable = new SkladTable();
        skladTable.CreateSkladTable();

        using (SqliteConnection connection = new SqliteConnection(skladTable.connectionString))
        {
            try
            {
                connection.Open();
                Console.WriteLine("Підключено до бази даних.");

                skladTable.DisplayAllProducts(connection);
                skladTable.DisplayAllProductTypes(connection);
                skladTable.DisplayAllSuppliers(connection);
                skladTable.DisplayProductWithMaxQuantity(connection);
                skladTable.DisplayProductWithMinQuantity(connection);
                skladTable.DisplayProductWithMinCost(connection);
                skladTable.DisplayProductWithMaxCost(connection);
                skladTable.DisplayProductsByCategory(connection, "Ноутбук");
                skladTable.DisplayProductsBySupplier(connection, "Sumsung");
                skladTable.DisplayProductWithLongestStorage(connection);
                Console.WriteLine();
                skladTable.InsertNewType(connection, 4, "Телефон");
                skladTable.InsertNewSupplier(connection, 4, "MSI");
                skladTable.InsertNewProduct(connection, 11, "Монитор 28\" Samsung Odyssey G7", 3, 1, 5, 18900, Convert.ToDateTime("2023-12-09"));
                Console.WriteLine();
                skladTable.DeleteType(connection, 4);
                skladTable.DeleteSupplier(connection, 4);
                skladTable.DeleteProduct(connection, 11);
                skladTable.DisplayAverageQuantityByProductType(connection);
                skladTable.DisplaySupplierWithMaxProducts(connection);
                skladTable.DisplaySupplierWithMinProducts(connection);
                skladTable.DisplayTypeWithMaxQuantity(connection);
                skladTable.DisplayTypesWithMinQuantity(connection);
                skladTable.DisplayProductsPastSupplyDate(connection, 30);
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }
    }
    public void InsertNewProduct(SqliteConnection connection, int productId, string name, int typeId, int supplierId, int quantity, decimal costPrice, DateTime supplyDate)
    {
        string query = $@"
        INSERT INTO Goods (ProductId, Name, TypeId, SupplierId, Quantity, CostPrice, SupplyDate) VALUES 
        ('{productId}','{name}', {typeId}, {supplierId}, {quantity}, {costPrice}, '{supplyDate:yyyy-MM-dd}')
    ";

        using (SqliteCommand command = new SqliteCommand(query, connection))
        {
            command.ExecuteNonQuery();
        }

        Console.WriteLine("Додано новий товар: {0}, {1}, {2}, {3}, {4}, {5}, {6}", productId, name, typeId, supplierId, quantity, costPrice, supplyDate);
    }

    public void InsertNewType(SqliteConnection connection, int typeId, string typeName)
    {
        string query = $@"
        INSERT INTO Types (TypeId, TypeName) VALUES ({typeId}, '{typeName}')
    ";

        using (SqliteCommand command = new SqliteCommand(query, connection))
        {
            command.ExecuteNonQuery();
        }
        Console.WriteLine("Додано новий тип: {0}, {1}", typeId, typeName);
    }

    public void InsertNewSupplier(SqliteConnection connection, int supplierId, string supplierName)
    {
        string query = $@"
        INSERT INTO Suppliers (SupplierId, SupplierName) VALUES ({supplierId}, '{supplierName}')
    ";

        using (SqliteCommand command = new SqliteCommand(query, connection))
        {
            command.ExecuteNonQuery();
        }
        Console.WriteLine("Додано нового постачальника: {0}, {1}", supplierId, supplierName);
    }

    public void UpdateType(SqliteConnection connection, int typeId, string newTypeName)
    {
        string query = $@"
        UPDATE Types SET TypeName = '{newTypeName}' WHERE TypeId = {typeId}
    ";

        using (SqliteCommand command = new SqliteCommand(query, connection))
        {
            command.ExecuteNonQuery();
        }
    }

    public void DeleteType(SqliteConnection connection, int typeId)
    {
        string query = $@"
        DELETE FROM Types WHERE TypeId = {typeId}
    ";

        using (SqliteCommand command = new SqliteCommand(query, connection))
        {
            command.ExecuteNonQuery();
        }
        Console.WriteLine("Видалено тип з індексом: " + typeId);
    }

    public void UpdateProduct(SqliteConnection connection, string name, int typeId, int supplierId, int quantity, decimal costPrice, DateTime supplyDate)
    {
        string query = $@"
        UPDATE Goods SET TypeId = {typeId},SupplierId= {supplierId},Quantity= {quantity},CostPrice= {costPrice},SupplyDate= {supplyDate} WHERE Name = {name}
    ";

        using (SqliteCommand command = new SqliteCommand(query, connection))
        {
            command.ExecuteNonQuery();
        }

    }

    public void UpdateSupplier(SqliteConnection connection, int supplierId, string newSupplierName)
    {
        string query = $@"
        UPDATE Suppliers SET SupplierName = '{newSupplierName}' WHERE SupplierId = {supplierId}
    ";

        using (SqliteCommand command = new SqliteCommand(query, connection))
        {
            command.ExecuteNonQuery();
        }
    }

    public void DeleteProduct(SqliteConnection connection, int productId)
    {
        string query = $@"
        DELETE FROM Goods WHERE ProductId = {productId}
    ";

        using (SqliteCommand command = new SqliteCommand(query, connection))
        {
            command.ExecuteNonQuery();
        }
        Console.WriteLine("Видалено товар з індексом: " + productId);
    }

    public void DeleteSupplier(SqliteConnection connection, int supplierId)
    {
        string query = $@"
        DELETE FROM Suppliers WHERE SupplierId = {supplierId}
    ";

        using (SqliteCommand command = new SqliteCommand(query, connection))
        {
            command.ExecuteNonQuery();
        }
        Console.WriteLine("Видалено постачальника з індексом: " + supplierId);
    }

    public void DisplaySupplierWithMaxProducts(SqliteConnection connection)
    {
        string query = "SELECT SupplierName FROM Suppliers INNER JOIN Goods ON Suppliers.SupplierId = Goods.SupplierId GROUP BY SupplierName ORDER BY SUM(Quantity) DESC LIMIT 1";
        using (SqliteCommand command = new SqliteCommand(query, connection))
        using (SqliteDataReader reader = command.ExecuteReader())
        {
            Console.WriteLine("\nІнформація про постачальника з найбільшою кількістю товарів:");
            while (reader.Read())
            {
                Console.WriteLine(reader["SupplierName"]);
            }
        }
    }

    public void DisplaySupplierWithMinProducts(SqliteConnection connection)
    {
        string query = "SELECT SupplierName FROM Suppliers INNER JOIN Goods ON Suppliers.SupplierId = Goods.SupplierId GROUP BY SupplierName ORDER BY SUM(Quantity) ASC LIMIT 1";
        using (SqliteCommand command = new SqliteCommand(query, connection))
        using (SqliteDataReader reader = command.ExecuteReader())
        {
            Console.WriteLine("\nІнформація про постачальника з найменшою кількістю товарів:");
            while (reader.Read())
            {
                Console.WriteLine(reader["SupplierName"]);
            }
        }
    }

    public void DisplayTypeWithMaxQuantity(SqliteConnection connection)
    {
        string query = "SELECT TypeName FROM Types INNER JOIN Goods ON Types.TypeId = Goods.TypeId GROUP BY TypeName ORDER BY SUM(Quantity) DESC LIMIT 1";
        using (SqliteCommand command = new SqliteCommand(query, connection))
        using (SqliteDataReader reader = command.ExecuteReader())
        {
            Console.WriteLine("\nІнформація про тип товару з найбільшою кількістю одиниць на складі:");
            while (reader.Read())
            {
                Console.WriteLine(reader["TypeName"]);
            }
        }
    }

    public void DisplayTypesWithMinQuantity(SqliteConnection connection)
    {
        string query = "SELECT TypeName FROM Types INNER JOIN Goods ON Types.TypeId = Goods.TypeId GROUP BY TypeName ORDER BY SUM(Quantity) ASC LIMIT 1";
        using (SqliteCommand command = new SqliteCommand(query, connection))
        using (SqliteDataReader reader = command.ExecuteReader())
        {
            Console.WriteLine("\nІнформація про тип товарів з найменшою кількістю одиниць на складі:");
            while (reader.Read())
            {
                Console.WriteLine(reader["TypeName"]);
            }
        }
    }

    public void DisplayProductsPastSupplyDate(SqliteConnection connection, int days)
    {
        DateTime targetDate = DateTime.Now.Date.AddDays(-days);
        string query = $"SELECT * FROM Goods WHERE SupplyDate < '{targetDate:yyyy-MM-dd}'";
        using (SqliteCommand command = new SqliteCommand(query, connection))
        using (SqliteDataReader reader = command.ExecuteReader())
        {
            Console.WriteLine($"\nТовари, з постачання яких минула задана кількість днів ({days}):");
            while (reader.Read())
            {
                Console.WriteLine($"{reader["Name"]}, {reader["SupplyDate"]}");
            }
        }
    }



}
