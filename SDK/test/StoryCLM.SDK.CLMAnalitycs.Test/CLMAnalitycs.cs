using Newtonsoft.Json;
using Shared;
using StoryCLM.SDK.CLMAnalitycs;
using StoryCLM.SDK.CLMAnalitycs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace StoryCLM.SDK.CLMAnalitycs.Test
{
    public class CLMAnalitycs
    {
        const int PRESENTATION_ID = 4991;
        const string USER_ID = "3ab4a1db-b434-4b00-bbd8-253863042bac";

        StoryCustomEvent GetCustomEvent(string sesstion) => new StoryCustomEvent
        {
            Key = "Name",
            Value = "Valera",
            PresentationId = PRESENTATION_ID,
            LocalTicks = DateTime.Now.Ticks,
            SessionId = sesstion,
            TimeZone = 3,
            UserId = USER_ID,
            Id = Guid.NewGuid().ToString()
        };

        StorySessionEvent GetSession() => new StorySessionEvent
        {
            Address = "",
            Complete = true,
            Duration = 365,
            SlidesCount = 1,
            Latitude = 45.0454764,
            Longtitude = 41.9683431,
            PresentationId = PRESENTATION_ID,
            LocalTicks = DateTime.Now.Ticks,
            TimeZone = 3,
            UserId = USER_ID,
            SessionId = Guid.NewGuid().ToString()
        };

        StorySlideEvent GetSlide(string sesstion) => new StorySlideEvent
        {
            PresentationId = PRESENTATION_ID,
            LocalTicks = DateTime.Now.Ticks,
            TimeZone = 3,
            UserId = USER_ID,
            SessionId = sesstion,
            Duration = 45,
            Navigation = "In",
            SlideId = 257756,
            SlideName = "index.html"
        };


        [Theory]
        [InlineData(0)]
        public async Task SetSessions(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var session = GetSession();
            var s = JsonConvert.SerializeObject(session);
            await sclm.SendSessionEvent(session);
        }

        [Theory]
        [InlineData(0)]
        public async Task SetSlide(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var session = GetSession();
            var slide = GetSlide(session.SessionId);
            await sclm.SendSlideEvent(slide);
        }

        [Theory]
        [InlineData(0)]
        public async Task SetCustomEvent(int uc)
        {
            SCLM sclm = await Utilities.GetContextAsync(uc);
            var session = GetSession();
            var customEvent = GetCustomEvent(session.SessionId);
            await sclm.SendCustomEvent(customEvent);
        }


        public class CS
        {
            public string Speciality { get; set; }
            public string Therapy_before_call { get; set; }
            public string How_many_patient_do_you_have { get; set; }
            public string the_high_risk_of_the_break_of_the_neck_of_the_thigh_therapy_before_call { get; set; }
            public string What_is_the_most_important_for_therapy { get; set; }
            public string The_additional_information { get; set; }
            public string the_high_risk_of_the_break_of_the_neck_of_the_thigh_Therapy_after_call { get; set; }
        }

        [Theory]
        [InlineData("9F874FB2-42E3-4448-857F-42CBC0ED55BD")]
        public async Task GetVisit(string sessionId)
        {
            SCLM sclm = await Utilities.GetContextAsync(0);
            var visit = await sclm.GetVisit<CS>(sessionId);
        }

        [Theory]
        [InlineData("9F874FB2-42E3-4448-857F-42CBC0ED55BD")]
        public async Task GetForm(string sessionId)
        {
            SCLM sclm = await Utilities.GetContextAsync(0);
            var formDynamic = await sclm.GetForm<dynamic>(sessionId);
            var formDictionary = await sclm.GetForm<IDictionary<string, string>>(sessionId);
        }

        [Theory]
        [InlineData(4774)]
        public async Task GetCustomEvents(int presentationId)
        {
            SCLM sclm = await Utilities.GetContextAsync(0);
            List<StoryCustomEvent> result = new List<StoryCustomEvent>();
            var feed = sclm.GetCustomEventsFeed(presentationId);
            foreach (var t in feed)
            {
                result.AddRange(t.Result);
            }
        }

        [Theory]
        [InlineData(5745)]
        public async Task GetSessions(int presentationId)
        {
            SCLM sclm = await Utilities.GetContextAsync(0);
            var feed = sclm.GetSessionFeed(presentationId);
            List<IFeedable> result = new List<IFeedable>();
            foreach (var t in feed)
            {
                result.AddRange(t.Result);
            }

        }


        [Theory]
        [InlineData(5745, "26717d2f-32f2-4348-8294-4d568d74a851")]
        public async Task GetSessionsByUser(int presentationId, string userId)
        {
            SCLM sclm = await Utilities.GetContextAsync(0);
            var feed = sclm.GetSessionFeed(presentationId, userId, new YearSection().Year(2019).Month(2).Day(15));
            List<IFeedable> result = new List<IFeedable>();
            foreach (var t in feed)
            {
                result.AddRange(t.Result);
            }
        }

    }
}
