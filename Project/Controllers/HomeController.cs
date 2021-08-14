using Project.Data;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Project.Controllers
{
    public class HomeController : Controller
    {

        modelEntities _context = new modelEntities();
        private object repository;
        private IEnumerable<int> musteriler;

        List<Musteri> musteri = new List<Musteri>();
        List<Sepet> sepet = new List<Sepet>();
        int sepetId = 0;


        public object Musteriler { get; private set; }

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public JsonResult TestVerisiOlustur(int musteriAdet)
        {
            string[] isimler = { "Ahmet", "Emre", "Faik", "Veli", "Burak", "Emine", "Canan", "Ayşe", "Simge", "Hüseyin" };
            string[] soyisimler = { "Alagöz", "Gezer", "Seyhan", "Özhan", "Soyer", "Özbey", "Durdu", "Aydın", "Kılıç" };
            string[] sehirler = { "Ankara", "İstanbul", "İzmir", "Bursa", "Edirne", "Konya", "Antalya", "Diyarbakır", "Van", "Rize" };
            string[] sepetUrunAciklama = { "Masa", "Bilgisayar", "Telefon", "TV", "Klima", "Sandalye", "Kulaklık", "Şarj Cihazı" };

            Random rastgelesayi = new Random();  // Rastgele sayı üretmek için

            for (int i = 1; i <= musteriAdet; i++)
            {
                Musteri mstr = new Musteri
                {
                    Ad = isimler[rastgelesayi.Next(0, isimler.Length)],
                    Soyad = soyisimler[rastgelesayi.Next(0, soyisimler.Length)],
                    Sehir = sehirler[rastgelesayi.Next(0, sehirler.Length)],
                };
                _context.Musteri.Add(mstr);
                if (_context.SaveChanges() > 0)
                {
                    Sepet model = new Sepet();
                    model.MusteriId = mstr.Id;
                    _context.Sepet.Add(model);
                    if (_context.SaveChanges() > 0)
                    {
                        List<SepetUrun> sepeturun = new List<SepetUrun>();
                        for (int a = 0; a < rastgelesayi.Next(1, 5); a++)
                        {
                            SepetUrun su = new SepetUrun
                            {
                                SepetId = model.Id,
                                Aciklama = sepetUrunAciklama[rastgelesayi.Next(0, sepetUrunAciklama.Length)],
                                Tutar = rastgelesayi.Next(100, 1000)
                            };
                            _context.SepetUrun.Add(su);
                            _context.SaveChanges();
                        }
                    }
                }

                //for (int u = sepetId; u < 1; u++)
                //{
                //    List<SepetUrun> sepeturun = new List<SepetUrun>();
                //    for (int a = 0; a < rastgelesayi.Next(1, 5); a++)
                //    {
                //        SepetUrun su = new SepetUrun
                //        {

                //            SepetId = u,
                //            Aciklama = sepetUrunAciklama[rastgelesayi.Next(0, sepetUrunAciklama.Length)],
                //            Tutar = rastgelesayi.Next(100, 1000)
                //        };
                //        sepeturun.Add(su);
                //        yeniKayitSepetUrun(su);
                //    }

                //    //Sepet spt = new Sepet
                //    //{
                //    //    MusteriId = i,
                //    //    SepetUrun = sepeturun
                //    //};
                //    //sepet.Add(spt);
                //    //yeniKayitSepet(spt);
                //    //sepetId++;
                //}
                //Sepet sepet1 = new Sepet
                //{
                //    Id = i,
                //    MusteriId = i,
                //} ;

                //Musteri mstr = new Musteri
                //{
                //    Id = i,
                //    Ad = isimler[rastgelesayi.Next(0, isimler.Length)],
                //    Soyad = soyisimler[rastgelesayi.Next(0, soyisimler.Length)],
                //    Sehir = sehirler[rastgelesayi.Next(0, sehirler.Length)],
                //    sepet = sepet1
                //};
                //musteri.Add(mstr);
                //yeniKayitMusteri(mstr);
                //yeniKayitSepet(sepet1);
            }
            var json = JsonConvert.SerializeObject(musteri);
            return Json(json, JsonRequestBehavior.AllowGet);
        }




        //[HttpPost]
        public void yeniKayitMusteri(Musteri model)
        {
            Musteri m = new Musteri
            {

                Ad = model.Ad,
                Soyad = model.Soyad,
                Sehir = model.Sehir,
            };
            _context.Musteri.Add(m);
            _context.SaveChanges();

        }
        public void yeniKayitSepet(Sepet model)
        {
            Sepet spt = new Sepet
            {

                MusteriId = model.MusteriId,
            };
            _context.Sepet.Add(spt);
            _context.SaveChanges();

        }
        public void yeniKayitSepetUrun(SepetUrun model)
        {
            SepetUrun su = new SepetUrun
            {
                SepetId = model.SepetId,
                Aciklama = model.Aciklama,
                Tutar = model.Tutar
            };
            _context.SepetUrun.Add(su);



        }
        //        for(int i = 0; i<count; i++)
        //            {
        //                int no = rastgelesayi.Next(0, isimler.Length);
        //        //Dizinin kaçıncı elemanını yazdıracağımızı belirlemek için
        //        //adlar[i] = isimler[no];
        //        sayilar.Add(isimler[no]+",");
        //                //Console.WriteLine(isimler[no]);
        //            }
        //    var json = JsonConvert.SerializeObject(sayilar);
        //           // Console.ReadKey();
        //            return Json(json, JsonRequestBehavior.AllowGet);
        //}





        public JsonResult SehirBazliAnalizYap(string sehir)
        {
            var sonuc = (from m in _context.Musteri
                         join s in _context.Sepet on m.Id equals s.MusteriId
                         join su in _context.SepetUrun on s.Id equals su.SepetId
                         where m.Sehir == sehir
                         select new
                         {
                             Sehir = m.Sehir,
                             SepetId = su.Id,
                             Tutar = su.Tutar
                         }).GroupBy(g => g.Sehir).Where(w => w.Count() > 1).Select(s => new
                         {

                             SehirAdi = s.Key,
                             SepetAdet = s.Count(),
                             ToplamTutar = s.Sum(k => k.Tutar)
                         }).Distinct().OrderByDescending(o => o.SepetAdet).ToList();
            return Json(sonuc, JsonRequestBehavior.AllowGet);
        }





        //    var results = db.siparis_urunler.GroupBy(g => g.urun_id).Where(w => w.Count() > 1).Select(s => new
        //    { 
        //        s.FirstOrDefault().urunler.urun_adi, adet = s.Count() 
        //    }).Distinct().Take(5).OrderByDesceding(o => o.adet).ToList();

        //}






        //[HttpGet]
        //linq sorgusu
        public JsonResult GetMusterilerByMusteriAdi(string ad)
        {
           



            var sonuc = (from m in _context.Musteri
                         join s in _context.Sepet on m.Id equals s.MusteriId
                         join su in _context.SepetUrun on s.Id equals su.SepetId
                         where (ad == null || m.Ad == ad)
                         select new
                         {
                             Ad = m.Ad,
                             Soyad = m.Soyad,
                             Sehir = m.Sehir,
                             Urun = su.Aciklama,
                             Tutar = su.Tutar + " TL"
                         }).ToList();
            return Json(sonuc, JsonRequestBehavior.AllowGet);
        }


        // [HttpGet]
        public JsonResult GetMusterilerByMusteriId(int id)
        {

            var result = _context.Musteri.ToList();

            var sonuc = (from m in _context.Musteri
                         join s in _context.Sepet on m.Id equals s.MusteriId
                         join su in _context.SepetUrun on s.Id equals su.SepetId
                         where m.Id == id
                         select new
                         {
                             Ad = m.Ad,
                             Urun = su.Aciklama,
                             Tutar = su.Tutar + " TL"
                         }).ToList();
            return Json(sonuc, JsonRequestBehavior.AllowGet);
        }



        //[HttpPut]
        public JsonResult UrunUpdate(string ad, string fiyat)
        {
            var model = _context.SepetUrun.Where(w => w.Aciklama == ad).FirstOrDefault();
            model.Tutar = decimal.Parse(fiyat);
            _context.Entry(model).State = System.Data.Entity.EntityState.Modified;
            return Json(_context.SaveChanges(), JsonRequestBehavior.AllowGet);
        }



        public JsonResult UrunDelete(string ad, string fiyat)
        {
            var model = _context.SepetUrun.Where(w => w.Aciklama == ad).FirstOrDefault();
            model.Tutar = decimal.Parse(fiyat);
            _context.Entry(model).State = System.Data.Entity.EntityState.Modified;
            return Json(_context.SaveChanges(), JsonRequestBehavior.AllowGet);
        }

    }
}











