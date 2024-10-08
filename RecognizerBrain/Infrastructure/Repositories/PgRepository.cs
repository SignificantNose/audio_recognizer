using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Infrastructure.Repositories.Interfaces;
using Infrastructure.Settings;
using Npgsql;

namespace Infrastructure.Repositories
{
    public class PgRepository : IPgRepository
    {
        private static bool _typesReloaded = false; 
        private readonly InfrastructureOptions _settings;

        protected const int DefaultTimeoutInSeconds = 5;
        
        public PgRepository(InfrastructureOptions settings)
        {
            _settings = settings;
        }

        protected async Task<NpgsqlConnection> GetConnection()
        {
            if(
                Transaction.Current is not null &&
                Transaction.Current.TransactionInformation.Status is TransactionStatus.Aborted
            )
            {
                throw new TransactionAbortedException("Transaction was aborted (probably by user cancellation request)");
            }

            var connection = new NpgsqlConnection(_settings.PostgresConnectionString);
            await connection.OpenAsync();

            // due to in-process migrations
            if(!_typesReloaded){
                await connection.ReloadTypesAsync();
                _typesReloaded = true;
            }

            return connection;
        }

    }
}