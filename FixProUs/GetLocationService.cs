
using FixProUs.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace FixProUs
{
    public class GetLocationService
    {
        readonly bool stopping = false;

        static int Idincerment = 0;
        public GetLocationService()
        {
        }


        public async Task Run(CancellationToken token)
        {

            await Task.Run(async () =>
            {
                while (!stopping)
                {
                    token.ThrowIfCancellationRequested();
                    try
                    {
                        //await Task.Delay(2000);

                        //var request = new GeolocationRequest(GeolocationAccuracy.High);
                        //var location = await Geolocation.GetLocationAsync(request);
                        var location = await MainThread.InvokeOnMainThreadAsync<Location>(() =>
                        {
                            var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                            return Geolocation.GetLocationAsync(request);
                        });

                        if (location != null)
                        {
                            var message = new LocationMessage
                            {
                                Latitude = location.Latitude,
                                Longitude = location.Longitude
                            };


                            List<DataMapsModel> Listmap = new List<DataMapsModel>();
                            Idincerment += 1;

                            Listmap.Add(new DataMapsModel
                            {
                                Id = Idincerment,
                                BranchId = int.Parse(Helpers.Settings.BranchId),
                                EmployeeId = int.Parse(Helpers.Settings.UserId),
                                Lat = location.Latitude.ToString(),
                                Long = location.Longitude.ToString(),
                                Time = location.Timestamp.ToString(),
                                CreateDate = DateTime.Now.ToShortDateString(),
                                MPosition = new Location(location.Latitude, location.Longitude),
                            });

                            await Helpers.Utility.PostData("api/UploadXML/PostXmlFile", JsonConvert.SerializeObject(Listmap, Newtonsoft.Json.Formatting.None,
                                        new JsonSerializerSettings()
                                        {
                                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                        }));

                            Device.BeginInvokeOnMainThread(() =>
                            {
                                MessagingCenter.Send(message, "Location");
                            });
                        }

                    }
                    catch (Exception ex)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            var errormessage = new LocationErrorMessage();
                            MessagingCenter.Send(errormessage, "LocationError");
                        });
                    }
                }
                return;
            }, token);
        }
    }
}
