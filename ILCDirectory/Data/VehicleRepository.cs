﻿namespace ILCDirectory.Data
{
    public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(IConfiguration configuration) : base(configuration[Constants.CONFIG_CONNECTION_STRING], "Vehicle")
        {

        }
    }
}
