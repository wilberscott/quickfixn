﻿using NUnit.Framework;
using QuickFix;

namespace UnitTests
{
    [TestFixture]
    public class SessionSettingsTest
    {
        protected System.Text.StringBuilder partialConfiguration;

        [SetUp]
        public void Init()
        {
            partialConfiguration = new System.Text.StringBuilder()
                .AppendLine("[SESSION]")
                .AppendLine("BeginString=FIX.4.2")
                .AppendLine("SenderCompID=ISLD")
                .AppendLine("TargetCompID=TW")
                .AppendLine("Value=1")
                .AppendLine("# this is a comment")
                .AppendLine("[SESSION]")
                .AppendLine("BeginString=FIX.4.1")
                .AppendLine("SenderCompID=ISLD")
                .AppendLine("TargetCompID=WT")
                .AppendLine("Value=2")
                .AppendLine("[SESSION]")
                .AppendLine("SenderCompID=ARCA")
                .AppendLine("TargetCompID=TW")
                .AppendLine("Value=3")
                .AppendLine("[SESSION]")
                .AppendLine("SenderCompID=ARCA")
                .AppendLine("TargetCompID=WT")
                .AppendLine("[SESSION]")
                .AppendLine("SenderCompID=NYSE")
                .AppendLine("TargetCompID=TW")
                .AppendLine("SessionQualifier=QUAL1")
                .AppendLine("Value=5")
                .AppendLine("[SESSION]")
                .AppendLine("SenderCompID=NYSE")
                .AppendLine("TargetCompID=TW")
                .AppendLine("SessionQualifier=QUAL2")
                .AppendLine("Value=6")
                .AppendLine("[SESSION]")
                .AppendLine("BeginString=FIXT.1.1")
                .AppendLine("SenderCompID=NYSE")
                .AppendLine("TargetCompID=TW");
        }

        [Test]
        public void Load()
        {
            string configuration = new System.Text.StringBuilder()
                .AppendLine("[DEFAULT]")
                .AppendLine("ConnectionType=initiator")
                .AppendLine("BeginString=FIX.4.0")
                .AppendLine("Value=4")
                .AppendLine("Empty=")
                .AppendLine(partialConfiguration.ToString())
                .ToString();
            SessionSettings settings = new SessionSettings(new System.IO.StringReader(configuration));
            
            SessionID session1 = new SessionID("FIX.4.2", "ISLD", "TW");
            SessionID session2 = new SessionID("FIX.4.1", "ISLD", "WT");
            SessionID session3 = new SessionID("FIX.4.0", "ARCA", "TW");
            SessionID session4 = new SessionID("FIX.4.0", "ARCA", "WT");
            SessionID session5 = new SessionID("FIX.4.0", "NYSE", "TW", "QUAL1");
            SessionID session6 = new SessionID("FIX.4.0", "NYSE", "TW", "QUAL2");
            
            Assert.That(settings.Get().GetString( "Empty" ), Is.EqualTo("") );
            
            Assert.That(settings.Get().GetLong( "Value" ), Is.EqualTo(4) );
            Assert.That(settings.Get(session1).GetLong("Value"), Is.EqualTo(1));
            Assert.That(settings.Get(session2).GetLong("Value"), Is.EqualTo(2));
            Assert.That(settings.Get(session3).GetLong("Value"), Is.EqualTo(3));
            Assert.That(settings.Get(session4).GetLong("Value"), Is.EqualTo(4));
            Assert.That(settings.Get(session5).GetLong("Value"), Is.EqualTo(5));
            Assert.That(settings.Get(session6).GetLong("Value"), Is.EqualTo(6));

            // case insensitivity
            Assert.That(settings.Get().GetLong("value"), Is.EqualTo(4));
            Assert.That(settings.Get(session1).GetLong("value"), Is.EqualTo(1));
            Assert.That(settings.Get(session2).GetLong("value"), Is.EqualTo(2));
            Assert.That(settings.Get(session3).GetLong("value"), Is.EqualTo(3));
            Assert.That(settings.Get(session4).GetLong("value"), Is.EqualTo(4));
            Assert.That(settings.Get(session5).GetLong("value"), Is.EqualTo(5));
            Assert.That(settings.Get(session6).GetLong("value"), Is.EqualTo(6));

            Assert.That(settings.Get().GetLong("VALUE"), Is.EqualTo(4));
            Assert.That(settings.Get(session1).GetLong("VALUE"), Is.EqualTo(1));
            Assert.That(settings.Get(session2).GetLong("VALUE"), Is.EqualTo(2));
            Assert.That(settings.Get(session3).GetLong("VALUE"), Is.EqualTo(3));
            Assert.That(settings.Get(session4).GetLong("VALUE"), Is.EqualTo(4));
            Assert.That(settings.Get(session5).GetLong("VALUE"), Is.EqualTo(5));
            Assert.That(settings.Get(session6).GetLong("VALUE"), Is.EqualTo(6));
        }

