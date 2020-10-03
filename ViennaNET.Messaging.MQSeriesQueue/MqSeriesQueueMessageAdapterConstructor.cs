﻿using Microsoft.Extensions.Configuration;
using ViennaNET.Messaging.Configuration;
using ViennaNET.Messaging.Factories.Impl;
using ViennaNET.Messaging.MQSeriesQueue.Infrastructure;
using ViennaNET.Utils;

namespace ViennaNET.Messaging.MQSeriesQueue
{
  /// <inheritdoc />
  public class MqSeriesQueueMessageAdapterConstructor : QueueMessageAdapterConstructorBase<MqSeriesConfiguration,
    MqSeriesQueueConfiguration>
  {
    /// <inheritdoc />
    public MqSeriesQueueMessageAdapterConstructor(IConfiguration configuration) : base(configuration, "mqseries")
    {
    }

    /// <inheritdoc />
    protected override IMessageAdapter CreateAdapter(MqSeriesQueueConfiguration queueConfiguration, bool isDiagnostic)
    {
      queueConfiguration.ThrowIfNull(nameof(queueConfiguration));

      var connectionFactoryProvider = new MqSeriesQueueConnectionFactoryProvider();
      return queueConfiguration.TransactionEnabled || queueConfiguration.ProcessingType == MessageProcessingType.ThreadStrategy
        ? new MqSeriesQueueTransactedMessageAdapter(connectionFactoryProvider, queueConfiguration)
        : (IMessageAdapter)new MqSeriesQueueSubscribingMessageAdapter(connectionFactoryProvider, queueConfiguration);
    }

    /// <inheritdoc />
    protected override void CheckConfigurationParameters(MqSeriesQueueConfiguration configuration)
    {
      configuration.Id.ThrowIfNullOrWhiteSpace(nameof(configuration.Id));
      configuration.ClientId.ThrowIfNullOrWhiteSpace(nameof(configuration.ClientId));
      configuration.Server.ThrowIfNullOrWhiteSpace(nameof(configuration.Server));
      configuration.QueueName.ThrowIfNullOrWhiteSpace(nameof(configuration.QueueName));
      configuration.QueueManager.ThrowIfNullOrWhiteSpace(nameof(configuration.QueueManager));
      if (configuration.UseQueueString)
      {
        configuration.QueueString.ThrowIfNullOrWhiteSpace(nameof(configuration.QueueString));
      }
    }
  }
}