using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Collections.ObjectModel;
using HesterConsultants.Properties;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace HesterConsultants.AppCode
{
    public static class SiteUtils
    {
		static HesterConsultants.Properties.Settings settings = HesterConsultants.Properties.Settings.Default;

        // constants that go here, not global.asax,
        // because they can be reused in other sites
        // site-concept constants, not db constants (those are in SiteData)
        // site-specific constants are in Global

        // session.contents keys
        public static string SESSION_SESSION_ID = "mySessionId";
        public static string SESSION_VISITOR_ID = "visitorId";
        public static string SESSION_REMOTE_ADDR = "remoteAddr";
        public static string SESSION_FIRST_URL = "firstRequestedUrl";
        public static string SESSION_SET_CLIENT_VALS_FLAG = "setClientVals";
        public static string SESSION_LOGIN_LEVEL = "loginLevel";
        public static string SESSION_LOCAL = "localSession";
		public static string SESSION_DONT_LOG = "sessionDontLog";
		public static string SESSION_DONT_SEND_ALERT = "sessionDontSendAlert";
        //public static string SESSION_DONT_LOG = "dontLogSession";
        //public static string SESSION_DONT_SEND_ALERT = "dontSendEmail";

        // query string keys
        public static string QS_BROWSER = "br";
        public static string QS_PLATFORM = "pl";
        //public static string QS_SCREEN_SIZE = "ss";
        public static string QS_SCREEN_WIDTH = "sw";
        public static string QS_SCREEN_HEIGHT = "sh";
        public static string QS_COLOR_DEPTH = "sc";
        public static string QS_TIMEZONE_OFFSET = "tzo";
        public static string QS_CALLING_SCRIPT = "caller";

        // html
        public static string CLIENT_VALS_SCRIPT_TAG = "<script type=\"text/javascript\" src=\"/scripts/hc_client_vals.js\"></script>";

        // whois
        public static string WHOIS_LOOKUP_URL = "http://whois.domaintools.com/";

		public static bool IsLocalSession(ref string remoteAddr)
		{
			bool local = false;
			//string remoteAddr = String.Empty;

			object o = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
			if (o != null)
			{
				remoteAddr = o.ToString();
				//Debug.WriteLine("!" + remoteAddr + "!");
				HttpContext.Current.Session[SESSION_REMOTE_ADDR] = remoteAddr;

				if (remoteAddr == settings.DevIspIpAddress)
					local = true;
				else if (remoteAddr.StartsWith(settings.DevLanIpAddressLeft))
					local = true;
				else if (remoteAddr == settings.DevLocalhostIpAddress)
					local = true;
				else if (remoteAddr.StartsWith(settings.DevIpAddressDebugger))
					local = true;
			}

			return local;
		}

		public static bool IgnoreReferrer(ref string ignoredReferrer)
		{
			bool ret = false;
			object r = HttpContext.Current.Request.ServerVariables["HTTP_REFERER"];
			string referrer = String.Empty;

			// debug: -------------------------------------------------------------------------------------------------------------
			//r = "http://buttons-for-website.com";
			// --------------------------------------------------------------------------------------------------------------------

			if (r != null)
			{
				string[] patterns = settings.ReferrerIgnoreRegexs.Split(new char[] { '|' });
				referrer = r.ToString().ToLower();

				foreach (string pattern in patterns)
				{
					Debug.WriteLine(pattern + ": " + Regex.Match(referrer, pattern).Success.ToString());
					if (Regex.Match(referrer, pattern).Success)
					{
						ret = true;
						ignoredReferrer = referrer; // pass back to caller
						break;
					}
				}
			}
			return ret;
		}

		public static bool IgnoreIpAddress()
		{
			// to do: regexs in settings

			string ip = String.Empty;

			object o = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
			if (o != null)
			{
				ip = o.ToString();

				if (ip.Length < 7)
					return true;

				int[] octets = { 0, 0, 0, 0 };
				string[] strOctets = ip.Split(new char[] { '.' });

				for (int k = 0; k < 4; k++)
				{
					octets[k] = Convert.ToInt32(strOctets[k]);
				}

				// google
				if (ip.StartsWith("66.249"))
					return true;

				// microsoft
				if (octets[0] == 65 && octets[1] >= 52 && octets[1] <= 55)
					return true;

				if (octets[0] == 155 && octets[1] >= 54 && octets[1] <= 60)
					return true;

				if (octets[0] == 157 && octets[1] >= 54 && octets[1] <= 60)
					return true;

				if (ip.StartsWith("199.30"))
					return true;

				if (ip.StartsWith("207.46"))
					return true;

				// baidu
				if (ip.StartsWith("220.181"))
					return true;
			}
			return false;

			//bool fourOctets = false;

			//int[] octets = { 0, 0, 0, 0 };
			//string[] strOctets = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString().Split(new char[] { '.' });

			//fourOctets = strOctets.Length > 3;

			//if (fourOctets)
			//{
			//	for (int k = 0; k < 4; k++)
			//	{
			//		octets[k] = Convert.ToInt32(strOctets[k]);
			//	}

			//	// microsoft
			//	if (octets[0] == 155 && octets[1] >= 54 && octets[1] <= 60)
			//		return true;
			//}

			//// google
			//if (ip.StartsWith("66.249"))
			//	return true;

			//return false;
		}
		
		public static bool DevServer()
        {
            // return true if running on local test server
            string[] devServers = Settings.Default.LocalServerNames.ToLower().Split(new char[] { ',' }).ToArray();
            //Debug.WriteLine(HttpContext.Current.Server.MachineName);
            return devServers.Contains<string>(HttpContext.Current.Server.MachineName.ToLower());
        }

        public static DateTime UtcToCompanyTime(DateTime utcDt)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utcDt, TimeZoneInfo.FindSystemTimeZoneById(Settings.Default.CompanyTimeZoneId));
        }

        public static object UtcToClientTime(object objUtcDt)
        {
            DateTime utcDt = DateTime.MinValue;

            if(objUtcDt != null && objUtcDt.ToString() != String.Empty)
                utcDt = DateTime.Parse(objUtcDt.ToString());
            
            if (utcDt != DateTime.MinValue)
                return UtcToCompanyTime(utcDt);
            else
                return "–";
        }

        public static string ContainerHtml(string containerTag, string htmlContents, object attributes = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<" + containerTag);
            foreach (PropertyValue prop in GetProperties(attributes))
            {
                sb.Append(" ");
                sb.Append(prop.Name);
                sb.Append("=\"");
                sb.Append(prop.Value);
                sb.Append("\"");
            }
            sb.Append(">");

            sb.Append(htmlContents);

            sb.Append("</" + containerTag + ">");

            return sb.ToString();
        }

        /// <summary>
        /// Returns HTML markup with each block of text (optionally HTML-encoded) surrounded by the specified tag.
        /// </summary>
        /// <remarks>
        /// Pass the tag without brackets (e.g., "div").
        /// </remarks>
        /// <param name="textBlocks">Text to be passed.</param>
        /// <param name="tag">HTML tag to surround each block (tag name only, without angle brackets).</param>
        /// <param name="attributes">Name/value pairs to add as attributes to each block (e.g., "new {style="color: #ffffff;", title="Your choice"}). (Default: null.)</param>
        /// <param name="encodeExistingHtml">Whether to encode existing HTML in textBlocks. (Default: true.)</param>
        /// <returns></returns>
        public static string SurroundTextBlocksWithHtmlTags(string textBlocks, string tag, object attributes = null, bool encodeExistingHtml = true)
        {
            if (String.IsNullOrEmpty(textBlocks))
                return String.Empty;

            // html encode it
            if (encodeExistingHtml)
                textBlocks = HttpContext.Current.Server.HtmlEncode(textBlocks);

            // convert text with line breaks to <p> etc.
            StringBuilder sb = new StringBuilder();

            string[] blocks = textBlocks.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string block in blocks)
            {
                // open tag
                sb.Append("<");
                sb.Append(tag);
                foreach (PropertyValue prop in GetProperties(attributes))
                {
                    sb.Append(" ");
                    sb.Append(prop.Name);
                    sb.Append("=\"");
                    sb.Append(prop.Value);
                    sb.Append("\"");
                }
                sb.Append(">");

                // text
                sb.Append(block.Trim());

                // closing tag
                sb.Append("</");
                sb.Append(tag);
                sb.Append(">");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns HTML markup with found terms highlighted, and surrounding text truncated with an indicator.
        /// </summary>
        /// <returns></returns>
        public static string HighlightTermsInTextBlocks(string textBlocks, List<string> searchTerms, string highlightTag, int numSurroundingChars, string indicator = "...", bool encodeExistingHtml = true)
        {
            if (encodeExistingHtml)
                textBlocks = HttpContext.Current.Server.HtmlEncode(textBlocks);

            string closeHighlightTag = highlightTag;
            int spaceInTag = highlightTag.IndexOf(" ");
            if (spaceInTag != -1)
                closeHighlightTag = highlightTag.Substring(0, spaceInTag);

            List<FoundTerm> foundTerms = new List<FoundTerm>();
            int endOfText = textBlocks.Length - 1;

            foreach (string term in searchTerms)
            {
                IEnumerable<int> poses = textBlocks.ToLower().IndexesOf(term.ToLower());
                foreach (int pos in poses)
                {
                    string termAsFound = textBlocks.Substring(pos, term.Length); // preserve capitalization
                    foundTerms.Add(new FoundTerm() { Term = termAsFound, Position = pos });
                }
                //int pos = textBlocks.ToLower().IndexOf(term.ToLower());
                //if (pos != -1)
                //{
                //}
            }

            // if no found terms,
            // just return original text truncated at right
            if (foundTerms.Count == 0)
                return TruncatedTextWithIndicator(textBlocks, numSurroundingChars);

            // order list, and make linked list
            foundTerms = foundTerms.OrderBy(ft => ft.Position).ToList();
            LinkedList<FoundTerm> linkedFoundTerms = new LinkedList<FoundTerm>(foundTerms);

            // list of truncated blocks
            StringBuilder sbBlocks = new StringBuilder();

            // traverse list, extracting truncated blocks
            LinkedListNode<FoundTerm> current = linkedFoundTerms.First;

            int startOfBlock = StartIndexOfWord(textBlocks, current.Value.Position - numSurroundingChars);
            int endOfBlock = 0;
            //string block = String.Empty;
            //bool stillInBlock = true;

            bool blockStartsAtTextStart = startOfBlock == 0;
            bool blockEndsAtTextEnd = false;

            List<string> foundTermsInBlock = new List<string>();
            foundTermsInBlock.Add(current.Value.Term);

            LinkedListNode<FoundTerm> nextNode = current.Next;

            while (nextNode != null)
            {
                if (nextNode.Value.Position - (current.Value.Position + current.Value.Term.Length) < numSurroundingChars)
                {
                    // the next found term stays in same block
                    if (foundTermsInBlock == null)
                        foundTermsInBlock = new List<string>();
                    foundTermsInBlock.Add(nextNode.Value.Term);

                    current = nextNode;
                    nextNode = current.Next;
                }

                else // reached numSurroundingChars, so make a truncated block
                {
                    endOfBlock = EndIndexOfWord(textBlocks, current.Value.Position + (current.Value.Term.Length - 1) + numSurroundingChars);
                    blockEndsAtTextEnd = (endOfBlock == textBlocks.Length - 1);

                    if (!blockStartsAtTextStart)
                        sbBlocks.Append(indicator);
                    sbBlocks.Append(textBlocks.Substring(startOfBlock, (endOfBlock - startOfBlock) + 1));
                    if (!blockEndsAtTextEnd)
                        sbBlocks.Append(indicator);
                    
                    // process
                    foreach (string foundWordInBlock in foundTermsInBlock.Distinct())
                        sbBlocks.Replace(foundWordInBlock, "<" + highlightTag + ">" + foundWordInBlock + "</" + closeHighlightTag + ">");

                    sbBlocks.Append(" ");

                    // reset - make new block
                    current = nextNode;
                    startOfBlock = StartIndexOfWord(textBlocks, current.Value.Position);
                    blockStartsAtTextStart = false;
                    foundTermsInBlock = new List<string>();
                    foundTermsInBlock.Add(current.Value.Term);

                    nextNode = current.Next;
                    //stillInBlock = false;
                }
            }

            // no next node
            //if (stillInBlock)
            //{
                endOfBlock = EndIndexOfWord(textBlocks, current.Value.Position + (current.Value.Term.Length - 1) + numSurroundingChars);
                blockEndsAtTextEnd = (endOfBlock == textBlocks.Length - 1);

                if (!blockStartsAtTextStart)
                    sbBlocks.Append(indicator);
                sbBlocks.Append(textBlocks.Substring(startOfBlock, (endOfBlock - startOfBlock) + 1));
                if (!blockEndsAtTextEnd)
                    sbBlocks.Append(indicator);

                //block = textBlocks.Substring(startOfBlock, (endOfBlock - startOfBlock) + 1);
                //Debug.WriteLine("block: " + block);

                foreach (string foundWordInBlock in foundTermsInBlock.Distinct())
                    //block = block.Replace(foundWordInBlock, "<" + highlightTag + ">" + foundWordInBlock + "</" + highlightTag + ">");
                    sbBlocks.Replace(foundWordInBlock, "<" + highlightTag + ">" + foundWordInBlock + "</" + closeHighlightTag + ">");

                //if (truncatedBlocks == null)
                //    truncatedBlocks = new List<string>();
                //truncatedBlocks.Add(block);
                //sbBlocks.Append(block);
            //}
            //else
            //    sbBlocks.Remove(sbBlocks.Length - 1, 1); // last space

            return sbBlocks.ToString();
        }

        /// <summary>
        /// Returns the index of the start of the word where pos is.
        /// (Used by truncating methods, so that we don't cut off words in the middle.)
        /// </summary>
        /// <param name="text"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        private static int StartIndexOfWord(string text, int pos)
        {
            bool notWhitespace = false;

            if (pos <= 0)
                return 0;

            // go left to whitespace (char before start of word)
            while (pos > 0 && (notWhitespace = !Char.IsWhiteSpace(text[--pos]))) ;

            if (notWhitespace)
                return pos;
            else // pos is on whitespace
                return pos + 1;
        }

        /// <summary>
        /// Returns the index of the end of the word where pos is.
        /// (Used by truncating methods, so that we don't cut off words in the middle.)
        /// </summary>
        /// <param name="text"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        private static int EndIndexOfWord(string text, int pos)
        {
            int end = text.Length - 1;
            bool notWhitespace = false;

            if (pos >= end)
                return end;

            while (pos < end && (notWhitespace = !Char.IsWhiteSpace(text[++pos]))) ;

            if (notWhitespace)
                return pos;
            else
                return pos - 1;
        }


        /// <summary>
        /// Used by HighlightTermsInTextBlocks
        /// </summary>
        private class FoundTerm
        {
            public string Term { get; set; }
            public int Position { get; set; }
        }

        /// <summary>
        /// Returns a truncated version of the input parameter string, with indicator of truncation (default: ellipsis) appended.
        /// </summary>
        /// <param name="text">Text to be truncated.</param>
        /// <param name="numCharsToShow">Number of characters to keep.</param>
        /// <param name="indicator">Indicator that text is truncated. (Default: ellipsis.)</param>
        /// <returns></returns>
        public static string TruncatedTextWithIndicator(string text, int numCharsToShow, string indicator = "...")
        {
            if (text.Length > numCharsToShow)
                return text.Substring(0, numCharsToShow) + "...";
            else
                return text;
        }

        public static string ExceptionMessageForCustomer(string specificErrorFirstSentence)
        {
            return specificErrorFirstSentence + " Please notify technical support at " + Settings.Default.CustomerServiceEmail + " or " + Settings.Default.CompanyPhoneTechSupport + ".";
        }

        //public static string PrivacyPolicy() // have this in a resource

        public static string CheckForClientValsScript(System.Web.UI.Page page)
        {
			// 2014 - no
			//return String.Empty;
			// ----------------------------

            if (page.Session[SESSION_SET_CLIENT_VALS_FLAG] == null
                    ||
                    Convert.ToBoolean(page.Session[SESSION_SET_CLIENT_VALS_FLAG]) == false)
                return CLIENT_VALS_SCRIPT_TAG;
            else
                return string.Empty;
        }

        public static void SetWroteClientValsFlag(System.Web.UI.Page page)
        {
            page.Session[SESSION_SET_CLIENT_VALS_FLAG] = true;
        }

        public static string AppendNumberToString(string number, string str)
        {
            return str + number;
        }

        public static void AddTextBoxAttributes(Control ctrl, string attr, string val, bool recurse)
        {
            if (ctrl is TextBox)
            {
                System.Web.UI.AttributeCollection attrs = ((TextBox)ctrl).Attributes;
                IEnumerable<string> keys = attrs.Keys.Cast<string>();

                if (!keys.Contains(attr))
                    ((TextBox)ctrl).Attributes.Add(attr, val);
            }

            // recurse
            if (recurse)
                foreach (Control c in ctrl.Controls)
                    AddTextBoxAttributes(c, attr, val, true);
        }

        public static string WindowsSafeFilename(string origFilename)
        {
            string newStr = origFilename;
            List<char> illegalChars = Path.GetInvalidFileNameChars().ToList();
            if (!illegalChars.Contains('#'))
                illegalChars.Add('#');

            // illegal chars
            foreach (char badChar in illegalChars)
                newStr = newStr.Replace(badChar, '_');

            // spaces
            newStr = newStr.Replace(' ', '_');

            // ends with dot
            if (newStr.EndsWith("."))
                newStr = newStr.Substring(0, newStr.Length - 1);

            return newStr;
        }

        /// <summary>
        /// Changes a filename if another file with the same filename exists in folder by appending numeral characters, e.g., "file001", "file002" etc. numDigits is number of digits to be appended (base 10).
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="filename"></param>
        /// <param name="numDigits"></param>
        /// <returns></returns>
        public static string ConflictFreeFilename(string folderPath, string filename, int numDigits)
        {
            // 
            string newFilename = filename;
            string newFileFullPath = folderPath + @"\" + newFilename;
            string filenameWithoutExtension = filename;
            string dotExtension = String.Empty;

            // separate extension
            int pos = filename.LastIndexOf(".");
            if (pos != -1)
            {
                filenameWithoutExtension = filename.Substring(0, pos);
                dotExtension = filename.Substring(pos, filename.Length - pos); // includes the dot
            }

            int k = 0;
            int upperLimit = (int)(Math.Pow(10d, (double)numDigits));

            //Debug.WriteLine(newFileFullPath);
            while (File.Exists(newFileFullPath)
                && 
                ++k < upperLimit)
            {
                int numZeroes = numDigits - ((int)(Math.Log((double)k, 10.0)) + 1);
                newFilename = filenameWithoutExtension + "-" + new String('0', numZeroes) + k.ToString() + dotExtension;
                newFileFullPath = folderPath + @"\" + newFilename;
            }

            if (k < upperLimit)
                return newFilename;
            else
                throw new Exception("Too many files in the folder.");
        }

        //public class TimeZoneExtender
        //{
        //    /// <summary>
        //    /// Provides an integer field to arbitrarily reorder time zones.
        //    /// </summary>
        //    public TimeZoneInfo TimeZoneInfo { get; set; }
        //    public int Orderer { get; set; }
        //    public string Id { get { return this.TimeZoneInfo.Id; } }
        //    public string DisplayName { get { return this.TimeZoneInfo.DisplayName; } }
        //}

        public static ReadOnlyCollection<TimeZoneInfo> AllSystemTimeZones()
        {
            return TimeZoneInfo.GetSystemTimeZones();
            
            //List<TimeZoneExtender> listZones = new List<TimeZoneExtender>();

            //TimeZoneInfo maxOffset = TimeZoneInfo.Utc;
            //TimeZoneInfo minOffset = TimeZoneInfo.Utc;
            //foreach (TimeZoneInfo tzi in systemZones)
            //{
            //    //Debug.WriteLine(tzi.Id + ": " + tzi.BaseUtcOffset.TotalMinutes.ToString());
            //    //if (tzi.BaseUtcOffset.TotalMinutes < minOffset.BaseUtcOffset.TotalMinutes)
            //    //    minOffset = tzi;
            //    //if (tzi.BaseUtcOffset.TotalMinutes > maxOffset.BaseUtcOffset.TotalMinutes)
            //    //    maxOffset = tzi;
            //    TimeZoneExtender tze = new TimeZoneExtender();
            //    tze.TimeZoneInfo = tzi;

            //    if ((int)tzi.BaseUtcOffset.TotalMinutes == 0)
            //        tze.Orderer = 0;
            //    else if (tzi.BaseUtcOffset.TotalMinutes > 0)
            //        // going east from utc
            //        // want to go west, and put these after the negative offsets
            //        tze.Orderer = -10000 + (int)tzi.BaseUtcOffset.TotalMinutes;
            //    else
            //        // going west from utc
            //        tze.Orderer = (int)tzi.BaseUtcOffset.TotalMinutes;

            //    listZones.Add(tze);
            //}

            //Debug.WriteLine("max: " + maxOffset.DisplayName + " - " + maxOffset.BaseUtcOffset.TotalMinutes.ToString());
            //Debug.WriteLine("min: " + minOffset.DisplayName + " - " + minOffset.BaseUtcOffset.TotalMinutes.ToString());
            //listZones.Reverse();

            //listZones = listZones.OrderByDescending(tze => tze.Orderer).ToList();
           
            //return listZones;
        }

        public static string NoSpaces(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return String.Empty;
            else
                return s.Replace(" ", "");
        }

        /// <summary>
        /// Encodes a string to be represented as a string literal. The format
        /// is essentially a JSON string.
        /// 
        /// The string returned includes outer quotes 
        /// Example Output: "Hello \"Rick\"!\r\nRock on"
        /// Note: This is from Rick Strahl (www.west-wind.com)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string EncodeJsString(string s)
        {
            StringBuilder sb = new StringBuilder();

            //sb.Append("\""); // kah commented out - will handle containing quotes myself
            
            foreach (char c in s)
            {
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        int i = (int)c;
                        if (i < 32 || i > 127)
                        {
                            sb.AppendFormat("\\u{0:X04}", i);
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }

            //sb.Append("\""); // kah commented out 

            return sb.ToString();
        }

        /// <summary>
        /// Extension method for string. Returns IEnumerable<int> of all indexes of a search term.
        /// </summary>
        /// <param name="text">The text to be searched.</param>
        /// <param name="searchTerm">The term searched for.</param>
        /// <returns></returns>
        public static IEnumerable<int> IndexesOf(this string text, string searchTerm)
        {
            int lastIndex = 0;
            while (true)
            {
                int index = text.IndexOf(searchTerm, lastIndex);
                if (index == -1)
                {
                    yield break;
                }
                yield return index;
                lastIndex = index + searchTerm.Length;
            }
        }

        private static IEnumerable<PropertyValue> GetProperties(object obj)
        {
            if (obj != null)
            {
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(obj);
                foreach (PropertyDescriptor prop in props)
                {
                    object val = prop.GetValue(obj);
                    if (val != null)
                    {
                        yield return new PropertyValue { Name = prop.Name, Value = val };
                    }
                }
            }
        }

        private sealed class PropertyValue
        {
            public string Name { get; set; }
            public object Value { get; set; }
        }

        /// <summary>
        /// Error logging for Visitors db, not clients.
        /// </summary>
        /// <param name="errMsg"></param>
        /// <param name="whenOccurred"></param>
        //public static void LogError(string errMsg, DateTime whenOccurred)
        //{
        //    SiteData.InsertError(errMsg, whenOccurred);
        //}
	}
}
