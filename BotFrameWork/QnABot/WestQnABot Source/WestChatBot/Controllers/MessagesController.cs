using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace WestChatBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                //ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                //// calculate something for us to return
                //int length = (activity.Text ?? string.Empty).Length;

                //// return our reply to the user
                //Activity reply = activity.CreateReply($"You sent {activity.Text} which was {length} characters");
                //await connector.Conversations.ReplyToActivityAsync(reply);
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));



                try
                {
                    string responseString = string.Empty;
                    //var arg = await argument;
                    var query = activity.Text;
                    var knowledgebaseId = ""; // Use knowledge base id created.
                    var qnamakerSubscriptionKey = ""; //Use subscription key assigned to you.

                    //Build the URI
                    Uri qnamakerUriBase = new Uri("https://westus.api.cognitive.microsoft.com/qnamaker/v1.0");
                    var builder = new UriBuilder($"{qnamakerUriBase}/knowledgebases/{knowledgebaseId}/generateAnswer");

                    //Add the question as part of the body
                    var postBody = $"{{\"question\": \"{query}\"}}";

                    //Send the POST request
                    using (WebClient client = new WebClient())
                    {
                        //Set the encoding to UTF8
                        client.Encoding = System.Text.Encoding.UTF8;

                        //Add the subscription key header
                        client.Headers.Add("Ocp-Apim-Subscription-Key", qnamakerSubscriptionKey);
                        client.Headers.Add("Content-Type", "application/json");
                        responseString = client.UploadString(builder.Uri, postBody);
                    }
                    QnaResult response1;
                    response1 = JsonConvert.DeserializeObject<QnaResult>(responseString);
                    if (response1.Answer.Equals("No good match found in the KB"))
                    {
                        response1.Answer = "Sorry I do not understand your question. You can reach out to our customer service representative at 425.672.8762 for further assistance. Have a good day!";
                    }
                    Activity reply = activity.CreateReply(response1.Answer);
                    await connector.Conversations.ReplyToActivityAsync(reply);
                }
                catch (Exception e)
                {
                    var ex = e.ToString();

                }
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}