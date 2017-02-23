using OpenWeatherMap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace LuisBot
{
    public class Weather
    {
        string zipcodecountry = "98221,us";
        
        public double getTemperature()
        {
            string uri = "http://api.openweathermap.org/data/2.5/weather?zip=" + zipcodecountry + "&mode=xml&appid=39236d4d8a9277eb9502b1a70a3b9ca0";
            var xDoc = XDocument.Load(uri);
            double temp = (double)xDoc.Root.Element("temperature").Attribute("value");
           
            return temp;
        }

        public double getMaxTemperature()
        {
            string uri = "http://api.openweathermap.org/data/2.5/weather?zip=" + zipcodecountry + "&mode=xml&appid=39236d4d8a9277eb9502b1a70a3b9ca0";
            var xDoc = XDocument.Load(uri);
            double maxtemp = (double)xDoc.Root.Element("temperature").Attribute("max");
            return maxtemp;
        }

        public double getMinTemperature()
        {
            string uri =" http://api.openweathermap.org/data/2.5/weather?zip=" + zipcodecountry + "&mode=xml&appid=39236d4d8a9277eb9502b1a70a3b9ca0";
            var xDoc = XDocument.Load(uri);
            double mintemp = (double)xDoc.Root.Element("temperature").Attribute("min");
            return mintemp;
        }

        public double getHumidity()
        {
            string uri = "http://api.openweathermap.org/data/2.5/weather?zip=" + zipcodecountry + "&mode=xml&appid=39236d4d8a9277eb9502b1a70a3b9ca0";
            var xDoc = XDocument.Load(uri);
            double humidity = (double)xDoc.Root.Element("humidity").Attribute("value");
            return humidity;
        }

        public string getWeather()
        {
            string uri = "http://api.openweathermap.org/data/2.5/weather?zip=" + zipcodecountry + "&mode=xml&appid=39236d4d8a9277eb9502b1a70a3b9ca0";
            var xDoc = XDocument.Load(uri);
            string weather1= (string)xDoc.Root.Element("weather").Attribute("value");
            return weather1;
        }

    }

}