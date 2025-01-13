
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FixPro.Services.Data
{
    public class SignalRServiceChangeUserData
    {
        private readonly HubConnection _hubConnection;

        public event Action<string, string, string, string> OnMessageReceived;

        public SignalRServiceChangeUserData()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://fixproapi.engprosoft.net/ChatHub") // Update with your hub URL
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On<string, string, string, string>("ChangeUserData", (user, message, userFrom, userTo) =>
            {
                OnMessageReceived?.Invoke(user, message, userFrom, userTo);
            });
        }

        public async Task StartAsync()
        {
            try
            {
                await _hubConnection.StartAsync();
                Console.WriteLine("SignalR connected.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SignalR connection failed: {ex.Message}");
            }
        }

        public async Task Disconnect()
        {
            try
            {
                await _hubConnection.StopAsync();
                Console.WriteLine("SignalR disconnected.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SignalR disconnection failed: {ex.Message}");
            }
        }

        public async Task SendMessage(string user, string message)
        {
            try
            {
                await _hubConnection.InvokeAsync("SendMessage", user, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SendMessage failed: {ex.Message}");
            }
        }
    }
    //public class SignalRServiceChangeUserData
    //{
    //    private readonly HubConnection _hubConnection;
    //    private readonly IHubProxy _hubProxy;
    //    public event Action<string, string, string, string> OnMessageReceived;

    //    public SignalRServiceChangeUserData()
    //    {

    //        //_hubConnection = new HubConnection("https://api.fixprous.com/");
    //        _hubConnection = new HubConnection("https://fixproapi.engprosoft.net/");
    //        _hubProxy = _hubConnection.CreateHubProxy("ChatHub");

    //        _hubProxy.On<string, string, string, string>("ChangeUserData", (user, message, userFrom, userTo) =>
    //        {
    //            // Handle received message
    //            OnMessageReceived?.Invoke(user, message, userFrom, userTo);
    //        });
    //    }

    //    public async Task StartAsync()
    //    {
    //        try
    //        {
    //            await _hubConnection.Start();
    //            Console.WriteLine("SignalR connected.");
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine($"SignalR connection failed: {ex.Message}");
    //        }
    //    }

    //    public async Task Disconnect()
    //    {
    //        try
    //        {
    //            _hubConnection.Stop();
    //            Console.WriteLine("SignalR disconnected.");
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine($"SignalR disconnection failed: {ex.Message}");
    //        }
    //    }

    //    public async Task SendMessage(string user, string message)
    //    {
    //        try
    //        {
    //            await _hubProxy.Invoke("SendMessage", user, message);
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine($"SendMessage failed: {ex.Message}");
    //        }
    //    }
    //}
}
