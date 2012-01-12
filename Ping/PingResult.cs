using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Ping
{
    public static class CoreExts
    {
        public static void MergeBang(this HttpResponseBase res, Dictionary<string, string> hdrs)
        {
            foreach (var hdr in hdrs)
            {
                res.AddHeader(hdr.Key, hdr.Value);
            }
        }
    }


    public class PingResult : ActionResult
    {
        private readonly PingOpts _opts;
        private readonly Dictionary<string, string> NO_CACHE = new Dictionary<string, string> {
          {"Cache-Control", "no-cache, no-store, max-age=0, must-revalidate"},
          {"Pragma", "no-cache"},
          {"Expires", "Tue, 8 Sep 1981 08:42:00 UTC"}
        };

        public PingResult(PingOpts opts)
        {
            _opts = opts;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var res = context.HttpContext.Response;

            if(_opts.Check != null)
            {
                try
                {
                    if (_opts.Check()) Ok(res);
                    else Err("logic", res);
                    return;
                }
                catch(Exception ex)
                {
                    Err(ex.Message, res);
                    return;
                }
            }

            if(_opts.OkRegex != null && !string.IsNullOrWhiteSpace(_opts.CheckUrl))
            {
                try
                {
                    var req = (HttpWebRequest)WebRequest.Create(_opts.CheckUrl);
                    req.Timeout = _opts.TimeoutSecs * 1000;
                    var webResponse = (HttpWebResponse)req.GetResponse();
                    using(var streamReader = new StreamReader(webResponse.GetResponseStream()))
                    {
                        var body = streamReader.ReadToEnd();
                        if (_opts.OkRegex.IsMatch(body)) Ok(res);
                        else Err("regex", res);
                        return;
                    }
                }
                catch(Exception ex)
                {
                    Err("url: " + ex.Message, res);
                    return;
                }
            }

            Ok(res);
        }

        private void Ok(HttpResponseBase response)
        {
            StandardHeaders(response);
            response.StatusCode = _opts.OkCode;
            response.Write(_opts.OkText);
        }

        private void Err(string message, HttpResponseBase response)
        {
            StandardHeaders(response);
            response.AddHeader("x-ping-error", message);
            response.StatusCode = _opts.ErrorCode;
            response.Write(_opts.ErrorText);
        }

        private void StandardHeaders(HttpResponseBase response)
        {
            response.MergeBang(NO_CACHE);
            response.AddHeader("x-app-version", _opts.Version);
            response.AddHeader("Content-Type", _opts.Version);
        }
    }

    public class PingOpts
    {
        public Regex OkRegex { get; set; }
        public string CheckUrl { get; set; }
        public Func<bool> Check { get; set; }

        private int _timeoutSecs = 5;
        public int TimeoutSecs
        {
            get { return _timeoutSecs; }
            set { _timeoutSecs = value; }
        }

        private string _okText = "ok";
        public string OkText
        {
            get { return _okText; }
            set { _okText = value; }
        }

        private string _errorText = "error";
        public string ErrorText
        {
            get { return _errorText; }
            set { _errorText = value; }
        }

        private string _version = "0";
        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }

        private int _errorCode = 500;
        public int ErrorCode
        {
            get { return _errorCode; }
            set { _errorCode = value; }
        }

        private int _okCode = 200;
        public int OkCode
        {
            get { return _okCode; }
            set { _okCode = value; }
        }
    }
}
