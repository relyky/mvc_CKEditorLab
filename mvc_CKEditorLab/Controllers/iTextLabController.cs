using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace mvc_CKEditorLab.Controllers
{
    public class iTextLabController : Controller
    {
        // GET: iTextLab
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            var doc = new Document(PageSize.A4, 25, 25, 25, 25);
            MemoryStream os = new MemoryStream();
            //FileStream os = new FileStream(@"c:\temp\test.pdf", FileMode.OpenOrCreate);
            PdfWriter pdf = PdfWriter.GetInstance(doc, os);

            // 字型設定
            string KAIU_font_path = Environment.GetEnvironmentVariable("windir") + @"\Fonts\KAIU.ttf"; // 標𪳠體
            BaseFont bfChinese = BaseFont.CreateFont(KAIU_font_path, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Font chFont = new Font(bfChinese, 12);
            Font chFont_Blue = new Font(bfChinese, 40, Font.NORMAL, new BaseColor(51, 0, 153));
            Font chFont_Msg = new Font(bfChinese, 12, Font.ITALIC, BaseColor.RED);

            // 寫入PDF
            doc.Open();
            doc.Add(new Paragraph("Hello. 哈囉。", chFont_Blue));
            doc.Close();

            // download file
            return File(os.GetBuffer(), "application/pdf", "sayhi.pdf");
        }

        public ActionResult Create2()
        {
            var doc = new Document(PageSize.A4, 25, 25, 25, 25);
            MemoryStream os = new MemoryStream();
            //FileStream os = new FileStream(@"c:\temp\test.pdf", FileMode.OpenOrCreate);
            PdfWriter pdf = PdfWriter.GetInstance(doc, os);

            // 字型設定
            string KAIU_font_path = Environment.GetEnvironmentVariable("windir") + @"\Fonts\KAIU.ttf"; // 標𪳠體
            BaseFont bfChinese = BaseFont.CreateFont(KAIU_font_path, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Font chFont = new Font(bfChinese, 12);
            Font chFont_Blue = new Font(bfChinese, 24, Font.NORMAL, new BaseColor(51, 0, 153));
            Font chFont_Msg = new Font(bfChinese, 12, Font.ITALIC, BaseColor.RED);

            // 寫入PDF
            doc.Open();
            doc.Add(new Paragraph("今天天氣很好", chFont_Blue));

            Chunk c = new Chunk("Google公司（英語：Google Inc.；中文：谷歌[3][註 1]），是一家美國的跨國科技企業，業務範圍涵蓋網際網路廣告、網際網路搜尋、雲端運算等領域，開發並提供大量基於網際網路的產品與服務[5]，其主要利潤來自於AdWords等廣告服務[6][7]。Google由在史丹佛大學攻讀理工博士的賴利·佩吉和謝爾蓋·布林共同建立，因此兩人也被稱為「Google Guys」[8][9][10]。1998年9月4日，Google以私營公司的形式創立，目的是設計並管理網際網路搜尋引擎「Google搜尋」。2004年8月19日，Google公司在納斯達克上市，後來被稱為「三駕馬車」的公司兩位共同創始人與出任執行長的埃里克·施密特在此時承諾：共同在Google工作至少二十年，即至2014年止[11]。Google的宗旨是「整合全球範圍的資訊，使人人皆可存取並從中受益」（To organize the world's information and make it universally accessible and useful）[12]；而非正式的口號則為「不作惡」（Don't be evil），由工程師阿米特·帕特爾（Amit Patel）所創[13]，並得到了保羅·布赫海特的支援[14][15]。Google公司的總部稱為「Googleplex」，位於美國加州聖塔克拉拉縣的山景城。2011年4月，佩吉接替施密特擔任執行長[16]。在2015年8月，Google進行宣布資產重組。重組後，Google劃歸新成立的Alphabet底下。同時，此舉把Google旗下的核心搜尋和廣告業務與Google無人車等新興業務分離開來[17]。", chFont);
            Phrase p1 = new Phrase(c);
            doc.Add(p1);

            Chunk c1 = new Chunk("據估計，Google在全世界的資料中心內營運著上百萬台的伺服器，", chFont);
            Chunk c2 = new Chunk("每天處理數以億計的搜尋請求和約二十四PB用戶生成的資料。", chFont_Blue);
            Chunk c3 = new Chunk("Google自創立起開始的快速成長同時也帶動了一系列的產品研發、併購事項與合作關係，而不僅僅是公司核心的網路搜尋業務。", chFont_Msg);
            Phrase p2 = new Phrase();
            p2.Add(c1);
            p2.Add(c2);
            p2.Add(c3);

            Paragraph pg = new Paragraph(p2);
            pg.Alignment = Element.ALIGN_JUSTIFIED;
            pg.FirstLineIndent = 20f;
            pg.SetLeading(0.0f, 2.0f);
            doc.Add(pg);

            // 寫入PDF-完成
            doc.Close(); 

            // download file
            return File(os.GetBuffer(), "application/pdf", "paragraph.pdf");
        }

        public ActionResult Create3()
        {
            string HTMLContent = "Hello 中文無效<b>World</b>";

            // download file
            return File(ConvertHtmlTextToPDF(HTMLContent), "application/pdf", "sayhello.pdf");
        }

        public byte[] ConvertHtmlTextToPDF(string htmlText)
        {
            if (string.IsNullOrEmpty(htmlText))
            {
                return null;
            }
            //避免當htmlText無任何html tag標籤的純文字時，轉PDF時會掛掉，所以一律加上<p>標籤
            htmlText = "<p>" + htmlText + "</p>";

            MemoryStream outputStream = new MemoryStream();//要把PDF寫到哪個串流
            byte[] data = Encoding.UTF8.GetBytes(htmlText);//字串轉成byte[]
            MemoryStream msInput = new MemoryStream(data);
            Document doc = new Document();//要寫PDF的文件，建構子沒填的話預設直式A4
            PdfWriter writer = PdfWriter.GetInstance(doc, outputStream);
            
            //指定文件預設開檔時的縮放為100%
            PdfDestination pdfDest = new PdfDestination(PdfDestination.XYZ, 0, doc.PageSize.Height, 1f);
            
            //開啟Document文件 
            doc.Open();
            
            //使用XMLWorkerHelper把Html parse到PDF檔裡
            XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msInput, null, Encoding.UTF8, new UnicodeFontFactory());
            
            //將pdfDest設定的資料寫到PDF檔
            PdfAction action = PdfAction.GotoLocalPage(1, pdfDest, writer);
            writer.SetOpenAction(action);
            doc.Close();
            msInput.Close();
            outputStream.Close();

            //回傳PDF檔案 
            return outputStream.ToArray();
        }
    }

    public class UnicodeFontFactory : FontFactoryImp
    {
        private static readonly string arialFontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
            "arialuni.ttf");//arial unicode MS是完整的unicode字型。
        private static readonly string 標楷體Path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
            "KAIU.TTF");//標楷體

        public override Font GetFont(string fontname, string encoding, bool embedded, float size, int style, BaseColor color, bool cached)
        {
            //可用Arial或標楷體，自己選一個
            BaseFont baseFont = BaseFont.CreateFont(標楷體Path, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            return new Font(baseFont, size, style, color);
        }
    }
}