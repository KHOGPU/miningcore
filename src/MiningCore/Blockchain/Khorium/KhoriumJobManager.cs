using System.Threading.Tasks;
using Autofac;
using MiningCore.Blockchain.Bitcoin;
using MiningCore.Blockchain.ZCash;
using MiningCore.Blockchain.ZCash.DaemonResponses;
using MiningCore.DaemonInterface;
using MiningCore.Notifications;
using MiningCore.Time;

namespace MiningCore.Blockchain.Khorium
{
    public class KhoriumJobManager : ZCashJobManager<KhoriumJob>
    {
        public KhoriumJobManager(IComponentContext ctx,
            NotificationService notificationService,
            IMasterClock clock,
            IExtraNonceProvider extraNonceProvider) : base(ctx, notificationService, clock, extraNonceProvider)
        {
            getBlockTemplateParams = new object[]
            {
                new
                {
                    capabilities = new[] { "coinbasetxn", "workid", "coinbase/append" },
                    rules = new[] { "segwit" }
                }
            };
        }

        #region Overrides of ZCashJobManager<KhoriumJob>

        protected override async Task<DaemonResponse<ZCashBlockTemplate>> GetBlockTemplateAsync()
        {
            var result = await daemon.ExecuteCmdAnyAsync<ZCashBlockTemplate>(
                BitcoinCommands.GetBlockTemplate, getBlockTemplateParams);

            return result;
        }

        #endregion
    }
}
