using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LuisBot.Dialogs
{
    [LuisModel("", "")]
    [Serializable]
    public class REIDialog : LuisDialog<object>
    {
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            try
            {
                string message = $"Sorry, I did not understand '{result.Query}'. Type 'help' if you need assistance.";

                await context.PostAsync(message);

                context.Wait(this.MessageReceived);
            }
            catch (Exception)
            {
                await context.PostAsync("Sorry, I did not understand.Type 'help' if you need assistance.");

            }
        }

        [LuisIntent("Help")]
        public async Task Help(IDialogContext context, LuisResult result)
        {
            try
            {
                await context.PostAsync("Hi Steve. Good evening. What can I do for you?");

                context.Wait(this.MessageReceived);
            }
            catch (Exception)
            {
                await context.PostAsync("Sorry, I did not understand.Type 'help' if you need assistance.");

            }
        }

        [LuisIntent("SearchCamping")]

        public async Task SearchCamping(IDialogContext context, LuisResult result)
        {
            try
            {
                await context.PostAsync("Sure I can assist you on that. How far from home are you willing to drive?");

                context.Wait(this.MessageReceived);
            }
            catch (Exception)
            {
                await context.PostAsync("Sorry, I did not understand.Type 'help' if you need assistance.");

            }
        }

        [LuisIntent("MeasureDistance")]

        public async Task MeasureDistance(IDialogContext context, LuisResult result)
        {
            //await context.PostAsync("The list has links for more details as well as customer ratings.Which option do you like?");
            try
            {
                await context.PostAsync("What kind of activities are you interested in?");
                context.Wait(this.MessageReceived);

            }
            catch (Exception)
            {
                await context.PostAsync("Sorry, I did not understand.Type 'help' if you need assistance.");

            }
        }

        [LuisIntent("SelectResort")]

        public async Task SelectResort(IDialogContext context, LuisResult result)
        {
            try
            {
                await context.PostAsync("Great. That's one of the more popular choices. Here's the weather forecast for next weekend.");
                Weather weather = new Weather();
                double temp = (weather.getTemperature() * 9 / 5 - 459.67);
                temp = Math.Round(temp, 0);
                string temperature =temp.ToString();

                double max = (weather.getMaxTemperature() * 9 / 5 - 459.67);
                max = Math.Round(max, 0);
                string maxtemp = max.ToString();

                double min = (weather.getMinTemperature() * 9 / 5 - 459.67);
                min = Math.Round(min, 0);
                string mintemp = min.ToString();

                string humidity = weather.getHumidity().ToString();
                string weather1 = weather.getWeather();
                await context.PostAsync(string.Format("Temperature:" + temperature + "F{0}Maximum Temperature: " + maxtemp + "F{1}Minimum Temperature: " + mintemp + "F{2}Humidity: " + humidity + "%{3}Weather: " + weather1, Environment.NewLine, Environment.NewLine, Environment.NewLine, Environment.NewLine));
                await context.PostAsync("Looks like it's going to be chilly! Make sure you pack some warm clothes…");

                var replyMessage = context.MakeMessage();
                replyMessage.Text = "Here are some more details on the site.";
                HeroCard heroCard = new HeroCard()
                {

                    //  Text = string.Format("Bird Watching{0}Boating{1}Hiking/Nature Trails{2}Kayaking{3}Wildlife Viewing", Environment.NewLine, Environment.NewLine, Environment.NewLine, Environment.NewLine),
                    Buttons = new List<CardAction>()
                        {
                            new CardAction()
                            {
                                Title = "More details",
                                Type = ActionTypes.OpenUrl,
                                Value = $"https://botdemostorage.blob.core.windows.net/botcontainer/Fidalgo%20Bay%20Info.pdf"

                            }
                        }

                   
                };
                replyMessage.Attachments.Add(heroCard.ToAttachment());
                await context.PostAsync(replyMessage);

                context.Wait(this.MessageReceived);
            }
            catch (Exception)
            {
                await context.PostAsync("Sorry, I did not understand.Type 'help' if you need assistance.");

            }
        }

        [LuisIntent("FindHikes")]

        public async Task FindHikes(IDialogContext context, LuisResult result)
        {
            try
            {
                var replyMessage = context.MakeMessage();
                replyMessage.Text = "Sounds fun! Here are some good options based on distance and the activities you're looking for…";
                replyMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                replyMessage.Attachments = new List<Attachment>();

                string[] title = { "Harbour Pointe RV Park \\*\\*\\*", "Eagle Tree RV park \\*\\*", "Fidalgo Bay Resort \\*\\*\\*\\*", "Deep Lake Resort \\*\\*\\*", "Mount Vernon RV Campground \\*\\*" };
                //string[] rating = { "", "2 star", "4 star", "3 star", "2 star" };
                string[] text = { "11501 Highway 99 Everett, WA 98204 ", "16280 State Highway 305 Poulsbo, WA 98370", "4701 Fidalgo Bay Road Anacortes, WA 98221","12405 Tilley Road South Olympia, WA 98512", "5409 N. Darrk Lane Bow, WA 98232",  };
                string[] value = { "http://www.gocampingamerica.com/parks/washington/harbour-pointe-rv-park", "http://www.gocampingamerica.com/parks/washington/eagle-tree-rv-park", "http://www.gocampingamerica.com/parks/washington/fidalgo-bay-resort", "http://www.gocampingamerica.com/parks/washington/deep-lake-resort", "http://www.gocampingamerica.com/parks/washington/mount-vernon-rv-campground" };
                string[] distance = { "18.3 miles", "22.1 miles", "61.8 miles", "62.7 miles", "64.4 miles" };
                string[] image = { "https://botdemostorage.blob.core.windows.net/botcontainer/blob1_resort1.jpg", "https://botdemostorage.blob.core.windows.net/botcontainer/blob1_resort2.jpg", "https://botdemostorage.blob.core.windows.net/botcontainer/blob1_resort3.jpg", "https://botdemostorage.blob.core.windows.net/botcontainer/blob1_resort4.jpg", "https://botdemostorage.blob.core.windows.net/botcontainer/blob1_resort5.jpg" };
                for (int i = 0; i < 5; i++)
                {
                    
                    HeroCard heroCard = new HeroCard()
                    {
                        //+ String.Format("{0}Ratings: ",Environment.NewLine) + subtitle[i]
                        Title = title[i],
                        Images = new List<CardImage>()
                        {
                            new CardImage() {
                                Url = image[i]
                            }
                        },
                        //Subtitle = "Ratings: " + rating[i] ,
                        Text = string.Format("Address : " + text[i] + "  " + "{0}Distance :" + distance[i], Environment.NewLine),
                       
                        Buttons = new List<CardAction>()
                        {
                            new CardAction()
                            {
                                Title = "More details",
                                Type = ActionTypes.OpenUrl,
                                Value = value[i]

                            }
                        }
                    };
                    replyMessage.Attachments.Add(heroCard.ToAttachment());
                }


                //associate the Attachments List with the Message and send it

                await context.PostAsync(replyMessage);




                await context.PostAsync("Are you interested in any of these options or would you like to see more?");
                context.Wait(this.MessageReceived);
            }
            catch (Exception)
            {
                await context.PostAsync("Sorry, I did not understand.Type 'help' if you need assistance.");

            }
        }

        [LuisIntent("GetTent")]
        public async Task GetTent(IDialogContext context, LuisResult result)
        {
            try
            {
                await context.PostAsync("I can help you with that. How many people are there in group?");

                context.Wait(this.MessageReceived);
            }
            catch (Exception)
            {
                await context.PostAsync("Sorry, I did not understand.Type 'help' if you need assistance.");

            }

        }

        [LuisIntent("GetPeople")]
        public async Task GetPeople(IDialogContext context, LuisResult result)
        {
            try
            {
                var replyMessage = context.MakeMessage();
                replyMessage.Text = "This is our most popular tent in this capacity range -";

                List<CardImage> cardImages = new List<CardImage>();
                cardImages.Add(new CardImage(url: "https://botdemostorage.blob.core.windows.net/botcontainer/Tent.PNG"));
                HeroCard heroCard = new HeroCard()
                {
                    Images = cardImages,

                    Buttons = new List<CardAction>()
                        {
                            new CardAction()
                            {
                                Title="REI Kingdom 8 Tent",
                                Type = ActionTypes.OpenUrl,
                                Value = $"https://www.rei.com/product/894017/rei-kingdom-8-tent"

                            }
                        }
                };
                replyMessage.Attachments.Add(heroCard.ToAttachment());


                await context.PostAsync(replyMessage);

                await context.PostAsync("Would you like to go with this tent or look at more options?");

                context.Wait(this.MessageReceived);
            }
            catch (Exception)
            {
                await context.PostAsync("Sorry, I did not understand.Type 'help' if you need assistance.");

            }
        }

        [LuisIntent("Buy")]

        public async Task BuyIntent(IDialogContext context, LuisResult result)
        {
            try
            {
                await context.PostAsync("There are North Face stores nearby with this tent in-stock. Would you like to see the list of stores or would you like to have me place an order for you and have it shipped to your home?");

                context.Wait(this.MessageReceived);
            }
            catch (Exception)
            {
                await context.PostAsync("Sorry, I did not understand.Type 'help' if you need assistance.");

            }

        }

        [LuisIntent("GetStoreList")]

        public async Task GetStoreList(IDialogContext context, LuisResult result)
        {
            try
            {
                var replyMessage = context.MakeMessage();
               
                replyMessage.Text = "Here are some stores near you with this tent in stock.";

                string[] title = { "The North Face - Bellevue", "Bellevue", "Seattle flagship", "Issaquah" };
                string[] text = { "Lynnwood, WA ", " Bellevue, WA", "Seattle, WA", "Issaquah, WA" };
                string[] value = { "http://www.bing.com/mapspreview?osid=73f66f6f-ecf2-474f-ba63-d265749a781a&cp=47.747682~-122.370087&lvl=10&v=2&sV=2&form=S00027", "http://www.bing.com/mapspreview?osid=0eea7cda-8766-4bec-a32e-068647b93b3d&cp=47.631361~-122.180364&lvl=13&v=2&sV=2&form=S00027", "http://www.bing.com/mapspreview?osid=4bfa4ff9-c915-4f31-928b-e0d1cea04e22&cp=47.635937~-122.271258&lvl=12&v=2&sV=2&form=S00027", "http://www.bing.com/mapspreview?osid=f5fd6917-5e22-4d55-96a1-5b98486553a4&cp=47.61262~-122.199875&lvl=11&v=2&sV=2&form=S00027" };
                string[] distance = { "12.4 miles", "5.7 miles", "10.6 miles", "10.4 miles" };
                for (int i = 0; i < 4; i++)
                {
                    HeroCard heroCard = new HeroCard()
                    {
                        Title = title[i],
                        Text = string.Format("Address : " + text[i] + "  " + "{0}Distance :" + distance[i], Environment.NewLine),
                        Buttons = new List<CardAction>()
                        {
                            new CardAction()
                            {
                                Title = "Directions",
                                Type = ActionTypes.OpenUrl,
                                Value = value[i]

                            }
                        }



                    };
                   
                    replyMessage.Attachments.Add(heroCard.ToAttachment());


                }


                //associate the Attachments List with the Message and send it

                await context.PostAsync(replyMessage);
                //var connector = new ConnectorClient(incomingMessage.ServiceUrl);
                //var replyMessage = incomingMessage.CreateReply("Yo, I heard you.", "en");
                //await connector.Conversations.ReplyToActivityAsync(replyMessage);

                context.Wait(MessageReceived);

                await context.PostAsync("Would you like me to have the tent put on hold for you at any of these stores?");
            }
            catch (Exception)
            {
                await context.PostAsync("Sorry, I did not understand.Type 'help' if you need assistance.");

            }
        }

        [LuisIntent("PutTentHold")]

        public async Task PutTentHold(IDialogContext context, LuisResult result)
        {
            try
            {
                await context.PostAsync("Done! Your tent is on hold at the Bellevue store and available for purchase/pickup whenever you are ready.");
                await context.PostAsync("It looks like it's been a while since you last bought hiking boots.  Are you interested in buying some new ones ?");
                context.Wait(MessageReceived);
            }
            catch (Exception)
            {
                await context.PostAsync("Sorry, I did not understand.Type 'help' if you need assistance.");
            }
        }

        [LuisIntent("BuyHikingBoots")]
        public async Task BuyHikingBoots(IDialogContext context, LuisResult result)
        {
            try
            {
                var replyMessage = context.MakeMessage();
                replyMessage.Text = "Here's what you bought last time  - Asolo Power Matic 200 GV Gore-Tex Hiking Boots Size 11";

                List<CardImage> cardImages = new List<CardImage>();
                //replyMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                cardImages.Add(new CardImage(url: "https://botdemostorage.blob.core.windows.net/botcontainer/hiking%20boots.jpg"));
                HeroCard heroCard = new HeroCard()
                {
                    Images = cardImages,

                    Buttons = new List<CardAction>()
                        {
                            new CardAction()
                            {
                                Title="Asolo Power Matic 200GV Gore-Tex Hiking Boots",
                                Type = ActionTypes.OpenUrl,
                                Value = $"https://www.rei.com/product/733624/asolo-power-matic-200-gv-gore-tex-hiking-boots-mens"

                            }
                        }
                };

                //HeroCard heroCard = new HeroCard()
                //{

                //    Buttons = new List<CardAction>()
                //        {
                //            new CardAction()
                //            {
                //                Title = "https://www.rei.com/c/mens-hiking-footwear?r=category%3Afootwear%7Chiking-footwear%7Cmens-hiking-footwear%3Bsize%3A11&ir=category%3Amens-hiking-footwear&sort=rating%7Cnum-reviews",
                //                Type = ActionTypes.OpenUrl,
                //                Value = "https://www.rei.com/c/mens-hiking-footwear?r=category%3Afootwear%7Chiking-footwear%7Cmens-hiking-footwear%3Bsize%3A11&ir=category%3Amens-hiking-footwear&sort=rating%7Cnum-reviews"
                //            }
                //        }



                //};
                replyMessage.Attachments.Add(heroCard.ToAttachment());
                await context.PostAsync(replyMessage);

                context.Wait(MessageReceived);
                await context.PostAsync(" Would you like to go with these or look at more options?");
            }
            catch (Exception)
            {
                await context.PostAsync("Sorry, I did not understand.Type 'help' if you need assistance.");

            }

        }

        [LuisIntent("SelectOption")]

        public async Task SelectOption(IDialogContext context, LuisResult result)
        {
            try
            {
                var replyMessage = context.MakeMessage();
                replyMessage.Text = "Here are more options in your size --";

                HeroCard heroCard = new HeroCard()
                {

                    Buttons = new List<CardAction>()
                        {
                            new CardAction()
                            {
                                Title = "https://www.rei.com/c/mens-hiking-footwear?r=category%3Afootwear%7Chiking-footwear%7Cmens-hiking-footwear%3Bsize%3A11&ir=category%3Amens-hiking-footwear&sort=rating%7Cnum-reviews",
                                Type = ActionTypes.OpenUrl,
                                Value = "https://www.rei.com/c/mens-hiking-footwear?r=category%3Afootwear%7Chiking-footwear%7Cmens-hiking-footwear%3Bsize%3A11&ir=category%3Amens-hiking-footwear&sort=rating%7Cnum-reviews"
                            }
                        }
                };
                replyMessage.Attachments.Add(heroCard.ToAttachment());
                await context.PostAsync(replyMessage);


                await context.PostAsync(" Do you like any of these?");
                context.Wait(MessageReceived);
            }
            catch (Exception)
            {
                await context.PostAsync("Sorry, I did not understand.Type 'help' if you need assistance.");

            }
        }

        [LuisIntent("SelectDanner")]
        public async Task SelectDanner(IDialogContext context, LuisResult result)
        {
            try
            {
                await context.PostAsync("We have a deal for our members to save 20% off one item with code WINTER21 till 11/21/2016. If you like I can put these on hold for you at the Bellevue store?");
                context.Wait(MessageReceived);
            }
            catch (Exception)
            {
                await context.PostAsync("Sorry, I did not understand.Type 'help' if you need assistance.");

            }
        }
        [LuisIntent("PutHold")]
        public async Task PutHold(IDialogContext context, LuisResult result)
        {
            try
            {
                await context.PostAsync("Is there anything else I can help you with?");
                context.Wait(MessageReceived);
            }
            catch (Exception)
            {
                await context.PostAsync("Sorry, I did not understand.Type 'help' if you need assistance.");

            }
        }

        [LuisIntent("EndIntent")]
        public async Task EndIntent(IDialogContext context, LuisResult result)
        {
            try
            {
                if (result.Query == "No I am good"||result.Query=="I am good"||result.Query== "No thanks. I’m good"||result.Query=="No thanks.I’m good"||result.Query== "No thanks.I'm good"||result.Query==" No thanks. I'm good"||result.Query=="No thanks. I'm good")
                {
                    await context.PostAsync("Happy to help. Hope you have a great time camping!");
                }

                else if(result.Query =="No. We're going to hit to road."||result.Query== "No.We're going to hit the road."||result.Query== "No.We're going to hit the road."||result.Query== "No. We're going to hit the road."||result.Query== "No. We're going to hit the road"||result.Query=="hit the road")
                {
                    await context.PostAsync("Have a great time!");
                }
                context.Wait(MessageReceived);
            }
            catch (Exception)
            {
                await context.PostAsync("Sorry, I did not understand.Type 'help' if you need assistance.");

            }
        }

        [LuisIntent("GetCampingDay")]
        public async Task GetCampingDay(IDialogContext context, LuisResult result)
        {
            try
            {
                await context.PostAsync("Hi Steve. Today is your camping trip day!! Hope you are all set.");
                context.Wait(MessageReceived);
            }
            catch (Exception)
            {
                await context.PostAsync("Sorry, I did not understand.Type 'help' if you need assistance.");

            }
        }

        [LuisIntent("GetTrailPass")]
        public async Task GetTrailPass(IDialogContext context, LuisResult result)
        {
            try
            {
                await context.PostAsync("Let me check the rules at Fidalgo Bay");
                await context.PostAsync("Yes you need a pass.");
                context.Wait(MessageReceived);
            }
            catch (Exception)
            {
                await context.PostAsync("Sorry, I did not understand.Type 'help' if you need assistance.");

            }
        }

        [LuisIntent("ForgotTrailPass")]
        public async Task ForgotTrailPass(IDialogContext context, LuisResult result)
        {
            try
            {
                var replyMessage = context.MakeMessage();
                replyMessage.Text = "Here are some places along your route where you can find the pass you need";

                HeroCard heroCard = new HeroCard()
                {

                    Buttons = new List<CardAction>()
                        {
                            new CardAction()
                            {
                                Title = "https://www.bing.com/mapspreview?osid=e83c8de4-541e-4cec-9aa8-cbc1a5807436&cp=47.816042~-122.741003&lvl=9&v=2&sV=2&form=S00027",
                                Type = ActionTypes.OpenUrl,
                                Value = "https://www.bing.com/mapspreview?osid=e83c8de4-541e-4cec-9aa8-cbc1a5807436&cp=47.816042~-122.741003&lvl=9&v=2&sV=2&form=S00027"
                            }
                        }



                };
                replyMessage.Attachments.Add(heroCard.ToAttachment());
                await context.PostAsync(replyMessage);
                context.Wait(MessageReceived);
            }
            catch (Exception)
            {
                await context.PostAsync("Sorry, I did not understand.Type 'help' if you need assistance.");

            }
        }

        [LuisIntent("LifeSaver")]
        public async Task LifeSaver(IDialogContext context, LuisResult result)
        {
            try
            {
                await context.PostAsync("You're welcome. Need help with anything else?");

                context.Wait(MessageReceived);
            }
            catch (Exception)
            {
                await context.PostAsync("Sorry, I did not understand.Type 'help' if you need assistance.");

            }
        }
    }
}