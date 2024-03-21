﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ILCDirectory.Data.Helpers;
public class CountryCodeAndIso
{
    public string Country { get; set; }
    public string CountryCode { get; set; }
    public string ISO2 { get; set; }
    public string ISO3 { get; set; }
}
public static class CountryHelper
{
    public static List<CountryCodeAndIso> GetCountryCodes()
    {
        return new List<CountryCodeAndIso>
        {
            new CountryCodeAndIso { Country = "Afghanistan", CountryCode = "93", ISO2 = "AF", ISO3 = "AFG" },
            new CountryCodeAndIso { Country = "Albania", CountryCode = "355", ISO2 = "AL", ISO3 = "ALB" },
            new CountryCodeAndIso { Country = "Algeria", CountryCode = "213", ISO2 = "DZ", ISO3 = "DZA" },
            new CountryCodeAndIso { Country = "American Samoa", CountryCode = "01/01/84", ISO2 = "AS", ISO3 = "ASM" },
            new CountryCodeAndIso { Country = "Andorra", CountryCode = "376", ISO2 = "AD", ISO3 = "AND" },
            new CountryCodeAndIso { Country = "Angola", CountryCode = "244", ISO2 = "AO", ISO3 = "AGO" },
            new CountryCodeAndIso { Country = "Anguilla", CountryCode = "01/01/64", ISO2 = "AI", ISO3 = "AIA" },
            new CountryCodeAndIso { Country = "Antarctica", CountryCode = "672", ISO2 = "AQ", ISO3 = "ATA" },
            new CountryCodeAndIso { Country = "Antigua and Barbuda", CountryCode = "01/01/68", ISO2 = "AG", ISO3 = "ATG" },
            new CountryCodeAndIso { Country = "Argentina", CountryCode = "54", ISO2 = "AR", ISO3 = "ARG" },
            new CountryCodeAndIso { Country = "Armenia", CountryCode = "374", ISO2 = "AM", ISO3 = "ARM" },
            new CountryCodeAndIso { Country = "Aruba", CountryCode = "297", ISO2 = "AW", ISO3 = "ABW" },
            new CountryCodeAndIso { Country = "Australia", CountryCode = "61", ISO2 = "AU", ISO3 = "AUS" },
            new CountryCodeAndIso { Country = "Austria", CountryCode = "43", ISO2 = "AT", ISO3 = "AUT" },
            new CountryCodeAndIso { Country = "Azerbaijan", CountryCode = "994", ISO2 = "AZ", ISO3 = "AZE" },
            new CountryCodeAndIso { Country = "Bahamas", CountryCode = "01/01/42", ISO2 = "BS", ISO3 = "BHS" },
            new CountryCodeAndIso { Country = "Bahrain", CountryCode = "973", ISO2 = "BH", ISO3 = "BHR" },
            new CountryCodeAndIso { Country = "Bangladesh", CountryCode = "880", ISO2 = "BD", ISO3 = "BGD" },
            new CountryCodeAndIso { Country = "Barbados", CountryCode = "01/01/46", ISO2 = "BB", ISO3 = "BRB" },
            new CountryCodeAndIso { Country = "Belarus", CountryCode = "375", ISO2 = "BY", ISO3 = "BLR" },
            new CountryCodeAndIso { Country = "Belgium", CountryCode = "32", ISO2 = "BE", ISO3 = "BEL" },
            new CountryCodeAndIso { Country = "Belize", CountryCode = "501", ISO2 = "BZ", ISO3 = "BLZ" },
            new CountryCodeAndIso { Country = "Benin", CountryCode = "229", ISO2 = "BJ", ISO3 = "BEN" },
            new CountryCodeAndIso { Country = "Bermuda", CountryCode = "01/01/41", ISO2 = "BM", ISO3 = "BMU" },
            new CountryCodeAndIso { Country = "Bhutan", CountryCode = "975", ISO2 = "BT", ISO3 = "BTN" },
            new CountryCodeAndIso { Country = "Bolivia", CountryCode = "591", ISO2 = "BO", ISO3 = "BOL" },
            new CountryCodeAndIso { Country = "Bosnia and Herzegovina", CountryCode = "387", ISO2 = "BA", ISO3 = "BIH" },
            new CountryCodeAndIso { Country = "Botswana", CountryCode = "267", ISO2 = "BW", ISO3 = "BWA" },
            new CountryCodeAndIso { Country = "Brazil", CountryCode = "55", ISO2 = "BR", ISO3 = "BRA" },
            new CountryCodeAndIso { Country = "British Indian Ocean Territory", CountryCode = "246", ISO2 = "IO", ISO3 = "IOT" },
            new CountryCodeAndIso { Country = "British Virgin Islands", CountryCode = "01/01/84", ISO2 = "VG", ISO3 = "VGB" },
            new CountryCodeAndIso { Country = "Brunei", CountryCode = "673", ISO2 = "BN", ISO3 = "BRN" },
            new CountryCodeAndIso { Country = "Bulgaria", CountryCode = "359", ISO2 = "BG", ISO3 = "BGR" },
            new CountryCodeAndIso { Country = "Burkina Faso", CountryCode = "226", ISO2 = "BF", ISO3 = "BFA" },
            new CountryCodeAndIso { Country = "Burundi", CountryCode = "257", ISO2 = "BI", ISO3 = "BDI" },
            new CountryCodeAndIso { Country = "Cambodia", CountryCode = "855", ISO2 = "KH", ISO3 = "KHM" },
            new CountryCodeAndIso { Country = "Cameroon", CountryCode = "237", ISO2 = "CM", ISO3 = "CMR" },
            new CountryCodeAndIso { Country = "Canada", CountryCode = "1", ISO2 = "CA", ISO3 = "CAN" },
            new CountryCodeAndIso { Country = "Cape Verde", CountryCode = "238", ISO2 = "CV", ISO3 = "CPV" },
            new CountryCodeAndIso { Country = "Cayman Islands", CountryCode = "01/01/45", ISO2 = "KY", ISO3 = "CYM" },
            new CountryCodeAndIso { Country = "Central African Republic", CountryCode = "236", ISO2 = "CF", ISO3 = "CAF" },
            new CountryCodeAndIso { Country = "Chad", CountryCode = "235", ISO2 = "TD", ISO3 = "TCD" },
            new CountryCodeAndIso { Country = "Chile", CountryCode = "56", ISO2 = "CL", ISO3 = "CHL" },
            new CountryCodeAndIso { Country = "China", CountryCode = "86", ISO2 = "CN", ISO3 = "CHN" },
            new CountryCodeAndIso { Country = "Christmas Island", CountryCode = "61", ISO2 = "CX", ISO3 = "CXR" },
            new CountryCodeAndIso { Country = "Cocos Islands", CountryCode = "61", ISO2 = "CC", ISO3 = "CCK" },
            new CountryCodeAndIso { Country = "Colombia", CountryCode = "57", ISO2 = "CO", ISO3 = "COL" },
            new CountryCodeAndIso { Country = "Comoros", CountryCode = "269", ISO2 = "KM", ISO3 = "COM" },
            new CountryCodeAndIso { Country = "Cook Islands", CountryCode = "682", ISO2 = "CK", ISO3 = "COK" },
            new CountryCodeAndIso { Country = "Costa Rica", CountryCode = "506", ISO2 = "CR", ISO3 = "CRI" },
            new CountryCodeAndIso { Country = "Croatia", CountryCode = "385", ISO2 = "HR", ISO3 = "HRV" },
            new CountryCodeAndIso { Country = "Cuba", CountryCode = "53", ISO2 = "CU", ISO3 = "CUB" },
            new CountryCodeAndIso { Country = "Curacao", CountryCode = "599", ISO2 = "CW", ISO3 = "CUW" },
            new CountryCodeAndIso { Country = "Cyprus", CountryCode = "357", ISO2 = "CY", ISO3 = "CYP" },
            new CountryCodeAndIso { Country = "Czech Republic", CountryCode = "420", ISO2 = "CZ", ISO3 = "CZE" },
            new CountryCodeAndIso { Country = "Democratic Republic of the Congo", CountryCode = "243", ISO2 = "CD", ISO3 = "COD" },
            new CountryCodeAndIso { Country = "Denmark", CountryCode = "45", ISO2 = "DK", ISO3 = "DNK" },
            new CountryCodeAndIso { Country = "Djibouti", CountryCode = "253", ISO2 = "DJ", ISO3 = "DJI" },
            new CountryCodeAndIso { Country = "Dominica", CountryCode = "01/01/67", ISO2 = "DM", ISO3 = "DMA" },
            new CountryCodeAndIso { Country = "Dominican Republic", CountryCode = "1-809, 1-829, 1-849", ISO2 = "DO", ISO3 = "DOM" },
            new CountryCodeAndIso { Country = "East Timor", CountryCode = "670", ISO2 = "TL", ISO3 = "TLS" },
            new CountryCodeAndIso { Country = "Ecuador", CountryCode = "593", ISO2 = "EC", ISO3 = "ECU" },
            new CountryCodeAndIso { Country = "Egypt", CountryCode = "20", ISO2 = "EG", ISO3 = "EGY" },
            new CountryCodeAndIso { Country = "El Salvador", CountryCode = "503", ISO2 = "SV", ISO3 = "SLV" },
            new CountryCodeAndIso { Country = "Equatorial Guinea", CountryCode = "240", ISO2 = "GQ", ISO3 = "GNQ" },
            new CountryCodeAndIso { Country = "Eritrea", CountryCode = "291", ISO2 = "ER", ISO3 = "ERI" },
            new CountryCodeAndIso { Country = "Estonia", CountryCode = "372", ISO2 = "EE", ISO3 = "EST" },
            new CountryCodeAndIso { Country = "Ethiopia", CountryCode = "251", ISO2 = "ET", ISO3 = "ETH" },
            new CountryCodeAndIso { Country = "Falkland Islands", CountryCode = "500", ISO2 = "FK", ISO3 = "FLK" },
            new CountryCodeAndIso { Country = "Faroe Islands", CountryCode = "298", ISO2 = "FO", ISO3 = "FRO" },
            new CountryCodeAndIso { Country = "Fiji", CountryCode = "679", ISO2 = "FJ", ISO3 = "FJI" },
            new CountryCodeAndIso { Country = "Finland", CountryCode = "358", ISO2 = "FI", ISO3 = "FIN" },
            new CountryCodeAndIso { Country = "France", CountryCode = "33", ISO2 = "FR", ISO3 = "FRA" },
            new CountryCodeAndIso { Country = "French Polynesia", CountryCode = "689", ISO2 = "PF", ISO3 = "PYF" },
            new CountryCodeAndIso { Country = "Gabon", CountryCode = "241", ISO2 = "GA", ISO3 = "GAB" },
            new CountryCodeAndIso { Country = "Gambia", CountryCode = "220", ISO2 = "GM", ISO3 = "GMB" },
            new CountryCodeAndIso { Country = "Georgia", CountryCode = "995", ISO2 = "GE", ISO3 = "GEO" },
            new CountryCodeAndIso { Country = "Germany", CountryCode = "49", ISO2 = "DE", ISO3 = "DEU" },
            new CountryCodeAndIso { Country = "Ghana", CountryCode = "233", ISO2 = "GH", ISO3 = "GHA" },
            new CountryCodeAndIso { Country = "Gibraltar", CountryCode = "350", ISO2 = "GI", ISO3 = "GIB" },
            new CountryCodeAndIso { Country = "Greece", CountryCode = "30", ISO2 = "GR", ISO3 = "GRC" },
            new CountryCodeAndIso { Country = "Greenland", CountryCode = "299", ISO2 = "GL", ISO3 = "GRL" },
            new CountryCodeAndIso { Country = "Grenada", CountryCode = "01/01/73", ISO2 = "GD", ISO3 = "GRD" },
            new CountryCodeAndIso { Country = "Guam", CountryCode = "01/01/71", ISO2 = "GU", ISO3 = "GUM" },
            new CountryCodeAndIso { Country = "Guatemala", CountryCode = "502", ISO2 = "GT", ISO3 = "GTM" },
            new CountryCodeAndIso { Country = "Guernsey", CountryCode = "44-1481", ISO2 = "GG", ISO3 = "GGY" },
            new CountryCodeAndIso { Country = "Guinea", CountryCode = "224", ISO2 = "GN", ISO3 = "GIN" },
            new CountryCodeAndIso { Country = "Guinea-Bissau", CountryCode = "245", ISO2 = "GW", ISO3 = "GNB" },
            new CountryCodeAndIso { Country = "Guyana", CountryCode = "592", ISO2 = "GY", ISO3 = "GUY" },
            new CountryCodeAndIso { Country = "Haiti", CountryCode = "509", ISO2 = "HT", ISO3 = "HTI" },
            new CountryCodeAndIso { Country = "Honduras", CountryCode = "504", ISO2 = "HN", ISO3 = "HND" },
            new CountryCodeAndIso { Country = "Hong Kong", CountryCode = "852", ISO2 = "HK", ISO3 = "HKG" },
            new CountryCodeAndIso { Country = "Hungary", CountryCode = "36", ISO2 = "HU", ISO3 = "HUN" },
            new CountryCodeAndIso { Country = "Iceland", CountryCode = "354", ISO2 = "IS", ISO3 = "ISL" },
            new CountryCodeAndIso { Country = "India", CountryCode = "91", ISO2 = "IN", ISO3 = "IND" },
            new CountryCodeAndIso { Country = "Indonesia", CountryCode = "62", ISO2 = "ID", ISO3 = "IDN" },
            new CountryCodeAndIso { Country = "Iran", CountryCode = "98", ISO2 = "IR", ISO3 = "IRN" },
            new CountryCodeAndIso { Country = "Iraq", CountryCode = "964", ISO2 = "IQ", ISO3 = "IRQ" },
            new CountryCodeAndIso { Country = "Ireland", CountryCode = "353", ISO2 = "IE", ISO3 = "IRL" },
            new CountryCodeAndIso { Country = "Isle of Man", CountryCode = "44-1624", ISO2 = "IM", ISO3 = "IMN" },
            new CountryCodeAndIso { Country = "Israel", CountryCode = "972", ISO2 = "IL", ISO3 = "ISR" },
            new CountryCodeAndIso { Country = "Italy", CountryCode = "39", ISO2 = "IT", ISO3 = "ITA" },
            new CountryCodeAndIso { Country = "Ivory Coast", CountryCode = "225", ISO2 = "CI", ISO3 = "CIV" },
            new CountryCodeAndIso { Country = "Jamaica", CountryCode = "01/01/76", ISO2 = "JM", ISO3 = "JAM" },
            new CountryCodeAndIso { Country = "Japan", CountryCode = "81", ISO2 = "JP", ISO3 = "JPN" },
            new CountryCodeAndIso { Country = "Jersey", CountryCode = "44-1534", ISO2 = "JE", ISO3 = "JEY" },
            new CountryCodeAndIso { Country = "Jordan", CountryCode = "962", ISO2 = "JO", ISO3 = "JOR" },
            new CountryCodeAndIso { Country = "Kazakhstan", CountryCode = "7", ISO2 = "KZ", ISO3 = "KAZ" },
            new CountryCodeAndIso { Country = "Kenya", CountryCode = "254", ISO2 = "KE", ISO3 = "KEN" },
            new CountryCodeAndIso { Country = "Kiribati", CountryCode = "686", ISO2 = "KI", ISO3 = "KIR" },
            new CountryCodeAndIso { Country = "Kosovo", CountryCode = "383", ISO2 = "XK", ISO3 = "XKX" },
            new CountryCodeAndIso { Country = "Kuwait", CountryCode = "965", ISO2 = "KW", ISO3 = "KWT" },
            new CountryCodeAndIso { Country = "Kyrgyzstan", CountryCode = "996", ISO2 = "KG", ISO3 = "KGZ" },
            new CountryCodeAndIso { Country = "Laos", CountryCode = "856", ISO2 = "LA", ISO3 = "LAO" },
            new CountryCodeAndIso { Country = "Latvia", CountryCode = "371", ISO2 = "LV", ISO3 = "LVA" },
            new CountryCodeAndIso { Country = "Lebanon", CountryCode = "961", ISO2 = "LB", ISO3 = "LBN" },
            new CountryCodeAndIso { Country = "Lesotho", CountryCode = "266", ISO2 = "LS", ISO3 = "LSO" },
            new CountryCodeAndIso { Country = "Liberia", CountryCode = "231", ISO2 = "LR", ISO3 = "LBR" },
            new CountryCodeAndIso { Country = "Libya", CountryCode = "218", ISO2 = "LY", ISO3 = "LBY" },
            new CountryCodeAndIso { Country = "Liechtenstein", CountryCode = "423", ISO2 = "LI", ISO3 = "LIE" },
            new CountryCodeAndIso { Country = "Lithuania", CountryCode = "370", ISO2 = "LT", ISO3 = "LTU" },
            new CountryCodeAndIso { Country = "Luxembourg", CountryCode = "352", ISO2 = "LU", ISO3 = "LUX" },
            new CountryCodeAndIso { Country = "Macau", CountryCode = "853", ISO2 = "MO", ISO3 = "MAC" },
            new CountryCodeAndIso { Country = "Macedonia", CountryCode = "389", ISO2 = "MK", ISO3 = "MKD" },
            new CountryCodeAndIso { Country = "Madagascar", CountryCode = "261", ISO2 = "MG", ISO3 = "MDG" },
            new CountryCodeAndIso { Country = "Malawi", CountryCode = "265", ISO2 = "MW", ISO3 = "MWI" },
            new CountryCodeAndIso { Country = "Malaysia", CountryCode = "60", ISO2 = "MY", ISO3 = "MYS" },
            new CountryCodeAndIso { Country = "Maldives", CountryCode = "960", ISO2 = "MV", ISO3 = "MDV" },
            new CountryCodeAndIso { Country = "Mali", CountryCode = "223", ISO2 = "ML", ISO3 = "MLI" },
            new CountryCodeAndIso { Country = "Malta", CountryCode = "356", ISO2 = "MT", ISO3 = "MLT" },
            new CountryCodeAndIso { Country = "Marshall Islands", CountryCode = "692", ISO2 = "MH", ISO3 = "MHL" },
            new CountryCodeAndIso { Country = "Mauritania", CountryCode = "222", ISO2 = "MR", ISO3 = "MRT" },
            new CountryCodeAndIso { Country = "Mauritius", CountryCode = "230", ISO2 = "MU", ISO3 = "MUS" },
            new CountryCodeAndIso { Country = "Mayotte", CountryCode = "262", ISO2 = "YT", ISO3 = "MYT" },
            new CountryCodeAndIso { Country = "Mexico", CountryCode = "52", ISO2 = "MX", ISO3 = "MEX" },
            new CountryCodeAndIso { Country = "Micronesia", CountryCode = "691", ISO2 = "FM", ISO3 = "FSM" },
            new CountryCodeAndIso { Country = "Moldova", CountryCode = "373", ISO2 = "MD", ISO3 = "MDA" },
            new CountryCodeAndIso { Country = "Monaco", CountryCode = "377", ISO2 = "MC", ISO3 = "MCO" },
            new CountryCodeAndIso { Country = "Mongolia", CountryCode = "976", ISO2 = "MN", ISO3 = "MNG" },
            new CountryCodeAndIso { Country = "Montenegro", CountryCode = "382", ISO2 = "ME", ISO3 = "MNE" },
            new CountryCodeAndIso { Country = "Montserrat", CountryCode = "01/01/64", ISO2 = "MS", ISO3 = "MSR" },
            new CountryCodeAndIso { Country = "Morocco", CountryCode = "212", ISO2 = "MA", ISO3 = "MAR" },
            new CountryCodeAndIso { Country = "Mozambique", CountryCode = "258", ISO2 = "MZ", ISO3 = "MOZ" },
            new CountryCodeAndIso { Country = "Myanmar", CountryCode = "95", ISO2 = "MM", ISO3 = "MMR" },
            new CountryCodeAndIso { Country = "Namibia", CountryCode = "264", ISO2 = "NA", ISO3 = "NAM" },
            new CountryCodeAndIso { Country = "Nauru", CountryCode = "674", ISO2 = "NR", ISO3 = "NRU" },
            new CountryCodeAndIso { Country = "Nepal", CountryCode = "977", ISO2 = "NP", ISO3 = "NPL" },
            new CountryCodeAndIso { Country = "Netherlands", CountryCode = "31", ISO2 = "NL", ISO3 = "NLD" },
            new CountryCodeAndIso { Country = "Netherlands Antilles", CountryCode = "599", ISO2 = "AN", ISO3 = "ANT" },
            new CountryCodeAndIso { Country = "New Caledonia", CountryCode = "687", ISO2 = "NC", ISO3 = "NCL" },
            new CountryCodeAndIso { Country = "New Zealand", CountryCode = "64", ISO2 = "NZ", ISO3 = "NZL" },
            new CountryCodeAndIso { Country = "Nicaragua", CountryCode = "505", ISO2 = "NI", ISO3 = "NIC" },
            new CountryCodeAndIso { Country = "Niger", CountryCode = "227", ISO2 = "NE", ISO3 = "NER" },
            new CountryCodeAndIso { Country = "Nigeria", CountryCode = "234", ISO2 = "NG", ISO3 = "NGA" },
            new CountryCodeAndIso { Country = "Niue", CountryCode = "683", ISO2 = "NU", ISO3 = "NIU" },
            new CountryCodeAndIso { Country = "North Korea", CountryCode = "850", ISO2 = "KP", ISO3 = "PRK" },
            new CountryCodeAndIso { Country = "Northern Mariana Islands", CountryCode = "01/01/70", ISO2 = "MP", ISO3 = "MNP" },
            new CountryCodeAndIso { Country = "Norway", CountryCode = "47", ISO2 = "NO", ISO3 = "NOR" },
            new CountryCodeAndIso { Country = "Oman", CountryCode = "968", ISO2 = "OM", ISO3 = "OMN" },
            new CountryCodeAndIso { Country = "Pakistan", CountryCode = "92", ISO2 = "PK", ISO3 = "PAK" },
            new CountryCodeAndIso { Country = "Palau", CountryCode = "680", ISO2 = "PW", ISO3 = "PLW" },
            new CountryCodeAndIso { Country = "Palestine", CountryCode = "970", ISO2 = "PS", ISO3 = "PSE" },
            new CountryCodeAndIso { Country = "Panama", CountryCode = "507", ISO2 = "PA", ISO3 = "PAN" },
            new CountryCodeAndIso { Country = "Papua New Guinea", CountryCode = "675", ISO2 = "PG", ISO3 = "PNG" },
            new CountryCodeAndIso { Country = "Paraguay", CountryCode = "595", ISO2 = "PY", ISO3 = "PRY" },
            new CountryCodeAndIso { Country = "Peru", CountryCode = "51", ISO2 = "PE", ISO3 = "PER" },
            new CountryCodeAndIso { Country = "Philippines", CountryCode = "63", ISO2 = "PH", ISO3 = "PHL" },
            new CountryCodeAndIso { Country = "Pitcairn", CountryCode = "64", ISO2 = "PN", ISO3 = "PCN" },
            new CountryCodeAndIso { Country = "Poland", CountryCode = "48", ISO2 = "PL", ISO3 = "POL" },
            new CountryCodeAndIso { Country = "Portugal", CountryCode = "351", ISO2 = "PT", ISO3 = "PRT" },
            new CountryCodeAndIso { Country = "Puerto Rico", CountryCode = "1-787, 1-939", ISO2 = "PR", ISO3 = "PRI" },
            new CountryCodeAndIso { Country = "Qatar", CountryCode = "974", ISO2 = "QA", ISO3 = "QAT" },
            new CountryCodeAndIso { Country = "Republic of the Congo", CountryCode = "242", ISO2 = "CG", ISO3 = "COG" },
            new CountryCodeAndIso { Country = "Reunion", CountryCode = "262", ISO2 = "RE", ISO3 = "REU" },
            new CountryCodeAndIso { Country = "Romania", CountryCode = "40", ISO2 = "RO", ISO3 = "ROU" },
            new CountryCodeAndIso { Country = "Russia", CountryCode = "7", ISO2 = "RU", ISO3 = "RUS" },
            new CountryCodeAndIso { Country = "Rwanda", CountryCode = "250", ISO2 = "RW", ISO3 = "RWA" },
            new CountryCodeAndIso { Country = "Saint Barthelemy", CountryCode = "590", ISO2 = "BL", ISO3 = "BLM" },
            new CountryCodeAndIso { Country = "Saint Helena", CountryCode = "290", ISO2 = "SH", ISO3 = "SHN" },
            new CountryCodeAndIso { Country = "Saint Kitts and Nevis", CountryCode = "01/01/69", ISO2 = "KN", ISO3 = "KNA" },
            new CountryCodeAndIso { Country = "Saint Lucia", CountryCode = "01/01/58", ISO2 = "LC", ISO3 = "LCA" },
            new CountryCodeAndIso { Country = "Saint Martin", CountryCode = "590", ISO2 = "MF", ISO3 = "MAF" },
            new CountryCodeAndIso { Country = "Saint Pierre and Miquelon", CountryCode = "508", ISO2 = "PM", ISO3 = "SPM" },
            new CountryCodeAndIso { Country = "Saint Vincent and the Grenadines", CountryCode = "01/01/84", ISO2 = "VC", ISO3 = "VCT" },
            new CountryCodeAndIso { Country = "Samoa", CountryCode = "685", ISO2 = "WS", ISO3 = "WSM" },
            new CountryCodeAndIso { Country = "San Marino", CountryCode = "378", ISO2 = "SM", ISO3 = "SMR" },
            new CountryCodeAndIso { Country = "Sao Tome and Principe", CountryCode = "239", ISO2 = "ST", ISO3 = "STP" },
            new CountryCodeAndIso { Country = "Saudi Arabia", CountryCode = "966", ISO2 = "SA", ISO3 = "SAU" },
            new CountryCodeAndIso { Country = "Senegal", CountryCode = "221", ISO2 = "SN", ISO3 = "SEN" },
            new CountryCodeAndIso { Country = "Serbia", CountryCode = "381", ISO2 = "RS", ISO3 = "SRB" },
            new CountryCodeAndIso { Country = "Seychelles", CountryCode = "248", ISO2 = "SC", ISO3 = "SYC" },
            new CountryCodeAndIso { Country = "Sierra Leone", CountryCode = "232", ISO2 = "SL", ISO3 = "SLE" },
            new CountryCodeAndIso { Country = "Singapore", CountryCode = "65", ISO2 = "SG", ISO3 = "SGP" },
            new CountryCodeAndIso { Country = "Sint Maarten", CountryCode = "01/01/21", ISO2 = "SX", ISO3 = "SXM" },
            new CountryCodeAndIso { Country = "Slovakia", CountryCode = "421", ISO2 = "SK", ISO3 = "SVK" },
            new CountryCodeAndIso { Country = "Slovenia", CountryCode = "386", ISO2 = "SI", ISO3 = "SVN" },
            new CountryCodeAndIso { Country = "Solomon Islands", CountryCode = "677", ISO2 = "SB", ISO3 = "SLB" },
            new CountryCodeAndIso { Country = "Somalia", CountryCode = "252", ISO2 = "SO", ISO3 = "SOM" },
            new CountryCodeAndIso { Country = "South Africa", CountryCode = "27", ISO2 = "ZA", ISO3 = "ZAF" },
            new CountryCodeAndIso { Country = "South Korea", CountryCode = "82", ISO2 = "KR", ISO3 = "KOR" },
            new CountryCodeAndIso { Country = "South Sudan", CountryCode = "211", ISO2 = "SS", ISO3 = "SSD" },
            new CountryCodeAndIso { Country = "Spain", CountryCode = "34", ISO2 = "ES", ISO3 = "ESP" },
            new CountryCodeAndIso { Country = "Sri Lanka", CountryCode = "94", ISO2 = "LK", ISO3 = "LKA" },
            new CountryCodeAndIso { Country = "Sudan", CountryCode = "249", ISO2 = "SD", ISO3 = "SDN" },
            new CountryCodeAndIso { Country = "Suriname", CountryCode = "597", ISO2 = "SR", ISO3 = "SUR" },
            new CountryCodeAndIso { Country = "Svalbard and Jan Mayen", CountryCode = "47", ISO2 = "SJ", ISO3 = "SJM" },
            new CountryCodeAndIso { Country = "Swaziland", CountryCode = "268", ISO2 = "SZ", ISO3 = "SWZ" },
            new CountryCodeAndIso { Country = "Sweden", CountryCode = "46", ISO2 = "SE", ISO3 = "SWE" },
            new CountryCodeAndIso { Country = "Switzerland", CountryCode = "41", ISO2 = "CH", ISO3 = "CHE" },
            new CountryCodeAndIso { Country = "Syria", CountryCode = "963", ISO2 = "SY", ISO3 = "SYR" },
            new CountryCodeAndIso { Country = "Taiwan", CountryCode = "886", ISO2 = "TW", ISO3 = "TWN" },
            new CountryCodeAndIso { Country = "Tajikistan", CountryCode = "992", ISO2 = "TJ", ISO3 = "TJK" },
            new CountryCodeAndIso { Country = "Tanzania", CountryCode = "255", ISO2 = "TZ", ISO3 = "TZA" },
            new CountryCodeAndIso { Country = "Thailand", CountryCode = "66", ISO2 = "TH", ISO3 = "THA" },
            new CountryCodeAndIso { Country = "Togo", CountryCode = "228", ISO2 = "TG", ISO3 = "TGO" },
            new CountryCodeAndIso { Country = "Tokelau", CountryCode = "690", ISO2 = "TK", ISO3 = "TKL" },
            new CountryCodeAndIso { Country = "Tonga", CountryCode = "676", ISO2 = "TO", ISO3 = "TON" },
            new CountryCodeAndIso { Country = "Trinidad and Tobago", CountryCode = "01/01/68", ISO2 = "TT", ISO3 = "TTO" },
            new CountryCodeAndIso { Country = "Tunisia", CountryCode = "216", ISO2 = "TN", ISO3 = "TUN" },
            new CountryCodeAndIso { Country = "Turkey", CountryCode = "90", ISO2 = "TR", ISO3 = "TUR" },
            new CountryCodeAndIso { Country = "Turkmenistan", CountryCode = "993", ISO2 = "TM", ISO3 = "TKM" },
            new CountryCodeAndIso { Country = "Turks and Caicos Islands", CountryCode = "01/01/49", ISO2 = "TC", ISO3 = "TCA" },
            new CountryCodeAndIso { Country = "Tuvalu", CountryCode = "688", ISO2 = "TV", ISO3 = "TUV" },
            new CountryCodeAndIso { Country = "U.S. Virgin Islands", CountryCode = "01/01/40", ISO2 = "VI", ISO3 = "VIR" },
            new CountryCodeAndIso { Country = "Uganda", CountryCode = "256", ISO2 = "UG", ISO3 = "UGA" },
            new CountryCodeAndIso { Country = "Ukraine", CountryCode = "380", ISO2 = "UA", ISO3 = "UKR" },
            new CountryCodeAndIso { Country = "United Arab Emirates", CountryCode = "971", ISO2 = "AE", ISO3 = "ARE" },
            new CountryCodeAndIso { Country = "United Kingdom", CountryCode = "44", ISO2 = "GB", ISO3 = "GBR" },
            new CountryCodeAndIso { Country = "United States", CountryCode = "1", ISO2 = "US", ISO3 = "USA" },
            new CountryCodeAndIso { Country = "Uruguay", CountryCode = "598", ISO2 = "UY", ISO3 = "URY" },
            new CountryCodeAndIso { Country = "Uzbekistan", CountryCode = "998", ISO2 = "UZ", ISO3 = "UZB" },
            new CountryCodeAndIso { Country = "Vanuatu", CountryCode = "678", ISO2 = "VU", ISO3 = "VUT" },
            new CountryCodeAndIso { Country = "Vatican", CountryCode = "379", ISO2 = "VA", ISO3 = "VAT" },
            new CountryCodeAndIso { Country = "Venezuela", CountryCode = "58", ISO2 = "VE", ISO3 = "VEN" },
            new CountryCodeAndIso { Country = "Vietnam", CountryCode = "84", ISO2 = "VN", ISO3 = "VNM" },
            new CountryCodeAndIso { Country = "Wallis and Futuna", CountryCode = "681", ISO2 = "WF", ISO3 = "WLF" },
            new CountryCodeAndIso { Country = "Western Sahara", CountryCode = "212", ISO2 = "EH", ISO3 = "ESH" },
            new CountryCodeAndIso { Country = "Yemen", CountryCode = "967", ISO2 = "YE", ISO3 = "YEM" },
            new CountryCodeAndIso { Country = "Zambia", CountryCode = "260", ISO2 = "ZM", ISO3 = "ZMB" },
            new CountryCodeAndIso { Country = "Zimbabwe", CountryCode = "263", ISO2 = "ZW", ISO3 = "ZWE" },
        };
    }
}