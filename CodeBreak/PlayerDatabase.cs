using SQLite;

namespace CodeBreak
{
    public static class PlayerDatabase
    {
        public const string connectionFile = "PlayerDB.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            SQLite.SQLiteOpenFlags.ReadWrite |
            SQLite.SQLiteOpenFlags.Create |
            SQLite.SQLiteOpenFlags.SharedCache;

        public static SQLite.SQLiteAsyncConnection connection;

        public static async Task Init()
        {
            if (connection is not null)
                return;

            string connectionPath = Path.Combine(FileSystem.AppDataDirectory, connectionFile);

            connection = new SQLiteAsyncConnection(connectionPath, Flags);

            _ = await connection.CreateTableAsync<EasyPlayers>();
            _ = await connection.CreateTableAsync<NormalPlayers>();
            _ = await connection.CreateTableAsync<HardPlayers>();
            _ = await connection.CreateTableAsync<ImpossiblePlayers>();
        }

        // Sorts all easy player records by score in descending order
        public static async Task<List<EasyPlayers>> GetEasyPlayersByScore()
        {
            await Init();
            return await connection.QueryAsync<EasyPlayers>("SELECT * FROM [EasyPlayers] ORDER BY Score DESC");
        }

        // Saves the data of an easy player to PlayerDatabase
        public static async Task<int> SaveEasyPlayerAsync(EasyPlayers player)
        {
            await Init();
            if (player.RowId != 0)
                return await connection.UpdateAsync(player);
            else
                return await connection.InsertAsync(player);
        }

        // Sorts all normal player data by score in descending order
        public static async Task<List<NormalPlayers>> GetNormalPlayersByScore()
        {
            await Init();
            return await connection.QueryAsync<NormalPlayers>("SELECT * FROM [NormalPlayers] ORDER BY Score DESC");
        }

        // Saves the data of a normal player to PlayerDatabase
        public static async Task<int> SaveNormalPlayerAsync(NormalPlayers player)
        {
            await Init();
            if (player.RowId != 0)
                return await connection.UpdateAsync(player);
            else
                return await connection.InsertAsync(player);
        }

        // Sorts all hard player data by score in descending order
        public static async Task<List<HardPlayers>> GetHardPlayersByScore()
        {
            await Init();
            return await connection.QueryAsync<HardPlayers>("SELECT * FROM [HardPlayers] ORDER BY Score DESC");
        }

        // Saves the data of a hard player to PlayerDatabase
        public static async Task<int> SaveHardPlayerAsync(HardPlayers player)
        {
            await Init();
            if (player.RowId != 0)
                return await connection.UpdateAsync(player);
            else
                return await connection.InsertAsync(player);
        }

        // Sorts all impossible player data by score in descending order
        public static async Task<List<ImpossiblePlayers>> GetImpossiblePlayersByScore()
        {
            await Init();
            return await connection.QueryAsync<ImpossiblePlayers>("SELECT * FROM [ImpossiblePlayers] ORDER BY Score DESC");
        }

        // Save the data of a impossible player to PlayerDatabase
        public static async Task<int> SaveImpossiblePlayerAsync(ImpossiblePlayers player)
        {
            await Init();
            if (player.RowId != 0)
                return await connection.UpdateAsync(player);
            else
                return await connection.InsertAsync(player);
        }

        // Deletes all player data in PlayerDatabase and resets the RowId count of each table by updating sqlite_sequence.
        public static async void DeleteAllPlayers()
        {
            await Init();
            await connection.DeleteAllAsync<EasyPlayers>();
            await connection.QueryAsync<EasyPlayers>("UPDATE sqlite_sequence SET seq = 0 WHERE 'name' = 'EasyPlayers'");
            await connection.DeleteAllAsync<NormalPlayers>();
            await connection.QueryAsync<NormalPlayers>("UPDATE sqlite_sequence SET seq = 0 WHERE 'name' = 'NormalPlayers'");
            await connection.DeleteAllAsync<HardPlayers>();
            await connection.QueryAsync<HardPlayers>("UPDATE sqlite_sequence SET seq = 0 WHERE 'name' = 'HardPlayers'");
            await connection.DeleteAllAsync<ImpossiblePlayers>();
            await connection.QueryAsync<ImpossiblePlayers>("UPDATE sqlite_sequence SET seq = 0 WHERE 'name' = 'ImpossiblePlayers'");
        }
    }

    [Table("EasyPlayers")]
    public class EasyPlayers
    {
        [PrimaryKey, AutoIncrement, Column("_rowId")]
        public int RowId { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public int Rank { get; set; }
    }

    [Table("NormalPlayers")]
    public class NormalPlayers
    {
        [PrimaryKey, AutoIncrement, Column("_rowId")]
        public int RowId { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public int Rank { get; set; }
    }

    [Table("HardPlayers")]
    public class HardPlayers
    {
        [PrimaryKey, AutoIncrement, Column("_rowId")]
        public int RowId { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public int Rank { get; set; }
    }

    [Table("ImpossiblePlayers")]
    public class ImpossiblePlayers
    {
        [PrimaryKey, AutoIncrement, Column("_rowId")]
        public int RowId { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public int Rank { get; set; }
    }
}
