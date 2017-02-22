using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuisBot
{
    [Serializable]
    public class REIQuery
    {
        [Prompt("Do you like any of these ?")]

        public string Item { get; set; }

        [Prompt("We have a deal for our members to save 20 % off one item with code WINTER21 till 11 / 21 / 2016.If you like I can put these on hold for you at the Bellevue store ? ")]

        public string Deal { get; set; }

        [Prompt("Is there anything else I can help you with?")]

        public string HelpNeeded { get; set; }

        [Prompt("Happy to help. Hope you have a great time camping!")]

        public string EndConv { get; set; }
    }
}