using Microsoft.Extensions.Logging;
using Openddns.Core.Enum;
using Openddns.Core.Interfaces;
using Openddns.Core.Models;
using Openddns.Providers.Interfaces;
using Openddns.Providers.Models;
using Openddns.Providers.Ovh.Models;
using Ovh.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Openddns.Providers.Ovh
{
    public class OvhProvider : IProvider
    {
        private readonly ILogger<OvhProvider> _logger;

        private readonly IRepository _repository;

        public OvhProvider(ILogger<OvhProvider> logger, IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public string Name => nameof(OvhProvider);

        public async Task Setup(ProviderOptionsModel providerOptionsModel, string internetProtocolAddress, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(providerOptionsModel.Domain))
                {
                    throw new ArgumentNullException(nameof(providerOptionsModel.Domain), "Domain is null or empty");
                }

                var secret = providerOptionsModel.Secret?.Split(":");

                var client = new Client("ovh-eu", secret![0], secret[1], secret[2]);

                var domains = await client.GetAsync<List<string>>("/domain");
                if (domains.Contains(providerOptionsModel.Domain) == false)
                {
                    throw new InvalidOperationException($"Domain '{providerOptionsModel.Domain}' is not assigned to OVH account.");
                }

                var internetProtocolAddressIsPresent = await InternetProtocolAddressIsPresent(client,
                    providerOptionsModel.Domain, providerOptionsModel.SubDomain, internetProtocolAddress);

                var recordIdsToRemove = await RecordIdsToRemove(client, providerOptionsModel.Domain,
                    providerOptionsModel.SubDomain, internetProtocolAddress);

                if (internetProtocolAddressIsPresent && recordIdsToRemove.Any() == false)
                {
                    _logger.LogInformation("IP is valid.");

                    await _repository.AddLog(new LogModel(nameof(OvhProvider), "IP is valid.", LogType.Debug), cancellationToken);
                    await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

                    return;
                }

                if (internetProtocolAddressIsPresent == false)
                {
                    _logger.LogInformation($"Adding 'A' record as '{providerOptionsModel.SubDomain}.{providerOptionsModel.Domain}' target '{internetProtocolAddress}'");

                    await _repository.AddLog(new LogModel(nameof(OvhProvider), $"Adding 'A' record as '{providerOptionsModel.SubDomain}.{providerOptionsModel.Domain}' target '{internetProtocolAddress}'", LogType.Information), cancellationToken);
                    await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

                    await client.PostAsync<DnsRecordPost>($"/domain/zone/{providerOptionsModel.Domain}/record", new DnsRecordPost(internetProtocolAddress, "A", providerOptionsModel.SubDomain ?? "", 0));
                }

                if (recordIdsToRemove.Any())
                {
                    foreach (var recordId in recordIdsToRemove)
                    {
                        _logger.LogInformation($"Deleting record id {recordId} from OVH {providerOptionsModel.SubDomain}.{providerOptionsModel.Domain}");

                        await _repository.AddLog(new LogModel(nameof(OvhProvider), "Deleting record id {recordId} from OVH {providerOptionsModel.SubDomain}.{providerOptionsModel.Domain}", LogType.Information), cancellationToken);
                        await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

                        await client.DeleteAsync($"/domain/zone/{providerOptionsModel.Domain}/record/{recordId}");
                    }
                }

                await client.PostAsync($"/domain/zone/{providerOptionsModel.Domain}/refresh");
            }
            catch (Exception ex)
            {
                await _repository.AddLog(new LogModel(nameof(OvhProvider), ex.Message, LogType.Error), cancellationToken);
                await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            }
        }

        private static async Task<bool> InternetProtocolAddressIsPresent(Client client, string domain, string? subDomain, string internetProtocolAddress)
        {
            var recordIds = await client.GetAsync<List<long>>($"/domain/zone/{domain}/record?fieldType=A");
            foreach (var recordId in recordIds)
            {
                var record = await client.GetAsync<DnsRecord>($"/domain/zone/{domain}/record/{recordId}");
                if (record.Target == internetProtocolAddress && record.SubDomain == subDomain)
                {
                    return true;
                }
            }

            return false;
        }

        private static async Task<List<long>> RecordIdsToRemove(Client client, string domain, string? subDomain, string internetProtocolAddress)
        {
            var recordIdsToRemove = new List<long>();

            var recordIds = await client.GetAsync<List<long>>($"/domain/zone/{domain}/record?fieldType=A");
            foreach (var recordId in recordIds)
            {
                var record = await client.GetAsync<DnsRecord>($"/domain/zone/{domain}/record/{recordId}");
                if (record.SubDomain == subDomain && record.Target != internetProtocolAddress)
                {
                    recordIdsToRemove.Add(record.Id);
                }
            }

            return recordIdsToRemove;
        }
    }
}
