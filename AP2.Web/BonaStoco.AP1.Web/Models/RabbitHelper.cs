using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Spring.Messaging.Amqp.Rabbit.Core;
using Spring.Context.Support;
using Spring.Messaging.Amqp.Support.Converter;
using BonaStoco.AP1.Web.Messages;
namespace BonaStoco.AP1.Web.Models
{
    public class RabbitHelper
    {
        const string MASTER_DATA_EXCHANGE = "masterData.topic";
        const string PENGIRIMAN_BARANG_EXCHANGE = "pengirimanBarang.topic";
        const string TYPE_MAPPER_ASSEMBLY = "BonaStoco.AP1.Web.Messages";
        const string TYPE_MAPPER_NAMESPACE = "BonaStoco.AP1.Web.Messages";
        const string POS_TRANSACTION_EXCHANGE = "posTransaction.topic";
        const string STOCKOPNAME_EXCHANGE ="stockOpname.topic";
        const string AP1_ROUTING_KEY = "ap1.queue";
        const string EXCHANGE_RATE = "exchangeRate.topic";
        const string EXCHANGE_RATE_ROUTING_KEY = "alltenant";

        public RabbitHelper()
        {

        }
        public void SendMasterDataExchange<T>(T msg) where T : ITenanIdentity
        {
            Send<T>(msg, MASTER_DATA_EXCHANGE, Route(msg.TenanId));
        }

        
        public void SendStockOpnameExchange<T>(T msg) where T : ITenanIdentity
        {
            Send<T>(msg, STOCKOPNAME_EXCHANGE, AP1_ROUTING_KEY);
        }

        public void SendRegisterMasterData(RegisterProductMessage msg)
        {
            Send<RegisterProductMessage>(msg, MASTER_DATA_EXCHANGE, AP1_ROUTING_KEY);
        }

        public void SendApproveMasterData(ApproveProductMessage msg)
        {
            Send<ApproveProductMessage>(msg, MASTER_DATA_EXCHANGE, AP1_ROUTING_KEY);
        }

        public void SendSyncronizationPart<T>(T msg) where T: ITenanIdentity
        {
            Send<T>(msg, MASTER_DATA_EXCHANGE, AP1_ROUTING_KEY);
        }
        
        public void SendRejectMasterData(RejectProductMessage msg)
        {
            Send<RejectProductMessage>(msg, MASTER_DATA_EXCHANGE, AP1_ROUTING_KEY);
        }

        public void SendUpdteExchangeRate(ExchangeRateMessage msg)
        {
            Send<ExchangeRateMessage>(msg, EXCHANGE_RATE, EXCHANGE_RATE_ROUTING_KEY);
        }

        public void SendPengirimanBarangExchange<T>(T msg) where T : ITenanIdentity
        {
            Send<T>(msg, PENGIRIMAN_BARANG_EXCHANGE, AP1_ROUTING_KEY);
        }

        public void SendVerifiedGRNMessage(VerifiedGRNMessage msg)
        {
            RabbitTemplate rabbitTemplate = (RabbitTemplate)ContextRegistry.GetContext().GetObject("RabbitTemplate");
            Send<VerifiedGRNMessage>(msg, PENGIRIMAN_BARANG_EXCHANGE, Route(msg.TenanId));
        }

        public void SendTenanCreatedMessage(TenanCreatedMessage msg)
        {
            Send<TenanCreatedMessage>(msg, MASTER_DATA_EXCHANGE, AP1_ROUTING_KEY);
        }

        public void SendTenanEditedMessage( TenanEditedMessage msg)
        {
            Send<TenanEditedMessage>(msg, MASTER_DATA_EXCHANGE, AP1_ROUTING_KEY);
        }

        public void SendPOSTransactionMessage(ShoppingCartCheckedOut msg)
        {
            Send<ShoppingCartCheckedOut>(msg, POS_TRANSACTION_EXCHANGE, AP1_ROUTING_KEY);
        }

        public void SendUploadSalesMessage(UploadSalesMessage msg)
        {
            Send<UploadSalesMessage>(msg, POS_TRANSACTION_EXCHANGE, AP1_ROUTING_KEY);
        }
       
        private void Send<T>(T msg, string exchange, string routingKey) where T : ITenanIdentity
        {
            JsonMessageConverter msgConverter = new JsonMessageConverter();
            msgConverter.TypeMapper = CreateTypeMapper();

            RabbitTemplate rabbitTemplate = (RabbitTemplate)ContextRegistry.GetContext().GetObject("RabbitTemplate");
            rabbitTemplate.MessageConverter = msgConverter;

            rabbitTemplate.ConvertAndSend(exchange, routingKey, msg, m =>
                {
                    m.MessageProperties.DeliveryMode = Spring.Messaging.Amqp.Core.MessageDeliveryMode.PERSISTENT;
                    return m; 
                });
        }

        private TypeMapper CreateTypeMapper()
        {
            TypeMapper tmap = new TypeMapper();
            tmap.DefaultAssemblyName = TYPE_MAPPER_ASSEMBLY;
            tmap.DefaultNamespace = TYPE_MAPPER_NAMESPACE;

            return tmap;
        }

        private string Route(int tenanId)
        {
            return string.Format("ap1.{0}", tenanId.ToString());
        }

        internal void SendImportProductMessageToTenant(int tenantId, TambahProductMessage msg)
        {
            Send<TambahProductMessage>(msg, MASTER_DATA_EXCHANGE, "ap1." + tenantId.ToString());
        }
    }
}