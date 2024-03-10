using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class StockProperties
    {
        public string? StockSymbole {  get; set; }
        public double CurrentPrice {  get; set; } 
        public double LowtPrice {  get; set; } 
        public double HighPrice {  get; set; } 
        public double OpenPrice {  get; set; } 

    }
}
