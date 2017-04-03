using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace rori.Network.Upload
{
    public class UploadService
    {
        public UploadService(CustomUploadService customUploadService)
        {
            _customUploadService = customUploadService;
        }

        private CustomUploadService _customUploadService;
        private CustomUploadService CustomUploadService { get { return _customUploadService; } }

        public delegate void UploadServiceProgressChangedEventHandler(object sender, UploadServiceProgressChangedEventArgs e);

        public event UploadServiceProgressChangedEventHandler UploadServiceProgressChanged;

        protected virtual void OnUploadServiceProgressChanged(UploadServiceProgressChangedEventArgs e)
        {
            var handler = UploadServiceProgressChanged;
            if (handler != null) handler(this, e);
        }

        public async Task<UploadServiceInfo> Upload(byte[] file, string fileName)
        {
            using (var client = new HttpClient())
            {
                using (var stream = new ProgressStream(file, UploadServiceProgressChanged))
                {
                    using (var content = new MultipartFormDataContent())
                    {
                        content.Add(new StreamContent(stream), CustomUploadService.FieldName, fileName);

                        if (CustomUploadService.Arguments != null)
                        {
                            for (int i = 0; i < CustomUploadService.Arguments.Count; i++)
                            {
                                content.Add(new StringContent(CustomUploadService.Arguments[i].Value), CustomUploadService.Arguments[i].Key);
                            }
                        }

                        if (CustomUploadService.Headers != null)
                        {
                            for (int i = 0; i < CustomUploadService.Headers.Count; i++)
                            {
                                content.Headers.Add(CustomUploadService.Headers[i].Key, CustomUploadService.Arguments[i].Value);
                            }
                        }

                        using (var response = await client.PostAsync(CustomUploadService.RequestUri, content))
                        {
                            var result = string.Empty;

                            if (response.IsSuccessStatusCode) result = await response.Content.ReadAsStringAsync();

                            return !string.IsNullOrEmpty(result)
                                ? new UploadServiceInfo(new Uri(Regex.Match(result, CustomUploadService.Pattern).Value), true)
                                : new UploadServiceInfo(null, false);
                        }
                    }
                }
            }
        }
    }
}