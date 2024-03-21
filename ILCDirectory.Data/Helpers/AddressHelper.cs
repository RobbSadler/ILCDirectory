using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILCDirectory.Data.Helpers
{
    public class AddressHelper
    {
        public static string GetAddress(string address1, string address2, string city, string state, string zip)
        {
            var address = new StringBuilder();
            if (!string.IsNullOrEmpty(address1))
            {
                address.Append(address1);
            }
            if (!string.IsNullOrEmpty(address2))
            {
                address.Append(" " + address2);
            }
            if (!string.IsNullOrEmpty(city))
            {
                address.Append(" " + city);
            }
            if (!string.IsNullOrEmpty(state))
            {
                address.Append(" " + state);
            }
            if (!string.IsNullOrEmpty(zip))
            {
                address.Append(" " + zip);
            }
            return address.ToString();
        }

        public static List<string> GetStates()
        {
            return new List<string> { "AL", "AK", "AZ", "AR", "CA", "CO", "CT", "DE", "DC", "FL", "GA", "HI", "ID", "IL", "IN", 
                "IA", "KS", "KY", "LA", "ME", "MD", "MA", "MI", "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ", "NM", "NY", 
                "NC", "ND", "OH", "OK", "OR", "PA", "RI", "SC", "SD", "TN", "TX", "UT", "VT", "VA", "WA", "WV", "WI", "WY" };
        }

        // valid canadian provinces / territories
        /*
         * Provinces
         * Alberta 	                AB 	
         * British Columbia 	    BC
         * Manitoba 	            MB
         * New Brunswick 	        NB
         * Newfoundland / Labrador 	NL
         * Nova Scotia 	            NS
         * Ontario 	                ON
         * Prince Edward Island 	PE
         * Quebec 	                QC
         * Saskatchewan 	        SK
         * 
         * Territories
         * Yukon 	                YT
         * Nunavut 	                NU
         * Northwest Territories 	NT
         */
        public static List<string> GetCanadianProvinces()
        {
            return new List<string> { "AB", "BC", "MB", "NB", "NL", "NS", "NT", "NU", "ON", "PE", "QC", "SK", "YT" };
        }

        public static bool IsCanadianProvincesOrTerritory(string abbreviation)
        {
            return GetCanadianProvinces().Contains(abbreviation);
        }


    }
}
