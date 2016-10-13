using Microsoft.Synchronization;
using Microsoft.Synchronization.Data;
using Microsoft.Synchronization.Data.SqlServer;
using naseNut.WebApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Timers;

namespace naseNut.WebApi.Models.Business.Services
{
    public class NaseNEntitiesSyncService
    {
        private SqlConnection _clientConn;
        private SqlConnection _serverConn;
        SyncOrchestrator _syncOrchestrator;
        DbSyncScopeDescription _sqlScopeDesc;
        DbSyncScopeDescription _dbScopeDesc;
        SqlSyncScopeProvisioning _serverProvision;
        IEnumerable<string> _tablesNames;
        BackgroundWorker _worker;
        Timer _timer;
        public static string _tableName = "";
        public NaseNEntitiesSyncService()
        {
            _clientConn = new SqlConnection(@"Data Source=.\NASE; Initial Catalog=NASE_DB_W; Integrated Security=True");
            _serverConn = new SqlConnection(@"Data Source=.\NASE; Initial Catalog=NASE_DB_W_Server; Integrated Security=True");
            _syncOrchestrator = new SyncOrchestrator();
            _tablesNames = GetTablesNames();
        }
        public void ExecuteSyncTask() {
            _worker = new BackgroundWorker();
            _worker.DoWork += worker_DoWork;
            _timer = new Timer(600000);
            _timer.Elapsed += timer_Elapsed;
            _timer.Start();
        }
        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!_worker.IsBusy)
                _worker.RunWorkerAsync();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            ExecuteSync();
        }

        public void ExecuteSync()
        {
            try
            {
                foreach (var tableName in _tablesNames)
                {
                    _tableName = tableName;
                    _syncOrchestrator.LocalProvider = new SqlSyncProvider(tableName + "Scope", _clientConn);
                    _syncOrchestrator.RemoteProvider = new SqlSyncProvider(tableName + "Scope", _serverConn);
                    _syncOrchestrator.Direction = SyncDirectionOrder.UploadAndDownload;
                    ((SqlSyncProvider)_syncOrchestrator.LocalProvider).ApplyChangeFailed += (new EventHandler<DbApplyChangeFailedEventArgs>(ApplyChangeFailed));
                    SyncOperationStatistics syncStats = _syncOrchestrator.Synchronize();
                    SaveChangeApplied(syncStats, tableName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetClientSyncConfiguration() {
            try
            {
                foreach (var tableName in _tablesNames)
                {
                    _sqlScopeDesc = SqlSyncDescriptionBuilder.GetDescriptionForScope(tableName + "Scope", _serverConn);
                    SqlSyncScopeProvisioning clientProvision = new SqlSyncScopeProvisioning(_clientConn, _sqlScopeDesc);
                    clientProvision.Apply();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SetServerSyncConfiguration() {
            try
            {
                foreach (var tableName in _tablesNames)
                {
                    _dbScopeDesc = new DbSyncScopeDescription(tableName + "Scope");
                    DbSyncTableDescription tableDesc = SqlSyncDescriptionBuilder.GetDescriptionForTable(tableName, _serverConn);
                    _dbScopeDesc.Tables.Add(tableDesc);
                    _serverProvision = new SqlSyncScopeProvisioning(_serverConn, _dbScopeDesc);
                    _serverProvision.SetCreateTableDefault(DbSyncCreationOption.Skip);
                    _serverProvision.Apply();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private IEnumerable<string> GetTablesNames() {
            var tablesNames = new List<string>();
            var properties = typeof(NaseNEntities).GetProperties();
            _clientConn.Open();
            var schema = _clientConn.GetSchema("Tables");
            foreach (DataRow row in schema.Rows)
            {
                if (row[2].ToString() == "sysdiagrams" 
                    || row[2].ToString().Contains("_tracking") 
                    || row[2].ToString() == "schema_info"
                    || row[2].ToString() == "scope_config"
                    || row[2].ToString() == "scope_info") continue;
                tablesNames.Add(row[2].ToString());
            }
            return tablesNames;
        }
        private static void ApplyChangeFailed(object sender, DbApplyChangeFailedEventArgs e)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var error = new ApplyChangeFailed
                    {
                        ConflictType = e.Conflict.Type.ToString(),
                        ErrorMessage = e.Error.ToString(),
                        TableName = _tableName
                    };
                    db.Set<ApplyChangeFailed>().Attach(error);
                    db.Set<ApplyChangeFailed>().Add(error);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static void SaveChangeApplied(SyncOperationStatistics syncStats, string tableName)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var syncInfo = new ChangeApplied
                    {
                        SyncStartTime = syncStats.SyncStartTime,
                        SyncEndTime = syncStats.SyncEndTime,
                        UploadChangesTotal = syncStats.UploadChangesTotal,
                        DownloadChangesTotal = syncStats.DownloadChangesTotal,
                        TableName = tableName
                        
                    };
                    db.Set<ChangeApplied>().Attach(syncInfo);
                    db.Set<ChangeApplied>().Add(syncInfo);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}