        [Test]
        public void LoadSettingsWithDefaultSectionLast()
        {
            string configuration = new System.Text.StringBuilder()
                .AppendLine(partialConfiguration.ToString())
                .AppendLine("[DEFAULT]")
                .AppendLine("ConnectionType=initiator")
                .AppendLine("BeginString=FIX.4.0")
                .AppendLine("Value=4")
                .AppendLine("Empty=")
                .ToString();
            SessionSettings settings = new SessionSettings(new System.IO.StringReader(configuration));

            SessionID session1 = new SessionID("FIX.4.2", "ISLD", "TW");
            SessionID session2 = new SessionID("FIX.4.1", "ISLD", "WT");
            SessionID session3 = new SessionID("FIX.4.0", "ARCA", "TW");
            SessionID session4 = new SessionID("FIX.4.0", "ARCA", "WT");
            SessionID session5 = new SessionID("FIX.4.0", "NYSE", "TW", "QUAL1");
            SessionID session6 = new SessionID("FIX.4.0", "NYSE", "TW", "QUAL2");

            Assert.That(settings.Get().GetString("Empty"), Is.EqualTo(""));

            Assert.That(settings.Get().GetLong("Value"), Is.EqualTo(4));
            Assert.That(settings.Get(session1).GetLong("Value"), Is.EqualTo(1));
            Assert.That(settings.Get(session2).GetLong("Value"), Is.EqualTo(2));
            Assert.That(settings.Get(session3).GetLong("Value"), Is.EqualTo(3));
            Assert.That(settings.Get(session4).GetLong("Value"), Is.EqualTo(4));
            Assert.That(settings.Get(session5).GetLong("Value"), Is.EqualTo(5));
            Assert.That(settings.Get(session6).GetLong("Value"), Is.EqualTo(6));

            // case insensitivity
            Assert.That(settings.Get().GetLong("value"), Is.EqualTo(4));
            Assert.That(settings.Get(session1).GetLong("value"), Is.EqualTo(1));
            Assert.That(settings.Get(session2).GetLong("value"), Is.EqualTo(2));
            Assert.That(settings.Get(session3).GetLong("value"), Is.EqualTo(3));
            Assert.That(settings.Get(session4).GetLong("value"), Is.EqualTo(4));
            Assert.That(settings.Get(session5).GetLong("value"), Is.EqualTo(5));
            Assert.That(settings.Get(session6).GetLong("value"), Is.EqualTo(6));

            Assert.That(settings.Get().GetLong("VALUE"), Is.EqualTo(4));
            Assert.That(settings.Get(session1).GetLong("VALUE"), Is.EqualTo(1));
            Assert.That(settings.Get(session2).GetLong("VALUE"), Is.EqualTo(2));
            Assert.That(settings.Get(session3).GetLong("VALUE"), Is.EqualTo(3));
            Assert.That(settings.Get(session4).GetLong("VALUE"), Is.EqualTo(4));
            Assert.That(settings.Get(session5).GetLong("VALUE"), Is.EqualTo(5));
            Assert.That(settings.Get(session6).GetLong("VALUE"), Is.EqualTo(6));
        }

