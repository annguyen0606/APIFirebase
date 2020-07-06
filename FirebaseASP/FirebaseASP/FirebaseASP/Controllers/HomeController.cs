using Firebase.Database;
using Firebase.Database.Query;
using FirebaseASP.Models;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FirebaseASP.Controllers
{
    public class HomeController : Controller
    {
        String pathDuLieuDiemDanh = "Conek/DuLieuDiemDanh/";
        String pathDanhSachNhanVien = "Conek/DanhSachNhanVien/";
        String pathServer = "https://cloud-nfc-proj.firebaseio.com/";
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "",
            BasePath = "https://cloud-nfc-proj.firebaseio.com/"
        };

        IFirebaseClient client;

        class KeyIDTagData
        {
            public string keyID { get; set; }
            public string objectID { get; set; }
        }
        public async Task<ActionResult> Index()
        {
            DataTable dsNhanVien = new DataTable();
            dsNhanVien.Columns.Add("UID");
            dsNhanVien.Columns.Add("Name");
            dsNhanVien.Columns.Add("Birth");
            dsNhanVien.Columns.Add("SDT");
            dsNhanVien.Columns.Add("CMND");
            dsNhanVien.Columns.Add("MST");
            dsNhanVien.Columns.Add("BHXH");
            dsNhanVien.Columns.Add("NgayBDLV");
            dsNhanVien.Columns.Add("StatusWork");
            client = new FireSharp.FirebaseClient(config);
            if (client != null)
            {
                ArrayList danhSachNhanVien = new ArrayList();
                var firebase1 = new FirebaseClient(pathServer);
                var duLieunhanvien = await firebase1.Child(pathDanhSachNhanVien)
                                    .OrderByKey()
                                    .OnceAsync<KeyIDTagData>();
                foreach (var anc in duLieunhanvien)
                {
                    danhSachNhanVien.Add(anc.Key);
                }
                foreach (String idTag in danhSachNhanVien)
                {
                    try
                    {
                        FirebaseResponse response;
                        response = await client.GetAsync(pathDanhSachNhanVien + idTag.Trim());
                        ThongTinNhanVien data = response.ResultAs<ThongTinNhanVien>();
                        DataRow row = dsNhanVien.NewRow();
                        row["UID"] = data.UID;
                        row["Name"] = data.Name;
                        row["Birth"] = data.BirthDay;
                        row["SDT"] = data.SDT;
                        row["CMND"] = data.CMND;
                        row["MST"] = data.MST;
                        row["BHXH"] = data.BHXH;
                        row["NgayBDLV"] = data.NBDLV;
                        row["StatusWork"] = data.statusWork;
                        dsNhanVien.Rows.Add(row);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            else
            {

            }
            return View(dsNhanVien);
        }

        public ActionResult About()
        {
            AuthorANC authorANC = new AuthorANC();
            authorANC.Name = "An Nguyễn";
            authorANC.Address = "Minh Khai - Hoài Đức - Hà Tây";
            authorANC.BirthDay = "06/06/1996";
            authorANC.University = "VNU University of Engineering and Technology";
            authorANC.Phone = "0356435101";
            ViewBag.Message = "Your application description page.";

            return View(authorANC);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}