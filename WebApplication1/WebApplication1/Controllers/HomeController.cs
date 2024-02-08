using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
//DocumentFormat.OpenXml.Wordprocessing

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public object BorderValues { get; private set; }

        public ActionResult Index(string pattern)
        {
            List<SearchResultLine> result;
            using (db_Voynova_6Entities db = new db_Voynova_6Entities())
            {
                result = db.Alp
                    .Join(
                        db.AlpsMountains,      // связываем таблицу Альпинисты с АльпинистыГоры
                        a => a.Id,             // берем Id альпиниста
                        am => am.Alp_Id,    // соединяем с Alp_Id в АльпинистыГоры
                        (a, am) => new         // проекция результата
                        {
                            AlpName = a.Name,
                            id_a_m = am.Id,      // Id связи
                            id_m = am.Mountain_Id    // Id горы
                        }
                    )
                    .Join(
                        db.Mountain,              // затем соединяем с таблицей горы
                        am => am.id_m,         // берем id горы из предыдущего соединения
                        m => m.Id,             // и соединяем с Id в таблице горы
                        (am, m) => new SearchResultLine() // результат в новую структуру данных
                        {
                            AlpName = am.AlpName,
                            MountainName = m.Name    // предполагаем, что в таблице горы есть поле Name
                        }
                    ).ToList();
            }
            if (pattern == null)
            {
                ViewBag.SearchData = result;
                return View();
            }
            else
            {
                result = result.Where((p) => p.AlpName.Contains(pattern) || p.MountainName.Contains(pattern)).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }


        public FileStreamResult GetWord()
        {
            /*
            string[,] data = new string[3, 5];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 5; j++)
                    data[i, j] = (i + j).ToString();
            MemoryStream memoryStream = GenerateWord(data);
            return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
            {
                FileDownloadName = "demo.docx"
            };
            */
            using (db_Voynova_6Entities db = new db_Voynova_6Entities())
            {
                IQueryable<WebApplication1.Models.AlpsMountains> bc = db.AlpsMountains;
                string[,] data = new string[bc.Count(), 3];
                int i = 0;
                foreach (var b in bc)
                {
                    data[i, 0] = b.Id.ToString();
                    data[i, 1] = b.Alp.Name;
                    data[i, 2] = b.Mountain.Name;
                    i++;
                }
                MemoryStream memoryStream = GenerateWord(data);
                memoryStream.Position = 0;
                return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
                {
                    FileDownloadName = "mountain.docx"
                };
            }
        }



        private MemoryStream GenerateWord(string[,] data)
        {
            MemoryStream mStream = new MemoryStream();
            // Создаем документ
            using (WordprocessingDocument document = WordprocessingDocument.Create(mStream, WordprocessingDocumentType.Document, true))
            {
                // Добавляется главная часть документа. 
                MainDocumentPart mainPart = document.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());
                // Создаем таблицу. 
                Table table = new Table();
                body.AppendChild(table);

                // Устанавливаем свойства таблицы(границы и размер).
                TableProperties props = new TableProperties(
                    new TableBorders(
                    new TopBorder
                    {
                        Val = new EnumValue<BorderValues>(DocumentFormat.OpenXml.Wordprocessing.BorderValues.Single),
                        Size = 12
                    },
                    new BottomBorder
                    {
                        Val = new EnumValue<BorderValues>(DocumentFormat.OpenXml.Wordprocessing.BorderValues.Single),
                        Size = 12
                    },
                    new LeftBorder
                    {
                        Val = new EnumValue<BorderValues>(DocumentFormat.OpenXml.Wordprocessing.BorderValues.Single),
                        Size = 12
                    },
                    new RightBorder
                    {
                        Val = new EnumValue<BorderValues>(DocumentFormat.OpenXml.Wordprocessing.BorderValues.Single),
                        Size = 12
                    },
                    new InsideHorizontalBorder
                    {
                        Val = new EnumValue<BorderValues>(DocumentFormat.OpenXml.Wordprocessing.BorderValues.Single),
                        Size = 12
                    },
                    new InsideVerticalBorder
                    {
                        Val = new EnumValue<BorderValues>(DocumentFormat.OpenXml.Wordprocessing.BorderValues.Single),
                        Size = 12
                    }));
                BorderValues f = DocumentFormat.OpenXml.Wordprocessing.BorderValues.Single;

                // Назначаем свойства props объекту table
                table.AppendChild<TableProperties>(props);

                // Заполняем ячейки таблицы.
                for (var i = 0; i <= data.GetUpperBound(0); i++)
                {
                    var tr = new TableRow();
                    for (var j = 0; j <= data.GetUpperBound(1); j++)
                    {
                        var tc = new TableCell();
                        tc.Append(new Paragraph(new Run(new Text(data[i, j]))));

                        // размер колонок определяется автоматически.
                        tc.Append(new TableCellProperties(
                            new TableCellWidth { Type = TableWidthUnitValues.Auto }));

                        tr.Append(tc);
                    }
                    table.Append(tr);
                }

                mainPart.Document.Save();
                //document.Close();
                //mStream.Position = 0;
                return mStream;
            }
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public FileStreamResult GetWord1()
        {
            // Create Stream
            MemoryStream mem = new MemoryStream();
            // Create Document
            using (WordprocessingDocument wordDocument =
                 WordprocessingDocument.Create(mem, WordprocessingDocumentType.Document, true))
            {
                // Add a main document part. 
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                // Create the document structure and add some text.
                mainPart.Document = new Document();
                Body docBody = new Body();
                mainPart.Document.AppendChild(docBody);
                // Add your docx content here
            }
            return new FileStreamResult(mem, "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
            {
                FileDownloadName = "demo.doc"
            };
        }

        public FileStreamResult GetWord2()
        {
            // Create Stream
            MemoryStream mem = new MemoryStream();
            // Create Document
            using (WordprocessingDocument wordDocument =
                WordprocessingDocument.Create(mem, WordprocessingDocumentType.Document, true))
            {
                // Add a main document part. 
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                // Create the document structure and add some text.
                mainPart.Document = new Document();
                Body docBody = new Body();
                mainPart.Document.AppendChild(docBody);
                // Add your docx content here
                Paragraph p = new Paragraph();
                Run r = new Run();
                Text t = new Text("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent quam augue, tempus id metus in, laoreet viverra quam. Sed vulputate risus lacus, et dapibus orci porttitor non.");
                r.Append(t);
                p.Append(r);
                docBody.Append(p);
                mainPart.Document.Save();
            }
            mem.Position = 0;
            return new FileStreamResult(mem, "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
            {
                FileDownloadName = "demo.doc"
            };
        }

        // Create a Paragraph with styles
        public FileStreamResult GetWord3()
        {
            // Create Stream
            MemoryStream mem = new MemoryStream();
            // Create Document
            using (WordprocessingDocument wordDocument =
                WordprocessingDocument.Create(mem, WordprocessingDocumentType.Document, true))
            {
                // Add a main document part. 
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                // Create the document structure and add some text.
                mainPart.Document = new Document();
                Body docBody = new Body();
                mainPart.Document.AppendChild(docBody);

                // Add your docx content here
                Paragraph p = new Paragraph();
                // Run 1
                Run r1 = new Run();
                Text t1 = new Text("Pellentesque ") { Space = SpaceProcessingModeValues.Preserve };
                // The Space attribute preserve white space before and after your text
                r1.Append(t1);
                p.Append(r1);

                // Run 2 - Bold
                Run r2 = new Run();
                RunProperties rp2 = new RunProperties();
                rp2.Bold = new Bold();
                // Always add properties first
                r2.Append(rp2);
                Text t2 = new Text("commodo ") { Space = SpaceProcessingModeValues.Preserve };
                r2.Append(t2);
                p.Append(r2);

                // Run 3
                Run r3 = new Run();
                Text t3 = new Text("rhoncus ") { Space = SpaceProcessingModeValues.Preserve };
                r3.Append(t3);
                p.Append(r3);

                // Run 4 – Italic
                Run r4 = new Run();
                RunProperties rp4 = new RunProperties();
                rp4.Italic = new Italic();
                // Always add properties first
                r4.Append(rp4);
                Text t4 = new Text("mauris") { Space = SpaceProcessingModeValues.Preserve };
                r4.Append(t4);
                p.Append(r4);

                // Run 5
                Run r5 = new Run();
                Text t5 = new Text(", sit ") { Space = SpaceProcessingModeValues.Preserve };
                r5.Append(t5);
                p.Append(r5);

                // Run 6 – Italic , bold and underlined
                Run r6 = new Run();
                RunProperties rp6 = new RunProperties();
                rp6.Italic = new Italic();
                rp6.Bold = new Bold();
                rp6.Underline = new Underline();
                // Always add properties first
                r6.Append(rp6);
                Text t6 = new Text("amet ") { Space = SpaceProcessingModeValues.Preserve };
                r6.Append(t6);
                p.Append(r6);

                // Run 7
                Run r7 = new Run();
                Text t7 = new Text("faucibus arcu ") { Space = SpaceProcessingModeValues.Preserve };
                r7.Append(t7);
                p.Append(r7);

                // Run 8 – Red color
                Run r8 = new Run();
                RunProperties rp8 = new RunProperties();
                rp8.Color = new Color() { Val = "FF0000" };
                // Always add properties first
                r8.Append(rp8);
                Text t8 = new Text("porttitor ") { Space = SpaceProcessingModeValues.Preserve };
                r8.Append(t8);
                p.Append(r8);

                // Run 9
                Run r9 = new Run();
                Text t9 = new Text("pharetra. Maecenas quis erat quis eros iaculis placerat ut at mauris.") { Space = SpaceProcessingModeValues.Preserve };
                r9.Append(t9);
                p.Append(r9);
                // Add your paragraph to docx body
                docBody.Append(p);

                mainPart.Document.Save();
            }
            mem.Position = 0;
            return new FileStreamResult(mem, "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
            {
                FileDownloadName = "demo.doc"
            };
        }

        public FileStreamResult GetWord4()
        {
            // Create Stream
            MemoryStream mem = new MemoryStream();
            // Create Document
            using (WordprocessingDocument wordDocument =
                WordprocessingDocument.Create(mem, WordprocessingDocumentType.Document, true))
            {
                // Add a main document part. 
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                // Create the document structure and add some text.
                mainPart.Document = new Document();
                Body docBody = new Body();
                mainPart.Document.AppendChild(docBody);

                // Add your docx content here
                Paragraph p = new Paragraph();
                ParagraphProperties pp = new ParagraphProperties();
                pp.Justification = new Justification() { Val = JustificationValues.Center };
                // Add paragraph properties to your paragraph
                p.Append(pp);
                // Run
                Run r = new Run();
                Text t = new Text("Nam eu tortor ut mi euismod eleifend in ut ante. Donec a ligula ante. Sed rutrum ex quam. Nunc id mi ultricies, vestibulum sapien vel, posuere dui.") { Space = SpaceProcessingModeValues.Preserve };
                r.Append(t);
                p.Append(r);
                // Add your paragraph to docx body
                docBody.Append(p);

                mainPart.Document.Save();
            }
            mem.Position = 0;
            return new FileStreamResult(mem, "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
            {
                FileDownloadName = "demo.doc"
            };
        }

        // Create Table
        public FileStreamResult GetWord5()
        {
            // Create Stream
            MemoryStream mem = new MemoryStream();
            // Create Document
            using (WordprocessingDocument wordDocument =
                WordprocessingDocument.Create(mem, WordprocessingDocumentType.Document, true))
            {
                // Add a main document part. 
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                // Create the document structure and add some text.
                mainPart.Document = new Document();
                Body docBody = new Body();
                mainPart.Document.AppendChild(docBody);

                // Add your docx content here
                Table table = new Table();

                /* ROW #1 */
                TableRow tr1 = new TableRow();

                TableCell tc11 = new TableCell();
                Paragraph p11 = new Paragraph(new Run(new Text("A")));
                tc11.Append(p11);
                tr1.Append(tc11);

                TableCell tc12 = new TableCell();
                Paragraph p12 = new Paragraph();
                Run r12 = new Run();
                RunProperties rp12 = new RunProperties();
                rp12.Bold = new Bold();
                r12.Append(rp12);
                r12.Append(new Text("Nice"));
                p12.Append(r12);
                tc12.Append(p12);

                tr1.Append(tc12);
                table.Append(tr1);


                /* ROW #2 */
                TableRow tr2 = new TableRow();

                TableCell tc21 = new TableCell();
                Paragraph p21 = new Paragraph(new Run(new Text("Little")));
                tc21.Append(p21);
                tr2.Append(tc21);

                TableCell tc22 = new TableCell();
                Paragraph p22 = new Paragraph();
                ParagraphProperties pp22 = new ParagraphProperties();
                pp22.Justification = new Justification() { Val = JustificationValues.Center };
                p22.Append(pp22);
                p22.Append(new Run(new Text("Table")));
                tc22.Append(p22);
                tr2.Append(tc22);

                table.Append(tr2);


                // Add your table to docx body
                docBody.Append(table);

                mainPart.Document.Save();
            }
            mem.Position = 0;
            return new FileStreamResult(mem, "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
            {
                FileDownloadName = "demo.doc"
            };
        }
    }
}