using System.Net;
using System.Net.Http;
using TMPro;
using UnityEngine;
using Newtonsoft.Json;
//send a message to a discord webhook when the game is finished
public class Timer : MonoBehaviour
{
    private static float startTime;

    private static HttpClient client; 
    
    public static HttpClient Client
    {
        get
        {
            if (client == null)
            {
                client = new HttpClient();
            }
            return client;
        }
    }
    
    
    
    void Start()
    {
        startTime = Time.time;
    }

    //send the message to the discord webhook
    public static void End()
    {
        string webhookUrl = "https://discord.com/api/webhooks/1359426393578143897/vKaro61MCTOUo8ctkept_mpAPAJ3ls8x-ZrziWtFAsof-EMIi8ORfpFFx8c0f2gRP6nq";
        var SuccessWebHook = new
        {
            username = "VR Game",
            content = "Une personne vient de finir le jeu ",
            embeds = new[]
            {
                new
                {
                    title = "Fin du jeu",
                    description = "Une personne vient de finir le jeu en " + (Time.time - startTime) + " secondes",
                    color = 0x00FF00,
                    footer = new
                    {
                        text = "VR Game"
                    }
                }
            }
        };
        var content = new StringContent(JsonConvert.SerializeObject(SuccessWebHook), System.Text.Encoding.UTF8, "application/json");
        Client.PostAsync(webhookUrl, content).Wait();
    }
}
