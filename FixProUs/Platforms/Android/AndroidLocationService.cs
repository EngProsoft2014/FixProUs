﻿using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixProUs.Platforms.Android
{
    [Service(ForegroundServiceType = ForegroundService.TypeLocation)]
    public class AndroidLocationService : Service
    {
        CancellationTokenSource _cts;
        public const int SERVICE_RUNNING_NOTIFICATION_ID = 10001;

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            _cts = new CancellationTokenSource();

            //===========================================================================================
            //Notification notification = new NotificationHelper().GetServiceStartedNotification();

            //StartForeground(SERVICE_RUNNING_NOTIFICATION_ID, notification);
            //===========================================================================================

            Task.Run(() => {
                try
                {
                    var locShared = new GetLocationService();
                    locShared.StartAsync(_cts.Token).Wait();
                }
                catch (System.OperationCanceledException)
                {
                }
                finally
                {
                    if (_cts.IsCancellationRequested)
                    {
                        var message = new StopServiceMessage();
                        Device.BeginInvokeOnMainThread(
                            () => MessagingCenter.Send(message, "ServiceStopped")
                        );
                    }
                }
            }, _cts.Token);

            return StartCommandResult.Sticky;
        }


        public override void OnDestroy()
        {
            if (_cts != null)
            {
                _cts.Token.ThrowIfCancellationRequested();
                _cts.Cancel();
            }
            base.OnDestroy();
        }

    }
}
