using ExpertSystem.Base;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpertSystem.Services
{
    class DbHandler : SingletonBase<DbHandler>
    {
        private string _pathToDb;

        private SQLiteConnection _connection;

        private DbHandler() { }

        /// <summary>
        /// Returns the singleton instance.
        /// </summary>
        public static DbHandler Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DbHandler();
                }
                return _instance;
            }
        }

        protected override void Init(object[] args)
        {
            if (!(args[0] is string))
                throw new InvalidDataException("First argument is not a string.");

            if (!File.Exists(args[0] as string))
                throw new FileNotFoundException();

            _pathToDb = args[0] as string;

            SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder()
            {
                DataSource = _pathToDb
            };

            _connection = new SQLiteConnection(builder.ConnectionString).OpenAndReturn();
        }

        public override void ResetState()
        {
            base.ResetState();
            _pathToDb = "";

            if (_connection?.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
                _connection = null;
            }
        }

        public void Close()
        {
            ResetState();
        }

        public SQLiteDataReader ExecuteQuery(string query)
        {
            SQLiteCommand command = new SQLiteCommand(query, _connection);

            try
            {
                command.VerifyOnly();
            }
            catch (SQLiteException e)
            {
                throw new SQLiteException("Query cannot be succesfully compiled.", e);
            }

            return command.ExecuteReader();
        }
    }
}
