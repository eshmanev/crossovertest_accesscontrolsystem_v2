using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace LDAPSimulator
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public void T()
        {
            
            bool validation;
            try
            {
                var directoryEntry = new DirectoryEntry("LDAP://192.168.1.201/DC=EVRIQUM,DC=ru", "evgeny", "Nata308son");
                

                //var directorySearcher = new DirectorySearcher(directoryEntry);
                //directorySearcher.Filter = "(&(objectClass=user))";

                var searcher = new DirectorySearcher(directoryEntry);
                searcher.Filter = "(objectCategory=organizationalUnit)";
                foreach (SearchResult result in searcher.FindAll())
                {
                    var entry = result.GetDirectoryEntry();
                }

                foreach (DirectoryEntry entry in directoryEntry.Children)
                {
                    if (entry.SchemaClassName == "organizationalUnit")
                    {
                        
                    }
                }

                //foreach (SearchResult result in directorySearcher.FindAll())
                //{
                //    var entry = result.GetDirectoryEntry();
                //}
                //var connection = new LdapConnection("192.168.1.201");
                //var nc = new NetworkCredential("evgeny", "Nata308son", "EVRIQUM");
                //connection.Credential = nc;
                //connection.AuthType = AuthType.Negotiate;
                //connection.Bind(nc);
                validation = true;
            }
            catch (LdapException)
            {
                validation = false;
            }
        }

        [Test]
        public void Test()
        {
            var list = new List<OrgUnit>();
            var directoryEntry = new DirectoryEntry("LDAP://192.168.1.201/DC=EVRIQUM,DC=ru", "evgeny", "Nata308son");

            var queue = new Queue<Tuple<DirectoryEntry, OrgUnit>>();
            foreach (var child in directoryEntry.Children.Cast<DirectoryEntry>().Where(x => x.SchemaClassName == "organizationalUnit"))
                queue.Enqueue(Tuple.Create(child, (OrgUnit)null));

            while (queue.Count > 0)
            {
                var tuple = queue.Dequeue();
                var unit = new OrgUnit {Name = (string) tuple.Item1.Properties["OU"][0]};

                var targetList = tuple.Item2 == null ? list : tuple.Item2.Children;
                targetList.Add(unit);

                foreach (var child in tuple.Item1.Children.Cast<DirectoryEntry>().Where(x => x.SchemaClassName == "organizationalUnit"))
                    queue.Enqueue(Tuple.Create(child, unit));
            }
        }

        [Test]
        public void AuthenticateTest()
        {
            Authenticate("evgeny", "Nata308son", "EVRIQUM");

        }

        [Test]
        public void GetUser()
        {
            var entry = new DirectoryEntry("LDAP://192.168.1.201/DC=EVRIQUM,DC=ru", "evgeny", "Nata308son");
            var searcher = new DirectorySearcher(entry);
            searcher.Filter = "(sAMAccountName=Ivan1)";
            var result = searcher.FindOne();
            var displayName = result.Properties["displayname"].Count > 0 ? result.Properties["displayname"][0] : "Ivan";
            var phone = result.Properties["telephonenumber"].Count > 0 ? result.Properties["telephonenumber"][0] : string.Empty;
            var email = result.Properties["mail"].Count > 0 ? result.Properties["mail"][0] : string.Empty;

            // name, email, login, phone
        }

        private bool Authenticate(string userName, string password, string domain)
        {
            bool authentic = false;
            try
            {
                var entry = new DirectoryEntry("LDAP://192.168.1.201/DC=EVRIQUM,DC=ru" , userName, password);
                object nativeObject = entry.NativeObject;
                authentic = true;
            }
            catch (DirectoryServicesCOMException) { }
            return authentic;
        }

        private class OrgUnit
        {
            public OrgUnit()
            {
                Children = new List<OrgUnit>();
            }
            public string Name { get; set; }
            public List<OrgUnit> Children { get; private set; }
        }
    }
}
