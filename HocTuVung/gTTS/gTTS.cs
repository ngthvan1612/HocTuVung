using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using NAudio.Wave;

namespace HocTuVung.gTTS
{
    public class gTTS
    {
        private static Dictionary<string, string> REQUEST_HEADERS = new Dictionary<string, string>()
        {
            {"Referer", "http://translate.google.com/" },
            {"User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36" },
            {"Content-Type", "application/x-www-form-urlencoded" },
        };

        private string TRANSLE_URL = "https://translate.google.com/_/TranslateWebserverUi/data/batchexecute";

        private static int MAX_LENGTH = 100;

        private static string RPC = "jQ1olc";

        public gTTS()
        {
            
        }

        private string SendPost(string req)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(TRANSLE_URL);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(REQUEST_HEADERS["Content-Type"]));
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, TRANSLE_URL);
            request.Content = new StringContent(string.Format("f.req={0}&", Uri.EscapeDataString(req)),
                Encoding.UTF8,
                REQUEST_HEADERS["Content-Type"]);

            string decode = client.SendAsync(request).GetAwaiter().GetResult().Content.ReadAsStringAsync().GetAwaiter().GetResult();
            Regex regex = new Regex("jQ1olc\",\"\\[\\\\\"(.*)\\\\\"]");
            MatchCollection matches = regex.Matches(decode);
            if (matches.Count == 0)
            {
                throw new Exception("Tải về lỗi");
            }
            else
            {
                string base64 = matches[0].Groups[1].Value;
                byte[] binaryData = Convert.FromBase64String(base64);
                using (Mp3FileReader mp3 = new Mp3FileReader(new MemoryStream(binaryData)))
                {
                    using (WaveStream pcm = WaveFormatConversionStream.CreatePcmStream(mp3))
                    {
                        MemoryStream wavmem = new MemoryStream();
                        WaveFileWriter.WriteWavFileToStream(wavmem, pcm);
                        Console.WriteLine(Convert.ToBase64String(wavmem.ToArray()));
                        return Convert.ToBase64String(wavmem.ToArray());
                        //File.WriteAllBytes("output.wav", wavmem.ToArray());
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Trả về chuỗi base64 của audio
        /// </summary>
        /// <returns></returns>
        public string GetAudio(string text)
        {
            string data = string.Format("[[[\"jQ1olc\",\"[\\\"{0}\\\",\\\"en\\\",null,\\\"null\\\"]\",null,\"generic\"]]]", text);
            return SendPost(data);
        }
    }
}
