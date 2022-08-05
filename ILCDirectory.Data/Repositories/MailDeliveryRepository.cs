﻿namespace ILCDirectory.Data
{
    public class MailDeliveryRepository : GenericRepository<MailDelivery>, IMailDeliveryRepository
    {
        public MailDeliveryRepository(IConfiguration configuration) : base(configuration[Constants.CONFIG_CONNECTION_STRING], "MailDelivery")
        {

        }
    }
}