using Firebase.Database;
using FirebaseASP.Models;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FirebaseASP.Controllers
{
    public class SetDataRealTimeController : Controller
    {
        // GET: SetDataRealTime
        String pathDuLieuDiemDanh = "Conek/DuLieuDiemDanh/";
        String pathDanhSachNhanVien = "Conek/DanhSachNhanVien/";
        //String pathServer = "https://annguyenhoctap.firebaseio.com/";
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "",
            BasePath = "https://annguyenhoctap.firebaseio.com/"
        };

        IFirebaseClient client;
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> SetDataFirebase(string uidTag)
        {
            string statusQuery = "";
            var cde = this.ControllerContext.RouteData.Values["Tag"];
            uidTag = cde.ToString();
            client = new FireSharp.FirebaseClient(config);
            int soGio = int.Parse(DateTime.Now.ToString("hh"));
            int soPhut = int.Parse(DateTime.Now.ToString("mm"));
            int soPhutMuon = 0;
            if(soGio >= 8 && soGio < 12)
            {
                soPhutMuon = soGio * 60 + soPhut - 8 * 60 - 30;
            }else if(soGio >= 13 && soGio < 18)
            {
                soPhutMuon = soGio * 60 + soPhut - 13 * 60 - 30;
            }
            else
            {
                soPhutMuon = 0;
            }
            if (client != null)
            {
                PushResponse response = client.Push(pathDuLieuDiemDanh + uidTag + "/"+DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("hh:mm:ss") + "," +soPhutMuon);
                if (!String.IsNullOrEmpty(response.Result.name.ToString()))
                {
                    //var firebaseClient = new FirebaseClient(pathServer);
                    //var dataStaff = await firebaseClient
                    //                .Child(pathDanhSachNhanVien + uidTag)
                    //                .OnceAsync<ThongTinNhanVien>();
                    //foreach(var i in dataStaff)
                    //{
                    //    statusQuery = i.Key;
                    //}
                    FirebaseResponse response1;
                    response1 = await client.GetAsync(pathDanhSachNhanVien + uidTag);
                    ThongTinNhanVien data = response1.ResultAs<ThongTinNhanVien>();
                    if (data.UID.Equals(uidTag.Trim()))
                    {
                        try
                        {
                            string webAddr = "https://api.conek.vn/SendSms";

                            var httpWebRequest = WebRequest.CreateHttp(webAddr);
                            httpWebRequest.ContentType = "application/json";
                            httpWebRequest.Accept = "application/json";
                            httpWebRequest.Method = "POST";

                            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                            {
                                string json = "{ \"id\" : \"ANC\", \"username\" : \"autocaretest\", \"password\" : \"autocaretest\",  \"brandname\": \"CONEK\",\"phone\": \""+data.SDT+"\",\"message\": \""+data.Name+" da diem danh luc: "+DateTime.Now.ToString("hh:mm:ss")+"\" }";

                                streamWriter.Write(json);
                                streamWriter.Flush();
                            }
                            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                            {
                                var responseText = streamReader.ReadToEnd();
                                statusQuery = responseText;
                            }
                        }
                        catch (WebException ex)
                        {
                            statusQuery = ex.Message;
                        }
                    }
                    
                }
                else
                {
                }
                //statusQuery = response.Result.name.ToString();
            }
            else
            {
                statusQuery = "Lỗi truy cập";
            }
            ViewBag.UIDTag = statusQuery;
            return View();
        }
    }
}