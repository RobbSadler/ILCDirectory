﻿namespace ILCDirectory.Data
{
    public class VisitRepository : GenericRepository<Visit>, IVisitRepository
    {
        public VisitRepository(IConfiguration configuration) : base(configuration[Constants.CONFIG_CONNECTION_STRING], "Visit")
        {

        }
    }
}
