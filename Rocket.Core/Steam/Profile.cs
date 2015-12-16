using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Xml;

namespace Rocket.Core.Steam
{
    public class Profile
    {
        public ulong SteamID64 { get; set; }
        public string SteamID { get; set; }
        public string OnlineState { get; set; }
        public string StateMessage { get; set; }
        public string PrivacyState { get; set; }
        public ushort VisibilityState { get; set; }
        public Uri AvatarIcon { get; set; }
        public Uri AvatarMedium { get; set; }
        public Uri AvatarFull { get; set; }
        public bool IsVacBanned { get; set; }
        public string TradeBanState { get; set; }
        public bool IsLimitedAccount { get; set; }
        public string CustomURL { get; set; }
        public DateTime MemberSince { get; set; }
        public double HoursPlayedLastTwoWeeks { get; set; }
        public string Headline { get; set; }
        public string Location { get; set; }
        public string RealName { get; set; }
        public string Summary { get; set; }
        public List<MostPlayedGame> MostPlayedGames { get; set; }
        public List<Group> Groups { get; set; }

        public class MostPlayedGame
        {
            public string Name { get; set; }
            public Uri Link { get; set; }
            public Uri Icon { get; set; }
            public Uri Logo { get; set; }
            public Uri LogoSmall { get; set; }
            public double HoursPlayed { get; set; }
            public double HoursOnRecord { get; set; }
        }

        public class Group
        {
            public ulong SteamID64 { get; set; }
            public bool IsPrimary { get; set; }
            public string Name { get; set; }
            public string URL { get; set; }
            public Uri AvatarIcon { get; set; }
            public Uri AvatarMedium { get; set; }
            public Uri AvatarFull { get; set; }
            public string Headline { get; set; }
            public string Summary { get; set; }
            public uint? MemberCount { get; set; }
            public uint? MembersInGame { get; set; }
            public uint? MembersInChat { get; set; }
            public uint? MembersOnline { get; set; }
        }

        public Profile(ulong steamID64)
        {
            SteamID64 = steamID64;
            Reload();
        }

        public void Reload()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(new WebClient().DownloadString("http://steamcommunity.com/profiles/" + SteamID64 + "?xml=1"));

            SteamID = doc["profile"]["steamID"].InnerText;
            OnlineState = doc["profile"]["onlineState"].InnerText;
            StateMessage = doc["profile"]["stateMessage"].InnerText;
            PrivacyState = doc["profile"]["privacyState"].InnerText;
            VisibilityState = ushort.Parse(doc["profile"]["visibilityState"].InnerText);
            AvatarIcon = new Uri(doc["profile"]["avatarIcon"].InnerText);
            AvatarMedium = new Uri(doc["profile"]["avatarMedium"].InnerText);
            AvatarFull = new Uri(doc["profile"]["avatarFull"].InnerText);
            IsVacBanned = doc["profile"]["vacBanned"].InnerText == "1";
            TradeBanState = doc["profile"]["tradeBanState"].InnerText;
            IsLimitedAccount = doc["profile"]["isLimitedAccount"].InnerText == "1";
            CustomURL = doc["profile"]["customURL"].InnerText;
            MemberSince = DateTime.Parse(doc["profile"]["memberSince"].InnerText.Replace("st", "").Replace("nd", "").Replace("rd", "").Replace("th", ""), new CultureInfo("en-US", false)); ;
            HoursPlayedLastTwoWeeks = double.Parse(doc["profile"]["hoursPlayed2Wk"].InnerText);
            Headline = doc["profile"]["headline"].InnerText;
            Location = doc["profile"]["location"].InnerText;
            RealName = doc["profile"]["realname"].InnerText;
            Summary = doc["profile"]["summary"].InnerText;

            MostPlayedGames = new List<MostPlayedGame>();
            foreach (XmlElement mostPlayedGame in doc["profile"]["mostPlayedGames"].ChildNodes)
            {
                MostPlayedGame newMostPlayedGame = new MostPlayedGame();
                newMostPlayedGame.Name = mostPlayedGame["gameName"].InnerText;
                newMostPlayedGame.Link = new Uri(mostPlayedGame["gameLink"].InnerText);
                newMostPlayedGame.Icon = new Uri(mostPlayedGame["gameIcon"].InnerText);
                newMostPlayedGame.Logo = new Uri(mostPlayedGame["gameLogo"].InnerText);
                newMostPlayedGame.LogoSmall = new Uri(mostPlayedGame["gameLogoSmall"].InnerText);
                newMostPlayedGame.HoursPlayed = double.Parse(mostPlayedGame["hoursPlayed"].InnerText);
                newMostPlayedGame.HoursOnRecord = double.Parse(mostPlayedGame["hoursOnRecord"].InnerText);
                MostPlayedGames.Add(newMostPlayedGame);
            }

            Groups = new List<Group>();
            foreach (XmlElement group in doc["profile"]["groups"].ChildNodes)
            {
                Group newGroup = new Group();
                newGroup.IsPrimary = group.Attributes["isPrimary"].InnerText == "1";
                newGroup.SteamID64 = ulong.Parse(group["groupID64"].InnerText);
                newGroup.Name = group["groupName"]?.InnerText;
                newGroup.URL = group["groupURL"]?.InnerText;
                newGroup.Headline = group["headline"]?.InnerText;
                newGroup.Summary = group["summary"]?.InnerText;
                newGroup.AvatarIcon = group["avatarIcon"] != null ? new Uri(group["avatarIcon"].InnerText) : null;
                newGroup.AvatarMedium = group["avatarMedium"] != null ? new Uri(group["avatarMedium"].InnerText) : null;
                newGroup.AvatarFull = group["avatarFull"] != null ? new Uri(group["avatarFull"].InnerText) : null;
                newGroup.MemberCount = group["memberCount"] != null ? (uint?)uint.Parse(group["memberCount"].InnerText) : null;
                newGroup.MembersInChat = group["membersInChat"] != null ? (uint?)uint.Parse(group["membersInChat"].InnerText) : null;
                newGroup.MembersInGame = group["membersInGame"] != null ? (uint?)uint.Parse(group["membersInGame"].InnerText) : null;
                newGroup.MembersOnline = group["membersOnline"] != null ? (uint?)uint.Parse(group["membersOnline"].InnerText) : null;
                Groups.Add(newGroup);
            }
        }
    }
}