        [Test]
        public void DuplicateSession()
        {
            string configuration = new System.Text.StringBuilder()
                    .AppendLine("[DEFAULT]")
                    .AppendLine("ConnectionType=initiator")
                    .AppendLine("[SESSION]")
                    .AppendLine("BeginString=FIX.4.2")
                    .AppendLine("SenderCompID=ISLD")
                    .AppendLine("TargetCompID=TW")
                    .AppendLine("[SESSION]")
                    .AppendLine("BeginString=FIX.4.2")
                    .AppendLine("SenderCompID=ISLD")
                    .AppendLine("TargetCompID=TW")
                    .ToString();
            Assert.Throws<ConfigError>(delegate { new SessionSettings(new System.IO.StringReader(configuration)); });
        }
        
        [Test]
        public void StripSpaces()
        {
            string configuration = new System.Text.StringBuilder()
                    .AppendLine("[ DEFAULT ]")
                    .AppendLine(" ConnectionType = initiator")
                    .AppendLine("  [  SESSION  ]  ")
                    .AppendLine("BeginString=FIX.4.2 ")
                    .AppendLine(" SenderCompID =ISLD")
                    .AppendLine("  TargetCompID  =  TW  ")
                    .AppendLine("  Long  =  123  ")
                    .AppendLine("  Double  =  1.23  ")
                    .AppendLine("  Bool  =  N  ")
                    .ToString();
            SessionSettings settings = new SessionSettings(new System.IO.StringReader(configuration));
            
            Assert.That(settings.Get().GetString("ConnectionType"), Is.EqualTo("initiator"));

            SessionID session = new SessionID("FIX.4.2", "ISLD", "TW");
            Assert.That(settings.Get(session).GetString("ConnectionType"), Is.EqualTo("initiator"));
            Assert.That(settings.Get(session).GetString("BeginString"), Is.EqualTo("FIX.4.2"));
            Assert.That(settings.Get(session).GetString("SenderCompID"), Is.EqualTo("ISLD"));
            Assert.That(settings.Get(session).GetString("TargetCompID"), Is.EqualTo("TW"));
            Assert.That(settings.Get(session).GetLong("Long"), Is.EqualTo(123));
            Assert.That(settings.Get(session).GetDouble("Double"), Is.EqualTo(1.23));
            Assert.That(settings.Get(session).GetBool("Bool"), Is.False);
        }

        [Test]
        public void Validate()
        {
            SessionSettings settings = new SessionSettings();
            SessionID sessionID = new SessionID("FIX.4.2", "SenderCompID", "TargetCompID");
            
            // ConnectionType not set
            QuickFix.Dictionary dictionary = new QuickFix.Dictionary();
            Assert.Throws<ConfigError>(delegate { settings.Set(sessionID, dictionary); });

            // ConnectionType set to invalid value
            dictionary.SetString(SessionSettings.CONNECTION_TYPE, "badvalue");
            Assert.Throws<ConfigError>(delegate { settings.Set(sessionID, dictionary); });

            // ConnectionType set to valid value
            dictionary.SetString(SessionSettings.CONNECTION_TYPE, "initiator");
            Assert.DoesNotThrow(delegate { settings.Set(sessionID, dictionary); });
            
            // Invalid BeginString
            sessionID = new SessionID("FIX4.2", "SenderCompID", "TargetCompID");
            Assert.Throws<ConfigError>(delegate { settings.Set(sessionID, dictionary); });
        }

        [Test]
        public void SettingsToString()
        {
            string configuration = new System.Text.StringBuilder()
                .AppendLine("[DEFAULT]")
                .AppendLine("CONNECTIONTYPE=initiator")
                .AppendLine("BEGINSTRING=FIX.4.0")
                .AppendLine()
                .AppendLine("[SESSION]")
                .AppendLine("BEGINSTRING=FIX.4.2")
                .AppendLine("SENDERCOMPID=ISLD")
                .AppendLine("TARGETCOMPID=TW")
                .AppendLine("VALUE=1")
                .AppendLine()
                .AppendLine("[SESSION]")
                .AppendLine("BEGINSTRING=FIX.4.1")
                .AppendLine("SENDERCOMPID=ISLD")
                .AppendLine("TARGETCOMPID=WT")
                .AppendLine("VALUE=2")
                .ToString();
            SessionSettings settings = new SessionSettings(new System.IO.StringReader(configuration));

            Assert.That(settings.ToString(), Is.EqualTo(configuration));
        }
    }
}
