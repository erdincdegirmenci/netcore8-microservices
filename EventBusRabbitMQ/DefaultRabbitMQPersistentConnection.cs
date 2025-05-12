using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EventBusRabbitMQ
{
    public class DefaultRabbitMQPersistentConnection : IRabbitMQPersistentConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private IConnection _connection;
        private readonly int _retryCount;
        private readonly ILogger<DefaultRabbitMQPersistentConnection> _logger;
        private bool _disposed;

        public DefaultRabbitMQPersistentConnection(IConnectionFactory connectionFactory, int retryCount, ILogger<DefaultRabbitMQPersistentConnection> logger)
        {
            _connectionFactory = connectionFactory;
            _retryCount = retryCount;
            _logger = logger;
        }
        public bool IsConnected
        {
            get
            {
                return _connection != null && _connection.IsOpen && !_disposed;
            }
        }

        public bool TryConnect()
        {

            _logger.LogInformation("RabbitMQ Client is trying to connect");
            var policy = RetryPolicy.Handle<SocketException>()
                .Or<BrokerUnreachableException>()
                .WaitAndRetry(_retryCount, retryAttemp => TimeSpan.FromSeconds(Math.Pow(2, retryAttemp)), (ex, time) =>
                {
                    _logger.LogWarning(ex, "RabbitMQ Client could not connect after {TimeOut}s ({ExceptionMessage})", $"{time.TotalSeconds:n1}", ex.Message);
                });

            policy.Execute(() =>
            {
                _connection = _connectionFactory.CreateConnection();
            });

            if(IsConnected)
            {
                _connection.ConnectionShutdown += OnConnectionShutDown;
                _connection.CallbackException += OnCallbackException;
                _connection.ConnectionBlocked += OnConnectionBlocked;

                _logger.LogInformation("RabbitMQ Client acquired a persistent connection to '{HostName}' and is subscribed to failure events", _connection);

                return true;
            }
            else
            {
                _logger.LogCritical("FATAL ERROR: RabbitMQ connections could not be created and opened");
                return false;
            }
        }

        private void OnConnectionBlocked(object? sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed) return;
            _logger.LogWarning("A RabbitMQ connection is shutdown. Try to re-connect...");
            TryConnect();
        }

        private void OnCallbackException(object? sender, CallbackExceptionEventArgs e)
        {
            if (_disposed) return;
            _logger.LogWarning("A RabbitMQ connection throw exception. Try to re-connect...");
            TryConnect();
        }

        private void OnConnectionShutDown(object? sender, ShutdownEventArgs e)
        {
            if (_disposed) return;
            _logger.LogWarning("A RabbitMQ connection is on shutdown. Try to re-connect...");
            TryConnect();
        }

        public IModel CreateModel()
        {
            if(!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are variabiles to perform this action");
            }
            return _connection.CreateModel();
        }
        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;

            try
            {
                _connection.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.ToString());
            }
        }
    }
}
